using Ecinema.Data.Base;
using Ecinema.Models;

namespace Ecinema.Data.Services
{
    public class ProducersService : EntityBaseRepository<Producer>, IProducersService
    {
        public ProducersService(AppDbContext context) : base(context)
        {
        }
    }
}
