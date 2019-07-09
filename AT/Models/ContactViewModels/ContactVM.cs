using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AT.Models.ContactViewModels
{
    public class ContactVM
    {
     
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Your name can only contain letters of the alphabet!")]
        public string Name { get; set; }
        [EmailAddress(ErrorMessage = "You must provide a valid email address")]
        public string Email { get; set; }

        public string Message { get; set; }

       
        public string Acknowledge { get; set; }
        public string Recaptcha { get; set; }
       
        public string Secret { get; set; }
    }
}
