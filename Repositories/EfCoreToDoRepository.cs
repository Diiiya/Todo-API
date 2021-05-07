using TodoApi.Models;

namespace TodoApi.Data.EfCore
{
    public class EfCoreToDoRepository : EfCoreRepository<ToDo, DataContext>
    {
        public EfCoreToDoRepository(DataContext context) : base(context)
        {

        }
    }
}