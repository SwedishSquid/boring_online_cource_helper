using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FillerApp
{
    public class Utils
    {
        public static JsonSerializerOptions GetDefaultJsonOptions()
        {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            return jso;
        }

        public static List<Task> ReadTasksFromFile(string filepath)
        {
            var jso = Utils.GetDefaultJsonOptions();
            Console.WriteLine(Constants.HistoryBasePath);
            using var streamReader = new StreamReader(filepath);
            return JsonSerializer.Deserialize<List<Task>>(streamReader.ReadToEnd(), jso);
        }

        public static void WriteTasksToFile(string filepath, IEnumerable<Task> tasks)
        {
            using var streamWriter = new StreamWriter(filepath);
            streamWriter.WriteLine(JsonSerializer.Serialize(tasks.ToList(), GetDefaultJsonOptions()));
        }
    }
}
