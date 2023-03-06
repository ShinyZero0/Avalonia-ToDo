using Avalonia.Controls;
using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using ReactiveUI;
using ToDo.ViewModels;
using ToDo.Models;

namespace ToDo.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
        this.WhenActivated(d =>
        {
            d(ViewModel.ShowEditItemDialog.RegisterHandler(DoShowEditItemDialogAsync));
        });
        this.WhenActivated(d =>
        {
            d(ViewModel.ShowNewItemDialog.RegisterHandler(DoShowNewItemDialogAsync));
        });
    }

    private async Task DoShowEditItemDialogAsync(
        InteractionContext<ItemViewModel, ItemViewModel?> interaction
    )
    {
        var dialog = new EditItemView();
        dialog.DataContext = new EditItemViewModel(interaction.Input);

        var result = await dialog.ShowDialog<ItemViewModel?>(this);
        interaction.SetOutput(result);
    }

    private async Task DoShowNewItemDialogAsync(
        InteractionContext<NewItemViewModel, ToDoItem?> interaction
    )
    {
        var dialog = new NewItemView();
        dialog.DataContext = interaction.Input;

        var result = await dialog.ShowDialog<ToDoItem?>(this);
        interaction.SetOutput(result);
    }
}
