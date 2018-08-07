using ApiDemo.Controllers;
using ApiDemo.Models;
using ApiDemo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ApiDemo.UnitTests.Controllers
{
    public class ProductControllerTests
    {
        private IProductService _mockProductService;
        private ILogger<ProductController> _mockLogger;

        public ProductControllerTests()
        {
            _mockProductService = Substitute.For<IProductService>();
            _mockLogger = Substitute.For<ILogger<ProductController>>();
        }

        [Fact]
        public void GetAll_WhenUnhandledException_ShouldReturnInternalServerError()
        {
            // arrange
            _mockProductService.GetAllProducts().Returns(x => { throw new Exception(); });

            var controller = new ProductController(_mockProductService, _mockLogger);

            // act
            var result = controller.GetAll();

            // assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result).StatusCode);
            Assert.Equal("Sorry something went wrong", ((ObjectResult)result).Value);
        }

        [Fact]
        public void GetAll_WhenNoResults_ShouldReturnNotFound()
        {
            // arrange
            _mockProductService.GetAllProducts().Returns(new List<ProductItem>());

            var controller = new ProductController(_mockProductService, _mockLogger);

            // act
            var result = controller.GetAll();

            // assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetAll_WhenSuccessful_ShouldReturnOkListOfProducts()
        {
            // arrange
            var expectedList = new List<ProductItem>()
            {
                new ProductItem{ Id = Guid.NewGuid(), Name = "Clark Kent", Balance = 56.45F },
                new ProductItem{ Id = Guid.NewGuid(), Name = "Bruce Wayne", Balance = 6575.45F },
                new ProductItem{ Id = Guid.NewGuid(), Name = "James Howlett", Balance = 7.00F },
            };

            _mockProductService.GetAllProducts().Returns(expectedList);

            var controller = new ProductController(_mockProductService, _mockLogger);

            // act
            var result = controller.GetAll();

            // assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedList, ((OkObjectResult)result).Value);
        }

        [Fact]
        public async Task GetById_WhenIdNotExist_ShouldReturnNotFound()
        {
            // arrange
            _mockProductService.GetProduct(Arg.Any<Guid>()).Returns(Task.FromResult<ProductItem>(null));

            var controller = new ProductController(_mockProductService, _mockLogger);

            // act
            var result = await controller.GetById(Guid.NewGuid());

            // assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetById_WhenIdExists_ShouldReturnProduct()
        {
            // arrange
            var fakeProduct = new ProductItem { Id = Guid.NewGuid(), Name = "Clark Kent", Balance = 56.45F };
            _mockProductService.GetProduct(Arg.Any<Guid>()).Returns(Task.FromResult(fakeProduct));

            var controller = new ProductController(_mockProductService, _mockLogger);

            // act
            var result = await controller.GetById(Guid.NewGuid());

            // assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(fakeProduct, ((OkObjectResult)result).Value);
        }


        [Fact]
        public async Task GetById_WhenUnhandledException_ShouldReturnInternalServerError()
        {
            // arrange
            _mockProductService.GetProduct(Arg.Any<Guid>()).Returns(Task.FromException<ProductItem>(new Exception()));
            
            var controller = new ProductController(_mockProductService, _mockLogger);

            // act
            var result = await controller.GetById(Guid.NewGuid());

            // assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result).StatusCode);
            Assert.Equal("Sorry something went wrong", ((ObjectResult)result).Value);
        }
    }
}
