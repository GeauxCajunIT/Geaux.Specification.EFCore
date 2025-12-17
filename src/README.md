# Geaux.Specification.EntityFrameworkCore

Entity Framework Core integration for [Geaux.Specification](https://www.nuget.org/packages/Geaux.Specification).  
Provides evaluators and repository implementations that apply specifications directly to EF Core DbSets.

## ✨ Features

- **SpecificationEvaluator** for EF Core
- **DbSet extensions** to apply specifications
- **RepositoryBase** implementations using EF Core
- Full support for includes, ordering, search, pagination, and caching

## 📦 Installation

```bash
dotnet add package Geaux.Specification.EntityFrameworkCore
```

## 🚀 Example Usage
```csharp
var spec = new ActiveOrdersSpec();
var orders = await dbContext.Orders.WithSpecification(spec).ToListAsync();
```

## 🔗 Related Packages
Geaux.Specification — Specification pattern abstractions

Geaux.SharedKernal — Domain‑driven building blocks