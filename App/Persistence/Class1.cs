using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public partial class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

    }
}