using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using DynamicData;
using DynamicData.Aggregation;
using ToDo.Models;
using System.Collections.Generic;

namespace ToDo.ViewModels;

public class MainWindowViewModel : ReactiveObject, IActivatableViewModel
{
    public MainWindowViewModel()
    {
        DB = Saver.Get();
        _sourceList = new SourceList<ItemViewModel>();
        foreach (ToDoItem item in DB.Items)
        {
            _sourceList.Add(new ItemViewModel(item));
        }

        _sourceList
            .Connect()
            .StartWithEmpty()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out _colle)
            .DisposeMany()
            .Subscribe();

        var shared = _sourceList
            .Connect()
            .AutoRefreshOnObservable(item => item.WhenAnyValue(x => x.IsDone))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Publish();

        _cleanUp = new CompositeDisposable(
            shared.Filter(item => item.IsDone == true).Count().Subscribe(cnt => DoneItemsCnt = cnt),
            shared.Count().Subscribe(cnt => ItemsCnt = cnt),
            shared.Connect()
        );

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

    public string ItemsCntString {get; set;}
    private int _doneItemsCnt;
    public int DoneItemsCnt
    {
        get => _doneItemsCnt;
        set => this.RaiseAndSetIfChanged(ref _doneItemsCnt, value);
    }
    private int _itemsCnt;
    public int ItemsCnt
    {
        get => _itemsCnt;
        set => this.RaiseAndSetIfChanged(ref _itemsCnt, value);
    }

    private int _selectedIndex;
    public int SelectedIndex
    {
        get => _selectedIndex;
        set => this.RaiseAndSetIfChanged(ref _selectedIndex, value);
    }

    private SourceList<ItemViewModel> _sourceList;

    private readonly IDisposable _cleanUp;

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
