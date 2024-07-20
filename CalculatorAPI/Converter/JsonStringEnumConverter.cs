using Calculator.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

public class JsonStringEnumConverter : JsonConverter<ExpressionType>
{
    public override ExpressionType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException();
        }

        string enumString = reader.GetString();
        foreach (var field in typeof(ExpressionType).GetFields())
        {
            if (Attribute.GetCustomAttribute(field, typeof(DisplayAttribute)) is DisplayAttribute attribute)
            {
                if (attribute.Name == enumString)
                {
                    return (ExpressionType)field.GetValue(null);
                }
            }
        }

        throw new JsonException($"Unable to convert \"{enumString}\" to Enum \"{typeof(ExpressionType)}\"");
    }

    public override void Write(Utf8JsonWriter writer, ExpressionType value, JsonSerializerOptions options)
    {
        var enumMember = typeof(ExpressionType)
            .GetMember(value.ToString())
            .FirstOrDefault();

        var displayAttribute = enumMember?
            .GetCustomAttribute<DisplayAttribute>();

        var stringValue = displayAttribute?.Name ?? value.ToString();
        writer.WriteStringValue(stringValue);
    }
}
