using Archive_System.Model.Attributes;
using Archive_System.Model.Data;
using Archive_System.Model.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;

namespace Archive_System.Model
{
    [ClassNameInRu("абонент")]
    public class Subscriber : BaseModel<Subscriber>, IDbModel<Subscriber>, IComparable
    {
        string name;
        [Searchable]
        public string Name {
            get => name;
            set {
                if(name != value)
                {
                    name = value;
                    NotifyPropertyChanged();
                }
            }
        } 
        string surname;
        [Searchable]
        public string Surname {
            get => surname;
            set {
                if (surname != value)
                {
                    surname = value;
                    NotifyPropertyChanged();
                }
            }
        } 
        string patronimic;
        [Searchable]
        public string Patronimic {
            get => patronimic;
            set {
                if (patronimic != value)
                {
                    patronimic = value;
                    NotifyPropertyChanged();
                }
            }
        } 
        string phoneNumber;
        [Searchable]
        public string PhoneNumber {
            get => phoneNumber;
            set {
                if (phoneNumber != value)
                {
                    phoneNumber = value;
                    NotifyPropertyChanged();
                }
            }
        } 
        public ObservableCollection<Document> Documents { get; set; } = [];
        public ObservableCollection<IssuedDocument> IssuedDocuments { get; set; } = [];

        public static Subscriber Create(Subscriber item)
        {
            using (ApplicationContext db = new())
            {
                try
                {
                    Subscriber subscriber = new()
                    {
                        Name = item.Name,
                        Surname = item.Surname,
                        Patronimic = item.Patronimic,
                        PhoneNumber = item.PhoneNumber
                    };
                    db.Subscribers.Add(subscriber);
                    db.SaveChanges();
                    MessageBox.Show("Успешно добавлено!");
                    return subscriber;
                }
                catch
                {
                    MessageBox.Show("Не удалось добавить!");
                    return null;
                }
            }
        }

        public static IEnumerable<Subscriber> GetAll(Func<Subscriber, bool> selector)
        {
            using (ApplicationContext db = new())
                return db.Subscribers
                    .ToList()
                    .Where(x => selector(x));
        }

        public static Subscriber GetById(int id)
        {
            using (ApplicationContext db = new())
                return db.Subscribers.First(x => x.Id == id);
        }

        public static bool Remove(Subscriber item)
        {
            using (ApplicationContext db = new())
            {
                try
                {
                    db.Subscribers.Remove(item);
                    db.SaveChanges();
                    MessageBox.Show($"Абонент {item} удалён из БД.");
                    return true;
                }
                catch
                {
                    MessageBox.Show($"Абонента {item} не удалось удалить из БД.");
                    return false;
                }
            }
        }

        public static Subscriber Update(Subscriber item)
        {
            string str = item.ToString();
            using (ApplicationContext db = new())
            {
                try
                {
                    Subscriber subscriber = db.Subscribers.First(x => x.Id == item.Id);
                    subscriber.Name = item.Name;
                    subscriber.Surname = item.Surname;
                    subscriber.Patronimic = item.Patronimic;
                    subscriber.PhoneNumber = item.PhoneNumber;
                    db.SaveChanges();
                    MessageBox.Show($"Абонент обновлён.");
                    return subscriber;
                }
                catch
                {
                    MessageBox.Show($"Абонента не удалось обновить.");
                    return null;
                }
            }
        }

        public override string ToString()
        {
            return $"{Surname} {Name} {Patronimic}";
        }

        public int CompareTo(object? obj)
        {
            if (obj == null)
                return 1;
            Subscriber subscriber = (Subscriber)obj;

            int surnameComparison = string.Compare(Surname, subscriber.Surname, StringComparison.CurrentCulture);
            if (surnameComparison != 0)
                return surnameComparison;

            int nameComparison = string.Compare(Name, subscriber.Name, StringComparison.CurrentCulture);
            if (nameComparison != 0)
                return nameComparison;

            int patronimicComparison = string.Compare(Patronimic, subscriber.Patronimic, StringComparison.CurrentCulture);
            if (patronimicComparison != 0)
                return patronimicComparison;

            return 0;
        }

        public Subscriber()
        {
            Name = string.Empty;
            Surname = string.Empty;
            Patronimic = string.Empty;
            PhoneNumber = string.Empty;
        }
    }
}
