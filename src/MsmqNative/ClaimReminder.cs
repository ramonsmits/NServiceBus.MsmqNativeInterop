namespace V1.Messages.Commands
{
    class MyMessage : IMyMessage
    {
        public string Id { get; set; }
        public System.DateTime At { get; set; }
    }
}