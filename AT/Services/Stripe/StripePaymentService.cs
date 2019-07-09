using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AT.Data.Identity;
using Microsoft.Extensions.Options;
using Stripe;

namespace AT.Services.Stripe
{
    public class StripePaymentService : IStripePaymentService
    {
        private readonly StripeConfig _config;

        public StripePaymentService(IOptions<StripeConfig> stripeSettings)
        {
            _config = stripeSettings.Value;
        }

        /// <summary>
        /// Called from the webhook so this needs the webhoook secret not the other one
        /// </summary>
        /// <param name="json"></param>
        /// <param name="stripesignature"></param>
        /// <returns></returns>
        public Event GetEvent(string json, string stripesignature)
        {
            return EventUtility.ConstructEvent(json,
                                        stripesignature,
                                        _config.WebHookSecret);
        }
        public async Task<PaymentIntentResult> CreatePaymentIntent(int amount, string email)
        {
            
            StripeConfiguration.ApiKey = _config.Secretkey;

            var service = new PaymentIntentService();
            var options = new PaymentIntentCreateOptions
            {
                Amount = amount,
                Currency = "gbp",
                ReceiptEmail = email,
                Description = "Amatis Training",
              
            };
            PaymentIntent intent = await service.CreateAsync(options);

            //this is what we want to pass back to the client for making the payment
            PaymentIntentResult result = new PaymentIntentResult()
            {
                ClientSecret = intent.ClientSecret,
                PublishableKey = _config.PublishableKey,
                PaymentIntentId = intent.Id
            };

            return result;
        }

        public Task<StripePaymentResult> ProcessPayment(ApplicationUser user)
        {
            throw new NotImplementedException();
        }
    }
}
