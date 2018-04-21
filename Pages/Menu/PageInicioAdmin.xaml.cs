using posk.BLL;
using posk.Models;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace posk.Pages.Menu
{
    public partial class PageInicioAdmin : Page, INotifyPropertyChanged
    {
        #region Constructor
        public PageInicioAdmin()
        {
            InitializeComponent();
            DataContext = this;
        }
        #endregion Constructor

        #region Otros
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion Otros

    }
}
