using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FluentAssertions.Numeric;
using FluentAssertions.Primitives;
using FluentAssertions.Types;

namespace CSharpFunctionalExtensions.AwesomeAssertions.Tests;

public static class ExtendedType
{
    public static readonly IEnumerable<object?[]> Primitive =
    [
        new TypedArray<string>("test", typeof(StringAssertions)),
        new TypedArray<bool>(true, typeof(BooleanAssertions)),
        new TypedArray<bool?>(true, typeof(NullableBooleanAssertions)),
        new TypedArray<DateOnly>(DateOnly.MinValue, typeof(DateOnlyAssertions)),
        new TypedArray<DateOnly?>(DateOnly.MinValue, typeof(NullableDateOnlyAssertions)),
        new TypedArray<TimeOnly>(TimeOnly.MinValue, typeof(TimeOnlyAssertions)),
        new TypedArray<TimeOnly?>(TimeOnly.MinValue, typeof(NullableTimeOnlyAssertions)),
        new TypedArray<DateTime>(DateTime.MinValue, typeof(DateTimeAssertions)),
        new TypedArray<DateTime?>(DateTime.MinValue, typeof(NullableDateTimeAssertions)),
        new TypedArray<DateTimeOffset>(DateTimeOffset.MinValue, typeof(DateTimeOffsetAssertions)),
        new TypedArray<DateTimeOffset?>(DateTimeOffset.MinValue, typeof(NullableDateTimeOffsetAssertions)),
        new TypedArray<Guid>(Guid.Empty, typeof(GuidAssertions)),
        new TypedArray<Guid?>(Guid.Empty, typeof(NullableGuidAssertions)),
        new TypedArray<TimeSpan>(TimeSpan.Zero, typeof(SimpleTimeSpanAssertions)),
        new TypedArray<TimeSpan?>(TimeSpan.Zero, typeof(NullableSimpleTimeSpanAssertions)),
        new TypedArray<object>(new object(), typeof(ObjectAssertions)),
        new TypedArray<Type>(typeof(string), typeof(TypeAssertions)),
        new TypedArray<MethodInfo>(typeof(TypedArray<>).GetMethod(nameof(TypedArray<int>.TestMethod))!,
            typeof(MethodInfoAssertions)),
        new TypedArray<PropertyInfo>(typeof(TypedArray<>).GetProperty(nameof(TypedArray<int>.TestProperty))!,
            typeof(PropertyInfoAssertions))
    ];

    public static readonly IEnumerable<object?[]> GenericAssertions =
    [
        new TypedArray<int>(1, typeof(NumericAssertions<>)),
        new TypedArray<int?>(1, typeof(NullableNumericAssertions<>)),
        new TypedArray<long>(1, typeof(NumericAssertions<>)),
        new TypedArray<long?>(1, typeof(NullableNumericAssertions<>)),
        new TypedArray<short>(1, typeof(NumericAssertions<>)),
        new TypedArray<short?>(1, typeof(NullableNumericAssertions<>)),
        new TypedArray<ushort>(1, typeof(NumericAssertions<>)),
        new TypedArray<ushort?>(1, typeof(NullableNumericAssertions<>)),
        new TypedArray<byte>(1, typeof(NumericAssertions<>)),
        new TypedArray<byte?>(1, typeof(NullableNumericAssertions<>)),
        new TypedArray<sbyte>(1, typeof(NumericAssertions<>)),
        new TypedArray<sbyte?>(1, typeof(NullableNumericAssertions<>)),
        new TypedArray<char>(char.MaxValue, typeof(NumericAssertions<>)),
        new TypedArray<char?>(char.MaxValue, typeof(NullableNumericAssertions<>)),
        new TypedArray<uint>(1, typeof(NumericAssertions<>)),
        new TypedArray<uint?>(1, typeof(NullableNumericAssertions<>)),
        new TypedArray<ulong>(1, typeof(NumericAssertions<>)),
        new TypedArray<ulong?>(1, typeof(NullableNumericAssertions<>)),
        new TypedArray<float>(1, typeof(NumericAssertions<>)),
        new TypedArray<float?>(1, typeof(NullableNumericAssertions<>)),
        new TypedArray<double>(1, typeof(NumericAssertions<>)),
        new TypedArray<double?>(1, typeof(NullableNumericAssertions<>)),
        new TypedArray<decimal>(1, typeof(NumericAssertions<>)),
        new TypedArray<decimal?>(1, typeof(NullableNumericAssertions<>))
    ];

    [ExcludeFromCodeCoverage]
    public record TypedArray<T>(T First, Type TargetType)
    {
        public int TestProperty => 1;

        public static implicit operator object?[](TypedArray<T> source)
            => [source.First, typeof(T), source.TargetType];

        public void TestMethod() { }
    }

    public enum TestEnum
    {
        First,
        Second,
        Third
    }
}