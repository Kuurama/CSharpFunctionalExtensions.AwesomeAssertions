using System.Collections.ObjectModel;
using FluentAssertions;
using FluentAssertions.Collections;
using FluentAssertions.Primitives;
using static CSharpFunctionalExtensions.AwesomeAssertions.Tests.GenericHelper;

namespace CSharpFunctionalExtensions.AwesomeAssertions.Tests.UnitResultE;

public class UnitResultEGenExtTests
{
    public static IEnumerable<object?[]> Primitive => ExtendedType.Primitive;
    public static IEnumerable<object?[]> GenericAssertions => ExtendedType.GenericAssertions;

    [Theory]
    [MemberData(nameof(Primitive))]
    public void TestUnitResultFailureShouldPrimitiveExtensions<E>(E error, Type errorType, Type expectedAssertionType)
    {
        // Arrange
        const string methodName = "FailureShould";
        var result = CreateFailureUnitResultE(errorType, error!);

        // Act
        var assertionMethod = GetMethodLeadingToAssertionTypeFromTargetTypeAndAssert(
            typeof(UnitResult<>), errorType, methodName);

        // Assert
        var assertionObj = assertionMethod.Invoke(null, [result]);

        assertionObj.Should().NotBeNull("because the method should return an assertion type").And
            .BeOfType(expectedAssertionType, $"because the method should return an {expectedAssertionType.Name} type");
    }

    [Theory]
    [MemberData(nameof(GenericAssertions))]
    public void TestUnitResultFailureShouldGenericAssertionsExtensions<E>(
        E error, Type errorType, Type expectedAssertionType)
    {
        // Arrange
        const string methodName = "FailureShould";
        var result = CreateFailureUnitResultE(errorType, error!);

        // Act
        var assertionMethod = GetMethodLeadingToAssertionTypeFromTargetTypeAndAssert(
            typeof(UnitResult<>), errorType, methodName);

        // Assert
        var assertionObj = assertionMethod.Invoke(null, [result]);

        assertionObj.Should().NotBeNull("because the method should return an assertion type").And
            .BeOfType(expectedAssertionType, $"because the method should return an {expectedAssertionType.Name} type");
    }

    [Fact]
    public void TestUnitResultFailureShouldGenericTypesWithGenericAssertionsExtensions()
    {
        // Arrange
        var resultIEnumerable = UnitResult.Failure<IEnumerable<int>>([1, 2, 3]);
        var resultICollection = UnitResult.Failure<ICollection<int>>([1, 2, 3]);
        var resultIList = UnitResult.Failure<IList<int>>([1, 2, 3]);
        var resultList = UnitResult.Failure<List<int>>([1, 2, 3]);
        var resultArray = UnitResult.Failure<int[]>([1, 2, 3]);
        var resultArraySegment = UnitResult.Failure(new ArraySegment<int>([1, 2, 3]));
        var resultIReadOnlyList = UnitResult.Failure<IReadOnlyList<int>>([1, 2, 3]);
        var resultIReadOnlyCollection = UnitResult.Failure<IReadOnlyCollection<int>>([1, 2, 3]);
        var resultReadOnlyCollection = UnitResult.Failure(new ReadOnlyCollection<int>([1, 2, 3]));
        var resultEnum = UnitResult.Failure(ExtendedType.TestEnum.First);
        var resultNullableEnum = UnitResult.Failure<ExtendedType.TestEnum?>(ExtendedType.TestEnum.First);
        var resultUnknownType = UnitResult.Failure(new ExtendedType.TestType());

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
        var enumAssertion = resultEnum.FailureShould<ExtendedType.TestEnum, int>();
        var nullableEnumAssertion = resultNullableEnum.FailureShould<ExtendedType.TestEnum, int>();
        var unknownTypeAssertion = resultUnknownType.FailureShould();

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
        Assert.Equal(
            typeof(ObjectAssertions),
            unknownTypeAssertion.GetType()
        );
    }
}