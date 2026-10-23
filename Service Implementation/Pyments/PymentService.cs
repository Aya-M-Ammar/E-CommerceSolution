using AutoMapper;
using E_Commerce.Domain.Entity.Order;
using E_Commerce.Domain.Entity.Product;
using E_Commerce.Domain.Interfaces.Repository;
using Service_Abstaction.PymentService;
using Service_Implementation.Specification;
using Shared.DTOS.BasketDTOs;
using Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Implementation.Pyments
{
    public class PymentService : IPymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IPymentGatway _paymentGateway;
        private readonly IUniteOfWork _uniteOfWork;
        private readonly IMapper _mapper;

        public PymentService(
            IBasketRepository basketRepository,
            IPymentGatway paymentGateway,
            IUniteOfWork uniteOfWork,
            IMapper mapper)
        {
            _basketRepository = basketRepository;
            _paymentGateway = paymentGateway;
            this._uniteOfWork = uniteOfWork;
            this._mapper = mapper;
        }
        public async Task<Result<BasketDTO>> CreateOrUpdatePaymentIntent(string basketId, CancellationToken ct = default)
        {
            var basket =await _basketRepository.GetBasketAsync(basketId);
            if (basket == null)
                return Error.NotFound($"Basket with {basketId} is Not Found", "Basket Not Found");
            if(basket.items.Count == 0)
                return Error.Validation($"Basket with {basketId} is Empty", "Basket is Empty");
            if(!basket.DeliveryMethodId.HasValue) return Error.Validation($"Basket with {basketId} has no Delivery Method", "Delivery Method Not Found");


            var deliveryMethod=await _uniteOfWork.GetRepositoryAsync<DeliveryMethod,int>().GetByIdAsync(basket.DeliveryMethodId.Value);
            if (deliveryMethod == null) return Error.Validation($"Delivery Method with {basket.DeliveryMethodId.Value} is Not Found", "Delivery Method Not Found");


                basket.ShippingPrice = deliveryMethod.Price;

            var productsIds = basket.items.Select(x => x.Id).ToHashSet();

            var products = await _uniteOfWork.GetRepositoryAsync<Product, int>()
                .GetAllAsync();

            foreach (var item in basket.items)
            {
                var product =await _uniteOfWork.GetRepositoryAsync<Product, int>().GetByIdAsync(item.Id);
                 
                if(product == null)
                    return Error.NotFound($"Product with {item.Id} is Not Found", "Product Not Found");
                item.Price = product.Price;
            }
            var subTotal = basket.items.Sum(x => x.Price * x.Quantity);
            var amount = (long)(subTotal + deliveryMethod.Price*100m);

            if(string.IsNullOrEmpty(basket.PaymentIntent))
            {
                var paymentIntent = await _paymentGateway.CreatePaymentIntentAsync(amount, "usd");
                basket.PaymentIntent = paymentIntent.PaymentIntentId;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var paymentIntent = await _paymentGateway.UpdatePaymentIntentAsync(amount, basket.PaymentIntent);
                basket.PaymentIntent = paymentIntent.PaymentIntentId;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }

           await _basketRepository.CreateOrUpdateBasketAsync(basket);
            return _mapper.Map<BasketDTO>(basket);

        }

        public async Task PaymentFailed(string paymentIntentId)
        {
           var order = await _uniteOfWork.GetRepositoryAsync<Order, Guid>().GetByIdAsync(new PymentIntentSpcific(paymentIntentId));
            if (order == null) return;
            order.Status = OrderStatus.PymentFailed;
            _uniteOfWork.GetRepositoryAsync<Order, Guid>().Update(order);
            await _uniteOfWork.SaveChangesAsync();
            return;
        }

        public Task PaymentSucceded(string paymentIntentId)
        {
           var order = _uniteOfWork.GetRepositoryAsync<Order, Guid>().GetByIdAsync(new PymentIntentSpcific(paymentIntentId)).Result;
            if (order == null) return Task.CompletedTask;
            order.Status = OrderStatus.PymentRecevied;
            _uniteOfWork.GetRepositoryAsync<Order, Guid>().Update(order);
            _uniteOfWork.SaveChangesAsync();
            return Task.CompletedTask;
        }
    }
}
