using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SV20T1020183.BusinessLayers;
using SV20T1020183.DomainModels;
using SV20T1020183.Web.Models;
using System;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SV20T1020183.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator},{WebUserRoles.Employee}")]
    public class ProductController : Controller
    {
        private const int PAGE_SIZE = 20;
        private const string PRODUCT_SEARCH = "product_search";
        public IActionResult Index()
        {
            //lấy đầu vào tìm kiếm hiện đang lưu lại trong session
            PaginationSearchInput? input = ApplicationContext.GetSessionData<ProductSearchInput>(PRODUCT_SEARCH);
            //Kiểm tra session chưa có điều kiện thì  tạo điều kiện mới
            if (input == null)
            {
                input = new ProductSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = "",
                    CategoryID = 0,
                    SupplierID = 0
                };
            }

            return View(input);
        }
        public IActionResult Search(ProductSearchInput input)
        {
            int rowCount = 0;
            var data = ProductDataService.ListProducts(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "", input.CategoryID, input.SupplierID);
            var model = new ProductSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };
            //Lưu lại điều kiện tìm kiếm vào trong session
            ApplicationContext.SetSessionData(PRODUCT_SEARCH, input);
            return View(model);
        } 

            public IActionResult Create()
        {
            ViewBag.title = "Bổ sung mặt hàng";
            Product data = new Product()
            {
                ProductID = 0,
                Photo = "noProduct.jpg"

            };
            var model = new Models.ProductEditForm()
            {
                Data = data
            };
            ViewBag.IsEdit = false;
            return View("Edit", model);
        }
        public IActionResult Edit(int id)
        {
            ViewBag.title = "Cập nhật thông tin mặt hàng";

            Product? data = ProductDataService.GetProduct(id);
            var attribute = ProductDataService.ListAttributes(id);
            var productphoto = ProductDataService.ListPhotos(id);
            if (data == null)
                return RedirectToAction("Index");

            if(string.IsNullOrEmpty(data.Photo))
                data.Photo="noProduct.jpg";

            ViewBag.IsEdit = true;

            var model = new Models.ProductEditForm()
            {
                Data = data,
                Attribute = attribute,
                photo = productphoto
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Save(Product data, IFormFile? uploadPhoto)
        {

            if (uploadPhoto != null)
            {
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}"; //tên file sẽ lưu
                string folder = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, @"images\products"); //đường dẫn lưu file
                string filePath = Path.Combine(folder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                data.Photo = fileName;
            }
            //thay vì khai báo từng tham số cho Action để nhận dữ liệu, thì nên sử dụng Model
            //Model (View Model): là một class có các thuộc tính trùng tên với các tham số
            try
            {
                ViewBag.Title = data.ProductID == 0 ? "Bổ sung mặt hàng" : "Cập nhật thông tin mặt hàng";
                ViewBag.IsEdit = data.ProductID == 0 ?  false :  true;

                if (string.IsNullOrWhiteSpace(data.ProductName))
                    ModelState.AddModelError(nameof(data.ProductName), "Tên mặt hàng không được trống");
                if (data.CategoryID <= 0)
                    ModelState.AddModelError(nameof(data.CategoryID), "Vui lòng chọn loại hàng");
                if (data.SupplierID <= 0)
                    ModelState.AddModelError(nameof(data.SupplierID), "Vui lòng chọn nhà cung cấp");
                //Thông  qua thuộc tính IsVaid của ModelState để kiểm tra xem có tồn tại lỗi hay không?

                if (!ModelState.IsValid)
                {

                    var model = new Models.ProductEditForm()
                    {
                        Data = data,
                        Attribute = ProductDataService.ListAttributes(data.ProductID),
                        photo = ProductDataService.ListPhotos(data.ProductID)
                    };
                    return View("Edit", model);
                }

                
                if (data.ProductID == 0)
                {
                    int id = ProductDataService.AddProduct(data);
                    
                }
                else
                {
                    bool result = ProductDataService.UpdateProduct(data);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }


        }
        public IActionResult Delete(int id)
        {
            if (Request.Method == "POST")
            {
                ProductDataService.DeleteProduct(id);
                return RedirectToAction("Index");
            }

            var model = ProductDataService.GetProduct(id);
            if (model == null)
                return RedirectToAction("Index");

            ViewBag.AllowDelete = !ProductDataService.InUsedProduct(id);
            return View(model);
        }
        public IActionResult Photo(int id, string method, int photoID = 0) {
            
            switch(method)
            {
                case "add":
                    ViewBag.title = "Bổ sung ảnh cho mặt hàng";
                    var r = new ProductPhoto
                    {   ProductID= id,
                        PhotoId = 0,
                        Photo= "noProduct.jpg"
                    };
                    return View(r);
                case "edit":
                    ViewBag.title = "Thay đổi ảnh cho mặt hàng";
                    var data = ProductDataService.GetPhoto(photoID);
                    return View(data);
                case "delete":
                    //xóa trực tiếp ảnh
                    ProductDataService.DeletePhoto(photoID);
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
        }
        public IActionResult SavePhoto(ProductPhoto data, IFormFile uploadPhoto)
        {
            ViewBag.Title = data.PhotoId == 0 ? "Bổ sung Ảnh" : "Thay đổi Ảnh cho mặt hàng";
            if (uploadPhoto != null)
            {
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}"; //tên file sẽ lưu
                string folder = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, @"images\products"); //đường dẫn lưu file
                string filePath = Path.Combine(folder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                data.Photo = fileName;
            }
            try
            {
                if (data.PhotoId == 0)
                {
                    long id = ProductDataService.AddPhoto(data);
                }
                else
                {
                    bool result = ProductDataService.UpdatePhoto(data);
                }
                return RedirectToAction("Edit", new { id = data.ProductID });
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public IActionResult Attribute(int id, string method,int attributeID = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.title = "Bổ sung thuộc tính cho mặt hàng";
                    var r = new ProductAttribute
                    {   ProductID =id,
                        AttributeID = 0
                    };
                    return View(r);
                case "edit":
                    ViewBag.title = "Thay đổi thuộc tính cho mặt hàng";
                    var data = ProductDataService.GetAttribute(attributeID);
                    return View(data);
                case "delete":
                    //xóa trực tiếp ảnh 
                    ProductDataService.DeleteAttribute(attributeID);
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
        }
        public IActionResult SaveAttribute(ProductAttribute data)
        {
            ViewBag.Title = data.AttributeID == 0 ? "Bổ sung Thuộc tính" : "Thay đổi thuộc tính cho mặt hàng";
            try
            {

                if (string.IsNullOrWhiteSpace(data.AttributeName))
                    ModelState.AddModelError(nameof(data.AttributeName), "Vui lòng nhập tên thuộc tính");
                if (string.IsNullOrWhiteSpace(data.AttributeValue))
                    ModelState.AddModelError(nameof(data.AttributeValue), "Vui lòng nhập giá trị thuộc tính");
                //Thông  qua thuộc tính IsVaid của ModelState để kiểm tra xem có tồn tại lỗi hay không?
                if (!ModelState.IsValid)
                {

                    return View("Attribute", data);
                }

                if (data.AttributeID == 0)
                {
                    long id = ProductDataService.AddAttribute(data);
                }
                else
                {
                    bool result = ProductDataService.UpdateAttribute(data);
                }
                return RedirectToAction("Edit", new { id = data.ProductID });
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}
