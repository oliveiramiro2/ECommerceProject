# 🚀 Enterprise E-Commerce Micro-Architecture (.NET 10, Clean Architecture & CQRS)

<p align="center">
  <img src="https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=.net&logoColor=white" />
  <img src="https://img.shields.io/badge/Architecture-Clean%20%2F%20DDD-007ACC?style=for-the-badge" />
  <img src="https://img.shields.io/badge/CQRS-MediatR-orange?style=for-the-badge" />
  <img src="https://img.shields.io/badge/Database-PostgreSQL-336791?style=for-the-badge&logo=postgresql&logoColor=white" />
  <img src="https://img.shields.io/badge/Tests-Testcontainers-2496ED?style=for-the-badge&logo=docker&logoColor=white" />
  <img src="https://img.shields.io/badge/License-MIT-green?style=for-the-badge" />
</p>

A production-grade, highly resilient E-commerce backend application designed to showcase senior-level software engineering practices, enterprise design patterns, and robust automated testing strategies in modern .NET.

---

## 🏗️ Architectural Overview

The solution adheres strictly to **Clean Architecture** principles, enforcing a strict separation of concerns where dependencies point inwards toward the Domain core.

```text
ECommerceProject/
│
├── 📂 ECommerce.Domain          # Core business models, Aggregates, Value Objects, Domain Events (Zero dependencies)
├── 📂 ECommerce.Application     # Use Cases, CQRS Handlers, MediatR Pipeline Behaviors, FluentValidation
├── 📂 ECommerce.Infrastructure  # EF Core, PostgreSQL mappings, Repositories, Optimistic Concurrency handling
├── 📂 ECommerce.API             # Minimal APIs, Global Exception Handling Middleware, Swagger
├── 📂 ECommerce.Tests.Unit      # Unit tests for domain entities and business rules (xUnit + FluentAssertions)
└── 📂 ECommerce.Tests.Integration # End-to-end integration tests using real PostgreSQL via Testcontainers
```

---

## 🎯 Technical Differentiators & Highlights

1. **Tactical Domain-Driven Design (DDD):**
   - Rich domain models with encapsulated state (`AggregateRoot`, `Entity`).
   - Immutable **Value Objects** (`Money`) to prevent primitive obsession and rounding errors.
   - Guarded invariants inside aggregate methods preventing invalid state mutations.

2. **CQRS with MediatR & Pipeline Behaviors:**
   - Strict segregation between Write commands (`IRequest<T>`) and Read queries.
   - Cross-cutting concerns handled gracefully via MediatR Pipeline Behaviors (e.g., automated validation via **FluentValidation**).

3. **Concurrency Control (Optimistic Concurrency):**
   - Built-in protection against race conditions (e.g., simultaneous checkout of the last available stock item).
   - Configured via EF Core concurrency tokens (`[Timestamp]`), throwing explicit conflict exceptions (`409 Conflict`) when concurrency collisions occur.

4. **Real Integration Testing with Testcontainers:**
   - Avoids unreliable in-memory databases. Tests spin up isolated, throwaway **PostgreSQL Docker containers** automatically via `Testcontainers`, execute migrations, validate endpoints, and tear down cleanly.

5. **Containerization Ready:**
   - Fully optimized multi-stage `Dockerfile` and automated service orchestration via `docker-compose`.

---

## 🛠️ Tech Stack

* **Language:** C# 12 / .NET 10
* **Architecture:** Clean Architecture, CQRS, Tactical DDD
* **Frameworks & Libraries:** MediatR, FluentValidation, Entity Framework Core
* **Database:** PostgreSQL
* **Testing:** xUnit, FluentAssertions, Testcontainers, Microsoft.AspNetCore.Mvc.Testing
* **DevOps:** Docker, Docker Compose

---

## 🚀 Getting Started (Run with Docker Compose)

The easiest way to run the application along with its database is using Docker Compose.

### Prerequisites
* [Docker Desktop](https://www.docker.com/products/docker-desktop/) installed and running.
* .NET SDK 10.0+ (if you wish to build or run tests locally).

### Running the Application
1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/ecommerce-dotnet.git
   cd ecommerce-dotnet
   ```

2. Spin up the containers (API + PostgreSQL):
   ```bash
   docker compose up --build
   ```

3. Open your browser and access the interactive Swagger documentation:
   👉 **`http://localhost:5080/swagger`**

---

## 🧪 Running Automated Tests

To execute the unit tests and real integration tests (which spin up the Testcontainers PostgreSQL instance):

```bash
dotnet test
```

---

## 📝 API Endpoints Example

### Create Order (`POST /api/orders`)
**Request Body:**
```json
{
  "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "items": [
    {
      "productId": "7b1b3697-8c35-42a1-9b57-613d7890c211",
      "productName": "Mechanical Keyboard RGB",
      "unitPrice": 350.00,
      "quantity": 1
    }
  ]
}
```

**Responses:**
* `201 Created`: Returns the generated order ID.
* `400 Bad Request`: Validation failure or business rule violation.
* `409 Conflict`: Optimistic concurrency conflict detected during stock/order processing.

---

## 📄 License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.