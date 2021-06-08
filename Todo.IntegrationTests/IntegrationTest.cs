using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Todo.Api;
using Todo.Api.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Todo.Api.DTOs;
using Newtonsoft.Json;
using System.Text;
using System.Linq;

namespace Todo.IntegrationTests
{
    public class IntegrationTest
    {
        protected readonly HttpClient testClient;

        protected IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>()
            .WithWebHostBuilder(builder =>
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType ==
                            typeof(DbContextOptions<DataContext>));

                    services.Remove(descriptor);

                    services.AddDbContext<DataContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                    });

                    var sp = services.BuildServiceProvider();

                    using (var scope = sp.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices.GetRequiredService<DataContext>();

                        db.Database.EnsureCreated();
                    }
                    
                })
            );

            testClient = appFactory.CreateClient();
        }

        protected async Task AuthenticateAsync()
        {
            testClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetJwtAsync());
        }

        private async Task<string> GetJwtAsync()
        {
            var userLogin = new LoginUserDTO
            {
                Login = "User1",
                Password = "Cvb123",
            };

            var content = new StringContent(JsonConvert.SerializeObject(userLogin), Encoding.UTF8, "application/json");
            var loginResponce = await testClient.PostAsync("/users/authenticate", content);

            var token = await loginResponce.Content.ReadAsStringAsync();
            return token;
        }
    }
}
