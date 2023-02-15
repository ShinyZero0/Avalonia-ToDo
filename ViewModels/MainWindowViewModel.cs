using ReactiveUI;
using System.Collections.ObjectModel;
using Avalonia.ReactiveUI;
using DynamicData;
using System.Reactive.Disposables;
using System.Reactive.Linq;

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
        _sourceList.Connect()
                   .ObserveOnDispatcher()
                   .Bind(out _list)
                   .DisposeMany()
                   .Subscribe();

        _sourceList.Add(_greeting);
        Greeting = "ну привет";
    }
    private string _greeting;
    public string Greeting 
    { 
        get => _greeting;
        set { this.RaiseAndSetIfChanged(ref _greeting, value); }
    }
    public ViewModelActivator Activator { get; }
    private SourceList<string> _sourceList;
    public ReadOnlyObservableCollection<string> List => _list;
    private readonly ReadOnlyObservableCollection<string> _list;
}
