
using AutoMapper;
using E_Commerce.Domain.Entity.Basket;
using E_Commerce.Domain.Entity.Order;
using E_Commerce.Domain.Entity.Product;
using E_Commerce.Domain.Interfaces.Repository;
using Service_Abstaction.OrderService;
using Shared.DTOS.OrderDTOs;
using Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Implementation.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IBasketRepository _basketRepository;
        private readonly IUniteOfWork _uniteOfWork;

        public OrderService(IMapper mapper,IBasketRepository basketRepository,IUniteOfWork uniteOfWork)
        {
            this._mapper = mapper;
            this._basketRepository = basketRepository;
            this._uniteOfWork = uniteOfWork;
        }
        public async Task<Result<OrderToReturnDTO>> CreateOrder(string Email, OrderDTO orderDTO)
        {
            var orderAddress = _mapper.Map<OrderAddress>(orderDTO.Address);
            var Basket =await _basketRepository.GetBasketAsync(orderDTO.BasketId);
            if (Basket == null)
                return  Error.NotFound("Basket.NotFound", $"This Basket With Id {orderDTO.BasketId} is Not Found");
           
            List<OrderItem> Items=new List<OrderItem>();
            foreach (var item in Basket.items)
            {
                var product = await _uniteOfWork.GetRepositoryAsync<Product, int>().GetByIdAsync(item.Id);
                if (product == null) return Error.NotFound("Product.NotFound", $"This Basket With Id {item.Id} is Not Found");
                Items.Add( CreateOrderItem(product,item));
                
            }
            var DeliveryMethods =await _uniteOfWork.GetRepositoryAsync<DeliveryMethod, int>().GetByIdAsync(orderDTO.DeliveryMethod);
            if(DeliveryMethods== null) return Error.NotFound("DeliveryMethod.NotFound", $"This DeliveryMethod With Id {orderDTO.DeliveryMethod} is Not Found");

            var SubTotal = Items.Sum(I => I.Price * I.Quantity);
            var Order = new Order()
            {
                SubTotal = SubTotal,
                Address= orderAddress,
                DeliveryMethod=DeliveryMethods,
                Item= Items,
                UserEmail=Email


            };
          await   _uniteOfWork.GetRepositoryAsync<Order,Guid>().AddAsync(Order);
            var Result=await _uniteOfWork.SaveChangesAsync();
            if (Result == 0) return Error.Failuer("This Order Can No Creation", "Order.Failuer");
            return  _mapper.Map<OrderToReturnDTO>(Order);

        }

        private static OrderItem CreateOrderItem(Product product,BasketItem item)
        {
            return  new OrderItem()
            {
                Product = new ProductItemOrder() { PictureURL = product.PictureURL, ProductId = product.Id, ProductName = product.Name },
                Price = product.Price,
                Quantity = item.Qantity,
            };
        }
    }
}
