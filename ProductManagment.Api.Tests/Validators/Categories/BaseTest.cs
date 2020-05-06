using static ProductManagment.Api.Controlers.Categories.AddOrUpdateCategoryController;

namespace ProductManagment.Api.Tests.Validators.Categories
{
    public class BaseTest
    {
        protected virtual CategoryValidtaor Create()
        {
            return new CategoryValidtaor();
        }
    }
}
