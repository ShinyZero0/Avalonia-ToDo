using Avalonia;
using ReactiveUI;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ToDo.Models;
using System.Reactive;

namespace actualToDo.ViewModels;

public partial class NewItemViewModel : ViewModelBase
{
    public NewItemViewModel()
    {
        AcceptNewItemCommand = ReactiveCommand.Create(() => new ToDoItem(Name, "Filler"));
    }

    private string _name;
    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }
    public ReactiveCommand<Unit, ToDoItem?> AcceptNewItemCommand { get; }
}
