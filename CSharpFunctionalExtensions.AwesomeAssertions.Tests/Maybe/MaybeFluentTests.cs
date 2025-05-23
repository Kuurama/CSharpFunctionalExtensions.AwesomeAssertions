using FluentAssertions;

namespace CSharpFunctionalExtensions.AwesomeAssertions.Tests.Maybe;

public class MaybeFluentTests
{
    [Fact]
    public void Should_ShouldReturnMaybeAssertions_WhenSuccess()
    {
        // Arrange
        var maybe = Maybe<int>.None;

        // Act
        var maybeAssertion = maybe.Should();

        // Assert
        Assert.Equal(
            typeof(MaybeAssertions<int>),
            maybeAssertion.GetType()
        );
    }
}