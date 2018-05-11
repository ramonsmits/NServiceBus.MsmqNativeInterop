using System.Threading.Tasks;
using NServiceBus;

class Receiver
{
    static IEndpointInstance instance;
    public static async Task Start()
    {
        var endpointConfiguration = new EndpointConfiguration("MsmqNative");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();

        var conventions = endpointConfiguration.Conventions();
        conventions.DefiningCommandsAs(type => type.Namespace == "V1.Messages.Commands");
        conventions.DefiningEventsAs(type => type.Namespace == "V1.Messages.Events");

        var transport = endpointConfiguration.UseTransport<MsmqTransport>();

        endpointConfiguration.EnableInstallers();

        instance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);
    }

    public static Task Stop()
    {
        return instance.Stop();
    }
}
