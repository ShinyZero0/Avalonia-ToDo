using System;
using ReactiveUI;
using System.Collections.ObjectModel;
using Avalonia.ReactiveUI;
using DynamicData;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ToDo.Models;
using System.Reactive;

namespace actualToDo.ViewModels;

public class MainWindowViewModel : ReactiveObject, IActivatableViewModel
{
    public MainWindowViewModel()
    {
        ShowDialog = new Interaction<NewItemViewModel, ToDoItem>();
        NewItemCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var adder = new NewItemViewModel();
            var result = await ShowDialog.Handle(adder);
            if (result is not null)
            {
                _sourceList.Add(result);
            }
        });
        RemoveItemCommand = ReactiveCommand.Create(() => _sourceList.RemoveAt(SelectedIndex));
        _sourceList = new SourceList<ToDoItem>();

        var item = new ToDoItem("Name", "Content");
        _sourceList.Add(item);

        _sourceList
            .Connect()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out _colle)
            .DisposeMany()
            .Subscribe();

        Activator = new ViewModelActivator();
        this.WhenActivated(
            (CompositeDisposable disposables) =>
            {
                Disposable.Create(() => { }).DisposeWith(disposables);
            }
        );
    }


    private int _selectedIndex;
    public int SelectedIndex
    {
        get => _selectedIndex;
        set => this.RaiseAndSetIfChanged(ref _selectedIndex, value);
    }

    private SourceList<ToDoItem> _sourceList;

    private readonly ReadOnlyObservableCollection<ToDoItem> _colle;
    public ReadOnlyObservableCollection<ToDoItem> Colle => _colle;

    public IReactiveCommand NewItemCommand { get; }
    public IReactiveCommand RemoveItemCommand { get; }

    public Interaction<NewItemViewModel, ToDoItem?> ShowDialog { get; set; }

    public ViewModelActivator Activator { get; }
}
