namespace TimeSpaceGenerator.Core.Serializing
{
    public abstract class PacketDefinition
    {
        public string OriginalContent { get; set; }

        public string OriginalHeader { get; set; }
    }
}