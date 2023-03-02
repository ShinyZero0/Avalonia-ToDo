using Avalonia;
using ReactiveUI;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ToDo.Models;
using System.Reactive;

namespace ToDo.ViewModels;

public partial class EditItemViewModel : ReactiveObject
{
    public EditItemViewModel(ItemViewModel EditedItem)
    {
        AcceptEditedItemCommand = ReactiveCommand.Create(() => new ItemViewModel(new ToDoItem(Name, "Filler", false, 0)));
        CancelCommand = ReactiveCommand.Create(() => new Unit());
    }

    private string _name;
    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }
    public ReactiveCommand<Unit, ItemViewModel> AcceptEditedItemCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelCommand { get; }
}

