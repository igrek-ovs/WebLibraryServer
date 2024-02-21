using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using testWabApi1.Data;
using testWabApi1.Models;

namespace testWabApi1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : Controller
    {
        private readonly StripeSettings _stripeOptions;
        private readonly PaymentIntentService _paymentIntentService;
        private readonly LibraryDbContext _libraryDbContext;
        const string endpointSecret = "whsec_jz57vvRe2pyLXQyg6buOfqntk45aT97V";

        public PaymentController(IOptions<StripeSettings> stripeOptions, LibraryDbContext libraryDbContext)
        {
            _stripeOptions = stripeOptions.Value;
            StripeConfiguration.ApiKey = _stripeOptions.SecretKey;
            _paymentIntentService = new PaymentIntentService();
            _libraryDbContext = libraryDbContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentRequest paymentRequest)
        {
            try
            {
                var createOptions = new PaymentIntentCreateOptions
                {
                    Amount = 500,
                    Currency = "usd",
                    PaymentMethod = paymentRequest.PaymentMethodId,
                    ConfirmationMethod = "manual",
                    Confirm = true,
                    ReturnUrl = "https://www.youtube.com/watch?v=HR10AT8fbvs"
                };
                var paymentIntent = await _paymentIntentService.CreateAsync(createOptions);

                return Ok(new { PaymentIntentId = paymentIntent.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Failed to create payment", Message = ex.Message });
            }
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> CreateTransaction()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);

                if (stripeEvent.Type == Events.ChargeSucceeded)
                {
                    var charge = stripeEvent.Data.Object as Charge;
                    if (charge != null)
                    {
                        var transaction = new PaymentTransaction
                        {
                            PaymentIntentId = charge.PaymentIntentId,
                            Amount = charge.Amount / 100,
                            Currency = charge.Currency,
                            PaymentDate = DateTime.UtcNow
                        };

                        await _libraryDbContext.PaymentTransactions.AddAsync(transaction);
                        await _libraryDbContext.SaveChangesAsync();
                    }
                }
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }
    }
}
