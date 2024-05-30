using Archive_System.Model;
using System.Collections.ObjectModel;
using System.Windows;


namespace Archive_System.ViewModel
{
    public class SubscriberViewModel : BaseArchiveViewModel<Subscriber>
    {

        string newSubscriberName;
        public string NewSubscriberName {
            get => newSubscriberName;
            set {
                if (value != newSubscriberName)
                {
                    newSubscriberName = value;
                    NotifyPropertyChanged();
                }
            }
        }
        string newSubscriberSurname;
        public string NewSubscriberSurname {
            get => newSubscriberSurname;
            set {
                if (value != newSubscriberSurname)
                {
                    newSubscriberSurname = value;
                    NotifyPropertyChanged();
                }
            }
        }
        string newSubscriberPatronimic;
        public string NewSubscriberPatronimic {
            get => newSubscriberPatronimic;
            set {
                if (value != newSubscriberPatronimic)
                {
                    newSubscriberPatronimic = value;
                    NotifyPropertyChanged();
                }
            }
        }
        ulong newSubscriberPhoneNumber;
        public ulong NewSubscriberPhoneNumber {
            get => newSubscriberPhoneNumber;
            set {
                if (value != newSubscriberPhoneNumber)
                {
                    newSubscriberPhoneNumber = value;
                    NotifyPropertyChanged();
                }
            }
        }

        protected override void AddNewItem()
        {
            if (string.IsNullOrEmpty(NewSubscriberName) || string.IsNullOrEmpty(NewSubscriberSurname) || string.IsNullOrEmpty(NewSubscriberPatronimic) || NewSubscriberPhoneNumber == 0)
            {
                MessageBox.Show("Для добавления абонента все поля должны быть заполнены!");
                return;
            }
            if (NewSubscriberPhoneNumber.ToString().Length < 10)
            {
                MessageBox.Show("Не хватает цифр в номере телефона.");
                return;
            }
            if (MessageBox.Show($"Вы действительно хотите добавить абонента {NewSubscriberSurname} {NewSubscriberName} {NewSubscriberPatronimic}?", "Подтверждение", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                Items.Add(Subscriber.Create(new Subscriber()
                {
                    Name = NewSubscriberName,
                    Surname = NewSubscriberSurname,
                    Patronimic = NewSubscriberPatronimic,
                    PhoneNumber = $"+7{NewSubscriberPhoneNumber}"
                }));
                NewSubscriberName = null;
                NewSubscriberSurname = null;
                NewSubscriberPatronimic = null;
                NewSubscriberPhoneNumber = 0;
            }
        }

        protected override void UpdateItem()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("Выберите абонента, которого хотите изменить.");
                return;
            }
            if (string.IsNullOrEmpty(NewSubscriberName) && string.IsNullOrEmpty(NewSubscriberSurname) && string.IsNullOrEmpty(NewSubscriberPatronimic) && NewSubscriberPhoneNumber == 0)
            {
                MessageBox.Show("Хотя бы одно поле должно быть заполнено для изменения.");
                return;
            }
            if (NewSubscriberPhoneNumber.ToString().Length < 10 && NewSubscriberPhoneNumber != 0)
            {
                MessageBox.Show("Не хватает цифр в номере телефона.");
                return;
            }
            if (MessageBox.Show($"Вы действительно хотите изменить абонента {SelectedItem}?", "Подтверждение", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                Subscriber.Update(new Subscriber()
                {
                    Id = SelectedItem.Id,
                    Name = NewSubscriberName ?? SelectedItem.Name,
                    Surname = NewSubscriberSurname ?? SelectedItem.Surname,
                    Patronimic = NewSubscriberPatronimic ?? SelectedItem.Patronimic,
                    PhoneNumber = NewSubscriberPhoneNumber != 0 ? $"+7{NewSubscriberPhoneNumber}" : SelectedItem.PhoneNumber
                });
                Items = new ObservableCollection<Subscriber>(Subscriber.GetAll(x => true));
                NewSubscriberName = null;
                NewSubscriberSurname = null;
                NewSubscriberPatronimic = null;
                NewSubscriberPhoneNumber = 0;
            }
        }
        public SubscriberViewModel()
        {
            Items = new ObservableCollection<Subscriber>(Subscriber.GetAll(x => true));
        }

    }
}
