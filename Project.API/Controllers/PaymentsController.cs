using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.DAL.Entities.Order;
using Stripe;

namespace Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger;
        private const string _whSecret = "whsec_VKhwCHdMMc4GBNtIC7v2nMWjbAgJoEtQ";

        public PaymentsController(IPaymentService paymentService,ILogger<PaymentsController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }


        [ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);

            if (basket == null) return BadRequest(new ApiResponse(400, "An Error with your basket"));
            return Ok(basket);
        }


        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
                

                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], _whSecret);
                var paymentIntent = (PaymentIntent) stripeEvent.Data.Object;

                Order order;

            switch (stripeEvent.Type)
            {

                case EventTypes.PaymentIntentSucceeded:
                    order = await _paymentService.UpdatedPaymentIntentToSucceededOrFailed(paymentIntent.Id, true);
                    _logger.LogInformation("Payment is succeeded !! ", paymentIntent.Id);
                    break;
                case EventTypes.PaymentIntentPaymentFailed:
                    order = await _paymentService.UpdatedPaymentIntentToSucceededOrFailed(paymentIntent.Id, false);
                    _logger.LogInformation($"Payment failed: {paymentIntent.Id}");
                    break;
            }
            return Ok();
            
        }
    }
}
