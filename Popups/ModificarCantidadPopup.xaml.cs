using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using posk.Controls;

namespace posk.Popups
{
    public partial class ModificarCantidadPopup : Window
    {
        public bool bCerrado = false;

        int cantidadAnterior, cantidadNueva, cantidadDiferencia;
        public event EventHandler<int> OnAgregarCantidad, OnQuitarCantidad;
        public event EventHandler OnFinish;
        public ItemVenta iv;
        public ItemVentaPedido ivp;

        public ModificarCantidadPopup(ItemVenta iv, ItemVentaPedido ivp = null)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Deactivated += (se, ev) => { if (!bCerrado) Close(); };

            txtCantidad.KeyUp += (se, a) =>
            {
                if (a.Key == Key.Enter)
                    Terminar();
            };



            if (iv != null)
            {

                this.iv = iv;
                cantidadAnterior = (int)iv.Cantidad;
                txtCantidad.Text = cantidadAnterior + "";
                var itn = new ItemTecladoNumerico(new List<TextBox>() { txtCantidad });
                borderTecladoNumerico.Child = itn;
                txtCantidad.GotFocus += (se, ev) => txtCantidad.Text = "";

                btnListo.Click += (se, ev) =>
                {
                    try
                    {
                        cantidadNueva = Convert.ToInt32(txtCantidad.Text);
                        //iv.AsignarCantidad(cantidadNueva);
                        cantidadDiferencia = cantidadAnterior - cantidadNueva;
                        if (cantidadDiferencia == 0)
                            return;
                        else if (cantidadDiferencia > 0) // quitando cantidad
                            OnQuitarCantidad(this, cantidadDiferencia);
                        else // agregando cantidad
                            OnAgregarCantidad(this, Math.Abs(cantidadDiferencia));
                    }
                    catch
                    {
                        MessageBox.Show("Algo salió mal");
                        txtCantidad.Text = "";
                    }
                    OnFinish(this, null);
                };
                btnCancelar.Click += (se, e) =>
                {
                    OnFinish(this, null);
                };
            }
            else if (ivp != null)
            {
                this.ivp = ivp;
                cantidadAnterior = (int)ivp.Cantidad;
                //txtCantidad.Text = cantidadAnterior + "";
                var itn = new ItemTecladoNumerico(new List<TextBox>() { txtCantidad });
                borderTecladoNumerico.Child = itn;
                txtCantidad.GotFocus += (se, ev) => txtCantidad.Text = "";
                txtCantidad.Focus();

                btnListo.Click += (se, ev) =>
                {
                    Terminar();
                };
                btnCancelar.Click += (se, e) =>
                {
                    OnFinish(this, null);
                };
            }
        }

        private void Terminar()
        {
            try
            {
                cantidadNueva = Convert.ToInt32(txtCantidad.Text);
                ivp.AsignarCantidad(cantidadNueva);
                /*
                cantidadDiferencia = cantidadAnterior - cantidadNueva;
                if (cantidadDiferencia == 0)
                    return;
                else if (cantidadDiferencia > 0) // quitando cantidad
                    OnQuitarCantidad(this, cantidadDiferencia);
                else // agregando cantidad
                    OnAgregarCantidad(this, Math.Abs(cantidadDiferencia));
                    */
            }
            catch
            {
                MessageBox.Show("Algo salió mal");
                txtCantidad.Text = "";
            }
            OnFinish(this, null);
        }

        public void Cerrar()
        {
            bCerrado = true;
            Close();
        }
    }
}
