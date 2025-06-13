using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.AwesomeAssertions.Generator;
using AwesomeAssertions.Execution;
using AwesomeAssertions.Primitives;

// ReSharper disable once CheckNamespace
namespace AwesomeAssertions;

[DebuggerNonUserCode]
[GeneratePrimitiveExtensions("ValueShould", "Value", "CSharpFunctionalExtensions.Maybe`1")]
public static class MaybeExtensions
{
    public static MaybeAssertions<T> Should<T>(this Maybe<T> instance)
        => new(instance, AssertionChain.GetOrCreate());
}

[DebuggerNonUserCode]
public class MaybeAssertions<T>(Maybe<T> instance, AssertionChain chain)
    : ReferenceTypeAssertions<Maybe<T>, MaybeAssertions<T>>(instance, chain)
{
    [ExcludeFromCodeCoverage]
    protected override string Identifier => "Maybe<T>";

    /// <summary>
    /// Asserts that the current <see cref="Maybe{T}" /> has some value.
    /// </summary>
    /// <param name="because"></param>
    /// <param name="becauseArgs"></param>
    /// <returns></returns>
    [CustomAssertion]
    public AndWhichConstraint<MaybeAssertions<T>, T> HaveSomeValue(string because = "", params object[] becauseArgs)
    {
        CurrentAssertionChain.BecauseOf(because, becauseArgs)
            .Given(() => Subject)
            .ForCondition(v => v.HasValue)
            .FailWith("Expected a value {reason}");

        return new AndWhichConstraint<MaybeAssertions<T>, T>(this, Subject.Value);
    }

    /// <summary>
    /// Asserts that the current <see cref="Maybe{T}" /> has a value.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="because"></param>
    /// <param name="becauseArgs"></param>
    /// <returns></returns>
    [CustomAssertion]
    public AndWhichConstraint<MaybeAssertions<T>, T> HaveValue(T value, string because = "",
                                                               params object[] becauseArgs)
    {
        CurrentAssertionChain.BecauseOf(because, becauseArgs)
            .Given(() => Subject)
            .ForCondition(v => v.HasValue)
            .FailWith(
                "Expected a value {0}{reason}",
                _ => value)
            .Then
            .Given(s => s.Value)
            .ForCondition(v => v!.Equals(value))
            .FailWith(
                "Expected {context:maybe} to have value {0}{reason}, but with value {1} it",
                _ => value,
                v => v);

        return new AndWhichConstraint<MaybeAssertions<T>, T>(this, Subject.Value);
    }

    /// <summary>
    /// Asserts that the current <see cref="Maybe{T}" /> has no value.
    /// </summary>
    /// <param name="because"></param>
    /// <param name="becauseArgs"></param>
    /// <returns></returns>
    [CustomAssertion]
    public AndConstraint<MaybeAssertions<T>> HaveNoValue(string because = "", params object[] becauseArgs)
    {
        CurrentAssertionChain.BecauseOf(because, becauseArgs)
            .Given(() => Subject)
            .ForCondition(v => v.HasNoValue)
            .FailWith(
                "Expected {context:maybe} to have no value{reason}, but with value {0} it",
                v => v.HasNoValue ? default : v.Value);

        return new AndConstraint<MaybeAssertions<T>>(this);
    }
}