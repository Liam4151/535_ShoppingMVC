namespace SamsWarehouseWebApp.Models.DTO
{
    public class ItemCreateDTO
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public double UnitPrice { get; set; }
    }
}
