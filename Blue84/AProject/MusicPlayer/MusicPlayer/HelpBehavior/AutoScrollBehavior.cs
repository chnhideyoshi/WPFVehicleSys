using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Interactivity;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
namespace MusicPlayer.HelpBehavior
{
    public class AutoScrollBehavior : Behavior<ListBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.SelectionChanged += new SelectionChangedEventHandler(AssociatedObject_SelectionChanged);
        }
        void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox)
            {
                ListBox listbox = (sender as ListBox);
                if (listbox.SelectedItem != null)
                {
                    listbox.Dispatcher.BeginInvoke((Action)delegate
                    {
                        listbox.UpdateLayout();
                        listbox.ScrollIntoView(listbox.SelectedItem);
                    });
                }
            }
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.SelectionChanged -=
                new SelectionChangedEventHandler(AssociatedObject_SelectionChanged);
        }
    }

    public class MouseScrollBehavior : Behavior<ListBox>
    {
        bool canMouseMove = false;
        double vertical;
        ScrollViewer listBoxScroller = new ScrollViewer();
        bool canBtnClick = true;
        ListBox listbox;
        bool ItemsChanged;
        #region Attach&Detach

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.PreviewMouseDown += new MouseButtonEventHandler(AssociatedObject_PreviewMouseDown);
            this.AssociatedObject.PreviewMouseMove += new MouseEventHandler(AssociatedObject_PreviewMouseMove);
            this.AssociatedObject.PreviewMouseUp += new MouseButtonEventHandler(AssociatedObject_PreviewMouseUp);
            this.AssociatedObject.Loaded += new RoutedEventHandler(AssociatedObject_Loaded);
            //当 listbox  中Litems改变的时候标记改变
            this.AssociatedObject.ItemContainerGenerator.ItemsChanged += (sender, e) => ItemsChanged = true;
        }

        void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            listbox = sender as ListBox;
            listBoxScroller = FindVisualChild<ScrollViewer>(listbox);
        }


        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.PreviewMouseDown -= new MouseButtonEventHandler(AssociatedObject_PreviewMouseDown);
            this.AssociatedObject.PreviewMouseMove -= new MouseEventHandler(AssociatedObject_PreviewMouseMove);
            this.AssociatedObject.PreviewMouseUp -= new MouseButtonEventHandler(AssociatedObject_PreviewMouseUp);

            foreach (var item in this.AssociatedObject.Items)
            {
                UIElement listItem = item as UIElement;
                listItem.PreviewMouseUp -= new MouseButtonEventHandler(listItem_PreviewMouseUp);
            }
        }
        #endregion //Attach&Detach

        #region EventHandler
        private void AssociatedObject_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            canBtnClick = true;
            canMouseMove = true;
            vertical = e.GetPosition(null).Y;
            e.Handled = true;
            if (ItemsChanged == true)
            {
                foreach (var item in listbox.Items)
                {
                    ListBoxItem lbi = listbox.ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem;
                    lbi.PreviewMouseUp += new MouseButtonEventHandler(listItem_PreviewMouseUp);
                }
                ItemsChanged = false;
            }
        }

        private void AssociatedObject_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (canMouseMove == true)
            {
                listBoxScroller.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                Point point = e.GetPosition(null);
                var offset = (vertical - point.Y) / 10;
                var temp = listBoxScroller.VerticalOffset;
                listBoxScroller.ScrollToVerticalOffset(temp + offset);
                canBtnClick = false;
            }
        }

        private void AssociatedObject_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            listBoxScroller.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            canMouseMove = false;
            if (canBtnClick == false)
                e.Handled = true;
        }
        private void listItem_PreviewMouseUp(object sender, RoutedEventArgs e)
        {
            var item = sender as ListBoxItem;
            listbox.SelectedItem = item.Content;
        }
        #endregion //EventHandler

        #region HelpMethods

        private ScrollViewer FindVisualChild<ScrollViewer>(DependencyObject obj)
        where ScrollViewer : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is ScrollViewer)
                {
                    return (ScrollViewer)child;
                }
                else
                {
                    ScrollViewer childOfChild = FindVisualChild<ScrollViewer>(child);
                    if (childOfChild != null)
                    {
                        return childOfChild;
                    }
                }
            }
            return null;
        }
        #endregion //HelpMethods

    }
}

