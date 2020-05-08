using FluentValidation.TestHelper;
using Xunit;
using static ProductManagment.Api.Controlers.Categories.AddOrUpdateCategoryController;

namespace ProductManagment.Api.Tests.Validators.Categories
{
    public class NameTest : BaseTest
    {
        protected string Name;

        protected override CategoryValidtaor Create()
        {
            var validator =  base.Create();
            CorrectFlow();
            return validator;
        }

        private void CorrectFlow()
        {
            Name = "Test";
        }

        [Fact]
        public void Return_Error()
        {
            //Arrange
            var validator = Create();
            //Assert
            validator.ShouldNotHaveValidationErrorFor(v => v.Name, Name);
        }

        [Fact]
        public void Return_Error_When_Name_Is_Empty()
        {
            //Arrange
            var validator = Create();
            Name = string.Empty;
            //Assert
            validator.ShouldHaveValidationErrorFor(v => v.Name, Name);
        }

        [Fact]
        public void Return_Error_When_Name_Is_To_Short()
        {
            //Arrange
            var validator = Create();
            Name = "Te";
            //Assert
            validator.ShouldHaveValidationErrorFor(v => v.Name, Name);
        }

        [Fact]
        public void Return_Error_When_Name_Is_To_Long()
        {
            //Arrange
            var validator = Create();
            Name = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
            //Assert
            validator.ShouldHaveValidationErrorFor(v => v.Name, Name);
        }
    }
}
