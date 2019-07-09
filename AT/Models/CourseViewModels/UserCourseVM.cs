using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AT.Models.CourseViewModels
{
    public class UserCourseVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public DateTime EnrolledOn { get; set; }
        public string CourseName { get; set; }

        public string UserId { get; set; }
        public int CourseId { get; set; }
    }
}
