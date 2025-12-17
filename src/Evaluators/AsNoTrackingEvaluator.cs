// // <copyright file="" company="GeauxCajunIT">
// // Copyright (c) GeauxCajunIT. All rights reserved.
// // </copyright>

using Geaux.Specification.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Geaux.Specification.EfCore.Evaluators
{

    /// <summary>
    /// AsNoTacking.
    /// </summary>
    public class AsNoTrackingEvaluator : IEvaluator
    {
        /// <inheritdoc/>
        private AsNoTrackingEvaluator()
        {
        }

        /// <inheritdoc/>
        public static AsNoTrackingEvaluator Instance { get; } = new AsNoTrackingEvaluator();


        /// <inheritdoc/>
        public bool IsCriteriaEvaluator { get; } = true;


        /// <inheritdoc/>
        public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
        {
            if (specification.AsNoTracking)
            {
                query = query.AsNoTracking();
            }

            return query;
        }
    }
}
