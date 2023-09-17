using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using mycartnow.productapi.Repository;

namespace mycartnow.productapi.Providers
{
    public interface IProductProvider
    {
        Task<IEnumerable<ProductResponse>> GetAllProductsAsync();
        Task<int> InsertProductAsync(ProductRequest productRequest);
    }

    public class ProductProvider : IProductProvider
    {
        private readonly string _connectionString;
        private readonly IProductRepository _repository;

        public ProductProvider(IConfiguration configuration, IProductRepository repository)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new ArgumentNullException(nameof(_connectionString), "Database connection string is missing or empty in appsettings.json.");
            }
            _repository = repository;

        }

        public async Task<IEnumerable<ProductResponse>> GetAllProductsAsync()
        {
           return await _repository.GetAllProductsAsync();
        }

        public async Task<int> InsertProductAsync(ProductRequest productRequest)
        {
            return  _repository.InsertProductAsync(productRequest);
        }

    }
}
