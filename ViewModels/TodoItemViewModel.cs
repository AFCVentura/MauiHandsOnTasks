using HandsOnToDoList.Models; // Importa o modelo TodoItem
using System.Collections.ObjectModel; // Necessário para ObservableCollection
using System.ComponentModel; // Permite a implementação do INotifyPropertyChanged
using System.Text.Json; // Usado para serialização e desserialização JSON
using System.Windows.Input; // Permite o uso de comandos para a UI
using Microsoft.Maui.Storage; // Fornece acesso às Preferências Locais

namespace HandsOnToDoList.ViewModels
{
    public class TodoItemViewModel : INotifyPropertyChanged
    {
        // Evento para notificar a UI quando uma propriedade mudar
        public event PropertyChangedEventHandler PropertyChanged;

        // Chave usada para armazenar os dados no Preferences
        private const string PreferencesKey = "TodoItems";

        private string nameTodoItem;
        public string NameTodoItem
        {
            get => nameTodoItem;
            set
            {
                nameTodoItem = value;
                // Notifica a UI sobre a alteração do nome da tarefa
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NameTodoItem)));
            }
        }

        // Coleção observável que armazena as tarefas da lista
        public ObservableCollection<TodoItem> TodoItems { get; set; }

        // Comandos para adicionar e remover tarefas
        public ICommand AddTodoItemCommand { get; }
        public ICommand RemoveTodoItemCommand { get; }

        public TodoItemViewModel()
        {
            // Inicializa a coleção de tarefas
            TodoItems = new ObservableCollection<TodoItem>();

            // Carrega as tarefas salvas anteriormente
            LoadTodoItems();

            // Associa os métodos aos comandos que serão chamados pela UI
            AddTodoItemCommand = new Command(AddTodoItem);
            RemoveTodoItemCommand = new Command<TodoItem>(RemoveTodoItem);
        }

        // Método para adicionar uma nova tarefa
        private void AddTodoItem()
        {
            if (!string.IsNullOrWhiteSpace(NameTodoItem))
            {
                // Adiciona um novo item na coleção com o nome inserido e marcação "não concluída"
                TodoItems.Add(new TodoItem { Name = NameTodoItem, Done = false });

                // Limpa o campo de entrada após a adição
                NameTodoItem = string.Empty;

                // Salva a lista atualizada
                SaveTodoItems();
            }
        }

        // Método para remover uma tarefa específica
        private void RemoveTodoItem(TodoItem todoItem)
        {
            // Remove o item da lista
            TodoItems.Remove(todoItem);

            // Salva a lista atualizada
            SaveTodoItems();
        }

        // Método para salvar a lista de tarefas no armazenamento local
        private void SaveTodoItems()
        {
            // Converte a lista para JSON
            string json = JsonSerializer.Serialize(TodoItems);

            // Armazena a string JSON no Preferences
            Preferences.Set(PreferencesKey, json);
        }

        // Método para carregar as tarefas salvas anteriormente
        private void LoadTodoItems()
        {
            // Verifica se há dados salvos
            if (Preferences.ContainsKey(PreferencesKey))
            {
                // Recupera os dados armazenados no Preferences
                string json = Preferences.Get(PreferencesKey, string.Empty);

                // Se houver dados, desserializa para a lista de tarefas
                if (!string.IsNullOrEmpty(json))
                {
                    var items = JsonSerializer.Deserialize<ObservableCollection<TodoItem>>(json);
                    if (items != null)
                    {
                        // Adiciona os itens carregados à coleção observável
                        foreach (var item in items)
                        {
                            TodoItems.Add(item);
                        }
                    }
                }
            }
        }
    }
}
