using Avalonia;
using ReactiveUI;
using System.Reactive;
using ToDo.Models;

namespace ToDo.ViewModels
{
    class NewItemViewModel : ViewModelBase
    {
        string name;
        public string Name {
            get => name;
            set => this.RaiseAndSetIfChanged(ref name, value);
        }
        public ReactiveCommand<Unit, ToDoItem> Ok { get; }
        public ReactiveCommand<Unit, Unit> Cancel { get; }
        public NewItemViewModel()
        {
            var okEnabled = this.WhenAnyValue(
                x => x.Name,
                x => !string.IsNullOrWhiteSpace(x)
            );
            Ok = ReactiveCommand.Create(
                () => new ToDoItem(Name, "Filler"), okEnabled
            );
            Cancel = ReactiveCommand.Create(() => {}); 
        }
    }
}