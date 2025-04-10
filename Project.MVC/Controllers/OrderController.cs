using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Project.BLL.Dtos;
using Project.BLL.Services;
using Project.BLL.Specifications.OrderSpecs;
using Project.DAL.Entities.Identity;
using Project.DAL.Entities.Order;

namespace Project.MVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IGenericRepository<Order> _repOrder;
        private readonly UserManager<ApplicationUser> _userRepository;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService, IMapper mapper, IGenericRepository<Order> repOrder, UserManager<ApplicationUser> RepoUser)
        {
            _orderService = orderService;
            _repOrder = repOrder;
            _mapper = mapper;
            _userRepository = RepoUser;
        }
        public async Task<IActionResult> Index(OrderSpecParams orderSpecParams)
        {

            var orders = await _orderService.GetAllOrdersAsync(orderSpecParams);
            var Data = _mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders);
            var count = _orderService.GetOrderCount(orderSpecParams).Result.Count();
            int totalPages = (int)Math.Ceiling((double)count / orderSpecParams.PageSize);

            ViewBag.Status = new Dictionary<int, string>
                 {
                  { 0, "Pending" },
                  { 1, "Payment Received" },
                  { 2, "Payment Failed" }
                 };
            ViewBag.Tracking = new Dictionary<int, string>
                 {
                  { 0, "Received" },
                  { 1, "In Stock" },
                  { 2, "Shipped" }
                 };
            ViewBag.PageIndex = orderSpecParams.PageIndex;
            ViewBag.PageSize = orderSpecParams.PageSize;
            ViewBag.TotalPages = totalPages;

            return View(Data);
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var Order = await _orderService.GetOrderByIdAsync(id);
            var Data = _mapper.Map<OrderToReturnDto>(Order);

            ViewBag.Tracking = new Dictionary<int, string>
                 {
                  { 0, "Received" },
                  { 1, "In Stock" },
                  { 2, "Shipped" }
                 };



            return View(Data);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAsync(int id, OrderToReturnDto model)
        {
            if (id != model.Id)
            {
                return View(model);
            }
            var oldOrder = await _orderService.GetOrderByIdAsync(id);
            oldOrder.OrderTrackingStatus = (OrderTracking)Enum.Parse(typeof(OrderTracking), model.OrderTrackingStatus);
            var result = await _repOrder.UpdateAsync(oldOrder);
            if (result == 1)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);

            }


        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var Order = await _orderService.GetOrderByIdAsync(id);
            var Data = _mapper.Map<OrderToReturnDto>(Order);
            var user = await _userRepository.FindByEmailAsync(Data.BuyerEmail);
            ViewBag.user = user;
            return View(Data);
        }

    }
}
