using AT.Models.ContactViewModels;
using AT.Services.Messaging;
using AT.Services.Recapture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace AT.Controllers
{
    public class ContactController : Controller
    {
        private readonly RecaptchaConfig _config;
      
        private readonly IEmailSender _emailSender;

        public ContactController(IEmailSender emailSender,
                                 IOptions<RecaptchaConfig> recaptchaSettings)
        {

            _config = recaptchaSettings.Value;
            _emailSender = emailSender;
           
        }

        /// <summary>
        /// The contact form messaging
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult Contact()
        {
            
            ContactVM model = new ContactVM();
            model.Recaptcha = _config.SiteKey;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Contact(ContactVM contact)
        {


            if (ModelState.IsValid)
            {
                if (!ReCaptchaPassed(
                                    Request.Form["g-recaptcha-response"], // that's how you get it from the Request object
                                    _config.SecretKey))
                {
                    ModelState.AddModelError(string.Empty, "You failed the CAPTCHA.");
                    return View(contact);
                }
                //sendemail
                //the message must include the person's email address and contact details
                string message = "<p>This message was from " + contact.Name + " and their email address is " + contact.Email + "</p>";

                


                message += "<p>" + contact.Message + "</p>";


                await _emailSender.SendContactEmailAsync(contact.Name,
                                                  "Message from Contact Form",
                                                  message);

                contact.Acknowledge = "Thank you.  We have received your email.";

                return View(contact);
            }

            else
            {
                return View(contact);
            }

        }

        // A function that checks reCAPTCHA results
        // You might want to move it to some common class
        [AllowAnonymous]
        public static bool ReCaptchaPassed(string gRecaptchaResponse, string secret)
        {
            HttpClient httpClient = new HttpClient();
            var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={gRecaptchaResponse}").Result;
            if (res.StatusCode != HttpStatusCode.OK)
            {

                return false;
            }

            string JSONres = res.Content.ReadAsStringAsync().Result;
            dynamic JSONdata = JObject.Parse(JSONres);
            if (JSONdata.success != "true")
            {
                return false;
            }

            return true;
        }


    }
}
