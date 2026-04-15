using BussnissLogicLayer.DTO;
using BussnissLogicLayer.ServiceContracts;
using DataAccessLayer.Entities;
using DataAccessLayer.RepositoryContracts;
using System.Linq.Expressions;

namespace BussnissLogicLayer.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IProductRepository _productRepository;

        public ProductsService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductResponse?> AddProduct(ProductAddRequest productAddRequest)
        {
            if (productAddRequest == null)
            {
                return null;
            }

            var productEntity = new Product
            {
                ProductID = Guid.NewGuid(),
                ProductName = productAddRequest.ProductName,
                Category = productAddRequest.Category.ToString(),
                UnitPrice = productAddRequest.UnitPrice.HasValue ? (decimal?)productAddRequest.UnitPrice.Value : null,
                QuantityInStock = productAddRequest.QuantityInStock
            };

            Product? addedProduct = await _productRepository.AddProduct(productEntity);

            return MapToProductResponse(addedProduct);
        }

        public async Task<bool> DeleteProduct(Guid productID)
        {
            return await _productRepository.DeleteProduct(productID);
        }

        public async Task<ProductResponse?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression)
        {
            Product? product = await _productRepository.GetProductByCondition(conditionExpression);

            return MapToProductResponse(product);
        }

        public async Task<List<ProductResponse?>> GetProducts()
        {
            IEnumerable<Product> products = await _productRepository.GetProducts();

            return products.Select(MapToProductResponse).ToList();
        }

        public async Task<List<ProductResponse?>> GetProductsByCondition(Expression<Func<Product, bool>> conditionExpression)
        {
            IEnumerable<Product?> products = await _productRepository.GetProductsByCondition(conditionExpression);

            return products.Select(MapToProductResponse).ToList();
        }

        public async Task<ProductResponse?> UpdateProduct(ProductUpdateRequest productUpdateRequest)
        {
            if (productUpdateRequest == null)
            {
                return null;
            }

            var productEntity = new Product
            {
                ProductID = productUpdateRequest.ProductID,
                ProductName = productUpdateRequest.ProductName,
                Category = productUpdateRequest.Category.ToString(),
                UnitPrice = productUpdateRequest.UnitPrice.HasValue ? (decimal?)productUpdateRequest.UnitPrice.Value : null,
                QuantityInStock = productUpdateRequest.QuantityInStock
            };

            Product? updatedProduct = await _productRepository.UpdateProduct(productEntity);

            return MapToProductResponse(updatedProduct);
        }

        private static ProductResponse? MapToProductResponse(Product? product)
        {
            if (product == null)
            {
                return null;
            }

            bool isValidCategory = Enum.TryParse(product.Category, true, out CategoryOptions category);

            return new ProductResponse
            {
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                Category = isValidCategory ? category : default,
                UnitPrice = product.UnitPrice.HasValue ? (double?)product.UnitPrice.Value : null,
                QuantityInStock = product.QuantityInStock
            };
        }
    }
}
