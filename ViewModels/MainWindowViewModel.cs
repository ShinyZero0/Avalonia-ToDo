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
        /// Добавить элементы в список из JSON
        DB = Saver.Get();
        _sourceList = new SourceList<ItemViewModel>();
        foreach (ToDoItem item in DB.Items)
        {
            _sourceList.Add(new ItemViewModel(item));
        }

        /// Забиндить список к ReadOnlyObservableCollection
        _sourceList
            .Connect()
            .StartWithEmpty()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out _colle)
            .DisposeMany()
            .Subscribe();

        /// СТАТИСТИКА

        var shared = _sourceList
            .Connect()
            .AutoRefreshOnObservable(item => item.WhenAnyValue(x => x.IsDone))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Publish();

        _cleanStats = new CompositeDisposable(
            shared.Filter(item => item.IsDone == true).Count().Subscribe(cnt => DoneItemsCnt = cnt),
            shared.Count().Subscribe(cnt => ItemsCnt = cnt),
            shared.Connect()
        );

        /// НОВАЯ ЗАДАЧА

        ShowNewItemDialog = new Interaction<NewItemViewModel, ItemViewModel>();

        NewItemCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var adder = new NewItemViewModel();
            var result = await ShowNewItemDialog.Handle(adder);
            if (result is not null)
            {
                _sourceList.Add(result);
            }
        });

        /// Удалить задачу
        RemoveItemCommand = ReactiveCommand.Create(() => _sourceList.RemoveAt(SelectedIndex));

        /// Активация VM
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

    /// Коллекции
    private SourceList<ItemViewModel> _sourceList;
    private readonly ReadOnlyObservableCollection<ItemViewModel> _colle;
    public ReadOnlyObservableCollection<ItemViewModel> Colle => _colle;

    /// Новая задача
    public IReactiveCommand NewItemCommand { get; }
    public Interaction<NewItemViewModel, ItemViewModel?> ShowNewItemDialog { get; set; }

    /// Редактирования задач
    public IReactiveCommand EditItemCommand { get; }
    public Interaction<ItemViewModel, ItemViewModel?> ShowEditItemDIalog { get; set; }

    /// Удаление задач
    public IReactiveCommand RemoveItemCommand { get; }
    private int _selectedIndex;
    public int SelectedIndex
    {
        get => _selectedIndex;
        set => this.RaiseAndSetIfChanged(ref _selectedIndex, value);
    }

    /// Статистика
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

    /// Сериализация
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

    /// Модель
    private DataBase _db;
    public DataBase DB
    {
        get => _db;
        set => _db = value;
    }

    public ViewModelActivator Activator { get; }
    private readonly IDisposable _cleanStats;
}
