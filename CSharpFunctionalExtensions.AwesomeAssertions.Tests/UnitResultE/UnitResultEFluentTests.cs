using FluentAssertions;

namespace CSharpFunctionalExtensions.AwesomeAssertions.Tests.UnitResultE;

public class UnitResultEFluentTests
{
    [Fact]
    public void FailureShould_ShouldReturnStringAssertions_WhenFailure()
    {
        // Arrange
        var result = UnitResult.Success<string>();

        // Act
        var stringAssertion = result.Should();

        // Assert
        Assert.Equal(
            typeof(UnitResultAssertions<string>),
            stringAssertion.GetType()
        );
    }
}