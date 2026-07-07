using DZDDashboard.Data;
using DZDDashboard.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

if (args.Length < 2 || !int.TryParse(args[1], out var performedByUserId))
{
    Console.WriteLine("Usage: dotnet run -- <fix-name> <performed-by-user-id>");
    Console.WriteLine();
    Console.WriteLine("Available fixes:");
    foreach (var fix in DiscoverFixes())
        Console.WriteLine($"  {fix.Name,-20} {fix.Description}");
    return 1;
}

var fixName = args[0];
var selected = DiscoverFixes().FirstOrDefault(f => f.Name.Equals(fixName, StringComparison.OrdinalIgnoreCase));
if (selected is null)
{
    Console.WriteLine($"No fix named '{fixName}'. Run without arguments to list available fixes.");
    return 1;
}

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile("appsettings.Development.json", optional: true)
    .AddUserSecrets(typeof(Program).Assembly)
    .AddEnvironmentVariables()
    .Build();

var connectionString = configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection is required.");

var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseNpgsql(connectionString)
    .Options;

await using var db = new AppDbContext(options, new FixedUserAuditProvider(performedByUserId));

Console.WriteLine($"Running fix '{selected.Name}' as user #{performedByUserId}...");
await selected.RunAsync(db, CancellationToken.None);
Console.WriteLine("Done.");
return 0;

static IEnumerable<IDataFix> DiscoverFixes() =>
    typeof(IDataFix).Assembly.GetTypes()
        .Where(t => typeof(IDataFix).IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false })
        .Select(t => (IDataFix)Activator.CreateInstance(t)!);
