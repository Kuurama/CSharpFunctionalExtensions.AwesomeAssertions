using FluentAssertions;

namespace CSharpFunctionalExtensions.AwesomeAssertions.Tests.ResultTE;

public class ResultTEFluentTests
{
    [Fact]
    public void Should_ShouldReturnResultAssertions_WhenSuccess()
    {
        // Arrange
        var result = Result.Success<int, string>(1);

        // Act
        var resultAssertion = result.Should();

        // Assert
        Assert.Equal(
            typeof(ResultAssertions<int, string>),
            resultAssertion.GetType()
        );
    }
}