namespace V1.Messages.Commands
{
    public interface IMyMessage
    {
        string Id { get; set; }
        System.DateTime At { get; set; }
    }
}