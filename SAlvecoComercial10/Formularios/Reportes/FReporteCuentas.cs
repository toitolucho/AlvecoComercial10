using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SAlvecoComercial10.Formularios.ReportesCR;
using CrystalDecisions.Shared;

namespace SAlvecoComercial10.Formularios.Reportes
{
    public partial class FReporteCuentas : SAlvecoComercial10.Formularios.Reportes.FReporteGeneral
    {
        public FReporteCuentas()
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

        public void ListarCuentasPorCobrarPagar(DataTable DTListarCuentasPagarCobrar, DateTime FechaInicio, DateTime FechaFin, string TituloReporte)
        {
            DTListarCuentasPagarCobrar.TableName = "ListarCuentasPorCobrarPagar";
            DTListarCuentasPagarCobrar.Constraints.Clear();

            

            this.DSReporteGeneral.Tables.Add(DTListarCuentasPagarCobrar.Copy());
            this.fuenteReporteGeneral = new CRCuentasPorCobrarPagar();
            fuenteReporteGeneral.SetDataSource(DSReporteGeneral);


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

            //------------------Titulo
            crtParamDiscreteValue = new ParameterDiscreteValue();
            crtParamField = new ParameterField();
            crtParamDiscreteValue.Value = TituloReporte;
            crtParamField.ParameterFieldName = "Titulo";
            crtParamField.CurrentValues.Add(crtParamDiscreteValue);
            crtParamFields.Add(crtParamField);


            this.CRVReporteGeneralAcceso.ParameterFieldInfo = crtParamFields;

        }


        public void ListarCuentasPorCobrarPagarDetallePagos(DataTable DTListarCuentasPagarCobrar, DateTime FechaInicio, DateTime FechaFin, string TituloReporte)
        {
            DTListarCuentasPagarCobrar.TableName = "ListarCuentasPorCobrarPagarDetalle";
            DTListarCuentasPagarCobrar.Constraints.Clear();



            this.DSReporteGeneral.Tables.Add(DTListarCuentasPagarCobrar);
            this.fuenteReporteGeneral = new CRCuentasPorCobrarPagarPagosDetalle();
            fuenteReporteGeneral.SetDataSource(DSReporteGeneral);


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

            //------------------Titulo
            crtParamDiscreteValue = new ParameterDiscreteValue();
            crtParamField = new ParameterField();
            crtParamDiscreteValue.Value = TituloReporte;
            crtParamField.ParameterFieldName = "Titulo";
            crtParamField.CurrentValues.Add(crtParamDiscreteValue);
            crtParamFields.Add(crtParamField);


            this.CRVReporteGeneralAcceso.ParameterFieldInfo = crtParamFields;

        }

        public void ListarMovimientoMonetario(DataTable DTListarMovimientoMonetario, DateTime FechaInicio, DateTime FechaFin, bool ordenarPorUsuarios)
        {


            this.DSReporteGeneral.Tables.Add(DTListarMovimientoMonetario);
            if(ordenarPorUsuarios)
                this.fuenteReporteGeneral = new CRListarMovimientoMonetario();
            else
                this.fuenteReporteGeneral = new CRListarMovimientoMonetario2();
            fuenteReporteGeneral.SetDataSource(DSReporteGeneral);
            cargarParametros(FechaInicio, FechaFin);            
            
        }

        public void ListarMovimientoMonetarioVentasDistribuibles(DataTable DTListarMovimientoMonetarioVentasDistribuibles, DateTime FechaInicio, DateTime FechaFin)
        {
            DTListarMovimientoMonetarioVentasDistribuibles.TableName = "ListarMovimientoMonetario";
            this.DSReporteGeneral.Tables.Add(DTListarMovimientoMonetarioVentasDistribuibles);            
            this.fuenteReporteGeneral = new CRListarMovimientoMonetarioVentasDistribuibles();
            fuenteReporteGeneral.SetDataSource(DSReporteGeneral);
            cargarParametros(FechaInicio, FechaFin);            
        }

        public void ListarUsuariosPersonas(DataTable DTListarUsuariosPersonas)
        {
            DTListarUsuariosPersonas.TableName = "ListarUsuariosPersonas";
            this.DSReporteGeneral.Tables.Add(DTListarUsuariosPersonas);
            fuenteReporteGeneral = new CRListarUsuariosPersonas();
            fuenteReporteGeneral.SetDataSource(DSReporteGeneral);

        }
    }
}
