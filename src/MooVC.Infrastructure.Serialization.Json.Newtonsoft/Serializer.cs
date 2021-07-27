namespace MooVC.Infrastructure.Serialization.Json.Newtonsoft
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using global::Newtonsoft.Json;
    using MooVC.Serialization;
    using static System.String;
    using static MooVC.Infrastructure.Serialization.Json.Newtonsoft.Resources;

    public sealed class Serializer
        : SynchronousSerializer
    {
        public const int DefaultBufferSize = 1024;
        public const int MinimumBufferSize = 8;
        public static readonly Encoding DefaultEncoding = new UTF8Encoding(false, true);
        private readonly Lazy<JsonSerializer> json;

        public Serializer(
            int bufferSize = DefaultBufferSize,
            Encoding? encoding = default,
            JsonSerializerSettings? settings = default)
        {
            BufferSize = Math.Max(bufferSize, MinimumBufferSize);
            Encoding = encoding ?? DefaultEncoding;
            json = new Lazy<JsonSerializer>(() => JsonSerializer.CreateDefault(settings));
        }

        public JsonSerializer Json => json.Value;

        public int BufferSize { get; }

        public Encoding Encoding { get; }

        protected override T PerformDeserialize<T>(IEnumerable<byte> data)
        {
            using var stream = new MemoryStream(data.ToArray());

            return PerformDeserialize<T>(stream);
        }

        protected override T PerformDeserialize<T>(Stream source)
        {
            using var reader = new StreamReader(source);
            using var text = new JsonTextReader(reader);

            T? result = Json.Deserialize<T>(text);

            if (result is null)
            {
                throw new JsonSerializationException(Format(
                    SerializerDeserializeFailure,
                    typeof(T).AssemblyQualifiedName));
            }

            return result;
        }

        protected override IEnumerable<byte> PerformSerialize<T>(T instance)
        {
            using var stream = new MemoryStream();

            PerformSerialize(instance, stream);

            return stream.ToArray();
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
}