using Geaux.Specification.EfCore.Evaluators;
using Microsoft.EntityFrameworkCore;

namespace Geaux.Specification.EFCore.Tests
{
    public class AsNoTrackingEvaluatorTests
    {
        private class Order { public int Id { get; set; } }

        [Fact]
        public void GetQuery_ShouldApply_AsNoTracking()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase("NoTrackingDb").Options;
            using var context = new TestDbContext(options);

            context.Orders.Add(new Order { Id = 1 });
            context.SaveChanges();

            var spec = new TestSpec<Order> { AsNoTracking = true };
            IQueryable<T> query = AsNoTrackingEvaluator.Instance.GetQuery(context.Orders, spec);

            Assert.True(query is IQueryable<Order>);
            Assert.Equal(1, query.Count());
        }

        [Fact]
        public void GetQuery_ShouldApply_AsTracking()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase("TrackingDb").Options;
            using var context = new TestDbContext(options);

            context.Orders.Add(new Order { Id = 1 });
            context.SaveChanges();

            var spec = new TestSpec<Order> { AsTracking = true };
            IQueryable<T> query = AsTrackingEvaluator.Instance.GetQuery(context.Orders, spec);

            Assert.Equal(1, query.Count());
        }

        [Fact]
        public void GetQuery_ShouldApply_AsSplitQuery()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase("SplitQueryDb").Options;
            using var context = new TestDbContext(options);

            context.Orders.Add(new Order { Id = 1 });
            context.SaveChanges();

            var spec = new TestSpec<Order> { AsSplitQuery = true };
            IQueryable<T> query = AsSplitQueryEvaluator.Instance.GetQuery(context.Orders, spec);

            Assert.Equal(1, query.Count());
        }

    }
}
