using ApiDemo.Models;
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

        public ProductRepository(ProductContext context)
        {
            _context = context;
        }

        public IEnumerable<ProductItem> GetAllProducts()
        {
            try
            {
                return _context.ProductItems.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ProductItem> GetProduct(Guid id)
        {
            try
            {
                return await _context.ProductItems.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task CreateProduct(ProductItem item)
        {
            try
            {
                _context.ProductItems.Add(item);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ProductItem> UpdateProduct(ProductItem item)
        {
            try
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ProductItem> DeleteProduct(Guid id)
        {
            try
            {
                var product = await _context.ProductItems.FindAsync(id);

                if (product == null)
                    return null;

                _context.ProductItems.Remove(product);
                await _context.SaveChangesAsync();

                return product;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
