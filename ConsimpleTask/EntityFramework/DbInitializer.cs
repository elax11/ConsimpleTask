using ConsimpleTask.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConsimpleTask.EntityFramework
{
    public static class DbInitializer
    {
        public static void Initialize(ShopDbContext context)
        {
            context.Database.Migrate();

            if (!context.Customers.Any())
            {
                var customers = new List<Customer>();
                for (int i = 1; i <= 50; i++)
                {
                    customers.Add(new Customer
                    {
                        FullName = $"Customer {i}",
                        BirthDate = new DateTime(1970 + (i % 20), (i % 12) + 1, (i % 28) + 1),
                        RegistrationDate = DateTime.Now.AddDays(-i * 5)
                    });
                }

                context.Customers.AddRange(customers);
            }

            if (!context.Products.Any())
            {
                var products = new List<Product>
                {
                    new Product { Name = "Laptop", Category = "Electronics", Article = "LPT123", Price = 1200 },
                    new Product { Name = "Smartphone", Category = "Electronics", Article = "SMP456", Price = 800 },
                    new Product { Name = "Table", Category = "Furniture", Article = "TBL789", Price = 150 },
                    new Product { Name = "Chair", Category = "Furniture", Article = "CHR101", Price = 80 },
                    new Product { Name = "Headphones", Category = "Electronics", Article = "HDN202", Price = 50 }
                };

                for (int i = 6; i <= 100; i++)
                {
                    products.Add(new Product
                    {
                        Name = $"Product {i}",
                        Category = i % 2 == 0 ? "Electronics" : "Furniture",
                        Article = $"PRD{i:D3}",
                        Price = 10 + (i % 100) * 5
                    });
                }

                context.Products.AddRange(products);
            }

            if (!context.Purchases.Any())
            {
                var random = new Random();

                for (int i = 1; i <= 100; i++)
                {
                    var customerId = random.Next(1, 51);

                    var randomDays = random.Next(1, 31);
                    var purchaseDate = DateTime.Now.AddDays(-randomDays);

                    var purchase = new Purchase
                    {
                        Date = purchaseDate,
                        TotalAmount = 0,
                        CustomerId = customerId,
                        PurchaseItemsId = new List<int>()
                    };

                    context.Purchases.Add(purchase);
                    context.SaveChanges();

                    decimal totalAmount = 0;

                    for (int j = 0; j < random.Next(1, 5); j++)
                    {
                        var productId = random.Next(1, 101);
                        var quantity = random.Next(1, 10);

                        var product = context.Products.FirstOrDefault(p => p.Id == productId);
                        if (product != null)
                        {
                            var purchaseItem = new PurchaseItem
                            {
                                PurchaseId = purchase.Id,
                                ProductId = productId,
                                Quantity = quantity
                            };

                            context.PurchaseItems.Add(purchaseItem);
                            context.SaveChanges();
                            purchase.PurchaseItemsId.Add(purchaseItem.Id);

                            totalAmount += product.Price * quantity;
                        }
                    }

                    purchase.TotalAmount = totalAmount;
                    context.SaveChanges(); 
                }
            }

            context.SaveChanges();
        }
    }

}
