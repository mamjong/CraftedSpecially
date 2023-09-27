// See https://aka.ms/new-console-template for more information

using CraftedSpeciallyLoadGenerator.IAndroidApi;
using Refit;

var cancellationTokenSource = new CancellationTokenSource();

Console.WriteLine("Press Ctrl+C to cancel.");

Console.CancelKeyPress += (_, args) =>
{
    args.Cancel = true;
    cancellationTokenSource.Cancel();
};

var androidApiClient = RestService.For<IAndroidApi>("http://localhost:8080");
var random = new Random();

while (!cancellationTokenSource.IsCancellationRequested)
{
    var sendFaultyRequest = random.Next(100) < 10;

    try
    {
        if (sendFaultyRequest)
        {
            Console.WriteLine("Sending faulty request...");
            await androidApiClient.CreateNewProductAsync(CreateProductForm.Faulty());
        }
        else
        {
            var sendSlowRequest = random.Next(100) < 25;

            if (sendSlowRequest)
            {
                Console.WriteLine("Sending slow request...");
                await androidApiClient.CreateNewProductAsync(CreateProductForm.Create(slow: true));
            }
            else
            {
                Console.WriteLine("Sending request...");
                await androidApiClient.CreateNewProductAsync(CreateProductForm.Create());
            }
        }
    }
    catch (HttpRequestException)
    {
        Console.WriteLine("Failed sending request.");
    }

    await Task.Delay(500);
}

Console.WriteLine("Cancellation requested, shutting down.");