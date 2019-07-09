using AT.Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stripe;

namespace AT.Services.Stripe
{
    public interface IStripePaymentService
    {
        Event GetEvent(string json, string stripesignature);

        Task<PaymentIntentResult> CreatePaymentIntent(int amount, string email);

        Task<StripePaymentResult> ProcessPayment(ApplicationUser user);
    }
}
