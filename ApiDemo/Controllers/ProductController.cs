using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiDemo.Models;
using ApiDemo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ApiDemo.Controllers
{
    [Produces("application/json")]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                _logger.LogInformation("GetAll");
                var products = _productService.GetAllProducts();

                return Ok(products);
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAll failure");
                return StatusCode(StatusCodes.Status500InternalServerError, "Sorry something went wrong");
            }
        }

        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var product = await _productService.GetProduct(id);

                if (product == null)
                {
                    return NotFound();
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Sorry something went wrong");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]ProductItem item)
        {            
            if (!ValidateProduct(item))
                return BadRequest();

            try
            {
                await _productService.CreateProduct(item);

                return CreatedAtRoute("GetProduct", new { id = item.Id }, item);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Sorry something went wrong");
            }
        }

        [HttpPut("{id}")]
        public async Task <IActionResult> Update([FromBody]ProductItem item)
        {            
            try
            {
                if (!ValidateProduct(item))
                    return BadRequest();

                var product = await _productService.UpdateProduct(item);

                if (product == null)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Sorry something went wrong");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var item = await _productService.DeleteProduct(id);

                if (item == null)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Sorry something went wrong");
            }
        }

        private bool ValidateProduct(ProductItem item)
        {
            if (item.Id == null || 
                item.Id == Guid.Empty ||
                string.IsNullOrWhiteSpace(item.Name) || 
                item.Balance < 0)
                return false;

            return true;
        }
    }
}