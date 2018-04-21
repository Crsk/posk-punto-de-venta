using posk.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using posk.Globals;
using System.Configuration;
using System.Data;
using posk.BLL;
using posk.Models;
using posk.Pages.PopUp;
using System.Windows.Threading;
using System.ComponentModel;
using posk.Components;
using posk.Popup;
using System.Windows.Media.Animation;

namespace posk.Components
{
    public partial class PedidoComponent : UserControl
    {
        #region Global

        #endregion

        #region Constructor
        public PedidoComponent()
        {
            InitializeComponent();
            InitEvents();
            lbUsuario.Content = $"Garzón: {Settings.NombreUsuario}";
            DispatcherTimer dtClockTime = new DispatcherTimer();
            dtClockTime.Interval = new TimeSpan(0, 0, 1); //in Hour, Minutes, Second.
            dtClockTime.Tick += dtClockTime_Tick;
            dtClockTime.Start();
        }
        private void dtClockTime_Tick(object sender, EventArgs e)
        {
            try
            {
                if (InternetChecker.IsConnectedToInternet())
                {
                    lbFecha.Content = $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}";
                    internetStatus.Foreground = new SolidColorBrush(Color.FromRgb(46, 139, 87));
                }
                else
                {
                    lbFecha.Content = $"{DateTime.Now}";
                    internetStatus.Foreground = new SolidColorBrush(Color.FromRgb(252, 52, 52));
                }
            }
            catch (Exception)
            {
                //lbInfo.Content = "ERROR: " + err.Message;
            }
        }


        #endregion

        #region Metodos

        #endregion

        #region Eventos
        private void InitEvents()
        {

        }
        #endregion Eventos



    }
}
