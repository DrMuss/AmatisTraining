using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AT.Services.Stripe
{
    public class PaymentIntentResult
    {
        public string PublishableKey { get; set; }
        public string ClientSecret { get; set; }
        public string PaymentIntentId { get; set; }

        public string ErrorMessage { get; set; }
        public bool HasErrors { get; set; }
    }
}
