// // <copyright file="" company="GeauxCajunIT">
// // Copyright (c) GeauxCajunIT. All rights reserved.
// // </copyright>

namespace Geaux.Specification.EfCore.Evaluators;

using Geaux.Specification.Abstractions;
using System.Linq;


/// <summary>
/// .
/// </summary>
public class AsSplitQueryEvaluator : IEvaluator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AsSplitQueryEvaluator"/> class.
    /// </summary>
    private AsSplitQueryEvaluator()
    {
    }

    /// <summary>
    /// .
    /// </summary>
    public static AsSplitQueryEvaluator Instance { get; } = new AsSplitQueryEvaluator();

    /// <inheritdoc/>
    public bool IsCriteriaEvaluator { get; } = true;

    /// <inheritdoc/>
    public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
    {
        if (specification.AsSplitQuery && query is Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryable<T>)
        {
            //query = query.AsSplitQuery();
        }
        return query;
    }



}
