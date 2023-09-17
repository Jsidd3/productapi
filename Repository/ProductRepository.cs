using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace mycartnow.productapi.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductResponse>> GetAllProductsAsync();
        int InsertProductAsync(ProductRequest productRequest);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<ProductResponse>> GetAllProductsAsync()
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            return await db.QueryAsync<ProductResponse>("SELECT * FROM Product");
        }
        public int InsertProductAsync(ProductRequest productRequest)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new
                {
                    CategoryID = productRequest.CategoryID,
                    SKU = productRequest.SKU,
                    Name = productRequest.Name,
                    Description = productRequest.Description,
                    Price = productRequest.Price,
                    StockQuantity = productRequest.StockQuantity,
                    CreatedDate = DateTime.Now,
                    ProductID = 0 // Initialize the output parameter
                };

                // Define the SQL command with output parameter
                var sqlCommand = new SqlCommand("InsertProduct", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                // Add input parameters
                sqlCommand.Parameters.AddWithValue("@CategoryID", parameters.CategoryID);
                sqlCommand.Parameters.AddWithValue("@SKU", parameters.SKU);
                sqlCommand.Parameters.AddWithValue("@Name", parameters.Name);
                sqlCommand.Parameters.AddWithValue("@Description", parameters.Description);
                sqlCommand.Parameters.AddWithValue("@Price", parameters.Price);
                sqlCommand.Parameters.AddWithValue("@StockQuantity", parameters.StockQuantity);
                sqlCommand.Parameters.AddWithValue("@CreatedDate", parameters.CreatedDate);

                // Add output parameter for ProductID
                var outputParameter = new SqlParameter("@ProductID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                sqlCommand.Parameters.Add(outputParameter);

                // Execute the stored procedure
                sqlCommand.ExecuteNonQuery();

                // Retrieve the ProductID from the output parameter
                var insertedProductID = Convert.ToInt32(outputParameter.Value);
                return insertedProductID;
            }
        }

    }
}
