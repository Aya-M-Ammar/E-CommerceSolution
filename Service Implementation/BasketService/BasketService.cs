using AutoMapper;
using E_Commerce.Domain.Entity.BasketModel;
using E_Commerce.Domain.Interfaces.Repository;
using Service_Abstaction.BasketService;
using Service_Implementation.Exciptions;
using Shared.DTOS.BasketDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Service_Implementation.BasketService
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketService(IBasketRepository basketRepository,IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        public async Task<BasketDTO> CreateOrUpdateAsync(BasketDTO basket)
        {
            var BasketMap=_mapper.Map<BasketCustomer>(basket);
            var Basket=await _basketRepository.CreateOrUpdateBasketAsync(BasketMap);
            if(Basket == null)
            {
                throw new BaskeyNotFoundException(basket.Id);
            }
           return _mapper.Map<BasketDTO>(Basket);

        }

        public async Task<bool> DeleteBasket(string basketId)
        {
            var deleteBasket= await _basketRepository.DeleteBasketAsync(basketId);
            return deleteBasket;
        }

        public async Task<BasketDTO> GetBasketByIdAsync(string basketId)
        {
           var basket= await _basketRepository.GetBasketAsync(basketId);
            if(basket == null)
            {
              throw new  BaskeyNotFoundException(basketId);
            }
            return  _mapper.Map<BasketDTO>(basket);
        }
    }
}
