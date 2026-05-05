using BussnissLogicLayer.DTO;
using BussnissLogicLayer.ServiceContracts;

namespace eCommerce.ProductsMicroService.API.APIEndpoints;

public static class ProductAPIEndpoints
{
    public static IEndpointRouteBuilder MapProductAPIEndpoints(this IEndpointRouteBuilder app)
    {
        //GET /api/products
        app.MapGet("/api/products", async (IProductsService productsService) =>
        {
            List<ProductResponse?> products = await productsService.GetProducts();
            return Results.Ok(products);
        });


        //GET /api/products/search/product-id/00000000-0000-0000-0000-000000000000
        app.MapGet("/api/products/search/product-id/{ProductID:guid}", async (IProductsService productsService, Guid ProductID) =>
        {
            ProductResponse? product = await productsService.GetProductByCondition(temp => temp.ProductID == ProductID);

            if (product == null)
            {
                return Results.NotFound($"Product with ID '{ProductID}' not found.");
            }

            return Results.Ok(product);
        });


        //GET /api/products/search/product-name/{ProductName}
        app.MapGet("/api/products/search/product-name/{ProductName}", async (IProductsService productsService, string ProductName) =>
        {
            List<ProductResponse?> products = await productsService.GetProductsByCondition(temp => temp.ProductName.Contains(ProductName));
            return Results.Ok(products);
        });


        //GET /api/products/search/category/{Category}
        app.MapGet("/api/products/search/category/{Category}", async (IProductsService productsService, string Category) =>
        {
            bool isValidCategory = Enum.TryParse<CategoryOptions>(Category, true, out CategoryOptions categoryOption);

            if (!isValidCategory)
            {
                return Results.BadRequest($"Invalid category '{Category}'.");
            }

            List<ProductResponse?> products = await productsService.GetProductsByCondition(temp => temp.Category == categoryOption.ToString());
            if (products.Count == 0)
            {
                return Results.NotFound($"No products found in category '{Category}'.");
            }
            return Results.Ok(products);
        });


        //POST /api/products
        app.MapPost("/api/products", async (IProductsService productsService, ProductAddRequest productAddRequest) =>
        {
            ProductResponse? addedProduct = await productsService.AddProduct(productAddRequest);

            if (addedProduct == null)
            {
                return Results.BadRequest("Product could not be created.");
            }

            return Results.Created($"/api/products/search/product-id/{addedProduct.ProductID}", addedProduct);
        });


        //PUT /api/products/{ProductID}
        app.MapPut("/api/products/{ProductID:guid}", async (IProductsService productsService, Guid ProductID, ProductUpdateRequest productUpdateRequest) =>
        {
            if (ProductID != productUpdateRequest.ProductID)
            {
                return Results.BadRequest("Route ProductID and body ProductID must match.");
            }

            ProductResponse? updatedProduct = await productsService.UpdateProduct(productUpdateRequest);

            if (updatedProduct == null)
            {
                return Results.NotFound($"Product with ID '{ProductID}' not found.");
            }

            return Results.Ok(updatedProduct);
        });


        //DELETE /api/products/{ProductID}
        app.MapDelete("/api/products/{ProductID:guid}", async (IProductsService productsService, Guid ProductID) =>
        {
            bool isDeleted = await productsService.DeleteProduct(ProductID);

            if (!isDeleted)
            {
                return Results.NotFound($"Product with ID '{ProductID}' not found.");
            }

            return Results.NoContent();
        });

        return app;
    }
}