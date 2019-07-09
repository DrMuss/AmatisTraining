using AT.Data.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AT.Models.CheckoutViewModels
{
    public class CheckoutVM 
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Mobile { get; set; }
        public string Division { get; set; }
        public string JobTitle { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        
        public string ClientSecret { get; set; }
        public string PublishableKey { get; set; }

         public ICart Cart { get; set; }
         
        

    }
}
