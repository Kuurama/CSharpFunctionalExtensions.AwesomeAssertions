using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.AwesomeAssertions.Generator;
using AwesomeAssertions.Execution;
using AwesomeAssertions.Primitives;

// ReSharper disable once CheckNamespace
namespace AwesomeAssertions;

[GeneratePrimitiveExtensions("FailureShould", "Error", "CSharpFunctionalExtensions.UnitResult`1")]
public static class UnitResultExtensions
{
    public static UnitResultAssertions<E> Should<E>(this UnitResult<E> instance)
        => new(instance, AssertionChain.GetOrCreate());
}

public class UnitResultAssertions<E>(UnitResult<E> instance, AssertionChain chain)
    : ReferenceTypeAssertions<UnitResult<E>, UnitResultAssertions<E>>(instance, chain)
{
    [ExcludeFromCodeCoverage]
    protected override string Identifier => "Result";

    /// <summary>
    /// Asserts a UnitResult is a success.
    /// </summary>
    /// <param name="because"></param>
    /// <param name="becauseArgs"></param>
    /// <returns></returns>
    [CustomAssertion]
    public AndConstraint<UnitResultAssertions<E>> Succeed(string because = "", params object[] becauseArgs)
    {
        CurrentAssertionChain.BecauseOf(because, becauseArgs)
            .ForCondition(Subject.IsSuccess)
            .FailWith(()
                => new FailReason(
                    @$"Expected {{context:result}} to succeed{{reason}}, but it failed with error ""{Subject.Error}""")
            );

        return new AndConstraint<UnitResultAssertions<E>>(this);
    }

    /// <summary>
    /// Asserts a UnitResult is a failure.
    /// </summary>
    /// <param name="because"></param>
    /// <param name="becauseArgs"></param>
    /// <returns></returns>
    [CustomAssertion]
    public AndWhichConstraint<UnitResultAssertions<E>, E> Fail(string because = "", params object[] becauseArgs)
    {
        CurrentAssertionChain.BecauseOf(because, becauseArgs)
            .ForCondition(Subject.IsFailure)
            .FailWith(()
                => new FailReason(
                    "Expected {context:result} to fail{reason}, but it succeeded"));

        return new AndWhichConstraint<UnitResultAssertions<E>, E>(this, Subject.Error);
    }

    /// <summary>
    /// Asserts a UnitResult is a failure with a specified error.
    /// </summary>
    /// <param name="error"></param>
    /// <param name="because"></param>
    /// <param name="becauseArgs"></param>
    /// <returns></returns>
    [CustomAssertion]
    public AndWhichConstraint<UnitResultAssertions<E>, E> FailWith(
        E error, string because = "",
        params object[] becauseArgs)
    {
        CurrentAssertionChain.BecauseOf(because, becauseArgs)
            .ForCondition(Subject.IsFailure)
            .FailWith(() => new FailReason("Expected {context:result} to fail, but it succeeded"))
            .Then
            .Given(() => Subject.Error)
            .ForCondition(e => e!.Equals(error))
            .FailWith("Expected {context:result} error to be {0}, but found {1}", error, Subject.Error);

        return new AndWhichConstraint<UnitResultAssertions<E>, E>(this, Subject.Error);
    }
}