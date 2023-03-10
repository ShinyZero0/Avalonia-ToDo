using System;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ToDo.ViewModels;
using ReactiveUI;

namespace ToDo.Views;

public partial class EditItemView : ReactiveWindow<EditItemViewModel>
{
    public EditItemView()
    {
        InitializeComponent();
        var NameBox = this.FindControl<TextBox>("NameBox");
        if (NameBox is not null)
        {
            NameBox.AttachedToVisualTree += (s, e) => NameBox.Focus();
        }
        this.WhenActivated(
            d => d(ViewModel.AcceptEditedItemCommand.Subscribe(result => this.Close(result)))
        );
        this.WhenActivated(d => d(ViewModel.CancelCommand.Subscribe(result => Close())));
    }
}
