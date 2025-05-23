// ReSharper disable SuggestVarOrType_Elsewhere

using NATS.Client.JetStream;
using NATS.Client.ObjectStore;
using NATS.Client.ObjectStore.Models;

#pragma warning disable SA1123
#pragma warning disable SA1124
#pragma warning disable SA1509
#pragma warning disable SA1515

namespace NATS.Net.DocsExamples.ObjectStore;

public class IntroPage
{
    public async Task Run()
    {
        Console.WriteLine("____________________________________________________________");
        Console.WriteLine("NATS.Net.DocsExamples.ObjectStore.IntroPage");

        #region obj
        await using NatsClient nc = new NatsClient();
        INatsObjContext obj = nc.CreateObjectStoreContext();
        #endregion

        try
        {
            await obj.DeleteObjectStore("test-bucket", CancellationToken.None);
            await Task.Delay(1000);
        }
        catch (NatsJSApiException)
        {
        }

        #region store
        INatsObjStore store = await obj.CreateObjectStoreAsync("test-bucket");
        #endregion

        await File.WriteAllTextAsync("data.bin", "tests");
        try
        {
            #region putget
            await store.PutAsync("my/random/data.bin", File.OpenRead("data.bin"));

            await store.GetAsync("my/random/data.bin", File.OpenWrite("data_copy.bin"));
            #endregion

            #region info
            ObjectMetadata metadata = await store.GetInfoAsync("my/random/data.bin");

            Console.WriteLine("Metadata:");
            Console.WriteLine($"  Bucket: {metadata.Bucket}");
            Console.WriteLine($"  Name: {metadata.Name}");
            Console.WriteLine($"  Size: {metadata.Size}");
            Console.WriteLine($"  Time: {metadata.MTime}");
            Console.WriteLine($"  Chunks: {metadata.Chunks}");
            #endregion

            #region delete
            await store.DeleteAsync("my/random/data.bin");
            #endregion
        }
        finally
        {
            File.Delete("data.bin");
            File.Delete("data_copy.bin");
        }
    }
}
