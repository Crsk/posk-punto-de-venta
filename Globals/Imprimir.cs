using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace posk.Globals
{
    public static class Imprimir
    {
        public static void PrintText(string text, string impresora, int fontSize = 13)
        {
            try
            {
                text += "\x1B" + "d" + "\x03";
                text += "\x1B" + "m";
                var printDlg = new PrintDialog();
                var doc = new FlowDocument(new Paragraph(new Run(text)));
                doc.PagePadding = new Thickness(10);
                doc.FontFamily = new System.Windows.Media.FontFamily("Consolas");
                doc.FontSize = fontSize;
                doc.TextAlignment = TextAlignment.Center;

                printDlg.PrintQueue = FindPrinter(impresora);
                if (printDlg.PrintQueue.IsOutOfPaper == true)
                    MessageBox.Show("Impresora sin papel");
                if (printDlg.PrintQueue.HasPaperProblem == true)
                    MessageBox.Show("Impresora en problemas");
                if (printDlg.PrintQueue.IsPaperJammed == true)
                    MessageBox.Show("Impresora con papel atascado");

                printDlg.PrintDocument((doc as IDocumentPaginatorSource)?.DocumentPaginator, "Posk");
            }
            catch
            {
                MessageBox.Show("Test Message");
            }
        }


        public static PrintQueue FindPrinter(string printerName)
        {
            var printers = new PrintServer().GetPrintQueues();
            foreach (var printer in printers)
            {
                if (printer.FullName == printerName)
                {
                    return printer;
                }
            }
            return LocalPrintServer.GetDefaultPrintQueue();
        }
    }
}