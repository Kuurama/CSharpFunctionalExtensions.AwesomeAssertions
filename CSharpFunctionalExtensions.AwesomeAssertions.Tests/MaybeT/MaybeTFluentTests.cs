using FluentAssertions;
using Xunit.Sdk;

namespace CSharpFunctionalExtensions.AwesomeAssertions.Tests.MaybeT;

public class MaybeTFluentTests
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

    [Fact]
    public void HaveSomeValue_ShouldPass_WhenMaybeHasValue()
    {
        // Arrange
        var maybe = Maybe<int>.From(42);

        // Act & Assert
        maybe.Should().HaveSomeValue();
    }

    [Fact]
    public void HaveSomeValue_ShouldThrow_WhenMaybeHasNoValue()
    {
        // Arrange
        var maybe = Maybe<int>.None;

        // Act
        Action act = () => maybe.Should().HaveSomeValue();

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*Expected a value*");
    }

    [Fact]
    public void HaveSomeValue_ShouldReturnConstraint_WhenMaybeHasValue()
    {
        // Arrange
        var maybe = Maybe<int>.From(42);

        // Act
        var result = maybe.Should().HaveSomeValue();

        // Assert
        result.Which.Should().Be(42);
    }

    [Fact]
    public void HaveValue_ShouldPass_WhenMaybeHasExpectedValue()
    {
        // Arrange
        var maybe = Maybe<string>.From("test");

        // Act & Assert
        maybe.Should().HaveValue("test");
    }

    [Fact]
    public void HaveValue_ShouldThrow_WhenMaybeHasNoValue()
    {
        // Arrange
        var maybe = Maybe<string>.None;

        // Act
        Action act = () => maybe.Should().HaveValue("test");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*Expected a value*");
    }

    [Fact]
    public void HaveValue_ShouldThrow_WhenMaybeHasDifferentValue()
    {
        // Arrange
        var maybe = Maybe<string>.From("actual");

        // Act
        Action act = () => maybe.Should().HaveValue("expected");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*Expected maybe to have value \"expected\"*, but with value \"actual\" it*");
    }

    [Fact]
    public void HaveValue_ShouldReturnConstraint_WhenMaybeHasExpectedValue()
    {
        // Arrange
        var maybe = Maybe<int>.From(42);

        // Act
        var result = maybe.Should().HaveValue(42);

        // Assert
        result.Which.Should().Be(42);
    }

    [Fact]
    public void HaveNoValue_ShouldPass_WhenMaybeHasNoValue()
    {
        var Maybe = Maybe<ICollection<int>>.From([]);


        // Arrange
        var maybe = Maybe<int>.None;

        // Act & Assert
        maybe.Should().HaveNoValue();
    }

    [Fact]
    public void HaveNoValue_ShouldThrow_WhenMaybeHasValue()
    {
        // Arrange
        var maybe = Maybe<int>.From(42);

        // Act
        Action act = () => maybe.Should().HaveNoValue();

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*Expected maybe to have no value*, but with value 42 it*");
    }

    [Fact]
    public void HaveNoValue_ShouldReturnConstraint_WhenMaybeHasNoValue()
    {
        // Arrange
        var maybe = Maybe<int>.None;

        // Act
        var result = maybe.Should().HaveNoValue();

        // Assert
        result.GetType()
            .Should()
            .Be(typeof(AndConstraint<MaybeAssertions<int>>),
                "because the method should return an AndConstraint type"
            );
    }
}