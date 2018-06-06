namespace TimeSpaceGenerator.Objects
{
    public class Message
    {
        public Message(string value, short type)
        {
            Value = value;
            Type = type;
        }

        public string Value { get; set; }

        //TODO: Add enum for this
        public short Type { get; set; }
    }
}
