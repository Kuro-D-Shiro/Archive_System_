using System.Collections.ObjectModel;

namespace Archive_System.ViewModel
{
    public class TabControlViewModel
    {
        public ObservableCollection<object> DataContext = [new DocumentViewModel(), new CellViewModel(), new SubscriberViewModel(), new IssuedDocumentViewModel()];
    }
}
