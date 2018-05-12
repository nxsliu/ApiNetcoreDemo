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

namespace ApiDemo.UnitTests.Controllers
{
    public class ProductControllerTests
    {
        [Fact]
        public void GetAll_WhenUnhandledException_ShouldReturnInternalServerError()
        {
            // arrange
            var service = Substitute.For<IProductService>();
            var logger = Substitute.For<ILogger<ProductController>>();
            service.GetAllProducts().Returns(x => { throw new Exception(); });

            var controller = new ProductController(service, logger);

            // act
            var result = controller.GetAll();

            // assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result).StatusCode);
            Assert.Equal("Sorry something went wrong", ((ObjectResult)result).Value);
        }

        [Fact]
        public void GetAll_WhenSuccessful_ShouldReturnOkListOfProducts()
        {
            // arrange
            var service = Substitute.For<IProductService>();
            var logger = Substitute.For<ILogger<ProductController>>();

            var expectedList = new List<ProductItem>()
            {
                new ProductItem{ Id = Guid.NewGuid(), Name = "Clark Kent", Balance = 56.45F },
                new ProductItem{ Id = Guid.NewGuid(), Name = "Bruce Wayne", Balance = 6575.45F },
                new ProductItem{ Id = Guid.NewGuid(), Name = "James Howlett", Balance = 7.00F },
            };

            service.GetAllProducts().Returns(expectedList);

            var controller = new ProductController(service, logger);

            // act
            var result = controller.GetAll();

            // assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedList, ((OkObjectResult)result).Value);
        }
    }
}
