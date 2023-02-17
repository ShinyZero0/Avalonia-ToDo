using System;
using actualToDo.ViewModels;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace actualToDo.Views;

public partial class NewItemView : ReactiveWindow<NewItemViewModel>
{
    public NewItemView()
    {
        InitializeComponent();
        this.WhenActivated(d => d(ViewModel.AcceptNewItemCommand.Subscribe(result => Close(result))));
    }
}
