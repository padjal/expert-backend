using System.Collections.Generic;
using System.Threading;
using ExpertAdministration.Core.Models;
using ExpertAdministration.Server.Controllers;
using ExpertAdministration.Server.Exceptions;
using ExpertAdministration.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ExpertAdministration.Server.UnitTests.Controllers;

public class OffersControllerTests
{
    [Fact]
    public async void GetAllOffers_DatabaseIssue_InternalServerError()
    {
        //Assign
        var databaseService = new Mock<IDatabaseService>();
        var logger = new Mock<ILogger<OffersController>>();

        databaseService
            .Setup(service => service.GetAllOffersAsync(new CancellationToken(), 1000, "any"))
            .ReturnsAsync((List<Offer>)null);

        var controller = new OffersController(logger.Object, databaseService.Object);

        //Act
        var response = await controller.Get(new CancellationToken());

        var result = response.Result;

        //Assert
        Assert.IsType<StatusCodeResult>(result);
        var statusCode = ((StatusCodeResult)result).StatusCode;

        Assert.Equal(500, statusCode);
    }

    [Fact]
    public async void GetAllOffers_NormalState_ReturnOffers()
    {
        //Assign
        var databaseService = new Mock<IDatabaseService>();
        var logger = new Mock<ILogger<OffersController>>();

        var returnOfferList = new List<Offer>() { new Offer(), new Offer() };

        databaseService
            .Setup(service => service.GetAllOffersAsync(new CancellationToken(), 1000, "any"))
            .ReturnsAsync(returnOfferList);

        var controller = new OffersController(logger.Object, databaseService.Object);

        //Act
        var response = await controller.Get(new CancellationToken());

        var result = response.Result;

        //Assert
        Assert.IsType<OkObjectResult>(result);

        var resultObject = ((OkObjectResult)result).Value;
        Assert.IsType<List<Offer>>(resultObject);

        var offerList = (List<Offer>)resultObject;
        Assert.Equal(2, offerList.Count);
    }

    [Theory]
    [InlineData("dlskjfl")]
    [InlineData("")]
    [InlineData("jfkflfdfgfhk{")]
    [InlineData("jfhdjf342hsdfdafsdfadfadfabdnfde")]
    public async void GetOfferByIdAsync_IncorrectId_BadRequest(string offerId)
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
                     "given id is in the format ^\\w{20}$", badRequestContent);
    }

    [Fact]
    public async void GetOfferByIdAsync_IdNotPresentInDatabase_NotFound()
    {
        //Assign
        var offerId = "jfhjrksifj34ffdgtr4v"; //Matches the pattern ^\w{21}$
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
    public async void GetOfferByIdAsync_OfferDoesNotMatchSchema_InternalServerError()
    {
        //Assign
        var offerId = "jfhjrksifj34ffdgtr4v"; //Matches the pattern ^\w{21}$
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

    [Fact]
    public async void GetOfferByIdAsync_NormalState_ReturnFoundOffer()
    {
        //Assign
        var offerId = "jfhjrksifj34ffdgtr4v"; //Matches the pattern ^\w{21}$
        var databaseService = new Mock<IDatabaseService>();
        var logger = new Mock<ILogger<OffersController>>();

        databaseService
            .Setup(service => service.GetOfferAsync(offerId, new CancellationToken()))
            .ReturnsAsync(new Offer() { Id = offerId });

        var controller = new OffersController(logger.Object, databaseService.Object);

        //Act
        var response = await controller.Get(offerId, new CancellationToken());

        var result = response.Result;

        //Assert
        Assert.IsType<OkObjectResult>(result);

        var resultObject = ((OkObjectResult)result).Value;
        Assert.IsType<Offer>(resultObject);

        var offer = (Offer)resultObject;
        Assert.Equal(offerId, offer.Id);
    }
}