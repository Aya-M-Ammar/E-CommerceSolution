using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.Pyment
{
    public sealed class PymentIntentResult
    {
        public PymentIntentResult(string clientSecret, string paymentIntentId)
        {
            ClientSecret = clientSecret;
            PaymentIntentId = paymentIntentId;
        }

        public string ClientSecret { get; set; } = default!;
        public string PaymentIntentId { get; set; } = default!;
    }
}
