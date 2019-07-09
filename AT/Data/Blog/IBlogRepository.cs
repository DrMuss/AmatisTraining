using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AT.Data.Blog
{
    public interface IBlogRepository
    {
        
        IQueryable<Article> GetBlog();
        IQueryable<Article> GetAllArticles();

        void UpdateArticle(Article article);
        void AddArticle(Article article);
        void DeleteArticle(int articleId);
        Article GetArticle(int id);
        bool ArticleExists(int id);



      

       
        int TotalPosts();
       

        IList<Article> GetRecentPosts();


    }
}
