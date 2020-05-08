using FluentValidation.TestHelper;
using Xunit;
using static ProductManagment.Api.Controlers.Products.AddOrUpdateProductController;

namespace ProductManagment.Api.Tests.Validators.Products
{
    public class DescriptionTest : BaseTest
    {
        protected string Decription;

        protected override ProductValidtaor Create()
        {
            var validator = base.Create();
            CorrectFlow();
            return validator;
        }

        private void CorrectFlow()
        {
            Decription = "Test";
        }

        [Fact]
        public void Return_Error()
        {
            //Arrange
            var validator = Create();
            //Assert
            validator.ShouldNotHaveValidationErrorFor(v => v.Name, Decription);
        }

        [Fact]
        public void Return_Error_When_Name_Is_To_Short()
        {
            //Arrange
            var validator = Create();
            Decription = "Te";
            //Assert
            validator.ShouldHaveValidationErrorFor(v => v.Description, Decription);
        }

        [Fact]
        public void Return_Error_When_Name_Is_To_Long()
        {
            //Arrange
            var validator = Create();
            Decription = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
            //Assert
            validator.ShouldHaveValidationErrorFor(v => v.Description, Decription);
        }
    }
}
