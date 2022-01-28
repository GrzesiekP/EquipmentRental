using System.Collections.Generic;
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
            return await _querySession.Query<OrderInfo>()
                .ToListAsync(token: cancellationToken);
        }
    }
}