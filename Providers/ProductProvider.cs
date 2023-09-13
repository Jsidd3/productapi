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
    }

    public class ProductProvider : IProductProvider
    {
        private readonly string _connectionString;

        public ProductProvider(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new ArgumentNullException(nameof(_connectionString), "Database connection string is missing or empty in appsettings.json.");
            }
        }

        public async Task<IEnumerable<ProductResponse>> GetAllProductsAsync()
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            var products = await db.QueryAsync<ProductResponse>("SELECT * FROM Products");
            return products;
        }
    }
}
