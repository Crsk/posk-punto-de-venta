using posk.BLL;
using posk.Controls;
using posk.Globals;
using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace posk.Popup
{
    public partial class UsuarioPendientePopup : UserControl, IDisposable
    {
        // Global
        public event EventHandler ReciclarEvent;
        private List<ItemVenta> listaItemsVenta;
        private List<ItemVenta> listaItemsVentaPlatoFondo;
        private bool bClientesFavToggle { get; set; }
        private ItemTeclado teclado;
        public mesa Mesa { get; set; }

        /// <summary>
        /// Recibe modo para saber como operar
        /// REST = crea un pendiente asociado a la mesa
        /// de lo contrario crea items individuales asociados a un usuario
        /// </summary>
        /// <param name="listaItemsVenta"></param>
        /// <param name="modo"></param>
        public UsuarioPendientePopup(List<ItemVenta> listaItemsVenta, List<ItemVenta> listaItemsVentaPlatoFondo)
        {
            InitializeComponent();
            teclado = new ItemTeclado(new List<TextBox>() { txtEscogerUsuarioEntregar });
            borderTeclado.Child = teclado;
            bClientesFavToggle = true;
            MostrarClientesFav(true);
            this.listaItemsVenta = listaItemsVenta;
            this.listaItemsVentaPlatoFondo = listaItemsVentaPlatoFondo;
            Events();
        }

        #region Metodos
        private void MostrarClientesFav(bool b)
        {
            if (b)
            {
                wrapEscogerUsuario.Children.Clear();
                List<usuario> listaUsuariosFav = UsuarioBLL.GetFavs();
                MostrarUsuarios(listaUsuariosFav);
                btnBusquedaUsuarios_fav.Foreground = new SolidColorBrush(Color.FromRgb(255, 150, 0)); // dorado
            }
            else
            {
                wrapEscogerUsuario.Children.Clear();
                btnBusquedaUsuarios_fav.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
        }

        private void MostrarUsuarios(List<usuario> listaClientesBusqueda)
        {
            foreach (usuario u in listaClientesBusqueda)
            {
                if (GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.RUA.ToString()) && !u.tipo.ToUpper().Equals("G")) continue;

                ItemUsuario ui = CreateItemUsuarioFromUsuario(u);
                wrapEscogerUsuario.Children.Add(ui);

                ui.btnUsuario.Click += (se2, ev2) =>
                {
                    if (GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.RESTAURANT.ToString()))
                    {
                        if (Mesa == null)
                        {
                            new Notification("ESCOGE MESA", "antes de escoger usuario", Notification.Type.Warning, 3);
                            return;
                        }
                        else
                        {
                            pedido ped = PedidoBLL.Crear(u, DateTime.Now, "mensaje", Mesa.id);

                            listaItemsVenta.ForEach(x =>
                            {
                                PedidosProductosBLL.Crear(ped.id, x.Producto.id, x.Promocion.id, x.Cantidad, Convert.ToInt32(x.txtTotal.Text), x.txtNota.Text);
                            });

                            listaItemsVentaPlatoFondo.ForEach(x =>
                            {
                                pedidos_productos pp = PedidosProductosBLL.Crear(ped.id, x.Producto.id, x.Promocion.id, x.Cantidad, Convert.ToInt32(x.txtTotal.Text), x.txtNota.Text);
                                if (x.AgregadoUno != null && x.AgregadoDos != null)
                                    PedidosAgregadosBLL.Crear(pp.id, x.AgregadoUno.id, x.AgregadoDos.id);
                            });
                        }
                    }
                    else
                    {
                        if (GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.KUPAL.ToString()))
                        {
                            if (dateDesde.SelectedDate == null || timeDesde.SelectedTime == null || dateHasta.SelectedDate == null || timeHasta.SelectedTime == null)
                            {
                                new Notification("CONFIGURA LA FECHA", "antes de arrendar", Notification.Type.Warning, 3);
                                return;
                            }
                        }

                        //iteración por cada producto
                        foreach (ItemVenta iv in listaItemsVenta)
                        {
                            decimal? cantidad = iv.Cantidad;
                            for (int i = 1; i <= cantidad; i++)
                            {
                                if (iv.Producto?.id != null)
                                {
                                    producto prod = ProductoBLL.GetProduct(iv.Producto.id);
                                    // ingresar pendiente a base de datos
                                    PendienteBLL.IngresarPendienteProducto(prod, u, DateTime.Now);
                                }
                                else if (iv.Promocion?.id != null)
                                {
                                    promocione promo = PromocionBLL.ObtenerPromocion(iv.Promocion.id);
                                    // ingresar pendiente a base de datos
                                    PendienteBLL.IngresarPendientePromo(promo, u, DateTime.Now);
                                }
                            }
                        }
                    }
                    ReciclarEvent(this, null);
                };
            }
        }

        public ItemUsuario CreateItemUsuarioFromUsuario(usuario u)
        {
            ItemUsuario ui = new ItemUsuario()
            {
                ID = u.id,
                Nombre = u.nombre,
                Tipo = u.tipo,
                Favorito = u.favorito == false ? false : true,
                Imagen = u.imagen
            };

            return ui;
        }

        // TODO - implementar
        void IDisposable.Dispose() { }
        #endregion Metodos

        #region eventos
        private void Events()
        {
            Loaded += (se, a) =>
            {
                if (GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.RESTAURANT.ToString()))
                {
                    spSecciones.Children.Remove(dpArriendo);
                    MesaBLL.ObtenerTodas().ForEach(m =>
                    {
                        ItemMesa im = new ItemMesa() { Mesa = m };
                        im.btnMesa.Click += (se2, a2) => Mesa = m;
                        spEscogerMesaPendiente.Children.Add(im);
                    });
                }
                else if (GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.RUA.ToString()))
                {
                    spSecciones.Children.Remove(dpMesas);
                    spSecciones.Children.Remove(dpArriendo);
                    dpUsuarios.Width = 800;
                }
                else if (GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.KUPAL.ToString()))
                {
                    spSecciones.Children.Remove(dpMesas);
                    dateDesde.SelectedDate = DateTime.Now;
                    timeDesde.SelectedTime = DateTime.Now;
                    timeHasta.SelectedTime = DateTime.Now;
                }
            };

            btnTecladoTogglePendiente.Click += (se, ev) =>
            {
                teclado.ExpTeclado.IsExpanded ^= true;
            };
            btnBusquedaUsuarios_fav.Click += (se, ev) =>
            {
                if (!bClientesFavToggle)
                {
                    MostrarClientesFav(true);
                    bClientesFavToggle = true;
                }
                else
                {
                    MostrarClientesFav(false);
                    bClientesFavToggle = false;
                }
            };

            txtEscogerUsuarioEntregar.TextChanged += (se, ev) =>
            {
                if (txtEscogerUsuarioEntregar.Text.Contains("'") || txtEscogerUsuarioEntregar.Text.Contains("\""))
                    wrapEscogerUsuario.Children.Clear();
                else if (txtEscogerUsuarioEntregar.Text == "")
                {
                    MostrarClientesFav(true);
                    bClientesFavToggle = true;
                }
                else
                {
                    MostrarClientesFav(false);
                    bClientesFavToggle = false;
                    List<usuario> listaUsuariosBusqueda = UsuarioBLL.GetUsers(txtEscogerUsuarioEntregar.Text);
                    if (listaUsuariosBusqueda != null)
                        MostrarUsuarios(listaUsuariosBusqueda);
                    else
                        wrapEscogerUsuario.Children.Clear();
                }
            };
        }
        #endregion Eventos
    }
}
