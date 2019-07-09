using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stripe;
using AT.Data.Courses;
using AT.Services.Stripe;
using System.IO;
using AT.Services.Messaging;
using AT.Data.Identity;
using Microsoft.AspNetCore.Identity;

namespace AT.Controllers
{
    [Route("api/stripe")]
    public class StripeWebhookController : Controller
    {
        private readonly ICart _cart;
        private readonly IStripePaymentService _stripeService;
        private readonly IATRepository _repository;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;

        public StripeWebhookController(ICart cart,
                                        IATRepository repository,
                                        IEmailSender emailSender,
                                        UserManager<ApplicationUser> userManager,
                                        IStripePaymentService stripeService)
        {
            _stripeService = stripeService;
            _cart = cart;
            _repository = repository;
            _emailSender = emailSender;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult> Index()
        {
            try
            {
                var json = new StreamReader(HttpContext.Request.Body).ReadToEnd();
                var stripeEvent = _stripeService.GetEvent(json,
                                                          Request.Headers["Stripe-Signature"]); 

                PaymentIntent intent = null;
                IList<UserCourse> ucList = new List<UserCourse>();

                switch (stripeEvent.Type)
                {
                    case "payment_intent.succeeded":
                        intent = (PaymentIntent)stripeEvent.Data.Object;

                        // Fulfil the customer's purchase by adding the info to the usercourses
                        ucList = _repository.GetUserCourses(intent.Id).ToList();

                        foreach(UserCourse uc in ucList)
                        {
                            uc.ChargeDate = intent.Created.Value;
                            uc.PaymentSuccessful = true;
                            int i = await _repository.UpdateUserCourseAsync(uc);


                            if (i < 1)
                            {
                                string message = CreateProblemMessage(uc);
                                await _emailSender.SendProblemRegisteringUserForCourseAsync(message);
                            }
                            else
                            {
                                string message = CreateThankyouMessage(uc);
                                string email = await GetCustomerEmailAsync(uc.UserId);

                                await _emailSender.SendRegisterUserForCourseAsync(email,message);
                            }
                               
                        }
                        

                        break;
                    case "payment_intent.payment_failed":
                        intent = (PaymentIntent)stripeEvent.Data.Object;
                        // Fulfil the customer's purchase by adding the info to the usercourses
                        ucList = _repository.GetUserCourses(intent.Id).ToList();
                        // Notify the customer that payment failed
                        foreach (UserCourse uc in ucList)
                        {
                            string customerEmail = await GetCustomerEmailAsync(uc.UserId);
                            string message = CreateCustomerPaymentFailedMessage(intent.Id);
                            await _emailSender.SendFailedPaymentMessageAsync(customerEmail, message);
                        }
                            

                        break;
                    default:
                        // Handle other event types

                        break;
                }
                return new EmptyResult();

            }
            catch (StripeException e)
            {
                // Invalid Signature
                return BadRequest();
            }
        }

        private async Task<string> GetCustomerEmailAsync(string userId)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            return user.Email;
        }

        private string CreateCustomerPaymentFailedMessage(string intentId)
        {
            string message = "<p>There was a problem with your payment to Amatis Training for the course(s) you have purchased.</p>";
            message += string.Format("<p>Please try again with another card or contact Amatis Training to activate your account.  The payment reference is {0}</<p>", intentId);

            return message;
        }


        private string CreateThankyouMessage(UserCourse uc)
        {
            string courseName = _repository.GetCourse(uc.CourseId).Name;
            string paymentreference = uc.StripePaymentId;

            string message = string.Format("<p>Thank you for your purchase of the course {0} from Amatis Training.</p>", courseName);
            message += string.Format("<p>Your payment reference is {0}</p>", paymentreference);
            message += "<p>If you have any questions or queries please do not hesitate to contact us.</p>";
            message += "<p><strong>Kind Regards</strong></p>";
            message += "<p><strong>Amatis Training</strong></p>";

            return message;
        }

        private string CreateProblemMessage(UserCourse uc)
        {
            string message = "<p>There was a problem adding the user to the course.  Their payment has been successful and so they should be added to the course.</p>";
            message += string.Format("<p>You can do this in the Admin Section manually.  The payment Id you need to update is {0}</<p>", uc.StripePaymentId);

            return message;
        }
    }
}
