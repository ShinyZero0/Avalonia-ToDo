using System;
using Avalonia.Controls;
using actualToDo.ViewModels;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace actualToDo.Views;

public partial class NewItemView : ReactiveWindow<NewItemViewModel>
{
    public NewItemView()
    {
        InitializeComponent();
        var InputBox = this.FindControl<TextBox>("InputBox");
        if (InputBox is not null)
        {
            InputBox.AttachedToVisualTree += (s, e) => InputBox.Focus();
        }
        this.WhenActivated(
            d => d(ViewModel.AcceptNewItemCommand.Subscribe(result => Close(result)))
        );
        this.WhenActivated(d => d(ViewModel.CancelCommand.Subscribe(result => Close())));
    }
}
