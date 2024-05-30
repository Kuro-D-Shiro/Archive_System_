using System.Windows;

namespace Archive_System.Extentions
{
    public static class WindowExtentions
    {
        public static void OpenAndSetCenterPosition(this Window window)
        {
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }
    }
}
