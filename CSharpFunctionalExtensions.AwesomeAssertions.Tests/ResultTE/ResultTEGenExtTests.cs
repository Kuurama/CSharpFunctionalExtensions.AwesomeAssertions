using System.Collections.ObjectModel;
using FluentAssertions;
using FluentAssertions.Collections;
using FluentAssertions.Primitives;
using static CSharpFunctionalExtensions.AwesomeAssertions.Tests.GenericHelper;

namespace CSharpFunctionalExtensions.AwesomeAssertions.Tests.ResultTE;

public class ResultTEGenExtTests
{
    public static IEnumerable<object?[]> Primitive => ExtendedType.Primitive;
    public static IEnumerable<object?[]> GenericAssertions => ExtendedType.GenericAssertions;

    [Theory]
    [MemberData(nameof(Primitive))]
    public void TestResultSuccessShouldPrimitiveExtensions<T>(T value, Type typeOfValue, Type expectedAssertionType)
    {
        // Arrange
        const string methodName = "SuccessShould";
        var result = CreateSuccessResultTEWithValue(typeOfValue, value!, typeof(string));

        // Act
        var assertionMethod = GetMethodLeadingToAssertionTypeFromTargetTypeWithGenericSecondParamAndAssert(
            typeof(Result<,>), typeOfValue, methodName);

        // Assert
        // Late Bound need generic type made
        var assertion = assertionMethod.MakeGenericMethod(typeof(string));
        var assertionObj = assertion.Invoke(null, [result]);

        assertionObj.Should().NotBeNull("because the method should return an assertion type").And
            .BeOfType(expectedAssertionType, $"because the method should return an {expectedAssertionType.Name} type");
    }

    [Theory]
    [MemberData(nameof(Primitive))]
    public void TestResultFailureShouldPrimitiveExtensions<T>(T value, Type typeOfValue, Type expectedAssertionType)
    {
        // Arrange
        const string methodName = "FailureShould";
        var result = CreateFailureResultTEWithError(typeof(string), typeOfValue, value!);

        // Act
        var assertionMethod = GetMethodLeadingToAssertionTypeFromTargetTypeWithGenericFirstParamAndAssert(
            typeof(Result<,>), typeOfValue, methodName);

        // Assert
        // Late Bound need generic type made
        var assertion = assertionMethod.MakeGenericMethod(typeof(string));
        var assertionObj = assertion.Invoke(null, [result]);

        assertionObj.Should().NotBeNull("because the method should return an assertion type").And
            .BeOfType(expectedAssertionType, $"because the method should return an {expectedAssertionType.Name} type");
    }

    [Theory]
    [MemberData(nameof(GenericAssertions))]
    public void TestResultSuccessShouldGenericAssertionsExtensions<T>(T value, Type typeOfValue,
                                                                      Type expectedAssertionType)
    {
        // Arrange
        const string methodName = "SuccessShould";
        var result = CreateSuccessResultTEWithValue(typeOfValue, value!, typeof(string));

        // Act
        var assertionMethod = GetMethodLeadingToAssertionTypeFromTargetTypeWithGenericSecondParamAndAssert(
            typeof(Result<,>), typeOfValue, methodName);

        // Assert
        // Late Bound need generic type made
        var assertion = assertionMethod.MakeGenericMethod(typeof(string));
        var assertionObj = assertion.Invoke(null, [result]);

        assertionObj.Should().NotBeNull("because the method should return an assertion type").And
            .BeOfType(expectedAssertionType, $"because the method should return an {expectedAssertionType.Name} type");
    }

    [Theory]
    [MemberData(nameof(GenericAssertions))]
    public void TestResultFailureShouldGenericAssertionsExtensions<T>(T value, Type typeOfValue,
                                                                      Type expectedAssertionType)
    {
        // Arrange
        const string methodName = "FailureShould";
        var result = CreateFailureResultTEWithError(typeof(string), typeOfValue, value!);

        // Act
        var assertionMethod = GetMethodLeadingToAssertionTypeFromTargetTypeWithGenericFirstParamAndAssert(
            typeof(Result<,>), typeOfValue, methodName);

        // Assert
        // Late Bound need generic type made
        var assertion = assertionMethod.MakeGenericMethod(typeof(string));
        var assertionObj = assertion.Invoke(null, [result]);

        assertionObj.Should().NotBeNull("because the method should return an assertion type").And
            .BeOfType(expectedAssertionType, $"because the method should return an {expectedAssertionType.Name} type");
    }

    [Fact]
    public void TestResultSuccessShouldGenericTypesWithGenericAssertionsExtensions()
    {
        // Arrange
        var resultIEnumerable = Result.Success<IEnumerable<int>, string>([1, 2, 3]);
        var resultICollection = Result.Success<ICollection<int>, string>([1, 2, 3]);
        var resultIList = Result.Success<IList<int>, string>([1, 2, 3]);
        var resultList = Result.Success<List<int>, string>([1, 2, 3]);
        var resultArray = Result.Success<int[], string>([1, 2, 3]);
        var resultArraySegment = Result.Success<ArraySegment<int>, string>(new ArraySegment<int>([1, 2, 3]));
        var resultIReadOnlyList = Result.Success<IReadOnlyList<int>, string>([1, 2, 3]);
        var resultIReadOnlyCollection = Result.Success<IReadOnlyCollection<int>, string>([1, 2, 3]);
        var resultReadOnlyCollection = Result.Success<ReadOnlyCollection<int>, string>(
            new ReadOnlyCollection<int>([1, 2, 3])
        );
        var resultEnum = Result.Success<ExtendedType.TestEnum, string>(ExtendedType.TestEnum.First);
        var resultNullableEnum = Result.Success<ExtendedType.TestEnum?, string>(ExtendedType.TestEnum.First);

        // Act
        var iEnumerableAssertion = resultIEnumerable.SuccessShould();
        var iCollectionAssertion = resultICollection.SuccessShould();
        var iListAssertion = resultIList.SuccessShould();
        var listAssertion = resultList.SuccessShould();
        var arrayAssertion = resultArray.SuccessShould();
        var arraySegmentAssertion = resultArraySegment.SuccessShould();
        var iReadOnlyListAssertion = resultIReadOnlyList.SuccessShould();
        var iReadOnlyCollectionAssertion = resultIReadOnlyCollection.SuccessShould();
        var readOnlyCollectionAssertion = resultReadOnlyCollection.SuccessShould();
        var enumAssertion = resultEnum.SuccessShould<ExtendedType.TestEnum, string, int>();
        var nullableEnumAssertion = resultNullableEnum.SuccessShould<ExtendedType.TestEnum, string, int>();

        // Assert
        Assert.Equal(
            typeof(GenericCollectionAssertions<int>),
            iEnumerableAssertion.GetType()
        );
        Assert.Equal(
            typeof(GenericCollectionAssertions<int>),
            iCollectionAssertion.GetType()
        );
        Assert.Equal(
            typeof(GenericCollectionAssertions<int>),
            iListAssertion.GetType()
        );
        Assert.Equal(
            typeof(GenericCollectionAssertions<int>),
            listAssertion.GetType()
        );
        Assert.Equal(
            typeof(GenericCollectionAssertions<int>),
            arrayAssertion.GetType()
        );
        Assert.Equal(
            typeof(GenericCollectionAssertions<int>),
            arraySegmentAssertion.GetType()
        );
        Assert.Equal(
            typeof(GenericCollectionAssertions<int>),
            iReadOnlyListAssertion.GetType()
        );
        Assert.Equal(
            typeof(GenericCollectionAssertions<int>),
            iReadOnlyCollectionAssertion.GetType()
        );
        Assert.Equal(
            typeof(GenericCollectionAssertions<int>),
            readOnlyCollectionAssertion.GetType()
        );
        Assert.Equal(
            typeof(EnumAssertions<ExtendedType.TestEnum>),
            enumAssertion.GetType()
        );
        Assert.Equal(
            typeof(NullableEnumAssertions<ExtendedType.TestEnum>),
            nullableEnumAssertion.GetType()
        );
    }

    [Fact]
    public void TestResultFailureShouldGenericTypesWithGenericAssertionsExtensions()
    {
        // Arrange
        var resultIEnumerable = Result.Failure<string, IEnumerable<int>>([1, 2, 3]);
        var resultICollection = Result.Failure<string, ICollection<int>>([1, 2, 3]);
        var resultIList = Result.Failure<string, IList<int>>([1, 2, 3]);
        var resultList = Result.Failure<string, List<int>>([1, 2, 3]);
        var resultArray = Result.Failure<string, int[]>([1, 2, 3]);
        var resultArraySegment = Result.Failure<string, ArraySegment<int>>(new ArraySegment<int>([1, 2, 3]));
        var resultIReadOnlyList = Result.Failure<string, IReadOnlyList<int>>([1, 2, 3]);
        var resultIReadOnlyCollection = Result.Failure<string, IReadOnlyCollection<int>>([1, 2, 3]);
        var resultReadOnlyCollection = Result.Failure<string, ReadOnlyCollection<int>>(
            new ReadOnlyCollection<int>([1, 2, 3])
        );
        var resultEnum = Result.Failure<string, ExtendedType.TestEnum>(ExtendedType.TestEnum.First);
        var resultNullableEnum = Result.Failure<string, ExtendedType.TestEnum?>(ExtendedType.TestEnum.First);


        // Act
        var iEnumerableAssertion = resultIEnumerable.FailureShould();
        var iCollectionAssertion = resultICollection.FailureShould();
        var iListAssertion = resultIList.FailureShould();
        var listAssertion = resultList.FailureShould();
        var arrayAssertion = resultArray.FailureShould();
        var arraySegmentAssertion = resultArraySegment.FailureShould();
        var iReadOnlyListAssertion = resultIReadOnlyList.FailureShould();
        var iReadOnlyCollectionAssertion = resultIReadOnlyCollection.FailureShould();
        var readOnlyCollectionAssertion = resultReadOnlyCollection.FailureShould();
        var enumAssertion = resultEnum.FailureShould<string, ExtendedType.TestEnum, int>();
        var nullableEnumAssertion = resultNullableEnum.FailureShould<string, ExtendedType.TestEnum, int>();

        // Assert
        Assert.Equal(
            typeof(GenericCollectionAssertions<int>),
            iEnumerableAssertion.GetType()
        );
        Assert.Equal(
            typeof(GenericCollectionAssertions<int>),
            iCollectionAssertion.GetType()
        );
        Assert.Equal(
            typeof(GenericCollectionAssertions<int>),
            iListAssertion.GetType()
        );
        Assert.Equal(
            typeof(GenericCollectionAssertions<int>),
            listAssertion.GetType()
        );
        Assert.Equal(
            typeof(GenericCollectionAssertions<int>),
            arrayAssertion.GetType()
        );
        Assert.Equal(
            typeof(GenericCollectionAssertions<int>),
            arraySegmentAssertion.GetType()
        );
        Assert.Equal(
            typeof(GenericCollectionAssertions<int>),
            iReadOnlyListAssertion.GetType()
        );
        Assert.Equal(
            typeof(GenericCollectionAssertions<int>),
            iReadOnlyCollectionAssertion.GetType()
        );
        Assert.Equal(
            typeof(GenericCollectionAssertions<int>),
            readOnlyCollectionAssertion.GetType()
        );
        Assert.Equal(
            typeof(EnumAssertions<ExtendedType.TestEnum>),
            enumAssertion.GetType()
        );
        Assert.Equal(
            typeof(NullableEnumAssertions<ExtendedType.TestEnum>),
            nullableEnumAssertion.GetType()
        );
    }
}