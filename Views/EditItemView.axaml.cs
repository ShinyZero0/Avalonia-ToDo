using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ToDo.ViewModels;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace ToDo.Views;

public partial class EditItemView : ReactiveWindow<EditItemViewModel>
{
    public EditItemView()
    {
        InitializeComponent();
        var InputBox = this.FindControl<TextBox>("InputBox");
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
