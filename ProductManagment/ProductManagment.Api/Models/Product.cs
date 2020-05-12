using System.Collections.Generic;

namespace ProductManagment.Api.Models
{
    public class Product : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<FileProduct> FileProducts { get; } = new List<FileProduct>(); 
    }
}
