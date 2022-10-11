using Microsoft.AspNetCore.Mvc;
using Folders.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Storage;

namespace Folders._3.Controllers
{
    public class HomeController : Controller
    {
        ApplicationContext db;
        public HomeController(ApplicationContext context)
        {
            db = context;
        }

        //first page
        public IActionResult Index()
        {
            return View();
        }  
        
        public IActionResult Info()
        {
            return View();
        }

        public IActionResult LoadFromSystem()
        {
            return View();
        }

        public IActionResult StaticLoad(UserPath? userPath = null)
        {
            if (userPath?.Path == null)
            {
                return View();
            }
            else
            {
                try
                {
                    List<Folder> folders = new List<Folder>();
                    GetDirs(ref folders, userPath.Path);
                    ApplicationContext.Serialize(folders, userPath.Path);
                    return RedirectToAction("Saved");
                }
                catch (Exception ex)    
                {
                    return View("Error", ex.Message);
                }
            }
        }
        public IActionResult DynamicExplorer(string currDir = @"C:\")
        {
            return View("DynamicExplorer", currDir);
        }

        public IActionResult Explorer(string? name = null)
        {
            ViewBag.Name = name;
            return View(db.Folders);
        }


        public IActionResult Saved(string name)
        {
            if(name == null)
            {
                return View();
            }
            List<Folder> folders = Deserialize(name);
            ClearDb();  
            db.Folders.AddRange(folders);
            db.SaveChanges();

            return RedirectToAction("Explorer");
        }

        //get names of directories and convert them into Folders objects
        static void GetDirs(ref List<Folder> folders, string path, Folder? parent = null)
        {
            Folder folder;
            try
            {
                List<string> dirs = Directory.GetDirectories(path).ToList();
                foreach (var d in dirs)
                {
                    folder = new Folder
                    {
                        Name = d.Substring(d.LastIndexOf(@"\") + 1),
                        Parent = path == @"C:\" ? null : folders.FirstOrDefault(f => f.Name == path.Substring(path.LastIndexOf(@"\") + 1))
                    };
                    if (!folders.Contains(folder)) folders.Add(folder);
                    Console.WriteLine($"Added: {path}{folder.Name}");
                    GetDirs(ref folders, d, parent);
                }
            }
            // directories without access will not be created
            catch (UnauthorizedAccessException)
            {

            }
        }
        
        //deserialize json
        private List <Folder> Deserialize(string saveName = "")
        {
            List<Folder>? deserialized = new List<Folder>();
            using (FileStream fs = new FileStream($"{saveName}", FileMode.OpenOrCreate))
            {
                deserialized = JsonSerializer.Deserialize<IEnumerable<Folder>>(fs) as List<Folder>;
            }

            // add parents back
            foreach (var folder in deserialized)  //I don't know how to rid off this warning
            {
                folder.Parent = deserialized.FirstOrDefault(f => f.Id == folder.ParentId);
            }
            return deserialized;
        }
        private void ClearDb()
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }
    }
}
