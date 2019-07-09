using AT.Data;
using AT.Data.Blog;
using AT.Models.BlogViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AT.Controllers
{
    
    public class BlogController: Controller
    {
        private readonly IBlogRepository _repository;
        private readonly IHostingEnvironment _environment;

        public BlogController(IBlogRepository rep, IHostingEnvironment environment)
        {
            _repository = rep;
            _environment = environment;
        }
        public IActionResult Blog()
        {
            return View();
        }

        #region CRUD Admin screens

        public IActionResult Create()
        {
            BlogVM model = new BlogVM();
            model.Article = new Article();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogVM model, IFormFile upload)
        {
            if (ModelState.IsValid)
            {


                Article art = model.Article;

                art.PostedOn = DateTime.Today;
                art.Published = false;

                art = await SaveImage(upload, art);


                _repository.AddArticle(art);
                model.ShowSuccessToast = true;
                return View(model);

            }
            return View(model);
           
        }
        private async Task<Article> SaveImage(IFormFile image, Article art)
        {
            if (image != null && image.Length > 0)
            {
                string fileName = image.FileName;

                art.Image = new FilePath();
                art.Image.FileName = fileName;
                art.Image.Url = Article.Folder + fileName;

               
                var uploads = Path.Combine(_environment.WebRootPath, "uploads", "blog");

                using (var fileStream = new FileStream(Path.Combine(uploads, art.Image.FileName), FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

            }

            return art;

        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            int articleId = id ?? default(int);
            var article = _repository.GetArticle(articleId);
            if (article == null)
            {
                return NotFound();
            }

            BlogVM model = new BlogVM();
            model.Article = article;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, BlogVM model)
        {
            if (id != model.Article.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    
                    _repository.UpdateArticle(model.Article);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_repository.ArticleExists(model.Article.Id))
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
            int articleid = id ?? default(int);
            var art = _repository.GetArticle(articleid);

            if (art == null)
            {
                return NotFound();
            }

            return View(art);
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _repository.DeleteArticle(id);
            return RedirectToAction("Index");
        }

        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            IPagedList<Article> articles = new PagedList<Article>(_repository.GetAllArticles(), pageNumber, pageSize);

            return View(articles);

        }

        #endregion
    }
}
