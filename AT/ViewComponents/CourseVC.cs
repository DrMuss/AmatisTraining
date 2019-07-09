using AT.Data.Courses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AT.ViewComponents
{
    public class CourseVC : ViewComponent
    {
        private readonly IATRepository _repository;
        public CourseVC(IATRepository rep)
        {
            _repository = rep;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            IList<Course>  courses= _repository.GetCourses().ToList();
            return View(courses);
        }


    }
}


