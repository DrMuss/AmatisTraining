using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AT.Services.Stripe
{
    public class StripePaymentResult 
    {
        public string ErrorMessage { get; set; }
        public bool HasErrors { get; set; }
    }
}
