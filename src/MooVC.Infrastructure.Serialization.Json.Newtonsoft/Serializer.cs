﻿namespace MooVC.Infrastructure.Serialization.Json.Newtonsoft;

using System;
using System.IO;
using System.Text;
using global::Newtonsoft.Json;
using MooVC.Compression;
using MooVC.Serialization;
using static System.String;
using static MooVC.Infrastructure.Serialization.Json.Newtonsoft.Resources;

public sealed class Serializer
    : SynchronousSerializer
{
    public const int DefaultBufferSize = 1024;
    public const int MinimumBufferSize = 8;
    public static readonly Encoding DefaultEncoding = Encoding.UTF8;
    private readonly Lazy<JsonSerializer> json;

    public Serializer(
        int bufferSize = DefaultBufferSize,
        Encoding? encoding = default,
        ICompressor? compressor = default,
        JsonSerializerSettings? settings = default)
        : base(compressor: compressor)
    {
        BufferSize = Math.Max(bufferSize, MinimumBufferSize);
        Encoding = encoding ?? DefaultEncoding;
        json = new Lazy<JsonSerializer>(() => JsonSerializer.CreateDefault(settings));
    }

    public JsonSerializer Json => json.Value;

    public int BufferSize { get; }

    public Encoding Encoding { get; }

    protected override T PerformDeserialize<T>(Stream source)
    {
        using var reader = new StreamReader(source);
        using var text = new JsonTextReader(reader);

        T? result = Json.Deserialize<T>(text);

        if (result is null)
        {
            throw new JsonSerializationException(Format(SerializerDeserializeFailure, typeof(T).AssemblyQualifiedName));
        }

        return result;
    }

    protected override void PerformSerialize<T>(T instance, Stream target)
    {
        using var writer = new StreamWriter(target, Encoding, BufferSize, true);
        using var text = new JsonTextWriter(writer);

        Json.Serialize(text, instance);

        text.Flush();
        writer.Flush();
    }
}