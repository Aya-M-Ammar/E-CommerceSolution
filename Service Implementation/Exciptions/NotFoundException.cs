using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Implementation.Exciptions
{
    public abstract class NotFoundException(string Message):Exception(Message)
    {
    }
    public sealed class ProductNotFoundException(int Id):NotFoundException($"Product with Id {Id} was not found.")
    {
    }
    public sealed class BaskeyNotFoundException(string Id) : NotFoundException($"Basket with Id {Id} was not found.")
    {
    }
}
