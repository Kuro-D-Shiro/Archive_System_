using Archive_System.Model;
using System.Collections.ObjectModel;
using System.Windows;

namespace Archive_System.ViewModel
{
    public class CellViewModel : BaseArchiveViewModel<Cell>
    {
        uint newRackNumber;
        public uint NewRackNumber
        {
            get => newRackNumber;
            set
            {
                if (value != newRackNumber)
                {
                    newRackNumber = value;
                    NotifyPropertyChanged();
                }
            }
        }
        uint newShalfNumber;
        public uint NewShalfNumber
        {
            get => newShalfNumber;
            set
            {
                if (value != newShalfNumber)
                {
                    newShalfNumber = value;
                    NotifyPropertyChanged();
                }
            }
        }
        protected override void AddNewItem()
        {
            if (NewRackNumber == 0 || NewShalfNumber == 0)
            {
                MessageBox.Show("Для добавления ячейки все поля должны быть заполнены!");
                return;
            }
            if (MessageBox.Show($"Вы действительно хотите добавить ячейку на {NewShalfNumber} полку {NewRackNumber} стеллажа?", "Подтверждение", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                Items.Add(Cell.Create(new Cell()
                {
                    RackNumber = NewRackNumber,
                    ShalfNumber = NewShalfNumber
                }));
                NewRackNumber = 0;
                NewShalfNumber = 0;
            }
        }

        protected override void UpdateItem()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("Выберите ячейку, которую хотите изменить.");
                return;
            }
            if (NewRackNumber == 0 && NewShalfNumber == 0)
            {
                MessageBox.Show("Хотя бы одно поле должно быть заполнено для изменения.");
                return;
            }
            if (MessageBox.Show($"Вы действительно хотите изменить ячейку {SelectedItem}?", "Подтверждение", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                Cell.Update(new Cell()
                {
                    Id = SelectedItem.Id,
                    RackNumber = NewRackNumber != 0 ? NewRackNumber : SelectedItem.RackNumber,
                    ShalfNumber = NewShalfNumber != 0 ? NewShalfNumber : SelectedItem.ShalfNumber,
                    Document = SelectedItem.Document
                });
                Items = new ObservableCollection<Cell>(Cell.GetAll(x => true));
                NewRackNumber = 0;
                NewShalfNumber = 0;
            }
        }

        public CellViewModel() 
        {
            Items = new ObservableCollection<Cell>(Cell.GetAll(x => true));
        }
    }
}
