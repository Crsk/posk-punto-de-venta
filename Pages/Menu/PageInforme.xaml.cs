using System;
using System.Windows.Controls;
//using LiveCharts;
using System.ComponentModel;

namespace posk.Pages.Menu
{
    public partial class PageInforme : UserControl
    {
        private double _value;
        //private int? _incomes;
        private decimal _incomesDecimal;

        public PageInforme()
        {
            InitializeComponent();

            //PointLabel = chartPoint => $"{chartPoint.Y} {chartPoint.Participation:P}";

            //_incomes = DB.GetIncomesOfTheDay();
            //_incomesDecimal = Convert.ToDecimal(_incomes) / 1000;
            Value = 460;

            DataContext = this;
        }
        public decimal IncomesDecimal
        {
            get { return _incomesDecimal; }
            set
            {
                _incomesDecimal = value;
                OnPropertyChanged("IncomesDecimal");
            }
        }

        //public int? Incomes
        //{
        //    get { return _incomes; }
        //    set
        //    {
        //        _incomes = value;
        //        OnPropertyChanged("Incomes");
        //    }
        //}

        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        //public Func<ChartPoint, string> PointLabel { get; set; }
    }
}
