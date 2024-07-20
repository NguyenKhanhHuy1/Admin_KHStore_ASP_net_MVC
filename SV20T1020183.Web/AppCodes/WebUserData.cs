using Microsoft.AspNetCore.Authentication.Cookies;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security.Claims;


namespace SV20T1020183.Web
{
    /// <summary>
    /// Thông tin tài khoản người dùng được lưu trong phiên đăng nhập
    /// </summary>
    public class WebUserData
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? DisplayName { get; set; }
        public string? Email { get; set; }
        public string? Photo { get; set; }
        public string? ClientIP { get; set; }
        public string? SessionId { get; set; }
        public string? AdditionalData { get; set; }
        public List<string>? Roles { get; set; }

        /// <summary>
        /// Thông tin người dùng dưới dạng danh sách các Claim
        /// </summary>
        /// <returns></returns>
        private List<Claim> Claims
        {
            get
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(nameof(UserId), UserId ?? ""),
                    new Claim(nameof(UserName), UserName ?? ""),
                    new Claim(nameof(DisplayName), DisplayName ?? ""),
                    new Claim(nameof(Email), Email ?? ""),
                    new Claim(nameof(Photo), Photo ?? ""),
                    new Claim(nameof(ClientIP), ClientIP ?? ""),
                    new Claim(nameof(SessionId), SessionId ?? ""),
                    new Claim(nameof(AdditionalData), AdditionalData ?? "")
                };
                if (Roles != null)
                    foreach (var role in Roles)
                        claims.Add(new Claim(ClaimTypes.Role, role));
                return claims;
            }
        }

        /// <summary>
        /// Tạo Pricipal dựa trên thông tin của người dùng
        /// </summary>
        /// <returns></returns>
        public ClaimsPrincipal CreatePrincipal()
        {
            var claimIdentity = new ClaimsIdentity(Claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimPrincipal = new ClaimsPrincipal(claimIdentity);
            return claimPrincipal;
        }
    }

}
