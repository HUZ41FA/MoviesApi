# 🎬 MoviesApi – Layered Architecture REST API in ASP.NET Core

This is a demo **RESTful API** built with **ASP.NET Core**, following a classic **Layered Architecture**. It includes features like JWT authentication, caching, pagination, and a Refit-powered SDK for easy consumption.

> ⚠️ **Security Warning:** The JWT secret is **hardcoded** in `Identity.Api`, which is an **extremely bad practice**. This is only for demonstration purposes. Always secure secrets properly using environment variables or secret managers.

---

## 🧱 Project Structure

| Project                   | Description                                                                 |
|---------------------------|-----------------------------------------------------------------------------|
| `Identity.Api`            | A fake identity service to generate JWT tokens (**with hardcoded secret**)  |
| `Movies.Api`              | Presentation layer exposing RESTful endpoints                              |
| `Movies.Application`      | Core business logic: services, repositories, database access                |
| `Movies.Contracts`        | Contracts shared across API and application layers                         |
| `Movies.Api.Sdk`          | Refit-based SDK to interact with the Movies API *(not yet pushed)*          |
| `Movies.Api.Sdk.Consumer` | Consumer project that uses the SDK *(not yet pushed)*                       |

---

## ✨ Features

- ✅ **Layered Architecture**
- 🔐 **JWT Authentication** (`/token` endpoint)
- 💾 **In-Memory Output Caching**
- 🗃️ **PostgreSQL Database**
- ⚙️ **Dapper** for data access
- 🔍 **Filtering, Pagination, and Sorting**
- 📘 **Detailed Swagger Documentation**
- 📡 **API Versioning**
- 🩺 **Health Checks Endpoint**
- 🔌 **Refit SDK Integration** (WIP)

---

## 📚 API Documentation

Access interactive API documentation at:  
`/swagger/index.html`

---

## 🔐 Authentication

To interact with protected endpoints, you must obtain a JWT token:

**POST** `/token`  
> Returns a JWT token for use in authenticated requests.

⚠️ The secret key used to generate JWTs is **hardcoded** in `Identity.Api`. This is insecure and should never be done in real-world applications. Always use environment-based secret management.

---

## 🎬 Movie Endpoints

| Method | Endpoint                           | Description                            |
|--------|------------------------------------|----------------------------------------|
| POST   | `/api/movies`                      | Create a new movie                     |
| GET    | `/api/movies/{idOrSlug}`           | Get movie by ID or slug                |
| GET    | `/api/movies`                      | Get all movies (with pagination, filtering, sorting) |
| PUT    | `/api/movies/{id}`                 | Update a movie by ID                   |
| DELETE | `/api/movies/{id}`                 | Delete a movie by ID                   |

---

## ⭐ Ratings Endpoints

| Method | Endpoint                           | Description                            |
|--------|------------------------------------|----------------------------------------|
| PUT    | `/api/movies/{id}/ratings`         | Rate a movie                           |
| GET    | `/api/ratings/me`                  | Get movies rated by the current user   |
| DELETE | `/api/movies/{id}/ratings`         | Remove rating for a specific movie     |

---

## 🩺 Health Checks

The API exposes a health check endpoint to monitor service status:

`/_health`  

---

## 📦 Repository

GitHub Link: [HUZ41FA/MoviesApi](https://github.com/HUZ41FA/MoviesApi)  
> The SDK and SDK consumer projects will be pushed soon.

---

## 📌 Notes

- JWT secret is **hardcoded** in `Identity.Api`, which is an **extremely bad practice**. Use secure secret storage mechanisms in real applications.
