using Microsoft.EntityFrameworkCore;

namespace Persistence;

public partial class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
}