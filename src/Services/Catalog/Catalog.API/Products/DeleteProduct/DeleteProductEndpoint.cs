namespace Catalog.API.Products.DeleteProduct
{
    public record DeleteProductRequest(Guid Id);
    public record DeleteProductResponse(bool IsSuccess);
    public class DeleteProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products/{id}", (Guid id, ISender sender) =>
            {
                var result = sender.Send(new DeleteProductCommand(id));
                var response = result.Result.Adapt<DeleteProductResponse>();

                return Results.Ok(response);

            }).WithName("DeleteProduct")
              .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
              .ProducesProblem(StatusCodes.Status400BadRequest)
              .WithSummary("Delete Product")
              .WithDescription("Delete Product");
        }
    }
}
