using posk.BLL;
using posk.Globals;
using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace posk.Controls
{
    public partial class ItemVenta : UserControl
    {
        public event EventHandler AlEliminar;
        public event EventHandler AlModificarCantidad;
        public event EventHandler AlModificarTotal;
        public int ProductoID { get; set; }
        public int? Cantidad { get; set; }

        public List<ItemVenta> ListaRollosTabla { get; set; }

        public bool Entrada { get; set; }

        public envoltura Envoltura { get; set; }

        public int Extra { get; set; }

        public promocione Promocion { get; set; }

        public List<ItemAgregadoHandroll> listaAgregadosSushi { get; set; }

        private producto producto;
        public producto Producto
        {
            get { return producto; }
            set { producto = value; }
        }


        private agregado agregadoUno;
        public agregado AgregadoUno
        {
            get { return agregadoUno; }
            set { agregadoUno = value; }
        }

        private agregado agregadoDos;
        public agregado AgregadoDos
        {
            get { return agregadoDos; }
            set { agregadoDos = value; }
        }

        private preparacione preparacion;

        public preparacione Preparacion
        {
            get { return preparacion; }
            set { preparacion = value; }
        }
        public string Nota { get; set; }

        public ItemVenta()
        {
            AgregadoUno = new agregado();
            AgregadoDos = new agregado();


            InitializeComponent();



            btnLapiz.Click += (se, a) => expNota.IsExpanded ^= true;

            Loaded += (se, ev) =>
            {
                btnUnagi.Click += (se2, a2) => txtNota.Text += $" UNAGI";
                btnSoya.Click += (se2, a2) => txtNota.Text += $" SOYA";

                /*
                if (Producto.nombre.Equals("COBRO EXTRA"))
                    txtTotal.IsReadOnly = false;
                else
                    txtTotal.IsReadOnly = true;
                */

                txtTotal.IsReadOnly = false;

                if (Envoltura != null)
                {
                    cbEnvoltura.ItemsSource = EnvolturaBLL.ObtenerTodas();
                    cbEnvoltura.DisplayMemberPath = "nombre";
                    cbEnvoltura.Text = Envoltura.nombre;
                    expEnvoltura.IsExpanded = true;
                }
                else
                    expEnvoltura.IsExpanded = false;

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

                if (listaAgregadosSushi == null)
                {
                    if (AgregadoUno?.id == 0 && AgregadoDos?.id == 0)
                    {
                        tbAgregados.Text = "";
                        CalcularTotal();
                        if (Preparacion != null)
                            lbDetalle.Text += $" / {Preparacion.nombre}".ToUpper();
                        return;
                    }
                    else
                    {
                        if (agregadoUno != null && AgregadoDos != null)
                        {
                            if (agregadoUno == agregadoDos)
                                tbAgregados.Text = $" CON {agregadoUno?.nombre} X2".ToUpper();
                            else
                                tbAgregados.Text = $" CON {agregadoUno?.nombre} y {agregadoDos?.nombre}".ToUpper();
                        }
                    }
                }

                if (Preparacion != null)
                    lbDetalle.Text += $" / {Preparacion.nombre}".ToUpper();


                listaAgregadosSushi.OrderBy(x => x.Cantidad).ToList().ForEach(a =>
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
            int? cobroExtra = Extra;
            try
            {
                if (AgregadoUno != null && AgregadoUno?.cobro_extra != null)
                    cobroExtra += AgregadoUno?.cobro_extra;
                if (AgregadoDos != null && AgregadoDos?.cobro_extra != null)
                    cobroExtra += AgregadoDos?.cobro_extra;
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

        public string ObtenerAgregadosStr()
        {
            string agregadosTemp = "";
            listaAgregadosSushi.ForEach(a =>
            {
                if (a.Cantidad == 1)
                    agregadosTemp += $"{a.txtNombre.Text}, ";
                else
                    agregadosTemp += $"{a.txtNombre.Text} x{a.Cantidad}, ";

            });
            if (agregadosTemp != "")
                agregadosTemp = agregadosTemp.Substring(0, agregadosTemp.Length - 2);
            agregadosTemp.ToUpper();
            return agregadosTemp;
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

            int? cobroExtra = Extra;
            if (AgregadoUno != null && AgregadoUno?.cobro_extra != null)
                cobroExtra += AgregadoUno?.cobro_extra;
            if (AgregadoDos != null && AgregadoDos?.cobro_extra != null)
                cobroExtra += AgregadoDos?.cobro_extra;

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

            int cantidadIngr = 0;
            int limiteIngr = 0;
            int valorIngExtra = 500;
            if (listaAgregadosSushi != null)
            {
                if (Producto.es_handroll == true)
                {
                    limiteIngr = 2;
                    listaAgregadosSushi.OfType<ItemAgregadoHandroll>().ToList().ForEach(x => cantidadIngr += x.Cantidad);
                    if (cantidadIngr >= limiteIngr) cobroExtra = valorIngExtra * (cantidadIngr - limiteIngr);
                }
                else if (Producto.es_superhandroll == true)
                {
                    limiteIngr = 3;
                    listaAgregadosSushi.OfType<ItemAgregadoHandroll>().ToList().ForEach(x => cantidadIngr += x.Cantidad);
                    if (cantidadIngr >= limiteIngr) cobroExtra = valorIngExtra * (cantidadIngr - limiteIngr);
                }
                else
                {
                    limiteIngr = 5;
                    listaAgregadosSushi.OfType<ItemAgregadoHandroll>().ToList().ForEach(x => cantidadIngr += x.Cantidad);
                    if (cantidadIngr >= limiteIngr) cobroExtra = valorIngExtra * (cantidadIngr - limiteIngr);
                }
            }
            int cantidadIngrTabla = 0; // la cantidad de ingredientes que tiene la tabla contando todos sus rollos
            int? limiteIngrTabla = 0; // la cantidad de ingredientes que debería tener una tabla contando todos sus rollos (sin ing extra)
            int? ingredienteExtraTabla = 0; // la cantidad de ingredientes extra que tiene una tabla considerando todos los rollos
            int limiteIngrRolloTabla = 5; // el numero de ingredientes que debería tener cada rollo de la tabla (sin ing extra)
            int valorIngrExrtaTabla = 500; // el valor de un ingrediente extra en un rollo de tabla

            if (ListaRollosTabla != null)
            {
                cobroExtra = 0;
                ListaRollosTabla.ForEach(iv => iv.listaAgregadosSushi.ForEach(x => cantidadIngrTabla += x.Cantidad));
                limiteIngrTabla = Producto.cantidad_rollos_tabla * limiteIngrRolloTabla;
                ingredienteExtraTabla = cantidadIngrTabla - limiteIngrTabla;
                if (cantidadIngrTabla >= limiteIngrTabla) cobroExtra = valorIngrExrtaTabla * (ingredienteExtraTabla);

                if (total % 1 == 0)
                    txtTotal.Text = $"{total + cobroExtra}";
                else
                    txtTotal.Text = $"${Convert.ToInt32(total) + cobroExtra}";
                return (int)total;
            }


            if (total % 1 == 0)
                txtTotal.Text = $"{total + cobroExtra}";
            else
                txtTotal.Text = $"${Convert.ToInt32(total) + cobroExtra}";
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



//Precio editable
/*
 
     
    using posk.Models;
using System;
using System.Windows.Controls;

namespace posk.Controls
{
    public partial class ItemVenta : UserControl
    {
        public event EventHandler AlEliminar;
        public event EventHandler AlModificarCantidad;
        public event EventHandler AlModificarTotal;
        public int ProductoID { get; set; }
        public int? Cantidad { get; set; }

        public promocione Promocion { get; set; }


        private producto producto;
        public producto Producto
        {
            get { return producto; }
            set { producto = value; }
        }


        private agregado agregadoUno;
        public agregado AgregadoUno
        {
            get { return agregadoUno; }
            set { agregadoUno = value; }
        }

        private agregado agregadoDos;
        public agregado AgregadoDos
        {
            get { return agregadoDos; }
            set { agregadoDos = value; }
        }

        private preparacione preparacion;

        public preparacione Preparacion
        {
            get { return preparacion; }
            set { preparacion = value; }
        }

        private int precio;

        public int Precio
        {
            get { return precio; }
            set { precio = value; }
        }



        public ItemVenta()
        {
            AgregadoUno = new agregado();
            AgregadoDos = new agregado();

            InitializeComponent();


            Loaded += (se, ev) =>
            {
                if (Precio == 0) precio = producto.precio;
                if (Cantidad == null) Cantidad = 1;
                else
                {
                    lbCantidad.Content = $"x{Cantidad}";
                    if (Cantidad > 0)
                        lbPrecioUnitario.Content = $"${producto?.precio} c/u  ";
                }

                if (Producto != null)
                {
                    lbDetalle.Text = Producto.nombre.ToUpper();
                }
                if (Promocion != null)
                {
                    lbDetalle.Text = Promocion.nombre.ToUpper();
                }

                if (AgregadoUno?.id == 0 && AgregadoDos?.id == 0)
                {
                    tbAgregados.Text = "";
                    CalcularTotal();
                    if (Preparacion != null)
                        lbDetalle.Text += $" / {Preparacion.nombre}".ToUpper();
                    return;
                }
                else
                {
                    if (agregadoUno != null && AgregadoDos != null)
                    {
                        if (agregadoUno == agregadoDos)
                            tbAgregados.Text = $" CON {agregadoUno?.nombre} X2".ToUpper();
                        else
                            tbAgregados.Text = $" CON {agregadoUno?.nombre} y {agregadoDos?.nombre}".ToUpper();
                    }
                }

                if (Preparacion != null)
                    lbDetalle.Text += $" / {Preparacion.nombre}".ToUpper();




                lbCantidad.Content = $"x{Cantidad}";
                if (Cantidad % 1 == 0)
                    lbCantidad.Content = $"x{Convert.ToInt32(Cantidad)}";

                CalcularTotal();

                if (Cantidad > 1)
                {
                    if (Producto != null)
                    {
                        lbPrecioUnitario.Content = $"${producto?.precio} c/u  ";

                    }
                    else if (Promocion != null)
                    {
                        lbPrecioUnitario.Content = $"${producto?.precio} c/u  ";

                    }
                    btnQuitarUnidad.Content = iconQuitarUnidad;
                }
                else
                {
                    lbPrecioUnitario.Content = "";
                    btnQuitarUnidad.Content = iconBorrar;
                }

                //if (TotalModificado != 0)
                //{
                //    txtTotal.Text = "" + TotalModificado;
                //}
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

        public void AgregarCantidad(int? cantidad)
        {
            Cantidad += cantidad;
            if (Cantidad % 1 == 0)
                lbCantidad.Content = $"x{Convert.ToInt32(Cantidad)}";
            if (Cantidad > 1)
            {
                if (Producto != null)
                {
                    lbPrecioUnitario.Content = $"${producto?.precio} c/u  ";

                }
                else if (Promocion != null)
                {
                    lbPrecioUnitario.Content = $"${producto?.precio} c/u  ";

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
                if (AgregadoUno != null && AgregadoUno?.cobro_extra != null)
                    cobroExtra += AgregadoUno?.cobro_extra;
                if (AgregadoDos != null && AgregadoDos?.cobro_extra != null)
                    cobroExtra += AgregadoDos?.cobro_extra;
            }
            catch
            {
                //txtTotal.Text = "" + ((producto.precio + cobroExtra) * Cantidad);
                txtTotal.Text = "" + ((Precio + cobroExtra) * Cantidad);
            }
            // return ((producto.precio + cobroExtra) * Cantidad);
            try
            {
                if (!string.IsNullOrEmpty(txtTotal.Text))
                    return ((Convert.ToInt32((txtTotal.Text).Replace("$", string.Empty).Replace(".", string.Empty)) + cobroExtra) * Cantidad);
                else
                   return ((Precio + cobroExtra) * Cantidad);
            }
            catch
            {
                return null;
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
            if (AgregadoUno != null && AgregadoUno?.cobro_extra != null)
                cobroExtra += AgregadoUno?.cobro_extra;
            if (AgregadoDos != null && AgregadoDos?.cobro_extra != null)
                cobroExtra += AgregadoDos?.cobro_extra;

            //if (totalAgregados != 0 && totalAgregados != null)
            //    lbTotal.Content = $"${producto?.precio} + ${totalAgregados} = ${producto?.precio + totalAgregados}";
            //else
            //    lbTotal.Content = $"${producto?.precio}";

            decimal? total = 0;
            if (cobroExtra != 0)
                total += cobroExtra * Cantidad;

            if (Producto != null)
                total += Precio * Cantidad;
            if (Promocion != null)
                total += Precio * Cantidad;


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
                total = Convert.ToInt32(Precio);
            else if (Promocion != null)
                total = Convert.ToInt32(Precio);
            return total;
        }
    }
}

     
     
     */
