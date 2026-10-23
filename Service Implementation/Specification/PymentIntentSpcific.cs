using E_Commerce.Domain.Entity.Order;
using Service_Abstaction.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service_Implementation.Specification
{
    internal class PymentIntentSpcific:BaseSpacification<Order,Guid>
    {
        public PymentIntentSpcific(string PymentIntentId):base(o=>o.PymentIntentId==PymentIntentId)
        {
            
        }

     
    }
}
