using Archive_System.Comands;
using Archive_System.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;


namespace Archive_System.ViewModel
{
    public class DocumentViewModel : BaseArchiveViewModel<Document>
    {
        ObservableCollection<Cell> emptyCells;
        public ObservableCollection<Cell> EmptyCells {
            get => emptyCells;
            set {
                if (value != emptyCells)
                {
                    emptyCells = value;
                    NotifyPropertyChanged();
                }
            }
        }
        ObservableCollection<Subscriber> subscribers;
        public ObservableCollection<Subscriber> Subscribers
        {
            get => subscribers;
            set
            {
                if (value != subscribers)
                {
                    subscribers = value;
                    NotifyPropertyChanged();
                }
            }
        }
        string newDocumentTitle;
        public string NewDocumentTitle {
            get => newDocumentTitle;
            set
            {
                if (value != newDocumentTitle)
                {
                    newDocumentTitle = value;
                    NotifyPropertyChanged();
                }
            }
        }
        string newDocumentTheme;
        public string NewDocumentTheme {
            get => newDocumentTheme;
            set
            {
                if (value != newDocumentTheme)
                {
                    newDocumentTheme = value;
                    NotifyPropertyChanged();
                }
            }
        }
        uint newDocumentInstancedCount;
        public uint NewDocumentInstancedCount {
            get => newDocumentInstancedCount;
            set
            {
                if (value != newDocumentInstancedCount)
                {
                    newDocumentInstancedCount = value;
                    NotifyPropertyChanged();
                }
            }
        }
        Cell newDocumentCell;
        public Cell NewDocumentCell {
            get => newDocumentCell;
            set
            {
                if (value != newDocumentCell)
                {
                    newDocumentCell = value;
                    NotifyPropertyChanged();
                }
            }
        }
        Subscriber selectedSubscriber;
        public Subscriber SelectedSubscriber
        {
            get => selectedSubscriber;
            set
            {
                if (value != selectedSubscriber)
                {
                    selectedSubscriber = value;
                    NotifyPropertyChanged();
                }
            }
        }
        uint issuedInstsncedCount;
        public uint IssuedInstsncedCount
        {
            get => issuedInstsncedCount;
            set
            {
                if (value != issuedInstsncedCount)
                {
                    issuedInstsncedCount = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public override ICommand RemoveItemCommand
        {
            get =>
                new RelayCommand(obj =>
                {
                    RemoveItem();
                    EmptyCells.Add(SelectedItem.Cell);
                });
        }

        public override ICommand SearchItemCommand
        {
            get =>
                new RelayCommand(obj =>
                    SearchInDataGrid(x => x.InstancedCount != 0));
        }

        public ICommand IssueDocumentComand
        {
            get =>
                new RelayCommand(obj => IssueDocument());
        }

        protected override void AddNewItem()
        {
            if(string.IsNullOrEmpty(NewDocumentTitle) || string.IsNullOrEmpty(NewDocumentTheme) || NewDocumentCell == null)
            {
                MessageBox.Show("Для добавления документа все поля должны быть заполнены!");
                return;
            }
            if(NewDocumentInstancedCount == 0)
            {
                MessageBox.Show("У документа не может быть 0 экземпляров");
                return;
            }
            if (MessageBox.Show($"Вы действительно хотите добавить документ {NewDocumentTitle}?", "Подтверждение", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                Items.Add(Document.Create(new Document()
                {
                    Title = NewDocumentTitle,
                    Theme = NewDocumentTheme,
                    InstancedCount = NewDocumentInstancedCount,
                    DateReceived = DateTime.Now,
                    Cell = NewDocumentCell
                }));
                NewDocumentTitle = null;
                NewDocumentTheme = null;
                NewDocumentInstancedCount = 0;
                EmptyCells.Remove(NewDocumentCell);
                NewDocumentCell = null;
            }
        }
        protected override void UpdateItem()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("Выберите документ, который хотите изменить.");
                return;
            }
            if (string.IsNullOrEmpty(NewDocumentTitle) && string.IsNullOrEmpty(NewDocumentTheme) && NewDocumentCell == null && NewDocumentInstancedCount == 0)
            {
                MessageBox.Show("Хотя бы одно поле должно быть заполнено для изменения.");
                return;
            }
            if (SelectedItem.Cell == null && NewDocumentCell == null)
            {
                MessageBox.Show("Для вновь появившегося документа должна быть указана ячейка.");
                return;
            }
            if (MessageBox.Show($"Вы действительно хотите изменить документ {SelectedItem}?", "Подтверждение", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                Document.Update(new Document()
                {
                    Id = SelectedItem.Id,
                    Title = NewDocumentTitle ?? SelectedItem.Title,
                    Theme = NewDocumentTheme ?? SelectedItem.Theme,
                    InstancedCount = NewDocumentInstancedCount != 0 ? NewDocumentInstancedCount : SelectedItem.InstancedCount,
                    Cell = NewDocumentCell ?? SelectedItem.Cell,
                    IssuedInstancedCount = SelectedItem.IssuedInstancedCount
                });
                Items = new ObservableCollection<Document>(Document.GetAll(x => x.InstancedCount != 0));
                EmptyCells = new ObservableCollection<Cell>(Cell.GetAll(x => !x.IsFilled));
                NewDocumentTitle = null;
                NewDocumentTheme = null;
                NewDocumentInstancedCount = 0;
                NewDocumentCell = null;
            }
        }

        private void IssueDocument()
        {
            if(SelectedItem == null)
            {
                MessageBox.Show("Выберите документ, который хотите выдать.");
                return;
            }
            if (SelectedSubscriber == null)
            {
                MessageBox.Show("Выберите абонента, которому хотите выдать документ.");
                return;
            }
            if (MessageBox.Show($"Вы действительно хотите выдать документ {SelectedItem} абоненту {SelectedSubscriber}?", "Подтверждение", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                SelectedItem.IssuedInstancedCount += IssuedInstsncedCount;
                SelectedItem.InstancedCount -= IssuedInstsncedCount;
                if (SelectedItem.InstancedCount == 0)
                    SelectedItem.Cell = null;
                IssuedDocument isDoc = IssuedDocument.Create(new IssuedDocument()
                {
                    Document = Document.Update(SelectedItem),
                    Subscriber = SelectedSubscriber,
                    IssueDate = DateTime.Now,
                    InstancedCount = IssuedInstsncedCount,
                });
                SelectedSubscriber.Documents.Add(SelectedItem);
                SelectedItem.Subscribers.Add(SelectedSubscriber);
                SelectedSubscriber.IssuedDocuments.Add(isDoc);
                SelectedItem.IssuedDocuments.Add(isDoc);
                SelectedSubscriber = null;
                Items = new ObservableCollection<Document>(Document.GetAll(x => x.InstancedCount != 0));
            }
        }

        public DocumentViewModel()
        {
            Items = new ObservableCollection<Document>(Document.GetAll(x => x.InstancedCount != 0));
            EmptyCells = new ObservableCollection<Cell>(Cell.GetAll(x => !x.IsFilled));
            Subscribers = new ObservableCollection<Subscriber>(Subscriber.GetAll(x => true));
        }
    }
}
