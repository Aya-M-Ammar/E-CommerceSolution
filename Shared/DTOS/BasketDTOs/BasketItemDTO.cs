using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.BasketDTOs
{
    public record BasketItemDTO
    (
          string Id= default!,
     string Name= default!,
     string PictureURL= default!,
     [Range (1, 100)]
     long Quantity= default!,
     [Range (1, (double)decimal.MaxValue)]
     decimal Price= default!
    );
}