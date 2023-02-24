using Newtonsoft.Json;
using System.IO;

namespace ToDo.Models;

public class Saver
{
    public static void Save(DataBase dataBase)
    {
        using (var jsonSW = new StreamWriter(@"data.json"))
        {
            jsonSW.Write(JsonConvert.SerializeObject(dataBase, Formatting.Indented));
        }
    }
    public static DataBase Get()
    {
        DataBase dataBase = new DataBase();
        if (File.Exists(@"data.json"))
        {
            using (var jsonSR = new StreamReader(@"data.json"))
            {
                string jsonstr = jsonSR.ReadToEnd();
                if (!string.IsNullOrWhiteSpace(jsonstr) && jsonstr.StartsWith("{") && jsonstr.EndsWith("}"))
                { 
                    dataBase = JsonConvert.DeserializeObject<DataBase>(jsonstr);
                }
            }
        }
        return dataBase;
    }
}
