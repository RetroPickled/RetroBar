using ManagedShell.ShellFolders;
using RetroBar.Extensions;
using RetroBar.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RetroBar
{
    /// <summary>
    /// Interaction logic for QuickLaunchOverflowWindow.xaml
    /// </summary>
    public partial class QuickLaunchOverflowWindow : Window
    {
        private static QuickLaunchOverflowWindow _instance;

        private readonly ShellFolder _quickLaunchFolder;

        private QuickLaunchOverflowWindow(ShellFolder quickLaunchFolder)
        {
            _quickLaunchFolder = quickLaunchFolder;

            InitializeComponent();

            QuickLaunchListView.ItemsSource = _quickLaunchFolder.Files;

            ListCollectionView cvs = (ListCollectionView)CollectionViewSource.GetDefaultView(_quickLaunchFolder.Files);
            cvs.CustomSort = new QuickLaunchOverflowSorter();
        }

        public static void Open(ShellFolder quickLaunchFolder, Point position)
        {
            if (_instance == null)
            {
                _instance = new QuickLaunchOverflowWindow(quickLaunchFolder);
                _instance.Left = position.X + 10;
                _instance.Top = position.Y + 10;
                _instance.Show();
            }
            else
            {
                _instance.Activate();
            }
        }

        private void OverflowCheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            var file = checkBox?.DataContext as ShellFile;

            if (file == null) return;

            checkBox.IsChecked = file.IsAlwaysInOverflow();
        }

        private void OverflowCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            var file = checkBox?.DataContext as ShellFile;
            file?.SetAlwaysInOverflow(true);
        }

        private void OverflowCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            var file = checkBox?.DataContext as ShellFile;
            file?.SetAlwaysInOverflow(false);
        }

        private void OK_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _instance = null;
        }
    }

    /// <summary>
    /// Orders Quick Launch shortcuts in the properties dialog the same way they're ordered on
    /// the toolbar itself, so the list here matches what the user sees left-to-right/top-to-bottom.
    /// </summary>
    public class QuickLaunchOverflowSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            if (x is ShellItem a && y is ShellItem b)
            {
                List<string> desiredSort = Settings.Instance.QuickLaunchOrder;

                bool hasA = desiredSort.Contains(a.Path);
                bool hasB = desiredSort.Contains(b.Path);

                if (!hasA && !hasB) return 0;
                if (!hasA) return 1;
                if (!hasB) return -1;

                return desiredSort.IndexOf(a.Path).CompareTo(desiredSort.IndexOf(b.Path));
            }

            return 0;
        }
    }
}
