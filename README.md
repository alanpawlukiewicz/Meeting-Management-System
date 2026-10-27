# Meeting Management System

Meeting Management System is a full-stack web application for planning meetings and keeping track of who can attend them. Users define their weekly availability, handle one-off exceptions, and respond to meeting invitations, while organizers and administrators manage the meetings themselves.

## Table of Contents

- [Features](#features)
- [Tech Stack](#tech-stack)
- [Architecture](#architecture)
- [Installation and Running](#installation-and-running)
- [Use of AI](#use-of-ai)

## Features

- **Registration and login**: fully working account registration and sign-in.
- **Role-based access control (RBAC)**: two roles, `Admin` and `Organizer`, with access to features and endpoints restricted by role.
- **Weekly availability**: each user sets the hours during which they are free in a typical week.
- **Availability exceptions**: on top of the weekly schedule, a user can mark additional free or busy time on a specific day. For example, if something comes up, they can block a particular time range on that one day without changing their regular weekly availability.
- **Meetings**: create, edit and delete meetings.
- **Attendance responses**: users can declare whether they will attend a meeting or not.
- **Account management**: edit account data (first name, last name, login, password) and delete the account.
- **Token-based authentication**: the whole authentication flow is based on JWT access tokens and refresh tokens.

## Tech Stack

| Layer        | Technology                                      |
| ------------ | ----------------------------------------------- |
| Backend      | ASP.NET Core (.NET 10), Entity Framework Core   |
| Frontend     | Angular 22, Angular Material , Tailwind CSS     |
| Database     | PostgreSQL 18                                   |
| Auth         | JWT access tokens + refresh tokens              |
| Deployment   | Docker, Docker Compose, nginx                   |

## Architecture

The backend is built following the principles of **Clean Architecture** and **SOLID**. It is split into four projects with dependencies pointing inwards, so the business logic does not depend on frameworks, the database or the web layer:

| Project                         | Responsibility                                                |
| ------------------------------- | ------------------------------------------------------------- |
| `MeetingManager.Domain`         | domain entities and repository contracts                      |
| `MeetingManager.Core`           | application logic  and services abstractions                  |
| `MeetingManager.Infrastructure` | database access (EF Core, migrations) and auth based services |
| `MeetingManager.WebApi`         | HTTP API, application startup                                 |

In the Docker setup, the frontend is served by nginx, which also proxies requests under `/api` to the backend, so the browser communicates with a single origin.

---

## Installation and Running

The application consists of three parts:

- **backend**: ASP.NET Core (.NET 10), projects `MeetingManager.Core`, `.Domain`, `.Infrastructure`, `.WebApi`
- **frontend**: Angular 22
- **database**: PostgreSQL 18

Entity Framework Core migrations are applied automatically when the backend starts, so there is no need to run them manually.

There are two ways to run the application: [with Docker](#option-1-docker-recommended) (the simplest) or [locally, without Docker](#option-2-without-docker).

---

### Option 1: Docker (recommended)

#### Requirements

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (Windows, macOS) or Docker Engine with the Docker Compose v2 plugin (Linux)

You do not need to install .NET, Node.js or PostgreSQL.

#### Steps

1. Clone the repository and go to its root directory:

   ```bash
   git clone https://github.com/alanpawlukiewicz/Meeting-Management-System.git
   cd Meeting-Management-System
   ```

2. Create a `.env` file in the root directory (next to `docker-compose.yml`) from the template:

   ```bash
   cp .env.example .env
   ```

   Fill in the values:

   | Variable            | Description                                                  |
   | ------------------- | ------------------------------------------------------------ |
   | `POSTGRES_DB`       | database name                                                |
   | `POSTGRES_USER`     | database user                                                |
   | `POSTGRES_PASSWORD` | database password                                            |
   | `JWT_SECRET`        | secret used to sign JWT tokens (at least 32 characters)      |
   | `FRONTEND_BASE_URL` | frontend origin used for CORS, `http://localhost` by default |

   You can generate a secure JWT secret with:

   ```bash
   openssl rand -base64 32
   ```

3. Build and start all containers:

   ```bash
   docker compose up --build
   ```

   The first build may take a few minutes.

4. Open the application in your browser: **http://localhost**

> If port 80 is already in use, change the port mapping in `docker-compose.yml` to `"8081:80"`
> and set `FRONTEND_BASE_URL=http://localhost:8081` in the `.env` file.

---

### Option 2: Without Docker

#### Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 24](https://nodejs.org/)
- [PostgreSQL 18](https://www.postgresql.org/download/)

#### 1. Database

Create a user and a database (e.g. using `psql`):

```sql
CREATE USER my_app WITH PASSWORD 'password';
CREATE DATABASE my_database OWNER my_app;
```

The tables will be created automatically by the migrations when the backend starts for the first time.

#### 2. Backend

1. Go to the backend directory:

   ```bash
   cd backend
   ```

2. Configure the connection string and the JWT secret. The most convenient way is [User Secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets), so the secrets never end up in the repository:

   ```bash
   dotnet user-secrets init --project MeetingManager.WebApi
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=my_database;Username=my_app;Password=your_password" --project MeetingManager.WebApi
   dotnet user-secrets set "Jwt:Secret" "<generated-secret>" --project MeetingManager.WebApi
   dotnet user-secrets set "FrontendBaseUrl" "http://localhost:4200" --project MeetingManager.WebApi
   ```

   Generate the secret with `openssl rand -base64 32`.

3. Start the API:

   ```bash
   dotnet run --project MeetingManager.WebApi
   ```

   Migrations are applied on the first start. The address the API listens on is shown in the logs (`Now listening on: ...`) and is defined in `MeetingManager.WebApi/Properties/launchSettings.json`.

#### 3. Frontend

1. In a second terminal, go to the frontend directory and install the dependencies:

   ```bash
   cd frontend
   npm install
   ```

2. Make sure the API address in `src/environments/environment.ts` points to the backend started in the previous step, e.g.:

   ```ts
   export const environment = {
     apiUrl: 'http://localhost:5285/api', // adjust to the address from the backend logs
   };
   ```

3. Start the development server:

   ```bash
   npm start
   ```

4. Open the application in your browser: **http://localhost:4200**

---

### Troubleshooting

| Problem                                         | Solution                                                                                              |
| ----------------------------------------------- | ----------------------------------------------------------------------------------------------------- |
| Backend exits on startup                        | Check the logs: usually a wrong connection string, a missing JWT secret or an unavailable database.   |
| CORS error in the browser console               | `FrontendBaseUrl` must exactly match the frontend origin (no trailing slash).                         |
| `port is already allocated` (Docker)            | Port 80 is in use, change the port mapping in `docker-compose.yml`.                                   |
| Changing the database password has no effect (Docker) | Postgres sets the password only on first initialization. Use `docker compose down -v` (deletes data). |

---

## Test Account

To try out the application, you can sign in with the default administrator account:

| Login   | Password            |
| ------- | ------------------- |
| `Admin` | `MeetingManager2026` |

> These credentials are intended for testing and demo purposes only. Change the password (or remove the account) before using the application in any real environment.

---

## Use of AI

I used Claude throughout the project, to a very different extent depending on the part:

- **Backend**: negligible use. The backend was written almost entirely by me.
- **Frontend**: AI wrote most of the Angular components (features folder) while I was responsible for services, models and authentication logic (core folder).
- **Docker**: all Docker-related files (Dockerfiles, `docker-compose.yml`, nginx configuration) were mostly generated with AI.
- **Favicon**: generated by AI.

The rest of the project was mostly written by me.