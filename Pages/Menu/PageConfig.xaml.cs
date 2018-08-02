﻿using posk.BLL;
using posk.Globals;
using posk.Models;
using MySql.Data.MySqlClient;
using System;

using posk.Popup;
using System.Deployment.Application;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Collections.Generic;

namespace posk.Pages.Menu
{
    public partial class PageConfig : Page, IDisposable
    {
        public PageConfig()
        {
            InitializeComponent();

            cbInicioJornadaDia.Items.Add("A");
            cbTerminoJornadaDia.Items.Add("A");
            cbTerminoJornadaDia.Items.Add("B");

            
            cbImpresoraUno.Items.Insert(0, new ComboBoxItem() { Content = "NO IMPRIMIR" });
            cbImpresoraDos.Items.Insert(0, new ComboBoxItem() { Content = "NO IMPRIMIR" });
            cbImpresoraTres.Items.Insert(0, new ComboBoxItem() { Content = "NO IMPRIMIR" });
            cbImpresoraCuatro.Items.Insert(0, new ComboBoxItem() { Content = "NO IMPRIMIR" });
            cbImpresoraCinco.Items.Insert(0, new ComboBoxItem() { Content = "NO IMPRIMIR" });
            foreach (var item in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                cbImpresoraUno.Items.Add(item);
                cbImpresoraDos.Items.Add(item);
                cbImpresoraTres.Items.Add(item);
                cbImpresoraCuatro.Items.Add(item);
                cbImpresoraCinco.Items.Add(item);
            }

            List<sector_impresion> listaSectorImpresion = SectorImpresionBLL.ObtenerTodo();

            cbSectorUno.ItemsSource = listaSectorImpresion;
            cbSectorUno.DisplayMemberPath = "nombre";

            cbSectorDos.ItemsSource = listaSectorImpresion;
            cbSectorDos.DisplayMemberPath = "nombre";

            cbSectorTres.ItemsSource = listaSectorImpresion;
            cbSectorTres.DisplayMemberPath = "nombre";

            cbSectorCuatro.ItemsSource = listaSectorImpresion;
            cbSectorCuatro.DisplayMemberPath = "nombre";

            cbSectorCinco.ItemsSource = listaSectorImpresion;
            cbSectorCinco.DisplayMemberPath = "nombre";

            int index = 0;
            listaSectorImpresion.ForEach(x => 
            {
                index++;
                if (index == 1)
                {
                    cbSectorUno.Text = x.nombre;
                    cbImpresoraUno.Text = x.impresora;
                }
                if (index == 2)
                {
                    cbSectorDos.Text = x.nombre;
                    cbImpresoraDos.Text = x.impresora;
                }
                if (index == 3)
                {
                    cbSectorTres.Text = x.nombre;
                    cbImpresoraTres.Text = x.impresora;
                }
                if (index == 4)
                {
                    cbSectorCuatro.Text = x.nombre;
                    cbImpresoraCuatro.Text = x.impresora;
                }
                if (index == 5)
                {
                    cbSectorCinco.Text = x.nombre;
                    cbImpresoraCinco.Text = x.impresora;
                }
            });

            btnGuardarImpresoras.Click += (se, a) =>
            {
                if (cbSectorUno.SelectedItem != null && cbImpresoraUno.Text != "")
                    ImpresoraBLL.EstablecerSectorImpresora((cbSectorUno.SelectedItem as sector_impresion).nombre, cbImpresoraUno.Text);
                if (cbSectorDos.SelectedItem != null && cbImpresoraDos.SelectedValue != null)
                    ImpresoraBLL.EstablecerSectorImpresora((cbSectorDos.SelectedItem as sector_impresion).nombre, cbImpresoraDos.Text);
                if (cbSectorTres.SelectedItem != null && cbImpresoraTres.SelectedValue != null)
                    ImpresoraBLL.EstablecerSectorImpresora((cbSectorTres.SelectedItem as sector_impresion).nombre, cbImpresoraTres.Text);
                if (cbSectorCuatro.SelectedItem != null && cbImpresoraCuatro.SelectedValue != null)
                    ImpresoraBLL.EstablecerSectorImpresora((cbSectorCuatro.SelectedItem as sector_impresion).nombre, cbImpresoraCuatro.Text);
                if (cbSectorCinco.SelectedItem != null && cbImpresoraCinco.SelectedValue != null)
                    ImpresoraBLL.EstablecerSectorImpresora((cbSectorCinco.SelectedItem as sector_impresion).nombre, cbImpresoraCinco.Text);

                new Notification("LISTO");

            };

            btnGuardarNotificaciones.Click += (se, a) =>
            {
                try
                {
                    DatosNegocioBLL.GuardarCorreos(txtCorreo_primario.Text, txtCorreo_secundario.Text);
                }
                catch (Exception ex)
                {
                    PoskException.Make(ex, "ERROR AL GUARDAR SECCION NOTIFICACIONES");
                }
            };

            btnActualizar.Click += (se, a) =>
            {
                InstallUpdateSyncWithInfo();
            };

            Loaded += (se, a) =>
            {
                string modo = DatosNegocioBLL.ObtenerModo();
                datos_negocio datosNegocio = DatosNegocioBLL.ObtenerDatos();
                checkMesasGarzon.IsChecked = datosNegocio.pago_inmediato == true ? false : true;

                txtIVA.Text = $"{DatosNegocioBLL.ObtenerIva()}";
                checkTecladoTactilIntegrado.IsChecked = DatosNegocioBLL.ObtenerConfiguracionTeclado();
                checkModoRestaurant.IsChecked = false;
                checkModoRua.IsChecked = false;
                checkModoKupal.IsChecked = false;
                checkModoNormal.IsChecked = false;
                checkModoSushi.IsChecked = false;
                switch (modo)
                {
                    case "RESTAURANT":
                        checkModoRestaurant.IsChecked = true;
                        break;
                    case "RUA":
                        checkModoRua.IsChecked = true;
                        break;
                    case "KUPAL":
                        checkModoKupal.IsChecked = true;
                        break;
                    case "NORMAL":
                        checkModoNormal.IsChecked = true;
                        break;
                    case "SUSHI":
                        checkModoSushi.IsChecked = true;
                        break;
                    default:
                        checkModoNormal.IsChecked = true;
                        break;
                }
            };

            cbTerminoJornadaDia.SelectionChanged += (se, ev) => { };

            datos_negocio dn = DatosNegocioBLL.GetDatosNegocio();
            if (dn != null)
            {
                txtNombreDelNegocio.Text = dn.nombre;
                txtDescripcionNegocio.Text = dn.mensaje;
                txtDireccionNegocio.Text = dn.direccion;

                txtCorreo_primario.Text = dn.correo_primario;
                txtCorreo_secundario.Text = dn.correo_secundario;

                cbInicioJornadaDia.Text = dn.inicio_jornada_dia;
                cbTerminoJornadaDia.Text = dn.termino_jornada_dia;

                //cbInicioJornadaDia.SelectedItem = dn.inicio_jornada_dia;
                //cbTerminoJornadaDia.SelectedItem = dn.termino_jornada_dia;

                DateTime now = new DateTime();
                timeTerminoJornadaHora.SelectedTime = now + TimeSpan.Parse(dn.termino_jornada_hora.ToString());
                timeInicioJornadaHora.SelectedTime = now + TimeSpan.Parse(dn.inicio_jornada_hora.ToString());
            }
            else
            {
                txtNombreDelNegocio.Text = "";
                txtDescripcionNegocio.Text = "";
                txtDireccionNegocio.Text = "";
                cbTerminoJornadaDia.SelectedItem = null;
                cbInicioJornadaDia.SelectedItem = null;
                timeInicioJornadaHora.SelectedTime = null;
                timeTerminoJornadaHora.SelectedTime = null;
            }


            btnRespaldarBD.Click += (se, ev) =>
            {
                try
                {
                    string constring = "server=localhost;user=root;pwd=MyTempPass.12;database=posk_db;";
                    string file = $"C:\\posk\\db\\dbbackup_{DateTime.Now.Date.Day}-{DateTime.Now.Month}-{DateTime.Now.Year}_{DateTime.Now.Hour}-{DateTime.Now.Minute}-{DateTime.Now.Second}.sql";
                    // string file2 = $@"../../db/dbbackup_{DateTime.Now.Date.Day}-{DateTime.Now.Month}-{DateTime.Now.Year}_{DateTime.Now.Hour}-{DateTime.Now.Minute}-{DateTime.Now.Second}.sql";

                    using (MySqlConnection conn = new MySqlConnection(constring))
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            using (MySqlBackup mb = new MySqlBackup(cmd))
                            {
                                cmd.Connection = conn;
                                conn.Open();
                                mb.ExportToFile(file);
                                // mb.ExportToFile(file2);
                                conn.Close();
                            }
                        }
                    }
                    new Notification("LISTO");
                }
                catch (Exception ex)
                {
                    PoskException.Make(ex, "ERROR AL GUARDAR BASE DE DATOS");
                    throw;
                }
            };

            btnRestaurarBD.Click += (se, ev) =>
            {
                try
                {
                    string constring = "server=localhost;user=root;pwd=MyTempPass.12;database=posk_db;";
                    string file = "C:\\posk\\db\\db.sql";
                    using (MySqlConnection conn = new MySqlConnection(constring))
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            using (MySqlBackup mb = new MySqlBackup(cmd))
                            {
                                cmd.Connection = conn;
                                conn.Open();
                                mb.ImportFromFile(file);
                                conn.Close();
                            }
                        }
                    }
                    new Notification("LISTO");
                }
                catch (Exception ex)
                {
                    PoskException.Make(ex, "ERROR AL RESTAURAR BASE DE DATOS");
                }
            };

            btnBorrarStock.Click += (se, a) => 
            {
                try
                {
                    StockBLL.BorrarTodo();
                    StockmpBLL.BorrarTodo();
                    CompraProductoBLL.BorrarTodo();
                    BodegaBLL.BorrarTodo();
                    CompraBLL.BorrarTodo();

                    new Notification("BORRADO STOCK", "Y SUS RELACIONES");
                }
                catch (Exception ex)
                {
                    PoskException.Make(ex, "ERROR AL BORRAR STOCK Y SUS RELACIONES");
                }
            };

            btnGuardarGlobal.Click += (se, ev) =>
            {
                try
                {
                    if (!String.IsNullOrEmpty(txtIVA.Text))
                        DatosNegocioBLL.SetIvaPct(Convert.ToInt32(txtIVA.Text));

                    string modo = GlobalSettings.ModoEnum.NORMAL.ToString();
                    if (checkModoRestaurant.IsChecked == true)
                        modo = GlobalSettings.ModoEnum.RESTAURANT.ToString();
                    else if (checkModoRua.IsChecked == true)
                        modo = GlobalSettings.ModoEnum.RUA.ToString();
                    else if (checkModoKupal.IsChecked == true)
                        modo = GlobalSettings.ModoEnum.KUPAL.ToString();
                    else if (checkModoNormal.IsChecked == true)
                        modo = GlobalSettings.ModoEnum.NORMAL.ToString();
                    else if (checkModoSushi.IsChecked == true)
                        modo = GlobalSettings.ModoEnum.SUSHI.ToString();
                    DatosNegocioBLL.EstablecerModo(modo);
                    GlobalSettings.Modo = modo;
                    DatosNegocioBLL.GuardarConfiguracionTeclado(checkTecladoTactilIntegrado.IsChecked);
                    GlobalSettings.UsarTecladoTactilIntegrado = checkTecladoTactilIntegrado.IsChecked == true ? true : false;
                    new Notification("GUARDADO");
                }
                catch (Exception ex)
                {
                    PoskException.Make(ex, "Error al guardar global");
                }
            };

            btnGuardarJornada.Click += (se, ev) =>
            {
                DateTime? timeInicio = timeInicioJornadaHora.SelectedTime;
                DateTime? timeTermino = timeTerminoJornadaHora.SelectedTime;
                int horaInicio = timeInicio.Value.Hour;
                int horaTermino = timeTermino.Value.Hour;

                if (cbInicioJornadaDia.SelectedValue.ToString() == cbTerminoJornadaDia.SelectedValue.ToString() && horaInicio > horaTermino)
                {
                    MessageBox.Show("Si la jornada es de un día, la hora de inicio no puede ser después de la hora de término, corrige eso en Sección Jornada");
                }
                else if (cbInicioJornadaDia.SelectedValue.ToString() == cbTerminoJornadaDia.SelectedValue.ToString() && horaInicio == horaTermino)
                {
                    MessageBox.Show("Si la jornada es de un día, la hora de inicio no puede ser igual a la hora de término, corrige eso en Sección Jornada");
                }
                else
                {
                    DateTime? dtInicio = timeInicioJornadaHora.SelectedTime;
                    DateTime? dtTermino = timeTerminoJornadaHora.SelectedTime;
                    DatosNegocioBLL.SetJornada(cbInicioJornadaDia.SelectedValue.ToString(), cbTerminoJornadaDia.SelectedValue.ToString(), dtInicio.Value.TimeOfDay, dtTermino.Value.TimeOfDay);
                }
            };

            btnGuardarDescripcionNegocio.Click += (se, ev) =>
            {
                DatosNegocioBLL.SetDescripcionNegocio(txtNombreDelNegocio.Text, "", txtDireccionNegocio.Text, txtImagenPortada.Text);
            };
        }


        private void InstallUpdateSyncWithInfo()
        {
            UpdateCheckInfo info = null;

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;

                try
                {
                    info = ad.CheckForDetailedUpdate();

                }
                catch (DeploymentDownloadException dde)
                {
                    MessageBox.Show("The new version of the application cannot be downloaded at this time. \n\nPlease check your network connection, or try again later. Error: " + dde.Message);
                    return;
                }
                catch (InvalidDeploymentException ide)
                {
                    //MessageBox.Show("Cannot check for a new version of the application. The ClickOnce deployment is corrupt. Please redeploy the application and try again. Error: " + ide.Message);
                    MessageBox.Show("Error: " + ide.Message);
                    return;
                }
                catch (InvalidOperationException ioe)
                {
                    MessageBox.Show("This application cannot be updated. It is likely not a ClickOnce application. Error: " + ioe.Message);
                    return;
                }

                if (info.UpdateAvailable)
                {
                    Boolean doUpdate = true;

                    if (!info.IsUpdateRequired)
                    {
                        DialogResult dr = MessageBox.Show("An update is available. Would you like to update the application now?", "Update Available", MessageBoxButtons.OKCancel);
                        if (!(DialogResult.OK == dr))
                        {
                            doUpdate = false;
                        }
                    }
                    else
                    {
                        // Display a message that the app MUST reboot. Display the minimum required version.
                        MessageBox.Show("This application has detected a mandatory update from your current " +
                            "version to version " + info.MinimumRequiredVersion.ToString() +
                            ". The application will now install the update and restart.",
                            "Update Available", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }

                    if (doUpdate)
                    {
                        try
                        {
                            ad.Update();
                            MessageBox.Show("The application has been upgraded, and will now restart.");
                            Application.Restart();
                        }
                        catch (DeploymentDownloadException dde)
                        {
                            MessageBox.Show("Cannot install the latest version of the application. \n\nPlease check your network connection, or try again later. Error: " + dde);
                            return;
                        }
                    }
                }
            }
        }

        void IDisposable.Dispose()
        {
        }
    }
}
