using Microsoft.AspNetCore.Authentication.Cookies;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security.Claims;

namespace SV20T1020183.Web
{
    /// <summary>
    /// Thông tin về nhóm/quyền
    /// </summary>
    public class WebUserRole
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="name">Tên/ký hiệu nhóm/quyền</param>
        /// <param name="description">Mô tả</param>
        public WebUserRole(string name, string description)
        {
            Name = name;
            Description = description;
        }
        /// <summary>
        /// Tên/Ký hiệu quyền
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }
    }

}
