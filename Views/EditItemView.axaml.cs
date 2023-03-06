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
        var InputBox = this.FindControl<TextBox>("NameBox");
        if (InputBox is not null)
        {
            InputBox.AttachedToVisualTree += (s, e) => InputBox.Focus();
        }
        this.WhenActivated(
            d => d(ViewModel.AcceptEditedItemCommand.Subscribe(result => this.Close(result)))
        );
        this.WhenActivated(d => d(ViewModel.CancelCommand.Subscribe(result => Close())));
    }
}
