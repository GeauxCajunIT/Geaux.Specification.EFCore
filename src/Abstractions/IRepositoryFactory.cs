// // <copyright file="" company="GeauxCajunIT">
// // Copyright (c) GeauxCajunIT. All rights reserved.
// // </copyright>

namespace Geaux.Specification.EfCore.Abstractions;

/// <summary>
/// Defines a factory for creating repository instances that encapsulate the
/// unit of work pattern for long-lived components (for example, Blazor components).
/// </summary>
/// <typeparam name="TRepository">
/// The repository interface type to create.
/// </typeparam>
public interface IRepositoryFactory<TRepository>
{
    /// <summary>
    /// Creates a new repository instance.
    /// </summary>
    /// <returns>
    /// A new instance of <typeparamref name="TRepository"/>.
    /// </returns>
    TRepository CreateRepository();
}
