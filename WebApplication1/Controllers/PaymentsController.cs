using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace WebApplication1.Controllers
{
    public class PaymentsController : Controller
    {
        public PaymentsController()
        {
        }

        //[HttpPost("create-checkout-session/{userName}")]

        //public ActionResult CreateCheckoutSession(string userName)
        //{
        //    var options = new SessionCreateOptions
        //    {
        //        LineItems = new List<SessionLineItemOptions>
        //        {
        //            new SessionLineItemOptions
        //            {
        //                PriceData = new SessionLineItemPriceDataOptions
        //                {
        //                    UnitAmount = 200000,
        //                    Currency = "USD",
        //                    ProductData = new SessionLineItemPriceDataProductDataOptions
        //                    {
        //                        Name = "T-shirt",
        //                    },
        //                },
        //                Quantity = 1,
        //            }

        //        },
        //        //PaymentMethodTypes = new List<string> { "card", "afterpay_clearpay" },
        //        Mode = "payment",
        //        SuccessUrl = "http://localhost:4200/success-transaction?session_id={CHECKOUT_SESSION_ID}&user_id=" + userName,
        //        CancelUrl = "http://localhost:4200/cancel-transaction",
        //        ExpiresAt = DateTime.Now.AddMinutes(30) 
        //        //? session_id ={ CHECKOUT_SESSION_ID }&user_id = 2424121",
        //    };

        //    var service = new SessionService();
        //    Session session = service.Create(options);

        //    Response.Headers.Add("Location", session.Url);
        //    return new StatusCodeResult(303);
        //}

        [HttpGet("/order/success")]
        public IActionResult OrderSuccess([FromQuery] string session_id, string user_id)
        {
            var sessionService = new SessionService();
            //Session session = sessionService.Get(session_id);

            //var customerService = new CustomerService();
            //Customer customer = customerService.Get(session.CustomerId);
            ObjectResult objectResult = new ObjectResult(user_id);
            return Ok(objectResult);
        }


    }
}


