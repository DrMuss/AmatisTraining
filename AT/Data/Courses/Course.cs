using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AT.Data.Courses
{
    public class Course
    {
        public static string Folder = "/uploads/courses/";
        public static string ResizeFolder = "/uploads/courses/600x400/";

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        //quoted in pennies
        [Required]
        public int Price { get; set; }

        public string ImageUrl { get; set; }
        public string ImageFileName { get; set; }

        public string WarrantyLeft { get; set; }
        public string WarrantyRight { get; set; }

        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string Slug
        {
            get
            {
                return CreateSlug(Name);
            }
        }

      

        public string Bullets { get; set; }
        public bool HostedOnAgylia { get; set; }

        #region Private Slug Methods
        private static string CreateSlug(string name)
        {
            //remove accents
            var slug = RemoveAccent(name);
            //remove characters nonalphanumeric
            slug = Regex.Replace(slug, @"[^a-zA-Z0-9 -]", "");

            // convert multiple spaces into one space   
            slug = Regex.Replace(slug, @"\s+", " ").Trim();

            // cut and trim 
            slug = slug.Substring(0, slug.Length <= 45 ? slug.Length : 45).Trim();
            slug = Regex.Replace(slug, @"\s", "-"); // hyphens 

            return slug.ToLower();
        }


        private static string RemoveAccent(string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
        #endregion
    }
}
