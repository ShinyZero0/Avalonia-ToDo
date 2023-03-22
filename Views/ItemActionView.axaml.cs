using System;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ToDo.ViewModels;
using ReactiveUI;

namespace ToDo.Views;

public partial class ItemActionView : ReactiveWindow<IItemActionViewModel>
{
    public ItemActionView()
    {
        InitializeComponent();
        var NameBox = this.FindControl<TextBox>("NameBox");
        if (NameBox is not null)
        {
            NameBox.AttachedToVisualTree += (s, e) => NameBox.Focus();
        }

        this.WhenActivated(
            d => d(ViewModel.AcceptItemCommand.Subscribe(result => this.Close(result)))
        );
        this.WhenActivated(d => d(ViewModel.CancelCommand.Subscribe(result => Close())));
    }
}
