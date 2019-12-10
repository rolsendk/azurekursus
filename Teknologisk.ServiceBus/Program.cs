using System;
using System.Text;
using Microsoft.Azure.ServiceBus;
using System.Threading.Tasks;

namespace Teknologisk.ServiceBus
{
    class Program
    {
        static string ServiceBusConnectionString => "Endpoint=sb://roldtibus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=UHo4we+kVZaKh57M+HtUjV5OJfgg3qVPFX0XhZX+ec4=";

        static async Task Main(string[] args)
        {
            // Current message count in queue can be viewed in Azure Portal => Service Bus => virtualmachines queue => Overview
            await SendMessagesAsync(100);

            // Beware! You have to create a subscription for the 'dog' topic, or else messages will not be saved.
            // The current message count is displayed under the topic subscription.
            await SendTopicsAsync(100, "dogs");

            Console.ReadKey();

        }

        static async Task SendMessagesAsync(int numberOfMessages)
        {
            const string queueName = "virtualmachines";
            IQueueClient queueClient = new QueueClient(ServiceBusConnectionString, queueName);

            for (var i = 0; i < numberOfMessages; i++)
            {
                string messageBody = $"Message {i}";
                var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                Console.WriteLine($"Sending message: {messageBody}");

                await queueClient.SendAsync(message);
            }

            await queueClient.CloseAsync();
        }

        static async Task SendTopicsAsync(int numberOfTopics, string topic)
        {
            ITopicClient topicClient = new TopicClient(ServiceBusConnectionString, topic);

            for (var i = 0; i < numberOfTopics; i++)
            {
                string messageBody = $"Topic {i}";
                var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                Console.WriteLine($"Sending message to topic '{topic}': {messageBody}");

                await topicClient.SendAsync(message);
            }

            await topicClient.CloseAsync();
        }
    }
}
