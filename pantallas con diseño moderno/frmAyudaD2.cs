using Microsoft.Web.WebView2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pantallas_con_diseño_moderno
{
    public partial class frmAyudaD2 : Form
    {
        public frmAyudaD2()
        {
            InitializeComponent();
            this.Load += new EventHandler(frmAyudaD2_Load);
        }

        private async void MostrarPaginaWebEnPanel()
        {
            WebView2 navegadorEdge = new WebView2();
            navegadorEdge.Dock = DockStyle.Fill;
            await navegadorEdge.EnsureCoreWebView2Async();
            navegadorEdge.CoreWebView2.Navigate("https://www.msn.com/es-co");
            WVnavegador.Controls.Add(navegadorEdge);
        }

        private void frmAyudaD2_Load(object sender, EventArgs e)
        {
            MostrarPaginaWebEnPanel();
        }
    }
}
