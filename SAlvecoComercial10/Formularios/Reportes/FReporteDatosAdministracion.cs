using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.Shared;
using SAlvecoComercial10.Formularios.ReportesCR;

namespace SAlvecoComercial10.Formularios.Reportes
{
    public partial class FReporteDatosAdministracion : FReporteGeneral
    {
        public FReporteDatosAdministracion()
        {
            InitializeComponent();
        }

        public void cargarParametros(DateTime FechaInicio, DateTime FechaFin)
        {
            ParameterDiscreteValue crtParamDiscreteValue;
            ParameterField crtParamField;
            ParameterFields crtParamFields;

            //------------------Fecha Inicio
            crtParamDiscreteValue = new ParameterDiscreteValue();
            crtParamField = new ParameterField();
            crtParamFields = new ParameterFields();
            crtParamDiscreteValue.Value = FechaInicio;
            crtParamField.ParameterFieldName = "FechaInicio";
            crtParamField.CurrentValues.Add(crtParamDiscreteValue);
            crtParamFields.Add(crtParamField);

            //------------------Fecha Fin
            crtParamDiscreteValue = new ParameterDiscreteValue();
            crtParamField = new ParameterField();
            crtParamDiscreteValue.Value = FechaFin;
            crtParamField.ParameterFieldName = "FechaFin";
            crtParamField.CurrentValues.Add(crtParamDiscreteValue);
            crtParamFields.Add(crtParamField);


            this.CRVReporteGeneralAcceso.ParameterFieldInfo = crtParamFields;
        }

        public void ListarProductosPorTipo(DataTable DTListarProductosPorTipo)
        {
            DSReporteGeneral.Tables.Clear();
            DSReporteGeneral.Tables.Add(DTListarProductosPorTipo);
            fuenteReporteGeneral = new CRListarProductosPorTipo();
            fuenteReporteGeneral.SetDataSource(DSReporteGeneral);
            //this.CRVReporteGeneralAcceso.ReportSource = ReporteMercaderiaEnTransito;
        }

        public void ListarMovimientoDevoluciones(DataTable DTListarMovimientoDevoluciones, DateTime FechaInicio, DateTime FechaFin)
        {            
            this.DSReporteGeneral.Tables.Add(DTListarMovimientoDevoluciones);
            this.fuenteReporteGeneral = new CRListarMovimientoDevoluciones1();
            fuenteReporteGeneral.SetDataSource(DSReporteGeneral);
            cargarParametros(FechaInicio, FechaFin); 
        }

        public void ListarBitacoraReporte(DataTable DTListarBitacoraReporte, DateTime FechaInicio, DateTime FechaFin)
        {
            this.DSReporteGeneral.Tables.Add(DTListarBitacoraReporte);
            this.fuenteReporteGeneral = new CRListarBitacoraReporte1();
            fuenteReporteGeneral.SetDataSource(DSReporteGeneral);
            cargarParametros(FechaInicio, FechaFin);
        }
    }
}
