﻿using ReactiveUI;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Linq;
using ToDo.Models;
using System;

namespace ToDo.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        ViewModelBase content;
        public ViewModelBase Content
        {
            get => content;
            private set => this.RaiseAndSetIfChanged(ref content, value);
        }
        public ToDoListViewModel Items { get; }
        public MainWindowViewModel(DataBase db)
        {
            Content = Items = new ToDoListViewModel(db.Get());
        }
        public void NewItem()
        {
            var vm = new NewItemViewModel();
            Observable.Merge(
                vm.Ok,
                vm.Cancel.Select(_ => (ToDoItem)null)
            ).Take(1).Subscribe(model =>
            {
                if (model != null)
                {
                    Items.Items.Add(model);
                }
                Content = Items;
            }
            );
            Content = vm;

        }
        public void RemoveItem()
        {
            Items.Items.Remove(Items.Items.ToList().Find(x => x.Name == Items.ItemSelected.Name));
        }
    }
}
