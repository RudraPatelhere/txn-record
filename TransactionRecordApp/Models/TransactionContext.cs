using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TransactionRecordApp.Models
{
    /// <summary>
    /// This class inherits from IdentityDbContext to support ASP.NET Identity
    /// and is used to interact with both Identity tables and custom app tables.
    /// </summary>
    public class TransactionContext : IdentityDbContext<User>
    {
        public TransactionContext(DbContextOptions options)
            : base(options)
        {
        }

        /// <summary>
        /// Gives access to the Transactions table in the DB
        /// </summary>
        public DbSet<Transaction> Transactions { get; set; }

        /// <summary>
        /// Gives access to the TransactionType table in the DB
        /// </summary>
        public DbSet<TransactionType> TransactionType { get; set; }

        /// <summary>
        /// Seed initial data and identity table setup
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ✅ Must call base for Identity-related tables setup
            base.OnModelCreating(modelBuilder);

            // Seed the TransactionType table
            modelBuilder.Entity<TransactionType>().HasData(
                new TransactionType() { TransactionTypeId = "S", Name = "Sell", CommissionFee = 5.99 },
                new TransactionType() { TransactionTypeId = "B", Name = "Buy", CommissionFee = 5.40 }
            );

            // Seed the Transactions table
            modelBuilder.Entity<Transaction>().HasData(
                new Transaction()
                {
                    TransactionId = 1,
                    TickerSymbol = "AAPL",
                    CompanyName = "Apple",
                    Quantity = 2,
                    SharePrice = 142.90,
                    TransactionTypeId = "B"
                },
                new Transaction()
                {
                    TransactionId = 2,
                    TickerSymbol = "F",
                    CompanyName = "Ford Motors Company",
                    Quantity = 4,
                    SharePrice = 12.82,
                    TransactionTypeId = "S"
                },
                new Transaction()
                {
                    TransactionId = 3,
                    TickerSymbol = "GOOG",
                    CompanyName = "Alphabet Inc.",
                    Quantity = 100,
                    SharePrice = 2701.76,
                    TransactionTypeId = "S"
                },
                new Transaction()
                {
                    TransactionId = 4,
                    TickerSymbol = "MSFT",
                    CompanyName = "Microsoft Corporation",
                    Quantity = 100,
                    SharePrice = 123.45,
                    TransactionTypeId = "B"
                }
            );
        }

        /// <summary>
        /// Static method to create Admin role and user
        /// </summary>
        public static async Task CreateAdminUser(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string adminEmail = "admin@conestoga.ca";
            string adminPassword = "Admin123!";

            // Create Admin role if it doesn't exist
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Create Admin user if it doesn't exist
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
