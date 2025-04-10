using Project.DAL.Entities;
using Project.DAL.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services
{
    public interface IPaymentService
    {
        Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId);
        Task<Order> UpdatedPaymentIntentToSucceededOrFailed(string paymentIntentId, bool isSucceeded);
    }
}
