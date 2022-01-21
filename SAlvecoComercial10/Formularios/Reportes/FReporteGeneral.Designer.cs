namespace SAlvecoComercial10.Formularios.Reportes
{
    partial class FReporteGeneral
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
            this.CRVReporteGeneral = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();
            // 
            // CRVReporteGeneral
            // 
            this.CRVReporteGeneral.ActiveViewIndex = -1;
            this.CRVReporteGeneral.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CRVReporteGeneral.Cursor = System.Windows.Forms.Cursors.Default;
            this.CRVReporteGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CRVReporteGeneral.Location = new System.Drawing.Point(0, 0);
            this.CRVReporteGeneral.Name = "CRVReporteGeneral";
            this.CRVReporteGeneral.ShowGroupTreeButton = false;
            this.CRVReporteGeneral.ShowParameterPanelButton = false;
            this.CRVReporteGeneral.ShowRefreshButton = false;
            this.CRVReporteGeneral.Size = new System.Drawing.Size(579, 420);
            this.CRVReporteGeneral.TabIndex = 0;
            this.CRVReporteGeneral.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            // 
            // FReporteGeneral
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(579, 420);
            this.Controls.Add(this.CRVReporteGeneral);
            this.Name = "FReporteGeneral";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reporte General";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FReporteGeneral_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer CRVReporteGeneral;
    }
}