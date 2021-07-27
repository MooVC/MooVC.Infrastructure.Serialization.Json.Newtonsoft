namespace MooVC.Infrastructure.Serialization.Json.Newtonsoft.SerializerTests
{
    using System.Collections.Generic;

    internal sealed record SerializableRecord(
        IEnumerable<ulong>? Array,
        int? Integer,
        ISerializableInstance? Object,
        string? String)
        : ISerializableInstance;
}