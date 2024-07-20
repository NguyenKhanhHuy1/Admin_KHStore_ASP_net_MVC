using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020183.BusinessLayers;
using SV20T1020183.DomainModels;
using SV20T1020183.Web.Models;
using System.Buffers;

namespace SV20T1020183.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator}")]
    public class EmployeeController : Controller
    {
        private const int PAGE_SIZE = 20;
        private const string EMPLOYEE_SEARCH = "employee_search";
        public IActionResult Index()
        {
            //lấy đầu vào tìm kiếm hiện đang lưu lại trong session
            PaginationSearchInput? input = ApplicationContext.GetSessionData<PaginationSearchInput>(EMPLOYEE_SEARCH);
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
            var data = CommonDataService.ListOfEmployees(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            var model = new EmployeeSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };
            //Lưu lại điều kiện tìm kiếm vào trong session
            ApplicationContext.SetSessionData(EMPLOYEE_SEARCH, input);
            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.title = "Bổ sung nhân viên";
            Employee model = new Employee()
            {
                EmployeeID = 0,
                BirthDate = new DateTime(1990,01,01),
                Photo = "nophoto.png"
            };
            return View("Edit", model);
        }
        public IActionResult Edit(int id)
        {

            ViewBag.title = "Cập nhật thông tin nhân viên";
            Employee? model = CommonDataService.GetEmployee(id);
            if (model == null)
                return RedirectToAction("Index");

            if (string.IsNullOrEmpty(model.Photo))
                model.Photo = "nophoto.png";

            return View(model);
        }
        public IActionResult Save(Employee data,string birthDateInput, IFormFile? uploadPhoto)
        {
            //xử lí ngày sinh
            DateTime? birthDate = birthDateInput.ToDateTime();
            if(birthDate.HasValue)
                data.BirthDate = birthDate.Value;

            //xử lí ảnh upload (Nếu có ảnh upload thì lưu ảnh và gán lại tên file ảnh mới cho employee)
            if(uploadPhoto != null)
            {
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}"; //tên file sẽ lưu
                string folder = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, @"images\employees"); //đường dẫn lưu file
                string filePath = Path.Combine(folder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                data.Photo = fileName;
            }

            try
            {
                ViewBag.Title = data.EmployeeID == 0 ? "Bổ sung nhân viên" : "Cập nhật thông tin nhân viên";

                //Kiểm soát đầu vào và đưa các thông báo lỗi vào trong ModelState ( nếu có)
                if (string.IsNullOrWhiteSpace(data.FullName))
                    ModelState.AddModelError(nameof(data.FullName), "Tên không được để trống");
                if (string.IsNullOrWhiteSpace(data.Email))
                    ModelState.AddModelError(nameof(data.Email), "Vui lòng nhập Email của khách hàng");
                //Thông  qua thuộc tính IsVaid của ModelState để kiểm tra xem có tồn tại lỗi hay không?
                if (!ModelState.IsValid)
                {

                    return View("Edit", data);
                }

                if (data.EmployeeID == 0)
                {
                    int id = CommonDataService.AddEmployee(data);
                    if (id <= 0)
                    {
                        ModelState.AddModelError(nameof(data.Email), "Địa chỉ Email bị trùng");
                        return View("Edit", data);
                    }
                }    
                else
                {
                    bool result = CommonDataService.UpdateEmployee(data);
                    if (!result)
                    {
                        ModelState.AddModelError(nameof(data.Email), "Địa chỉ email bị trùng với nhân viên khác");
                        return View("Edit", data);
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "Không thể lưu được dữ liệu");
                return View("Edit", data);
            }
            
        }
        public IActionResult Delete(int id)
        {
            if (Request.Method == "POST")
            {
                CommonDataService.DeleteEmployee(id);
                return RedirectToAction("Index");
            }

            var model = CommonDataService.GetEmployee(id);
            if (model == null)
                return RedirectToAction("Index");

            ViewBag.AllowDelete = !CommonDataService.IsUsedEmployee(id);
            return View(model);
        }
    }
}
