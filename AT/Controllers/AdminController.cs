using AT.Data.Courses;
using AT.Data.Identity;
using AT.Models.CourseViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AT.Controllers
{
    /// <summary>
    /// This is the main hub for the Admin user to carry out the Admin functions
    /// </summary>
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IATRepository _repository;

        public AdminController(UserManager<ApplicationUser> userManager,
                                IATRepository repository)
        {
            _userManager = userManager;
            _repository = repository;

        }

        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult ManageUsers()
        {
            return View();
        }
        public IActionResult Users()
        {
            IQueryable<ApplicationUser> users = _userManager.Users;
            IQueryable<UserCourse> usercourses = _repository.GetAllUserCourses();
            IQueryable<Course> courses = _repository.GetCourses();

            //A monster Linq query to join 3 lists.  The lists will be relatively small
            var query = from user in users
                        join usercourse in usercourses on user.Id equals usercourse.UserId
                        select new {user.Id,user.Email, user.FirstName, user.LastName, usercourse.CourseId, usercourse.ChargeDate} 
                        into intermediate
                        join course in courses on intermediate.CourseId equals course.Id
                        select new UserCourseVM
                            {UserId = intermediate.Id,
                            CourseId = intermediate.CourseId,
                              CourseName = course.Name,
                              Email = intermediate.Email,
                              FirstName = intermediate.FirstName,
                              LastName = intermediate.LastName,
                              EnrolledOn = intermediate.ChargeDate.Value};


            return View(query);

        }


    }
}
