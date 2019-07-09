using AT.Data.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AT.Models.CourseViewModels
{
    public class CourseVM
    {
        public Course Course { get; set; }
        public bool ShowSuccessToast { get; set; }

        public string Info { get; set; }

        /*
        public void SetCourseInfoFromString(string input)
        {
            //convert </li> closing tags to pipes
            var s = input.Replace("</li>", "|");
            //split the string into an array based on this
            string[] arr = s.Split('|');

            Course.CourseInfo = new List<CourseInfo>();
            foreach(string output in arr)
            {
                int pos = output.LastIndexOf('>');
                if(pos >=0)
                {
                    string w = output.Substring(pos, output.Length - pos);
                    w = w.Replace(">", "");
                    CourseInfo courseInfoobj = new CourseInfo() { Text = w };
                    Course.CourseInfo.Add(courseInfoobj);
                }
                
            }
           
        }
        */
    }
}
