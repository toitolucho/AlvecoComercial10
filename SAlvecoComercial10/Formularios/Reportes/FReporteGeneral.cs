using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;

namespace SAlvecoComercial10.Formularios.Reportes
{
    public partial class FReporteGeneral : Form
    {
        protected Button btnCerrar;
        protected ReportClass fuenteReporteGeneral{get;set;}
        protected DataSet DSReporteGeneral { get; set; }
        protected CrystalDecisions.Windows.Forms.CrystalReportViewer CRVReporteGeneralAcceso {
            get { return CRVReporteGeneral; }
        }
        public FReporteGeneral()
        {
            InitializeComponent();
            btnCerrar = new Button();
            btnCerrar.Click += new EventHandler(btnCerrar_Click);
            this.CancelButton = btnCerrar;
            DSReporteGeneral = new DataSet();
        }

        void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void configurarReporteSencillo()
        {
            CRVReporteGeneral.ShowGroupTreeButton = CRVReporteGeneral.ShowParameterPanelButton = CRVReporteGeneral.ShowRefreshButton = false;
            CRVReporteGeneral.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
        }

        public void configurarReporteAgrupacion(bool mostrarPanelAgrupacion)
        {
            CRVReporteGeneral.ShowGroupTreeButton = true;
            CRVReporteGeneral.ShowParameterPanelButton = CRVReporteGeneral.ShowRefreshButton = false;
            CRVReporteGeneral.ToolPanelView = mostrarPanelAgrupacion ? CrystalDecisions.Windows.Forms.ToolPanelViewType.GroupTree :
                CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
        }

        private void FReporteGeneral_Load(object sender, EventArgs e)
        {
            
            CRVReporteGeneral.ReportSource = fuenteReporteGeneral;
        }
    }
}
