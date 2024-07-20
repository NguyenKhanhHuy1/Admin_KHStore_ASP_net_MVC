using Dapper;
using SV20T1020183.DomainModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SV20T1020183.DataLayers.SQLServer
{
    public class ProductDAL : _BaseDAL, IProductDAL
    {
        /// <summary>
        /// ctr
        /// </summary>
        /// <param name="connectionString"></param>
        public ProductDAL(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// Bổ sung sẩn phẩm
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Add(Product data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"    insert into Products(ProductName,ProductDescription,SupplierID, CategoryID, Unit, Price, Photo, IsSelling)
                                values(@ProductName,@ProductDescription,@SupplierID, @CategoryID, @Unit, @Price, @Photo, @IsSelling);
                                select @@identity;
                            ";
                var parameters = new
                {
                    ProductName = data.ProductName ?? "",
                    ProductDescription = data.ProductDescription ?? "",
                    SupplierID = data.SupplierID,
                    CategoryID = data.CategoryID,
                    Unit = data.Unit ?? "",
                    Price = data.Price,
                    Photo = data.Photo ?? "",
                    IsSelling = data.IsSelling,

                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return id;
        }
        /// <summary>
        /// bổ sung thuộc tính
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public long AddAttribute(ProductAttribute data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"    insert into ProductAttributes(ProductID,AttributeName, AttributeValue, DisplayOrder)
                                values(@ProductID,@AttributeName, @AttributeValue, @DisplayOrder);
                                select @@identity;
                            ";
                var parameters = new
                {
                    ProductID = data.ProductID,
                    AttributeName = data.AttributeName ?? "",
                    AttributeValue = data.AttributeValue ?? "0",
                    DisplayOrder = data.DisplayOrder 

                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return id;
        }
        /// <summary>
        /// bổ sung ảnh
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public long AddPhoto(ProductPhoto data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"    insert into ProductPhotos(ProductID,Photo, Description, DisplayOrder, IsHidden)
                                values(@ProductID, @Photo, @Description, @DisplayOrder, @IsHidden);
                                select @@identity;
                            ";
                var parameters = new
                {
                    ProductID = data.ProductID,
                    Photo = data.Photo,
                    Description = data.Description ?? "",
                    DisplayOrder = data.DisplayOrder,
                    IsHidden = data.IsHidden

                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return id;
        }
        /// <summary>
        /// Đếm 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <param name="categoryID"></param>
        /// <param name="supplierID"></param>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <returns></returns>
        public int Count(string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            int count = 0;

            if (!string.IsNullOrEmpty(searchValue))           
                searchValue = "%" + searchValue + "%";           
            using (var connection = OpenConnection())
            {
                var sql = @"
                                select count(*) from  Products
                                where   (@SearchValue = N'' or ProductName like @SearchValue)
                                    and (@CategoryID = 0 or CategoryID = @CategoryID)
                                    and (@SupplierID = 0 or SupplierId = @SupplierID)
                                    and (Price >= @MinPrice)
                                    and (@MaxPrice <= 0 or Price <= @MaxPrice)
                            ";
                var parameters = new
                {
                    searchValue = searchValue ?? "",
                    CategoryID = categoryID ,
                    SupplierID = supplierID,
                    MinPrice = minPrice,
                    MaxPrice = maxPrice
                };
                count = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }

            return count;
        }
        /// <summary>
        /// Xóa sẩn phảm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"delete from Products where ProductID = @ProductID";
                var parameters = new
                {
                    ProductID = id,
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }
        /// <summary>
        /// Xóa thuộc tính
        /// </summary>
        /// <param name="attributeID"></param>
        /// <returns></returns>
        public bool DeleteAttribute(long attributeID)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"delete from ProductAttributes where AttributeID = @attributeID";
                var parameters = new
                {
                    attributeID = attributeID,
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }
        /// <summary>
        /// Xóa ảnh
        /// </summary>
        /// <param name="photoID"></param>
        /// <returns></returns>
        public bool DeletePhoto(long photoID)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"delete from ProductPhotos where PhotoID = @PhotoID";
                var parameters = new
                {
                    PhotoID = photoID,
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }
        /// <summary>
        /// Lấy về snr phẩm theo id
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public Product? get(int productID)
        {
            Product? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select * from Products where ProductID = @ProductID";
                var parameters = new
                {
                    ProductID = productID,
                };
                data = connection.QueryFirstOrDefault<Product>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return data;
        }
        /// <summary>
        /// Lấy về thuộc tính theo id
        /// </summary>
        /// <param name="attributeID"></param>
        /// <returns></returns>
        public ProductAttribute? GetAttribute(long attributeID)
        {
            ProductAttribute? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select * from ProductAttributes where AttributeID = @AttributeID";
                var parameters = new
                {
                    AttributeID = attributeID,
                };
                data = connection.QueryFirstOrDefault<ProductAttribute>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return data;
        }
        /// <summary>
        /// Lấy về ảnh theo id
        /// </summary>
        /// <param name="photoID"></param>
        /// <returns></returns>
        public ProductPhoto? GetPhoto(long photoID)
        {
            ProductPhoto? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select * from ProductPhotos where PhotoID = @PhotoID";
                var parameters = new
                {
                    PhotoID = photoID 
                };
                data = connection.QueryFirstOrDefault<ProductPhoto>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return data;
        }
        /// <summary>
        /// Kiểm tra xe sản phẩm có dữ liệu liên quan hay không?
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public bool InUsed(int productID)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"IF EXISTS (
                                    SELECT 1
                                    FROM OrderDetails
                                    WHERE ProductID = @ProductID
                                    UNION
                                    SELECT 1
                                    FROM ProductAttributes
                                    WHERE ProductID = @ProductID
                                    UNION
                                    SELECT 1
                                    FROM ProductPhotos
                                    WHERE ProductID = @ProductID
                                )
                                    SELECT 1
                                ELSE 
                                    SELECT 0";
                var parameters = new
                {
                    ProductID = productID
                };
                result = connection.ExecuteScalar<bool>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return result;
        }
        /// <summary>
        /// Lấy về danh sách sản phẩm
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="categoryID"></param>
        /// <param name="supplierID"></param>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <returns></returns>
        public IList<Product> List(int page = 1, int pageSize = 0, string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            List<Product> data = new List<Product>();
            if(!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            using(var connection = OpenConnection())
            {
                var sql = @"select *
                            from
                            (
                                select  *,
                                        row_number() over(order by ProductName) as RowNumber
                                from    Products
                                where   (@SearchValue = N'' or ProductName like @SearchValue)
                                    and (@CategoryID = 0 or CategoryID = @CategoryID)
                                    and (@SupplierID = 0 or SupplierId = @SupplierID)
                                    and (Price >= @MinPrice)
                                    and (@MaxPrice <= 0 or Price <= @MaxPrice)
                            )
                            as t
                            where   (@PageSize = 0)
                                or (RowNumber between (@Page - 1)*@PageSize + 1 and @Page * @PageSize)
                            order by RowNumber
                            ";
                var parameters = new
                {
                    page = page,
                    pageSize = pageSize,
                    searchValue = searchValue ?? "",
                    categoryID = categoryID,
                    supplierID = supplierID,
                    minPrice = minPrice,
                    maxPrice = maxPrice
                };
                data = connection.Query<Product>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text).ToList();
                connection.Close();
            }
            return data;
        }
        /// <summary>
        /// Lấy về thuộc tính của sản phẩm theo ID
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public IList<ProductAttribute> ListAttributes(int productID)
        {
            List<ProductAttribute> data = new List<ProductAttribute>();
            using(var connection = OpenConnection())
            {
                var sql = @"select * from ProductAttributes 
                            where ProductID = @productID";
                var parameters = new
                {
                    productID = productID,
                };
                data = connection.Query<ProductAttribute>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text).ToList();
                connection.Close();
            };
            return data;
        }
        /// <summary>
        /// Lấy về ảnh của sản phẩm theo ID
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public IList<ProductPhoto> ListPhotos(int productID)
        {
            List<ProductPhoto> data = new List<ProductPhoto>();
            using (var connection = OpenConnection())
            {
                var sql = @"select * from ProductPhotos  
                            where ProductID = @productID";
                var parameters = new
                {
                    productID = productID,
                };
                data = connection.Query<ProductPhoto>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text).ToList();
                connection.Close();
            };
            return data;
        }
        /// <summary>
        /// Cập nhật thông tin sản phẩm
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Update(Product data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @" update Products 
                                set ProductName = @ProductName,
	                                ProductDescription = @Description,
	                                SupplierID = @SupplierID,
	                                CategoryID = @CategoryID,
	                                Unit = @Unit,
	                                Price = @Price,
	                                Photo = @Photo,
	                                IsSelling= @IsSelling
                                    where ProductID = @ProductID
                            ";
                var parameters = new
                {
                    ProductID = data.ProductID,
                    ProductName = data.ProductName ?? "",
                    Description = data.ProductDescription ?? "",
                    SupplierID = data.SupplierID,
                    CategoryID = data.CategoryID,
                    Unit = data.Unit ?? "",
                    Price= data.Price ,
                    Photo = data.Photo ?? "",
                    IsSelling = data.IsSelling,


                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }
        /// <summary>
        /// Cập nhật thuộc tính của sản phẩm
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool UpdateAttribute(ProductAttribute data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @" update ProductAttributes
                            set ProductID =@ProductID,
	                            AttributeName = @AttributeName,
	                            AttributeValue= @AttributeValue,
	                            DisplayOrder = @DisplayOrder
                            where AttributeID = @AttributeID
                            ";
                var parameters = new
                {
                    ProductID = data.ProductID,
                    AttributeName = data.AttributeName ?? "",
                    AttributeValue = data.AttributeValue ?? "",
                    DisplayOrder = data.DisplayOrder,
                    AttributeID = data.AttributeID,


                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }
        /// <summary>
        /// Cập nhật ảnh của sản phảm
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool UpdatePhoto(ProductPhoto data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @" update ProductPhotos
                            set ProductID =@ProductID,
	                            Photo = @Photo,
	                            Description= @Description,
	                            DisplayOrder = @DisplayOrder,
	                            IsHidden =@IsHidden
                                where PhotoID = @PhotoID
                        ";
                var parameters = new
                {
                    ProductID = data.ProductID ,
                    Photo= data.Photo ?? "",
                    Description = data.Description ?? "",
                    DisplayOrder = data.DisplayOrder,
                    IsHidden = data.IsHidden,
                    PhotoID = data.PhotoId

                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }
    }
}
