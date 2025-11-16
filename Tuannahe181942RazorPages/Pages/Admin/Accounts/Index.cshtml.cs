using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace Tuannahe181942RazorPages.Pages.Admin.Accounts
{
    public class IndexModel : PageModel
    {
        private readonly IAccountService _accountService;

        public IndexModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public List<SystemAccount> AccountList { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; }

        [BindProperty]
        public SystemAccount Account { get; set; } // Dùng cho Create/Edit

        // 1. Check quyền và Load danh sách
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToPage("/Login");
            }
            AccountList = _accountService.GetAccounts(SearchQuery);
            return Page();
        }

        // 2. Xử lý Delete (với Confirmation)
        public IActionResult OnPostDelete(short id)
        {
            _accountService.DeleteAccount(id);
            return RedirectToPage("Index");
        }

        // --- YÊU CẦU: POPUP DIALOG HANDLERS ---

        // 3. Trả về Partial View cho popup Create
        public IActionResult OnGetCreatePartial()
        {
            return Partial("_Create", new SystemAccount());
        }

        // 4. Trả về Partial View cho popup Edit
        public IActionResult OnGetEditPartial(short id)
        {
            Account = _accountService.GetAccountById(id);
            return Partial("_Edit", Account);
        }

        // 5. Xử lý Submit từ popup Create
        public IActionResult OnPostCreate()
        {
            if (!ModelState.IsValid)
            {
                return Partial("_Create", Account); // Trả về form với lỗi
            }
            _accountService.AddAccount(Account);
            return new JsonResult(new { success = true }); // Báo cho AJAX biết là thành công
        }

        // 6. Xử lý Submit từ popup Edit
        public IActionResult OnPostEdit()
        {
            if (!ModelState.IsValid)
            {
                return Partial("_Edit", Account); // Trả về form với lỗi
            }
            _accountService.UpdateAccount(Account);
            return new JsonResult(new { success = true }); // Báo cho AJAX biết là thành công
        }
    }
}
