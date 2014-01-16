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
	/// Interaction logic for MainControl.xaml
	/// </summary>
	public partial class MainControl : UserControl
	{
        public MainControl()
        {
            this.InitializeComponent();
            Init();
            InitEventHandler();
        }

        #region 初始化
        ManipTest.TouchMouseWithRebound touchClass = new ManipTest.TouchMouseWithRebound();
        private void Init()
        {
            touchClass.InitTouchListOperation(CA_Main, STK_Main);
        }
        private void InitEventHandler()
        {
            this.Loaded += (sender, e) => { InitData(); };
            #region 滑竿操作
            this.CA_ExpSL.PreviewMouseMove += (sender, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    double rate = e.GetPosition(CA_ExpSL).Y / CA_ExpSL.ActualHeight;
                    touchClass.ResetBack(false);
                    SL_Down.Value = rate * SL_Down.Maximum;
                    Canvas.SetTop(STK_Main, -(touchClass.MaxTop) * rate);
                }
            };
            touchClass.EndOperation += () =>
            {
                if (touchClass.MaxTop > 0)
                {
                    SL_Down.Value = SL_Down.Maximum * (-Canvas.GetTop(STK_Main) / touchClass.MaxTop);
                }
            }; 
            #endregion
            #region 按钮事件
            BTN_BackHome.Click += (sender, e) => { App.Current.Shutdown(); };
            BTN_Switch.Click += (sender, e) => { new MainPanel.PopOk().ShowDialog("暂无扩展功能"); };
            #endregion
        } 
        #endregion
        #region 界面逻辑
        private void PageListItem_BookButtonClicked(object sender, BookInfo b)
        {
            GoToExpanderView(b);
        }
        private void GoToExpanderView(BookInfo b)
        {
            ExpandShow exp;
            if (CC_MainEXP.Content == null || (CC_MainEXP.Content as ExpandShow) == null)
            {
                exp = new ExpandShow();
                exp.Closed += () => { GoToListView(); };
                CC_MainEXP.Content = exp;
            }
            else
            {
                exp = (CC_MainEXP.Content as ExpandShow);
            }
            CC_MainEXP.Visibility = Visibility.Visible;
            exp.BookInfo = b;
            exp.RefreshText();
        }
        private void GoToListView()
        {
            CC_MainEXP.Visibility = Visibility.Collapsed;
        }
        private List<BookInfo> GetSplitList(int index, List<BookInfo> allBooks)
        {
            List<BookInfo> list = new List<BookInfo>(8);
            if (8 * index + 7 >= allBooks.Count)
            {
                for (int i = 8 * index; i <= allBooks.Count - 1; i++)
                {
                    list.Add(allBooks[i]);
                }
            }
            else
            {
                for (int i = 8 * index; i <= 8 * index + 7; i++)
                {
                    list.Add(allBooks[i]);
                }
            }
            return list;
        } 
        #endregion
        #region 数据逻辑
        private void InitData()
        {
            AllBooks = App.ViewModel.GetAllBooks();
        }
        List<BookInfo> allBooks;
        public List<BookInfo> AllBooks
        {
            get { return allBooks; }
            set
            {
                allBooks = value;
                LoadControls();
            }
        }
        private void LoadControls()
        {
            STK_Main.Children.Clear();
            int controlNumber = 0;
            if (allBooks.Count % 8 == 0)
            {
                controlNumber = allBooks.Count / 8;
            }
            else
            {
                controlNumber = allBooks.Count / 8 + 1;
            }
            for (int i = 0; i < controlNumber; i++)
            {
                PageListItem p = new PageListItem();
                p.BookButtonClicked += new PageListItem.BookButtonClickedEventHandler(PageListItem_BookButtonClicked);
                p.Margin = new Thickness(0, -1, 0, 0);
                p.BookList = GetSplitList(i, allBooks);
                STK_Main.Children.Add(p);
            }

        }
        #endregion
    }
}