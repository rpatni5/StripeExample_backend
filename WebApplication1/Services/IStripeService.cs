using Stripe.Checkout;

namespace WebApplication1.Services
{
    public interface IStripeService
    {
        Task<Session> CheckOut(long price, string userId);
    }
}
