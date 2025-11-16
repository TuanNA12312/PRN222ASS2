using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.Interfaces;
using System.Collections.Generic;
namespace Repositories.Implementation
{
    public class TagRepository : ITagRepository
    {
        public List<Tag> GetTags() => TagDAO.Instance.GetTags();
    }
}