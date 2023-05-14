using System.Threading;
using System.Threading.Tasks;
using ExpertAdministration.Core.Models;
using ExpertAdministration.Server.Controllers;
using ExpertAdministration.Server.Exceptions;
using ExpertAdministration.Server.Interfaces;
using Google.Rpc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ExpertAdministration.Server.UnitTests.Controllers;

public class OffersControllerTests
{
    [Theory]
    [InlineData("dlskjfl")]
    [InlineData("")]
    [InlineData("jfkflfdfgfhk{")]
    [InlineData("jfhdjf342hsdfbdnfde")]
    public async void GetOfferAsync_IncorrectId_BadRequest(string offerId)
    {
        //Assign
        var databaseService = new Mock<IDatabaseService>();
        var logger = new Mock<ILogger<OffersController>>();
        var controller = new OffersController(logger.Object, databaseService.Object);
        
        //Act
        var response = await controller.Get(offerId, new CancellationToken());

        var result = response.Result;

        //Assert
        Assert.IsType<BadRequestObjectResult>(result);
        var badRequestContent = ((BadRequestObjectResult)result).Value;
        
        Assert.Equal("Id does not match schema. Please verify that the " +
                     "given id is in the format \"\\w{13}\"", badRequestContent);
    }

    [Fact]
    public async void GetOfferAsync_IdNotPresentInDatabase_NotFound()
    {
        //Assign
        var offerId = "jfhjrksifj34f"; //Matches the pattern ^\w{13}$
        var databaseService = new Mock<IDatabaseService>();
        var logger = new Mock<ILogger<OffersController>>();

        databaseService
            .Setup(service => service.GetOfferAsync(offerId, new CancellationToken()))
            .ThrowsAsync(new IdNotFoundException(offerId, "Could not find specified offer in database."));
        var controller = new OffersController(logger.Object, databaseService.Object);
        
        //Act
        var response = await controller.Get(offerId, new CancellationToken());

        var result = response.Result;

        //Assert
        Assert.IsType<NotFoundObjectResult>(result);
        var badRequestContent = ((NotFoundObjectResult)result).Value;
        
        Assert.Equal($"Could not find specified offer in database.", badRequestContent);
    }

    [Fact]
    public async void GetOfferAsync_OfferDoesNotMatchSchema_Null()
    {
        //Assign
        var offerId = "jfhjrksifj34f"; //Matches the pattern ^\w{13}$
        var databaseService = new Mock<IDatabaseService>();
        var logger = new Mock<ILogger<OffersController>>();

        databaseService
            .Setup(service => service.GetOfferAsync(offerId, new CancellationToken()))
            .ReturnsAsync((Offer)null);
        
        var controller = new OffersController(logger.Object, databaseService.Object);
        
        //Act
        var response = await controller.Get(offerId, new CancellationToken());

        var result = response.Result;

        //Assert
        Assert.IsType<StatusCodeResult>(result);
        var statusCode = ((StatusCodeResult)result).StatusCode;
        
        Assert.Equal(500, statusCode);
    }
}