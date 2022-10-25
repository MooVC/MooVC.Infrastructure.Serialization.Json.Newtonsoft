namespace MooVC.Infrastructure.Serialization.Json.Newtonsoft.SerializerTests;

using System.Text;
using global::Newtonsoft.Json;
using Xunit;

public sealed class WhenSerializerIsConstructed
{
    [Fact]
    public void GivenNoSettingsThenADefaultSerializerIsCreated()
    {
        var serializer = new Serializer();
        var settings = new JsonSerializerSettings();

        AssertEqual(Serializer.DefaultBufferSize, Serializer.DefaultEncoding, serializer, settings);
    }

    [Fact]
    public void GivenABufferSizeThenASerializerIsCreatedWithTheBufferSizeApplied()
    {
        const int BufferSize = 32;
        var serializer = new Serializer(bufferSize: 32);
        var settings = new JsonSerializerSettings();

        AssertEqual(BufferSize, Serializer.DefaultEncoding, serializer, settings);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(1)]
    public void GivenABelowMinimumBufferSizeThenASerializerIsCreatedWithTheMinimumBufferSizeApplied(int bufferSize)
    {
        var serializer = new Serializer(bufferSize: bufferSize);
        var settings = new JsonSerializerSettings();

        AssertEqual(Serializer.MinimumBufferSize, Serializer.DefaultEncoding, serializer, settings);
    }

    [Fact]
    public void GivenAEncodingThenASerializerIsCreatedWithTheEncodingApplied()
    {
        Encoding encoding = Encoding.ASCII;
        var serializer = new Serializer(encoding: encoding);
        var settings = new JsonSerializerSettings();

        AssertEqual(Serializer.DefaultBufferSize, encoding, serializer, settings);
    }

    [Fact]
    public void GivenSettingsThenASerializerIsCreatedWithTheSettingsApplied()
    {
        var settings = new JsonSerializerSettings
        {
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            TypeNameHandling = TypeNameHandling.All,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
        };

        var serializer = new Serializer(settings: settings);

        AssertEqual(Serializer.DefaultBufferSize, Serializer.DefaultEncoding, serializer, settings);
    }

    private static void AssertEqual(int bufferSize, Encoding encoding, Serializer serializer, JsonSerializerSettings settings)
    {
        Assert.Equal(bufferSize, serializer.BufferSize);
        Assert.Equal(encoding, serializer.Encoding);
        Assert.Equal(settings.DateTimeZoneHandling, serializer.Json.DateTimeZoneHandling);

        AssertEqual(settings, serializer);
    }

    private static void AssertEqual(JsonSerializerSettings expected, Serializer serializer)
    {
        Assert.Equal(expected.DefaultValueHandling, serializer.Json.DefaultValueHandling);
        Assert.Equal(expected.NullValueHandling, serializer.Json.NullValueHandling);
        Assert.Equal(expected.ReferenceLoopHandling, serializer.Json.ReferenceLoopHandling);
        Assert.Equal(expected.TypeNameHandling, serializer.Json.TypeNameHandling);
        Assert.Equal(expected.TypeNameAssemblyFormatHandling, serializer.Json.TypeNameAssemblyFormatHandling);
    }
}