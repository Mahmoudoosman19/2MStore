namespace Project.BLL.Dtos
{
    public class OrderDto
    {
        public string buyerEmail { get; set; }
        public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }
        public AddressDto ShipToAddress { get; set; }
    }
}
