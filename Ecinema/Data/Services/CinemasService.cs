using Ecinema.Data.Base;
using Ecinema.Models;

namespace Ecinema.Data.Services
{
    public class CinemasService : EntityBaseRepository<Cinema>, ICinemasService
    {
        public CinemasService(AppDbContext context) : base(context)
        {
        }
    }
}
