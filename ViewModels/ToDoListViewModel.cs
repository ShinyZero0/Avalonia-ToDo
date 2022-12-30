using System.Collections.Generic;
using System.Collections.ObjectModel;
using ToDo.Models;
using ReactiveUI;
using System.Reactive;
using System.Linq;

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
        public void RemoveItem()
        {
            Items.Remove(Items.ToList().Find(x => x.Name == ItemSelected.Name));
        }   
    }
}