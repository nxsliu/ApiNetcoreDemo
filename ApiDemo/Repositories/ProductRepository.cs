using ApiDemo.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDemo.Repositories
{
    public interface IProductRepository
    {
        Task CreateProduct(ProductItem item);
        IEnumerable<ProductItem> GetAllProducts();
        Task<ProductItem> GetProduct(Guid id);
        Task<ProductItem> UpdateProduct(ProductItem item);
        Task<ProductItem> DeleteProduct(Guid id);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _context;
        private readonly ILogger _logger;

        public ProductRepository(ProductContext context, ILogger<ProductRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<ProductItem> GetAllProducts()
        {
            _logger.LogInformation("GetAllProducts");
            return _context.ProductItems.ToList();
        }

        public async Task<ProductItem> GetProduct(Guid id)
        {
            return await _context.ProductItems.FindAsync(id);
        }

        public async Task CreateProduct(ProductItem item)
        {
            _context.ProductItems.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task<ProductItem> UpdateProduct(ProductItem item)
        {
            var product = await _context.ProductItems.FindAsync(item.Id);

            if (product == null)
                return null;

            product.Balance = item.Balance;
            product.Name = item.Name;

            _context.ProductItems.Update(product);
            await _context.SaveChangesAsync();

            return product;
    }

        public async Task<ProductItem> DeleteProduct(Guid id)
        {
            var product = await _context.ProductItems.FindAsync(id);

            if (product == null)
                return null;

            _context.ProductItems.Remove(product);
            await _context.SaveChangesAsync();

            return product;
        }
    }
}
