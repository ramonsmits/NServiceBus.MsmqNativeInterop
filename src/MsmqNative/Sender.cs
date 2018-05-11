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

            // Create a message queuing transaction.
            using (var transaction = new MessageQueueTransaction())
            {
                // Begin a transaction.
                transaction.Begin();

                var message = new Message();
                var json = new JsonMessageFormatter();
                json.Write(message, claim);

                // Set the message headers
                List<HeaderInfo> headers = new List<HeaderInfo>
                {
                    new HeaderInfo {Key = "NServiceBus.ContentType", Value = "application/json"},
                    new HeaderInfo {Key = "NServiceBus.EnclosedMessageTypes", Value = typeof(MyMessage).FullName},
                    new HeaderInfo {Key = "NServiceBus.MessageIntent", Value = "Send"}
                };

                message.Extension = CreateHeaders(headers);

                // Send the message
                messageQueue.Send(message, transaction);

                // Commit the transaction.
                transaction.Commit();
            }
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