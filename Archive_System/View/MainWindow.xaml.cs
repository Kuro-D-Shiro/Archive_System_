using Archive_System.Model;
using Archive_System.Model.Data;
using Archive_System.ViewModel;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;

namespace Archive_System.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ResetSortingDocumentButton_Click(object sender, RoutedEventArgs e)
        {
            var view = CollectionViewSource.GetDefaultView(dataGrid1.ItemsSource);
            view?.SortDescriptions.Clear();

            foreach (var column in dataGrid1.Columns)
            {
                column.SortDirection = null;
            }
        }

        private void ResetSortingCellButton_Click(object sender, RoutedEventArgs e)
        {
            var view = CollectionViewSource.GetDefaultView(dataGrid2.ItemsSource);
            view?.SortDescriptions.Clear();

            foreach (var column in dataGrid2.Columns)
            {
                column.SortDirection = null;
            }
        }

        private void ResetSortingAbonentButton_Click(object sender, RoutedEventArgs e)
        {
            var view = CollectionViewSource.GetDefaultView(dataGrid3.ItemsSource);
            view?.SortDescriptions.Clear();

            foreach (var column in dataGrid3.Columns)
            {
                column.SortDirection = null;
            }
        }

        private void ResetSortingIssueDocumentButton_Click(object sender, RoutedEventArgs e)
        {
            var view = CollectionViewSource.GetDefaultView(dataGrid4.ItemsSource);
            view?.SortDescriptions.Clear();

            foreach (var column in dataGrid4.Columns)
            {
                column.SortDirection = null;
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MyTabControl.SelectedItem is TabItem selectedTab)
            {
                Type dataContextType = selectedTab.DataContext.GetType();
                selectedTab.DataContext = dataContextType.GetConstructor(new Type[] {}).Invoke(null);
            }
        }
    }
}

