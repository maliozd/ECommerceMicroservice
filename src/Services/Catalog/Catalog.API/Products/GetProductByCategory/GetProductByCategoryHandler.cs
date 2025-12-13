namespace Catalog.API.Products.GetProductByCategory;
public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;
public record GetProductByCategoryResult(IEnumerable<Product> Products);

internal class GetProductByCategoryQueryHandler(IDocumentSession documentSession) : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
{

    public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
    {
        var queryable = documentSession.Query<Product>().Where(x => x.Category.Contains(query.Category));
        var result = await queryable.ToListAsync(cancellationToken);

        return new GetProductByCategoryResult(result);
    }
}