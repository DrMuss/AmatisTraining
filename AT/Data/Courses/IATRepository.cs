using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AT.Data.Courses
{
    public interface IATRepository
    {
        Course GetCourse(int courseId);
        IQueryable<Course> GetCourses();
        void UpdateCourse(Course crs);
        void AddCourse(Course crs);
        void DeleteCourse(int crsId);
        bool CourseExists(int id);

        //methods to GetUsersCourses and add, edit and delete
        //we get the usercourses list from the payment intent Id in the webhook callback
        IQueryable<UserCourse> GetUserCourses(string paymentIntentId);
        UserCourse GetUserCourse(string userId, int CourseId);
        Task<int> UpdateUserCourseAsync(UserCourse uc);
        void AddUserToCourse(UserCourse uc);
        void RemoveUserFromCourse(UserCourse uc);
        IQueryable<UserCourse> GetAllUserCourses();


    }
}
