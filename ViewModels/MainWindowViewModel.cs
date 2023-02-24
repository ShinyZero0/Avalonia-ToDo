using System;
using ReactiveUI;
using System.Collections.ObjectModel;
using DynamicData;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ToDo.Models;
using System.Collections.Generic;

namespace ToDo.ViewModels;

public class MainWindowViewModel : ReactiveObject, IActivatableViewModel
{
    public MainWindowViewModel()
    {
        DoneItems = 0;
        DB = Saver.Get();
        _sourceList = new SourceList<ItemViewModel>();
        foreach (ToDoItem item in DB.Items)
        {
            _sourceList.Add(new ItemViewModel(item));
        }

        _sourceList
            .Connect()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out _colle)
            .DisposeMany()
            .Subscribe();

        _sourceList
            .Connect()
            .Sum(item => 1)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(result => DoneItems = result);

        ShowDialog = new Interaction<NewItemViewModel, ItemViewModel>();

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

        Activator = new ViewModelActivator();
        this.WhenActivated(
            (CompositeDisposable disposables) =>
            {
                Disposable
                    .Create(() =>
                    {
                        this.SaveData();
                    })
                    .DisposeWith(disposables);
            }
        );
    }

    private int _doneItems;
    public int DoneItems
    {
        get => _doneItems;
        set => this.RaiseAndSetIfChanged(ref _doneItems, value);
    }
    private int _selectedIndex;
    public int SelectedIndex
    {
        get => _selectedIndex;
        set => this.RaiseAndSetIfChanged(ref _selectedIndex, value);
    }

    private SourceList<ItemViewModel> _sourceList;

    private readonly ReadOnlyObservableCollection<ItemViewModel> _colle;
    public ReadOnlyObservableCollection<ItemViewModel> Colle => _colle;

    public IReactiveCommand NewItemCommand { get; }
    public IReactiveCommand RemoveItemCommand { get; }

    public void SaveData()
    {
        var list = new List<ToDoItem>();
        foreach (ItemViewModel item in this._sourceList.Items)
        {
            list.Add(item.ToToDoItem());
        }
        DB.Items = list;
        Saver.Save(DB);
    }

    private DataBase _db;
    public DataBase DB
    {
        get => _db;
        set => _db = value;
    }
    public Interaction<NewItemViewModel, ItemViewModel?> ShowDialog { get; set; }

    public ViewModelActivator Activator { get; }
}
