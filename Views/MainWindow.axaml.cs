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
        this.WhenActivated(d => d(ViewModel.ShowNewItemDialog.RegisterHandler(DoShowNewItemDialogAsync)));
    }

    private async Task DoShowNewItemDialogAsync(
        InteractionContext<NewItemViewModel, ItemViewModel?> interaction
    )
    {
        var dialog = new NewItemView();
        dialog.DataContext = interaction.Input;

        var result = await dialog.ShowDialog<ItemViewModel?>(this);
        interaction.SetOutput(result);
    }
}
