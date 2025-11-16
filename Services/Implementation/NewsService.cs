using BusinessObjects;
using BusinessObjects.Models;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;

namespace Services.Implementation
{
    public class NewsService : INewsService
    {
        private readonly INewsArticleRepository _repository;

        public NewsService(INewsArticleRepository repository)
        {
            _repository = repository;
        }

        public List<NewsArticle> GetActiveNews() => _repository.GetActiveNews();

        public List<NewsArticle> GetNewsByDateRange(DateTime startDate, DateTime endDate) => _repository.GetNewsByDateRange(startDate, endDate);
        public List<NewsArticle> GetNewsByAuthorId(short authorId) => _repository.GetNewsByAuthorId(authorId);

        public List<NewsArticle> GetNewsArticles(string? searchQuery)
        {
            var articles = _repository.GetNewsArticles();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                articles = articles.Where(n =>
                    n.NewsTitle.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            return articles;
        }

        public void DeleteNewsArticle(string newsArticleId) => _repository.DeleteNewsArticle(newsArticleId);

        public NewsArticle GetNewsArticleById(string id) => _repository.GetNewsArticleById(id);
        public void AddNewsArticle(NewsArticle article, List<int> tagIds) => _repository.AddNewsArticle(article, tagIds);
        public void UpdateNewsArticle(NewsArticle article, List<int> tagIds) => _repository.UpdateNewsArticle(article, tagIds);
    }
}