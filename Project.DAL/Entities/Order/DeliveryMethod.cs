namespace Project.DAL.Entities.Order
{
    public class DeliveryMethod : BaseEntity
    {
        public DeliveryMethod()
        {
        }

        public DeliveryMethod(string shortName, string description, decimal cost, string deliverTime)
        {
            ShortName = shortName;
            Description = description;
            Cost = cost;
            DeliverTime = deliverTime;
        }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public string DeliverTime { get; set; }



    }
}
