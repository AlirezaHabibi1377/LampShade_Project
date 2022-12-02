using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using _0_Framework.Application;
using _0_Framework.Application.Sms;
using IPE.SmsIrClient;
using IPE.SmsIrClient.Models.Requests;
using Microsoft.Extensions.Configuration;
using ShopManagement.Application.Contracts.Order;
using ShopManagement.Domain.OrderAgg;
using ShopManagement.Domain.Services;

namespace ShopManagement.Application
{
    public class OrderApplication : IOrderApplication
    {
        private readonly IAuthHelper _authHelper;
        private readonly IConfiguration _configuration;
        private readonly IOrderRepository _orderRepository;
        private readonly IShopInventoryAcl _shopInventoryAcl; 
        //private readonly ISmsService _smsService;
        private readonly IShopAccountAcl _shopAccountAcl;
        public OrderApplication(IOrderRepository orderRepository, IAuthHelper authHelper, IConfiguration configuration, IShopInventoryAcl shopInventoryAcl, IShopAccountAcl shopAccountAcl)
        {
            _orderRepository = orderRepository;
            _authHelper = authHelper;
            _configuration = configuration;
            _shopInventoryAcl = shopInventoryAcl;
            //_smsService = smsService;
            _shopAccountAcl = shopAccountAcl;
        }

        public long PlaceOrder(Cart cart)
        {
            var currentAccountId = _authHelper.CurrentAccountId();
            var order = new Order(currentAccountId, cart.PaymentMethod, cart.TotalAmount, cart.DiscountAmount,
                cart.PayAmount);

            foreach (var cartItem in cart.Items)
            {
                var orderItem = new OrderItem(cartItem.Id, cartItem.Count, cartItem.UnitPrice, cartItem.DiscountRate);
                order.AddItem(orderItem);
            }

            _orderRepository.Create(order);
            _orderRepository.SaveChanges();
            return order.Id;
        }

        public string PaymentSucceeded(long id, long refId)
        {
            var order = _orderRepository.Get(id);
            order.PaymentSucceeded(refId);
            var symbol = _configuration.GetSection("Symbol").ToString();
            var issueTrackingNo = CodeGenerator.Generate(symbol);

            if (!_shopInventoryAcl.ReduceFromInventory(order.Items)) return "";
            _orderRepository.SaveChanges();


            var user = _shopAccountAcl.GetAccountBy(order.AccountId);

            var a = SmsService.SendAsync(user.mobile, $"{user.name} گرامی سفارش شما با شماره پیگیری {issueTrackingNo} با موفقیت ثبت و ارسال شد.");


            return issueTrackingNo;

        }

        public void Cancel(long id)
        {
            var order =_orderRepository.Get(id);
            order.Cancel();
            _orderRepository.SaveChanges();
        }

        public List<OrderViewModel> Search(OrderSearchModel searchModel)
        {
            return _orderRepository.Search(searchModel);
        }

        public List<OrderItemViewModel> GetItems(long orderId)
        {
            return _orderRepository.GetItems(orderId);
        }

        public double GetAmountBy(long id)
        {
            return _orderRepository.GetAmountBy(id);
        }
    }
}
