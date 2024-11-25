using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanMayTinh.Models;
using WebBanMayTinh.Models.ViewModel;

namespace WebBanMayTinh.Controllers
{
    public class ShoppingCartController : Controller
    {
        private Web_Ban_May_TinhEntities db = new Web_Ban_May_TinhEntities();



        //Hàm lấy ra dịch vụ giỏ hàng
        public CartService Get_CartService()
        {
            CartService CrtService = new CartService(Session);
            return CrtService;
        }




        public ActionResult ShoppingCartPage()
        {
            var cart = Get_CartService().GetCart();

            return View(cart);
        }


        //Chức năng thêm vào giỏ hàng
        public ActionResult AddToCart(int id,int quantity = 1)
        {
            var products = db.Products.Find(id);


            var cartItem = new CartItem();


            if(products != null)
            {
                var cartService = Get_CartService();

                if(products.DiscountValue == null)
                {
                    cartService.GetCart().AddItems(products.ProductID,products.ProductName, products.ProductImage, products.ProductPrice, quantity, products.Category.CategoryName,products.ProductPrice,products.DiscountValue);
                }


                else
                {
                    cartService.GetCart().AddItems(products.ProductID, products.ProductName, products.ProductImage, products.DiscountValue ?? 0, quantity, products.Category.CategoryName, products.ProductPrice, products.DiscountValue);
                }
            }
            return RedirectToAction("ShoppingCartPage");
        }


        //Chức năng Xóa sản phẩm khỏi giỏ hàng
        public ActionResult DeleteItemsOnCart(int id)
        {
            var cartService = Get_CartService();

            cartService.GetCart().RemoveProductOutCart(id);

            return RedirectToAction("ShoppingCartPage");
        }


        //Chức năng xóa Sản phẩm
        public ActionResult ClearCart()
        {
            Get_CartService().clearCart();

            return RedirectToAction("ShoppingCartPage");
        }


        //Chức năng cập nhật số lượng
        [HttpPost]
        public ActionResult UpdateQuantity(int id,int quantity)
        {
            var cartService = Get_CartService();


            cartService.GetCart().UpdateQuantity(id, quantity);


            return RedirectToAction("ShoppingCartPage");
        }


        // Get:Order/Checkout
        [Authorize]
        public ActionResult CheckoutPage()
        {
            //kiểm tra giỏ hàng theo session
            //nếu giỏ hàng rỗng hoặc không có sản phẩm thì về trang chủ
            var cartService = Get_CartService();

            if (cartService.GetCart() == null)
            {
                return RedirectToAction("HomePage", "Home");
            }

            //Xác thực người dùng đã đăng nhập chưa, nếu chưa thì chuyển tới trang đăng nhập
            var user = db.Users.SingleOrDefault(u => u.Email == User.Identity.Name);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            //Lấy thông tin khách hàng từ database, nếu chưa có thì chuyển hướng tới trang đăng nhập
            //Nếu có rồi thì lấy địa chỉ của khách hàng và gắn vào shippingaddress của checkoutVM
            var customer = db.Customers.SingleOrDefault(c => c.Email_User == User.Identity.Name);
            if (customer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new CheckoutVM //tạo dữ liệu hiển thị cho checkoutVM
            {
                CartItems = cartService.GetCart().CartItems.ToList(),//Lấy danh sách các sản phâm trong giỏ hàng
                TotalAmount = cartService.GetCart().TotalValue(),//Tổng giá trị mặt hàng trong giỏ hàng
                OrderDate = DateTime.Now, //Mặc định lấy thời gian đặt hàng
                ShippingAddress = customer.CustomerAddress,//Lấy địa chỉ mặc định từ bảng customer
                CustomerID = customer.CustomerID,//Lấy mã khách hàng từ bảng customer
                Username = customer.CustomerName,//lấy tên đăng nhập từ bảng customer
                Phone = customer.CustomerPhone,
            };
            return View(model);
        }


        // POST : Order/CheckOut
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult CheckoutPage(CheckoutVM model)
        {
            if (ModelState.IsValid)
            {
                var cart = Get_CartService();
                //Nếu giỏ hàng rỗng sẽ điều hướng tới trang Home
                if (cart == null) { return RedirectToAction("HomePage", "Home"); }
                //Nếu người dùng chưa đăng nhập sẽ điều hướng tới trang Login
                var user = db.Users.SingleOrDefault(u => u.Email == User.Identity.Name);
                if (user == null) { return RedirectToAction("Login", "Account"); }
                //nếu khách hàng không khớp với tên đăng nhập, sẽ điều hướng tới trang Login
                var customer = db.Customers.SingleOrDefault(c => c.Email_User == User.Identity.Name);
                if (customer == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                //Thiết lập PaymentStatus dựa trên PaymentMethod
                string paymentStatus = "Chưa thanh toán";
                switch (model.PaymentMethod)
                {
                    case "Tiền mặt": paymentStatus = "Thanh toán tiền mặt"; break;
                    case "Mua trước trả sau": paymentStatus = "Trả góp"; break;
                    default: paymentStatus = "chưa thanh toán"; break;
                }

                //Tạo đơn hàng và chi tiết đơn hàng liên quan
                var order = new Order
                {
                    CustomerID = customer.CustomerID,
                    OrderDate = model.OrderDate,
                    TotalAmount = model.TotalAmount,
                    PaymentStatus = paymentStatus,
                    PaymentMethod = model.PaymentMethod,
                    ShippingAddress = model.ShippingAddress,
                    OrderDetails = cart.GetCart().CartItems.Select(item => new OrderDetail
                    {
                        ProductID = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = item.TotalPrice,
                    }).ToList()
                };

                //lưu đơn hàng vào csdl
                db.Orders.Add(order);
                db.SaveChanges();

                //Xóa giỏ hàng sau khi đặt hàng thành công
                cart.clearCart();
                return RedirectToAction("OrderSuccess","Order", new { id = order.OrderID });
            }
            return View(model);
        }
    }
}