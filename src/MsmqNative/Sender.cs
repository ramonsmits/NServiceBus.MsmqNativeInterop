using System;
using System.Collections.Generic;
using System.IO;
using System.Messaging;
using V1.Messages.Commands;

class Sender
{
    public static void Send()
    {
        string path = @".\Private$\MsmqNative";

        if (!MessageQueue.Exists(path))
        {
            MessageQueue.Create(path, true); // Important to create it as an Transactional queue!
        }

        var messageQueue = new MessageQueue(path, QueueAccessMode.Send);

        using (messageQueue)
        {
            var claim = new MyMessage
            {
                Id = Guid.NewGuid().ToString("N"),
                At = DateTime.UtcNow
            };

            var message = new Message();
            var json = new JsonMessageFormatter();
            json.Write(message, claim);

            // Set the message headers
            var headers = new List<HeaderInfo>
                {
                    new HeaderInfo {Key = "NServiceBus.EnclosedMessageTypes", Value = typeof(MyMessage).FullName},
                };

            message.Extension = CreateHeaders(headers);

            messageQueue.Send(message, MessageQueueTransactionType.Single);
        }
    }

    static byte[] CreateHeaders(List<HeaderInfo> headerInfos)
    {
        var serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<HeaderInfo>));
        using (var stream = new MemoryStream())
        {
            serializer.Serialize(stream, headerInfos);
            return stream.ToArray();
        }
    }

}