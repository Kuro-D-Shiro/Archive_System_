using Archive_System.Model.Attributes;
using Archive_System.Model.Data;
using Archive_System.Model.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;

namespace Archive_System.Model
{
    [ClassNameInRu("документ")]
    public class Document : BaseModel<Document>, IDbModel<Document>, IComparable
    {
        string title;
        [Searchable]
        public string Title {
            get => title;
            set {
                if (value != title)
                {
                    title = value;
                    NotifyPropertyChanged();
                }
            }
        }

        string theme;
        [Searchable]
        public string Theme {
            get => theme;
            set {
                if (value != theme)
                {
                    theme = value;
                    NotifyPropertyChanged();
                }
            }
        }
        DateTime dateReceived;
        [Searchable]
        public DateTime DateReceived {
            get => dateReceived;
            set {
                if (value != dateReceived)
                {
                    dateReceived = value;
                    NotifyPropertyChanged();
                }
            }
        }

        uint instancedCount;
        [Searchable]
        public uint InstancedCount {
            get => instancedCount;
            set {
                if (value != instancedCount)
                {
                    if (value == 0)
                        Cell = null;
                    instancedCount = value;
                    NotifyPropertyChanged();
                }
            }
        }
        uint issuedInstancedCount;
        [Searchable]
        public uint IssuedInstancedCount
        {
            get => issuedInstancedCount;
            set
            {
                if (value != issuedInstancedCount)
                {
                    issuedInstancedCount = value;
                    NotifyPropertyChanged();
                }
            }
        }
        int? cellId;
        [Searchable]
        public int? CellId {
            get => cellId;
            set {
                if (value != cellId)
                {
                    cellId = value;
                    NotifyPropertyChanged();
                }
            }
        }

        Cell? cell;
        [Searchable]
        public Cell? Cell {
            get => cell;
            set {
                if (value != cell)
                {
                    cell = value;
                    NotifyPropertyChanged();
                }
            }
        } 
        public ObservableCollection<Subscriber> Subscribers { get; set; } = [];
        public ObservableCollection<IssuedDocument> IssuedDocuments { get; set; } = [];

        public override string ToString()
        {
            return Title;
        }

        public static IEnumerable<Document> GetAll(Func<Document, bool> selector)
        {
            using (ApplicationContext db = new())
                return db.Documents
                    .Include(d => d.Cell)
                    .ToList()
                    .Where(x => selector(x));
        }

        public static Document GetById(int id)
        {
            using (ApplicationContext db = new())
                return db.Documents.First(x => x.Id == id);
        }

        public static Document Create(Document item)
        {
            using (ApplicationContext db = new())
            {
                try
                {
                    if(item.Cell != null)
                        db.Cells.Attach(item.Cell); 
                    Document document = item;
                    db.Documents.Add(document);
                    db.SaveChanges();
                    MessageBox.Show("Успешно добавлено!");
                    return document;
                }
                catch
                {
                    MessageBox.Show("Не удалось добавить!");
                    return null;
                }
            }
        }

        public static Document Update(Document item)
        {
            using (ApplicationContext db = new())
            {
                try
                {
                    Document document = db.Documents.First(x => x.Id == item.Id);
                    document.Title = item.Title;
                    document.Theme = item.Theme;
                    document.InstancedCount = item.InstancedCount;
                    document.IssuedInstancedCount = item.IssuedInstancedCount;
                    document.Cell = item.Cell;
                    db.SaveChanges();
                    MessageBox.Show($"Документ обновлён.");
                    return document;
                }
                catch
                {
                    MessageBox.Show($"Документ не удалось обновить.");
                    return null;
                }
            }
        }

        public static bool Remove(Document item)
        {
            using (ApplicationContext db = new())
            {
                try
                {
                    db.Documents.Remove(item);
                    db.SaveChanges();
                    MessageBox.Show($"Документ {item} удалён из БД.");
                    return true;
                }
                catch
                {
                    MessageBox.Show($"Документ {item} не удалось удалить из БД.");
                    return false;
                }
            }
        }

        public int CompareTo(object? obj)
        {
            if(obj == null)
                return 1;
            
            Document document = (Document)obj;
            return this.Title.CompareTo(document.Title);
        }

        public Document()
        {
            Title = string.Empty;
            Theme = string.Empty;
            Cell = new();
            IssuedInstancedCount = 0;
        }
    }
}
