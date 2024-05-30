
using Archive_System.Model.Attributes;
using Archive_System.Model.Data;
using Archive_System.Model.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace Archive_System.Model
{
    [ClassNameInRu("ячейка")]
    public class Cell : BaseModel<Cell>, IComparable, IDbModel<Cell>
    {
        uint rackNumber;
        [Searchable]
        public uint RackNumber {
            get => rackNumber;
            set
            {
                if (value != rackNumber)
                {
                    rackNumber = value;
                    NotifyPropertyChanged();
                }
            }
        }
        uint shalfNumber;
        [Searchable]
        public uint ShalfNumber {
            get => shalfNumber;
            set {
                if(value != shalfNumber)
                {
                    shalfNumber = value;
                    NotifyPropertyChanged();
                }
            }
        }
        Document? document;
        [Searchable]
        public Document? Document {
            get => document;
            set
            {
                if (value != null && !value.Equals(document))
                {
                    document = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public bool IsFilled
        {
            get
            {
                if (Document?.InstancedCount == 0)
                    Document = null;
                return Document != null;
            }
        }

        public static Cell Create(Cell item)
        {
            using (ApplicationContext db = new())
            {
                try
                {
                    if(item.Document != null)
                        db.Documents.Attach(item.Document);
                    Cell cell = item;
                    db.Cells.Add(cell);
                    db.SaveChanges();
                    MessageBox.Show("Успешно добавлено!");
                    return cell;
                }
                catch
                {
                    MessageBox.Show("Не удалось добавить!");
                    return null;
                }
            }
        }

        public static IEnumerable<Cell> GetAll(Func<Cell, bool> selector)
        {
            using (ApplicationContext db = new())
                return db.Cells
                    .Include(c => c.Document)
                    .ToList()
                    .Where(x => selector(x));
        }

        public static Cell GetById(int id)
        {
            using (ApplicationContext db = new())
                return db.Cells.First(x => x.Id == id);
        }

        public static bool Remove(Cell item)
        {
            using (ApplicationContext db = new())
            {
                try
                {
                    db.Cells.Remove(item);
                    db.SaveChanges();
                    MessageBox.Show($"Ячейка {item} удалена.");
                    return true;
                }
                catch
                {
                    MessageBox.Show($"Ячейку {item} не удалось удалить.");
                    return false;
                }
            }
        }

        public static Cell Update(Cell item)
        {
            string str = item.ToString();
            using (ApplicationContext db = new())
            {
                try
                {
                    Cell cell = db.Cells.First(x => x.Id == item.Id);
                    cell.RackNumber = item.RackNumber;
                    cell.ShalfNumber = item.ShalfNumber;
                    cell.Document = item.Document;
                    db.SaveChanges();
                    MessageBox.Show($"Ячекйка перемещена в {str}.");
                    return cell;
                }
                catch
                {
                    MessageBox.Show($"Не удалось переместить ячейку в {str}.");
                    return null;
                }
            }
        }

        public int CompareTo(object? obj)
        {
            if (obj == null)
                return 1;
            Cell cell = (Cell)obj;

            int rackComparison = RackNumber.CompareTo(cell.RackNumber);
            if (rackComparison != 0)
                return rackComparison;

            int shalfComparison = ShalfNumber.CompareTo(cell.ShalfNumber);
            if (shalfComparison != 0)
                return shalfComparison;

            return Id.CompareTo(cell.Id);
        }

        public override string ToString()
        {
            return $"{RackNumber}/{ShalfNumber}/{Id}";
        }

        public Cell()
        {
            Document = null;
        }
    }
}
