using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace Tuannahe181942RazorPages.Pages.Admin.Categories
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public IndexModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public List<Category> CategoryList { get; set; } = new List<Category>();

        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; }

        [BindProperty]
        public Category Category { get; set; } // Dùng cho Create/Edit

        [TempData]
        public string ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToPage("/Login");
            }
            CategoryList = _categoryService.GetCategories(SearchQuery);
            return Page();
        }

        public IActionResult OnPostDelete(short id)
        {
            try
            {
                _categoryService.DeleteCategory(id);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return RedirectToPage("Index", new { SearchQuery = this.SearchQuery });
        }

        // --- SỬA LỖI MODEL BINDING ---

        // 3. Trả về Partial View cho popup Create
        public IActionResult OnGetCreatePartial()
        {
            // Khởi tạo Category trên PageModel
            Category = new Category { IsActive = true }; // Đặt giá trị mặc định nếu muốn

            // Trả về TOÀN BỘ PageModel ('this')
            return Partial("_Create", this);
        }

        // 4. Trả về Partial View cho popup Edit
        public IActionResult OnGetEditPartial(short id)
        {
            // Gán Category trên PageModel
            Category = _categoryService.GetCategoryById(id);

            // Trả về TOÀN BỘ PageModel ('this')
            return Partial("_Edit", this);
        }

        // --- HẾT SỬA LỖI ---

        // 5. Xử lý Submit từ popup Create
        public IActionResult OnPostCreate()
        {
            if (!ModelState.IsValid)
            {
                // Trả về PageModel ('this') với lỗi validation
                return Partial("_Create", this);
            }
            _categoryService.AddCategory(Category);
            return new JsonResult(new { success = true });
        }

        // 6. Xử lý Submit từ popup Edit
        public IActionResult OnPostEdit()
        {
            if (!ModelState.IsValid)
            {
                // Trả về PageModel ('this') với lỗi validation
                return Partial("_Edit", this);
            }
            _categoryService.UpdateCategory(Category);
            return new JsonResult(new { success = true });
        }
    }
}