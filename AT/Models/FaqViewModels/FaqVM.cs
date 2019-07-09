using AT.Data.FAQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AT.Models.FaqViewModels
{
    public class FaqVM
    {
        public FAQ Faq { get; set; }
        public bool ShowSuccessToast { get; set; }
    }
}
