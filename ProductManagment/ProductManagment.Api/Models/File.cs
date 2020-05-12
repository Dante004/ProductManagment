using System.Collections.Generic;

namespace ProductManagment.Api.Models
{
    public class File : BaseModel
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public virtual ICollection<FileProduct> FileProducts { get; } = new List<FileProduct>();
    }
}
