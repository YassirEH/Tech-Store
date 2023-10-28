using Core.Models;
using Infrastructure.Data;

namespace webApi
{
    public class Seed
    {
        private readonly DataContext dataContext;
        public Seed(DataContext context)
        {
            this.dataContext = context;
        }
        public void SeedDataContext(DataContext context)
        {
            if (!dataContext.ProductBuyers.Any())
            {
                var productBuyers = new List<ProductBuyer>()
                {
                    new ProductBuyer()
                    {
                        Product = new Product()
                        {
                            Name = "Galaxy A52",
                            Description = "A great mid tier phone (still better than any iphone out there)",
                            ProductCategories = new List<ProductCategory>()
                            {
                                new ProductCategory { Category = new Category() { Name = "Phones"} }
                            }
                        },
                        Buyer = new Buyer()
                        {
                            FName = "Yassir",
                            LName = "El Halouti",
                            Email = "elhaloutiyassir@gmail.com",
                            BirthDate = DateTime.ParseExact("02/04/2005", "dd/MM/yyyy",null)
                        }
                    },
                    new ProductBuyer()
                    {
                        Product = new Product()
                        {
                            Name = "Lenovo Legion 5",
                            Description = "one of the best gaming laptops out there too bad mine isn't",
                            ProductCategories = new List<ProductCategory>()
                            {
                                new ProductCategory { Category = new Category() {Name = "Laptops"}}
                            }
                        },
                        Buyer = new Buyer()
                        {
                            FName = "Shulk",
                            LName = "Klaus",
                            Email = "Shulk2010@nintendo.jp",
                            BirthDate = DateTime.ParseExact("03/08/1992", "dd/MM/yyyy",null)
                        }
                    },
                    new ProductBuyer()
                    {
                        Product = new Product()
                        {
                            Name = "Galaxy Buds 2",
                            Description = "Samsung's wireless earbuds",
                            ProductCategories = new List<ProductCategory>()
                            {
                                new ProductCategory { Category = new Category() {Name = "earphones" } }
                            }
                        },
                        Buyer = new Buyer()
                        {
                            FName = "Mouad",
                            LName = "Braim",
                            Email = "mbraim@gmail.com",
                            BirthDate = DateTime.ParseExact("18/04/2004", "dd/MM/yyyy", null)
                        }
                    }
                };
                dataContext.ProductBuyers.AddRange(productBuyers);
                dataContext.SaveChanges();
            }
        }
    }
}
