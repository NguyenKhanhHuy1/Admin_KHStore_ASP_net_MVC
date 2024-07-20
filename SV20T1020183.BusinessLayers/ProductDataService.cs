using SV20T1020183.DataLayers;
using SV20T1020183.DataLayers.SQLServer;
using SV20T1020183.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020183.BusinessLayers
{
    public static class ProductDataService
    {
        private static readonly IProductDAL productDB;
        /// <summary>
        /// Ctor
        /// </summary>
        static ProductDataService()
        {
            productDB = new ProductDAL(Configuration.ConnectionString);
        }
        /// <summary>
        /// Tìm kiếm và trẩ về danh sách sản phẩm không phân trang
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Product> ListProducts(string searchValue = "")
        {
            return productDB.List().ToList();
        }
        /// <summary>
        /// Tìm kiếm và trả về danh sách sản phẩn có phân trang
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="categoryId"></param>
        /// <param name="supplierId"></param>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <returns></returns>
        public static List<Product> ListProducts(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "", int categoryId = 0, int supplierId = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            rowCount = productDB.Count(searchValue,categoryId, supplierId, minPrice, maxPrice);
            return productDB.List(page, pageSize, searchValue, categoryId, supplierId, minPrice, maxPrice).ToList();
        }
        /// <summary>
        /// lấy về thông tin sản phaamr theo ID
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public static Product? GetProduct(int productID)
        {
            return productDB.get(productID);
        }
        /// <summary>
        /// Bổ sung sản phẩm
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddProduct(Product data)
        {
            return productDB.Add(data);
        }
        /// <summary>
        /// Cập nhật thông tin sẩn phẩm
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateProduct(Product data)
        {
            return productDB.Update(data);
        }
        /// <summary>
        /// Xóa sản phẩm
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public static bool DeleteProduct(int productID)
        {
            if(productDB.InUsed(productID))
                return false;
            return productDB.Delete(productID);
        }
        /// <summary>
        /// Kiểm tra xem sản phẩm có dữ liệu liên quan
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public static bool InUsedProduct(int productID)
        {
            return productDB.InUsed(productID);
        }
        /// <summary>
        /// Lấy về danh sách ảnh của sản phẩm
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public static List<ProductPhoto>? ListPhotos(int productID)
        {
            return productDB.ListPhotos(productID).ToList();
        }
        /// <summary>
        /// Lấy thông tin ảnh của sản phẩm
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public static ProductPhoto? GetPhoto(int productID)
        {
            return productDB.GetPhoto(productID);
        }
        /// <summary>
        /// Thêm ảnh vào sản phẩm
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static long AddPhoto(ProductPhoto data)
        {
            return productDB.AddPhoto(data);
        }
        /// <summary>
        /// Cập nhật ảnh của sản phẩm
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdatePhoto(ProductPhoto data)
        {
            return productDB.UpdatePhoto(data);
        }
        /// <summary>
        /// Xóa ảnh của sẩn phẩm
        /// </summary>
        /// <param name="photoID"></param>
        /// <returns></returns>
        public static bool DeletePhoto(long photoID)
        {
            return productDB.DeletePhoto(photoID);
        }
        /// <summary>
        /// Lấy về danh sách thuộc tính của sản phẩm
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public static List<ProductAttribute>? ListAttributes(int productID)
        {
            return productDB.ListAttributes(productID).ToList();
        }
        /// <summary>
        /// Lấy về thuộc tính sẩn phẩm theo ID
        /// </summary>
        /// <param name="AttributeID"></param>
        /// <returns></returns>
        public static ProductAttribute? GetAttribute(int AttributeID)
        {
            return productDB.GetAttribute(AttributeID);
        }
        /// <summary>
        /// Bổ sung thuộc tính của sản phẩm
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static long AddAttribute(ProductAttribute data)
        {
            return productDB.AddAttribute(data);
        }
        /// <summary>
        /// Cập nhật thuộc tính
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateAttribute(ProductAttribute data)
        {
            return productDB.UpdateAttribute(data);
        }
        /// <summary>
        /// Xóa thuộc tính
        /// </summary>
        /// <param name="attributeID"></param>
        /// <returns></returns>
        public static bool DeleteAttribute(long  attributeID)
        {
            return productDB.DeleteAttribute(attributeID);
        }
    }
}
