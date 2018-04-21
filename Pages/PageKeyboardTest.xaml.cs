using System;
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
using WindowsInput;
using WindowsInput.Native;

namespace posk.Pages
{
    public partial class PageKeyboardTest : Page
    {
        public PageKeyboardTest()
        {
            InitializeComponent();
            KeyboardInit();
        }

        #region Keyboard
        TextBox lastFocusControl = null;
        int lastFocusSelectionStart = 0;
        bool bKeyboardIsUpper = false;
        bool bKeyboardIsMore = false;
        //bool bKeyboardIsLetterClicked = false; // used into lambda expression
        private void KeyboardInit()
        {
            List<TextBox> listaTextBox = new List<TextBox>();
            listaTextBox.Add(txtUser);
            foreach (TextBox tb in listaTextBox)
            {
                tb.LostFocus += (se, ev) => { lastFocusSelectionStart = lastFocusControl.SelectionStart; };
                tb.GotMouseCapture += (se, ev) => { lastFocusControl = tb; expTeclado.IsExpanded = true; };
            }
            btnTeclado.Click += (se, ev) => { expTeclado.IsExpanded ^= true; };
            btnEnter.Click += (se, ev) => { KeyboardWrite("\n"); };
            btnSpace.Click += (se, ev) => { KeyboardWrite(" "); };
            btnDelete.Click += (se, ev) =>
            {
                if (lastFocusSelectionStart > 0)
                {
                    lastFocusControl.Text = lastFocusControl.Text.Remove(lastFocusSelectionStart - 1, 1);
                    lastFocusControl.SelectionStart = lastFocusSelectionStart - 1;
                }
                lastFocusControl.Focus();
            };
            btnShift.Click += (se, ev) =>
            {
                if (bKeyboardIsUpper)
                    KeyboardToLower();
                else
                    KeyboardToUpper();
            };
            btnSiguiente.Click += (se, ev) =>
            {
            };
            btnLeft.Click += (se, ev) =>
            {
                if (lastFocusControl != null)
                {
                    lastFocusControl.Focus();
                    if (lastFocusControl.SelectionStart != 0)
                        lastFocusControl.SelectionStart = lastFocusSelectionStart - 1;
                }
            };
            btnRight.Click += (se, ev) =>
            {
                if (lastFocusControl != null)
                {
                    lastFocusControl.Focus();
                    lastFocusControl.SelectionStart = lastFocusSelectionStart + 1;
                }
            };
            btnShift.Focusable = false;
            btnSiguiente.Focusable = false;
            btnMore.Focusable = false;
            btnMore.Click += (se, ev) => { BtnMorePressed(); };
            foreach (StackPanel sp in spTeclado.Children)
            {
                foreach (Button item in sp.Children)
                {
                    if (item != btnDelete && item != btnShift && item != btnEnter && item != btnMore && item != btnSpace && item != btnSiguiente && item != btnLeft && item != btnRight)
                    {
                        item.Click += (se, ev) =>
                        {
                            KeyboardWrite(item.Content.ToString());
                            KeyboardToLower();
                            //bKeyboardIsLetterClicked = true;
                        };
                    }
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
                    if (item != btnDelete && item != btnShift && item != btnEnter && item != btnMore && item != btnSpace && item != btnSiguiente && item != btnLeft && item != btnRight)
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
                    if (item != btnDelete && item != btnShift && item != btnEnter && item != btnMore && item != btnSpace && item != btnSiguiente && item != btnLeft && item != btnRight)
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
            if (lastFocusControl != null)
            {
                lastFocusControl.Text = lastFocusControl.Text.Insert(lastFocusSelectionStart, letter);
                lastFocusControl.Focus();
                lastFocusControl.SelectionStart = lastFocusSelectionStart + 1;
            }
        }
        private void KeyboardRecoverFocus() // lo uso desde lambda
        {
            if (lastFocusControl != null)
            {
                lastFocusControl.Focus();
                lastFocusControl.SelectionStart = lastFocusSelectionStart;
            }
        }
        #endregion Keyboard
    }
}
