using BusinessObjects;
using BusinessObjects.Models;
using DataAccessObjects;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repositories.Implementation
{
    public class NewsArticleRepository : INewsArticleRepository
    {
        public List<NewsArticle> GetActiveNews() => NewsArticleDAO.Instance.GetActiveNews();
        public List<NewsArticle> GetNewsByDateRange(DateTime startDate, DateTime endDate) => NewsArticleDAO.Instance.GetNewsByDateRange(startDate, endDate);
        public List<NewsArticle> GetNewsByAuthorId(short authorId) => NewsArticleDAO.Instance.GetNewsByAuthorId(authorId);
        public List<NewsArticle> GetNewsArticles() => NewsArticleDAO.Instance.GetNewsArticles();
        public void DeleteNewsArticle(string newsArticleId)=> NewsArticleDAO.Instance.DeleteNewsArticle(newsArticleId);

        public NewsArticle GetNewsArticleById(string id)
        {
            using (var context = new FunewsManagementContext())
            {
                // Fix: Replace .Include(n => n.NewsTags) with .Include(n => n.Tags)
                return context.NewsArticles
                    .Include(n => n.Tags)
                    .FirstOrDefault(n => n.NewsArticleId == id);
            }
        }

        // Change the type of tagIds in AddNewsArticle from List<short> to List<int>
        public void AddNewsArticle(NewsArticle article, List<int> tagIds)
        {
            // Dùng Transaction
            using (var context = new FunewsManagementContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // Thêm bài báo
                        context.NewsArticles.Add(article);
                        context.SaveChanges(); // Lưu để lấy ID

                        // Lấy các Tag object từ DB
                        var tagsToAdd = context.Tags
                                               .Where(t => tagIds.Contains(t.TagId))
                                               .ToList();

                        // Gán Tag objects vào bài báo
                        var articleFromDb = context.NewsArticles.Find(article.NewsArticleId);
                        foreach (var tag in tagsToAdd)
                        {
                            articleFromDb.Tags.Add(tag);
                        }
                        context.SaveChanges();

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void UpdateNewsArticle(NewsArticle article, List<int> tagIds)
        {
            NewsArticleDAO.Instance.UpdateNewsArticle(article);
            NewsArticleDAO.Instance.UpdateNewsTags(article.NewsArticleId, tagIds);
        }
    }
}