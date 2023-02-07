using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
namespace ToDo.Models
{
    public class DataBase
    {
        public DataBase()
        {
            items = new List<ToDoItem>();
            inTimeCnt = 0;
        }
        public IEnumerable<ToDoItem> items { get; set; }
        public int inTimeCnt 
        {
            get
            {
                int cnt = 0;
                foreach (var item in items)
                    if(item.IsDone == true)
                    {
                        cnt++;
                    }
                return cnt;
            }
            set
            {
                
            }
        }


    }

}
