using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : Controller
    {
        private readonly IStripeService _stripeService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _frontendSuccessUrl;
        private readonly string _frontendCanceledUrl;
        public PaymentController(IStripeService stripeService, IHttpContextAccessor httpContextAccessor)
        {
            _stripeService = stripeService;
            _httpContextAccessor = httpContextAccessor;

            var request = _httpContextAccessor.HttpContext!.Request;
            var baseUrl = "http://localhost:4200/";// $"{request.Scheme}://{request.Host}";
            _frontendSuccessUrl = baseUrl + "success-transaction?session_id={CHECKOUT_SESSION_ID}&user_id={userName}";
            _frontendCanceledUrl = baseUrl + "/cancel-transactions";

        }

        [HttpPost("PlaceOrder")]
        public async Task<Results<Ok<string>, BadRequest>> PlaceOrder([FromBody] OrderModel order)
        {

            try
            {
                long total = order.Amount * 100;
                Session session = await _stripeService.CheckOut(total, order.UserId);
                //await _orderData.InsertOrderInDb(sessionId, order);
                return TypedResults.Ok(session.Id);

            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest();
            }

        }

        [HttpGet("success")]
        public async Task<Results<RedirectHttpResult, BadRequest>> CheckoutSuccess([FromQuery] string sessionId, string userId)
        {
            try
            {
                var sessionService = new SessionService();
                var session = sessionService.Get(sessionId);


                //var total = session.AmountTotal.Value; <- total from Stripe side also
                //var customerEmail = session.CustomerDetails.Email;

                //CustomerModel customer = new CustomerModel(session.Id, session.CustomerDetails.Name, session.CustomerDetails.Email, session.CustomerDetails.Phone);

                // Save the customer details to your database.
                //await _customerData.InsertCustomerInDb(customer);

                return TypedResults.Redirect(_frontendSuccessUrl.Replace("{userName}", userId), true, true);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest();
            }


        }

        /// <summary>
        /// this API is going to be hit when order is a failure
        /// </summary>
        /// <returns>A redirect to the front end success page</returns>
        [HttpGet("canceled")]
        public async Task<Results<RedirectHttpResult, BadRequest>> CheckoutCanceled([FromQuery] string? sessionId)
        {
            try
            {
                var sessionService = new SessionService();
                var session = sessionService.Get(sessionId);

                sessionService.Expire(sessionId);
                // Insert here failure data in data base
                return TypedResults.Redirect(_frontendCanceledUrl, true, true);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest();
            }


        }



        [HttpPost("GetPaymentLink")]
        public async Task<Results<Ok<string>, BadRequest>> GetPaymentLink([FromBody] OrderModel order)
        {

            try
            {
                long total = order.Amount * 100;
                Session session = await _stripeService.CheckOut(total, order.UserId);
                //await _orderData.InsertOrderInDb(sessionId, order);
                return TypedResults.Ok(session.Url);

            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest();
            }

        }

    }

}
