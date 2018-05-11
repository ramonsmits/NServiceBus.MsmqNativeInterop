using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        await Receiver.Start();

        do
        {
            Sender.Send();
            await Console.Out.WriteLineAsync("Press ESC to exit, any other key to send native msmq message...");
        } while (Console.ReadKey().Key!=ConsoleKey.Escape);

        await Receiver.Stop();
    }
}