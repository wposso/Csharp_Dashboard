using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pantallas_con_diseño_moderno
{
    public partial class frmPrincipal : Form
    {


        //Nuevo
        private frmClientesD2 frmClientesD2;
        private frmProductosD2 frmProductosD2;
        private frmCategoriasD2 frmCategoriasD2;
        private frmFacturasD2 frmFacturasD2;
        private frmInformesD2 frmInformesD2;
        private frmEmpleadosD2 frmEmpleadosD2;
        private frmRolesD2 frmRolesD2;
        private frmAyudaD2 frmAyudaD2;
        private frmSeguridadD2 frmSeguridadD2;

        public frmPrincipal()
        {
            InitializeComponent();
            InitializeScreens();


            BtnTablas.ContextMenuStrip = SubOpcTablas;

            // Si el ContextMenuStrip no se ha añadido a BtnTablas, puedes mostrarlo manualmente
            BtnTablas.Click += (s, e) =>
            {
                // Mostrar el ContextMenuStrip justo debajo del botón
                SubOpcTablas.Show(BtnTablas, new Point(0, BtnTablas.Height));
            };

            //ContextMenuStrip de Opción facturación
            BtnFacturacion.ContextMenuStrip = SubOpcFacturacion;


            BtnFacturacion.Click += (s, e) =>
            {

                SubOpcFacturacion.Show(BtnFacturacion, new Point(0, BtnFacturacion.Height));
            };

            //ContextMenuStrip de Opción seguridad
            BtnSeguridad.ContextMenuStrip = SubOpcSeguridad;


            BtnSeguridad.Click += (s, e) =>
            {

                SubOpcSeguridad.Show(BtnSeguridad, new Point(0, BtnSeguridad.Height));
            };

            //ContextMenuStrip de Opción Acerca De
            BtnAcercaDe.ContextMenuStrip = SubOpcAcercaDe;


            BtnAcercaDe.Click += (s, e) =>
            {

                SubOpcAcercaDe.Show(BtnAcercaDe, new Point(0, BtnAcercaDe.Height));
            };

        }

        private void InitializeScreens()
        {

            frmClientesD2 = new frmClientesD2();
            frmProductosD2 = new frmProductosD2();
            frmCategoriasD2 = new frmCategoriasD2();
            frmFacturasD2 = new frmFacturasD2();
            frmInformesD2 = new frmInformesD2();
            frmEmpleadosD2 = new frmEmpleadosD2();
            frmAyudaD2 = new frmAyudaD2();
            frmRolesD2 = new frmRolesD2();
            frmSeguridadD2 = new frmSeguridadD2();
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit(); //cierra la aplicación
        }
        public void pnlContainer(object _container)
        {
            this.pnlContenedor.Controls.Clear();
            Form? P = _container as Form;
            P.TopLevel = false;
            P.Dock = DockStyle.Fill;
            this.pnlContenedor.Controls.Add(P);
            this.pnlContenedor.Tag = P;
            P.Show();
        }

        public void BtnTablas_Click(object sender, EventArgs e)
        {
            //pnlContainer(FrmClientes);
        }

        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContainer(frmClientesD2);

        }

        private void productosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContainer(frmProductosD2);
        }

        private void categoríasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContainer(frmCategoriasD2);
        }

        private void facturasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContainer(frmFacturasD2);
        }

        private void informesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContainer(frmInformesD2);
        }

        private void empleadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContainer(frmEmpleadosD2);
        }

        private void rolesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContainer(frmRolesD2);
        }

        private void ayudaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContainer(frmAyudaD2);
        }

        private void seguridadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContainer(frmSeguridadD2);
        }
    }
}



