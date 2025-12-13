namespace Catalog.API.Products.GetProductByCategory;
public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;
public record GetProductByCategoryResult(IEnumerable<Product> Products);

internal class GetProductByCategoryQueryHandler(IDocumentSession documentSession, ILogger<GetProductByCategoryQueryHandler> logger) : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
{

    public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetProductByCategoryQueryHandler.Handle called with {@Query}", query);
        var queryable = documentSession.Query<Product>().Where(x => x.Category.Contains(query.Category));
        var result = await queryable.ToListAsync(cancellationToken);

        return new GetProductByCategoryResult(result);
    }
}