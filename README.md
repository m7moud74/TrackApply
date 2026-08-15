# Job Application Tracker API

A small backend API for tracking job applications, their statuses, and interviews. Built as a training/practice project — not intended for production use.

![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![Entity Framework Core](https://img.shields.io/badge/EF%20Core-512BD4?style=for-the-badge&logo=nuget&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
![Redis](https://img.shields.io/badge/Redis-DC382D?style=for-the-badge&logo=redis&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)

## Tech Stack & Concepts

| Concept | الاستخدام |
|---|---|
| **Minimal APIs** | كل الـ Endpoints |
| **CQRS** | فصل Commands عن Queries بشكل صريح |
| **EF Core** | الـ Persistence Layer |
| **FluentValidation** | التحقق من صحة الـ Requests |
| **Result Pattern** | التعامل مع الأخطاء المتوقعة (Not Found, Validation...) من غير Exceptions |
| **Redis** | Caching للـ Read Queries + Cache Invalidation عند أي تعديل |
| **Channel\<T\> + BackgroundService** | معالجة غير متزامنة (Async Processing) لأحداث زي تغيير حالة الطلب |
| **ProblemDetails** | شكل موحد للأخطاء عبر الـ API |

## Domain

```
User
Company
JobApplication (Status: Applied → Screening → Interview → Offer → Accepted/Rejected/Withdrawn)
Interview
```

## Project Structure

Organized as **Vertical Slices** — each feature owns its own Commands/Queries, handlers, and validators, instead of splitting by technical layer.

```
JobTracker/
├── Data/               → DbContext, EF Core configuration
├── Domain/
│   └── Enum/           → ApplicationStatus and other enums
├── EndPoint/
│   └── JobApplication/ → Minimal API endpoint definitions
├── Features/
│   └── JobApplication/
│       ├── Commands/
│       │   ├── Create/
│       │   ├── Update/
│       │   └── Delete/
│       └── Queries/
│           └── Get/
└── Migrations/         → EF Core migrations
```

## Endpoints

| Method | Route | Description |
|---|---|---|
| POST | /applications | Create a new job application |
| GET | /applications | List all applications |
| GET | /applications/{id} | Get application by id |
| PUT | /applications/{id} | Update an application |
| DELETE | /applications/{id} | Delete an application |
| POST | /applications/{id}/status | Change application status |

## Running Locally

```bash
dotnet restore
dotnet ef database update
dotnet run
```

Redis required for caching (via Docker):
```bash
docker run -d -p 6379:6379 redis
```
