using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AT.Data.Courses
{
    public class UserCourse
    {
       
        public string UserId { get; set; }
        public int CourseId { get; set; }
        public string StripePaymentId { get; set; }
        public DateTime? ChargeDate { get; set; }
        public bool PaymentSuccessful { get; set; }


    }
}
