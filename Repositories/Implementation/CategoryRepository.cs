using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        public List<Category> GetCategories() => CategoryDAO.Instance.GetCategories();
        public Category GetCategoryById(short id) => CategoryDAO.Instance.GetCategoryById(id);
        public void AddCategory(Category category) => CategoryDAO.Instance.AddCategory(category);
        public void UpdateCategory(Category category) => CategoryDAO.Instance.UpdateCategory(category);
        public void DeleteCategory(short id) => CategoryDAO.Instance.DeleteCategory(id);
    }
}
