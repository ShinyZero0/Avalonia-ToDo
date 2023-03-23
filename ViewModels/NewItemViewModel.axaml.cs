using Avalonia;
using ReactiveUI;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ToDo.Models;
using System.Reactive;

namespace ToDo.ViewModels;

public partial class NewItemViewModel : ReactiveObject, IItemActionViewModel
{
    public NewItemViewModel()
    {
        AcceptItemCommand = ReactiveCommand.Create(() =>
        {
            if (string.IsNullOrWhiteSpace(Content))
                Content = null;
            return new ToDoItem(Name, Content, false, 0);
        });
        CancelCommand = ReactiveCommand.Create(() => new Unit());
    }

    private string _name;
    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }
    private string? _content;
    public string? Content
    {
        get => _content;
        set => this.RaiseAndSetIfChanged(ref _content, value);
    }
    public ReactiveCommand<Unit, ToDoItem> AcceptItemCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelCommand { get; }
    public const string WindowTitle = "Создание задачи";
}
