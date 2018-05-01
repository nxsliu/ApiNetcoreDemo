using ApiDemo.Models;
using ApiDemo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDemo.Services
{
    public interface IProductService
    {
        Task CreateProduct(ProductItem item);
        IEnumerable<ProductItem> GetAllProducts();
        Task<ProductItem> GetProduct(Guid id);
        Task<ProductItem> UpdateProduct(ProductItem item);
        Task<ProductItem> DeleteProduct(Guid id);
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IEnumerable<ProductItem> GetAllProducts()
        {
            return _productRepository.GetAllProducts();
        }

        public async Task<ProductItem> GetProduct(Guid id)
        {
            return await _productRepository.GetProduct(id);
        }

        public async Task CreateProduct(ProductItem item)
        {
            await _productRepository.CreateProduct(item);
        }

        public async Task<ProductItem> UpdateProduct(ProductItem item)
        {
            return await _productRepository.UpdateProduct(item);
        }

        public async Task<ProductItem> DeleteProduct(Guid id)
        {
            return await _productRepository.DeleteProduct(id);
        }
    }
}
