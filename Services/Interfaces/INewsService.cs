using BusinessObjects;
using BusinessObjects.Models;
using System;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface INewsService
    {
        List<NewsArticle> GetActiveNews();
        List<NewsArticle> GetNewsByDateRange(DateTime startDate, DateTime endDate);
        List<NewsArticle> GetNewsByAuthorId(short authorId);
        List<NewsArticle> GetNewsArticles(string? searchQuery);
        void DeleteNewsArticle(string newsArticleId);
        NewsArticle GetNewsArticleById(string id);
        void AddNewsArticle(NewsArticle article, List<int> tagIds);
        void UpdateNewsArticle(NewsArticle article, List<int> tagIds);
    }
}