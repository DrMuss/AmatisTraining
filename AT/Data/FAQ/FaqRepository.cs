using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace AT.Data.FAQ
{
    public class FaqRepository : IFaqRepository
    {
        private readonly FaqContext _context;

        public FaqRepository(FaqContext context)
        {
            _context = context;
        }
        public void AddFAQ(FAQ faq)
        {
            _context.FAQs.Add(faq);
            _context.SaveChanges();
        }

        

        public void DeleteFAQ(int faqId)
        {
            var faq = _context.FAQs.SingleOrDefault(m => m.Id == faqId);
            _context.FAQs.Remove(faq);
            _context.SaveChanges();
        }

      

        public bool FAQExists(int id)
        {
            return _context.FAQs.Any(e => e.Id == id);
        }

      

        public FAQ GetFAQ(int id)
        {
            return _context.FAQs.SingleOrDefault(m => m.Id == id);
        }

        public IQueryable<FAQ> GetFAQs()
        {
            return _context.FAQs;
        }

      
        public void UpdateFAQ(FAQ faq)
        {
            _context.FAQs.Update(faq);
            _context.SaveChanges();
        }

    }
}
