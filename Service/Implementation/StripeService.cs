using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementation
{
    using Stripe;
    using System;
    using Microsoft.Extensions.Configuration;
    using Stripe.Checkout;

    public class StripeService
    {
        private readonly string _stripeSecretKey;

        public StripeService(IConfiguration configuration)
        {
            StripeConfiguration.ApiKey = "sk_test_51PfJ5ZCIzuMsZ1B84oco5TwUKgXMnbfINEupJs6BdcRdap68w2XDI4Naf3JQukGXF1dJLy4gkZ49vaExwts3aG9q00kIAmyGVA";
        }

        public Session CreateCheckoutSession(string successUrl, string cancelUrl, double amount)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        UnitAmount = (long)(amount * 100),
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Album Purchase",
                        },
                    },
                    Quantity = 1,
                },
            },
                Mode = "payment",
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
            };

            var service = new SessionService();
            return service.Create(options);
        }
    }

}
