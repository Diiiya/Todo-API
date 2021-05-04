using TodoApi.Models;

namespace TodoApi.Data.EfCore
{
    public class EfCoreTagRepository : EfCoreRepository<Tag, DataContext>
    {
        public EfCoreTagRepository(DataContext context) : base(context)
        {

        }
    }
}