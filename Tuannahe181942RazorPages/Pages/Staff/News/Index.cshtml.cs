using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Services.Interfaces;
using Tuannahe181942RazorPages.Hubs;

namespace Tuannahe181942RazorPages.Pages.Staff.News
{
    public class IndexModel : PageModel
    {
        private readonly INewsService _newsService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        private readonly IHubContext<NewsHub> _hubContext; // Inject Hub

        public IndexModel(INewsService newsService, ICategoryService categoryService, ITagService tagService, IHubContext<NewsHub> hubContext)
        {
            _newsService = newsService;
            _categoryService = categoryService;
            _tagService = tagService;
            _hubContext = hubContext;
        }

        public List<NewsArticle> NewsList { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; }
        [BindProperty]
        public NewsArticle News { get; set; }
        [BindProperty]
        public List<int> SelectedTagIds { get; set; } // Dùng để nhận Tags từ form

        // Load trang
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("Role") != "Staff")
            {
                return RedirectToPage("/Login");
            }
            NewsList = _newsService.GetNewsArticles(SearchQuery);
            return Page();
        }

        // Xử lý Delete
        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            _newsService.DeleteNewsArticle(id);
            // Gửi thông báo SignalR
            await _hubContext.Clients.All.SendAsync("LoadNews");
            return RedirectToPage("Index");
        }

        // --- POPUP HANDLERS ---

        private void LoadDropdowns()
        {
            // Dùng để load Categories/Tags cho popup
            ViewData["Categories"] = _categoryService.GetCategories(null);
            ViewData["Tags"] = _tagService.GetTags();
        }

        // Load popup Create
        public IActionResult OnGetCreatePartial()
        {
            LoadDropdowns();
            return Partial("_Create", new NewsArticle { NewsStatus = true, CreatedDate = DateTime.Now });
        }

        // Load popup Edit
        public IActionResult OnGetEditPartial(string id)
        {
            LoadDropdowns();
            News = _newsService.GetNewsArticleById(id);
            // Lấy các tag đã chọn
            SelectedTagIds = News.Tags.Select(nt => nt.TagId).ToList();
            return Partial("_Edit", this); // Gửi cả PageModel vì _Edit cần SelectedTagIds
        }

        // Submit popup Create
        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                LoadDropdowns();
                return Partial("_Create", News);
            }

            News.NewsArticleId = $"News{DateTime.Now.Ticks}"; // Tạo ID tạm
            News.CreatedById = short.Parse(HttpContext.Session.GetString("AccountId"));
            _newsService.AddNewsArticle(News, SelectedTagIds ?? new List<int>());

            // Gửi thông báo SignalR
            await _hubContext.Clients.All.SendAsync("LoadNews");
            return new JsonResult(new { success = true });
        }

        // Submit popup Edit    
        public async Task<IActionResult> OnPostEditAsync()
        {
            if (!ModelState.IsValid)
            {
                LoadDropdowns();
                // Cần load lại News object đầy đủ để gửi lại
                var model = new { News, SelectedTagIds };
                return Partial("_Edit", model);
            }

            // Lấy AccountId từ Session
            News.CreatedById = short.Parse(HttpContext.Session.GetString("AccountId"));
            _newsService.UpdateNewsArticle(News, SelectedTagIds ?? new List<int>());

            // Gửi thông báo SignalR
            await _hubContext.Clients.All.SendAsync("LoadNews");
            return new JsonResult(new { success = true });
        }
    }
}
