using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookReader
{
	/// <summary>
	/// Interaction logic for PageListItem.xaml
	/// </summary>
	public partial class PageListItem : UserControl
	{
		public PageListItem()
		{
			this.InitializeComponent();
            
		}
        #region 数据逻辑
        List<BookInfo> bookList = new List<BookInfo>();
        public List<BookInfo> BookList
        {
            get { return bookList; }
            set
            {
                bookList = value;
                LoadControls(value);
            }
        }
        private void LoadControls(List<BookInfo> value)
        {
            if (value.Count != 0 && value.Count < 9)
            {
                GD_Main.Children.Clear();
                for (int i = 0; i < value.Count; i++)
                {
                    Button b = new Button();
                    b.Template = this.FindResource("BookControlTemplate2") as ControlTemplate;
                    b.Tag = i;
                    b.Click += new RoutedEventHandler(BookButton_Click);
                    b.Content =value[i].Name;
                    b.Style = this.FindResource("BookButtonStyle" + (i % 4 + 1)) as Style;
                    GD_Main.Children.Add(b);
                    Grid.SetColumn(b, i % 4);
                    Grid.SetRow(b, i / 4);
                }
            }
        } 
        #endregion
        #region 界面逻辑
        void BookButton_Click(object sender, RoutedEventArgs e)
        {
            if (BookButtonClicked != null)
            {
                int index = (int)(sender as Button).Tag;
                if (index >= 0 && index < bookList.Count)
                    BookButtonClicked(sender, bookList[index]);
            }
        }
        public delegate void BookButtonClickedEventHandler(object sender, BookInfo b);
        public event BookButtonClickedEventHandler BookButtonClicked; 
        #endregion

	}
}