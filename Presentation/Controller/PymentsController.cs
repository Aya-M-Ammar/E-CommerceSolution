using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Service_Abstaction.PymentService;
using Service_Implementation.Pyments;
using Shared.DTOS.BasketDTOs;
using Shared.DTOS.Pyment;
using Stripe;
using Stripe.V2.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Controller
{
    public class PymentsController : ApiBaseController
    {
        private readonly IPymentService _pymentService;
        private readonly IConfiguration _configuration;
        private readonly PaymentGetwaySettings _paymentsettings;

        public PymentsController(IPymentService pymentService, IConfiguration configuration, IOptions<PaymentGetwaySettings> options)
        {
            this._pymentService = pymentService;
            _configuration = configuration;
            this._paymentsettings = options.Value;
        }
        [Authorize]
        [HttpPost("{basketid}")]
        public async Task<ActionResult<BasketDTO>> CreateOrUpdatePaymentIntent(string basketid, CancellationToken ct)
        {
            var result = await _pymentService.CreateOrUpdatePaymentIntent(basketid, ct);
            return Ok(result);
        }





        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(
                HttpContext.Request.Body
            ).ReadToEndAsync();



            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                      json,
                      Request.Headers["Stripe-Signature"],

                      _paymentsettings.WebhookSeceret
                  );

                switch (stripeEvent.Type)
                {
                    case EventTypes.PaymentIntentSucceeded:
                        {
                            var paymentIntent =
                                stripeEvent.Data.Object as PaymentIntent;
                           
                            if (paymentIntent != null)
                                await _pymentService.PaymentSucceded(paymentIntent.Id);


                            break;
                        }

                    case EventTypes.PaymentIntentPaymentFailed:
                        {
                            var paymentIntent =
                                stripeEvent.Data.Object as PaymentIntent;
                            if(paymentIntent != null)
                                await _pymentService.PaymentFailed(paymentIntent.Id);


                            break;
                        }
                        break;
                }
            }
            catch (StripeException)
            {
                return BadRequest("Invalid Stripe signature.");
            }

            return Ok();
        }



    }
}
