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

while (!cancellationTokenSource.IsCancellationRequested)
{
    var sendFaultyRequest = new Random().Next(100) < 10;

    if (sendFaultyRequest)
    {
        Console.WriteLine("Sending faulty request...");
        await androidApiClient.CreateNewProductAsync(CreateProductForm.Faulty());
    }
    else
    {
        Console.WriteLine("Sending request...");
        await androidApiClient.CreateNewProductAsync(CreateProductForm.Create());
    }

    await Task.Delay(500);
}

Console.WriteLine("Cancellation requested, shutting down.");