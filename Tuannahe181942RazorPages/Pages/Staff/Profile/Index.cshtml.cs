using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace Tuannahe181942RazorPages.Pages.Staff.Profile
{
    public class IndexModel : PageModel
    {
        private readonly IAccountService _accountService;
        public IndexModel(IAccountService accountService) { _accountService = accountService; }

        [BindProperty]
        public SystemAccount Account { get; set; }

        public string SuccessMessage { get; set; }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("Role") != "Staff")
            {
                return RedirectToPage("/Login");
            }

            var email = HttpContext.Session.GetString("Email");
            Account = _accountService.GetAccountByEmail(email);
            Account.AccountPassword = ""; // Không hiển thị password
            return Page();
        }

        public IActionResult OnPost()
        {
            if (HttpContext.Session.GetString("Role") != "Staff")
            {
                return RedirectToPage("/Login");
            }

            // Lấy lại account gốc từ DB để tránh mất dữ liệu
            var accountFromDb = _accountService.GetAccountById(Account.AccountId);

            // Cập nhật tên
            accountFromDb.AccountName = Account.AccountName;

            // Nếu user nhập password mới thì mới update
            if (!string.IsNullOrEmpty(Account.AccountPassword))
            {
                accountFromDb.AccountPassword = Account.AccountPassword;
            }

            _accountService.UpdateAccount(accountFromDb);

            SuccessMessage = "Your profile has been updated successfully!";
            Account = accountFromDb;
            Account.AccountPassword = "";
            return Page();
        }
    }
}
