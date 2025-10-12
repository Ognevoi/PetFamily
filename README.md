# PetFamily 🐾

**PetFamily** is a comprehensive API application designed to help pets find loving homes. It provides a platform for volunteers to manage pet profiles, track their care, and facilitate adoptions. Built with **Clean Architecture** principles and **Domain-Driven Design (DDD)**, this project demonstrates modern .NET development practices with a focus on maintainability, testability, and scalability.

---

## 🚀 Features

### Core Functionality
- 🐕 **Pet Management**: Create and manage detailed pet profiles with photos, health information, and adoption status
- 👥 **Volunteer System**: Comprehensive volunteer management with experience tracking and assistance capabilities
- 🏷️ **Species & Breeds**: Organized categorization system for different animal types and breeds
- 📸 **Photo Management**: Upload and manage pet photos with cloud storage integration
- 🔍 **Advanced Search**: Filter pets by various criteria (species, breed, age, health status, etc.)
- 📊 **Pagination**: Efficient data loading with configurable page sizes

### Technical Features
- 🏗️ **Clean Architecture**: Separation of concerns with Domain, Application, Infrastructure, and API layers
- 🧪 **Comprehensive Testing**: Unit tests, integration tests, and acceptance tests
- 🐳 **Docker Support**: Containerized application with PostgreSQL, Seq logging, MinIO storage, and Redis
- 📝 **Structured Logging**: Advanced logging with Serilog and Seq integration
- ✅ **Data Seeding**: Automated database population with realistic test data
- 🔄 **CQRS Pattern**: Command Query Responsibility Segregation with MediatR
- 🛡️ **Validation**: FluentValidation for robust input validation
- ⚡ **Distributed Caching**: Redis caching for improved performance and scalability

---

## 🛠️ Tech Stack

### Backend
- **.NET 9** - Latest .NET framework
- **ASP.NET Core** - Web API framework
- **Entity Framework Core 9** - ORM with PostgreSQL provider
- **MediatR** - CQRS implementation
- **FluentValidation** - Input validation
- **Serilog** - Structured logging
- **CSharpFunctionalExtensions** - Functional programming utilities

### Database & Storage
- **PostgreSQL** - Primary database
- **MinIO** - Object storage for pet photos
- **Redis** - Distributed caching layer
- **Seq** - Log aggregation and analysis

### Testing
- **xUnit** - Unit testing framework
- **Reqnroll** - BDD testing with Gherkin
- **WebApplicationFactory** - Integration testing

### DevOps & Tools
- **Docker & Docker Compose** - Containerization
- **Swagger/OpenAPI** - API documentation
- **Entity Framework Migrations** - Database versioning

---

## 📂 Project Structure

```
PetFamily/
├── PetFamily.Backend/                                   # Main backend solution
│   ├── src/
│   │   ├── PetFamily.API/                               # Web API layer
│   │   │   ├── Controllers/                             # API endpoints
│   │   │   ├── DTO/Requests/                            # Request DTOs
│   │   │   ├── Extensions/                              # API extensions
│   │   │   ├── Middlewares/                             # Custom middlewares
│   │   │   └── Processors/                              # File processing
│   │   ├── PetFamily.Application/                       # Application layer
│   │   │   ├── Features/Volunteers/                     # Volunteer features
│   │   │   │   ├── Commands/                            # Command handlers
│   │   │   │   ├── Queries/                             # Query handlers
│   │   │   │   └── DTOs/                                # Data transfer objects
│   │   │   ├── Behaviors/                               # Cross-cutting concerns
│   │   │   ├── Database/                                # Database interfaces
│   │   │   └── Extensions/                              # Application extensions
│   │   ├── PetFamily.Domain/                            # Domain layer
│   │   │   ├── PetManagement/                           # Pet aggregate
│   │   │   │   ├── AggregateRoot/                       # Volunteer aggregate root
│   │   │   │   ├── Entities/                            # Pet entity
│   │   │   │   ├── ValueObjects/                        # Domain value objects
│   │   │   │   └── Enums/                               # Domain enums
│   │   │   ├── SpecieManagement/                        # Species aggregate
│   │   │   └── Shared/                                  # Shared domain concepts
│   │   └── PetFamily.Infrastructure/                    # Infrastructure layer
│   │       ├── Configurations/                          # EF Core configurations
│   │       ├── DbContexts/                              # Database contexts
│   │       ├── Repositories/                            # Data access implementations
│   │       ├── Seeding/                                 # Database seeding
│   │       └── Providers/                               # External service providers
│   └── tests/                                           # Test projects
│       ├── PetFamily.Domain.UnitTests/                  # Domain unit tests
│       ├── PetFamily.Application.IntegrationTests/      # Integration tests
│       ├── PetFamily.AcceptanceTests/                   # BDD acceptance tests
│       └── PetFamily.TestUtils/                         # Test utilities
├── docker-compose.yml                                   # Multi-container setup
└── README.md                                            # Project documentation
```

---

## ⚙️ Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [Git](https://git-scm.com/)

### Quick Start with Docker

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/PetFamily.git
   cd PetFamily
   ```

2. **Start the infrastructure services**
   ```bash
   docker-compose up -d
   ```
   This will start:
   - PostgreSQL database (port 5432)
   - Redis cache (port 6379)
   - Seq logging server (port 5346)
   - MinIO object storage (port 9001)

3. **Run the application with seeding**
   ```bash
   cd PetFamily.Backend
   dotnet run --project src/PetFamily.API -- --seeding
   ```

4. **Access the application**
   - API: `https://localhost:7115` or `http://localhost:5217`
   - Swagger UI: `https://localhost:7115/swagger`
   - Seq Logs: `http://localhost:5346`
   - MinIO Console: `http://localhost:9001`

### Development Setup

1. **Configure the database connection**
   ```bash
   # Update appsettings.Development.json with your database connection
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Port=5432;Database=pet_family;User Id=postgres;Password=postgres;",
       "Redis": "localhost:6379",
       "Seq": "http://localhost:5345"
     }
   }
   ```

2. **Run database migrations**
   ```bash
   cd PetFamily.Backend
   dotnet ef database update --project src/PetFamily.Infrastructure --startup-project src/PetFamily.API
   ```

3. **Run the application**
   ```bash
   dotnet run --project src/PetFamily.API
   ```

---

## 🧪 Testing

### Run All Tests
```bash
cd PetFamily.Backend
dotnet test
```

### Run Specific Test Projects
```bash
# Unit tests
dotnet test tests/PetFamily.Domain.UnitTests/

# Integration tests
dotnet test tests/PetFamily.Application.IntegrationTests/

# Acceptance tests
dotnet test tests/PetFamily.AcceptanceTests/
```

### Test Coverage
The project includes comprehensive test coverage:
- **Unit Tests**: Domain logic and value objects
- **Integration Tests**: API endpoints and database operations
- **Acceptance Tests**: End-to-end scenarios using BDD

---

## 📊 API Documentation

### Core Endpoints

#### Volunteers
- `GET /Volunteer` - Get paginated list of volunteers
- `GET /Volunteer/{id}` - Get volunteer by ID (includes pets)
- `POST /Volunteer` - Create new volunteer
- `PUT /Volunteer/{id}/main-info` - Update volunteer information
- `DELETE /Volunteer/{id}/soft` - Soft delete volunteer

#### Pets
- `GET /Volunteer/pets` - Get paginated list of pets
- `GET /Volunteer/pets/{id}` - Get pet by ID
- `POST /Volunteer/{volunteerId}/pet` - Add pet to volunteer
- `DELETE /Volunteer/{volunteerId}/pet/{petId}/hard` - Delete pet

#### Species
- `GET /Species` - Get all species with breeds

### Data Seeding
The application includes a comprehensive data seeding system that populates the database with:
- 5 different species (Dogs, Cats, Birds, Rabbits, Hamsters)
- 2-8 breeds per species
- 50 volunteers with realistic data
- 0-5 pets per volunteer
- Random health conditions, colors, and other attributes

---

## 🏗️ Architecture

### Clean Architecture Layers

1. **Domain Layer** (`PetFamily.Domain`)
   - Contains business logic and domain entities
   - Implements DDD patterns with aggregates, entities, and value objects
   - No external dependencies

2. **Application Layer** (`PetFamily.Application`)
   - Contains use cases and application logic
   - Implements CQRS with MediatR
   - Defines interfaces for external dependencies

3. **Infrastructure Layer** (`PetFamily.Infrastructure`)
   - Implements external concerns (database, file storage, etc.)
   - Contains EF Core configurations and repositories
   - Handles data seeding and migrations

4. **API Layer** (`PetFamily.API`)
   - Web API controllers and endpoints
   - Request/response DTOs
   - Middleware and configuration

### Key Design Patterns

- **Domain-Driven Design (DDD)**: Rich domain models with aggregates
- **CQRS**: Separate command and query responsibilities
- **Repository Pattern**: Data access abstraction
- **Unit of Work**: Transaction management
- **Value Objects**: Immutable domain concepts
- **Aggregate Pattern**: Consistency boundaries

---

## 🔧 Configuration

### Environment Variables
```bash
# Database
ConnectionStrings__DefaultConnection="Server=localhost;Port=5432;Database=pet_family;User Id=postgres;Password=postgres;"

# Redis
ConnectionStrings__Redis="localhost:6379"

# Logging
ConnectionStrings__Seq="http://localhost:5345"

# MinIO Storage
Minio__Endpoint="localhost:9000"
Minio__SSL="false"
```

### Seeding Configuration
Modify `SeedingConstants.cs` to adjust data generation:
```csharp
public const int SPECIES_COUNT = 5;                    // Number of species
public const int VOLUNTEERS_COUNT = 50;                // Number of volunteers
public const int PETS_PER_VOLUNTEER_MIN = 0;           // Min pets per volunteer
public const int PETS_PER_VOLUNTEER_MAX = 5;           // Max pets per volunteer
public const int BATCH_SIZE = 100;                     // Batch processing size
```

---

## 🚀 Deployment

### Docker Production Build
```bash
# Build the application
docker build -t petfamily-api -f PetFamily.Backend/src/PetFamily.API/Dockerfile .

# Run with production configuration
docker run -p 8080:8080 petfamily-api
```

### Environment-Specific Configuration
- **Development**: Uses `appsettings.Development.json`
- **Production**: Uses `appsettings.json` with environment variables
- **Docker**: Uses `docker-compose.yml` for multi-container setup

---

### Development Guidelines
- Follow Clean Architecture principles
- Write comprehensive tests for new features
- Use meaningful commit messages
- Ensure all tests pass before submitting PR

---

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.