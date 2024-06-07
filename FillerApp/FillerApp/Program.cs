using System.Runtime.InteropServices;
using System.Text.Json;

namespace FillerApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var filepath = Constants.HistoryBasePath;
            
            var taskList = new List<Task>();

            if (Path.Exists(filepath))
            {
                taskList = Utils.ReadTasksFromFile(filepath);
            }

            while (true)
            {
                var nextTask = Task.ActiveParse();
                taskList.Add(nextTask);
                Utils.WriteTasksToFile(filepath, taskList);
            }
        }

        //private static void ReadFromFile()
        //{
        //    var jso = Utils.GetDefaultJsonOptions();
        //    Console.WriteLine(Constants.HistoryBasePath);
        //    using (var streamReader = new StreamReader(Constants.HistoryBasePath))
        //    {
        //        var taskList = JsonSerializer.Deserialize<List<Task>>(streamReader.ReadToEnd(), jso);
        //        Console.WriteLine(JsonSerializer.Serialize(taskList, jso));
        //    }
        //}

        //private static void WriteToFile()
        //{
        //    var t = Task.ActiveParse();

        //    var taskList = new List<Task>() { t };

        //    JsonSerializerOptions jso = new JsonSerializerOptions();
        //    jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

            
        //    using (var streamWriter = new StreamWriter(Constants.HistoryBasePath))
        //    {
        //        streamWriter.WriteLine(JsonSerializer.Serialize(taskList, jso));
        //    }
        //}
    }
}
