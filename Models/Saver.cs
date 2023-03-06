using Newtonsoft.Json;
using System;
using System.IO;

namespace ToDo.Models;

public class Saver
{
    private static string _data =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "AvaloniaToDo.json"
        );

    public static void Save(DataBase dataBase)
    {
        using (var jsonSW = new StreamWriter(_data))
        {
            jsonSW.Write(JsonConvert.SerializeObject(dataBase, Formatting.Indented));
        }
    }

    public static DataBase Get()
    {
        DataBase dataBase = new DataBase();
        if (File.Exists(_data))
        {
            using (var jsonSR = new StreamReader(_data))
            {
                string jsonstr = jsonSR.ReadToEnd();
                if (!string.IsNullOrWhiteSpace(jsonstr))
                // && jsonstr.StartsWith("{")
                // && jsonstr.EndsWith("}")
                {
                    dataBase = JsonConvert.DeserializeObject<DataBase>(jsonstr);
                }
            }
        }
        return dataBase;
    }
}
