# ToolSnap — Backend API

ToolSnap is a backend service designed for managing, issuing, and returning tools within an enterprise.  
The system is built using **ASP.NET Core**, **Entity Framework Core**, **JWT authentication**, and follows clean, maintainable architectural principles.

---

## 🚀 Features
- User authentication (JWT)
- Tool issuing and returning workflows
- Photo documentation for tool operations
- Geolocation tracking linked to user actions
- Tool dictionary management (types, brands, models)
- Operation history and auditing
- REST API with Swagger/OpenAPI documentation

---

## 🛠 Tech Stack
- **ASP.NET Core 8 Web API**
- **Entity Framework Core**
- **MediatR**
- **FluentValidation**
- **JWT Authentication**
- **Swagger / OpenAPI**

### 🗄 Database
- **PostgreSQL**
- Runs inside **Docker** to ensure consistent development environments.

---

## 🐳 Docker + PostgreSQL

The backend uses PostgreSQL running inside a Docker container.

Example command to start a local database instance:

```bash
docker run --name toolsnap-db \
  -e POSTGRES_PASSWORD=yourpassword \
  -e POSTGRES_USER=postgres \
  -e POSTGRES_DB=toolsnap-db \
  -p 5432:5432 \
  -d postgres:latest
```

> Replace `yourpassword` with your local database password.  
> Real secrets should be stored via environment variables or Secret Manager, not inside source code.

---

## ▶️ Run the Project

```bash
dotnet restore
dotnet ef database update
dotnet run
```

Swagger UI:  
➡ **https://localhost:7062/swagger**

---

## 📁 Project Structure
- `Api/` — Controllers, DTOs, application configuration  
- `Application/` — CQRS logic, business processes, validation  
- `Domain/` — Domain entities and core rules  
- `Infrastructure/` — PostgreSQL context, EF Core migrations, repositories  

---

## 📄 License
This project is intended for learning, research, and demonstration purposes.
