using System.Collections.ObjectModel;
using FluentAssertions;
using FluentAssertions.Collections;
using FluentAssertions.Primitives;
using static CSharpFunctionalExtensions.AwesomeAssertions.Tests.GenericHelper;

namespace CSharpFunctionalExtensions.AwesomeAssertions.Tests.ResultT;

public class ResultTGenExtTests
{
    public static IEnumerable<object?[]> Primitive => ExtendedType.Primitive;
    public static IEnumerable<object?[]> GenericAssertions => ExtendedType.GenericAssertions;

    [Theory]
    [MemberData(nameof(Primitive))]
    public void TestResultSuccessShouldPrimitiveExtensions<T>(T value, Type typeOfValue, Type expectedAssertionType)
    {
        // Arrange
        const string methodName = "SuccessShould";
        var result = CreateSuccessResultT(typeOfValue, value!);

        // Act
        var assertionMethod = GetMethodLeadingToAssertionTypeFromTargetTypeAndAssert(
            typeof(Result<>), typeOfValue, methodName);

        // Assert
        var assertionObj = assertionMethod.Invoke(null, [result]);

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
        var result = CreateSuccessResultT(typeOfValue, value!);

        // Act
        var assertionMethod = GetMethodLeadingToAssertionTypeFromTargetTypeAndAssert(
            typeof(Result<>), typeOfValue, methodName);

        // Assert
        var assertionObj = assertionMethod.Invoke(null, [result]);

        assertionObj.Should().NotBeNull("because the method should return an assertion type").And
            .BeOfType(expectedAssertionType, $"because the method should return an {expectedAssertionType.Name} type");
    }

    [Fact]
    public void TestResultSuccessShouldGenericTypesWithGenericAssertionsExtensions()
    {
        // Arrange
        var resultIEnumerable = Result.Success<IEnumerable<int>>([1, 2, 3]);
        var resultICollection = Result.Success<ICollection<int>>([1, 2, 3]);
        var resultIList = Result.Success<IList<int>>([1, 2, 3]);
        var resultList = Result.Success<List<int>>([1, 2, 3]);
        var resultArray = Result.Success<int[]>([1, 2, 3]);
        var resultArraySegment = Result.Success(new ArraySegment<int>([1, 2, 3]));
        var resultIReadOnlyList = Result.Success<IReadOnlyList<int>>([1, 2, 3]);
        var resultIReadOnlyCollection = Result.Success<IReadOnlyCollection<int>>([1, 2, 3]);
        var resultReadOnlyCollection = Result.Success(new ReadOnlyCollection<int>([1, 2, 3]));
        var resultEnum = Result.Success(ExtendedType.TestEnum.First);
        var resultNullableEnum = Result.Success<ExtendedType.TestEnum?>(ExtendedType.TestEnum.First);

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
        var enumAssertion = resultEnum.SuccessShould<ExtendedType.TestEnum, int>();
        var nullableEnumAssertion = resultNullableEnum.SuccessShould<ExtendedType.TestEnum, int>();

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