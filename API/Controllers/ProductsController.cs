using System;
using System.Net.Mime;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController (IProductRepository repo) : ControllerBase
{
    
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand , string? type , string? sort)
    {
        return Ok(await repo.GetProductsAsync(brand,type,sort)); 
    }
    [HttpGet("{id:int}")] //api/product/4
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await repo.GetProductByIdAsync(id);
        if (product == null) return NotFound();
        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        repo.AddProduct(product);
        if (await repo.SaveChangesAsync())
        {
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }
        return BadRequest("problem creating product");

    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (product.Id != id || !ProductExist(id)) //check if the id param is same for product and check if product exists in database
            return BadRequest("Can not update this product");
        repo.UpdateProduct(product);
        if (await repo.SaveChangesAsync())
        {
            return NoContent();
        }
        return BadRequest("problem updating the product ");
    }
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await repo.GetProductByIdAsync(id);
        if (product == null) return NotFound();
        repo.DeleteProduct(product);

        if (await repo.SaveChangesAsync())
        {
            return NoContent();
        }
        return BadRequest("problem deleting the product ");
        
    }
    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        return Ok(await repo.GetBrandsAsync());
        
    }
     [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        return Ok(await repo.GetTypesAsync());
        
    }
    private bool ProductExist(int id)
    {
        return repo.ProductExists(id);
    }
}
