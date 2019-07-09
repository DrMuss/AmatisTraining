using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AT.Data.Blog
{
    public class Article
    {
        public static string Folder = "/uploads/blog/";
        public static string ResizeFolder = "/uploads/blog/600x400/";

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }

        [DisplayName("Article")]
        public string Description { get; set; }
        public string Author { get; set; }
        public string UrlSlug { get; set; }
        public string ResizeUrl { get; set; }
        public bool Published { get; set; }
        [DisplayFormat(DataFormatString = "{0: dd-MMM-yyyy}")]
        public DateTime? PostedOn { get; set; }
        public DateTime? Modified { get; set; }
      

        public FilePath Image { get; set; }
        public FilePath Document { get; set; }

        [Url]
        public string ExternalWebsite { get; set; }


    }
}
