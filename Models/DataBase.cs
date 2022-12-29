using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
namespace ToDo.Models
{
    public class DataBase
    {
        public IEnumerable<ToDoItem> Get()
        {
            if (File.Exists(@"data.json"))
            {
                using (var jsonSR = new StreamReader(@"data.json"))
                {
                    return  JsonConvert.DeserializeObject<List<ToDoItem>>(jsonSR.ReadToEnd());
                }
            }
            else return new List<ToDoItem>();
        }
        public void Save(IEnumerable<ToDoItem> items)
        {
            using (var jsonSW = new StreamWriter(@"data.json"))
                {
                    jsonSW.Write(JsonConvert.SerializeObject(items, Formatting.Indented));
                }
        }

    }

}
