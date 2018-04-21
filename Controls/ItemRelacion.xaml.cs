using posk.BLL;
using posk.Models;
using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace posk.Controls
{
    public partial class ItemRelacion : UserControl
    {
        public int ID { get; set; }
        public string Nombre { get; set; }

        public event EventHandler AlCambiarEstadoSeleccionable;

        private SolidColorBrush colorAzul;
        private SolidColorBrush colorNeutro;



        private bool? seleccionable;
        public bool? Seleccionable
        {
            get { return seleccionable; }
            set
            {
                seleccionable = value;
                ConfigurarSeleccion();
            }
        }

        private categoria categoria;

        public categoria Categoria
        {
            set
            {
                categoria = value;
            }
        }

        private sectore sector;

        public sectore Sector
        {
            set
            {
                sector = value;
            }
        }

        public bool Eliminable { get; set; }

        /// <summary>
        /// seleccionable: el la seccion buscar por categorias, el item no se expande sino que busca directamente
        /// si tiene Categoria, no incluir seleccionable ya que esta propiedad va incluida dentro de Categoria
        /// </summary>
        public ItemRelacion()
        {
            colorAzul = new SolidColorBrush(Color.FromRgb(8, 39, 133));
            colorNeutro = new SolidColorBrush(Color.FromRgb(4, 4, 4));

            InitializeComponent();
            spItem.Children.Remove(btnGuardar);
            spItem.Children.Remove(btnEditar);
            spItem.Children.Remove(btnAgregar);
            Loaded += (se, a) =>
            {
                txtNombre.IsReadOnly = true;
                txtNombre.Text = Nombre;

                if (categoria != null)
                {
                    Seleccionable = categoria.seleccionable;
                    ConfigurarSeleccion();
                }
            };

            btnSeleccionable.Click += (se, a) =>
            {
                Seleccionable ^= true;

                if (categoria != null)
                    CategoriaBLL.EstablecerSeleccionable(categoria.id, Seleccionable);
                if (sector != null)
                    SectorBLL.EstablecerSeleccionable(sector.id, (bool)Seleccionable);

                ConfigurarSeleccion();
                AlCambiarEstadoSeleccionable(this, null);
            };
        }

        private void ConfigurarSeleccion()
        {
            if (categoria != null)
            {
                if (Seleccionable == true)
                {
                    spItem.Children.Clear();
                    spItem.Children.Add(btnSeleccionable);
                    if (Eliminable)
                        spItem.Children.Add(btnEliminar);
                    spItem.Children.Add(borderItem);
                    btnSeleccionable.Foreground = colorAzul;
                }
                else
                {
                    spItem.Children.Clear();
                    spItem.Children.Add(btnSeleccionable);
                    if (Eliminable)
                        spItem.Children.Add(btnEliminar);
                    spItem.Children.Add(borderItem);
                    spItem.Children.Add(btnVer);
                    btnSeleccionable.Foreground = colorNeutro;
                }
            }
            else
            {
                if (Seleccionable == true)
                {
                    spItem.Children.Clear();
                    spItem.Children.Add(btnSeleccionable);
                    if (Eliminable)
                        spItem.Children.Add(btnEliminar);
                    spItem.Children.Add(borderItem);
                    btnSeleccionable.Foreground = colorAzul;
                }
                else
                {
                    spItem.Children.Clear();
                    spItem.Children.Add(btnSeleccionable);
                    if (Eliminable)
                        spItem.Children.Add(btnEliminar);
                    spItem.Children.Add(borderItem);
                    spItem.Children.Add(btnVer);
                    btnSeleccionable.Foreground = colorNeutro;
                }
            }
        }
    }
}

