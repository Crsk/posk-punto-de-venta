using posk.Models;
using System;
using System.Windows;
using System.Windows.Threading;

namespace posk.Pages.PopUp
{
    public partial class WindowPointsInfo : Window
    {
        bool bCerrado = false; // necesario para cerrar la ventana al pasar a segundo plano y evitar error
        punto puntos;
        DateTime startTime, endTime;
        TimeSpan span;

        public WindowPointsInfo(punto pts)
        {
            InitializeComponent();
            this.puntos = pts;
            this.Deactivated += (se, ev) => { if (!bCerrado) Close(); };
            btnCerrar.Click += (se, ev) => { bCerrado = true; Close(); };

            startTime = DateTime.Now;
            endTime = pts.fecha_expiracion.Value;
            span = endTime.Subtract(startTime);
            lbInfo.Content = $"{pts.puntos_activos} puntos activos\n{pts.puntos_expirados} expirados\n\nFecha de expiración: {pts.fecha_expiracion}\nFaltan {span.Days} dias, {span.Hours} horas, {span.Minutes} minutos y {span.Seconds} segundos";
            DispatcherTimer dtClockTime = new DispatcherTimer();
            dtClockTime.Interval = new TimeSpan(0, 0, 1); //in Hour, Minutes, Second.
            dtClockTime.Tick += dtClockTime_Tick;
            dtClockTime.Start();
        }

        private void dtClockTime_Tick(object sender, EventArgs e)
        {
            try
            {
                startTime = DateTime.Now;
                endTime = puntos.fecha_expiracion.Value;
                span = endTime.Subtract(startTime);
                lbInfo.Content = $"{puntos.puntos_activos} puntos activos\n{puntos.puntos_expirados} expirados\n\nFecha de expiración: {puntos.fecha_expiracion}\nFaltan {span.Days} dias, {span.Hours} horas, {span.Minutes} minutos y {span.Seconds} segundos";
            }
            catch
            {
                lbInfo.Content = "ERROR";
            }
        }
    }
}
