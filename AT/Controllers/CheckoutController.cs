using AT.Data.Courses;
using AT.Data.Identity;
using AT.Models.CheckoutViewModels;
using AT.Services.Messaging;
using AT.Services.Stripe;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AT.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IATRepository _repository;
        private readonly IHostingEnvironment _environment;
        private readonly ICart _cart;
        private readonly IEmailSender _emailSender;
        private readonly IStripePaymentService _stripeService;

        public CheckoutController(IATRepository rep,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ICart cart,
            IStripePaymentService stripeService,
            IHostingEnvironment environment)
        {
            _repository = rep;
            _environment = environment;
            _cart = cart;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _stripeService = stripeService;
        }


        public IActionResult CheckoutCart()
        {
            CheckoutVM model = new CheckoutVM();
            model.Cart = _cart;
            return View(model);
        }
        /// <summary>
        /// This takes us to the form where we collect the user details if they are
        /// not logged in otherwise we collect the same details here we do in the
        /// Registration form and create the account
        /// 
        /// If user is logged in then we can go straight to the payment screen
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckoutUserInfo(CheckoutVM model)
        {
            
            //if user is logged in already then jump to the payment screen
            ApplicationUser user = await CurrentUser();
            if (user != null)
                return RedirectToAction("CheckoutPayment",model);


            //pass the model into the view so we can collect the uer details
            return View(model);
        }

        private async Task<ApplicationUser> CurrentUser()
        {

            return await _userManager.GetUserAsync(User);


        }

        private async Task<CheckoutUserResult> CreateUser(CheckoutVM model)
        {

            CheckoutUserResult userresult = new CheckoutUserResult();
            var user = new ApplicationUser
            {
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Company = model.Company,
                Division = model.Division,
                JobTitle = model.JobTitle,
                Mobile = model.Mobile,
                Email = model.Email
            };


            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=532713
                // Send an email with this link
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Action(nameof(AccountController.ConfirmEmail),
                                            "Account", 
                                            new { userId = user.Id, code = code },
                                            protocol: HttpContext.Request.Scheme);

                await _emailSender.SendEmailAsync(model.Email,
                                                    "Confirm your account",
                                                    $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");

                await _signInManager.SignInAsync(user, isPersistent: false);

                userresult.User = user;
            }
            else
            {
                IdentityError error = result.Errors.FirstOrDefault();
                if(error.Code == "DuplicateUserName")
                {
                    userresult.ErrorMessage = error.Description + "  Please Log in to continue.";
                }
                else
                {
                    userresult.ErrorMessage = "User could not be created" + error.Description;
                }
            
                userresult.HasErrors = true;
            }

            return userresult;
        }

       
      
        [AcceptVerbs("Get","Post")]
        public async Task<IActionResult> CheckoutPayment(CheckoutVM model)
        {

            //register the user and sign them in if they are not already signed in
            ApplicationUser user = await CurrentUser();
            if (user == null)
            {
                var userresult = await CreateUser(model);
                if(userresult.HasErrors)
                {
                    CheckoutErrorVM errorVM = new CheckoutErrorVM()
                    {
                        ErrorMessage = userresult.ErrorMessage
                    };
                     return RedirectToAction("CheckoutError", errorVM);
                }
                else
                {
                    //we now have a user
                    user = userresult.User;
                }

            }

           

            //hook up with the stripe service and get the payment intent
            //add this to the model and pass it on
            model.Cart = _cart;
            PaymentIntentResult result = await _stripeService.CreatePaymentIntent((model.Cart.Total * 100), model.Email);

            //add the user to the courses but with the PaymentSucceeded set to false for now
            //this is set when the webhook handles the payment success event
            var useraddedresult = AddUserToCourses(user, result.PaymentIntentId);

            if(!useraddedresult.Succeeded)
            {
                //we've been able to create the user but unable to add them to any courses
                //this is highly unlikely but in this event we don't want them to go through
                //to the payment form where the payment will be asked to be collected
                //from stripe.  So just send them to an error page
                CheckoutErrorVM errorVM = new CheckoutErrorVM()
                {
                    ErrorMessage = "There was a problem adding the user to the course(s).  This may be a system problem.  Please try again later."
                };
                return RedirectToAction("CheckoutError", errorVM);
            }
            


            model.Cart.ClientSecret = result.ClientSecret;
            model.PublishableKey = result.PublishableKey;
            return View(model);
        }
       

        public IActionResult CheckoutError(CheckoutErrorVM model)
        {
            return View(model);
        }

        private AddUserCourseResult AddUserToCourses(ApplicationUser user, string stripePaymentId)
        {
            var result = new AddUserCourseResult();
            try
            {
                //Create the UserCourse record so we know what the user has bought
                foreach (CartLine line in _cart.CartLines)
                {
                    var usercourse = new UserCourse()
                    {
                        StripePaymentId = stripePaymentId,
                        UserId = user.Id,
                        CourseId = line.Product.Id
                    };

                    _repository.AddUserToCourse(usercourse);
                }
            }
            catch(Exception ex)
            {
                result.ErrorMessage = ex.Message;
                result.Succeeded = false;
            }

            result.Succeeded = true;
            return result;

        }

        /// <summary>
        /// This will be called from the StripeController Webhook that has processed
        /// the payment
        /// </summary>
        /// <returns></returns>
        public IActionResult CheckoutConfirmation()
        {
            return View();
        }

    }
}
