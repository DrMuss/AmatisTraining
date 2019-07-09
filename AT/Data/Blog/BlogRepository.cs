using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AT.Data.Blog
{
    public class BlogRepository : IBlogRepository
    {
        private readonly BlogContext _context;

        public BlogRepository(BlogContext context)
        {
            _context = context;
        }
        public void AddArticle(Article article)
        {
            _context.Articles.Add(article);
            _context.SaveChanges();
        }

       

        public bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }

       

        public void DeleteArticle(int articleId)
        {
            var article = _context.Articles.SingleOrDefault(m => m.Id == articleId);
            _context.Articles.Remove(article);
            _context.SaveChanges();
        }

      

        public Article GetArticle(int id)
        {
            return _context.Articles
                .Include(c=>c.Image)
                .Include(c=>c.Document)
                .SingleOrDefault(m => m.Id == id);
        }


        


        public IQueryable<Article> GetBlog()
        {
            return _context.Articles
                                    .Include(c => c.Image)
                                    
                                    .Where(a=>a.Published == true)
                                    .OrderByDescending(p => p.PostedOn);
        }

        public IQueryable<Article> GetAllArticles()
        {
            return _context.Articles
                                    .Include(c => c.Image)
                                    .OrderByDescending(p => p.PostedOn);
        }

        public void UpdateArticle(Article article)
        {
            _context.Articles.Update(article);
            _context.SaveChanges();
        }

   


  

      

        public int TotalPosts()
        {
            return _context.Articles
                .Where(p => p.Published)
                .Count();
        }

     




        public IList<Article> GetRecentPosts()
        {
            var query = _context.Articles.Include(a=>a.Image)
                            .OrderByDescending(m => m.PostedOn)
                            .Where(m => m.PostedOn <= DateTime.Now  && m.Published == true)
                            .Take(3);

            return query.ToList();
        }

       
    }
}
