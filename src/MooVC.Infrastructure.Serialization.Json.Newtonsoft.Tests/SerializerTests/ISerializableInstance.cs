namespace MooVC.Infrastructure.Serialization.Json.Newtonsoft.SerializerTests
{
    using System.Collections.Generic;

    internal interface ISerializableInstance
    {
        IEnumerable<ulong>? Array { get; }

        int? Integer { get; }

        ISerializableInstance? Object { get; }

        string? String { get; }
    }
}