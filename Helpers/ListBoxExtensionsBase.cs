
namespace Stalker2ModManager.Helpers
{
    public static class ListBoxExtensionsBase
    {
        public static object? GetItemAt(this System.Windows.Controls.ListBox listBox, System.Windows.Point point)
        {
            var element = listBox.InputHitTest(point) as System.Windows.DependencyObject;
            while (element != null)
            {
                if (element is System.Windows.Controls.ListBoxItem item)
                {
                    return item.Content;
                }
                element = System.Windows.Media.VisualTreeHelper.GetParent(element);
            }
            return null;
        }
    }
}