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
        Name = EditedItem.Name;
        AcceptEditedItemCommand = ReactiveCommand.Create(() =>
        {
            EditedItem.Name = Name;
            return EditedItem;
        });
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
