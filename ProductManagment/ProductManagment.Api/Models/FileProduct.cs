namespace ProductManagment.Api.Models
{
    public class FileProduct : BaseModel
    {
        public int FileId { get; set; }
        public virtual File File { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
