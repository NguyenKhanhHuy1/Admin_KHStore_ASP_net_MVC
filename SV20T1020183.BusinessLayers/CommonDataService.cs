using SV20T1020183.BusinessLayers;
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
    /// <summary>
    /// cung cấp các chức năng xử lý dữ liệu chung
    /// (Tỉnh/thành, khách hàng, nhà cung cấp, loại hàng, người giao hàng, nhân viên)
    /// </summary>
    public static class CommonDataService
    {
        private static readonly ICommonDAL<Province> provinceDB;
        private static readonly ICommonDAL<Customer> customerDB;
        private static readonly ICommonDAL<Category> categoryDB;
        private static readonly ICommonDAL<Employee> employeeDB;
        private static readonly ICommonDAL<Shipper> shipperDB;
        private static readonly ICommonDAL<Supplier> supplierDB;
        /// <summary>
        /// Ctror( câu hỏi: static contrutor hoạt động như thế nào?)
        /// </summary>
        static CommonDataService()
        {
            string connectionString = Configuration.ConnectionString;

            provinceDB = new ProvinceDAL(connectionString);
            customerDB = new CustomerDAL(connectionString);
            categoryDB = new CategoryDAL(connectionString);
            employeeDB = new EmployeeDAL(connectionString);
            shipperDB = new ShipperDAL(connectionString);
            supplierDB = new SupplierDAL(connectionString);
        }
        /// <summary>
        /// lấy danh sách các tỉnh thành
        /// </summary>
        /// <returns></returns>
        public static List<Province> ListOfProvinces()
        {
            return provinceDB.List().ToList();
        }
        /// <summary>
        /// Lấy danh sách loại hàng không phân trang
        /// </summary>
        /// <returns></returns>
        public static List<Category> ListCategories() { 
            return categoryDB.List().ToList();
        }
        /// <summary>
        /// lấy danh sách nhà cung cấp không phân trang
        /// </summary>
        /// <returns></returns>
        public static List<Supplier> ListSuppliers()
        {
            return supplierDB.List().ToList();
        }
        //Shipper
        public static List<Shipper> ListShippers()
        {
            return shipperDB.List().ToList();
        }

        //khách hàng

        /// <summary>
        /// Tìm kiếm và lấy danh sách khách hàng
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Customer> ListOfCustomers(out int rowCount, int page = 1, int pageSize = 0, string searchValue= "")
        {
            rowCount = customerDB.Count(searchValue);
            return customerDB.List(page, pageSize, searchValue).ToList();
        }
        /// <summary>
        /// lấy về thông tin 1 khách hàng theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Customer? GetCustomer(int id)
        {
            return customerDB.Get(id);
        }
        /// <summary>
        /// bổ sung khách hàng mới
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static int AddCustomer(Customer customer)
        {
            return customerDB.Add(customer);
        }
        /// <summary>
        /// cập nhật thông tin khách hàng
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static bool UpdateCustomer(Customer customer)
        {
            return customerDB.Update(customer);
        }
        /// <summary>
        /// xóa khách hàng có mã là id nếu kh có dữ liệu liên quan
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteCustomer(int id)
        {
            if(customerDB.IsUsed(id))
                return false;
            return customerDB.Delete(id);
        }
        /// <summary>
        /// kiểm tra khách hàng có mã là id, có dữ liệu liên quan hay không?
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsUsedCustomer(int id)
        {
            return customerDB.IsUsed(id);
        }

        //Loại hàng

        /// <summary>
        /// Tìm kiếm và lấy danh sách loại hàng 
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Category> ListOfCategories(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount= categoryDB.Count(searchValue);
            return categoryDB.List(page, pageSize,searchValue).ToList();
        }
        /// <summary>
        /// Lấy về thông tin của 1 loại hàng theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Category? GetCategory(int id)
        {
            return categoryDB.Get(id);
        }
        /// <summary>
        /// Bổ sung loại hàng mới
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddCategory(Category data)
        {
            return categoryDB.Add(data);
        }
        /// <summary>
        /// Cập nhật thông tin loại hàng theo id
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateCategory(Category data)
        {
            return categoryDB.Update(data);
        }
        /// <summary>
        /// Xóa loại hàng theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteCategory(int id)
        {
            return categoryDB.Delete(id);
        }
        /// <summary>
        /// Kiểm tra xem loại hàng này có được sử dụng hay không?
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsUsedCategory(int id)
        {
            return categoryDB.IsUsed(id);
        }

        //nhân viên
        
        /// <summary>
        /// Tìm kiếm và lấy về danh sách nhân viên
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Employee> ListOfEmployees(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = employeeDB.Count(searchValue);
            return employeeDB.List(page, pageSize, searchValue).ToList();
        }
        /// <summary>
        /// Lấy thông tin 1 nhân viên bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Employee? GetEmployee(int id)
        {
            return employeeDB.Get(id);
        }
        /// <summary>
        /// Thêm nhân viên mới
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddEmployee(Employee data)
        {
            return employeeDB.Add(data);
        }
        /// <summary>
        /// Cập nhật thông tin nhân viên theo ID
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateEmployee(Employee data)
        {
            return employeeDB.Update(data);
        }
        /// <summary>
        /// Xóa nhân viên theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteEmployee(int id)
        {
            if(employeeDB.IsUsed(id))
                return false;
            return employeeDB.Delete(id);
        }
        /// <summary>
        ///  Kiểm tra nhân viên có id trên có dữ liệu liên quan hay không?
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsUsedEmployee(int id)
        {
            return employeeDB.IsUsed(id);
        }

        //Người giao hàng
        /// <summary>
        /// Tìm kiếm và lấy về danh sách người giao hàng
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Shipper> ListOfShippers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = shipperDB.Count();
            return shipperDB.List(page, pageSize,searchValue).ToList();
        }
        /// <summary>
        /// Lấy thông tin 1 người giao hàng theo mã id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Shipper? GetShipper(int id)
        {
            return shipperDB.Get(id);
        }
        /// <summary>
        /// Thêm người giao hàng mới
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddShipper(Shipper data)
        {
            return shipperDB.Add(data);
        }
        /// <summary>
        /// Cập nhật thông tin người giao hàng 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateShipper(Shipper data)
        {
            return shipperDB.Update(data);
        }
        /// <summary>
        /// Xóa người giao hàng có mã ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteShipper(int id)
        {
            return shipperDB.Delete(id);
        }
        /// <summary>
        /// Kiểm tra người giao hàng có mã ID có dữ liệu liên quan hay không?
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsUsedShipper(int id)
        {
            return shipperDB.IsUsed(id);
        }

        //Nhà Cung Cấp
        /// <summary>
        /// Tìm kiếm và lấy về danh sách các nhà cung cấp
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Supplier> ListOfSupplier(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = supplierDB.Count();
            return supplierDB.List(page, pageSize, searchValue).ToList();
        }
        /// <summary>
        /// Lấy thông tin nhà cung cấp theo mã ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Supplier? GetSupplier(int id)
        {
            return supplierDB.Get(id);
        }
        /// <summary>
        /// Thêm nhà cung cấp mới
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddSupplier(Supplier data)
        {
            return supplierDB.Add(data);
        }
        /// <summary>
        /// Cập nhật thông tin nhà cung cấp
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateSupplier(Supplier data)
        {
            return supplierDB.Update(data);
        }
        /// <summary>
        /// Xóa nhà cung cấp có mã ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteSupplier(int id)
        {
            return supplierDB.Delete(id);
        }
        /// <summary>
        /// Kiểm tra xem nhà cung cấp có mã ID có dữ liệu liên quan hay không?
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsUsedSupplier(int id)
        {
            return supplierDB.IsUsed(id);
        }

    }
}