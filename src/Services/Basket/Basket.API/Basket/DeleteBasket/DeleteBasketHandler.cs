using BuildingBlocks.CQRS;

namespace Basket.API.Basket.DeleteBasket;

public record DeleteBasketCommand() : ICommand<DeleteBasketResult>;
public record DeleteBasketResult();
public class DeleteBasketHandler : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public Task<DeleteBasketResult> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
