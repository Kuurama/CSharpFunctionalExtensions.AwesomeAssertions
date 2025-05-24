# CSharpFunctionalExtensions.AwesomeAssertions

An **AwesomeAssertions** extension library 
for [CSharpFunctionalExtensions](https://github.com/vkhorikov/CSharpFunctionalExtensions), providing expressive,
type-safe, and discoverable assertions for `Result`, `Result<T>`, `Result<T, E>`, `Maybe<T>`, and `UnitResult<E>` types. Latest version available here: [![NuGet](https://img.shields.io/nuget/v/CSharpFunctionalExtensions.AwesomeAssertions.svg)](https://www.nuget.org/packages/CSharpFunctionalExtensions.AwesomeAssertions/)

## Features

- **Strongly-typed assertions** for all CSharpFunctionalExtensions result types.
- **Source-generated extension methods** for primitive, collection, and enum types, making assertions more discoverable
  and concise.
- **Seamless integration** with AwesomeAssertions.

---

## Getting Started

Add the NuGet package to your test project:

```shell
dotnet add package CSharpFunctionalExtensions.AwesomeAssertions
```

---

## Usage

### 1. Generated Extension Methods (Recommended)

The real power of this library comes from the **source-generated extension methods**. These methods are context-aware
and provide direct access to the underlying value or error, returning the most appropriate AwesomeAssertions/FluentAssertions assertion
type for the value. This makes your assertions more concise, type-safe, and discoverable.

#### Example: Asserting on the Value of a Result

```csharp
using FluentAssertions;
using CSharpFunctionalExtensions;

Result<string> result = Result.Success("hello");
result.SuccessShould().Be("hello");
result.SuccessShould().StartWith("he"); // Direct access to the StringAssertions (Or any other type of assertions based on the value)
```

#### Example: Asserting on the Error of a Result

```csharp
Result<string> result = Result.Failure("fail!");
result.FailureShould().Be("fail!");
result.FailureShould().Contain("fail"); // Direct access to the StringAssertions (Or any other type of assertions based on the error type)
result.FailureShould().NotBeNullOrWhiteSpace();
```

#### Example: Asserting on Maybe<T>

```csharp
Maybe<string> maybe = Maybe<string>.From("hello");
maybe.ValueShould().StartWith("he");
```

#### Example: Asserting on Collections

```csharp

Result<int[]> result = Result.Success(new[] { 1, 2, 3 });
result.SuccessShould().Contain(2);
result.SuccessShould().HaveCount(3).And.OnlyContain(x => x > 0);
result.SuccessShould().NotBeEmpty().And.BeEquivalentTo([1, 2, 3]);
```

#### Example: Asserting on Enums

```csharp
enum Status { Ok, Error }
Result<Status> result = Result.Success(Status.Ok);
result.SuccessShould<Status, int>().Be(Status.Ok); // The generic type parameters are a due to a limitation with generic type inference in C#.
```

#### Example: Asserting on Result<T, E> (with value or error)

```csharp
Result<int, string> result = Result.Success<int, string>(5);
result.SuccessShould().Be(5);

Result<int, string> failed = Result.Failure<int, string>("bad");
failed.FailureShould().Be("bad");
```

#### Example: Asserting on UnitResult<E>

```csharp
UnitResult<string> result = UnitResult.Failure("error");
result.FailureShould().Be("error");
```

---

You can also use the classic assertion methods if you prefer, or if you need to assert on enums with custom logic:

### 2. Classic Assertions

You can use the classic assertion methods for all result types. Note that with classic assertions, you need to use
`.Which` to access the value for further assertions:

```csharp
using FluentAssertions;
using CSharpFunctionalExtensions;

Result result = Result.Success();
result.Should().Succeed();

Result<int> resultT = Result.Success(42);
resultT.Should().SucceedWith(42);

Result<int, string> resultTE = Result.Success<int, string>(99);
resultTE.Should().SucceedWith(99);

Maybe<string> maybe = Maybe<string>.From("hello");
maybe.Should().HaveSomeValue().Which.Should().StartWith("he"); // .Which required for further assertions

Result<int[]> resultArr = Result.Success(new[] { 1, 2, 3 });
resultArr.Should().Succeed().Which.Should().Contain(2);
resultArr.Should().Succeed().Which.Should().HaveCount(3).And.OnlyContain(x => x > 0);
resultArr.Should().Succeed().Which.Should().NotBeEmpty().And.BeEquivalentTo([1, 2, 3]);
// Yeah, the generated extension methods are more concise!

Result<string> failed = Result.Failure<int>("fail!");
failed.Should().Fail().Which.Should().Be("fail!");
```

---

## Why Use the Generated Extensions?

- **Discoverability:** Just type `.` after your result/maybe and see the available assertion methods in IntelliSense.
- **Type Safety:** The extension returns the most specific assertion type (e.g., `NumericAssertions<int>`,
  `GenericCollectionAssertions<T>`, `EnumAssertions<TEnum>`, etc.).
- **Less Boilerplate:** No need to chain `.Should().Succeed().Which`—just call the generated method and assert directly.

---

## Advanced: Customizing the Generated Extensions

The library uses a source generator to create these extension methods for all supported types.
If you want to see or extend the generated code, check the `[GeneratePrimitiveExtensions]` attribute usage in the
codebase.

---

