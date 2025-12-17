// // <copyright file="" company="GeauxCajunIT">
// // Copyright (c) GeauxCajunIT. All rights reserved.
// // </copyright>

namespace Geaux.Specification.EfCore.Repositories;

using Geaux.Specification.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
// Alias the evaluator type so we don’t clash with the instance property name
using SpecificationEvaluatorType = Geaux.Specification.EfCore.Evaluators.SpecificationEvaluator;

/// <summary>
/// Default Entity Framework Core implementation of <see cref="IRepositoryBase{T}"/>.
/// Wraps a <see cref="DbContext"/> and applies <see cref="ISpecification{T}"/> instances
/// via an <see cref="ISpecificationEvaluator"/>.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
public class RepositoryBase<T> : IRepositoryBase<T>
    where T : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryBase{T}"/> class
    /// using the default <see cref="SpecificationEvaluator"/> instance.
    /// </summary>
    /// <param name="dbContext">The EF Core <see cref="DbContext"/> to operate on.</param>
    public RepositoryBase(DbContext dbContext)
        : this(dbContext, SpecificationEvaluatorType.Default)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryBase{T}"/> class
    /// using the specified <paramref name="specificationEvaluator"/>.
    /// </summary>
    /// <param name="dbContext">The EF Core <see cref="DbContext"/> to operate on.</param>
    /// <param name="specificationEvaluator">
    /// The evaluator used to apply specifications to queries.
    /// </param>
    public RepositoryBase(DbContext dbContext, ISpecificationEvaluator specificationEvaluator)
    {
        this.DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        this.SpecificationEvaluator = specificationEvaluator ?? throw new ArgumentNullException(nameof(specificationEvaluator));
    }

    /// <summary>
    /// Gets the underlying <see cref="DbContext"/>.
    /// </summary>
    protected DbContext DbContext { get; }

    /// <summary>
    /// Gets the specification evaluator used to apply specifications to queries.
    /// </summary>
    protected ISpecificationEvaluator SpecificationEvaluator { get; }

    /// <inheritdoc/>
    public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        await this.DbContext.Set<T>().AddAsync(entity, cancellationToken).ConfigureAwait(false);
        await this.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return entity;
    }

    // NOTE: explicit interface implementation to guarantee signature alignment
    Task<IEnumerable<T>> IRepositoryBase<T>.AddRangeAsync(
        IEnumerable<T> entities,
        CancellationToken cancellationToken)
        => this.AddRangeInternalAsync(entities, cancellationToken);

    /// <summary>
    /// Internal implementation of AddRangeAsync that does the actual work
    /// and returns the materialized entities.
    /// </summary>
    /// <param name="entities">The entities to add.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The added entities.</returns>
    protected virtual async Task<IEnumerable<T>> AddRangeInternalAsync(
        IEnumerable<T> entities,
        CancellationToken cancellationToken = default)
    {
        if (entities is null)
        {
            throw new ArgumentNullException(nameof(entities));
        }

        var entityList = entities.ToList();

        await this.DbContext.Set<T>().AddRangeAsync(entityList, cancellationToken).ConfigureAwait(false);
        await this.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return entityList;
    }

    /// <inheritdoc/>
    public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        this.DbContext.Set<T>().Update(entity);
        await this.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task UpdateRangeAsync(
        IEnumerable<T> entities,
        CancellationToken cancellationToken = default)
    {
        if (entities is null)
        {
            throw new ArgumentNullException(nameof(entities));
        }

        this.DbContext.Set<T>().UpdateRange(entities);
        await this.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        this.DbContext.Set<T>().Remove(entity);
        await this.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task DeleteRangeAsync(
        IEnumerable<T> entities,
        CancellationToken cancellationToken = default)
    {
        if (entities is null)
        {
            throw new ArgumentNullException(nameof(entities));
        }

        this.DbContext.Set<T>().RemoveRange(entities);
        await this.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<T?> GetByIdAsync<TId>(
        TId id,
        CancellationToken cancellationToken = default)
        where TId : notnull
    {
        return await this.DbContext.Set<T>()
            .FindAsync(new object[] { id }, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<T?> GetBySpecAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default)
    {
        return await this.ApplySpecification(specification)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<TResult?> GetBySpecAsync<TResult>(
        ISpecification<T, TResult> specification,
        CancellationToken cancellationToken = default)
    {
        return await this.ApplySpecification(specification)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<T?> FirstOrDefaultAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default)
    {
        return await this.ApplySpecification(specification)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<TResult?> FirstOrDefaultAsync<TResult>(
        ISpecification<T, TResult> specification,
        CancellationToken cancellationToken = default)
    {
        return await this.ApplySpecification(specification)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<T?> SingleOrDefaultAsync(
        ISingleResultSpecification<T> specification,
        CancellationToken cancellationToken = default)
    {
        return await this.ApplySpecification(specification)
            .SingleOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<TResult?> SingleOrDefaultAsync<TResult>(
        ISingleResultSpecification<T, TResult> specification,
        CancellationToken cancellationToken = default)
    {
        return await this.ApplySpecification(specification)
            .SingleOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<List<T>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await this.DbContext.Set<T>()
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<List<T>> ListAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default)
    {
        return await this.ApplySpecification(specification)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<List<TResult>> ListAsync<TResult>(
        ISpecification<T, TResult> specification,
        CancellationToken cancellationToken = default)
    {
        return await this.ApplySpecification(specification)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<int> CountAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default)
    {
        return await this.ApplySpecification(specification, evaluateCriteriaOnly: true)
            .CountAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await this.DbContext.Set<T>()
            .CountAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<bool> AnyAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default)
    {
        return await this.ApplySpecification(specification, evaluateCriteriaOnly: true)
            .AnyAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return await this.DbContext.Set<T>()
            .AnyAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual IAsyncEnumerable<T> AsAsyncEnumerable(ISpecification<T> specification)
    {
        return this.ApplySpecification(specification).AsAsyncEnumerable();
    }

    /// <summary>
    /// Applies a non-projected specification to the underlying set.
    /// </summary>
    protected virtual IQueryable<T> ApplySpecification(
        ISpecification<T> specification,
        bool evaluateCriteriaOnly = false)
    {
        if (specification is null)
        {
            throw new ArgumentNullException(nameof(specification));
        }

        var query = this.DbContext.Set<T>().AsQueryable();
        return this.SpecificationEvaluator.GetQuery(query, specification, evaluateCriteriaOnly);
    }

    /// <summary>
    /// Applies a projected specification to the underlying set.
    /// </summary>
    protected virtual IQueryable<TResult> ApplySpecification<TResult>(
        ISpecification<T, TResult> specification)
    {
        if (specification is null)
        {
            throw new ArgumentNullException(nameof(specification));
        }

        var query = this.DbContext.Set<T>().AsQueryable();
        return this.SpecificationEvaluator.GetQuery(query, specification);
    }

    /// <summary>
    /// Persists changes to the underlying <see cref="DbContext"/>.
    /// Overridable so derived repositories can plug in domain events, auditing, etc.
    /// </summary>
    protected virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return this.DbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task DeleteRangeAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        // You can implement this via a spec-based query + RemoveRange if you want.
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    Task<int> IRepositoryBase<T>.SaveChangesAsync(CancellationToken cancellationToken)
    {
        return this.SaveChangesAsync(cancellationToken);
    }
}
