using SV20T1020183.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020183.DataLayers
{
    public interface IOrderDAL
    {
        /// <summary>
        /// Tìm kiếm và lấy danh sách đơn hàng dưới dạng phân trang
        /// </summary>
        IList<Order> List(int page = 1, int pageSize = 0,
        int status = 0, DateTime? fromTime = null, DateTime? toTime = null,
        string searchValue = "");

        /// <summary>
        /// Đếm số lượng đơn hàng thỏa điều kiện tìm kiếm
        /// </summary>
        int Count(int status = 0, DateTime? fromTime = null, DateTime? toTime = null,
        string searchValue = "");
        /// <summary>
        /// Lấy thông tin đơn hàng theo mã đơn hàng
        /// </summary>
        Order? Get(int orderID);
        /// <summary>
        /// Bổ sung đơn hàng mới
        /// </summary>
        int Add(Order data);
        /// <summary>
        /// Cập nhật thông tin của đơn hàng
        /// </summary>
        bool Update(Order data);
        /// <summary>
        /// Xóa đơn hàng và chi tiết của đơn hàng
        /// </summary>
        bool Delete(int orderID);
        /// <summary>
        /// Lấy danh sách hàng được bán trong đơn hàng (chi tiết đơn hàng)
        /// </summary>
        IList<OrderDetail> ListDetails(int orderID);
        /// <summary>
        /// Lấy thông tin của 1 mặt hàng được bán trong đơn hàng (1 chi tiết trong đơn hàng)
        /// </summary>
        OrderDetail? GetDetail(int orderID, int productID);
        /// <summary>
        /// Thêm mặt hàng được bán trong đơn hàng) theo nguyên tắc:
        /// - Nếu mặt hàng chưa có trong chi tiết đơn hàng thì bổ sung
        /// - Nếu mặt hàng đã có trong chi tiết đơn hàng thì cập nhật lại số lượng và giá bán
        /// </summary>
        bool SaveDetail(int orderID, int productID, int quantity, decimal salePrice);
        /// <summary>
        /// Xóa 1 mặt hàng ra khỏi đơn hàng
        /// </summary>
        bool DeleteDetail(int orderID, int productID);

        bool EditProvince(int id, string DeliveryProvince, string DeliveryAddress);
    }
}
