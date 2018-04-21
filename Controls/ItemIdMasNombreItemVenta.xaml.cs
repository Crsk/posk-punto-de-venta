﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace posk.Controls
{
    public partial class ItemIdMasNombreItemVenta : UserControl
    {
        public string Nombre
        {
            get { return $"{lbNombre.Content}"; }
            set { lbNombre.Content = value; }
        }

        public int? ID { get; set; }

        public ItemIdMasNombreItemVenta()
        {
            InitializeComponent();
        }
    }
}
