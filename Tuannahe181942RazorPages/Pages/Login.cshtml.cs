using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Tuannahe181942RazorPages.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IAccountService _accountService; // Dùng Service của bạn
        private readonly IConfiguration _configuration;

        public LoginModel(IAccountService accountService, IConfiguration configuration)
        {
            _accountService = accountService;
            _configuration = configuration;
        }

        [BindProperty, Required, EmailAddress]
        public string Email { get; set; }

        [BindProperty, Required, DataType(DataType.Password)]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            HttpContext.Session.Clear(); // Xóa session khi vào trang Login
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            var adminEmail = _configuration["AdminAccount:Email"];
            var adminPassword = _configuration["AdminAccount:Password"];

            // Check Admin
            if (Email == adminEmail && Password == adminPassword)
            {
                HttpContext.Session.SetString("Role", "Admin");
                HttpContext.Session.SetString("Email", adminEmail);
                return RedirectToPage("/Admin/Accounts/Index"); // Trang chủ Admin
            }

            // Check Staff
            var staff = _accountService.GetAccountByEmail(Email);
            if (staff != null && staff.AccountPassword == Password)
            {
                HttpContext.Session.SetString("Role", "Staff");
                HttpContext.Session.SetString("Email", staff.AccountEmail);
                HttpContext.Session.SetString("AccountId", staff.AccountId.ToString());
                return RedirectToPage("/Staff/News/Index"); // Trang chủ Staff
            }

            ErrorMessage = "Invalid email or password.";
            return Page();
        }
    }
}
