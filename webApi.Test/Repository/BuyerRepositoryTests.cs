using Core.Models;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace webApi.Test.Repository
{
    public class BuyerRepositoryTests
    {
        private async Task<DataContext> GetDatabaseContext()
        {
            var option = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new DataContext(option);
            databaseContext.Database.EnsureCreated();
            if (await databaseContext.Products.CountAsync() <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    databaseContext.Buyers.Add(
                        new Buyer()
                        {
                            FName = "Yassir",
                            LName = "El Halouti",
                            Email = "elhaloutiyassir@gmail.com",
                            BirthDate = DateTime.ParseExact("02/04/2005", "dd/MM/yyyy", null)
                        }
                        );
                    await databaseContext.SaveChangesAsync();
                }
            }
            return databaseContext;
        }

        [Fact]
        public async void BuyerRepository_BuyerExists_ReturnOk()
        {
            //Assemble
            var id = 3;
            var dbContext = await GetDatabaseContext();
            var buyerRepository = new BuyerRepository(dbContext);

            //Act
            var result = buyerRepository.BuyerExists(id);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async void BuyerRepository_GetBuyer_ReturnOk()
        {
            var expectedName = "Yassir";
            var expectedEmail = "elhaloutiyassir@gmail.com";
            var id = 3;
            var dbContext = await GetDatabaseContext();
            var buyerRepository = new BuyerRepository(dbContext);

            var result = buyerRepository.GetBuyerById(id);

            dbContext.SaveChanges();

            Assert.NotNull(result);
            Assert.Equal(expectedName, result.FName);
            Assert.Equal(expectedEmail, result.Email);


        }

        [Fact]
        public async void BuyerRepository_GetBuyers_ReturnOk()
        {
            var dbContext = await GetDatabaseContext();
            var buyerRepository = new BuyerRepository(dbContext);

            var result = buyerRepository.GetBuyers();

            Assert.NotNull(result);
            Assert.IsAssignableFrom<ICollection<Buyer>>(result);
            Assert.True(result.Count > 0);
            Assert.True(result.All(buyer => buyer.GetType() == typeof(Buyer)));
        }

        [Fact]
        public async void BuyerRepository_CreateBuyer_ReturnOk()
        {
            var dbContext = await GetDatabaseContext();
            var buyerRepository = new BuyerRepository(dbContext);
            var buyer = new Buyer()
            {
                FName = "Yassir",
                LName = "El Halouti",
                Email = "elhaloutiyassir@gmail.com",
                BirthDate = DateTime.ParseExact("02/04/2005", "dd/MM/yyyy", null)
            };

            var result = buyerRepository.CreateBuyer(buyer);

            Assert.True(result);
        }

        [Fact]
        public async void BuyerRepository_UpdateBuyer_ReturnOk()
        {
            var buyerId = 1;
            var dbContext = await GetDatabaseContext();
            var buyerRepository = new BuyerRepository(dbContext);
            var existingBuyer = dbContext.Buyers.FirstOrDefault(b => b.Id == buyerId);

            var updatedBuyer = new Buyer()
            {
                Id = buyerId,
                FName = "Yessir",
                LName = "El Baloti",
                Email = "yessirbaloteli@gmail.com",
                BirthDate = DateTime.ParseExact("02/04/2005", "dd/MM/yyyy", null)
            };

            var result = buyerRepository.UpdateBuyer(updatedBuyer);
            Assert.True(result);
        }

        [Fact]
        public async void ProductRepository_DeleteBuyer_ReturnOk()
        {
            //Assemble

            var dbContext = await GetDatabaseContext();
            var buyerRepository = new BuyerRepository(dbContext);

            var buyer = new Buyer()
            {
                FName = "Yessir",
                LName = "El Baloti",
                Email = "yessirbaloteli@gmail.com",
                BirthDate = DateTime.ParseExact("02/04/2005", "dd/MM/yyyy", null)
            };

            dbContext.Buyers.Add(buyer);
            dbContext.SaveChanges();


            //Act
            var result = buyerRepository.DeleteBuyer(buyer);

            //Assert
            Assert.True(result);
        }
    }
}
