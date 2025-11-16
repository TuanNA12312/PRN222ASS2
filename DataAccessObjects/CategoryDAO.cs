using BusinessObjects;
using BusinessObjects.Models;
using DataAccessObjects;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessObjects
{
    public class CategoryDAO
    {
        private static CategoryDAO instance = null;
        private static readonly object instanceLock = new object();
        private CategoryDAO() { }

        public static CategoryDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CategoryDAO();
                    }
                    return instance;
                }
            }
        }
        public List<Category> GetCategories()
        {
            using (var context = new FunewsManagementContext())
            {
                return context.Categories.ToList();
            }
        }

        public Category GetCategoryById(short id)
        {
            using (var context = new FunewsManagementContext())
            {
                return context.Categories.Find(id);
            }
        }

        public void AddCategory(Category category)
        {
            using (var context = new FunewsManagementContext())
            {
                context.Categories.Add(category);
                context.SaveChanges();
            }
        }

        public void UpdateCategory(Category category)
        {
            using (var context = new FunewsManagementContext())
            {
                context.Entry(category).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteCategory(short id)
        {
            using (var context = new FunewsManagementContext())
            {
                var category = context.Categories.Find(id);
                if (category != null)
                {
                    bool isInUse = context.NewsArticles.Any(n => n.CategoryId == id);
                    if (!isInUse)
                    {
                        context.Categories.Remove(category);
                        context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Category is in use and cannot be deleted.");
                    }
                }
            }
        }
    }
}