using System.Text.Json;
using LibYear.Output.Json;
using Xunit;

namespace LibYear.Tests.Output.Json;

public class DoubleConverterTests
{
	private static TestObject DateTimeObject = new()
	{
		Test = 15
	};
	private static string ExpectedJson = @"{""Test"":15}";
	private static JsonSerializerOptions Options = new JsonSerializerOptions
	{
		Converters =
		{
			new DateTimeConverter()
		}
	};

	[Fact]
	public void ShouldSerializeProperly()
	{
		var serialized = JsonSerializer.Serialize(DateTimeObject, Options);
		Assert.Equal(ExpectedJson, serialized);
	}

	[Fact]
	public void ShouldDeserializeProperly()
	{
		var deserialized = JsonSerializer.Deserialize<TestObject>(ExpectedJson, Options);
		Assert.Equal(DateTimeObject, deserialized);
	}

	private sealed record TestObject
	{
		public double Test { get; set; }
	}
}