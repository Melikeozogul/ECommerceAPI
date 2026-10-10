
# ECommerceAPI

A RESTful e-commerce API built with **ASP.NET Core 8**, **Entity Framework Core**, and **PostgreSQL**. The project includes JWT authentication, role-based authorization, shopping cart management, order processing, and inventory control.

## Features

- **Authentication & Authorization**
  - User registration and login
  - JWT-based authentication
  - Role-based access control for administrators

- **Product & Category Management**
  - Retrieve products and categories
  - Admin-only product management

- **Shopping Cart**
  - Add products to the cart
  - Update product quantities
  - Remove individual items
  - Clear the cart
  - Calculate cart totals

- **Order Management**
  - Create orders from the shopping cart
  - Retrieve the authenticated user's orders
  - Calculate order totals

- **Inventory Management**
  - Validate stock before order creation
  - Update stock quantities when an order is placed
  - Use database transactions to maintain data consistency

- **API Documentation**
  - Interactive API testing with Swagger / OpenAPI

## Technologies

- C#
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- ASP.NET Core Identity
- JWT Bearer Authentication
- Swagger / OpenAPI
- Layered Architecture

## Project Architecture

The solution follows a layered architecture to separate responsibilities and improve maintainability.

| Project | Responsibility |
|---|---|
| `ECommerceAPI.API` | HTTP endpoints, controllers, authentication, and authorization |
| `ECommerceAPI.Application` | DTOs and service interfaces |
| `ECommerceAPI.Domain` | Core business entities |
| `ECommerceAPI.Infrastructure` | Database access and service implementations |

## Prerequisites

Make sure you have the following installed:

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

Configure the database connection string securely using .NET user secrets or another secure configuration method.

Do not commit database passwords, JWT secrets, or other sensitive credentials to source control.

### 3. Apply Database Migrations

Run the following command from the solution directory:

```bash
dotnet ef database update \
  --project ECommerceAPI.Infrastructure \
  --startup-project ECommerceAPI.API
```

Make sure the Entity Framework Core tools are installed and the database connection is configured correctly.

### 4. Restore Dependencies

```bash
dotnet restore
```

### 5. Build the Solution

```bash
dotnet build
```

### 6. Run the API

```bash
dotnet run --project ECommerceAPI.API
```

Open the Swagger URL displayed in the terminal to explore and test the available endpoints.

## Authentication

Protected endpoints require a valid JWT access token.

1. Register a user or log in.
2. Obtain the JWT token from the authentication response.
3. Click **Authorize** in Swagger.
4. Enter the token using the format expected by the Swagger configuration.
5. Send requests to protected endpoints.

Administrator-only operations require the appropriate role.

## Error Handling and Validation

The API validates common invalid operations, including:

- Invalid product quantities
- Non-existent products
- Empty shopping carts
- Insufficient stock
- Unauthorized access to protected endpoints

Order creation validates stock before updating inventory. Database transactions help prevent failed operations from leaving partial database changes.

## Project Status

The project is under active development. Core e-commerce API functionality has been implemented, including authentication, product management, shopping carts, order processing, and inventory control.

**Note:** Payment gateway integration and a frontend application are not currently included.

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
