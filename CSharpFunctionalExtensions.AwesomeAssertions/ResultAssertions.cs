using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using AwesomeAssertions.Execution;
using AwesomeAssertions.Primitives;

// ReSharper disable once CheckNamespace
namespace AwesomeAssertions;

public static class ResultExtensions
{
    public static ResultAssertions Should(this Result instance)
        => new(instance, AssertionChain.GetOrCreate());

    public static StringAssertions FailureShould(this Result instance)
        => new(instance.Error, AssertionChain.GetOrCreate());
}

public class ResultAssertions(Result instance, AssertionChain chain)
    : ReferenceTypeAssertions<Result, ResultAssertions>(instance, chain)
{
    [ExcludeFromCodeCoverage]
    protected override string Identifier => "Result";

    /// <summary>
    /// Asserts a result is a success.
    /// </summary>
    /// <param name="because"></param>
    /// <param name="becauseArgs"></param>
    /// <returns></returns>
    [CustomAssertion]
    public AndConstraint<ResultAssertions> Succeed(string because = "", params object[] becauseArgs)
    {
        CurrentAssertionChain.BecauseOf(because, becauseArgs)
            .ForCondition(Subject.IsSuccess)
            .FailWith(()
                => new FailReason(
                    @$"Expected {{context:result}} to succeed{{reason}}, but it failed with error ""{Subject.Error}""")
            );

        return new AndConstraint<ResultAssertions>(this);
    }

    /// <summary>
    /// Asserts a result is a failure.
    /// </summary>
    /// <param name="because"></param>
    /// <param name="becauseArgs"></param>
    /// <returns></returns>
    [CustomAssertion]
    public AndWhichConstraint<ResultAssertions, string> Fail(string because = "", params object[] becauseArgs)
    {
        CurrentAssertionChain.BecauseOf(because, becauseArgs)
            .ForCondition(Subject.IsFailure)
            .FailWith(()
                => new FailReason(
                    "Expected {context:result} to fail{reason}, but it succeeded"));

        return new AndWhichConstraint<ResultAssertions, string>(this, Subject.Error);
    }

    /// <summary>
    /// Asserts a result is a failure with a specified error.
    /// </summary>
    /// <param name="error"></param>
    /// <param name="because"></param>
    /// <param name="becauseArgs"></param>
    /// <returns></returns>
    [CustomAssertion]
    public AndWhichConstraint<ResultAssertions, string> FailWith(string error, string because = "",
                                                                 params object[] becauseArgs)
    {
        CurrentAssertionChain.BecauseOf(because, becauseArgs)
            .ForCondition(Subject.IsFailure)
            .FailWith(() => new FailReason("Expected {context:result} to fail, but it succeeded"))
            .Then
            .Given(() => Subject.Error)
            .ForCondition(e => e!.Equals(error))
            .FailWith("Expected {context:result} error to be {0}, but found {1}", error, Subject.Error);

        return new AndWhichConstraint<ResultAssertions, string>(this, Subject.Error);
    }
}