using posk.BLL;
using posk.Models;
using System.Windows.Controls;

namespace posk.Controls
{
    public partial class ItemVentaPlatoFondo : UserControl
    {
        private producto producto;
        public producto Producto
        {
            get { return producto; }
            set { producto = value; }
        }

        private promocione promo;

        public promocione Promo
        {
            get { return promo; }
            set { promo = value; }
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


        public ItemVentaPlatoFondo()
        {
            AgregadoUno = new agregado();
            AgregadoDos = new agregado();
            //Producto = new producto();

            InitializeComponent();
            Loaded += (se, a) =>
            {
                //cbPreparacion.ItemsSource = PreparacionesBLL.ObtenerTodos();
                //cbPreparacion.DisplayMemberPath = "nombre";
                //cbPreparacion.SelectedValue = Preparacion?.nombre;

                if (Producto != null)
                {
                    lbPlatoPrincipal.Text = Producto.nombre;
                }
                if (promo != null)
                {
                    lbPlatoPrincipal.Text = Promo.nombre;
                }

                if (AgregadoUno == null && AgregadoDos == null)
                {
                    tbAgregados.Text = "";
                    CalcularTotal();
                    if (Preparacion != null)
                        lbPlatoPrincipal.Text += $" / {Preparacion.nombre}".ToUpper();
                    return;
                }
                if (agregadoUno == agregadoDos)
                    tbAgregados.Text = $" CON {agregadoUno?.nombre} X2".ToUpper();
                else
                    tbAgregados.Text = $" CON {agregadoUno?.nombre} y {agregadoDos?.nombre}".ToUpper();

                if (Preparacion != null)
                    lbPlatoPrincipal.Text += $" / {Preparacion.nombre}".ToUpper();

                CalcularTotal();
            };
        }

        private void CalcularTotal()
        {
            int? totalAgregados = 0;

            if (AgregadoUno == null && AgregadoDos == null)
                totalAgregados = 0;
            if (AgregadoUno != null)
                totalAgregados += AgregadoUno.cobro_extra;
            if (AgregadoDos != null)
                totalAgregados += AgregadoDos.cobro_extra;

            if (totalAgregados != 0)
                tbTotal.Text = $"${producto?.precio} + ${totalAgregados} = ${producto?.precio + totalAgregados}";
            else
                tbTotal.Text = $"${producto?.precio}";
        }

        public int? ObtenerTotal()
        {
            int? total = 0;
            if (Producto != null) total += producto.precio;
            if (AgregadoUno != null) total += AgregadoUno.cobro_extra;
            if (AgregadoDos != null) total += AgregadoDos.cobro_extra;

            return total;
        }
    }
}
