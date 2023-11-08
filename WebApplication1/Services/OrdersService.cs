using Stripe;
using Stripe.Checkout;

namespace WebApplication1.Services
{
    public class StripeService : IStripeService
    {
        private readonly ILogger<StripeService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StripeService(ILogger<StripeService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Session> CheckOut(long price, string userId)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("user_Id", userId);

            try
            {
                // Get the base URL 
                var request = _httpContextAccessor.HttpContext!.Request;
                var baseUrl = $"{request.Scheme}://{request.Host}";

                var options = new SessionCreateOptions
                {
                    // Stripe calls these user defined endpoints
                    SuccessUrl = $"{baseUrl}/payment/success?sessionId=" + "{CHECKOUT_SESSION_ID}&userId=" + userId,
                    CancelUrl = baseUrl + "/payment/canceled?sessionId={CHECKOUT_SESSION_ID}",

                    //PaymentMethodTypes = new List<string>
                    //{
                    //    "card"
                    //},
                    LineItems = new List<SessionLineItemOptions>
                    {
                        new()
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                UnitAmount = price, // Price defined in usd cents.
                                Currency = "USD",
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                Name = "Payment for scans",
                                Description = "1322 pages of scans printed"
                                //Images = new List<string> { product.OrderedTemplate.CoverImgPath }
                                },
                            },
                           Quantity = 1,
                        },
                    },
                    Metadata = dic,
                    Mode = "payment", // One time payment
                    InvoiceCreation = new SessionInvoiceCreationOptions { Enabled = true }
                };

                var service = new SessionService();
                var session = await service.CreateAsync(options);
                return session;
            }
            catch (Exception ex)
            {
                _logger.LogError("error into Stripe Service on CheckOut() " + ex.Message);
                throw;
            }

        }

        public Stripe.StripeSearchResult<Stripe.Invoice> getLatestInvoiceDetail(string userId, string orderId)
        {

            try
            {
                var options = new InvoiceSearchOptions
                {
                    Query = "customer:\"" + userId + "\" AND metadata['order_id']:'" + orderId + "'",
                };
                var service = new InvoiceService();

                return service.Search(options);
            }
            catch (Exception ex)
            {
                _logger.LogError("error into Stripe Service on getLatestInvoiceDetail() " + ex.Message);
                throw;
            }

        }

        public Stripe.StripeSearchResult<Stripe.Invoice> getPaymentHistoryList(string userId, string orderId)
        {

            try
            {
                var options = new InvoiceSearchOptions
                {
                    Query = "customer:\"" + userId + "\" AND metadata['order_id']:'" + orderId + "'",
                };
                var service = new InvoiceService();

                return service.Search(options);
            }
            catch (Exception ex)
            {
                _logger.LogError("error into Stripe Service on getLatestInvoiceDetail() " + ex.Message);
                throw;
            }

        }
    }
}
