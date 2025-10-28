using Microsoft.EntityFrameworkCore;

namespace LoggingService;
public partial class LoggingServiceDbContext(DbContextOptions<LoggingServiceDbContext> options) : DbContext(options)
{
}