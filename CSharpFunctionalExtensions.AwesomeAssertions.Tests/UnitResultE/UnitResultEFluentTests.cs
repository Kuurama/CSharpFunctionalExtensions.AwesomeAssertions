using AwesomeAssertions;
using Xunit.Sdk;

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

    [Fact]
    public void Should_ShouldReturnUnitResultAssertions_WhenUnitResult()
    {
        // Arrange
        var result = UnitResult.Success<string>();

        // Act
        var resultAssertion = result.Should();

        // Assert
        Assert.Equal(
            typeof(UnitResultAssertions<string>),
            resultAssertion.GetType()
        );
    }

    [Fact]
    public void Succeed_ShouldPass_WhenUnitResultIsSuccess()
    {
        // Arrange
        var result = UnitResult.Success<string>();

        // Act & Assert
        result.Should().Succeed();
    }

    [Fact]
    public void Succeed_ShouldThrow_WhenUnitResultIsFailure()
    {
        // Arrange
        var result = UnitResult.Failure("error");

        // Act
        Action act = () => result.Should().Succeed();

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*Expected result to succeed*, but it failed with error \"error\"*");
    }

    [Fact]
    public void Succeed_ShouldReturnConstraint_WhenUnitResultIsSuccess()
    {
        // Arrange
        var result = UnitResult.Success<string>();

        // Act
        var constraint = result.Should().Succeed();

        // Assert
        constraint.GetType().Should().Be(typeof(AndConstraint<UnitResultAssertions<string>>));
    }

    [Fact]
    public void Fail_ShouldPass_WhenUnitResultIsFailure()
    {
        // Arrange
        var result = UnitResult.Failure("error");

        // Act & Assert
        result.Should().Fail();
    }

    [Fact]
    public void Fail_ShouldThrow_WhenUnitResultIsSuccess()
    {
        // Arrange
        var result = UnitResult.Success<string>();

        // Act
        Action act = () => result.Should().Fail();

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*Expected result to fail*, but it succeeded*");
    }

    [Fact]
    public void Fail_ShouldReturnConstraint_WhenUnitResultIsFailure()
    {
        // Arrange
        var error = "error message";
        var result = UnitResult.Failure(error);

        // Act
        var constraint = result.Should().Fail();

        // Assert
        constraint.Which.Should().Be(error);
    }

    [Fact]
    public void FailWith_ShouldPass_WhenUnitResultIsFailureWithExpectedError()
    {
        // Arrange
        var error = "expected error";
        var result = UnitResult.Failure(error);

        // Act & Assert
        result.Should().FailWith(error);
    }

    [Fact]
    public void FailWith_ShouldThrow_WhenUnitResultIsSuccess()
    {
        // Arrange
        var result = UnitResult.Success<string>();

        // Act
        Action act = () => result.Should().FailWith("expected error");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*Expected result to fail*, but it succeeded*");
    }

    [Fact]
    public void FailWith_ShouldThrow_WhenUnitResultIsFailureWithDifferentError()
    {
        // Arrange
        var result = UnitResult.Failure("actual error");

        // Act
        Action act = () => result.Should().FailWith("expected error");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*Expected result error to be \"expected error\", but found \"actual error\"*");
    }

    [Fact]
    public void FailWith_ShouldReturnConstraint_WhenUnitResultIsFailureWithExpectedError()
    {
        // Arrange
        var error = "error message";
        var result = UnitResult.Failure(error);

        // Act
        var constraint = result.Should().FailWith(error);

        // Assert
        constraint.Which.Should().Be(error);
    }
}