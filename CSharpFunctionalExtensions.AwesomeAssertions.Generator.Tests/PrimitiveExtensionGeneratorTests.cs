using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace CSharpFunctionalExtensions.AwesomeAssertions.Generator.Tests;

public class PrimitiveExtensionGeneratorTests
{
    private const string TestClassWithAttributeText = @"
using CSharpFunctionalExtensions.AwesomeAssertions.Generator;

[GeneratePrimitiveExtensions(""TestShould"", ""Value"", ""TestNamespace.TestResult`1"")]
public static class TestExtensions;

namespace TestNamespace
{
    public class TestResult<T>
    {
        public T Value { get; }
        
        public TestResult(T value)
        {
            Value = value;
        }
    }
}";

    [Fact]
    public void Generator_ShouldProduceExtensionMethods_ForSingleGenericClass()
    {
        // Arrange
        var generator = new PrimitiveExtensionGenerator();
        var driver = CSharpGeneratorDriver.Create(generator);

        // Create a compilation with necessary references
        var compilation = CSharpCompilation.Create(
            assemblyName: "TestAssembly",
            syntaxTrees: [CSharpSyntaxTree.ParseText(TestClassWithAttributeText)],
            references:
            [
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(CompilerGeneratedAttribute).Assembly.Location)
            ],
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        // Act
        var result = driver.RunGenerators(compilation).GetRunResult();

        // Assert
        // Verify the attribute was generated
        var attributeFile = result.GeneratedTrees.FirstOrDefault(t =>
            t.FilePath.EndsWith("GeneratePrimitiveExtensionsAttribute.g.cs"));
        Assert.NotNull(attributeFile);

        // Verify the extension file was generated
        var extensionsFile = result.GeneratedTrees.FirstOrDefault(t =>
            t.FilePath.EndsWith("GeneratedExtensions.g.cs"));
        Assert.NotNull(extensionsFile);

        // Verify content contains expected methods (just checking a few examples)
        var extensionContent = extensionsFile.GetText().ToString();

        // Check for string extension
        Assert.Contains("public static StringAssertions TestShould", extensionContent);

        // Check for some generic assertions
        Assert.Contains("public static NumericAssertions<int> TestShould", extensionContent);

        // Check for some generic type with generic assertion
        Assert.Contains("public static GenericCollectionAssertions<U> TestShould<U>", extensionContent);

        // Check for enum extension
        Assert.Contains("public static EnumAssertions<TEnum> TestShould<TEnum, TIgnored>", extensionContent);

        // Verify the methods use the correct field accessor
        Assert.Contains("instance.Value", extensionContent);
    }

    [Fact]
    public void Generator_ShouldHandleGenericPosition_ForTwoGenericParameters()
    {
        // Arrange
        var testClassWithTwoGenericParams = @"
using CSharpFunctionalExtensions.AwesomeAssertions.Generator;

[GeneratePrimitiveExtensions(""LeftShould"", ""Left"", ""TestNamespace.Pairs`2"", genericPosition: 0)]
[GeneratePrimitiveExtensions(""RightShould"", ""Right"", ""TestNamespace.Pairs`2"", genericPosition: 1)]
public static class PairExtensions;

namespace TestNamespace
{
    public class Pairs<TLeft, TRight>
    {
        public TLeft Left { get; }
        public TRight Right { get; }
        
        public Pairs(TLeft left, TRight right)
        {
            Left = left;
            Right = right;
        }
    }
}";

        var generator = new PrimitiveExtensionGenerator();
        var driver = CSharpGeneratorDriver.Create(generator);

        var compilation = CSharpCompilation.Create(
            assemblyName: "TestAssembly",
            syntaxTrees: [CSharpSyntaxTree.ParseText(testClassWithTwoGenericParams)],
            references:
            [
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(CompilerGeneratedAttribute).Assembly.Location)
            ],
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        // Act
        var result = driver.RunGenerators(compilation).GetRunResult();

        // Assert
        var extensionsFile = result.GeneratedTrees.FirstOrDefault(t =>
            t.FilePath.EndsWith("GeneratedExtensions.g.cs"));
        Assert.NotNull(extensionsFile);

        var extensionContent = extensionsFile.GetText().ToString();

        // Verify left side extensions use the first generic parameter
        Assert.Contains("public static StringAssertions LeftShould<TRight>", extensionContent);
        Assert.Contains("instance.Left", extensionContent);

        // Verify right side extensions use the second generic parameter
        Assert.Contains("public static StringAssertions RightShould<TLeft>", extensionContent);
        Assert.Contains("instance.Right", extensionContent);

        // Check for some generic assertion
        Assert.Contains("public static NumericAssertions<int> LeftShould", extensionContent);
        Assert.Contains("public static NumericAssertions<int> RightShould", extensionContent);

        // Check for some generic type with generic assertion
        Assert.Contains(
            "public static GenericCollectionAssertions<U> LeftShould<TRight, U>(this TestNamespace.Pairs<U[], TRight>",
            extensionContent);
        Assert.Contains(
            "public static GenericCollectionAssertions<U> RightShould<TLeft, U>(this TestNamespace.Pairs<TLeft, U[]>",
            extensionContent);
    }
}