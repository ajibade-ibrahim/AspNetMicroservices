using System.Data;
using System.Threading;
using Discount.API.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Discount.API.Extensions
{
    public static class HostExtensions
    {
        private const string CreateCouponTable1 = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY, 
                                                                ProductName VARCHAR(24) NOT NULL,
                                                                Description TEXT,
                                                                Amount INT)";

        private const string CreateIPhoneX =
            "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('IPhone X', 'IPhone Discount', 150);";

        private const string CreateSamsung10 =
            "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Samsung 10', 'Samsung Discount', 100);";

        private const string DropCouponTable = "DROP TABLE IF EXISTS Coupon";

        public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
        {
            var retryForAvailability = retry.Value;

            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<TContext>>();
            var dbSettings = services.GetRequiredService<DatabaseSettings>();

            try
            {
                logger.LogInformation("Migrating postresql database.");

                using var connection = new NpgsqlConnection(dbSettings.ConnectionString);
                connection.Open();

                using var command = new NpgsqlCommand
                {
                    Connection = connection
                };

                ExecuteNonQuery(command, DropCouponTable);
                ExecuteNonQuery(command, CreateCouponTable1);
                ExecuteNonQuery(command, CreateIPhoneX);
                ExecuteNonQuery(command, CreateSamsung10);

                logger.LogInformation("Migrated postresql database.");
            }
            catch (NpgsqlException ex)
            {
                logger.LogError(ex, "An error occurred while migrating the postresql database");

                if (retryForAvailability < 50)
                {
                    retryForAvailability++;
                    Thread.Sleep(2000);
                    MigrateDatabase<TContext>(host, retryForAvailability);
                }
            }

            return host;
        }

        private static void ExecuteNonQuery(IDbCommand command, string commandText)
        {
            command.CommandText = commandText;
            command.ExecuteNonQuery();
        }
    }
}