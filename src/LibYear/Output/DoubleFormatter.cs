using System.Text.Json;
using System.Text.Json.Serialization;

namespace LibYear.Output;

internal sealed class DoubleFormatter : JsonConverter<double>
{
	public override double Read(
		ref Utf8JsonReader reader,
		Type typeToConvert,
		JsonSerializerOptions options) => reader.GetDouble();

	public override void Write(
		Utf8JsonWriter writer,
		double value,
		JsonSerializerOptions options) =>
		writer.WriteNumberValue(Math.Round(value, 2));
}