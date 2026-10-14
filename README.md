
# ECommerceAPI

A RESTful e-commerce API built with **ASP.NET Core 8**, **Entity Framework Core**, and **PostgreSQL**. The project provides JWT-based authentication, role-based authorization, product and category management, shopping cart operations, order processing, and inventory control.

## Features

### Authentication and Authorization
- User registration and login
- JWT-based authentication
- Role-based authorization for administrator operations

### Product and Category Management
- Retrieve products and categories
- Create, update, and delete products
- Administrator-only product management

### Shopping Cart
- Add products to the shopping cart
- Update product quantities
- Remove individual items
- Clear the shopping cart
- Calculate cart totals
- Create orders from cart items

### Order Management
- Create orders from shopping cart items
- Retrieve the authenticated user's order history
- Calculate order totals
- Store order items and unit prices at the time of purchase

### Inventory Management
- Validate stock availability before checkout
- Update stock quantities when an order is placed
- Use database transactions to help maintain data consistency

### API Documentation
- Explore and test API endpoints using Swagger / OpenAPI

### Docker Support
- Run the API inside a Docker container
- Manage application configuration with Docker Compose
- Configure the database connection and JWT signing key through environment variables

## Technologies

- **Language:** C#
- **Framework:** .NET 8
- **Backend:** ASP.NET Core Web API
- **ORM:** Entity Framework Core
- **Database:** PostgreSQL
- **Identity Management:** ASP.NET Core Identity
- **Authentication:** JWT Bearer Authentication
- **Validation:** FluentValidation
- **API Documentation:** Swagger / OpenAPI
- **Containerization:** Docker and Docker Compose
- **Version Control:** Git and GitHub

## Project Architecture

The solution follows a layered architecture.

| Project | Responsibility |
|---|---|
| `ECommerceAPI.API` | Controllers, HTTP endpoints, authentication, and authorization |
| `ECommerceAPI.Application` | DTOs, service interfaces, and validation rules |
| `ECommerceAPI.Domain` | Core business entities and domain models |
| `ECommerceAPI.Infrastructure` | Database access and service implementations |

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/download/)
- [Git](https://git-scm.com/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (optional, for containerized deployment)

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/Melikeozogul/ECommerceAPI.git
cd ECommerceAPI
```

### 2. Configure the Database

Create a PostgreSQL database named `ecommerce_db`.

Configure the database connection string according to your local PostgreSQL setup. If your PostgreSQL server uses port `5433`, make sure the connection string uses the same port.

For local development, sensitive configuration values can be managed with .NET User Secrets or environment variables.

**Security note:** Never commit database passwords or JWT signing keys to source control.

### 3. Restore Dependencies

```bash
dotnet restore
```

### 4. Apply Database Migrations

Make sure the Entity Framework Core tools are installed and the database connection is configured correctly.

```bash
dotnet ef database update \
  --project ECommerceAPI.Infrastructure \
  --startup-project ECommerceAPI.API
```

### 5. Build the Solution

```bash
dotnet build
```

### 6. Run the API

```bash
dotnet run --project ECommerceAPI.API
```

Open the Swagger URL displayed in the terminal to explore and test the API endpoints.

## Running with Docker

Make sure Docker Desktop is installed and running.

Run the following commands from the project root directory.

### 1. Build the Docker Image

```bash
docker compose build
```

### 2. Start the Container

```bash
docker compose up -d
```

### 3. Open Swagger

Visit the following URL in your browser:

http://localhost:8080/swagger

The Docker Compose configuration uses environment variables to configure the API and its database connection. Make sure the required `JWT_KEY` variable is defined in your local `.env` file before starting the container.

**Important:** The `.env` file contains sensitive configuration and must not be committed to GitHub. Never include your actual JWT signing key in this README.

### 4. Stop the Container

```bash
docker compose down
```

This command stops and removes the Compose containers. It does not delete the separately running PostgreSQL database.

## Authentication

Protected endpoints require a valid JWT access token.

1. Register a user or log in.
2. Obtain the JWT token from the authentication response.
3. Click **Authorize** in Swagger.
4. Enter the token according to the Bearer authentication configuration.
5. Send requests to protected endpoints.

Administrator-only operations require the appropriate user role.

## Error Handling and Validation

The API includes validation and handling for common invalid operations, such as:

- Invalid product quantities
- Non-existent products
- Empty shopping carts
- Insufficient stock
- Unauthorized access to protected endpoints

Checkout validates stock availability before updating inventory. Database transactions help prevent failed operations from leaving partial changes.

## Project Status

The project includes core e-commerce API functionality, including authentication, product and category management, shopping cart operations, order processing, and inventory updates.

The API can also be run using Docker Compose.

## Future Improvements

- Automated unit and integration tests
- Product search, filtering, and pagination
- Refresh token support
- CI/CD pipeline
- Payment gateway integration
- Frontend application

## License

No license has been specified yet.
