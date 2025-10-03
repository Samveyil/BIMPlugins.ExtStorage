using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace BIMPlugins.ExtStorage.Behaviors
{
    public static class DataGridBehavior
    {
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.RegisterAttached("SelectedItems", typeof(IList),
                typeof(DataGridBehavior),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnSelectedItemsChanged));

        public static IList GetSelectedItems(DependencyObject obj) => (IList)obj.GetValue(SelectedItemsProperty);
        public static void SetSelectedItems(DependencyObject obj, IList value) => obj.SetValue(SelectedItemsProperty, value);

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGrid dataGrid)
            {
                dataGrid.SelectedItems.Clear();
                if (e.NewValue is IList items)
                {
                    foreach (var item in items)
                    {
                        dataGrid.SelectedItems.Add(item);
                    }
                }

                dataGrid.SelectionChanged += (sender, args) =>
                {
                    var selectedItems = GetSelectedItems(dataGrid);
                    selectedItems.Clear();
                    foreach (var item in dataGrid.SelectedItems)
                    {
                        selectedItems.Add(item);
                    }
                };
            }
        }
    }
}