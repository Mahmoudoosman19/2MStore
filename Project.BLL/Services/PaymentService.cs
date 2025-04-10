using Microsoft.Extensions.Configuration;
using Project.BLL.Interfaces;
using Project.BLL.Specifications.OrderSpecs;
using Project.DAL.Entities;
using Project.DAL.Entities.Order;
using Stripe;
using Product = Project.DAL.Entities.Product;

namespace Project.BLL.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketRepository _basketRepository;
        private readonly IConfiguration _configuration;

        public PaymentService(IUnitOfWork unitOfWork, IBasketRepository basketRepository, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _basketRepository = basketRepository;
            _configuration = configuration;
        }
        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:Secretkey"];
            var basket = await _basketRepository.GetBasketAsync(basketId);

            if (basket is null) return null;
            var shippingPrice = 0m;
            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                basket.ShippingPrice = deliveryMethod.Cost;
                shippingPrice = deliveryMethod.Cost;
            }

            if (basket?.BasketItems.Count > 0)
            {
                foreach (var item in basket.BasketItems)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    if (item.Price != product.PriceBeforeDiscount)
                        item.Price = product.PriceBeforeDiscount;
                }
            }

            PaymentIntentService paymentIntentService = new PaymentIntentService();

            PaymentIntent paymentIntent;

            if (string.IsNullOrEmpty(basket.PaymentIntentId)) //Crreate new paymentintent
            {
                var createOptions = new PaymentIntentCreateOptions()
                {
                    Amount = (long)basket.BasketItems.Sum(item => item.Price * item.Quantity * 100) + (long)shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }
                };

                paymentIntent = await paymentIntentService.CreateAsync(createOptions); //integrate with stripe

                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else //update existing intent
            {
                var updateOptions = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)basket.BasketItems.Sum(item => item.Price * item.Quantity * 100) + (long)shippingPrice * 100
                };
                await paymentIntentService.UpdateAsync(basket.PaymentIntentId, updateOptions);


            }

            await _basketRepository.UpdateBasketAsync(basket);
            return basket;
        }

        public async Task<Order> UpdatedPaymentIntentToSucceededOrFailed(string paymentIntentId, bool isSucceeded)
        {
            var spec = new OrderWithPaymentIntentSpecifications(paymentIntentId);
            var order = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);
            if (isSucceeded)
                order.Status = OrderStatus.PaymentReceived;
            else
                order.Status = OrderStatus.PaymentFailed;
            _unitOfWork.Repository<Order>().UpdateAsync(order);
            if (isSucceeded)
                order.Status = OrderStatus.PaymentReceived;
            else
                order.Status = OrderStatus.PaymentFailed;
            order.OrderTrackingStatus = OrderTracking.InStock;
            await _unitOfWork.Repository<Order>().UpdateAsync(order);
            await _unitOfWork.CompleteAsync();
            return order;
        }
    }
}
