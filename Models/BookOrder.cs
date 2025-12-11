namespace BookStore2.Models
{
    public class BookOrder
    {
        public int OrderID { get; set; }
        public string CustomerName { get; set; }
        public string BookTitle { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
