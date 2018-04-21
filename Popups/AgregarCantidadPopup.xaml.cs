using posk.Controls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace posk.Popup
{
    public partial class AgregarCantidadPopup : UserControl, IDisposable
    {
        int cantidadAnterior, cantidadNueva, cantidadDiferencia;
        public event EventHandler<int> OnAgregarCantidad, OnQuitarCantidad;
        public event EventHandler OnFinish;
        public ItemVenta iv;
        public ItemVentaPedido ivp;

        /// <summary>
        /// Implementar evento <param name="OnFinish"></param> para cerrar overlay al terminar acción
        /// </summary>
        /// <param name="iv"></param>
        public AgregarCantidadPopup(ItemVenta iv, ItemVentaPedido ivp = null)
        {
            InitializeComponent();

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

        }

        void IDisposable.Dispose()
        {
        }
    }
}
