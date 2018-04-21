using posk.Globals;
using System.Collections.Generic;
using System.Windows.Controls;

namespace posk.Controls
{
    public partial class ItemTeclado : UserControl
    {
        // al instanciar, entregar una lista de los textbox que abran el teclado
        public List<TextBox> ListaItemsTeclado { get; set; }
        public Expander ExpTeclado { get; set; }

        public ItemTeclado(List<TextBox> listaItemsTeclado)
        {
            InitializeComponent();
            ListaItemsTeclado = listaItemsTeclado;
            ExpTeclado = expTeclado;

            Loaded += (se, a) =>
            {
                if (GlobalSettings.UsarTecladoTactilIntegrado == true)
                    KeyboardInit();
            };
        }

        #region Keyboard

        public TextBox lastFocusControl = new TextBox();

        int lastFocusSelectionStart = 0;
        bool bKeyboardIsUpper = false;
        bool bKeyboardIsMore = false;
        //bool bKeyboardIsLetterClicked = false; // used into lambda expression
        private void KeyboardInit()
        {
            //List<TextBox> listaTextBox = new List<TextBox>();
            //listaTextBox.Add(txtBuscar);
            //listaTextBox.Add(txtClient_rut);
            //listaTextBox.Add(txtClient_name);
            //listaTextBox.Add(txtClient_contact);
            //listaTextBox.Add(txtClient_comment);
            //listaTextBox.Add(txtEscogerClienteEntregar);

            btnCerrarTeclado.Click += (se, ev) => { expTeclado.IsExpanded = false; };

            foreach (TextBox tb in ListaItemsTeclado)
            {
                //tb.LostFocus += (se, ev) => { lastFocusControl = null; };
                tb.GotMouseCapture += (se, ev) =>
                {
                    tb.Focus();
                    lastFocusControl = tb;
                    expTeclado.IsExpanded = true;
                    lastFocusSelectionStart = tb.SelectionStart;
                    tb.SelectionStart = lastFocusSelectionStart;
                };
                tb.GotFocus += (se, ev) =>
                {
                    lastFocusControl = tb;
                    expTeclado.IsExpanded = true;
                    lastFocusSelectionStart = tb.SelectionStart;
                    tb.SelectionStart = lastFocusSelectionStart;
                };
            }
            //btnTeclado.Click += (se, ev) => { expTeclado.IsExpanded ^= true; };
            //btnEnter.Click += (se, ev) => { if (lastFocusControl != null) KeyboardWrite("\n"); };
            btnSpace.Click += (se, ev) => { if (lastFocusControl != null) KeyboardWrite(" "); };
            btnDelete.Click += (se, ev) =>
            {
                try
                {
                    if (lastFocusSelectionStart > 0 && lastFocusControl != null)
                    {
                        lastFocusControl.Text = lastFocusControl.Text.Remove(lastFocusSelectionStart - 1, 1);
                        lastFocusControl.SelectionStart = --lastFocusSelectionStart;
                    }
                    //lastFocusControl.Focus();
                }
                catch (System.Exception)
                {

                }
            };

            btnDelete.MouseDoubleClick += (se, a) =>
            {
                try
                {
                    if (lastFocusSelectionStart > 0 && lastFocusControl != null)
                    {
                        lastFocusControl.Text = "";
                        lastFocusControl.SelectionStart = 0;
                        lastFocusSelectionStart = 0;
                    }
                }
                catch (System.Exception)
                {

                }
            };

            btnShift.Click += (se, ev) =>
            {
                if (bKeyboardIsUpper)
                    KeyboardToLower();
                else
                    KeyboardToUpper();
            };


            //btnLeft.Click += (se, ev) =>
            //{
            //    if (lastFocusControl != null)
            //    {
            //        //lastFocusControl.Focus();
            //        if (lastFocusControl.SelectionStart != 0)
            //            lastFocusControl.SelectionStart = --lastFocusSelectionStart;
            //    }
            //};
            //btnRight.Click += (se, ev) =>
            //{
            //    if (lastFocusControl != null)
            //    {
            //        //lastFocusControl.Focus();
            //        if (lastFocusControl.SelectionStart < lastFocusControl.Text.Length)
            //            lastFocusControl.SelectionStart = ++lastFocusSelectionStart;
            //    }
            //};
            btnShift.Focusable = false;
            btnCerrarTeclado.Focusable = false;
            btnMore.Focusable = false;
            btnQ.Focusable = false;
            btnW.Focusable = false;
            btnMore.Click += (se, ev) => { BtnMorePressed(); };
            foreach (StackPanel sp in spTeclado.Children)
            {
                foreach (Button item in sp.Children)
                {
                    if (item != btnDelete && item != btnShift && /*item != btnEnter &&*/ item != btnMore && item != btnSpace && item != btnCerrarTeclado /*&& item != btnLeft && item != btnRight*/)
                    {
                        item.Click += (se, ev) =>
                        {
                            KeyboardWrite(item.Content.ToString());
                            KeyboardToLower();
                            //bKeyboardIsLetterClicked = true;
                            //lastFocusSelectionStart = lastFocusControl.SelectionStart;
                        };
                    }
                    item.Focusable = false;
                }
            }
        }


        private void BtnMorePressed()
        {
            if (!bKeyboardIsMore)
            {
                btnQ.Content = "1";
                btnW.Content = "2";
                btnE.Content = "3";
                btnR.Content = "4";
                btnT.Content = "5";
                btnY.Content = "6";
                btnU.Content = "7";
                btnI.Content = "8";
                btnO.Content = "9";
                btnP.Content = "0";

                btnA.Content = "+";
                btnS.Content = "@";
                btnD.Content = "¿";
                btnF.Content = "?";
                btnG.Content = "¡";
                btnH.Content = "!";
                btnJ.Content = "(";
                btnK.Content = ")";
                btnL.Content = "/";
                btnN2.Content = "$";

                btnZ.Content = ";";
                btnX.Content = ",";
                btnC.Content = ":";
                btnV.Content = ".";
                btnB.Content = "-";
                btnN.Content = "_";
                btnM.Content = "\"";

                bKeyboardIsMore = true;
            }
            else
            {
                KeyboardRestart();
                bKeyboardIsMore = false;
            }
        }
        private void KeyboardRestart()
        {
            btnQ.Content = "q";
            btnW.Content = "w";
            btnE.Content = "e";
            btnR.Content = "r";
            btnT.Content = "t";
            btnY.Content = "y";
            btnU.Content = "u";
            btnI.Content = "i";
            btnO.Content = "o";
            btnP.Content = "p";

            btnA.Content = "a";
            btnS.Content = "s";
            btnD.Content = "d";
            btnF.Content = "f";
            btnG.Content = "g";
            btnH.Content = "h";
            btnJ.Content = "j";
            btnK.Content = "k";
            btnL.Content = "l";
            btnN2.Content = "ñ";

            btnZ.Content = "z";
            btnX.Content = "x";
            btnC.Content = "c";
            btnV.Content = "v";
            btnB.Content = "b";
            btnN.Content = "n";
            btnM.Content = "m";
            bKeyboardIsUpper = false;
            bKeyboardIsMore = false;
        }
        private void KeyboardToLower()
        {
            foreach (StackPanel sp in spTeclado.Children)
            {
                foreach (Button item in sp.Children)
                {
                    if (item != btnDelete && item != btnShift /*&& item != btnEnter*/ && item != btnMore && item != btnSpace && item != btnCerrarTeclado /*&& item != btnLeft && item != btnRight*/)
                    {
                        item.Content = item.Content.ToString().ToLower();
                    }
                }
                bKeyboardIsUpper = false;
            }
        }
        private void KeyboardToUpper()
        {
            foreach (StackPanel sp in spTeclado.Children)
            {
                foreach (Button item in sp.Children)
                {
                    if (item != btnDelete && item != btnShift /*&& item != btnEnter*/ && item != btnMore && item != btnSpace && item != btnCerrarTeclado /*&& item != btnLeft && item != btnRight*/)
                    {
                        item.Content = item.Content.ToString().ToUpper();
                    }
                }
                bKeyboardIsUpper = true;
                //bKeyboardIsLetterClicked = false;
            }
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
            catch (System.Exception)
            {
            }
        }
        //private void KeyboardRecoverFocus() // lo uso desde lambda
        //{
        //    if (lastFocusControl != null)
        //    {
        //        lastFocusControl.Focus();
        //        lastFocusControl.SelectionStart = lastFocusSelectionStart;
        //    }
        //}
        #endregion Keyboard


    }
}
