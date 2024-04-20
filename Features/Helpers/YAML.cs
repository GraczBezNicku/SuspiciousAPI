using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SuspiciousAPI.Features.Helpers;

public static class YAML
{
    private static IDeserializer _Deserializer { get; } = new DeserializerBuilder().WithNamingConvention(UnderscoredNamingConvention.Instance).Build();
    private static ISerializer _Serializer { get; } = new SerializerBuilder().WithNamingConvention(UnderscoredNamingConvention.Instance).Build();

    public static string Serialize(object value) => _Serializer.Serialize(value);
    public static object? Deserialize(string input) => _Deserializer.Deserialize(input);
    public static object? Deserialize(string input, Type type) => _Deserializer.Deserialize(input, type);
    public static object? Deserialize<T>(string input) => _Deserializer.Deserialize<T>(input);
}
