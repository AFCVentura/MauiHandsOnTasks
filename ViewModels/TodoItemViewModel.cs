using HandsOnToDoList.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace HandsOnToDoList.ViewModels
{
    public class TodoItemViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string nameTodoItem;
        public string NameTodoItem
        {
            get => nameTodoItem;
            set
            {
                nameTodoItem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NameTodoItem)));
            }
        }

        public ObservableCollection<TodoItem> TodoItems { get; set; }

        public ICommand AddTodoItemCommand { get; }
        public ICommand RemoveTodoItemCommand { get; }

        public TodoItemViewModel()
        {
            TodoItems = new ObservableCollection<TodoItem>();

            AddTodoItemCommand = new Command(AddTodoItem);
            RemoveTodoItemCommand = new Command<TodoItem>(RemoveTodoItem);
        }

        private void AddTodoItem()
        {
            if (!string.IsNullOrWhiteSpace(NameTodoItem))
            {
                TodoItems.Add(new TodoItem { Name = NameTodoItem, Done = false });
                NameTodoItem = string.Empty;
            }
        }

        private void RemoveTodoItem(TodoItem todoItem)
        {
            TodoItems.Remove(todoItem);
        }
    }

}


