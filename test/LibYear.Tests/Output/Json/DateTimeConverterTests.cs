using LibYear.Output.Json;
using System.Text.Json;
using Xunit;

namespace LibYear.Tests.Output.Json;

public class DateTimeConverterTests
{
	private static TestObject DateTimeObject = new()
	{
		Test = new DateTime(2020, 01, 01)
	};
	private static string ExpectedJson = @"{""Test"":""2020-01-01""}";
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
		public DateTime Test { get; set; }
	}
}