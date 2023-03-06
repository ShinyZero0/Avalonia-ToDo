﻿using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using DynamicData;
using DynamicData.Aggregation;
using DynamicData.PLinq;
using DynamicData.Binding;
using ToDo.Models;
using System.Collections.Generic;
using System.Linq;

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
            // .AutoRefreshOnObservable(item => item.WhenAnyValue(i => i.IsDone, i => i.Name))
            .Filter(filter)
            // .Sort(SortExpressionComparer<ItemViewModel>.Ascending(i => Convert.ToByte(i.IsDone)))
            // .AutoRefreshOnObservable(item => item.WhenAnyValue(i => i.IsDone, i => i.Name))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out _colle)
            .DisposeMany()
            .Subscribe();

        // .AutoRefreshOnObservable(item => item.WhenAnyValue(i => i.IsDone, i => i.Name))
        // .ThenByAscending(i => i.Name)

        // СТАТИСТИКА

        // var shared = _sourceCache
        //     .Connect()
        //     .AutoRefreshOnObservable(item => item.WhenAnyValue(x => x.IsDone))
        //     .ObserveOn(RxApp.MainThreadScheduler)
        //     .Publish();
        //
        // _cleanStats = new CompositeDisposable(
        //     shared.Filter(item => item.IsDone == true).Count().Subscribe(cnt => DoneItemsCnt = cnt),
        //     shared.Count().Subscribe(cnt => ItemsCnt = cnt),
        //     shared.Connect()
        // );

        // НОВАЯ ЗАДАЧА

        ShowNewItemDialog = new Interaction<NewItemViewModel, ToDoItem?>();

        NewItemCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var adder = new NewItemViewModel();
            var result = await ShowNewItemDialog.Handle(adder);
            if (result is not null)
            {
                _sourceCache.AddOrUpdate(new ItemViewModel (result, KeyGen()));
            }
        });

        // РЕДАКТИРОВАТЬ ЗАДАЧУ

        ShowEditItemDialog = new Interaction<ItemViewModel, ItemViewModel?>();

        EditItemCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var result = await ShowEditItemDialog.Handle(SelectedItem);
            if (result is not null)
            {
                // _sourceCache.Remove(SelectedItem);
                _sourceCache.AddOrUpdate(result);
            }
        });

        // Удалить задачу
        RemoveItemCommand = ReactiveCommand.Create(() => _sourceCache.Remove(SelectedItem));

        // Активация VM
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

    // Коллекции
    private SourceCache<ItemViewModel, uint> _sourceCache;
    private readonly ReadOnlyObservableCollection<ItemViewModel> _colle;
    public ReadOnlyObservableCollection<ItemViewModel> Colle => _colle;

    // Новая задача
    public IReactiveCommand NewItemCommand { get; }
    public Interaction<NewItemViewModel, ToDoItem?> ShowNewItemDialog { get; set; }

    // Редактирование задач
    public IReactiveCommand EditItemCommand { get; }
    public Interaction<ItemViewModel, ItemViewModel?> ShowEditItemDialog { get; set; }

    // Удаление задач
    public IReactiveCommand RemoveItemCommand { get; }
    private ItemViewModel _selectedItem;
    public ItemViewModel SelectedItem
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
