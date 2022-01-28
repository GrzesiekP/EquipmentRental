using Marten;
using Orders.Aggregate;
using Orders.Projections;

namespace Orders.Config
{
    public static class OrderConfig
    {
        public static void ConfigureProjections(this StoreOptions options)
        {
            options.Projections.SelfAggregate<Order>();
            
            options.Projections.Add<OrderInfoProjection>();
        }
    }
}