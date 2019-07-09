using AT.Models.CourseViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;
using AT.Data.Courses;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System.IO;

namespace AT.Controllers
{
    public class CourseController : Controller
    {
        private readonly IATRepository _repository;
        private readonly IHostingEnvironment _environment;

        public CourseController(IATRepository rep, IHostingEnvironment environment)
        {
            _repository = rep;
            _environment = environment;
        }

        public IActionResult Course(int id, string slug = null)
        {
            ViewData["ReturnUrl"] = GetRawTarget(Request);
            Course model = _repository.GetCourse(id);

            //replace the string for Bullets with proper Html


            return View(model);
        }
        #region CRUD Admin screens

        public IActionResult Create()
        {
            CourseVM model = new CourseVM();
            model.Course = new Course();
            return View(model);
           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseVM model, IFormFile upload)
        {
            if (ModelState.IsValid)
            {

                Course art = model.Course;
                art = await SaveImage(upload, art);
                art.Bullets = DecodeBullets(model.Info);
                //Create the CourseInfo objects from the model.Info string
                //model.SetCourseInfoFromString(model.Info);

                _repository.AddCourse(art);
                model.ShowSuccessToast = true;
                return View(model);

            }
            return View(model);

        }

        private async Task<Course> SaveImage(IFormFile image, Course art)
        {
            if (image != null && image.Length > 0)
            {
                string fileName = image.FileName;

                art.ImageFileName = fileName;
                art.ImageUrl = AT.Data.Courses.Course.Folder + fileName;
                


                var uploads = Path.Combine(_environment.WebRootPath, "uploads", "courses");

                using (var fileStream = new FileStream(Path.Combine(uploads, art.ImageFileName), FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

            }

            return art;

        }
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            int courseId = id ?? default(int);
            var course = _repository.GetCourse(courseId);
            if (course == null)
            {
                return NotFound();
            }

            CourseVM model = new CourseVM();
            model.Course = course;
            model.Info = course.Bullets;

            //create a string from the model info objects and set it

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, CourseVM model)
        {
            if (id != model.Course.Id)
            {
                return NotFound();
    }

            if (ModelState.IsValid)
            {
                try
                {
                    //update the course info objects list from the model.Info
                    Course crs = model.Course;

                    //replace the string for Bullets with proper Html

                    crs.Bullets = DecodeBullets(model.Info);
                    _repository.UpdateCourse(crs);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_repository.CourseExists(model.Course.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                model.ShowSuccessToast = true;
                return View(model);
            }
            return View(model);
        }

        private string DecodeBullets(string inputBullets)
        {
            string s = inputBullets.Replace("&lt;i class='fa fa-circle'&gt;&lt;/i&gt;", "<i class=\"fa fa-circle\"></i>");
            return s;
        }
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            int courseid = id ?? default(int);
            var course = _repository.GetCourse(courseid);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _repository.DeleteCourse(id);
            return RedirectToAction("Index");
        }

        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            IPagedList<Course> courses = new PagedList<Course>(_repository.GetCourses(), pageNumber, pageSize);

            return View(courses);

        }

        #endregion


        private static string GetRawTarget(HttpRequest request)
        {
            var httpRequestFeature = request.HttpContext.Features.Get<IHttpRequestFeature>();
            return httpRequestFeature.RawTarget;
        }

    }
}
