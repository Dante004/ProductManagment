using static ProductManagment.Api.Controlers.Products.AddOrUpdateProductController;

namespace ProductManagment.Api.Tests.Validators.Products
{
    public class BaseTest
    {
        protected virtual ProductValidtaor Create()
        {
            return new ProductValidtaor();
        }
    }
}
