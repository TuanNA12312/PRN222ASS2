using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace Tuannahe181942RazorPages.Pages.Staff.History
{
    public class IndexModel : PageModel
    {
        private readonly INewsService _newsService;
        public IndexModel(INewsService newsService) { _newsService = newsService; }

        public List<NewsArticle> NewsList { get; set; } = new List<NewsArticle>();

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Staff")
            {
                return RedirectToPage("/Login");
            }

            var accountIdStr = HttpContext.Session.GetString("AccountId");
            if (short.TryParse(accountIdStr, out short accountId))
            {
                NewsList = _newsService.GetNewsByAuthorId(accountId);
            }
            else
            {
                NewsList = new List<NewsArticle>();
            }

            return Page();
        }
    }
}
