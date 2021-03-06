﻿using posk.BLL;
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

        public int TotalIV { get; set; }


        public List<ItemVenta> ListaRollosTabla { get; set; }

        public bool Entrada { get; set; }

        public envoltura Envoltura { get; set; }

        public int Extra_ { get; set; }

        public agregado PaltaCebollin { get; set; }

        public promocione Promocion { get; set; }

        public List<ItemAgregadoHandroll> listaAgregadosSushi { get; set; }

        private producto producto;
        public producto Producto
        {
            get { return producto; }
            set { producto = value; }
        }

        public opcionale Opcion { get; set; }

        public int LimiteIngrGlobal { get; set; }

        public int sumarAlTotal { get; set; }

        public List<ItemIngrediente> listaIngredientes { get; set; }

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

            sumarAlTotal = 0;


            btnLapiz.Click += (se, a) => expNota.IsExpanded ^= true;

            Loaded += (se, ev) =>
            {
                btnSinQuesoCrema.Click += (se2, a2) => txtNota.Text += $" SIN QUESO CREMA";

                /*
                if (Producto.nombre.Equals("COBRO EXTRA"))
                    txtTotal.IsReadOnly = false;
                else
                    txtTotal.IsReadOnly = true;
                */

                //txtTotal.IsReadOnly = false;

                if (Envoltura != null)
                {
                    cbEnvoltura.ItemsSource = EnvolturaBLL.ObtenerTodas();
                    cbEnvoltura.DisplayMemberPath = "nombre";
                    cbEnvoltura.Text = Envoltura.nombre;
                    expEnvoltura.IsExpanded = true;
                }
                else
                    expEnvoltura.IsExpanded = false;

                if (Opcion != null)
                {
                    if (Opcion.nombre.Contains("N/A"))
                    {
                        txtOpcion.Text = "N/A";
                    }
                    else
                    {
                        txtOpcion.Text = Opcion.nombre;
                    }
                    expOpcion.IsExpanded = true;
                }
                else
                    expOpcion.IsExpanded = false;

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

                /* TODO - eliminar
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
                */

                if (Preparacion != null)
                    lbDetalle.Text += $" / {Preparacion.nombre}".ToUpper();

                /* TODO - eliminar
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
                */

                
                listaIngredientes?.OrderBy(x => x.Cantidad).ToList().ForEach(a =>
                {
                    if (a.Cantidad == 1)
                        tbAgregados.Text += $"{a.Ingrediente.nombre}, ";
                    else
                        tbAgregados.Text += $"{a.Ingrediente.nombre} x{a.Cantidad}, ";
                });
                try
                {
                    if (!String.IsNullOrEmpty(tbAgregados.Text))
                        tbAgregados.Text = tbAgregados.Text.Substring(0, tbAgregados.Text.Length - 2);
                }
                catch (Exception ex)
                {
                    PoskException.Make(ex, "ERROR AL GENERAR LISTA INGREDIENTES");
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
            var sumar = 0;
            listaIngredientes?.OfType<ItemIngrediente>().ToList().ForEach(x => {
                if (x.Ingrediente != null)
                    sumar += x.Ingrediente.precio;
            });
            if (Opcion != null)
                sumar += Opcion.precio;

            Cantidad += cantidad;
            int cobroExtra = ObtenerCobroExtra();

            if (Cantidad % 1 == 0)
                lbCantidad.Content = $"x{Convert.ToInt32(Cantidad)}";
            if (Cantidad > 1)
            {
                if (Producto != null)
                {
                    lbPrecioUnitario.Content = $"${Producto?.precio + cobroExtra + sumar} c/u  ";
                    txtTotal.Text = $"{(Producto?.precio + cobroExtra + sumar) * Cantidad}";

                }
                else if (Promocion != null)
                {
                    lbPrecioUnitario.Content = $"${Promocion?.precio + cobroExtra + sumar} c/u  ";
                    txtTotal.Text = $"{(Producto?.precio + cobroExtra + sumar) * Cantidad}";
                }
                btnQuitarUnidad.Content = iconQuitarUnidad;
            }
            else
            {
                if (Producto != null)
                    txtTotal.Text = $"{Producto?.precio + cobroExtra + sumar}";
                else if (Promocion != null)
                    txtTotal.Text = $"{Promocion?.precio + cobroExtra + sumar}";
                lbPrecioUnitario.Content = "";
                btnQuitarUnidad.Content = iconBorrar;
            }
            //CalcularTotal();
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
            int? cobroExtra = Extra_;
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
            //string agregadosTemp = $"{PaltaCebollin.nombre}";

            string agregadosTemp = "";

            //if (PaltaCebollin != null)
            //    agregadosTemp += $"{PaltaCebollin.nombre} - ";

            listaAgregadosSushi.ForEach(a =>
            {
                if (a.Cantidad == 1)
                    agregadosTemp += $"{a.Agregado.nombre}, ";
                else
                    agregadosTemp += $"{a.Agregado.nombre} x{a.Cantidad}, ";

            });
            if (agregadosTemp != "")
                agregadosTemp = agregadosTemp.Substring(0, agregadosTemp.Length - 2);
            agregadosTemp.ToUpper();
            return agregadosTemp;
        }

        public string ObtenerIngredientesStr()
        {
            string ingredientesTemp = "";

            listaIngredientes?.ForEach(a =>
            {
                if (a.Cantidad == 1)
                    ingredientesTemp += $"{a.Ingrediente.nombre}, ";
                else
                    ingredientesTemp += $"{a.Ingrediente.nombre} x{a.Cantidad}, ";

            });
            if (ingredientesTemp != "")
                ingredientesTemp = ingredientesTemp.Substring(0, ingredientesTemp.Length - 2);
            ingredientesTemp.ToUpper();
            return ingredientesTemp;
        }

        public int CalcularTotal()
        {
            int? cobroExtra = Extra_;
            if (AgregadoUno != null && AgregadoUno?.cobro_extra != null)
                cobroExtra += AgregadoUno?.cobro_extra;
            if (AgregadoDos != null && AgregadoDos?.cobro_extra != null)
                cobroExtra += AgregadoDos?.cobro_extra;

            decimal? total = 0;
            if (cobroExtra != 0)
                total += cobroExtra * Cantidad;

            if (Producto != null)
                total += Producto.precio * Cantidad;
            if (Promocion != null)
                total += Promocion.precio * Cantidad;

            cobroExtra = ObtenerCobroExtra();

            if (total % 1 == 0)
                txtTotal.Text = $"{total + cobroExtra + sumarAlTotal}";
            else
                txtTotal.Text = $"${Convert.ToInt32(total) + cobroExtra + sumarAlTotal}";
            Extra_ = (int) cobroExtra + sumarAlTotal;
            return (int)total;

            //if (TotalIV != 0)
            //{
            //    txtTotal.Text = $"{TotalIV + Extra}";
            //    return TotalIV + Extra;
            //}

            //int? totalAgregados = 0;

            //if (AgregadoUno == null && AgregadoDos == null)
            //    totalAgregados = 0;
            //if (AgregadoUno != null)
            //    totalAgregados += AgregadoUno.cobro_extra;
            //if (AgregadoDos != null)
            //    totalAgregados += AgregadoDos.cobro_extra;

            //if (totalAgregados != 0 && totalAgregados != null)
            //    lbTotal.Content = $"${producto?.precio} + ${totalAgregados} = ${producto?.precio + totalAgregados}";
            //else
            //    lbTotal.Content = $"${producto?.precio}";

            /*
            if (ListaRollosTabla != null)
            {
                int cantidadIngrTabla = 0; // la cantidad de ingredientes que tiene la tabla contando todos sus rollos
                int? limiteIngrTabla = 0; // la cantidad de ingredientes que debería tener una tabla contando todos sus rollos (sin ing extra)
                int? ingredienteExtraTabla = 0; // la cantidad de ingredientes extra que tiene una tabla considerando todos los rollos
                int limiteIngrRolloTabla = 2; // el numero de ingredientes que debería tener cada rollo de la tabla (sin ing extra)
                int valorIngrExrtaTabla = 500; // el valor de un ingrediente extra en un rollo de tabla

                cobroExtra = 0;
                ListaRollosTabla.ForEach(iv => iv.listaAgregadosSushi.ForEach(x => cantidadIngrTabla += x.Cantidad));
                limiteIngrTabla = Producto.cantidad_rollos_tabla * limiteIngrRolloTabla;
                ingredienteExtraTabla = cantidadIngrTabla - limiteIngrTabla;
                if (cantidadIngrTabla >= limiteIngrTabla) cobroExtra = valorIngrExrtaTabla * (ingredienteExtraTabla);

                if (TotalIV != 0 && cobroExtra != null)
                {
                    txtTotal.Text = $"{TotalIV + cobroExtra}";
                    return TotalIV + (int)cobroExtra;
                }
                if (total % 1 == 0)
                    txtTotal.Text = $"{total + cobroExtra}";
                else
                    txtTotal.Text = $"${Convert.ToInt32(total) + cobroExtra}";
                return (int)total;
            }
            */


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

        private int ObtenerCobroExtra()
        {
            int cobroExtra = 0;

            int cantidadIngr = 0;
            int limiteIngr = 0;
            int valorIngExtra = 500; // TODO - obtener valor de bd

            if (listaIngredientes != null)
            {
                if (LimiteIngrGlobal == 0)
                    limiteIngr = 1000;
                else
                    limiteIngr = LimiteIngrGlobal;

                listaIngredientes?.OfType<ItemIngrediente>().ToList().ForEach(x =>
                {
                    cantidadIngr += x.Cantidad;
                    if (x.Ingrediente != null)
                        sumarAlTotal += x.Ingrediente.precio;
                });
                if (Opcion != null)
                    sumarAlTotal += Opcion.precio;

                if (cantidadIngr >= limiteIngr) cobroExtra = valorIngExtra * (cantidadIngr - limiteIngr);
            }
            Extra_ = cobroExtra;
            return cobroExtra;
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
