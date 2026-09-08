using E_Commerce.Domain.Entity.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entity.BasketModel
{
    public class BasketCustomer
    {
        public string Id { get; set; } = default!;
        public ICollection<BasketItem> items { get; set; } = [];
    }
}
