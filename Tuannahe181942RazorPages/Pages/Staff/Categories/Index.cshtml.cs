using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace Tuannahe181942RazorPages.Pages.Staff.Categories
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public IndexModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public List<Category> CategoryList { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; }

        [BindProperty]
        public Category Category { get; set; } // Dùng cho Create/Edit

        [TempData]
        public string ErrorMessage { get; set; }

        // 1. Check quyền và Load danh sách (Search)
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("Role") != "Staff")
            {
                return RedirectToPage("/Login");
            }
            CategoryList = _categoryService.GetCategories(SearchQuery);
            return Page();
        }

        // 2. Xử lý Delete (với Confirmation)
        public IActionResult OnPostDelete(short id)
        {
            try
            {
                // Hàm DeleteCategory trong DAO/Repo của bạn đã có logic kiểm tra ràng buộc
                _categoryService.DeleteCategory(id);
            }
            catch (Exception ex)
            {
                // Bắt lỗi "Category is in use" từ DAO
                ErrorMessage = ex.Message;
            }
            return RedirectToPage("Index", new { SearchQuery = this.SearchQuery });
        }

        // --- YÊU CẦU: POPUP DIALOG HANDLERS ---

        // 3. Trả về Partial View cho popup Create
        public IActionResult OnGetCreatePartial()
        {
            // Chỉ trả về form rỗng
            return Partial("_Create", new Category());
        }

        // 4. Trả về Partial View cho popup Edit
        public IActionResult OnGetEditPartial(short id)
        {
            Category = _categoryService.GetCategoryById(id);
            // Trả về form chứa thông tin category
            return Partial("_Edit", Category);
        }

        // 5. Xử lý Submit từ popup Create
        public IActionResult OnPostCreate()
        {
            if (!ModelState.IsValid)
            {
                return Partial("_Create", Category); // Trả về form với lỗi validation
            }
            _categoryService.AddCategory(Category);
            return new JsonResult(new { success = true }); // Báo cho AJAX biết là thành công
        }

        // 6. Xử lý Submit từ popup Edit
        public IActionResult OnPostEdit()
        {
            if (!ModelState.IsValid)
            {
                return Partial("_Edit", Category); // Trả về form với lỗi validation
            }
            _categoryService.UpdateCategory(Category);
            return new JsonResult(new { success = true }); // Báo cho AJAX biết là thành công
        }
    }
}
