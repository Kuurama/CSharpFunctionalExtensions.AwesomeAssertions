using FluentAssertions;
using FluentAssertions.Primitives;
using Xunit.Sdk;

namespace CSharpFunctionalExtensions.AwesomeAssertions.Tests.ResultNoT;

public class ResultNoTFluentTests
{
    [Fact]
    public void FailureShould_ShouldReturnStringAssertions_WhenFailure()
    {
        // Arrange
        var result = Result.Failure("Some error");

        // Act
        var stringAssertion = result.FailureShould();

        // Assert
        Assert.Equal(
            typeof(StringAssertions),
            stringAssertion.GetType()
        );
    }

    [Fact]
    public void Should_ShouldReturnResultAssertions_WhenResult()
    {
        // Arrange
        var result = Result.Success();

        // Act
        var resultAssertion = result.Should();

        // Assert
        Assert.Equal(
            typeof(ResultAssertions),
            resultAssertion.GetType()
        );
    }

    [Fact]
    public void Succeed_ShouldPass_WhenResultIsSuccess()
    {
        // Arrange
        var result = Result.Success();

        // Act & Assert
        result.Should().Succeed();
    }

    [Fact]
    public void Succeed_ShouldThrow_WhenResultIsFailure()
    {
        // Arrange
        var result = Result.Failure("error");

        // Act
        Action act = () => result.Should().Succeed();

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*Expected result to succeed*, but it failed with error \"error\"*");
    }

    [Fact]
    public void Succeed_ShouldReturnConstraint_WhenResultIsSuccess()
    {
        // Arrange
        var result = Result.Success();

        // Act
        var constraint = result.Should().Succeed();

        // Assert
        constraint.GetType().Should().Be(typeof(AndConstraint<ResultAssertions>));
    }

    [Fact]
    public void Fail_ShouldPass_WhenResultIsFailure()
    {
        // Arrange
        var result = Result.Failure("error");

        // Act & Assert
        result.Should().Fail();
    }

    [Fact]
    public void Fail_ShouldThrow_WhenResultIsSuccess()
    {
        // Arrange
        var result = Result.Success();

        // Act
        Action act = () => result.Should().Fail();

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*Expected result to fail*, but it succeeded*");
    }

    [Fact]
    public void Fail_ShouldReturnConstraint_WhenResultIsFailure()
    {
        // Arrange
        var error = "error message";
        var result = Result.Failure(error);

        // Act
        var constraint = result.Should().Fail();

        // Assert
        constraint.Which.Should().Be(error);
    }

    [Fact]
    public void FailWith_ShouldPass_WhenResultIsFailureWithExpectedError()
    {
        // Arrange
        var error = "expected error";
        var result = Result.Failure(error);

        // Act & Assert
        result.Should().FailWith(error);
    }

    [Fact]
    public void FailWith_ShouldThrow_WhenResultIsSuccess()
    {
        // Arrange
        var result = Result.Success();

        // Act
        Action act = () => result.Should().FailWith("expected error");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*Expected result to fail*, but it succeeded*");
    }

    [Fact]
    public void FailWith_ShouldThrow_WhenResultIsFailureWithDifferentError()
    {
        // Arrange
        var result = Result.Failure("actual error");

        // Act
        Action act = () => result.Should().FailWith("expected error");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*Expected result error to be \"expected error\", but found \"actual error\"*");
    }

    [Fact]
    public void FailWith_ShouldReturnConstraint_WhenResultIsFailureWithExpectedError()
    {
        // Arrange
        var error = "error message";
        var result = Result.Failure(error);

        // Act
        var constraint = result.Should().FailWith(error);

        // Assert
        constraint.Which.Should().Be(error);
    }
}