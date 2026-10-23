using Shared.DTOS.BasketDTOs;
using Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Abstaction.PymentService
{
    public interface IPymentService
    {
        Task<Result<BasketDTO>> CreateOrUpdatePaymentIntent(string basketId,CancellationToken ct=default);
        Task PaymentSucceded(string paymentIntentId);
        Task PaymentFailed(string paymentIntentId);
    }
}
