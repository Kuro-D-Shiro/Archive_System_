using Archive_System.Model;
using System.Collections.ObjectModel;
using System.Windows;

namespace Archive_System.ViewModel
{
    class IssuedDocumentViewModel : BaseArchiveViewModel<IssuedDocument>
    {
        protected override void AddNewItem()
        {
            throw new NotImplementedException();
        }

        protected override void UpdateItem()
        {
            throw new NotImplementedException();
        }

        public IssuedDocumentViewModel()
        {
            Items = new ObservableCollection<IssuedDocument>(IssuedDocument.GetAll(x => true));
        }
    }
}
