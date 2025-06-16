using System;
using Core.Entities;

namespace Core.Interfaces;

public interface IProductRepository
{
    //define all the methods that need to supprot inside the repo 
    Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type , string? sort);
    Task<Product?> GetProductByIdAsync(int id);

    Task<IReadOnlyList<string>> GetBrandsAsync();
    
    Task<IReadOnlyList<string>> GetTypesAsync();

    void AddProduct(Product product);
    void UpdateProduct(Product product);
    void DeleteProduct(Product product);
    bool ProductExists(int id);
    Task<bool> SaveChangesAsync();


}
