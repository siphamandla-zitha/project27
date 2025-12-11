using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using BookStore2.Models;

namespace BookStore2.Controllers
{
    public class BookController : Controller
    {
        private readonly MySqlConnection _conn;

        public BookController(MySqlConnection conn)
        {
            _conn = conn;
        }

        // Show form
        public IActionResult Index()
        {
            return View();
        }

        // Handle POST
        [HttpPost]
        public IActionResult SubmitOrder(string customerName, string bookTitle, int quantity)
        {
            _conn.Open();

            string sql = @"INSERT INTO BookOrders (CustomerName, BookTitle, Quantity) 
                           VALUES (@CustomerName, @BookTitle, @Quantity)";

            MySqlCommand cmd = new MySqlCommand(sql, _conn);
            cmd.Parameters.AddWithValue("@CustomerName", customerName);
            cmd.Parameters.AddWithValue("@BookTitle", bookTitle);
            cmd.Parameters.AddWithValue("@Quantity", quantity);

            cmd.ExecuteNonQuery();
            _conn.Close();

            ViewBag.Message = "Order placed successfully!";
            return View("OrderSuccess");
        }

        public IActionResult OrderHistory()
        {
            List<BookOrder> orders = new List<BookOrder>();

            _conn.Open();

            string sql = "SELECT * FROM BookOrders ORDER BY OrderDate DESC";
            MySqlCommand cmd = new MySqlCommand(sql, _conn);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                orders.Add(new BookOrder
                {
                    OrderID = Convert.ToInt32(reader["OrderID"]),
                    CustomerName = reader["CustomerName"].ToString(),
                    BookTitle = reader["BookTitle"].ToString(),
                    Quantity = Convert.ToInt32(reader["Quantity"]),
                    OrderDate = Convert.ToDateTime(reader["OrderDate"])
                });
            }

            reader.Close();
            _conn.Close();

            return View(orders);
        }

    }
}
