
# ECommerceAPI

A RESTful e-commerce API built with **ASP.NET Core 8**, **Entity Framework Core**, and **PostgreSQL**. The project includes JWT authentication, role-based authorization, product management, shopping cart operations, order processing, and inventory control.

## Features

### Authentication & Authorization
- User registration and login
- JWT-based authentication
- Role-based authorization for administrators

### Product & Category Management
- Retrieve products and categories
- Administrator-only product management

### Shopping Cart
- Add products to the cart
- Update product quantities
- Remove individual items
- Clear the cart
- Calculate cart totals
- Checkout from the shopping cart

### Order Management
- Create orders from cart items
- Retrieve the authenticated user's order history
- Calculate order totals
- Store order items and unit prices

### Inventory Management
- Validate stock before checkout
- Update stock quantities when an order is placed
- Use database transactions to maintain data consistency

### API Documentation
- Interactive API documentation with Swagger / OpenAPI

## Technologies

- C#
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- ASP.NET Core Identity
- JWT Bearer Authentication
- FluentValidation
- Swagger / OpenAPI

## Project Architecture

The solution follows a layered architecture.

| Project | Responsibility |
|---|---|
| `ECommerceAPI.API` | Controllers, HTTP endpoints, authentication, and authorization |
| `ECommerceAPI.Application` | DTOs, service interfaces, and validation |
| `ECommerceAPI.Domain` | Core business entities |
| `ECommerceAPI.Infrastructure` | Database access and service implementations |

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/download/)
- [Git](https://git-scm.com/)

## Getting Started

### 1. Clone the Repository

```bash
git clone <YOUR_REPOSITORY_URL>
cd ECommerceAPI
```

Replace `<YOUR_REPOSITORY_URL>` with your GitHub repository URL.

### 2. Configure the Database

Create a PostgreSQL database named `ecommerce_db`.

Configure the connection string using .NET User Secrets or environment variables. Use the port configured for your PostgreSQL server.

Do not commit database passwords or JWT signing keys to source control.

### 3. Restore Dependencies

```bash
dotnet restore
```

### 4. Apply Database Migrations

```bash
dotnet ef database update \
  --project ECommerceAPI.Infrastructure \
  --startup-project ECommerceAPI.API
```

Ensure the Entity Framework Core tools are installed and the database connection is configured.

### 5. Build the Solution

```bash
dotnet build
```

### 6. Run the API

```bash
dotnet run --project ECommerceAPI.API
```

Open the Swagger URL displayed in the terminal to explore and test the API.

## Authentication

Protected endpoints require a valid JWT access token.

1. Register a user or log in.
2. Obtain the JWT token from the authentication response.
3. Click **Authorize** in Swagger.
4. Enter the token using the Bearer authentication configuration.
5. Send requests to protected endpoints.

Administrator-only operations require the appropriate role.

## Error Handling and Validation

The API handles common invalid operations, including:

- Invalid product quantities
- Non-existent products
- Empty shopping carts
- Insufficient stock
- Unauthorized access to protected endpoints

Checkout validates stock availability before updating inventory. Database transactions help prevent failed operations from leaving partial changes.

## Project Status

Core e-commerce API functionality has been implemented, including authentication, product and category management, shopping cart operations, order processing, and inventory updates.

## Future Improvements

- Automated unit and integration tests
- Product search, filtering, and pagination
- Refresh token support
- Docker support
- CI/CD pipeline
- Payment gateway integration
- Frontend application

## License

No license has been specified yet.
