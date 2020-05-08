using FluentValidation.TestHelper;
using Xunit;
using static ProductManagment.Api.Controlers.Products.AddOrUpdateProductController;

namespace ProductManagment.Api.Tests.Validators.Products
{
    public class PriceTest : BaseTest
    {
        public decimal Price;

        protected override ProductValidtaor Create()
        {
            var validator = base.Create();
            CorrectFlow();
            return validator;
        }

        private void CorrectFlow()
        {
            Price = 10m;
        }

        [Fact]
        public void Return_Success()
        {
            //Arrange
            var validator = Create();
            //Assert
            validator.ShouldNotHaveValidationErrorFor(v => v.Price, Price);
        }

        [Fact]
        public void Return_Error_When_Price_Is_Out_Of_Scale_Precision()
        {
            //Arrange
            var validator = Create();
            Price = 10.4444m;
            //Assert
            validator.ShouldHaveValidationErrorFor(v => v.Price, Price);
        }

        [Fact]
        public void Return_Error_When_Price_Is_Less_Than_0()
        {
            //Arrange
            var validator = Create();
            Price = -1m;
            //Assert
            validator.ShouldHaveValidationErrorFor(o => o.Price, Price);
        }
    }
}
