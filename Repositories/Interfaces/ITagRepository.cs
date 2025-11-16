using BusinessObjects.Models;
using System.Collections.Generic;
namespace Repositories.Interfaces
{
    public interface ITagRepository
    {
        List<Tag> GetTags();
    }
}