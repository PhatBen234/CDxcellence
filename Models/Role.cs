namespace Unilever.CDExcellent.API.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }  // Ensure it's initialized

        public Role()
        {
            Name = string.Empty;  // Initialize the property
        }
    }
}
