using System;
using ToDo.Models;
using System.Reactive;
using ReactiveUI;

namespace ToDo.ViewModels;
public interface IItemActionViewModel 
{
    public ReactiveCommand<Unit, ToDoItem> AcceptItemCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelCommand { get; }
}
