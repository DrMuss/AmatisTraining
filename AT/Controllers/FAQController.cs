using AT.Data.FAQ;
using AT.Models.FaqViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AT.Controllers
{
    public class FAQController: Controller
    {
        private readonly IFaqRepository _repository;
     

        public FAQController(IFaqRepository rep)
        {
            _repository = rep;
            
        }

        public IActionResult FAQs()
        {
            var model = _repository.GetFAQs().ToList();
            return View(model);
        }


        #region CRUD Admin screens

        public IActionResult Create()
        {
            FaqVM model = new FaqVM();
            model.Faq = new FAQ();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FaqVM model)
        {
            if (ModelState.IsValid)
            {


                FAQ faq = model.Faq;
                _repository.AddFAQ(faq);
                model.ShowSuccessToast = true;
                return View(model);

            }
            return View(model);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            int faqId = id ?? default(int);
            var faq = _repository.GetFAQ(faqId);
            if (faq == null)
            {
                return NotFound();
            }

            FaqVM model = new FaqVM();
            model.Faq = faq;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, FaqVM model)
        {
            if (id != model.Faq.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    _repository.UpdateFAQ(model.Faq);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_repository.FAQExists(model.Faq.Id))
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
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            int faqId = id ?? default(int);
            var faq = _repository.GetFAQ(faqId);

            if (faq == null)
            {
                return NotFound();
            }

            return View(faq);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _repository.DeleteFAQ(id);
            return RedirectToAction("Index");
        }

        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            IPagedList<FAQ> faqs = new PagedList<FAQ>(_repository.GetFAQs(), pageNumber, pageSize);

            return View(faqs);
        }

        #endregion
    
}
}
