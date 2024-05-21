using System.Text.Json.Serialization;

namespace LibYear.Output;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(ResultOutput))]
internal partial class IndentedJsonSerializerContext: JsonSerializerContext
{
}