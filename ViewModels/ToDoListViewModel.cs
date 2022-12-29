using System.Collections.Generic;
using System.Collections.ObjectModel;
using ToDo.Models;
using ReactiveUI;

namespace ToDo.ViewModels
{
    public class ToDoListViewModel : ViewModelBase
    {
        public ObservableCollection<ToDoItem> Items {get;}
        public ToDoListViewModel(IEnumerable<ToDoItem> items)
        {
            Items = new ObservableCollection<ToDoItem>(items);
        }
        ToDoItem itemSelected;
        public ToDoItem ItemSelected
        {
            get
            {
                return itemSelected;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref itemSelected, value);
            }       
        }   
    }
}