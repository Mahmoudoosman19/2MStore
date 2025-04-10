using Project.DAL.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Specifications.OrderSpecs
{
    public class OrderWithPaymentIntentSpecifications : BaseSpecification<Order>
    {
        public OrderWithPaymentIntentSpecifications(string paymentIntentId) : base(o => o.PaymentIntentId == paymentIntentId)
        {

        }
    }
}
