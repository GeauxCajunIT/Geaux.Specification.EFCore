// // <copyright file="" company="GeauxCajunIT">
// // Copyright (c) GeauxCajunIT. All rights reserved.
// // </copyright>

namespace Geaux.Specification.EfCore.Evaluators;

using Geaux.Specification.Abstractions;
using Geaux.Specification.EfCore.Extensions;
using Geaux.Specification.Expressions;
using System.Linq;

/// <summary>
/// .
/// </summary>
public class SearchEvaluator : IEvaluator
{
    private SearchEvaluator()
    {
    }

    /// <summary>
    /// .
    /// </summary>
    public static SearchEvaluator Instance { get; } = new SearchEvaluator();

    /// <inheritdoc/>
    public bool IsCriteriaEvaluator { get; } = true;

    /// <inheritdoc/>
    public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
    {
        foreach (IGrouping<int, SearchExpressionInfo<T>> searchCriteria in specification.SearchCriterias.GroupBy(x => x.SearchGroup))
        {
            query = query.Search(searchCriteria);
        }

        return query;
    }
}
