using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AT.Data.Courses
{
    public class ATRepository : IATRepository
    {
        private readonly ATContext _context;

        public ATRepository(ATContext context)
        {
            _context = context;
        }

        public void AddCourse(Course crs)
        {
            _context.Courses.Add(crs);
            _context.SaveChanges();
        }

        public void AddUserToCourse(UserCourse uc)
        {
            _context.UserCourses.Add(uc);
            _context.SaveChanges();
        }

        public bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }

        public void DeleteCourse(int crsId)
        {
            var course = _context.Courses.SingleOrDefault(m => m.Id == crsId);
            _context.Courses.Remove(course);
            _context.SaveChanges();
        }

        public Course GetCourse(int courseId)
        {
            return _context.Courses
               .SingleOrDefault(m => m.Id == courseId);
        }

        public IQueryable<Course> GetCourses()
        {
            return _context.Courses;
        }

        public UserCourse GetUserCourse(string userId, int courseId)
        {
            return _context.UserCourses
                            .Where(uc => uc.UserId == userId &&
                                         uc.CourseId == courseId)
                            .FirstOrDefault();
        }

        public IQueryable<UserCourse> GetUserCourses(string paymentIntentId)
        {
            return _context.UserCourses.Where(uc => uc.StripePaymentId == paymentIntentId);
        }

        /// <summary>
        /// returns all user courses that users have attemptede to sign up for
        /// </summary>
        /// <returns></returns>
        public IQueryable<UserCourse> GetAllUserCourses()
        {
            return _context.UserCourses;
        }

        public void RemoveUserFromCourse(UserCourse uc)
        {
            _context.UserCourses.Remove(uc);
            _context.SaveChanges();
        }

        public async Task<int> UpdateUserCourseAsync(UserCourse uc)
        {
            _context.UserCourses.Update(uc);
            int i = await _context.SaveChangesAsync();

            return i;
        }

        public void UpdateCourse(Course crs)
        {
            _context.Courses.Update(crs);
            _context.SaveChanges();
        }
    }
}
