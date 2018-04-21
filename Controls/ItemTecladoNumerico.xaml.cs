using posk.Globals;
using System.Collections.Generic;
using System.Windows.Controls;

namespace posk.Controls
{
    public partial class ItemTecladoNumerico : UserControl
    {
        // al instanciar, entregar una lista de los textbox que abran el teclado
        public List<TextBox> ListaItemsTeclado { get; set; }
        public Expander ExpTecladoNumerico { get; set; }
        public Expander ExpBilletes { get; set; }

        public ItemTecladoNumerico(List<TextBox> listaItemsFoco)
        {
            InitializeComponent();
            ListaItemsTeclado = listaItemsFoco;
            ExpTecladoNumerico = expTecladoNum;
            ExpBilletes = expBilletes;
            KeyboardInit_numeric();
        }

        public static TextBox lastFocusControl = new TextBox();
        int lastFocusSelectionStart = 0;

        //private void HabilitarDeshabilitarBilletes()
        //{
        //    if (VentaActual_total > 1000)
        //        btn1000.Opacity = 0.2;
        //    else
        //        btn1000.Opacity = 1;
        //    if (VentaActual_total > 2000)
        //        btn2000.Opacity = 0.2;
        //    else
        //        btn2000.Opacity = 1;
        //    if (VentaActual_total > 5000)
        //        btn5000.Opacity = 0.2;
        //    else
        //        btn5000.Opacity = 1;
        //    if (VentaActual_total > 10000)
        //        btn10000.Opacity = 0.2;
        //    else
        //        btn10000.Opacity = 1;
        //    if (VentaActual_total > 20000)
        //        btn20000.Opacity = 0.2;
        //    else
        //        btn20000.Opacity = 1;
        //}

        private void KeyboardInit_numeric()
        {
            foreach (TextBox tb in ListaItemsTeclado)
            {

                tb.LostFocus += (se, ev) => { lastFocusControl = null; };

                tb.GotMouseCapture += (se, ev) =>
                {
                    tb.Focus();
                    lastFocusControl = tb;
                    expTecladoNum.IsExpanded = true;
                    expBilletes.IsExpanded = false;
                    expBilletes.Width = 0;
                    lastFocusSelectionStart = tb.SelectionStart;

                    if (tb.Name.Equals("txtPagaCon"))
                    {
                        expTecladoNum.IsExpanded = true;
                        expBilletes.IsExpanded = true;
                        expBilletes.Width = 444;
                        btn1000.Click += (se2, e2) => { tb.Text = "1000"; expTecladoNum.IsExpanded = false; expBilletes.IsExpanded = false; };
                        btn2000.Click += (se2, e2) => { tb.Text = "2000"; expTecladoNum.IsExpanded = false; expBilletes.IsExpanded = false; };
                        btn5000.Click += (se2, e2) => { tb.Text = "5000"; expTecladoNum.IsExpanded = false; expBilletes.IsExpanded = false; };
                        btn10000.Click += (se2, e2) => { tb.Text = "10000"; expTecladoNum.IsExpanded = false; expBilletes.IsExpanded = false; };
                        btn20000.Click += (se2, e2) => { tb.Text = "20000"; expTecladoNum.IsExpanded = false; expBilletes.IsExpanded = false; };
                    }
                };
                tb.GotFocus += (se, ev) =>
                {
                    lastFocusControl = tb;
                    expTecladoNum.IsExpanded = true;
                    expBilletes.IsExpanded = false;
                    expBilletes.Width = 0;
                    lastFocusSelectionStart = tb.SelectionStart;
                    tb.SelectionStart = lastFocusSelectionStart;

                    if (tb.Name.Equals("txtPagaCon"))
                    {
                        expTecladoNum.IsExpanded = true;
                        expBilletes.IsExpanded = true;
                        expBilletes.Width = 444;
                        btn1000.Click += (se2, e2) => { tb.Text = "1000"; expTecladoNum.IsExpanded = false; expBilletes.IsExpanded = false; };
                        btn2000.Click += (se2, e2) => { tb.Text = "2000"; expTecladoNum.IsExpanded = false; expBilletes.IsExpanded = false; };
                        btn5000.Click += (se2, e2) => { tb.Text = "5000"; expTecladoNum.IsExpanded = false; expBilletes.IsExpanded = false; };
                        btn10000.Click += (se2, e2) => { tb.Text = "10000"; expTecladoNum.IsExpanded = false; expBilletes.IsExpanded = false; };
                        btn20000.Click += (se2, e2) => { tb.Text = "20000"; expTecladoNum.IsExpanded = false; expBilletes.IsExpanded = false; };
                    }
                };


            }

            btnCerrarTecladoNumerico.Click += (se, ev) => { expTecladoNum.IsExpanded = false; expBilletes.IsExpanded = false; };

            //txtDescuentoGlobalPct.GotFocus += (se, ev) =>
            //{
            //    expTecladoNum.IsExpanded = true;
            //    expBilletes.IsExpanded = false;
            //};
            //txtDescuentoGlobalPesos.GotFocus += (se, ev) =>
            //{
            //    expTecladoNum.IsExpanded = true;
            //    expBilletes.IsExpanded = false;
            //};

            //txtDescuentoGlobalPct.GotMouseCapture += (se, ev) =>
            //{
            //    expTecladoNum.IsExpanded = true;
            //    expBilletes.IsExpanded = false;
            //};
            //txtDescuentoGlobalPesos.GotMouseCapture += (se, ev) =>
            //{
            //    expTecladoNum.IsExpanded = true;
            //    expBilletes.IsExpanded = false;
            //};



            foreach (Button item in spTecladoNumerico.Children)
            {
                if (item != btnDelete_tn && item != btnCerrarTecladoNumerico)
                {
                    item.Click += (se, ev) =>
                    {
                        KeyboardWrite(item.Content.ToString());
                    };
                }
                item.Focusable = false;
            }
            btnCerrarTecladoNumerico.Focusable = true;

            btnDelete_tn.Click += (se, ev) =>
            {
                if (lastFocusSelectionStart > 0 && lastFocusControl != null)
                {
                    lastFocusControl.Text = lastFocusControl.Text.Remove(lastFocusSelectionStart - 1, 1);
                    lastFocusControl.SelectionStart = --lastFocusSelectionStart;
                }
            };
        }


        private void KeyboardWrite(string letter)
        {
            try
            {
                if (lastFocusControl != null)
                {
                    lastFocusControl.Text = lastFocusControl.Text.Insert(lastFocusSelectionStart, letter);
                    //lastFocusControl.Focus();
                    lastFocusControl.SelectionStart = ++lastFocusSelectionStart;

                    //lastFocusControl.SelectionStart = lastFocusSelectionStart++;
                }
            }
            catch (System.Exception ex)
            {
                PoskException.Make(ex, "ERROR AL ESCRIBIR EN TECLADO NUMERICO"); // sucede al modificar texto desde programación antes de usar el teclado
            }
        }
    }
}
