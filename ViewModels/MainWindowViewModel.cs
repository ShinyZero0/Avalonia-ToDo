using System;
using ReactiveUI;
using System.Collections.ObjectModel;
using Avalonia.ReactiveUI;
using DynamicData;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ToDo.Models;

namespace actualToDo.ViewModels;

public class MainWindowViewModel : ReactiveObject, IActivatableViewModel
{
    public MainWindowViewModel()
    {
        Activator = new ViewModelActivator();
        this.WhenActivated((CompositeDisposable disposables) =>
        {
            Disposable.Create(() => { })
                      .DisposeWith(disposables);
        });

        var item = new ToDoItem("Name", "Content");
        _sourceList = new SourceList<ToDoItem>();

        _sourceList.Add(item);

        _sourceList.Connect()
                   .ObserveOn(RxApp.MainThreadScheduler)
                   .Bind(out _list)
                   .DisposeMany()
                   .Subscribe();

        _sourceList.Add(item);


    }
    private string _greeting;
    public string Greeting 
    { 
        get => _greeting;
        set { this.RaiseAndSetIfChanged(ref _greeting, value); }
    }
    public ViewModelActivator Activator { get; }
    private SourceList<ToDoItem> _sourceList;
    public ReadOnlyObservableCollection<ToDoItem> List => _list;
    private readonly ReadOnlyObservableCollection<ToDoItem> _list;
}
