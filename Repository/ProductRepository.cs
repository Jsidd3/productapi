
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace mycartnow.productapi.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductResponse>> GetAllProductsAsync();
        Task<int> CreateProductAsync(ProductRequest request);
    }
    public class ProductRepository : IProductRepository
    {

        private readonly string? _connectionString;

        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");

        }

        public async Task<IEnumerable<ProductResponse>> GetAllProductsAsync()
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            return await db.QueryAsync<ProductResponse>("SELECT * FROM Products");
        }

        public async Task<int> CreateProductAsync(ProductRequest request)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            const string query = "INSERT INTO Products (Name, Price) VALUES (@Name, @Price); SELECT CAST(SCOPE_IDENTITY() as int)";
            return await db.ExecuteScalarAsync<int>(query, request);
        }
    }
}

