using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.Pyment
{
    public class PaymentGetwaySettings
    {
        public string DefaultCurrency { get; set; }=default!;
        public string SecretKey { get; set; }=default!;
        public string WebhookSeceret  { get; set; }=default!;

    }
}
