using FluentAssertions;
using FluentAssertions.Primitives;

namespace CSharpFunctionalExtensions.AwesomeAssertions.Tests.ResultT;

public class ResultTFluentTests
{
    [Fact]
    public void FailureShould_ShouldReturnStringAssertions_WhenFailure()
    {
        // Arrange
        var result = Result.Failure<int>("Some error");

        // Act
        var stringAssertion = result.FailureShould();

        // Assert
        Assert.Equal(
            typeof(StringAssertions),
            stringAssertion.GetType()
        );
    }

    [Fact]
    public void Should_ShouldReturnResultAssertions_WhenSuccess()
    {
        // Arrange
        var result = Result.Success(1);

        // Act
        var resultAssertion = result.Should();

        // Assert
        Assert.Equal(
            typeof(ResultAssertions<int>),
            resultAssertion.GetType()
        );
    }
}