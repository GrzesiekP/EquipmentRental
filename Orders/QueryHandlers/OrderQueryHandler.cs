using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Domain.Queries;
using Marten;
using Marten.Linq;
using Orders.Projections;
using Orders.Queries;

namespace Orders.QueryHandlers
{
    public class OrderQueryHandler: 
        IQueryHandler<GetOrders, IEnumerable<OrderInfo>>,
        IQueryHandler<GetOrder, OrderInfo>
    {
        private readonly IDocumentSession _querySession;

        public OrderQueryHandler(IDocumentSession querySession)
        {
            _querySession = querySession;
        }

        private IMartenQueryable<OrderInfo> Orders => _querySession.Query<OrderInfo>();

        public async Task<IEnumerable<OrderInfo>> Handle(GetOrders request, CancellationToken cancellationToken)
        {
            // Fails when user has no orders?
            return await Orders
                .Where(o => o.ClientEmail == request.ClientEmail)
                .ToListAsync(token: cancellationToken);
        }

        public async Task<OrderInfo> Handle(GetOrder request, CancellationToken cancellationToken)
        {
            return await Orders
                .FirstOrDefaultAsync(o => 
                        o.ClientEmail == request.ClientEmail && 
                        o.Id == request.OrderId, 
                    token: cancellationToken);
        }
    }
}