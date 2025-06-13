using System.Reflection;
using AwesomeAssertions;

namespace CSharpFunctionalExtensions.AwesomeAssertions.Tests;

public static class GenericHelper
{
    /// <summary>
    /// Creates a closed generic Maybe{T} type from the provided type and value.
    /// </summary>
    /// <param name="type">The type of <paramref name="value" /></param>
    /// <param name="value">The value to be wrapped in a Maybe{T} instance.</param>
    /// <returns>A type erased Maybe{<paramref name="type" />}</returns>
    /// <remarks>
    /// This method asserts that:
    /// - The From method exists for the specified type.
    /// - The From method returns a Maybe{T} instance that's not null.
    /// </remarks>
    public static object MakeGenericMaybeFromCallingFromMethodAndAssert(Type type, object value)
    {
        // Create the closed generic Maybe<T> type
        var maybeType = typeof(Maybe<>).MakeGenericType(type);

        // Find the correct From method
        var fromMethod = maybeType.GetMethod("From", BindingFlags.Public | BindingFlags.Static,
            null, [type], null);

        fromMethod.Should().NotBeNull($"because the From method should exist for type {type.Name}");

        // Invoke From to create a Maybe<T> instance
        var maybe = fromMethod.Invoke(null, [value]);
        maybe.Should().NotBeNull($"because the From method should return a Maybe<{type.Name}> instance");

        return maybe;
    }


    /// <summary>
    /// Finds a static extension method in the GeneratedExtensions class that accepts a specific generic type.
    /// </summary>
    /// <param name="targetType">The generic type definition (e.g., Maybe{T}) to match in the first parameter.</param>
    /// <param name="underlyingType">The specific type argument used in the generic parameter type.</param>
    /// <param name="methodName">The name of the method to find.</param>
    /// <returns>A MethodInfo representing the found extension method.</returns>
    /// <remarks>
    /// This method asserts that:
    /// - The method with the specified name exists for the given target and underlying types.
    /// - The first parameter of the method is of type targetType{underlyingType}.
    /// </remarks>
    public static MethodInfo GetMethodLeadingToAssertionTypeFromTargetTypeAndAssert(
        Type targetType, Type underlyingType, string methodName)
    {
        // Find the correct assertion type
        var assertionMethod = typeof(GeneratedExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.Name == methodName)
            .FirstOrDefault(m => m.GetParameters().Length > 0 &&
                                 m.GetParameters()[0].ParameterType.IsGenericType &&
                                 m.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == targetType &&
                                 m.GetParameters()[0].ParameterType.GetGenericArguments()[0] == underlyingType);

        assertionMethod.Should().NotBeNull(
            $"because the {methodName} method should exist for type {targetType.Name}<{underlyingType.Name}>"
        );

        return assertionMethod;
    }

    /// <summary>
    /// Creates a Result{T} success instance with the specified value type and value.
    /// </summary>
    /// <param name="valueType">The generic type parameter for Result{T}.</param>
    /// <param name="value">The success value to be wrapped in the Result{T}.</param>
    /// <returns>A type-erased Result{T} success instance.</returns>
    /// <remarks>
    /// This method asserts that:
    /// - The Success{T} method exists on the Result class.
    /// - The Success{T} method returns a Result{T} instance that's not null.
    /// </remarks>
    public static object CreateSuccessResultT(Type valueType, object value)
    {
        // Find the Success<T> method
        var successMethod = typeof(Result).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m is { Name: "Success", IsGenericMethod: true })
            .Where(m => m.GetGenericArguments().Length == 1)     // Has one generic parameter (T)
            .FirstOrDefault(m => m.GetParameters().Length == 1); // Takes one parameter

        successMethod.Should().NotBeNull("because the Success<T> method should exist on Result");

        // Create the closed generic method
        var genericSuccessMethod = successMethod.MakeGenericMethod(valueType);

        // Call Success<valueType>(value)
        var result = genericSuccessMethod.Invoke(null, [value]);
        result.Should().NotBeNull($"because Success<{valueType.Name}> should create a Result instance");

        return result;
    }

    /// <summary>
    /// Creates a UnitResult{E} failure instance with the specified error type and error value.
    /// </summary>
    /// <param name="errorType">The generic type parameter for UnitResult{E}.</param>
    /// <param name="error">The error value to be wrapped in the UnitResult{E}.</param>
    /// <returns>A type-erased UnitResult{E} failure instance.</returns>
    /// <remarks>
    /// This method asserts that:
    /// - The Failure{E} method exists on the UnitResult class.
    /// - The Failure{E} method returns a UnitResult{E} instance that's not null.
    /// </remarks>
    public static object CreateFailureUnitResultE(Type errorType, object error)
    {
        // Find the Failure<E> method
        var failureMethod = typeof(UnitResult).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m is { Name: "Failure", IsGenericMethod: true })
            .Where(m => m.GetGenericArguments().Length == 1)     // Has one generic parameter (E)
            .FirstOrDefault(m => m.GetParameters().Length == 1); // Takes one parameter

        failureMethod.Should().NotBeNull("because the Failure<E> method should exist on UnitResult");

        // Create the closed generic method
        var genericFailureMethod = failureMethod.MakeGenericMethod(errorType);

        // Call Failure<errorType>(error)
        var result = genericFailureMethod.Invoke(null, [error]);
        result.Should().NotBeNull($"because Failure<{errorType.Name}> should create a UnitResult instance");

        return result;
    }

    /// <summary>
    /// Creates a Result{T,E} success instance with the specified value type, value, and error type.
    /// </summary>
    /// <param name="valueType">The first generic type parameter for Result{T,E}.</param>
    /// <param name="value">The success value to be wrapped in the Result{T,E}.</param>
    /// <param name="errorType">The second generic type parameter for Result{T,E}.</param>
    /// <returns>A type-erased Result{T,E} success instance.</returns>
    /// <remarks>
    /// This method asserts that:
    /// - The Success{T,E} method exists on the Result class.
    /// - The Success{T,E} method returns a Result{T,E} instance that's not null.
    /// </remarks>
    public static object CreateSuccessResultTEWithValue(Type valueType, object value, Type errorType)
    {
        // Find the Success<T,E> method
        var successMethod = typeof(Result).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.Name == "Success" && m.IsGenericMethod)
            .Where(m => m.GetGenericArguments().Length == 2)     // Has two generic parameters (T,E)
            .FirstOrDefault(m => m.GetParameters().Length == 1); // Takes one parameter

        successMethod.Should().NotBeNull("because the Success<T,E> method should exist on Result");

        // Create the closed generic method
        var genericSuccessMethod = successMethod.MakeGenericMethod(valueType, errorType);

        // Call Success<valueType, errorType>(value)
        var result = genericSuccessMethod.Invoke(null, [value]);
        result.Should()
            .NotBeNull($"because Success<{valueType.Name}, {errorType.Name}> should create a Result instance");

        return result;
    }

    /// <summary>
    /// Creates a Result{T,E} failure instance with the specified value type, error type, and error value.
    /// </summary>
    /// <param name="valueType">The first generic type parameter for Result{T,E}.</param>
    /// <param name="errorType">The second generic type parameter for Result{T,E}.</param>
    /// <param name="error">The error value to be wrapped in the Result{T,E}.</param>
    /// <returns>A type-erased Result{T,E} failure instance.</returns>
    /// <remarks>
    /// This method asserts that:
    /// - The Failure{T,E} method exists on the Result class.
    /// - The Failure{T,E} method returns a Result{T,E} instance that's not null.
    /// </remarks>
    public static object CreateFailureResultTEWithError(Type valueType, Type errorType, object error)
    {
        // Find the Failure<T,E> method
        var failureMethod = typeof(Result).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m is { Name: "Failure", IsGenericMethod: true })
            .Where(m => m.GetGenericArguments().Length == 2)     // Has two generic parameters (T,E)
            .FirstOrDefault(m => m.GetParameters().Length == 1); // Takes one parameter

        failureMethod.Should().NotBeNull("because the Failure<T,E> method should exist on Result");

        // Create the closed generic method
        var genericFailureMethod = failureMethod.MakeGenericMethod(valueType, errorType);

        // Call Failure<valueType, errorType>(error)
        var result = genericFailureMethod.Invoke(null, [error]);
        result.Should()
            .NotBeNull($"because Failure<{valueType.Name}, {errorType.Name}> should create a Result instance");

        return result;
    }

    /// <summary>
    /// Finds a static extension method in the GeneratedExtensions class that accepts a specific generic type
    /// with a focus on the first generic parameter.
    /// </summary>
    /// <param name="targetType">The generic type definition (e.g., Result{T,E}) to match in the first parameter.</param>
    /// <param name="firstGenericParamType">The specific type argument used in the first generic parameter position.</param>
    /// <param name="methodName">The name of the method to find.</param>
    /// <returns>A MethodInfo representing the found extension method.</returns>
    /// <remarks>
    /// This method asserts that:
    /// - The method with the specified name exists for the given target type with firstGenericParamType as its first generic
    /// argument.
    /// - The first parameter of the method is of type targetType{firstGenericParamType,E} for some E.
    /// </remarks>
    public static MethodInfo GetMethodLeadingToAssertionTypeFromTargetTypeWithGenericSecondParamAndAssert(
        Type targetType, Type firstGenericParamType, string methodName)
    {
        // Find the correct assertion method
        var assertionMethod = typeof(GeneratedExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.Name == methodName)
            .FirstOrDefault(m => m.GetParameters().Length > 0 &&
                                 m.GetParameters()[0].ParameterType.IsGenericType &&
                                 m.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == targetType
                                 && m.GetParameters()[0].ParameterType.GenericTypeArguments[0] ==
                                 firstGenericParamType);

        assertionMethod.Should().NotBeNull(
            $"because the {methodName} method should exist for Result<{firstGenericParamType.Name}, E>");

        return assertionMethod;
    }

    /// <summary>
    /// Finds a static extension method in the GeneratedExtensions class that accepts a specific generic type
    /// with a focus on the second generic parameter.
    /// </summary>
    /// <param name="targetType">The generic type definition (e.g., Result{T,E}) to match in the first parameter.</param>
    /// <param name="secondGenericParamType">The specific type argument used in the second generic parameter position.</param>
    /// <param name="methodName">The name of the method to find.</param>
    /// <returns>A MethodInfo representing the found extension method.</returns>
    /// <remarks>
    /// This method asserts that:
    /// - The method with the specified name exists for the given target type with secondGenericParamType as its second generic
    /// argument.
    /// - The first parameter of the method is of type targetType{T,secondGenericParamType} for some T.
    /// </remarks>
    public static MethodInfo GetMethodLeadingToAssertionTypeFromTargetTypeWithGenericFirstParamAndAssert(
        Type targetType, Type secondGenericParamType, string methodName)
    {
        // Find the correct assertion method
        var assertionMethod = typeof(GeneratedExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.Name == methodName)
            .FirstOrDefault(m => m.GetParameters().Length > 0 &&
                                 m.GetParameters()[0].ParameterType.IsGenericType &&
                                 m.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == targetType
                                 && m.GetParameters()[0].ParameterType.GenericTypeArguments[1] ==
                                 secondGenericParamType);

        assertionMethod.Should().NotBeNull(
            $"because the {methodName} method should exist for {targetType.Name}<T, {secondGenericParamType.Name}>");

        return assertionMethod;
    }
}