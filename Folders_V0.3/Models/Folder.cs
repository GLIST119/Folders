namespace Folders
{
    public class Folder
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? ParentId { get; set; }
        public Folder? Parent { get; set; }

        //useless?
        public List<Folder> Children { get; set; } = new();
    }
}
