// // <copyright file="" company="GeauxCajunIT">
// // Copyright (c) GeauxCajunIT. All rights reserved.
// // </copyright>

namespace Geaux.Specification.EfCore.Evaluators;

using Geaux.Specification.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq;

/// <summary>
/// .
/// </summary>
public class AsTrackingEvaluator : IEvaluator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AsTrackingEvaluator"/> class.
    /// </summary>
    private AsTrackingEvaluator()
    {
    }

    /// <summary>
    /// .
    /// </summary>
    public static AsTrackingEvaluator Instance { get; } = new AsTrackingEvaluator();

    /// <summary>
    /// .
    /// </summary>
    public bool IsCriteriaEvaluator { get; } = true;


    /// <inheritdoc/>
    public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
    {
        if (specification.AsTracking)
        {
            query = query.AsTracking();
        }

        return query;
    }
}
