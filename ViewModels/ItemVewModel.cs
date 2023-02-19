using ToDo.Models;
using ReactiveUI;

namespace ToDo.ViewModels;

public class ItemViewModel : ViewModelBase
{
    public ItemViewModel(ToDoItem item)
    {
        Name = item.Name;
        Content = item.Content;
        IsDone = item.IsDone;
        Priority = item.Priority;
    }

    private string _content;
    public string Content
    {
        get => _content;
        set => this.RaiseAndSetIfChanged(ref _content, value);
    }
    private string _name;
    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }
    private bool _isDone;
    public bool IsDone
    {
        get => _isDone;
        set => this.RaiseAndSetIfChanged(ref _isDone, value);
    }
    public int Priority { get; set; }
}
