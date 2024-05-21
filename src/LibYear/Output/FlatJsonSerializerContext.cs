using System.Text.Json.Serialization;

namespace LibYear.Output;

[JsonSerializable(typeof(ResultOutput))]
[JsonSourceGenerationOptions(WriteIndented = false)]
internal partial class FlatJsonSerializerContext: JsonSerializerContext
{
}