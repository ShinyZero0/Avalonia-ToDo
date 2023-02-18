using Avalonia.Controls;
using ToDo.ViewModels;
using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using ReactiveUI;
using ToDo.Models;

namespace ToDo.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
        this.WhenActivated(d => d(ViewModel.ShowDialog.RegisterHandler(DoShowDialogAsync)));
    }

    private async Task DoShowDialogAsync(
        InteractionContext<NewItemViewModel, ToDoItem?> interaction
    )
    {
        var dialog = new NewItemView();
        dialog.DataContext = interaction.Input;

        var result = await dialog.ShowDialog<ToDoItem?>(this);
        interaction.SetOutput(result);
    }
}
