using Newtonsoft.Json;

namespace SV20T1020183.Web
{
    public class ApplicationContext
    {
        private static IHttpContextAccessor? _httpContextAccessor;
        private static IWebHostEnvironment? _hostEnvironment;

        public static void Configure(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostEnvironment)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException();
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException();
        }
        /// <summary>
        /// HttpContext
        /// </summary>
        public static HttpContext? HttpContext => _httpContextAccessor?.HttpContext;
        /// <summary>
        /// HostEnviroment
        /// </summary>
        public static IWebHostEnvironment? HostEnviroment => _hostEnvironment;
        /// <summary>
        /// Get the absolute path to the directory that contains the web-servable application content files.
        /// </summary>
        public static string WebRootPath => _hostEnvironment?.WebRootPath ?? string.Empty;
        /// <summary>
        /// Gets the absolute path to the directory that contains the application content files.
        /// </summary>
        public static string ContentRootPath => _hostEnvironment?.ContentRootPath ?? string.Empty;


        /// <summary>
        /// Ghi dữ liệu vào session
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetSessionData(string key, object value)
        {
            try
            {

                string sValue = JsonConvert.SerializeObject(value);
                if (!string.IsNullOrEmpty(sValue))
                {
                    _httpContextAccessor?.HttpContext?.Session.SetString(key, sValue);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Đọc dữ liệu từ session
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T? GetSessionData<T>(string key) where T : class
        {
            try
            {
                string sValue = _httpContextAccessor?.HttpContext?.Session.GetString(key) ?? "";
                if (!string.IsNullOrEmpty(sValue))
                {
                    return JsonConvert.DeserializeObject<T>(sValue);
                }
            }
            catch
            {
            }
            return null;
        }
    }
}
