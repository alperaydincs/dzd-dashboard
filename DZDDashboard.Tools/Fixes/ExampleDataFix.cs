using DZDDashboard.Data;
using Microsoft.EntityFrameworkCore;

namespace DZDDashboard.Tools.Fixes;

/// <summary>
/// Template for a one-off fix. Copy this file, rename the class, adjust Name/Description,
/// and implement RunAsync. It is auto-discovered by Program.cs (any IDataFix in this assembly).
/// </summary>
public class ExampleDataFix : IDataFix
{
    public string Name => "example";
    public string Description => "Template fix - copy this file to add a real one-off correction.";

    public async Task RunAsync(AppDbContext db, CancellationToken cancellationToken)
    {
        var user = await db.Users.FirstOrDefaultAsync(cancellationToken);
        if (user is null)
        {
            Console.WriteLine("No users found - nothing to do.");
            return;
        }

        Console.WriteLine($"Would touch user #{user.Id} ({user.Email}). Replace this with the real fix.");
        // await db.SaveChangesAsync(cancellationToken);
    }
}
