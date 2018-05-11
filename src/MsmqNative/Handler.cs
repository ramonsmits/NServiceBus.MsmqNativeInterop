using System;
using System.Threading.Tasks;
using NServiceBus;
using V1.Messages.Commands;

class ClaimantReminderHandler : IHandleMessages<MyMessage>
{
    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        await Console.Out.WriteLineAsync("Received claim!");
    }
}
