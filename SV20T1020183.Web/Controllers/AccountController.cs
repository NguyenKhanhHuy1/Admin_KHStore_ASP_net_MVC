using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020183.BusinessLayers;

namespace SV20T1020183.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username = "", string password = "")
        {
            ViewBag.Username = username;
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("Error", "Nhập tên và mật khẩu!");
                return View();
            }
            var userAccount = UserAccountService.Authorize(username, password);
            if (userAccount == null)
            {
                ModelState.AddModelError("Error", "Đăng nhập thất bại");
                return View();
            }
            //Dăng nhập thành công, tạo dữ liệu lưu thông tin đăng nhập
            var userData = new WebUserData()
            {
                UserId = userAccount.UserID,
                UserName = userAccount.UserName,
                DisplayName = userAccount.FullName,
                Email = userAccount.Email,
                Photo = userAccount.Photo,
                ClientIP = HttpContext.Connection.RemoteIpAddress?.ToString(),
                SessionId = HttpContext.Session.Id,
                AdditionalData = "",
                Roles = userAccount.RoleNames.Split(",").ToList(),
            };
            //Thiết lập phiên đăng nhập cho tài khoản
            await HttpContext.SignInAsync(userData.CreatePrincipal());
            return RedirectToAction("Index", "Home");

        }


        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult ChangePassword(int userid)
        {
            ViewBag.userID = userid;
            return View();
        }
        [HttpPost]
        public IActionResult ChangePassword(string userName, string oldPassword, string newPassword, string newPassword1)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword)
                    || string.IsNullOrWhiteSpace(newPassword1))
                {
                    ModelState.AddModelError("Error", "Vui lòng điền đầy đủ thông tin!");
                    return View("ChangePassword");
                }

                if (!ModelState.IsValid)
                {
                    return View("ChangePassword");
                }
                if (newPassword != newPassword1)
                {
                    ModelState.AddModelError("Error", "Xác nhận mật khẩu mới không khớp!");
                    return View("ChangePassword");
                }

                bool result = UserAccountService.ChangePassword(userName, oldPassword, newPassword);
                if (!result)
                {
                    ModelState.AddModelError("Error", "Mật khẩu cũ không đúng!");
                    return View("ChangePassword");
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {

                return View("Error");
            }

            
        }
        public IActionResult AccessDenined()
        {
            return View();
        }

    }
}
