using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace AT.Data.FAQ
{
    public interface IFaqRepository
    {
        IQueryable<FAQ> GetFAQs();
        void UpdateFAQ(FAQ faq);
        void AddFAQ(FAQ faq);
        void DeleteFAQ(int faqId);
        FAQ GetFAQ(int id);

        bool FAQExists(int id);


       
    }
}
