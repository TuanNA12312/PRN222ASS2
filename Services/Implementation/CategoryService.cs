using BusinessObjects.Models;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public List<Category> GetCategories(string? searchQuery)
        {
            var categories = _repository.GetCategories();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                categories = categories.Where(c =>
                    c.CategoryName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            return categories;
        }

        public Category GetCategoryById(short id) => _repository.GetCategoryById(id);
        public void AddCategory(Category category) => _repository.AddCategory(category);
        public void UpdateCategory(Category category) => _repository.UpdateCategory(category);
        public void DeleteCategory(short id) => _repository.DeleteCategory(id);
    }
}
