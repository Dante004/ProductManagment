using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace Microsoft.AspNetCore.Mvc
{
    public static class ActionResultExtension
    {
        public static ActionResultAssertion Should(this IActionResult instance)
        {
            return new ActionResultAssertion(instance);
        }
    }

    public class ActionResultAssertion : ReferenceTypeAssertions<IActionResult, ActionResultAssertion>
    {
        protected override string Identifier => "IActionResult";

        public ActionResultAssertion(IActionResult action)
        {
            Subject = action;
        }

        public ActionResultAssertion BeSuccess<T>(T value, string because = "",
            params object[] becauseArgs) where T : class
        {

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject.GetType() == typeof(CreatedResult))
                .FailWith("Subject should be of type Created Result");

            var createdResult = (CreatedResult)Subject;

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(createdResult.StatusCode == 201)
                .FailWith("Status code should be 201");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(createdResult.Value.IsSameOrEqualTo(value))
                .FailWith("Values should be the same");

            return this;
        }
        public ActionResultAssertion BeOk<T>(T value, string because = "",
            params object[] becauseArgs) where T : class
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject.GetType() == typeof(OkObjectResult))
                .FailWith("Subject should be of type OkObjectResult ");

            var returnedResult = (OkObjectResult)Subject;

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(returnedResult.StatusCode == 200)
                .FailWith("Status code should be 200");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(returnedResult.Value.IsSameOrEqualTo(value))
                .FailWith("Values should be the same");

            return this;
        }

        public ActionResultAssertion BeBadRequest<T>(T value, string because = "",
            params object[] becauseArgs) where T : class
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject.GetType() == typeof(BadRequestObjectResult))
                .FailWith("Subject should be of type BadRequestObjectResult");

            var returnedResult = (BadRequestObjectResult)Subject;

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(returnedResult.StatusCode == 400)
                .FailWith("Status code should be 400");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(returnedResult.Value.IsSameOrEqualTo(value));

            return this;
        }
    }
}
