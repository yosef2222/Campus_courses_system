using System.Text.Json.Serialization;

namespace Campuscourse.Models.Enums;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum StudentMarks
{
    NotDefined,
    Passed,
    Failed
}