using Day16_JWT_Role_Based_Authentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    static List<Product> products = new List<Product>()
    {
        new Product { Id = 1, Name = "Laptop", Price = 50000 }
    };

    [HttpGet]
    [Authorize]
    public IActionResult GetProducts()
    {
        return Ok(products);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult AddProduct(Product product)
    {
        products.Add(product);

        return Ok(product);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult UpdateProduct(int id, Product updatedProduct)
    {
        var product = products.FirstOrDefault(x => x.Id == id);

        if (product == null)
            return NotFound();

        product.Name = updatedProduct.Name;
        product.Price = updatedProduct.Price;

        return Ok(product);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult DeleteProduct(int id)
    {
        var product = products.FirstOrDefault(x => x.Id == id);

        if (product == null)
            return NotFound();

        products.Remove(product);

        return Ok("Deleted Successfully");
    }
}