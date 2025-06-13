using System.Collections.ObjectModel;
using AwesomeAssertions;
using AwesomeAssertions.Collections;
using AwesomeAssertions.Primitives;
using static CSharpFunctionalExtensions.AwesomeAssertions.Tests.GenericHelper;

namespace CSharpFunctionalExtensions.AwesomeAssertions.Tests.MaybeT;

public class MaybeTGenExtTests
{
    public static IEnumerable<object?[]> Primitive => ExtendedType.Primitive;
    public static IEnumerable<object?[]> GenericAssertions => ExtendedType.GenericAssertions;

    [Theory]
    [MemberData(nameof(Primitive))]
    public void TestMaybePrimitiveExtensions<T>(T value, Type typeOfValue, Type expectedAssertionType)
    {
        // Arrange
        const string methodName = "ValueShould";
        var maybe = MakeGenericMaybeFromCallingFromMethodAndAssert(typeOfValue, value!);

        // Act
        var assertionType = GetMethodLeadingToAssertionTypeFromTargetTypeAndAssert(
            typeof(Maybe<>), typeOfValue, methodName
        );

        // Assert
        var result = assertionType.Invoke(null, [maybe]);

        result.Should().NotBeNull("because the method should return an assertion type").And
            .BeOfType(expectedAssertionType, $"because the method should return an {expectedAssertionType.Name} type");
    }

    [Theory]
    [MemberData(nameof(GenericAssertions))]
    public void TestMaybePrimitiveWithGenericAssertionsExtensions<T>(T value, Type typeOfValue,
                                                                     Type expectedAssertionType)
    {
        const string methodName = "ValueShould";
        var maybe = MakeGenericMaybeFromCallingFromMethodAndAssert(typeOfValue, value!);

        // Act
        var assertionTypeMethod = GetMethodLeadingToAssertionTypeFromTargetTypeAndAssert(
            typeof(Maybe<>), typeOfValue, methodName
        );

        // Invoke the method to get the assertion type
        var result = assertionTypeMethod.Invoke(null, [maybe]);

        // Assert
        result.Should().NotBeNull("because the method should return an assertion type").And
            .BeOfType(expectedAssertionType, $"because the method should return an {expectedAssertionType.Name} type");
    }

    [Fact]
    public void TestMaybeGenericTypesWithGenericAssertionsExtensions()
    {
        // Arrange
        var maybeIEnumerable = Maybe<IEnumerable<int>>.From([1, 2, 3]);
        var maybeICollection = Maybe<ICollection<int>>.From([1, 2, 3]);
        var maybeIList = Maybe<IList<int>>.From([1, 2, 3]);
        var maybeList = Maybe<List<int>>.From([1, 2, 3]);
        var maybeArray = Maybe<int[]>.From([1, 2, 3]);
        var maybeArraySegment = Maybe<ArraySegment<int>>.From(new ArraySegment<int>());
        var maybeIReadOnlyList = Maybe<IReadOnlyList<int>>.From([1, 2, 3]);
        var maybeIReadOnlyCollection = Maybe<IReadOnlyCollection<int>>.From([1, 2, 3]);
        var maybeReadOnlyCollection = Maybe<ReadOnlyCollection<int>>.From(new ReadOnlyCollection<int>([1, 2, 3]));
        var maybeEnum = Maybe<ExtendedType.TestEnum>.From(ExtendedType.TestEnum.First);
        var maybeNullableEnum = Maybe<ExtendedType.TestEnum?>.From(ExtendedType.TestEnum.First);
        var maybeUnknownType = Maybe<ExtendedType.TestType>.From(new ExtendedType.TestType());

        // Act
        var iEnumerableAssertion = maybeIEnumerable.ValueShould();
        var iCollectionAssertion = maybeICollection.ValueShould();
        var iListAssertion = maybeIList.ValueShould();
        var listAssertion = maybeList.ValueShould();
        var arrayAssertion = maybeArray.ValueShould();
        var arraySegmentAssertion = maybeArraySegment.ValueShould();
        var iReadOnlyListAssertion = maybeIReadOnlyList.ValueShould();
        var iReadOnlyCollectionAssertion = maybeIReadOnlyCollection.ValueShould();
        var readOnlyCollectionAssertion = maybeReadOnlyCollection.ValueShould();
        var enumAssertion = maybeEnum.ValueShould<ExtendedType.TestEnum, int>();
        var nullableEnumAssertion = maybeNullableEnum.ValueShould<ExtendedType.TestEnum, int>();
        var unknownTypeAssertion = maybeUnknownType.ValueShould();

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