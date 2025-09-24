# DDD-CleanArchitecture

## Project Objective

This project is a production-ready Customer Relationship Management (CRM) system implementing Domain-Driven Design (DDD), Command Query Responsibility Segregation (CQRS), and Clean Architecture principles. This document outlines the technical specifications, architectural patterns, and implementation requirements.

## System Requirements

The system provides:

1. Complete Customer management functionality (CRUD operations)
2. Strict enforcement of business rules for data integrity
3. Event-driven architecture for system extensibility
4. Soft-deletion mechanism for data retention
5. Clean separation of concerns across architectural layers
6. RESTful API endpoints for all customer operations
7. Appropriate error handling and validation
8. Test covers Domain, Application, API

## Technical Requirements

### Technology Stack

- **Backend**:
  - .NET 9.0
  - Entity Framework Core 9.0
  - SQL Server
  - MediatR for CQRS implementation
  - FluentValidation for validation
  - xUnit, Moq, and FluentAssertions for testing

## Architecture Specification

The solution strictly adheres to a clean architecture with the following layers:

### 1. Core Layer

#### 1.1 Domain Layer

Implement the domain layer with the following components:

- **Aggregate Roots**:
  - Create a `Customer` aggregate root that inherits from a generic `AggregateRoot<TId>` base class
  - Implement soft-delete capability via `ISoftDelete` interface
  - Encapsulate all domain logic and state changes in the aggregate

- **Value Objects**:
  - Create strongly-typed, immutable value objects for all customer properties:
    - `FirstName`: String with appropriate validation
    - `LastName`: String with appropriate validation
    - `DateOfBirth`: Date type with age validation
    - `PhoneNumber`: Complex type with country code and number parts
    - `Email`: String with email format validation
    - `BankAccountNumber`: String with appropriate format validation
  - Implement `ValueObject` base class with equality comparison based on property values

- **Domain Events**:
  - Implement the following domain events:
    - `CustomerCreatedDomainEvent`
    - `CustomerUpdatedDomainEvent`
    - `CustomerDeletedDomainEvent`
    - `CustomerRestoredDomainEvent`
  - Events must capture all relevant state changes

- **Business Rules**:
  - Implement validation rules as separate classes adhering to `IBusinessRule` interface:
    - `CustomerEmailMustBeUniqueRule`: Ensures email uniqueness
    - `CustomerPersonalInfoMustBeUniqueRule`: Ensures no duplicate customers with same name and birth date
  - Rules must be checked during both creation and updates

- **Domain Services**:
  - Implement `ICustomerUniquenessCheckerService` for validating customer uniqueness constraints
  - Domain services must be abstracted through interfaces

#### 1.2 Application Layer

Implement the application layer with these components:

- **Commands**:
  - `CreateCustomerCommand`: Creates a new customer
  - `UpdateCustomerCommand`: Updates an existing customer
  - `DeleteCustomerCommand`: Soft-deletes a customer
  - `RestoreCustomerCommand`: Restores a soft-deleted customer
  - All commands must include appropriate validation

- **Command Handlers**:
  - Implement handlers for each command
  - Handlers must orchestrate domain operations and persistence

- **Queries**:
  - `GetCustomerByIdQuery`: Gets customer by ID
  - `GetCustomersListQuery`: Gets paginated list of customers
  - `GetCustomerByEmailQuery`: Gets customer by email

- **Query Handlers**:
  - Implement handlers for each query
  - Return appropriate DTO models, not domain entities

- **DTOs**:
  - Create data transfer objects for all API responses
  - DTOs must not expose domain implementation details

- **Behaviors**:
  - Implement cross-cutting concerns as MediatR behaviors:
    - `ValidationBehavior`: For FluentValidation integration
 
### 2. Infrastructure Layer

Implement the infrastructure layer with these components:

- **Persistence**:
  - Entity Framework Core DbContext
  - Repository implementations
  - Service implementations
  - Entity configurations and mappings
  - Migration scripts

### 3. Presentation Layer

Implement the presentation layer with these components:

- **API Controllers**:
  - RESTful endpoints for all customer operations
  - Proper status codes and error responses
  - API documentation via Swagger

## Implementation Requirements

### Customer Aggregate Implementation

The `Customer` aggregate has been implemented to:

1. Be implemented as a rich domain model with encapsulated business logic
2. Have private setters for all properties to enforce invariants
3. Use factory methods for creation with full validation
4. Implement the following public methods:
   - `Create`: Static factory method that validates and creates a new customer
   - `ChangeAttribute`: Updates customer properties with full validation
   - `Delete`: Implements soft-deletion
   - `Restore`: Reverts soft-deletion

Example signature for the `Create` method:

```csharp
public static Customer Create(
    FirstName firstName,
    LastName lastName,
    DateOfBirth dateOfBirth,
    PhoneNumber phoneNumber,
    Email email,
    BankAccountNumber bankAccountNumber,
    ICustomerUniquenessCheckerService customerUniquenessCheckerService
);
```

### Domain Events Implementation

Domain events are designed to:

1. Inherit from a common `DomainEventBase` class
2. Be raised during appropriate state changes in the aggregate
3. Contain all relevant information about the state change
4. Be handled by separate event handlers in the application layer

### Business Rules Implementation

Business rules have been implemented to:

1. Be implemented as separate classes implementing `IBusinessRule` interface
2. Have an `IsBroken()` method that returns a boolean
3. Provide a meaningful error message when broken
4. Be checked during domain operations using a `CheckRule` method

### CQRS Implementation

CQRS implementation in this project:

1. Strictly separate commands (write operations) from queries (read operations)
2. Use MediatR as the mediator pattern implementation
3. Implement commands as request objects with validators
4. Implement queries as request objects with handlers
5. Return Result objects from command handlers
6. Return DTOs from query handlers

### Validation Implementation

Validation implemented at multiple levels:

1. Domain-level validation through business rules
2. Application-level validation through FluentValidation
3. API-level validation through ModelState validation
4. Infrastructure-level validation through database constraints

## Testing Requirements

The project includes a comprehensive test suite covering:

1. **Unit Tests**:
   - Domain logic testing
   - Command and query handler testing
   - Validation testing

2. **Integration Tests**:
   - Repository testing
   - Service testing
   - API controller testing

3. **End-to-End Tests**:
   - Complete API flow testing
