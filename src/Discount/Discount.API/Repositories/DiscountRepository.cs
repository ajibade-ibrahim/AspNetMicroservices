using System.Threading.Tasks;
using Dapper;
using Discount.API.Entities;
using Discount.API.Helpers;
using Discount.API.Repositories.Contracts;
using Npgsql;

namespace Discount.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        public DiscountRepository(DatabaseSettings dbSettings)
        {
            _dbSettings = dbSettings;
        }

        private readonly DatabaseSettings _dbSettings;

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            await using var connection = GetConnection();

            var affected = await connection.ExecuteAsync(
                "INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                new
                {
                    coupon.ProductName,
                    coupon.Description,
                    coupon.Amount
                });

            return affected != 0;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            await using var connection = GetConnection();

            var affected = await connection.ExecuteAsync(
                "DELETE FROM Coupon WHERE ProductName = @ProductName",
                new
                {
                    ProductName = productName
                });

            return affected != 0;
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            await using var connection = GetConnection();

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(
                "SELECT * FROM Coupon WHERE ProductName = @ProductName",
                new
                {
                    ProductName = productName
                });

            return coupon
                ?? new Coupon
                {
                    ProductName = "No Discount",
                    Amount = 0,
                    Description = "No Discount Desc"
                };
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            await using var connection = GetConnection();

            var affected = await connection.ExecuteAsync(
                "UPDATE Coupon SET ProductName=@ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
                new
                {
                    coupon.ProductName,
                    coupon.Description,
                    coupon.Amount,
                    coupon.Id
                });

            return affected != 0;
        }

        private NpgsqlConnection GetConnection()
        {
            return new(_dbSettings.ConnectionString);
        }
    }
}