using Microsoft.Extensions.Options;
using Service_Abstaction.PymentService;
using Shared.DTOS.Pyment;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Implementation.Pyments
{
    public class StripPaymentGatway : IPymentGatway
    {
        private readonly PaymentGetwaySettings _payment;
        private readonly PaymentIntentService _paymentIntent = new PaymentIntentService();

        public StripPaymentGatway(IOptions<PaymentGetwaySettings> options)
        {
            StripeConfiguration.ApiKey = options.Value.SecretKey;

        }
        public async Task<PymentIntentResult> CreatePaymentIntentAsync(decimal amount, string currency, CancellationToken ct = default)
        {
            var option = new PaymentIntentCreateOptions()
            {

                Amount = (long)amount ,
                Currency=currency.ToLower(),
                AllowedPaymentMethodTypes = ["card"]
             

            };
         var intent=await _paymentIntent.CreateAsync(option, cancellationToken: ct);
            return new PymentIntentResult(intent.ClientSecret, intent.Id);

        }

        public async Task<PymentIntentResult> UpdatePaymentIntentAsync(decimal amount, string paymentIntentId, CancellationToken ct = default)
        {
            var option = new PaymentIntentUpdateOptions()
            {
                Amount = (long)amount
            };
            var intent =await _paymentIntent.UpdateAsync(paymentIntentId,option, cancellationToken: ct);
            return new PymentIntentResult(intent.ClientSecret, intent.Id);
        }
    }
}
