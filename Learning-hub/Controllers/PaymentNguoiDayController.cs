using Learning_hub.Entities;
using Learning_hub.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using PayPal.Api;
using System.Collections.Generic;

namespace Learning_hub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentNguoiDayController : ControllerBase
    {
        private readonly LearningHubContext _contexts;
        private readonly PayPalSettings _paypalSettings;



        public PaymentNguoiDayController(IOptions<PayPalSettings> paypalSettings, LearningHubContext context)
        {
            _contexts = context;
            _paypalSettings = paypalSettings.Value;
        }


        [HttpGet("ListNguoiDayThanhToan")]
        public IActionResult ListNguoiDayThanhToan(int userId)
        {
            try
            {
                var tyGiaUSDToVND = 23000;
                var ListPayment = from ThanhToan in _contexts.ThanhToans
                                 join NguoiDay in _contexts.NguoiDays on ThanhToan.MaNguoiDay equals NguoiDay.MaNguoiDay
                                 where ThanhToan.TinhTrang == "chuathanhtoan"
                                 select new
                                 {
                                    MaThanhToan = ThanhToan.MaThanhToan,
                                    MaDangKy = ThanhToan.MaDangKy,
                                    TenNguoiDay = NguoiDay.TenNguoiDay,
                                    HinhAnh = NguoiDay.HinhAnh,
                                     NgayTao = ThanhToan.NgayTao,
                                    SoTien = ThanhToan.SotTien * tyGiaUSDToVND,
                                    SoTienNhanDuoc = ThanhToan.SoTienNhanDuoc * tyGiaUSDToVND,
                                 };

                return Ok(ListPayment);
            }
            catch (Exception ex)
            {
                // Log lỗi ở đây nếu cần thiết
                Console.Error.WriteLine($"Lỗi khi lấy tiến trình học tập: {ex.Message}");

                // Trả về mã lỗi và thông điệp lỗi
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        public class thanhtoannguoiday
        {

            public int MaDangKy { get; set; }
            public decimal SoTien { get; set; }
            public DateTime NgayThanhToan { get; set; }
        }

        [HttpPost("PostThanhToanNguoiDay")]
        public IActionResult PostThanhToanNguoiDay([FromBody] thanhtoannguoiday thanhtoan )
        {
            try
            {
                var madks = new List<int>();
                madks.Add(thanhtoan.MaDangKy);


                // Tạo một APIContext với thông tin cấu hình của bạn
                var apiContext = new APIContext(new OAuthTokenCredential(_paypalSettings.ClientId, _paypalSettings.ClientSecret).GetAccessToken());

                var idsParam = String.Join(",", madks);

                // Tạo đơn hàng và cài đặt thông tin thanh toán
                var payment = CreatePaymentObject(thanhtoan, idsParam);

                // Thực hiện thanh toán
                var createdPayment = payment.Create(apiContext);

                // Trả về URL redirect để chuyển hướng người dùng đến trang thanh toán của PayPal
                var redirectUrl = createdPayment.links.FirstOrDefault(l => l.rel.Equals("approval_url", StringComparison.OrdinalIgnoreCase))?.href;

                return Ok(new { RedirectUrl = redirectUrl });
                //return Ok(createdPayment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Đã xảy ra lỗi trong quá trình xử lý thanh toán: {ex}");
            }
        }

        private Payment CreatePaymentObject(thanhtoannguoiday thanhtoan, string param)
        {
            // Tạo đơn hàng và cài đặt thông tin thanh toán
            // Lưu ý: Bạn cần thay đổi logic này phù hợp với cấu trúc đơn hàng của bạn

            var itemList = new ItemList()
            {
                items = new List<Item>()
            {
                new Item()
                {
                    name = "Tên Sản Phẩm", // Thay thế bằng tên sản phẩm thực tế
                    currency = "USD",       // Đơn vị tiền tệ
                    price = thanhtoan.SoTien.ToString(), // Giá sản phẩm
                    quantity = "1",         // Số lượng
                    sku = "SKU123"          // SKU của sản phẩm
                }
            }
            };

            var paymentDetails = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = thanhtoan.SoTien.ToString(),
            };

            var amount = new Amount()
            {
                currency = "USD",
                total = (Convert.ToDouble(paymentDetails.tax) + Convert.ToDouble(paymentDetails.shipping) + Convert.ToDouble(paymentDetails.subtotal)).ToString(),
                details = paymentDetails
            };

            var transactionList = new List<Transaction>
            {
                new Transaction()
                {
                    description = "Transaction description",
                    invoice_number = Guid.NewGuid().ToString(), // Sử dụng một số hóa đơn duy nhất
                    amount = amount,
                    item_list = itemList
                }
            };

            var returnUrl = "http://localhost:3000/dashboard/thanhtoan?oderids=" + param;
            var cancelUrl = "http://yourwebsite.com/cancel";

            var redirectUrls = new RedirectUrls()
            {
                cancel_url = cancelUrl,
                return_url = returnUrl
            };

            var payment = new Payment()
            {
                intent = "sale",
                payer = new Payer() { payment_method = "paypal" },
                transactions = transactionList,
                redirect_urls = redirectUrls
            };

            return payment;
        }


        [HttpGet("GetThanhToanNguoiDay")]
        public IActionResult GetThanhToanNguoiDay(int oderids, string paymentId, string payerId)
        {
            //Tạo một APIContext với thông tin cấu hình của bạn
            //var apiContext = new APIContext(new OAuthTokenCredential(_paypalSettings.ClientId, _paypalSettings.ClientSecret).GetAccessToken());

            //Thực hiện xác nhận thanh toán
            //var paymentExecution = new PaymentExecution() { payer_id = payerId };
            //var executedPayment = new Payment() { id = paymentId }.Execute(apiContext, paymentExecution);

            //Xử lý kết quả thanh toán ở đây

            //var invoiceNumber = executedPayment.transactions.FirstOrDefault();


            var dangKy = _contexts.ThanhToans.FirstOrDefault(dk => dk.MaThanhToan == oderids);

                if (dangKy != null)
                {
                    dangKy.TinhTrang = "dathanhtoan";
                    dangKy.NgayThanhToan = DateTime.Now;
                    _contexts.SaveChanges();
                }

            return Ok("thanh toán thành công");

    }










}
}
