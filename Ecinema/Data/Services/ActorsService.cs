using Ecinema.Data.Base;
using Ecinema.Models;

namespace Ecinema.Data.Services
{
    public class ActorsService : EntityBaseRepository<Actor>, IActorsService
    {
        public ActorsService(AppDbContext context) : base(context) { }
    }
}
