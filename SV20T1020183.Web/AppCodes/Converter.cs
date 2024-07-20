using System.Globalization;

namespace SV20T1020183.Web
{
    public static class Converter
    {
        /// <summary>
        /// Chuyển 1 chuổi s sang kiểu DateTime (nếu chuyển không thành công
        /// thì trả về giá trị null)
        /// </summary>
        /// <param name="s"></param>
        /// <param name="formats"></param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string s, string formats = "d/M/yyyy;d-M-yyyy;d.M.yyyy")
        {
            try
            {
                return DateTime.ParseExact(s, formats.Split(';'), CultureInfo.InvariantCulture);

            }
            catch
            {
                return null;
            }
        }
    }
}
