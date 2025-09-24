# DRB Task Solution - Route Scheduling System

## 📋 Overview

A comprehensive route scheduling system built with .NET 8 and ASP.NET Core Web API. This system manages drivers, routes, and scheduling operations with a clean architecture approach following Domain-Driven Design (DDD) principles.

## 🏗️ Architecture

The solution follows **Clean Architecture** with the following layers:

### 📁 Project Structure

```
DRB-Task Solution/
├── 🔧 Core/
│   ├── Domain/                    # Domain entities, contracts, and business rules
│   ├── Service.Abstraction/       # Service interfaces
│   └── Service.Implementation/    # Business logic implementation
├── 🏗️ Infrastructure/
│   ├── Persistence/               # Data access layer (EF Core, Repositories)
│   └── Presentation/              # API controllers and presentation logic
├── 📦 Shared/                     # Shared DTOs, models, and utilities
└── 🚀 DRB-Task.API/              # Web API startup project
```

## 🛠️ Technologies Used

- **.NET 8** - Latest .NET framework
- **ASP.NET Core Web API** - RESTful API development
- **Entity Framework Core** - ORM for data persistence
- **SQL Server** - Database management system
- **AutoMapper** - Object-to-object mapping
- **Swagger/OpenAPI** - API documentation
- **Clean Architecture** - Architectural pattern
- **Repository Pattern** - Data access abstraction
- **Specification Pattern** - Query logic encapsulation

## 🚀 Features

### 👥 Driver Management
- ✅ Add new drivers to the system
- 📊 View driver history and assigned routes
- 🔍 Track driver availability status

### 🛣️ Route Management
- ➕ Create and manage routes
- 📋 List routes with pagination and filtering
- 🔄 Update route status (Pending, Assigned, Active, Completed, Unassigned)
- 🔍 Search and sort routes by various criteria

### 📅 Schedule Operations
- 🎯 Assign drivers to routes
- ⚡ Real-time availability checking
- 📈 Schedule optimization
- 🔄 Route status management

## 🗂️ API Endpoints

### Drivers
```http
POST /api/drivers/add              # Add a new driver
GET  /api/drivers/{id}/history     # Get driver's route history
```

### Routes
```http
POST /api/routes/add               # Create a new route
GET  /api/routes                   # Get paginated routes with filtering
```

### Schedule
```http
POST /api/schedule                 # Schedule a driver to a route
```

## 💾 Database Schema

The system uses Entity Framework Core with the following main entities:

- **Driver** - Driver information and availability
- **Route** - Route details and status
- **RouteHistory** - Historical records of driver-route assignments

## 🔧 Configuration

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or SQL Server instance)
- Visual Studio 2022 or VS Code

### Database Setup
1. Update the connection string in `appsettings.json`
2. Run Entity Framework migrations:
   ```bash
   dotnet ef database update --project Infrastructure/Persistence --startup-project DRB-Task.API
   ```

### Running the Application
1. **Using Visual Studio:**
   - Open `DRB-Task Solution.sln`
   - Set `DRB-Task.API` as startup project
   - Press F5 or click Run

2. **Using Command Line:**
   ```bash
   cd DRB-Task.API
   dotnet run
   ```

3. **Access the API:**
   - Swagger URL: `http://192.168.1.7/swagger`

## 📋 Data Transfer Objects (DTOs)

### Request DTOs
- `CreatedDriverDto` - Driver creation
- `CreatedRouteDto` - Route creation
- `ScheduleOperationDto` - Schedule assignment

### Response DTOs
- `RouteDto` - Route information
- `DriverHistoryDto` - Driver history
- `HistoryRecordDto` - Individual history record

### Utility DTOs
- `PaginatedResult<T>` - Paginated responses
- `CreatedResult` - Creation confirmation
- `RouteQueryParams` - Route filtering parameters

## 🔍 Query Features

### Route Filtering
- **Status filtering:** Pending, Assigned, Active, Completed, Unassigned
- **Sorting options:** Name, Date, Status
- **Pagination:** Page size and number control
- **Search:** Route name and description search

## 🏛️ Design Patterns

1. **Repository Pattern** - Data access abstraction
2. **Unit of Work Pattern** - Transaction management
3. **Specification Pattern** - Complex query logic
4. **Service Layer Pattern** - Business logic encapsulation
5. **Dependency Injection** - Loose coupling and testability

## 🧪 Exception Handling

Custom exception hierarchy for better error management:
- `BusinessLogicException` - Business rule violations
- `ConflictException` - Data conflicts
- `NotFoundException` - Resource not found
- `NotAllowedException` - Operation not permitted

## 📊 Logging & Monitoring

- Built-in ASP.NET Core logging
- Structured logging support
- Exception middleware for centralized error handling

## 🔒 Security Considerations

- Input validation through DTOs
- Model binding protection
- SQL injection prevention through EF Core
- CORS configuration for cross-origin requests

## 🚀 Development Guidelines

### Code Organization
- Follow Clean Architecture principles
- Separate concerns across layers
- Use dependency injection for loose coupling
- Implement proper error handling

### Database Operations
- Use Entity Framework Core for data access
- Implement repository pattern for data abstraction
- Use specifications for complex queries
- Handle transactions through Unit of Work

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is part of the DRB internship task and is intended for educational and evaluation purposes.

## 👨‍💻 Author

**Mohamed Saber**
- GitHub: [@MohamedSaber2004](https://github.com/MohamedSaber2004) 

---

*Built with ❤️ using .NET 8 and Clean Architecture principles*