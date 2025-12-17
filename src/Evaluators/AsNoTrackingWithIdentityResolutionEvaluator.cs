// // <copyright file="" company="GeauxCajunIT">
// // Copyright (c) GeauxCajunIT. All rights reserved.
// // </copyright>

using Geaux.Specification.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Geaux.Specification.EfCore.Evaluators;


/// <summary>
/// .
/// </summary>
public class AsNoTrackingWithIdentityResolutionEvaluator : IEvaluator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AsNoTrackingWithIdentityResolutionEvaluator"/> class.
    /// </summary>
    private AsNoTrackingWithIdentityResolutionEvaluator()
    {
    }

    /// <summary>
    /// .
    /// </summary>
    public static AsNoTrackingWithIdentityResolutionEvaluator Instance { get; } = new AsNoTrackingWithIdentityResolutionEvaluator();

    /// <inheritdoc/>
    public bool IsCriteriaEvaluator { get; } = true;


    /// <inheritdoc/>
    public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
    {
        if (specification.AsNoTrackingWithIdentityResolution)
        {
            query = query.AsNoTrackingWithIdentityResolution();
        }

        return query;
    }
}
