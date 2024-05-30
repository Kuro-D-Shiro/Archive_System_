using Archive_System.Comands;
using Archive_System.Model;
using Archive_System.Model.Attributes;
using Archive_System.Model.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;


namespace Archive_System.ViewModel
{
    public abstract class BaseArchiveViewModel<T> : INotifyPropertyChanged
        where T : BaseModel<T>, IDbModel<T>
    {
        string searchTextBoxContent;
        public string SearchTextBoxContent
        {
            get => searchTextBoxContent;
            set {
                if (value != searchTextBoxContent) {
                    searchTextBoxContent = value;
                    NotifyPropertyChanged();
                }
            }
        }
        T selectedItem;
        public T SelectedItem {
            get => selectedItem;
            set {
                if (value != null && !value.Equals(selectedItem))
                {
                    selectedItem = value;
                    NotifyPropertyChanged();
                }
            }
        }
        ObservableCollection<T> items;
        public ObservableCollection<T> Items {
            get => items;
            set {
                if (value != items)
                {
                    items = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public virtual ICommand RemoveItemCommand
        {
            get =>
                new RelayCommand(obj => RemoveItem());
        }

        public virtual ICommand SearchItemCommand
        {
            get =>
                new RelayCommand(obj => SearchInDataGrid(x => true));
        }

        public virtual ICommand AddNewItemCommand
        {
            get =>
                new RelayCommand(obj => AddNewItem());
        }
        public virtual ICommand EditItemCommand
        {
            get =>
                new RelayCommand(obj => UpdateItem());
        }

        protected void RemoveItem()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show($"Выберите объект \"{(ClassNameInRuAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(ClassNameInRuAttribute))}\", который хотите удалить.");
                return;
            }
            if (MessageBox.Show($"Вы действительно хотите удалить объект \"{(ClassNameInRuAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(ClassNameInRuAttribute))}\"?", "Подтверждение", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                if (T.Remove(SelectedItem))
                    Items.Remove(SelectedItem);
        }
        protected void SearchInDataGrid(Func<T, bool> selector)
        {
            if (string.IsNullOrEmpty(SearchTextBoxContent))
            {
                Items = new ObservableCollection<T>(T.GetAll(x => true).Where(x => selector(x)));
                return;
            }
            Items = new ObservableCollection<T>(T.GetAll(x => {
                var properties = x.GetType().GetProperties().Where(p => p.IsDefined(typeof(SearchableAttribute), false));
                return properties.Any(y => y.GetValue(x) != null &&
                    y.GetValue(x).ToString().Contains(SearchTextBoxContent, StringComparison.CurrentCultureIgnoreCase));
            }).Where(x => selector(x)));
        }
        protected abstract void AddNewItem();
        protected abstract void UpdateItem();

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
