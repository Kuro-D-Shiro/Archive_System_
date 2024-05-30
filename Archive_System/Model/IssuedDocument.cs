using Archive_System.Model.Attributes;
using Archive_System.Model.Data;
using Archive_System.Model.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace Archive_System.Model
{
    [ClassNameInRu("выданный документ")]
    public class IssuedDocument : BaseModel<IssuedDocument>, IDbModel<IssuedDocument>
    {
        int documentId;
        public int DocumentId { 
            get => documentId;
            set {
                if(documentId != value)
                {
                    documentId = value;
                    NotifyPropertyChanged();
                }
            }
        }
        Document document;
        [Searchable]
        public Document Document {
            get => document;
            set {
                if (document != value)
                {
                    document = value;
                    NotifyPropertyChanged();
                }
            }
        }
        int subscriberId;
        public int SubscriberId {
            get => subscriberId;
            set {
                if (subscriberId != value)
                {
                    subscriberId = value;
                    NotifyPropertyChanged();
                }
            }
        }
        Subscriber subscriber;
        [Searchable]
        public Subscriber Subscriber {
            get => subscriber;
            set {
                if (subscriber != value)
                {
                    subscriber = value;
                    NotifyPropertyChanged();
                }
            }
        }
        DateTime issueDate;
        [Searchable]
        public DateTime IssueDate {
            get => issueDate;
            set {
                if (issueDate != value)
                {
                    issueDate = value;
                    NotifyPropertyChanged();
                }
            }
        }
        uint instancedCount;
        public uint InstancedCount
        {
            get => instancedCount;
            set
            {
                if (instancedCount != value)
                {
                    instancedCount = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public static IssuedDocument Create(IssuedDocument item)
        {
            using (ApplicationContext db = new())
            {
                try
                {
                    if (item.Document != null)
                    {
                        db.Documents.Attach(item.Document);
                        if (item.Document.Cell != null)
                            db.Cells.Attach(item.Document.Cell);
                    } 
                    if (item.Subscriber != null)
                        db.Subscribers.Attach(item.Subscriber);
                    IssuedDocument issuedDocument = item;
                    db.IssuedDocuments.Add(issuedDocument);
                    db.SaveChanges();
                    MessageBox.Show($"Документ {item.Document} успешло выдан абоненту {item.Subscriber} в количестве {item.InstancedCount}!");
                    return issuedDocument;
                }
                catch
                {
                    MessageBox.Show($"Не удалось выдать документ {item.Document}!");
                    return null;
                }
            }
        }

        public static IEnumerable<IssuedDocument> GetAll(Func<IssuedDocument, bool> selector)
        {
            using (ApplicationContext db = new())
                return db.IssuedDocuments
                    .Include(i => i.Document)
                    .Include(i => i.Subscriber)
                    .ToList()
                    .Where(x => selector(x));
        }

        public static IssuedDocument GetById(int id)
        {
            using (ApplicationContext db = new())
                return db.IssuedDocuments.First(x => x.Id == id);
        }

        public static bool Remove(IssuedDocument item)
        {
            /*throw new NotImplementedException("Этот метод ещё не реализован, либо не будет реализован вообще.");*/
            using (ApplicationContext db = new())
            {
                try
                {
                    db.IssuedDocuments.Remove(item);
                    db.SaveChanges();
                    MessageBox.Show($"Выданный документ {item} удалён из БД.");
                    return true;
                }
                catch
                {
                    MessageBox.Show($"Выданный документ {item} не удалось удалить из БД.");
                    return false;
                }
            }
        }

        public static IssuedDocument Update(IssuedDocument item)
        {
            throw new NotImplementedException("Этот метод ещё не реализован, либо не будет реализован вообще.");
        }

        public override string ToString()
        {
            return $"{Document}-{Subscriber}";
        }
    }
}
