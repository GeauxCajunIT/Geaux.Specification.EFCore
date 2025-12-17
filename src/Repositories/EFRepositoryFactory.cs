// // <copyright file="" company="GeauxCajunIT">
// // Copyright (c) GeauxCajunIT. All rights reserved.
// // </copyright>

namespace Geaux.Specification.EfCore.Repositories;

using Geaux.Specification.EfCore.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TRepository">The Interface of the repository created by this Factory</typeparam>
/// <typeparam name="TConcreteRepository">
/// The Concrete implementation of the repository interface to create
/// </typeparam>
/// <typeparam name="TContext">The DbContext derived class to support the concrete repository</typeparam>
public class EFRepositoryFactory<TRepository, TConcreteRepository, TContext> : IRepositoryFactory<TRepository>
  where TConcreteRepository : TRepository
  where TContext : DbContext
{
    private readonly IDbContextFactory<TContext> _dbContextFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="EFRepositoryFactory{TRepository, TConcreteRepository, TContext}"/> class.
    /// Initialises a new instance of the EFRepositoryFactory
    /// </summary>
    /// <param name="dbContextFactory">The IDbContextFactory to use to generate the TContext</param>
    public EFRepositoryFactory(IDbContextFactory<TContext> dbContextFactory)
    {
        this._dbContextFactory = dbContextFactory;
    }

    /// <inheritdoc />
    public TRepository CreateRepository()
    {
        object[] args = new object[] { _dbContextFactory.CreateDbContext() };
        return (TRepository)Activator.CreateInstance(typeof(TConcreteRepository), args)!;
    }
}
