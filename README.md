# Property Api

![CI](https://github.com/blakebyron/dotnetcore-propertyapi/workflows/CI/badge.svg)

This Api will allow basic searching and retrieval of property information. Sample data will be created for testing.

## Description

The purpose of this api is to combine a number of design concepts into an application. Those concepts are as follows:

- REST
  - Response Codes
  - Media Types
  - HATEOAS (Hypermedia as the Engine of Application State)
- OpenApi Documentation
  - Swagger UI
  - Customisation of Outputs by Swashbuckle
- Software Architecture and Patterns
  - CQRS
  - Mediator
  - Features Slices
- Automated Testing
  - Unit Testing
  - Integration Testing

## Projects

As the goal of this Api is to focus on desing concepts the number of projects being kept to the minimum.

- Property.Api: Main application containing the majority of the code
- Property.Core: Simplified domain objects
- Property.Api.UnitTests: Testing isolated components
- Property.Api.IntegrationTests: Testing multiple components in combination

## Entities

### Property

A property will consist of a unique reference, description and address including UPRN and UDPRN.
