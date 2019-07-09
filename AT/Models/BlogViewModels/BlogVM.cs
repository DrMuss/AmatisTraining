using AT.Data.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AT.Models.BlogViewModels
{
    public class BlogVM
    {
        public Article Article { get; set; }
        public bool ShowSuccessToast { get; set; }
    }
}
