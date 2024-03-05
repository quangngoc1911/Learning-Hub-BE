using Learning_hub.Data;
using Learning_hub.Entities;
using Learning_hub.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Collections.Specialized.BitVector32;
using Microsoft.AspNetCore.Session;

namespace Learning_hub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly LearningHubContext _context;
        private readonly AppSetting _appSettings;
        private readonly PayPalSettings _paypalSettings;

     

        public PaymentController(IOptions<PayPalSettings> paypalSettings,LearningHubContext context, IOptionsMonitor<AppSetting> optionsMonitor)
        {
            _context = context;
            _appSettings = optionsMonitor.CurrentValue;
            _paypalSettings = paypalSettings.Value;
        }

        public class Order
        {
            public int MaNguoiDay { get; set; }
            public int MaDangKy { get; set; }
            public string MaKhoaHoc { get; set; }
            public int MaHocVien { get; set; }
            public string TieuDeKhoaHoc { get; set; }
            public decimal HocPhi { get; set; }
            public DateTime NgayThanhToan { get; set; }
            public string TinhTrang { get; set; }
        }


        [HttpPost("CreatePayment")]
        public IActionResult CreatePayment([FromBody] List<Order> orders)
        {
            try
            {

                var madks = new List<int>();
                foreach (var item in orders)
                {
                    decimal giamGia = item.HocPhi * 0.1m;
                    decimal hocPhiSauGiam = item.HocPhi - giamGia;
                    var khoaHoc = new DangKyHoc
                    {
                        MaDangKy = item.MaDangKy,
                        MaKhoaHoc = item.MaKhoaHoc,
                        MaHocVien = item.MaHocVien,
                        TieuDeKhoaHoc = item.TieuDeKhoaHoc,
                        HocPhi = item.HocPhi,
                        NgayThanhToan = item.NgayThanhToan,
                        TinhTrang = item.TinhTrang,
                    };
                    var thanhtoan = new ThanhToan
                    {
                        MaNguoiDay = item.MaNguoiDay,
                        MaDangKy = item.MaDangKy,
                        SotTien = hocPhiSauGiam,
                        SoTienNhanDuoc = giamGia,
                        TinhTrang = "chuathanhtoan",
                    };
                    _context.KhoaHocs.Where(x => x.MaKhoaHoc == item.MaKhoaHoc).FirstOrDefault().SoLuongHocVien += 1;
                    // Lưu vào cơ sở dữ liệu
                    _context.DangKyHocs.Add(khoaHoc);
                    _context.ThanhToans.Add(thanhtoan);
                    madks.Add(khoaHoc.MaDangKy);
                }
                _context.SaveChanges();

                // Tạo một APIContext với thông tin cấu hình của bạn
                var apiContext = new APIContext(new OAuthTokenCredential(_paypalSettings.ClientId, _paypalSettings.ClientSecret).GetAccessToken());

                var idsParam = String.Join(",", madks);

                // Tạo đơn hàng và cài đặt thông tin thanh toán
                var payment = CreatePaymentObject(orders, idsParam);

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



        [HttpGet("ExecutePayment")]
        public IActionResult ExecutePayment(string oderids, string paymentId, string payerId)
        {
            // Tạo một APIContext với thông tin cấu hình của bạn
            var apiContext = new APIContext(new OAuthTokenCredential(_paypalSettings.ClientId, _paypalSettings.ClientSecret).GetAccessToken());

            // Thực hiện xác nhận thanh toán
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            var executedPayment = new Payment() { id = paymentId }.Execute(apiContext, paymentExecution);

            // Xử lý kết quả thanh toán ở đây

            var invoiceNumber = executedPayment.transactions.FirstOrDefault();
            List<int> maDangKyList = oderids.Split(',').Select(int.Parse).ToList();
            foreach (int maDangKy in maDangKyList)
            {
                var dangKy = _context.DangKyHocs.FirstOrDefault(dk => dk.MaDangKy == maDangKy);

                if (dangKy != null)
                {
                    dangKy.TinhTrang = "thanhcong";
                    _context.SaveChanges();
                }
            }


            return Ok(invoiceNumber);
            //return Ok(paypalInvoiceNumber);

        }



        private Payment CreatePaymentObject(List<Order> orders, string param)
        {
            // Tạo đơn hàng và cài đặt thông tin thanh toán
            // Lưu ý: Bạn cần thay đổi logic này phù hợp với cấu trúc đơn hàng của bạn

            var itemList = new ItemList()
            {
                items = new List<Item>()
            };

            foreach (var order in orders)
            {

                itemList.items.Add(new Item()
                {
                    name = order.TieuDeKhoaHoc,
                    currency = "USD",
                    price = order.HocPhi.ToString(),
                    quantity = "1",
                    sku = order.MaKhoaHoc

                });
            }

            var paymentDetails = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = orders.Sum(o => o.HocPhi).ToString()
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

            var returnUrl = "http://localhost:3000/thanhcong?oderids=" + param;
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

        





    }  
}

