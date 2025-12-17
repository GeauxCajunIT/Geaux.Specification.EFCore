---
title: Geaux.Specification.EfCore
uid: Geaux.Specification.EfCore.index
---

 # Geaux.Specification.EntityFrameworkCore

Entity Framework Core integration for **Geaux.Specification**.  
This package provides EF-aware evaluators, repository base classes, and extensions so you can apply specifications directly to `DbSet<T>` and `IQueryable<T>`.

It is designed to be:

- Reusable outside the Geaux Platform
- Clean Architecture–friendly
- Ready for NuGet distribution

---

## ✨ Features

- 🧠 EF-aware `ISpecificationEvaluator` implementation
- 🔌 Extension methods for `DbSet<T>` / `IQueryable<T>`
- 📚 Base repository implementations for EF Core
- 🔍 Support for:
  - `Where`, `OrderBy`, `Pagination`
  - `Include` / `ThenInclude`
  - `AsNoTracking`, `AsTracking`
  - `IgnoreQueryFilters`
  - `AsSplitQuery`
  - `Search` expressions
- ⚙ Compatible with .NET 8 and .NET 9

---

## 📦 Installation

```bash
dotnet add package Geaux.Specification.EntityFrameworkCore

 You should also reference the core spec library:
 ```bash
dotnet add package Geaux.Specification
 ```

## 🚀 Getting Started
1. Register DbContext and evaluator

builder.Services.AddDbContext<MyAppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddSingleton<ISpecificationEvaluator>(SpecificationEvaluator.Cached);

2. Wire repositories

Using Geaux.SharedKernal abstractions:

builder.Services.AddScoped(typeof(IReadRepository<>), typeof(RepositoryBaseOfT<,>));
builder.Services.AddScoped(typeof(IRepository<>), typeof(RepositoryBaseOfT<,>));


(Adapt the type parameters to your actual RepositoryBaseOfT<TEntity, TContext> shape.)

3. Use specifications

public class ProductsByCategorySpec : Specification<Product>
{
    public ProductsByCategorySpec(int categoryId)
    {
        Query.Where(p => p.CategoryId == categoryId)
             .Include(p => p.Category)
             .OrderBy(p => p.Name)
             .AsNoTracking();
    }
}

 
Then in your handler/service:

```
var spec = new ProductsByCategorySpec(42);
var products = await _repository.ListAsync(spec, cancellationToken);


Under the hood, SpecificationEvaluator applies:

WhereEvaluator, OrderEvaluator, PaginationEvaluator

IncludeEvaluator (with cached reflection)

SearchEvaluator

AsNoTrackingEvaluator, AsTrackingEvaluator

IgnoreQueryFiltersEvaluator

AsSplitQueryEvaluator