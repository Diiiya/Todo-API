using TodoApi.Models;

namespace TodoApi.Data.EfCore
{
    public class EfCoreUserRepository : EfCoreRepository<User, DataContext>
    {
        // For specific to the user methods if to be added
        public EfCoreUserRepository(DataContext context) : base(context)
        {

        }
    }
}