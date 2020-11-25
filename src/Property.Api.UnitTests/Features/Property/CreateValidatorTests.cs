using System;
using Property.Api.Features.Property;
using Xunit;
using FluentValidation;
using FluentValidation.TestHelper;

namespace Property.Api.UnitTests.Features.Property
{
    public class CreateValidatorTests
    {
        public CreateValidatorTests()
        {
        }

        [Fact]
        public void Should_Be_Valid_When_Reference_Starts_With_P()
        {
            string propertyReference = "P009";
            string propertyDescription = "This ia a test description";

            //Arrange
            var command = new CreateWithReferenceAndDescription.Command { PropertyReference = propertyReference, PropertyDescription = propertyDescription };
            //Act
            var sut = new CreateWithReferenceAndDescription.Validator();
            var result = sut.Validate(command);

            //Assert
            Assert.True(result.IsValid);
        }


        [Fact]
        public void Should_Not_Be_Valid_When_Reference_Does_Not_Start_With_P()
        {
            string propertyReference = "a009";
            string propertyDescription = "This ia a test description";

            //Arrange
            var command = new CreateWithReferenceAndDescription.Command { PropertyReference = propertyReference, PropertyDescription = propertyDescription };
            //Act
            var sut = new CreateWithReferenceAndDescription.Validator();
            var result = sut.Validate(command);

            //Assert
            Assert.False(result.IsValid);
        }
    }
}
