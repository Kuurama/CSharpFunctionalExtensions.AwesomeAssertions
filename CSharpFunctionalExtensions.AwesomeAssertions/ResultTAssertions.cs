using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.AwesomeAssertions.Generator;
using AwesomeAssertions.Execution;
using AwesomeAssertions.Primitives;

// ReSharper disable once CheckNamespace
namespace AwesomeAssertions;

[GeneratePrimitiveExtensions("SuccessShould", "Value", "CSharpFunctionalExtensions.Result`1")]
public static class ResultTExtensions
{
    extension<T>(Result<T> instance)
    {
        public ResultAssertions<T> Should() => new(instance, AssertionChain.GetOrCreate());
        public StringAssertions FailureShould() => new(instance.Error, AssertionChain.GetOrCreate());
    }
}

public class ResultAssertions<T>(Result<T> instance, AssertionChain chain)
    : ReferenceTypeAssertions<Result<T>, ResultAssertions<T>>(instance, chain)
{
    [ExcludeFromCodeCoverage]
    protected override string Identifier => "Result<T>";

    /// <summary>
    /// Asserts a result is a success.
    /// </summary>
    /// <param name="because"></param>
    /// <param name="becauseArgs"></param>
    /// <returns></returns>
    [CustomAssertion]
    public AndWhichConstraint<ResultAssertions<T>, T> Succeed(string because = "", params object[] becauseArgs)
    {
        CurrentAssertionChain.BecauseOf(because, becauseArgs)
            .ForCondition(Subject.IsSuccess)
            .FailWith(()
                => new FailReason(
                    @$"Expected {{context:result}} to succeed{{reason}}, but it failed with error ""{Subject.Error}""")
            );

        return new AndWhichConstraint<ResultAssertions<T>, T>(this, Subject.Value);
    }

    /// <summary>
    /// Asserts a result is a success with a specified value.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="because"></param>
    /// <param name="becauseArgs"></param>
    /// <returns></returns>
    [CustomAssertion]
    public AndWhichConstraint<ResultAssertions<T>, T> SucceedWith(T? value, string because = "",
                                                                  params object[] becauseArgs)
    {
        CurrentAssertionChain.BecauseOf(because, becauseArgs)
            .ForCondition(Subject.IsSuccess)
            .FailWith(()
                => new FailReason(
                    @$"Expected {{context:result}} to succeed{{reason}}, but it failed with error ""{Subject.Error}"""))
            .Then
            .Given(() => Subject.Value)
            .ForCondition(v => v switch
            {
                null when value == null => true,
                null => false,
                _ => v.Equals(value)
            })
            .FailWith("Expected {context:result} value to be {0}, but found {1}", value, Subject.Value);

        return new AndWhichConstraint<ResultAssertions<T>, T>(this, Subject.Value);
    }

    /// <summary>
    /// Asserts a result is a failure.
    /// </summary>
    /// <param name="because"></param>
    /// <param name="becauseArgs"></param>
    /// <returns></returns>
    [CustomAssertion]
    public AndWhichConstraint<ResultAssertions<T>, string> Fail(string because = "", params object[] becauseArgs)
    {
        CurrentAssertionChain.BecauseOf(because, becauseArgs)
            .ForCondition(Subject.IsFailure)
            .FailWith(()
                => new FailReason(
                    @$"Expected {{context:result}} to fail, but it succeeded with value ""{Subject.Value}"""));

        return new AndWhichConstraint<ResultAssertions<T>, string>(this, Subject.Error);
    }

    /// <summary>
    /// Asserts a result is a failure with a specified error.
    /// </summary>
    /// <param name="error"></param>
    /// <param name="because"></param>
    /// <param name="becauseArgs"></param>
    /// <returns></returns>
    [CustomAssertion]
    public AndWhichConstraint<ResultAssertions<T>, string> FailWith(string error, string because = "",
                                                                    params object[] becauseArgs)
    {
        CurrentAssertionChain.BecauseOf(because, becauseArgs)
            .ForCondition(Subject.IsFailure)
            .FailWith(() => new FailReason("Expected {context:result} to fail, but it succeeded"))
            .Then
            .Given(() => Subject.Error)
            .ForCondition(e => e!.Equals(error))
            .FailWith("Expected {context:result} error to be {0}, but found {1}", error, Subject.Error);

        return new AndWhichConstraint<ResultAssertions<T>, string>(this, Subject.Error);
    }
}