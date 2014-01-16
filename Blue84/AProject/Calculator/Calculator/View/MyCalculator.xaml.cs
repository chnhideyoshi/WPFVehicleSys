using System;
using System.Collections.Generic;
using System.Linq;
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
using Microsoft.Windows.Controls;

namespace Calculator.View
{
    /// <summary>
    /// Interaction logic for MyCalculator.xaml
    /// </summary>
    public partial class MyCalculator : UserControl
    {
        private Button TempButton = new Button();
        private string InputNow = "0";
        private string History;
        private int CalcFlag = 0;
        private bool IsinCalc;
        private decimal OperandsX;
        private decimal OperandsY;
        private bool IsHasDot;
        private bool IsGetResult;
        private bool IsSingleCal;
        public MyCalculator()
        {
            InitializeComponent();
            InitData();
            ShowDate();
            KeyBoardPanel.Children.GetEnumerator();
        }

        #region 初始化数据
        public void InitData()
        {
            TempButton = new Button();
            InputNow = string.Empty;
            IsinCalc = false;
            History = string.Empty;
            CalcFlag = 0;
            OldCalcFlag = 0;
            OperandsX = OperandsY = 0;
            IsHasDot = false;
            IsGetResult = false;
            IsSingleCal = false;
        }
        #endregion

        #region 显示数据
        public void ShowDate()
        {
            if (!History.Equals(string.Empty))
            {
                HistoryScreem.Text = History + " ";
            }
            else
            {
                HistoryScreem.Text = "";
            }
            ResultScreem.Text = (InputNow.Equals(string.Empty) ? "0" : InputNow) + " ";
            if (ResultScreem.Text.Length > 20)
                ResultScreem.Text = ResultScreem.Text.Substring(0, 19) +" ";
        }
        #endregion

        #region 按钮操作
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TempButton = sender as Button;
                if (IsGetResult&&!TempButton.Equals(BEqu))
                {
                    InitData();
                }
                #region 0-9按钮,小数点、正负号操作
                if (TempButton.Equals(B1))
                {
                    if (IsSingleCal)
                    {
                        InputNow = string.Empty;
                        IsSingleCal = false; IsHasDot = false; 
                    }
                    if (InputNow.Length < 18)
                        InputNow += "1";
                }
                else if (TempButton.Equals(B2))
                {
                    if (IsSingleCal)
                    {
                        InputNow = string.Empty;
                        IsSingleCal = false; IsHasDot = false; 
                    }
                    if (InputNow.Length < 18)
                        InputNow += "2";
                }
                else if (TempButton.Equals(B3))
                {
                    if (IsSingleCal)
                    {
                        InputNow = string.Empty;
                        IsSingleCal = false; IsHasDot = false; 
                    }
                    if (InputNow.Length < 18)
                        InputNow += "3";
                }
                else if (TempButton.Equals(B4))
                {
                    if (IsSingleCal)
                    {
                        InputNow = string.Empty;
                        IsSingleCal = false; IsHasDot = false; 
                    }
                    if (InputNow.Length < 18)
                        InputNow += "4";
                }
                else if (TempButton.Equals(B5))
                {
                    if (IsSingleCal)
                    {
                        InputNow = string.Empty;
                        IsSingleCal = false; IsHasDot = false; 
                    }
                    if (InputNow.Length < 18)
                        InputNow += "5";
                }
                else if (TempButton.Equals(B6))
                {
                    if (IsSingleCal)
                    {
                        InputNow = string.Empty;
                        IsSingleCal = false; IsHasDot = false; 
                    }
                    if (InputNow.Length < 18)
                        InputNow += "6";
                }
                else if (TempButton.Equals(B7))
                {
                    if (IsSingleCal)
                    {
                        InputNow = string.Empty;
                        IsSingleCal = false; IsHasDot = false; 
                    }
                    if (InputNow.Length < 18)
                        InputNow += "7";
                }
                else if (TempButton.Equals(B8))
                {
                    if (IsSingleCal)
                    {
                        InputNow = string.Empty;
                        IsSingleCal = false; IsHasDot = false; 
                    }
                    if (InputNow.Length < 18)
                        InputNow += "8";
                }
                else if (TempButton.Equals(B9))
                {
                    if (IsSingleCal)
                    {
                        InputNow = string.Empty;
                        IsSingleCal = false; IsHasDot = false; 
                    }
                    if (InputNow.Length < 18)
                        InputNow += "9";
                }
                else if (TempButton.Equals(B0))
                {
                    if (IsSingleCal)
                    {
                        InputNow = string.Empty;
                        IsSingleCal = false; IsHasDot = false; 
                    }
                    if (InputNow.Length < 18)
                        if (!decimal.Parse(InputNow).Equals(0))
                            InputNow += "0";
                }
                else if (TempButton.Equals(BDot))
                {
                    if (IsSingleCal)
                    {
                        InputNow = string.Empty;
                        IsSingleCal = false; IsHasDot = false; 
                    }
                    if (InputNow.Equals(string.Empty))
                    {
                        InputNow = "0";
                    }
                    if (InputNow.Length < 18 && !IsHasDot)
                    {
                        InputNow += ".";
                        IsHasDot = true;
                    }
                }
                else if (TempButton.Equals(BPAN))
                {
                    if (!double.Parse(InputNow).Equals(0))
                    {
                        InputNow = ((-decimal.Parse(InputNow))).ToString();
                    }
                }
                #endregion

                #region 运算操作
                else if (TempButton.Equals(BPlus))
                {
                    CalcFlag = 1;
                    CalcOperation(CalcFlag);
                }
                else if (TempButton.Equals(BMinus))
                {
                    CalcFlag = 2;
                    CalcOperation(CalcFlag);
                }
                else if (TempButton.Equals(BMuti))
                {
                    CalcFlag = 3;
                    CalcOperation(CalcFlag);
                }
                else if (TempButton.Equals(BDiv))
                {
                    CalcFlag = 4;
                    CalcOperation(CalcFlag);
                }
                else if (TempButton.Equals(BSqrt))
                {
                    IsSingleCal = true;
                    CalcFlag = 5;
                    CalcOperation(CalcFlag);
                }
                else if (TempButton.Equals(BPer))
                {
                    IsSingleCal = true;
                    CalcFlag = 6;
                    CalcOperation(CalcFlag);
                }
                else if (TempButton.Equals(BReci))
                {
                    IsSingleCal = true;
                    CalcFlag = 7;
                    CalcOperation(CalcFlag);
                }
                else if (TempButton.Equals(BEqu))
                {
                    CalcFlag = 8;
                    CalcOperation(CalcFlag);
                }
                #endregion

                #region 功能操作
                else if (TempButton.Equals(BC))
                {
                    InitData();
                    ShowDate();
                }
                else if (TempButton.Equals(BCE))
                {
                    InputNow = string.Empty;
                    IsHasDot = false;
                    ShowDate();
                }
                else if (TempButton.Equals(BBack))
                {
                    InputNow = InputNow.Substring(0, InputNow.Length - 1);
                    ShowDate();
                }
                #endregion
            }
            catch (Exception) { }

            ShowDate();
        }
        #endregion

        #region 运算操作
        private void CalcOperation(int flag)
        {
            if (0 < flag && 5 > flag)
            {
                IsHasDot = false;
                if (!IsinCalc)
                {
                    IsinCalc = true;
                    if (!InputNow.Equals(string.Empty))
                        OperandsX = decimal.Parse(InputNow);
                    else
                        OperandsX = 0;
                    
                    History = OperandsX.ToString() + " " + GetCalcSign(flag);
                    InputNow = string.Empty;
                    OldCalcFlag = flag;
                }
                else
                {
                    if (InputNow.Equals(string.Empty))
                    {
                        History = History.Substring(0, History.Length - 1) + GetCalcSign(flag);
                        OldCalcFlag = flag;
                    }
                    else
                    {
                        if (!InputNow.Equals(string.Empty))
                            OperandsY = decimal.Parse(InputNow);
                        else
                            OperandsY = 0;
                        OperandsX = GetCalcResult(OperandsX, OperandsY, OldCalcFlag);
                        History = OperandsX.ToString() + " " + GetCalcSign(flag);
                        InputNow = string.Empty;
                    }
                }
            }
            else
            {
                switch (flag)
                {
                    case 5:
                        InputNow = GetCalcResult( decimal.Parse(InputNow),0, flag).ToString();
                        break;
                    case 6:
                        InputNow = GetCalcResult( decimal.Parse(InputNow), 0,flag).ToString();
                        break;
                    case 7:
                        InputNow = GetCalcResult( decimal.Parse(InputNow), 0,flag).ToString();
                        break;
                    case 8:
                        if (OldCalcFlag != 0)
                        {
                            if (!InputNow.Equals(string.Empty))
                                OperandsY = decimal.Parse(InputNow);
                            else
                                OperandsY = 0;
                            History = string.Empty;
                            IsinCalc = false;
                            InputNow = GetCalcResult(OperandsX, OperandsY, OldCalcFlag).ToString();
                            OldCalcFlag = 0;
                        }
                        //IsGetResult = true;
                        break;
                }
            }
        }

        private decimal GetCalcResult(decimal x, decimal y, int flag)
        {
            switch (flag)
            {
                case 1:
                    return x + y;
                case 2:
                    return x - y;
                case 3:
                    return x * y;
                case 4:
                    if (!y.Equals(0))
                        return x / y;
                    return 0;
                case 5:
                    if (x >= 0)
                        return (decimal)(Math.Sqrt((double)x));
                    else
                        return x;
                case 6:
                    return x / 100;
                case 7:
                    return (decimal)(1 / (double)x);
            }
            return 0;
        }

        private string GetCalcSign(int flag)
        {
            switch (flag)
            {
                case 1:
                    return "＋";
                case 2:
                    return "－";
                case 3:
                    return "×";
                case 4:
                    return "÷";
            }
            return string.Empty;
        }
        #endregion

        public int OldCalcFlag { get; set; }
    }
}
