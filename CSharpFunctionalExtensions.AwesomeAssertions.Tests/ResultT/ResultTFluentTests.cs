using AwesomeAssertions;
using AwesomeAssertions.Primitives;
using Xunit.Sdk;

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

    [Fact]
    public void Succeed_ShouldPass_WhenResultIsSuccess()
    {
        // Arrange
        var result = Result.Success(42);

        // Act & Assert
        result.Should().Succeed();
    }

    [Fact]
    public void Succeed_ShouldThrow_WhenResultIsFailure()
    {
        // Arrange
        var result = Result.Failure<int>("error");

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
        var result = Result.Success(42);

        // Act
        var constraint = result.Should().Succeed();

        // Assert
        constraint.GetType().Should().Be(typeof(AndWhichConstraint<ResultAssertions<int>, int>));
    }

    [Fact]
    public void SucceedWith_ShouldPass_WhenResultIsSuccessWithExpectedValue()
    {
        // Arrange
        var result = Result.Success(123);

        // Act & Assert
        result.Should().SucceedWith(123);
    }

    [Fact]
    public void SucceedWith_ShouldPass_WhenResultIsSuccessWithNullValue()
    {
        // Arrange
        var result = Result.Success<int?>(null);

        // Act & Assert
        result.Should().SucceedWith(null);
    }

    [Fact]
    public void SucceedWith_ShouldThrow_WhenResultIsSuccessWithNullValueAndExpectedValueIsNotNull()
    {
        // Arrange
        var result = Result.Success<int?>(null);

        // Act
        Action act = () => result.Should().SucceedWith(123);

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*Expected result value to be 123, but found <null>*");
    }

    [Fact]
    public void SucceedWith_ShouldThrow_WhenResultIsFailure()
    {
        // Arrange
        var result = Result.Failure<int>("error");

        // Act
        Action act = () => result.Should().SucceedWith(123);

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*Expected result to succeed*, but it failed with error \"error\"*");
    }

    [Fact]
    public void SucceedWith_ShouldThrow_WhenResultIsSuccessWithDifferentValue()
    {
        // Arrange
        var result = Result.Success(456);

        // Act
        Action act = () => result.Should().SucceedWith(123);

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*Expected result value to be 123, but found 456*");
    }

    [Fact]
    public void SucceedWith_ShouldReturnConstraint_WhenResultIsSuccessWithExpectedValue()
    {
        // Arrange
        var result = Result.Success(789);

        // Act
        var constraint = result.Should().SucceedWith(789);

        // Assert
        constraint.Which.Should().Be(789);
    }

    [Fact]
    public void Fail_ShouldPass_WhenResultIsFailure()
    {
        // Arrange
        var result = Result.Failure<int>("error");

        // Act & Assert
        result.Should().Fail();
    }

    [Fact]
    public void Fail_ShouldThrow_WhenResultIsSuccess()
    {
        // Arrange
        var result = Result.Success(42);

        // Act
        Action act = () => result.Should().Fail();

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*Expected result to fail, but it succeeded with value \"42\"*");
    }

    [Fact]
    public void Fail_ShouldReturnConstraint_WhenResultIsFailure()
    {
        // Arrange
        var error = "error message";
        var result = Result.Failure<int>(error);

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
        var result = Result.Failure<int>(error);

        // Act & Assert
        result.Should().FailWith(error);
    }

    [Fact]
    public void FailWith_ShouldThrow_WhenResultIsSuccess()
    {
        // Arrange
        var result = Result.Success(42);

        // Act
        Action act = () => result.Should().FailWith("expected error");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*Expected result to fail, but it succeeded*");
    }

    [Fact]
    public void FailWith_ShouldThrow_WhenResultIsFailureWithDifferentError()
    {
        // Arrange
        var result = Result.Failure<int>("actual error");

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
        var result = Result.Failure<int>(error);

        // Act
        var constraint = result.Should().FailWith(error);

        // Assert
        constraint.Which.Should().Be(error);
    }
}