# ReqResUserService (.NET Contractor Assignment)

A clean, robust, and testable .NET 8 solution for interacting with the [ReqRes API](https://reqres.in), built as part of the RaftLabs .NET Developer Contractor Assignment.

---

## 🚀 Project Structure

- **ReqRes.Core**  
  Class Library containing:
  - HttpClient-based API client
  - Domain models (`User`)
  - Service logic (`ExternalUserService`)
  - Error handling and optional in-memory caching

- **ReqRes.ConsoleDemo**  
  Console app demonstrating use of the `ExternalUserService`.

- **ReqRes.Tests**  
  xUnit test project validating behavior of `ExternalUserService`.

---

## 📦 Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- Internet connection (for calling the public API)

---

## ⚙️ Setup & Run

```bash
git clone https://github.com/your-username/ReqResUserService.git
cd ReqResUserService

# Restore dependencies & build
dotnet build

# Run demo
dotnet run --project ReqRes.ConsoleDemo

# Run unit tests
dotnet test
