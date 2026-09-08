using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entity.Basket
{
    public class BasketItem
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string PictureURL { get; set; } = default!;
        public int Qantity { get; set; }
        public decimal Price { get; set; }


    }
}
