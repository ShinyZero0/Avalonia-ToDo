using System;
using Avalonia.Controls;
using ToDo.ViewModels;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace ToDo.Views;

public partial class NewItemView : ReactiveWindow<NewItemViewModel>
{
    public NewItemView()
    {
        InitializeComponent();
        var NameBox = this.FindControl<TextBox>("NameBox");
        if (NameBox is not null)
        {
            NameBox.AttachedToVisualTree += (s, e) => NameBox.Focus();
        }

        this.WhenActivated(
            d => d(ViewModel.AcceptItemCommand.Subscribe(result => Close(result)))
        );
        this.WhenActivated(d => d(ViewModel.CancelCommand.Subscribe(result => Close())));
    }
}
