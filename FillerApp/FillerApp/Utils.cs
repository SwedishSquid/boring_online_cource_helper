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
            using var streamReader = new StreamReader(filepath);
            var text = streamReader.ReadToEnd();
            if (text.Length == 0)
            {
                return new List<Task> { };
            }
            return JsonSerializer.Deserialize<List<Task>>(text, jso);
        }

        public static void WriteTasksToFile(string filepath, IEnumerable<Task> tasks)
        {
            using var streamWriter = new StreamWriter(filepath);
            streamWriter.WriteLine(JsonSerializer.Serialize(tasks.ToList(), GetDefaultJsonOptions()));
        }

        public static List<T> ReadListFromFile<T>(string filepath)
        {
            using var streamReader = new StreamReader(filepath);
            var text = streamReader.ReadToEnd();
            var jso = Utils.GetDefaultJsonOptions();
            if (text.Length == 0)
            {
                return new List<T> { };
            }
            return JsonSerializer.Deserialize<List<T>>(text, jso);
        }

        public static void WriteListToFile<T>(string filepath, List<T> data)
        {
            using var streamWriter = new StreamWriter(filepath);
            streamWriter.WriteLine(JsonSerializer.Serialize(data, GetDefaultJsonOptions()));
        }
    }
}
