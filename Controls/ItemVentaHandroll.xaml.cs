using posk.Globals;
using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;


namespace posk.Controls
{
    /// <summary>
    /// Interaction logic for ItemVentaHandroll.xaml
    /// </summary>
    public partial class ItemVentaHandroll : UserControl
    {
        public event EventHandler AlEliminar;
        public event EventHandler AlModificarCantidad;
        public event EventHandler AlModificarTotal;
        public int ProductoID { get; set; }
        public int? Cantidad { get; set; }

        public bool Entrada { get; set; }

        public promocione Promocion { get; set; }

        public List<ItemAgregadoHandroll> listaAgregados { get; set; }

        private producto producto;
        public producto Producto
        {
            get { return producto; }
            set { producto = value; }
        }

        private preparacione preparacion;

        public preparacione Preparacion
        {
            get { return preparacion; }
            set { preparacion = value; }
        }
        public string Nota { get; set; }

        public ItemVentaHandroll()
        {
            InitializeComponent();

            btnLapiz.Click += (se, a) => expNota.IsExpanded ^= true;

            Loaded += (se, ev) =>
            {
                if (Cantidad == null) Cantidad = 1;
                else lbCantidad.Content = $"x{Cantidad}";

                if (!string.IsNullOrEmpty(Nota)) txtNota.Text = Nota;

                if (Producto != null)
                {
                    lbDetalle.Text = Producto.nombre.ToUpper();
                }
                if (Promocion != null)
                {
                    lbDetalle.Text = Promocion.nombre.ToUpper();
                }

                if (Preparacion != null)
                    lbDetalle.Text += $" / {Preparacion.nombre}".ToUpper();

                listaAgregados.OrderBy(x => x.Cantidad).ToList().ForEach(a =>
                {
                    if (a.Cantidad == 1)
                        tbAgregados.Text += $"{a.Agregado.nombre}, ";
                    else
                        tbAgregados.Text += $"{a.Agregado.nombre} x{a.Cantidad}, ";
                });
                try
                {
                    tbAgregados.Text = tbAgregados.Text.Substring(0, tbAgregados.Text.Length - 2);
                }
                catch (Exception ex)
                {
                    PoskException.Make(ex, "ERROR AL GENERAR LISTA AGREGADOS");
                }


                lbCantidad.Content = $"x{Cantidad}";
                if (Cantidad % 1 == 0)
                    lbCantidad.Content = $"x{Convert.ToInt32(Cantidad)}";

                CalcularTotal();

                if (Cantidad > 1)
                {
                    if (Producto != null)
                    {
                        lbPrecioUnitario.Content = $"${Producto?.precio} c/u  ";

                    }
                    else if (Promocion != null)
                    {
                        lbPrecioUnitario.Content = $"${Promocion?.precio} c/u  ";

                    }
                    btnQuitarUnidad.Content = iconQuitarUnidad;
                }
                else
                {
                    lbPrecioUnitario.Content = "";
                    btnQuitarUnidad.Content = iconBorrar;
                }

            };

            btnAgregarUnidad.Click += (se, a) => AgregarCantidad(1);
            btnQuitarUnidad.Click += (se, a) =>
            {
                AgregarCantidad(-1);
                if (Cantidad == 0)
                    AlEliminar?.Invoke(this, null);
            };

            txtTotal.TextChanged += (se2, a2) =>
            {
                try
                {
                    int total = Convert.ToInt32(txtTotal.Text);
                    AlModificarTotal?.Invoke(this, null);
                }
                catch
                {
                    txtTotal.Text = "" + CalcularTotal();
                }
            };
        }

        public string ObtenerAgregadosStr()
        {
            string agregadosTemp = "";
            listaAgregados.ForEach(a =>
            {
                agregadosTemp += $"{a.txtNombre.Text} x{a.Cantidad}, ";
            });
            if (agregadosTemp != "")
                agregadosTemp = agregadosTemp.Substring(0, agregadosTemp.Length - 2);
            agregadosTemp.ToUpper();
            return agregadosTemp;
        }

        public void AgregarCantidad(int? cantidad)
        {
            Cantidad += cantidad;
            if (Cantidad % 1 == 0)
                lbCantidad.Content = $"x{Convert.ToInt32(Cantidad)}";
            if (Cantidad > 1)
            {
                if (Producto != null)
                {
                    lbPrecioUnitario.Content = $"${Producto?.precio} c/u  ";

                }
                else if (Promocion != null)
                {
                    lbPrecioUnitario.Content = $"${Promocion?.precio} c/u  ";

                }
                btnQuitarUnidad.Content = iconQuitarUnidad;
            }
            else
            {
                lbPrecioUnitario.Content = "";
                btnQuitarUnidad.Content = iconBorrar;
            }
            CalcularTotal();
            AlModificarCantidad?.Invoke(this, null);
        }
        //public int ObtenerTotal()
        //{
        //    //int total = 0;
        //    //if (Producto != null)
        //    //    total = Convert.ToInt32(Producto.precio * Cantidad);
        //    //else if (Promocion != null)
        //    //    total = Convert.ToInt32(Promocion.precio * Cantidad);
        //    //return total;
        //    return CalcularTotal();
        //}

        public int? ObtenerTotal()
        {
            int? cobroExtra = 0;
            try
            {
                listaAgregados.Where(x => x.CobroExtra != 0).ToList().ForEach(a => cobroExtra += a.CobroExtra);
                
                /*
                if (AgregadoUno != null && AgregadoUno?.cobro_extra != null)
                    cobroExtra += AgregadoUno?.cobro_extra;
                if (AgregadoDos != null && AgregadoDos?.cobro_extra != null)
                    cobroExtra += AgregadoDos?.cobro_extra;
                */
            }
            catch
            {
                txtTotal.Text = "" + ((producto.precio + cobroExtra) * Cantidad);
            }
            try
            {
                if (!string.IsNullOrEmpty(txtTotal.Text))
                    return Convert.ToInt32((txtTotal.Text).Replace("$", string.Empty).Replace(".", string.Empty));
                //return ((producto.precio + cobroExtra) * Cantidad);
                else
                    return ((producto.precio + cobroExtra) * Cantidad);
            }
            catch
            {
                return 0;
            }
        }

        public int CalcularTotal()
        {
            //int? totalAgregados = 0;

            //if (AgregadoUno == null && AgregadoDos == null)
            //    totalAgregados = 0;
            //if (AgregadoUno != null)
            //    totalAgregados += AgregadoUno.cobro_extra;
            //if (AgregadoDos != null)
            //    totalAgregados += AgregadoDos.cobro_extra;


            int? cobroExtra = 0;
            listaAgregados.Where(x => x.CobroExtra != 0).ToList().ForEach(a => cobroExtra += a.CobroExtra);
            /*
            if (AgregadoUno != null && AgregadoUno?.cobro_extra != null)
                cobroExtra += AgregadoUno?.cobro_extra;
            if (AgregadoDos != null && AgregadoDos?.cobro_extra != null)
                cobroExtra += AgregadoDos?.cobro_extra;
            */
            //if (totalAgregados != 0 && totalAgregados != null)
            //    lbTotal.Content = $"${producto?.precio} + ${totalAgregados} = ${producto?.precio + totalAgregados}";
            //else
            //    lbTotal.Content = $"${producto?.precio}";

            decimal? total = 0;
            if (cobroExtra != 0)
                total += cobroExtra * Cantidad;

            if (Producto != null)
                total += Producto.precio * Cantidad;
            if (Promocion != null)
                total += Promocion.precio * Cantidad;


            if (total % 1 == 0)
                txtTotal.Text = $"${Convert.ToInt32(total)}";
            else
                txtTotal.Text = $"{total}";
            return (int)total;
        }

        // cuestionable
        public int ObtenerTotalUnitario()
        {
            int total = 0;
            if (Producto != null)
                total = Convert.ToInt32(Producto.precio);
            else if (Promocion != null)
                total = Convert.ToInt32(Promocion.precio);
            return total;
        }
    }
}