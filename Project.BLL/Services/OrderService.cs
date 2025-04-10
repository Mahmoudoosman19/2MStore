using Project.BLL.Interfaces;
using Project.BLL.Specifications.OrderSpecs;
using Project.DAL.Entities;
using Project.DAL.Entities.Order;

namespace Project.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        ///private readonly IGenericRepository<Product> _productsRepo;
        ///private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
        ///private readonly IGenericRepository<Order> _orderRepo;

        public OrderService(IBasketRepository basketRepo,
            IUnitOfWork unitOfWork,
            IPaymentService paymentService)

        ///IGenericRepository<Product> productsRepo,
        ///IGenericRepository<DeliveryMethod> deliveryMethodRepo,
        ///IGenericRepository<Order> orderRepo)
        {
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
            ///_productsRepo = productsRepo;
            ///_deliveryMethodRepo = deliveryMethodRepo;
            ///_orderRepo = orderRepo;
        }


        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address addressShiping)
        {
            var basket = await _basketRepo.GetBasketAsync(basketId);

            var orderItems = new List<OrderItem>();
            var productRepository = _unitOfWork.Repository<Product>();
            if (basket?.BasketItems?.Count > 0)
            {
                foreach (var item in basket.BasketItems)
                {
                    var product = await productRepository.GetByIdAsync(item.Id);
                    var productItemOrdered = new ProductItemOrdered(item.Id, item.ProductName, item.PictureUrl);
                    var orderItem = new OrderItem(productItemOrdered, product.PriceBeforeDiscount, item.Quantity);
                    orderItems.Add(orderItem);
                    if (item.Quantity > product.Count)
                    {
                        return null;
                    }
                    product.Count -= item.Quantity;

                    await productRepository.UpdateAsync(product);
                }
            }

            #region Order Validation
            var subtotal = orderItems.Sum(orderItem => orderItem.Price * orderItem.Quantity);
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            var OrdersRepo = _unitOfWork.Repository<Order>();

            var orderSpecs = new OrderWithPaymentIntentSpecifications(basket.PaymentIntentId);
            var existingOrder = await OrdersRepo.GetByIdWithSpecAsync(orderSpecs);

            if (existingOrder != null)
            {
                OrdersRepo.DeleteAsync(existingOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            }
            #endregion


            var order = new Order(buyerEmail, addressShiping, deliveryMethod, orderItems, subtotal, basket.PaymentIntentId);  //Create Order
            await OrdersRepo.AddAsync(order);

            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return null;
            return order;


        }

        public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var deliveryMethodsRepo = _unitOfWork.Repository<DeliveryMethod>();
            var deliveryMethods = deliveryMethodsRepo.GetAllAsync();
            return deliveryMethods;
        }

        public Task<Order?> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
        {
            var orderRepo = _unitOfWork.Repository<Order>();
            var orderSpec = new OrderSpecification(orderId, buyerEmail);
            var order = orderRepo.GetByIdWithSpecAsync(orderSpec);
            return order;
        }
        public async Task<IReadOnlyList<Order>> GetAllOrdersAsync(OrderSpecParams orderSpecParams)
        {
            var spec = new OrderSpecification(orderSpecParams);

            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            return orders;
        }
        public async Task<IReadOnlyList<Order>> GetOrderCount(OrderSpecParams orderSpecParams)
        {
            var spec = new OrderSpecificationCount(orderSpecParams);

            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            return orders;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var orderRepo = _unitOfWork.Repository<Order>();
            var spec = new OrderSpecification(buyerEmail);
            var orders = await orderRepo.GetAllWithSpecAsync(spec);
            return orders;
        }

        public Task<Order?> GetOrderByIdAsync(int orderId)
        {
            var orderRepo = _unitOfWork.Repository<Order>();
            var orderSpec = new OrderSpecification(orderId);
            var order = orderRepo.GetByIdWithSpecAsync(orderSpec);
            return order;
        }
    }
}
