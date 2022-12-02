namespace Micro.API.Networking;

internal class NetworkingOptions
{
    public const string SectionName = "Networking";
    public List<KnownNetwork> KnownNetworks { get; set; } = new();

    internal class KnownNetwork
    {
        public string Prefix { get; set; } = string.Empty;
        public int PrefixLength { get; set; }
    }
}