using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.IO;

namespace Folders
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Folder> Folders { get; set; } = null!;
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated(); 
        }

        //serialize and save as .json
        public static void Serialize(List<Folder> folders, string saveName)
        {
            var json = JsonSerializer.Serialize(folders, new JsonSerializerOptions()
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            });
            File.WriteAllText($"Saved/{saveName.Substring(saveName.LastIndexOf(@"\") + 1)}.json", json);
        }
    }
}
