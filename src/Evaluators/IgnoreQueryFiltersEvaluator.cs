// // <copyright file="" company="GeauxCajunIT">
// // Copyright (c) GeauxCajunIT. All rights reserved.
// // </copyright>

namespace Geaux.Specification.EfCore.Evaluators;

using Geaux.Specification.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq;

/// <summary>
/// This evaluator applies EF Core's IgnoreQueryFilters feature to a given query
/// See: https://docs.microsoft.com/en-us/ef/core/querying/filters
/// </summary>
public class IgnoreQueryFiltersEvaluator : IEvaluator
{
    private IgnoreQueryFiltersEvaluator()
    {
    }

    /// <summary>
    /// .
    /// </summary>
    public static IgnoreQueryFiltersEvaluator Instance { get; } = new IgnoreQueryFiltersEvaluator();

    /// <inheritdoc/>
    public bool IsCriteriaEvaluator { get; } = true;

    /// <inheritdoc/>
    public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
    {
        if (specification.IgnoreQueryFilters)
        {
            query = query.IgnoreQueryFilters();
        }

        return query;
    }
}
