using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace BIMPlugins.ExtStorage.Behaviors
{
    public static class ListBoxBehavior
    {
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.RegisterAttached("SelectedItems", typeof(IList),
                typeof(ListBoxBehavior),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnSelectedItemsChanged));

        public static IList GetSelectedItems(DependencyObject obj) => (IList)obj.GetValue(SelectedItemsProperty);
        public static void SetSelectedItems(DependencyObject obj, IList value) => obj.SetValue(SelectedItemsProperty, value);

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ListBox listBox)
            {
                listBox.SelectedItems.Clear();
                if (e.NewValue is IList items)
                {
                    foreach (var item in items)
                    {
                        listBox.SelectedItems.Add(item);
                    }
                }

                listBox.SelectionChanged += (sender, args) =>
                {
                    var selectedItems = GetSelectedItems(listBox);
                    selectedItems.Clear();
                    foreach (var item in listBox.SelectedItems)
                    {
                        selectedItems.Add(item);
                    }
                };
            }
        }
    }
}