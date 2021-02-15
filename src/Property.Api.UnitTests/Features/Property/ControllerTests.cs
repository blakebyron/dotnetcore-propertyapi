using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Property.Api.Features.Property;
using Xunit;

namespace Property.Api.UnitTests.Features.Property
{
    public class ControllerTests
    {
        private readonly Mock<IMediator> mediatorMock;

        public ControllerTests()
        {
            mediatorMock = new Mock<IMediator>();
        }

        [Fact]
        public void Given_NullValue_InConstructor()
        {
            //var sut = new PropertyController(mediatorMock.Object);

            Assert.Throws<ArgumentNullException>(() => new PropertyController(null));

        }

        [Fact]
        public async Task Given_A_Request_For_A_List_Then_Return_Success()
        {
            //Arrange
            var handlerResponse = new List.Result(new List<List.Result.Property>()
            {
                new List.Result.Property(){ AddressLine1 = "First Line of Address", Postcode = "AB12 4CD"}
            });
            mediatorMock.Setup(x => x.Send(It.IsAny<List.Query>(), default(CancellationToken))).Returns(Task.FromResult(handlerResponse));

            //Act
            var sut = new PropertyController(mediatorMock.Object);
            var actionResult = await sut.List(new List.Query()) as OkObjectResult;

            //Assert
            Assert.NotNull(actionResult);
            Assert.Equal(actionResult.StatusCode, (Int32)System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Given_A_Request_For_An_Individual_Property_When_ThePropertyExists_Then_ReturnSuccess()
        {
            //Arrange
            string knownPropertyRefernce = "P001";
            var handlerResponse = new Detail.Result() { PropertyReference = knownPropertyRefernce };
            mediatorMock.Setup(x => x.Send(It.IsAny<Detail.Query>(), default(CancellationToken))).Returns(Task.FromResult(handlerResponse));

            //Act
            var sut = new PropertyController(mediatorMock.Object);
            var actionResult = await sut.Detail(new Detail.Query() { PropertyReference = knownPropertyRefernce }) as OkObjectResult;

            //Assert
            Assert.NotNull(actionResult);
            Assert.Equal(actionResult.StatusCode, (Int32)System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Given_A_Request_For_An_Individual_Property_When_ThePropertyDoesNotExist_Then_ReturnNotFound()
        {
            //Arrange
            string unknownPropertyReference = "P009";
            var handlerResponse = new Detail.Result() { PropertyReference = unknownPropertyReference };
            mediatorMock.Setup(x => x.Send(It.IsAny<Detail.Query>(), default(CancellationToken))).Returns(Task.FromResult<Detail.Result>(null));

            //Act
            var sut = new PropertyController(mediatorMock.Object);
            var actionResult = await sut.Detail(new Detail.Query() { PropertyReference = unknownPropertyReference }) as NotFoundResult;

            //Assert
            Assert.NotNull(actionResult);
            Assert.Equal(actionResult.StatusCode, (Int32)System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Given_A_Request_To_Create_An_Individual_Property_When_ThePropertyIsValid_Then_ReturnCreated()
        {
            //Arrange
            string propertyReference = "P009";
            string propertyDescription = "This ia a test description";

            mediatorMock.Setup(x => x.Send(It.IsAny<CreateWithReferenceAndDescription.Command>(), default(CancellationToken))).Returns(Task.FromResult<Int32>(0));

            //Act
            var sut = new PropertyController(mediatorMock.Object);
            var actionResult = await sut.Create(new CreateWithReferenceAndDescription.Command() { PropertyReference = propertyReference, PropertyDescription = propertyDescription }) as CreatedAtActionResult;

            //Assert
            Assert.NotNull(actionResult);
            Assert.Equal(actionResult.StatusCode, (Int32)System.Net.HttpStatusCode.Created);
        }


    }
}
