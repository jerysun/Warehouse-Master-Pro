using MediatR;
using Core.IRepositories;
using Core.Models;

namespace Core.UseCases;
public class DecreaseStocks
{
    public class Command : IRequest<int>
    {
        public int ProductId { get; set; }
        public int Amount { get; set; }
    }

    public class Handler : IRequestHandler<Command, int>
    {
        private readonly IRepository<int, Product> _repository;

        public Handler(IRepository<int, Product> repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(Command request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.ProductId, cancellationToken);
            if (product == null)
            {
                throw new ProductNotFoundException(request.ProductId);
            }
            product.DecreaseStock(request.Amount);
            await _repository.UpdateAsync(product, cancellationToken);
            return product.QuantityInStock;
        }
    }
}
