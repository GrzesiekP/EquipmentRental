using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Domain.Queries;
using Marten;
using Orders.Projections;
using Orders.Queries;

namespace Orders.QueryHandlers
{
    public class GetOrderQueryHandler : IQueryHandler<GetOrders, IEnumerable<OrderInfo>>
    {
        private readonly IDocumentSession _querySession;

        public GetOrderQueryHandler(IDocumentSession querySession)
        {
            _querySession = querySession;
        }

        public async Task<IEnumerable<OrderInfo>> Handle(GetOrders request, CancellationToken cancellationToken)
        {
            // Fails when user has no orders?
            return await _querySession.Query<OrderInfo>()
                .Where(o => o.ClientEmail == request.ClientEmail)
                .ToListAsync(token: cancellationToken);
        }
    }
}