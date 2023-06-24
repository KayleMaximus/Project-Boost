// #define LOCAL_TEST

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public static class SaveService
{

    private static readonly ISaveClient Client = new CloudSaveClient();

    #region Example client usage

    private class ExampleObject
    {
        public string Some;
        public int Stuff;
    }

    public static async void SimpleExample()
    {
        // Save primitive
        await Client.Save("one", "just a string");

        // Load
        var stringData = await Client.Load<string>("one");

        // Save complex
        await Client.Save("one", new ExampleObject { Some = "Example", Stuff = 420 });

        // Load complex
        var objectData = await Client.Load<ExampleObject>("one");

        // Delete
        await Client.Delete("one");

        // Save multiple
        await Client.Save(("one", new ExampleObject { Some = "More", Stuff = 69 }),
            ("two", "string data"),
            ("three", "Another set"));

        // Load multiple. Restricted to same type
        var multipleData = await Client.Load<string>("two", "three");

        // Delete all
        await Client.DeleteAll();
    }

    #endregion
}