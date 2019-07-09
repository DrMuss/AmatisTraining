using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AT.Services.Stripe
{
    public class PaymentSuccessResult
    {
        public string CustomerEmail { get; set; }
        public string Message { get; set; }

        public string ErrorMessage { get; set; }
        public string HasError { get; set; }
    }
}
