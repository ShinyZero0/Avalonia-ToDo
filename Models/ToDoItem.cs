namespace ToDo.Models
{
    public class ToDoItem
    {
        public string Name {get; set;}
        public string Content {get; set;}
        public bool IsDone {get; set;}
        public int Priority {get; set;}
        public ToDoItem(string n, string c) 
        {
            Name = n;
            Content = c;
            IsDone = false;
            Priority = 0;
        }
    }
}