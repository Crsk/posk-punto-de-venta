using posk.BLL;
using posk.Controls;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace posk.Popup
{
    public partial class ProveedorPopup : UserControl
    {
        public event EventHandler<string> OnSelect;
        private ItemTeclado teclado;
        private List<TextBox> listaItemsTeclado;

        public ProveedorPopup()
        {
            InitializeComponent();
            listaItemsTeclado = new List<TextBox>();
            listaItemsTeclado.Add(txtProveedor);
            listaItemsTeclado.Add(txtRepresentante);
            listaItemsTeclado.Add(txtContacto);

            btnAgregar.Click += (se, a) => AgregarProveedor(txtProveedor.Text, txtRepresentante.Text, txtContacto.Text);
            CargarProveedor();
        }

        private void CargarProveedor()
        {
            spItems.Children.Clear();
            ProveedorBLL.GetAll().ForEach(x =>
            {
                var ic = new ProveedorItem();
                ic.ID = x.id;
                ic.txtProveedor.Text = x.nombre;
                ic.txtRepresentante.Text = x.representante;
                ic.txtContacto.Text = x.contacto;

                ic.spItem.Children.Remove(ic.btnGuardar);
                spItems.Children.Add(ic);

                listaItemsTeclado.Add(ic.txtProveedor);
                listaItemsTeclado.Add(ic.txtRepresentante);
                listaItemsTeclado.Add(ic.txtContacto);

                ic.btnEliminar.Click += (se, a) =>
                {
                    ProveedorBLL.Eliminar(x.id);
                    CargarProveedor();
                };
                ic.btnEditar.Click += (se, a) =>
                {
                    ic.txtProveedor.IsReadOnly = false;
                    ic.txtRepresentante.IsReadOnly = false;
                    ic.txtContacto.IsReadOnly = false;
                    ic.spItem.Children.Clear();
                    ic.spItem.Children.Add(ic.btnEliminar);
                    ic.spItem.Children.Add(ic.dpItem);
                    ic.spItem.Children.Add(ic.btnGuardar);
                };
                ic.btnGuardar.Click += (se, a) =>
                {
                    ProveedorBLL.Actualizar(x.id, ic.txtProveedor.Text, ic.txtRepresentante.Text, ic.txtContacto.Text);
                    ic.txtProveedor.IsReadOnly = true;
                    ic.txtRepresentante.IsReadOnly = true;
                    ic.txtContacto.IsReadOnly = true;
                    ic.spItem.Children.Remove(ic.btnGuardar);
                    ic.spItem.Children.Add(ic.btnEditar);
                    ic.spItem.Children.Add(ic.btnAgregar);
                };
                ic.btnAgregar.Click += (se, a) =>
                {
                    OnSelect(this, ic.txtProveedor.Text);
                };
            });
            teclado = new ItemTeclado(listaItemsTeclado);
            borderTeclado.Child = teclado;
        }

        private void AgregarProveedor(string nombre, string representante, string contacto)
        {
            ProveedorBLL.Crear(nombre, representante, contacto);
            CargarProveedor();
            txtProveedor.Clear();
            txtRepresentante.Clear();
            txtContacto.Clear();
            teclado.expTeclado.IsExpanded = false;
        }
    }
}
