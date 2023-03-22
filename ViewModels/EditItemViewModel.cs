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
        Content = EditedItem.Content;
        AcceptItemCommand = ReactiveCommand.Create(() =>
        {
            EditedItem.Name = Name;
            if (string.IsNullOrWhiteSpace(Content)) Content = null;
            EditedItem.Content = Content;
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
    private string? _content;
    public string? Content
    {
        get => _content;
        set => this.RaiseAndSetIfChanged(ref _content, value);
    }
    public ReactiveCommand<Unit, ItemViewModel> AcceptItemCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelCommand { get; }
}
