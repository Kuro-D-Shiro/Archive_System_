using Archive_System.Model.Data;
using System.Linq;
using System.Data;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace Archive_System.Model
{
    public static class DBWorker
    {
        #region ВЫБОРКА ОБЪЕКТА ИЗ БД
        static Cell GetCell(int cellId)
        {
            using (ApplicationContext db = new())
                return db.Cells.First(x => x.Id == cellId);//??
        }
        static Document GetDocument(int documentId)
        {
            using (ApplicationContext db = new())
                return db.Documents.First(x => x.Id == documentId);//??
        }
        static Subscriber GetSubscriber(int subscriberId)
        {
            using (ApplicationContext db = new())
                return db.Subscribers.First(x => x.Id == subscriberId);//??
        }
        static IssuedDocument GetIssuedDocument(int issueDocumentId)
        {
            using (ApplicationContext db = new())
                return db.IssuedDocuments.First(x => x.Id == issueDocumentId);//??
        }
        #endregion

        #region ВЫБОРКА СПИСКА ИЗ БД
        public static IEnumerable<Cell> GetCells(Func<Cell, bool> selector)
        {
            using (ApplicationContext db = new())
                return db.Cells
                    .Include(c => c.Document)
                    .ToList()
                    .Where(x => selector(x));
                    /*(from cell in db.Cells
                        join doc in db.Documents on cell.Id equals doc.CellId
                        select new
                        {
                            cell.Id,
                            cell.RackNumber,
                            cell.ShalfNumber,
                            doc
                        })
                        .Cast<Cell>()
                        .Where(x => selector(x));*/
        }
        public static IEnumerable<Document> GetDocuments(Func<Document, bool> selector)
        {
            using (ApplicationContext db = new())
                return db.Documents
                    .Include(d => d.Cell)
                    .ToList()
                    .Where(x => selector(x));
                    //.Where(x => selector(x));
            /*(from doc in db.Documents
                       join cell in db.Cells on doc.CellId equals cell.Id
                       select new
                       {
                           doc.Id,
                           doc.Title,
                           doc.Theme,
                           doc.DateReceived,
                           doc.InstancedCount,
                           cell
                       })
                       .Cast<Document>()
                       .Where(x => selector(x));*/
        }
        public static IEnumerable<Subscriber> GetSubscribers(Func<Subscriber, bool> selector)
        {
            using (ApplicationContext db = new())
                return db.Subscribers
                    .ToList()
                    .Where(x => selector(x));
        }
        public static IEnumerable<IssuedDocument> GetIssuedDocuments(Func<IssuedDocument, bool> selector)
        {
            using (ApplicationContext db = new())
                return db.IssuedDocuments
                    .ToList()
                    .Where(x => selector(x));
        }
        #endregion

        #region СОЗДАНИЕ ОБЪЕКТА И ПОМЕЩЕНИЕ В БД
        public static bool CreateCell(uint rackNumber, uint shalfNumber)
        {
            using (ApplicationContext db = new())
            {
                try
                {
                    Cell cell = new()
                    {
                        RackNumber = rackNumber,
                        ShalfNumber = shalfNumber
                    };
                    db.Cells.Add(cell);
                    db.SaveChanges();
                    MessageBox.Show("Успешно добавлено!");
                    return true;
                }
                catch
                {
                    MessageBox.Show("Не удалось добавить!");
                    return false;
                }
            }
        }
        public static Document CreateDocument(string title, string theme, uint instancedCount, Cell cell)
        {
            using (ApplicationContext db = new())
            {
                try
                {
                    if(cell.Id == 0)
                    {
                        db.Cells.Add(cell);
                        db.SaveChanges();
                    }
                    db.Cells.Attach(cell); 
                    Document document = new()
                    {
                        Title = title,
                        Theme = theme,
                        InstancedCount = instancedCount,
                        DateReceived = DateTime.Now,
                        Cell = cell
                    };
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
        public static Subscriber CreateSubscriber(string name, string surname, string patrinimic, string phoneNumber) //??
        {
            using (ApplicationContext db = new())
            {
                try
                {
                    Subscriber subscriber = new()
                    {
                        Name = name,
                        Surname = surname,
                        Patronimic = patrinimic,
                        PhoneNumber = phoneNumber
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
        public static bool CreateIssuedDocument(Document document, Subscriber subscriber) //??
        {
            using (ApplicationContext db = new())
            {
                try
                {
                    IssuedDocument issuedDocument = new()
                    {
                        Document = document,
                        DocumentId = document.Id,
                        Subscriber = subscriber,
                        SubscriberId = subscriber.Id,
                        IssueDate = DateTime.Now
                    };
                    db.IssuedDocuments.Add(issuedDocument);
                    db.SaveChanges();
                    MessageBox.Show("Успешно добавлено!");
                    return true;
                }
                catch
                {
                    MessageBox.Show("Не удалось добавить!");
                    return false;
                }
            }
        }
        #endregion

        #region УДАЛЕНИЕ ОБЪЕКТА ИЗ БД
/*        private static string RemoveDbElement<T>(T element, DbSet<T> dbSet)
            where T : class
        {
            dbSet.Remove(element);
            result = $"Ячейка {element} удалена.";
            return result;
        }*/
        public static bool RemoveCell(Cell cell)
        {
            using (ApplicationContext db = new())
            {
                try
                {
                    db.Cells.Remove(cell);
                    db.SaveChanges();
                    MessageBox.Show($"Ячейка {cell} удалена.");
                    return true;
                }
                catch
                {
                    MessageBox.Show($"Ячейку {cell} не удалось удалить.");
                    return false;
                }
            }
        }
        public static bool RemoveDocument(Document document)
        {
            using (ApplicationContext db = new())
            {
                try
                {
                    db.Documents.Remove(document);
                    db.SaveChanges();
                    MessageBox.Show($"Документ {document} удалён из БД.");
                    return true;
                }
                catch
                {
                    MessageBox.Show($"Документ {document} не удалось удалить из БД.");
                    return false;
                }
            }
        }
        public static bool RemoveSubscriber(Subscriber subscriber)
        {
            using (ApplicationContext db = new())
            {
                try
                {
                    db.Subscribers.Remove(subscriber);
                    db.SaveChanges();
                    MessageBox.Show($"Абонент {subscriber} удалён из БД.");
                    return true;
                }
                catch
                {
                    MessageBox.Show($"Абонента {subscriber} не удалось удалить из БД.");
                    return false;
                }
            }
        }
        public static bool RemoveIssuedDocument(IssuedDocument issuedDocument)
        {
            using (ApplicationContext db = new())
            {
                try
                {
                    db.IssuedDocuments.Remove(issuedDocument);
                    db.SaveChanges();
                    MessageBox.Show($"Выданный документ {issuedDocument} удалён из БД.");
                    return true;
                }
                catch
                {
                    MessageBox.Show($"Выданный документ {issuedDocument} не удалось удалить из БД.");
                    return false;
                }
            }
        }
        #endregion

        #region ИЗМЕНЕНИЕ ОБЪЕКТА В БД
        public static void EditCell(Cell oldCell, uint? newRackNumber = null, uint? newShalfNumber = null)
        {
            using (ApplicationContext db = new())
            {
                try
                {
                    Cell cell = db.Cells.First(x => x.Id == oldCell.Id);
                    cell.RackNumber = newRackNumber ?? oldCell.RackNumber;
                    cell.ShalfNumber = newShalfNumber ?? oldCell.ShalfNumber;
                    db.SaveChanges();
                    MessageBox.Show($"Ячекйка {oldCell} перемещена в {cell}.");
                }
                catch
                {
                    MessageBox.Show($"Не удалось переместить ячейку {oldCell}.");
                }
            }
        }
        public static Document EditDocument(Document oldDocument, string? newTitle,
            string? newTheme, Cell? newCell, uint newInstancedCount)
        {
            using (ApplicationContext db = new())
            {
                try
                {
                    Document document = db.Documents.First(x => x.Id == oldDocument.Id);
                    document.Title = newTitle ?? oldDocument.Title;
                    document.Theme = newTheme ?? oldDocument.Theme;
                    document.Cell = newCell ?? oldDocument.Cell;
                    document.InstancedCount = newInstancedCount == 0 ? oldDocument.InstancedCount : newInstancedCount;
                    db.SaveChanges();
                    MessageBox.Show($"Документ {oldDocument} обновлён.");
                    return document;
                }
                catch
                {
                    MessageBox.Show($"Документ {oldDocument} не удалось обновить.");
                    return null;
                }
            }
        }
        public static void EditSubscriber(Subscriber oldSubscriber, string? newName = null,
            string? newSurname = null, string? newPatronimic = null, string? newPhoneNumber = null)
        {
            using (ApplicationContext db = new())
            {
                try
                {
                    Subscriber subscriber = db.Subscribers.First(x => x.Id == oldSubscriber.Id);
                    subscriber.Name = newName ?? oldSubscriber.Name;
                    subscriber.Surname = newSurname ?? oldSubscriber.Surname;
                    subscriber.Patronimic = newPatronimic ?? oldSubscriber.Patronimic;
                    subscriber.PhoneNumber = newPhoneNumber ?? oldSubscriber.PhoneNumber;
                    db.SaveChanges();
                    MessageBox.Show($"Абонент {oldSubscriber} обновлён.");
                }
                catch
                {
                    MessageBox.Show($"Абонент {oldSubscriber} не удалось обновить.");
                }
            }
        }
        #endregion
    }
}
