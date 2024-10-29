namespace pantallas_con_diseño_moderno
{
    partial class frmAyudaD2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            WVnavegador = new Microsoft.Web.WebView2.WinForms.WebView2();
            ((System.ComponentModel.ISupportInitialize)WVnavegador).BeginInit();
            SuspendLayout();
            // 
            // WVnavegador
            // 
            WVnavegador.AllowExternalDrop = true;
            WVnavegador.CreationProperties = null;
            WVnavegador.DefaultBackgroundColor = Color.White;
            WVnavegador.Location = new Point(12, 12);
            WVnavegador.Name = "WVnavegador";
            WVnavegador.Size = new Size(1174, 760);
            WVnavegador.TabIndex = 0;
            WVnavegador.ZoomFactor = 1D;
            // 
            // frmAyudaD2
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1198, 801);
            Controls.Add(WVnavegador);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmAyudaD2";
            Text = "frmAyudaD2";
            Load += frmAyudaD2_Load;
            ((System.ComponentModel.ISupportInitialize)WVnavegador).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 WVnavegador;
    }
}