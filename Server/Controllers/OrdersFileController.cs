using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

[ApiController]
[Route("api/orders")]

// Handles all orders request, Client in this app only uses the GET requests because this is the camp leader part of the program
// The kids will use the POST requests from another client to order meals
public class OrdersController : ControllerBase
{

    private static readonly List<OrderDto> _orders;
    private static int _nextId;

    static OrdersController()
    {
        _orders = LoadOrdersFromFile();
        SetIds(_orders);
    }

    private static List<OrderDto> LoadOrdersFromFile()
    {
        var path = "TestRequests/OrdersPopulate.http";
        var text = System.IO.File.ReadLines(path).Skip(3);
        var json_text = string.Join("\n", text);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<List<OrderDto>>(json_text,options)!; 

    }

    private static void SetIds(List<OrderDto> orders)
    {
        foreach (var o in orders)
        {
            o.Id = _nextId++;
        }
    }

    // Gets the list of all meals from the meals table
    [HttpGet("all")]
    public IEnumerable<OrderDto> GetAllOrders()
    {
        return _orders;
    }

    // Gets list of all orders of the given meal
    [HttpGet("meal/{id:int}")]
    public IEnumerable<OrderDto> GetMealOrders(int id)
    {
        return _orders.Where(order => order.MealId == id);
    }

    // Add new order to the orders
    [HttpPost("add")]
    public IActionResult AddNewOrder([FromBody] OrderDto orderDto)
    {
        orderDto.Id = _nextId++;
        _orders.Add(orderDto);
        return CreatedAtAction(nameof(GetAllOrders), orderDto);
    }

    // Adds a whole list of Orders
    [HttpPost("add-many")]
    public IActionResult AddMultipleOrders([FromBody] ICollection<OrderDto> orders)
    {
        foreach (var o in orders)
        {
            o.Id = _nextId++;
            _orders.Add(o);
        }
        return CreatedAtAction(nameof(GetAllOrders), orders);
    }

    // Deletes everything from the Orders table
    [HttpDelete("delete-all")]
    public IActionResult DeleteAllOrders()
    {
        _orders.Clear();
        return NoContent();
    }

}