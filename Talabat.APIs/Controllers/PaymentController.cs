using Talabat.APIs.Errors;
using Talabat.Core.Entities.Basket;
using Talabat.Core.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Talabat.APIs.Controllers
{

    public class PaymentController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private const string _webhooksecret = "whsec_71132edecffa039f6efcd8cddedf53a17021434dfc872007a39d6e1cab24afff";
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [Authorize]
        [ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("{basketid}")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            if (basket == null) return BadRequest(new ApiResponse(400, "An Error with your Basket"));
            return Ok(basket);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeSignature = Request.Headers["Stripe-Signature"];
                var stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, _webhooksecret);

                if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded || stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

                    if (paymentIntent == null)
                    {
                        return BadRequest("Invalid PaymentIntent object.");
                    }

                    var succeeded = stripeEvent.Type == EventTypes.PaymentIntentSucceeded;
                    await _paymentService.UpdatePaymentIntentToSucceedOrFailed(paymentIntent.Id, succeeded);
                }
            }
            catch (StripeException ex)
            {
                // Log the error
                return BadRequest("Stripe webhook error");
            }
            catch (Exception ex)
            {
                // Log unexpected error
                return StatusCode(500, "Internal server error");
            }

            return Ok();
        }




    }
}
