using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace Tuannahe181942RazorPages.Pages.Admin.Reports
{
    public class IndexModel : PageModel
    {
        private readonly INewsService _newsService;
        public IndexModel(INewsService newsService) { _newsService = newsService; }

        public List<NewsArticle> NewsList { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? EndDate { get; set; }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToPage("/Login");
            }

            if (StartDate.HasValue && EndDate.HasValue)
            {
                // Đảm bảo EndDate bao gồm cả ngày
                NewsList = _newsService.GetNewsByDateRange(StartDate.Value, EndDate.Value.AddDays(1).AddTicks(-1));
            }
            return Page();
        }
    }
}
