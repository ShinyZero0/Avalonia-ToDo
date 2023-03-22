using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using DynamicData;
using DynamicData.PLinq;
using ToDo.Models;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;

// using Lucene.Net;

namespace ToDo.ViewModels;

public class MainWindowViewModel : ReactiveObject, IActivatableViewModel
{
    public MainWindowViewModel()
    {
        // Добавить элементы в список из JSON
        DB = Saver.Get();
        _sourceCache = new SourceCache<ItemViewModel, uint>(i => i.Key);
        foreach (ToDoItem item in DB.Items)
        {
            _sourceCache.AddOrUpdate(new ItemViewModel(item, KeyGen()));
        }

        // Поиск
        var filter = this.WhenAnyValue(vm => vm.SearchText)
            .Throttle(TimeSpan.FromMilliseconds(50))
            .Select(MakeFilter);

        // Забиндить список к Colle
        _sourceCache
            .Connect()
            .StartWithEmpty()
            .AutoRefreshOnObservable(item => item.WhenAnyValue(i => i.IsDone, i => i.Name))
            .Filter(filter)
            // .Sort(SortExpressionComparer<ItemViewModel>.Ascending(i => Convert.ToByte(i.IsDone)))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out _colle)
            .DisposeMany()
            .Subscribe();

        // .AutoRefreshOnObservable(item => item.WhenAnyValue(i => i.IsDone, i => i.Name))
        // .ThenByAscending(i => i.Name)

        // NEW ITEM

        ShowNewItemDialog = new Interaction<Unit, ToDoItem?>();

        NewItemCommand = ReactiveCommand.Create(() =>
        {
            ToDoItem result = null;
            ShowNewItemDialog.Handle(new Unit()).Subscribe(x => result = x);
            if (result is not null)
            {
                _sourceCache.AddOrUpdate(new ItemViewModel(result, KeyGen()));
            }
        });

        // EDIT ITEM

        ShowEditItemDialog = new Interaction<ItemViewModel, ItemViewModel?>();

        EditItemCommand = ReactiveCommand.Create(() =>
        {
            ItemViewModel result = null;
            ShowEditItemDialog.Handle(SelectedItem).Subscribe(x => result = x);
            if (result is not null)
            {
                _sourceCache.AddOrUpdate(result);
            }
        });

        // Удалить задачу
        RemoveItemCommand = ReactiveCommand.Create(() => _sourceCache.Remove(SelectedItem));

        // VM ACTIVATION
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
        // STATISTICS
        // var shared = _sourceCache
        //     .Connect()
        //     .AutoRefreshOnObservable(item => item.WhenAnyValue(i => i.IsDone))
        //     .ObserveOn(RxApp.MainThreadScheduler)
        //     .Publish();
        //
        // _cleanStats = new CompositeDisposable(
        //     shared.Filter(item => item.IsDone == true).Count().Subscribe(cnt => DoneItemsCnt =
        //     cnt), shared.Count().Subscribe(cnt => ItemsCnt = cnt), shared.Connect()
        // );
        // this.WhenAnyValue(vm => vm.DoneItemsCnt, vm => vm.ItemsCnt)
        //     .Subscribe(cnt => StatsString = $"Выполнено задач: {cnt.Item1.ToString()} из
        //     {cnt.Item2.ToString()}");
        );
    }

    // Коллекции
    private SourceCache<ItemViewModel, uint> _sourceCache;
    private readonly ReadOnlyObservableCollection<ItemViewModel> _colle;
    public ReadOnlyObservableCollection<ItemViewModel> Colle => _colle;

    // Новая задача
    public IReactiveCommand NewItemCommand { get; }
    public Interaction<Unit, ToDoItem?> ShowNewItemDialog { get; set; }

    // Редактирование задач
    public IReactiveCommand EditItemCommand { get; }
    public Interaction<ItemViewModel, ItemViewModel?> ShowEditItemDialog { get; set; }

    // Удаление задач
    public IReactiveCommand RemoveItemCommand { get; }
    private ItemViewModel? _selectedItem;
    public ItemViewModel? SelectedItem
    {
        get => _selectedItem;
        set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
    }

    private string _searchText;
    public string SearchText
    {
        get => _searchText;
        set => this.RaiseAndSetIfChanged(ref _searchText, value);
    }

    private static Func<ItemViewModel, bool> MakeFilter(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
            return i => true;
        return i => i.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase);
    }

    // Статистика
    public string ItemsCntString { get; set; }
    private string _statsString;
    public string StatsString
    {
        get => _statsString;
        set => this.RaiseAndSetIfChanged(ref _statsString, value);
    }
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

    // Сериализация
    public void SaveData()
    {
        var list = new List<ToDoItem>();
        foreach (ItemViewModel item in this._sourceCache.Items)
        {
            list.Add(item.ToToDoItem());
        }
        DB.Items = list;
        Saver.Save(DB);
    }

    // Модель
    private DataBase _db;
    public DataBase DB
    {
        get => _db;
        set => _db = value;
    }

    private uint KeyGen()
    {
        uint i = 0;
        while (i < uint.MaxValue && this._sourceCache.Keys.Contains(i))
        {
            i++;
        }
        return i;
    }

    public ViewModelActivator Activator { get; }

    private readonly IDisposable _cleanStats;
}

// vim:fdm=indent:fdl=1:fcl=all
