using BusinessObjects;
using BusinessObjects.Models;
using DataAccessObjects;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataAccessObjects
{
    public class NewsArticleDAO
    {
        private static NewsArticleDAO instance = null;
        public static NewsArticleDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NewsArticleDAO();
                }
                return instance;
            }
        }

        public List<NewsArticle> GetActiveNews()
        {
            using (var context = new FunewsManagementContext())
            {
                return context.NewsArticles
                    .Where(n => n.NewsStatus == true)
                    .OrderByDescending(n => n.CreatedDate)
                    .ToList();
            }
        }

        public List<NewsArticle> GetNewsByDateRange(DateTime startDate, DateTime endDate)
        {
            using (var context = new FunewsManagementContext())
            {
                return context.NewsArticles
                    .Where(n => n.CreatedDate >= startDate && n.CreatedDate <= endDate)
                    .OrderByDescending(n => n.CreatedDate)
                    .ToList();
            }
        }

        public List<NewsArticle> GetNewsByAuthorId(short authorId)
        {
            using (var context = new FunewsManagementContext())
            {
                return context.NewsArticles
                    .Where(n => n.CreatedById == authorId)
                    .OrderByDescending(n => n.CreatedDate)
                    .ToList();
            }
        }

        public List<NewsArticle> GetNewsArticles()
        {
            using (var context = new FunewsManagementContext())
            {
                return context.NewsArticles
                    .Include(n => n.Category)
                    .OrderByDescending(n => n.CreatedDate)
                    .ToList();
            }
        }

        public void DeleteNewsArticle(string newsArticleId)
        {
            using (var context = new FunewsManagementContext())
            {
                var article = context.NewsArticles.Find(newsArticleId);
                if (article != null)
                {
                    context.NewsArticles.Remove(article);
                    context.SaveChanges();
                }
            }
        }

        public void AddNewsArticle(NewsArticle article)
        {
            using (var context = new FunewsManagementContext())
            {
                // Chúng ta sẽ xử lý Tags ở tầng Repository
                context.NewsArticles.Add(article);
                context.SaveChanges();
            }
        }

        public void UpdateNewsArticle(NewsArticle article)
        {
            using (var context = new FunewsManagementContext())
            {
                context.Entry(article).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        // Change the type of tagIds parameter from List<short> to List<int> in UpdateNewsTags method
        public void UpdateNewsTags(string newsArticleId, List<int> tagIds)
        {
            using (var context = new FunewsManagementContext())
            {
                var articleToUpdate = context.NewsArticles
                                             .Include(n => n.Tags)
                                             .FirstOrDefault(n => n.NewsArticleId == newsArticleId);

                if (articleToUpdate == null)
                {
                    throw new Exception("Article not found.");
                }

                articleToUpdate.Tags.Clear();

                var newTags = context.Tags
                                     .Where(t => tagIds.Contains(t.TagId))
                                     .ToList();

                foreach (var tag in newTags)
                {
                    articleToUpdate.Tags.Add(tag);
                }

                context.SaveChanges();
            }
        }
    }
}