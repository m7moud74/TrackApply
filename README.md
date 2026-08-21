# TrackApply

![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![Entity Framework Core](https://img.shields.io/badge/EF%20Core-512BD4?style=for-the-badge&logo=nuget&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
![Redis](https://img.shields.io/badge/Redis-DC382D?style=for-the-badge&logo=redis&logoColor=white)
![RabbitMQ](https://img.shields.io/badge/RabbitMQ-FF6600?style=for-the-badge&logo=rabbitmq&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)

## Overview

A small backend API for tracking job applications, their statuses, and interviews.

**This is a practice project, not a product.** The business domain is intentionally simple — its purpose is to apply and connect a set of backend concepts (CQRS, caching, async processing) in one working codebase, not to solve a real business problem.

## Tech Stack & Concepts

| Concept | Where it's used |
|---|---|
| **Minimal APIs** | All HTTP endpoints |
| **CQRS** | Commands and Queries are explicitly separated per feature |
| **EF Core** | Persistence layer |
| **FluentValidation** | Request validation |
| **Result Pattern** | Expected failures (not found, invalid state) are returned as `Result<T>`, not thrown as exceptions |
| **Redis** | Caching for read queries, with invalidation on writes |
| **`Channel<T>` + `BackgroundService`** | Asynchronous, in-process event processing (e.g. status-change notifications) |
| **`IServiceScopeFactory`** | Safely resolving scoped services (like `DbContext`) inside a singleton background worker |
| **RabbitMQ** | Durable, decoupled event processing — replaces the in-process `Channel<T>` so notification events survive a process restart |
| **Global Exception Middleware** | Consistent error responses via `ProblemDetails` |

## Domain

```
User
Company
JobApplication  (Status: Applied → Screening → Interview → Offer → Accepted / Rejected / Withdrawn)
Interview
```

## Architecture

Organized as **Vertical Slices**: each feature owns its Commands, Queries, handlers, and validators, instead of being split by technical layer (no shared "Services" or "Repositories" folder holding unrelated logic).

```
TrackApply/
├── Api/
│   ├── Endpoints/          → Minimal API endpoint definitions
│   ├── Middleware/         → Global exception handling
│   └── Workers/            → Hosted background services
├── App/
│   ├── Abstractions/       → Interfaces used by features (ICacheService, IEventPublisher...)
│   └── Features/
│       └── JobApplication/
│           ├── Commands/
│           │   ├── Create/
│           │   ├── Update/
│           │   └── Delete/
│           └── Queries/
│               └── Get/
├── Domain/
│   ├── Classes/            → Entities
│   ├── Enum/
│   ├── Events/              → e.g. ApplicationStatusChangedEvent
│   ├── Migrations/
│   └── Shared/              → Result<T>, Error
└── Infra/
    ├── Data/                → DbContext, EF Core configuration
    ├── Redis/               → ICacheService implementation
    └── RabbitMQ/            → Connection provider, message producer
```

## How a Status Change Flows Through the System

```
POST /applications/{id}/status
        │
        ▼
  UpdateApplicationHandler
        │
        ├──▶ EF Core: persist new status
        ├──▶ Redis: invalidate cached list
        └──▶ RabbitMQ: publish ApplicationStatusChangedEvent
                        │
                        ▼
              RabbitMqEmailWorker (Consumer)
                        │
                        ▼
              Simulated email notification (Ack on success, Nack + requeue on failure)
```

Events are durable: if the worker is offline when a status change happens, the message stays queued in RabbitMQ and is processed once the worker comes back — unlike an in-process `Channel<T>`, where pending messages are lost if the process restarts.

## Endpoints

| Method | Route | Description |
|---|---|---|
| POST | `/applications` | Create a new job application |
| GET | `/applications` | List all applications (cached) |
| GET | `/applications/{id}` | Get application by id |
| PUT | `/applications/{id}` | Update an application |
| DELETE | `/applications/{id}` | Delete an application |
| POST | `/applications/{id}/status` | Change application status |

## Running Locally

```bash
dotnet restore
dotnet ef database update
dotnet run
```

Redis is required for caching:

```bash
docker run -d -p 6379:6379 redis
```

RabbitMQ is required for status-change notifications:

```bash
docker run -d -p 5672:5672 -p 15672:15672 rabbitmq:management
```

Management UI available at `http://localhost:15672` (default guest/guest).

## What's Deliberately Out of Scope

- Authentication / Authorization
- Frontend / UI
- Microservices, Kubernetes, cloud deployment
- Full automated test coverage
- Outbox pattern / exactly-once delivery guarantees