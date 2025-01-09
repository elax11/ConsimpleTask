namespace ConsimpleTask.EntityFramework.Entities
{
    public class Purchase
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }
        public int CustomerId { get; set; }
        public List<int> PurchaseItemsId { get; set; }
    }
}
