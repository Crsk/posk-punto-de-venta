using posk.BLL;
using posk.Controls;
using posk.Globals;
using posk.Models;
using posk.Popup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace posk.Popups
{
    public partial class RealizarPedidoPopup : Window
    {
        // Global
        public event EventHandler<string[]> ReciclarEvent;
        private List<ItemVenta> listaItemsVentaEntrada;
        private List<ItemVenta> listaItemsVentaPlatoFondo;
        private List<ItemVenta> listaItemsVentaOtro;
        private bool bClientesFavToggle { get; set; }
        private ItemTeclado teclado;
        public mesa Mesa { get; set; }
        public pedido Ped { get; set; }
        public int? idMesaClickeada = null;
        private ItemMesaPedido _ItemMesaPedido { get; set; }


        /// <summary>
        /// Recibe modo para saber como operar
        /// REST = crea un pendiente asociado a la mesa
        /// de lo contrario crea items individuales asociados a un usuario
        /// </summary>
        /// <param name="listaItemsVentaEntrada"></param>
        /// <param name="modo"></param>
        public RealizarPedidoPopup(List<ItemVenta> listaItemsVentaEntrada, List<ItemVenta> listaItemsVentaPlatoFondo, List<ItemVenta> listaItemsVentaOtro, string tipoUsuario)
        {
            InitializeComponent();
            if (tipoUsuario.ToLower() == "g")
            {
                gridPadre.Children.Remove(gridUsuarios);
            }
            teclado = new ItemTeclado(new List<TextBox>() { txtEscogerUsuarioEntregar });
            borderTeclado.Child = teclado;
            bClientesFavToggle = true;
            MostrarClientesFav(true);
            this.listaItemsVentaEntrada = listaItemsVentaEntrada;
            this.listaItemsVentaPlatoFondo = listaItemsVentaPlatoFondo;
            this.listaItemsVentaOtro = listaItemsVentaOtro;
            Events();
            PonerMesasPorSector(0); // todas
            PonerSectores();
            btnCancelar.Click += (se, a) => { this.Close(); };
        }

        private void PonerSectores()
        {
            var ispTodo = new ItemSectorPedido();
            ispTodo.lbSector.Content = "TODOS";
            ispTodo.AlClickear += (se, a) =>
            {
                PonerMesasPorSector(0); // todas
                lbSector.Content = $"SECTOR: TODOS";

            };
            wrapSector.Children.Add(ispTodo);

            SectorMesaBLL.ObtenerTodo().ForEach(sectorMesa =>
            {
                var isp = new ItemSectorPedido() { SectorMesa = sectorMesa };
                isp.AlClickear += (se, a) =>
                {
                    PonerMesasPorSector(sectorMesa.id);
                    lbSector.Content = $"SECTOR: {sectorMesa.nombre}".ToString();
                };
                wrapSector.Children.Add(isp);
            });
        }

        private void PonerMesasPorSector(int sectorMesaID)
        {
            var bc = new BrushConverter();
            wrapMesas.Children.Clear();
            if (sectorMesaID == 0)
            {
                MesaBLL.ObtenerTodas().ForEach(mesa =>
                {
                    var imp = new ItemMesaPedido() { Mesa = mesa };
                    imp.AlClickear += (se, a) =>
                    {
                        if (_ItemMesaPedido != null)
                            _ItemMesaPedido.Colorear();
                        _ItemMesaPedido = imp;
                        _ItemMesaPedido.lbMesa.Content = _ItemMesaPedido.Mesa.codigo;
                        _ItemMesaPedido.lbEstado.Content = "selección";
                        lbMesa.Content = $"MESA: { _ItemMesaPedido.Mesa.codigo }";
                        Mesa = _ItemMesaPedido.Mesa;
                        if (Settings.Usuario.tipo.ToLower() == "g")
                            CrearPedido(Settings.Usuario);
                    };
                    wrapMesas.Children.Add(imp);
                });
            }
            else
            {
                MesaBLL.ObtenerTodasPorSector(sectorMesaID).ForEach(mesa =>
                {
                    var imp = new ItemMesaPedido() { Mesa = mesa };
                    imp.AlClickear += (se, a) =>
                    {
                        if (_ItemMesaPedido != null)
                            _ItemMesaPedido.Colorear();
                        _ItemMesaPedido = imp;
                        _ItemMesaPedido.lbMesa.Content = _ItemMesaPedido.Mesa.codigo;
                        _ItemMesaPedido.lbEstado.Content = "selección";
                        lbMesa.Content = $"MESA: { _ItemMesaPedido.Mesa.codigo }";
                        Mesa = _ItemMesaPedido.Mesa;
                        if (Settings.Usuario.tipo.ToLower() == "g")
                            CrearPedido(Settings.Usuario);
                    };
                    wrapMesas.Children.Add(imp);
                });
            }
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

        private void CrearPedido(usuario u)
        {
            Ped = PedidoBLL.Crear(u, DateTime.Now, "mensaje", Mesa.id);
            if (Ped != null)
            {
                MesaBLL.OcuparMesa(Mesa.id);

                listaItemsVentaEntrada.ForEach(x =>
                {
                    try
                    {
                        PedidosProductosBLL.Crear(Ped.id, x.Producto?.id, x.Promocion?.id, x.Cantidad, Convert.ToInt32(x.txtTotal.Text), x.txtNota.Text);
                    }
                    catch
                    {
                    }
                });

                listaItemsVentaPlatoFondo.ForEach(x =>
                {
                    try
                    {
                        pedidos_productos pp = PedidosProductosBLL.Crear(Ped.id, x.Producto?.id, x.Promocion?.id, x.Cantidad, Convert.ToInt32(x.txtTotal.Text), x.txtNota.Text);
                        if (x.AgregadoUno != null && x.AgregadoDos != null)
                            PedidosAgregadosBLL.Crear(pp.id, x.AgregadoUno.id, x.AgregadoDos.id);
                        if (x.Preparacion != null)
                            PedidosPreparaciones.Crear(pp.id, x.Preparacion.id);
                    }
                    catch
                    {
                    }
                });

                listaItemsVentaOtro.ForEach(x =>
                {
                    try
                    {
                        PedidosProductosBLL.Crear(Ped.id, x.Producto?.id, x.Promocion?.id, x.Cantidad, Convert.ToInt32(x.txtTotal.Text), x.txtNota.Text);
                    }
                    catch
                    {
                    }
                });

                string[] array = new string[4];
                array[0] = $"{u?.nombre}";
                array[1] = $"{Mesa?.codigo}";
                array[2] = $"{Ped?.id}";
                ReciclarEvent(this, array);
            }
        }

        private void MostrarUsuarios(List<usuario> listaClientesBusqueda)
        {
            foreach (usuario u in listaClientesBusqueda)
            {
                //if (GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.RESTAURANT.ToString()) && !u.tipo.ToUpper().Equals("G")) continue;
                if (!u.tipo.ToUpper().Equals("G")) continue;

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
                            CrearPedido(u);
                        }
                    }
                    else
                    {
                        //if (GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.KUPAL.ToString()))
                        //{
                        //    if (dateDesde.SelectedDate == null || timeDesde.SelectedTime == null || dateHasta.SelectedDate == null || timeHasta.SelectedTime == null)
                        //    {
                        //        new Notification("CONFIGURA LA FECHA", "antes de arrendar", Notification.Type.Warning, 3);
                        //        return;
                        //    }
                        //}

                        //iteración por cada producto
                        foreach (ItemVenta iv in listaItemsVentaEntrada)
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
                };
            }
        }

        public ItemUsuario CreateItemUsuarioFromUsuario(usuario u)
        {
            ItemUsuario ui = new ItemUsuario()
            {
                ID = u.id,
                Nombre = u.nombre,
                NombreUsuario = u.nombre_usuario,
                Tipo = u.tipo,
                Favorito = u.favorito == false ? false : true,
                Imagen = u.imagen
            };

            string fav = "";
            fav = ui.Favorito == true ? "Favorito: si" : "Favorito: no";
            ui.ToolTip = $"Nombre: {ui.Nombre}\nUsuario: {ui.NombreUsuario}\n{fav}";
            return ui;
        }

        #endregion Metodos

        #region eventos
        private void Events()
        {
            Loaded += (se, a) =>
            {
                //if (GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.RESTAURANT.ToString()))
                //{
                //    spSecciones.Children.Remove(dpArriendo);
                //    MesaBLL.ObtenerTodas().ForEach(m =>
                //    {
                //        ItemMesa im = new ItemMesa() { Mesa = m };
                //        im.btnMesa.Click += (se2, a2) => Mesa = m;
                //        spEscogerMesaPendiente.Children.Add(im);
                //    });
                //}
                //else if (GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.RUA.ToString()))
                //{
                //    spSecciones.Children.Remove(dpMesas);
                //    spSecciones.Children.Remove(dpArriendo);
                //    dpUsuarios.Width = 800;
                //}
                //else if (GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.KUPAL.ToString()))
                //{
                //    spSecciones.Children.Remove(dpMesas);
                //    dateDesde.SelectedDate = DateTime.Now;
                //    timeDesde.SelectedTime = DateTime.Now;
                //    timeHasta.SelectedTime = DateTime.Now;
                //}
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
