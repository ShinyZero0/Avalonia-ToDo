using System.Collections.Generic;

namespace ToDo.Models
{
    public class DataBase
    {
        public DataBase()
        {
            Items = new List<ToDoItem>();
            inTimeCnt = 0;
        }

        public IEnumerable<ToDoItem> Items { get; set; }
        public int inTimeCnt
        {
            get
            {
                int cnt = 0;
                foreach (var item in Items)
                    if (item.IsDone == true)
                    {
                        cnt++;
                    }
                return cnt;
            }
            set { }
        }
    }
}
