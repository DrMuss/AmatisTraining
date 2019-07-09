using AT.Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AT.Models.CheckoutViewModels
{
    public class CheckoutUserResult
    {
        public string ErrorMessage { get; set; }
        public ApplicationUser User { get; set; }
        public bool HasErrors { get; set; }
    }
}
