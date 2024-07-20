using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020183.BusinessLayers;
using SV20T1020183.DomainModels;
using SV20T1020183.Web.Models;

namespace SV20T1020183.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator},{WebUserRoles.Employee}")]
    public class CategoryController : Controller
    {
        private const int PAGE_SIZE = 20;
        private const string CATEGORY_SEARCH = "category_search";
        public IActionResult Index()
        {
            //lấy đầu vào tìm kiếm hiện đang lưu lại trong session
            PaginationSearchInput? input = ApplicationContext.GetSessionData<PaginationSearchInput>(CATEGORY_SEARCH);
            //Kiểm tra session chưa có điều kiện thì  tạo điều kiện mới
            if (input == null)
            {
                input = new PaginationSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = ""
                };
            }

            return View(input);
        }

        public IActionResult Search(PaginationSearchInput input)
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfCategories(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            var model = new CategorySearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };
            //Lưu lại điều kiện tìm kiếm vào trong session
            ApplicationContext.SetSessionData(CATEGORY_SEARCH, input);
            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.title = "Bổ sung loại hàng";
            Category model = new Category()
            {
                CategoryID = 0,
                Photo = "noProduct.jpg"
            };
            return View("Edit", model);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.title = "Cập nhật thông tin loại hàng";
            Category? model = CommonDataService.GetCategory(id);
            if (model == null)
                return RedirectToAction("Index");
            if (string.IsNullOrEmpty(model.Photo))
                model.Photo = "noProduct.jpg";
            return View(model);
        }
        public IActionResult Delete(int id) {
            if (Request.Method == "POST")
            {
                CommonDataService.DeleteCategory(id);
                return RedirectToAction("Index");
            }

            var model = CommonDataService.GetCategory(id);
            if (model == null)
                return RedirectToAction("Index");

            ViewBag.AllowDelete = !CommonDataService.IsUsedCategory(id);
            return View(model);
        }
        public IActionResult Save(Category data, IFormFile? uploadPhoto)
        {
            if (uploadPhoto != null)
            {
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}"; //tên file sẽ lưu
                string folder = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, @"images\categories"); //đường dẫn lưu file
                string filePath = Path.Combine(folder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                data.Photo = fileName;
            }

            try
            {
                ViewBag.Title = data.CategoryID == 0 ? "Bổ sung loại hàng" : "Cập nhật thông tin loại hàng";
                if (string.IsNullOrWhiteSpace(data.CategoryName))
                    ModelState.AddModelError(nameof(data.CategoryName), "Vui lòng nhập tên loại hàng");
                if (string.IsNullOrWhiteSpace(data.Description))
                    ModelState.AddModelError(nameof(data.Description), "Vui lòng nhập mô tả loại hàng");
                if (!ModelState.IsValid)
                {
                    return View("Edit", data);
                }

                if (data.CategoryID == 0)
                {
                    int id = CommonDataService.AddCategory(data);
                }
                else
                {
                    bool result = CommonDataService.UpdateCategory(data);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}
