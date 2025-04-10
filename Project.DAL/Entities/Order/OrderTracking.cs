using System.Runtime.Serialization;

namespace Project.DAL.Entities.Order
{
    public enum OrderTracking
    {
        [EnumMember(Value = "Received")]
        Received,

        [EnumMember(Value = "In Stock")]
        InStock,

        [EnumMember(Value = "Shipped")]
        Shipped
    }
}
