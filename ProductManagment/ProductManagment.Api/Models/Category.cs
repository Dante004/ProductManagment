using System.Collections.Generic;

namespace ProductManagment.Api.Models
{
    public class Category : BaseModel
    {
        public string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
