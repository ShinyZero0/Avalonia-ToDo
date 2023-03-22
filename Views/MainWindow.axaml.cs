using Avalonia.Controls;
using System;
using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using ReactiveUI;
using ToDo.ViewModels;
using ToDo.Models;
using System.Reactive;

namespace ToDo.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
        this.WhenActivated(d =>
        {
            d(ViewModel.ShowEditItemDialog.RegisterHandler(DoShowEditItemDialogAsync));
            d(ViewModel.ShowNewItemDialog.RegisterHandler(DoShowNewItemDialogAsync));
        });
    }

    private async Task DoShowEditItemDialogAsync(
        InteractionContext<ToDoItem, ToDoItem?> interaction
    )
    {
        var dialog = new ItemActionView();
        dialog.DataContext = new EditItemViewModel(interaction.Input);

        var result = await dialog.ShowDialog<ToDoItem?>(this);
        interaction.SetOutput(result);
    }

    private async Task DoShowNewItemDialogAsync(
        InteractionContext<Unit, ToDoItem?> interaction
    )
    {
        var dialog = new ItemActionView();
        dialog.DataContext = new NewItemViewModel();

        var result = await dialog.ShowDialog<ToDoItem?>(this);
        interaction.SetOutput(result);
    }
}
