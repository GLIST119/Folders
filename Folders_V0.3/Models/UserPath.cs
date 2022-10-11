namespace Folders.Models
{
    public class UserPath
    {
        public string? Path { get; set; } = null;
        public static string? path = null; 

        public void SetPath()
        {
            path = Path;
        }
    }
}
