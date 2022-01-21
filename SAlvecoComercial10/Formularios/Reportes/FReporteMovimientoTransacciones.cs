using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SAlvecoComercial10.Formularios.ReportesCR;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

namespace SAlvecoComercial10.Formularios.Reportes
{
    public partial class FReporteMovimientoTransacciones : FReporteGeneral
    {
        public FReporteMovimientoTransacciones()
        {
            InitializeComponent();
            this.Text = "Movimiento de Transacciones";
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
            crtParamField.ParameterFieldName = "FechaHoraInicio";
            crtParamField.CurrentValues.Add(crtParamDiscreteValue);
            crtParamFields.Add(crtParamField);

            //------------------Fecha Fin
            crtParamDiscreteValue = new ParameterDiscreteValue();
            crtParamField = new ParameterField();
            crtParamDiscreteValue.Value = FechaFin;
            crtParamField.ParameterFieldName = "FechaHoraFin";
            crtParamField.CurrentValues.Add(crtParamDiscreteValue);
            crtParamFields.Add(crtParamField);


            this.CRVReporteGeneralAcceso.ParameterFieldInfo = crtParamFields;
        }

        public void ListarProductosEnTransitoPorPedido(DataTable DTMercaderiaEnTransito)
        {
            DSReporteGeneral.Tables.Clear();
            DSReporteGeneral.Tables.Add(DTMercaderiaEnTransito);
            DTMercaderiaEnTransito.Columns["NumeroAlmacen"].ColumnName = "NumeroAgencia";
            fuenteReporteGeneral = new CRListarProductosEnTransitoPorPedido();
            fuenteReporteGeneral.SetDataSource(DSReporteGeneral);
            //this.CRVReporteGeneralAcceso.ReportSource = ReporteMercaderiaEnTransito;
        }

        public void ListarComprasProductosUsuarios(DataTable DTListarComprasProductosUsuarios, DateTime FechaHoraInicio, DateTime FechaHoraFin, bool agruparPorUsuario)
        {
            DSReporteGeneral.Tables.Clear();

            if (agruparPorUsuario)
            {
                DSReporteGeneral.Tables.Add(DTListarComprasProductosUsuarios);
                fuenteReporteGeneral = new CRListarComprasProductosUsuarios1();
            }
            else
            {
                DTListarComprasProductosUsuarios.DefaultView.Sort = "FechaHoraCompra ASC";
                DSReporteGeneral.Tables.Add(DTListarComprasProductosUsuarios.DefaultView.ToTable());
                fuenteReporteGeneral = new CRListarComprasProductosUsuarios2();
            }
            fuenteReporteGeneral.SetDataSource(DSReporteGeneral);
            cargarParametros(FechaHoraInicio, FechaHoraFin);
            
        }

        public void ListarVentasProductosUsuarios(DataTable DTListarVentasProductosUsuarios, DateTime FechaHoraInicio, DateTime FechaHoraFin, bool agruparPorUsuario)
        {
            DSReporteGeneral.Tables.Clear();
            DSReporteGeneral.Tables.Add(DTListarVentasProductosUsuarios);
            if (agruparPorUsuario)
                fuenteReporteGeneral = new CRListarVentasProductosUsuarios1();
            else
                fuenteReporteGeneral = new CRListarVentasProductosUsuarios2();
            fuenteReporteGeneral.SetDataSource(DSReporteGeneral);
            cargarParametros(FechaHoraInicio, FechaHoraFin);

        }

        public void ListarVentasProductosPersonasDistribucion(DataTable DTListarVentasProductosUsuarios, DateTime FechaHoraInicio, DateTime FechaHoraFin, bool agruparPorUsuario)
        {
            DSReporteGeneral.Tables.Clear();
            DSReporteGeneral.Tables.Add(DTListarVentasProductosUsuarios);
            if (agruparPorUsuario)
                fuenteReporteGeneral = new CRListarVentasProductosPersonasDistribucion1();
            else
                fuenteReporteGeneral = new CRListarVentasProductosPersonasDistribucion2();
            fuenteReporteGeneral.SetDataSource(DSReporteGeneral);
            cargarParametros(FechaHoraInicio, FechaHoraFin);

        }


        public void ListarComprasProductosReportesPorFechasProveedor(DataTable DTListarComprasProductosReportesPorFechasProveedor, DateTime FechaHoraInicio, DateTime FechaHoraFin, bool AgruparPorProveedor)
        {
            DTListarComprasProductosReportesPorFechasProveedor.TableName = "ListarComprasProductosReportesPorFechasProveedor";
            DTListarComprasProductosReportesPorFechasProveedor.Constraints.Clear();

            this.DSReporteGeneral.Tables.Add(DTListarComprasProductosReportesPorFechasProveedor);
            
            if (AgruparPorProveedor)
                fuenteReporteGeneral = new CRListarComprasProductosReportesPorFechasProveedor();
            else
                fuenteReporteGeneral = new CRListarComprasProductosReportesPorFechasProveedor2();
            fuenteReporteGeneral.SetDataSource(DTListarComprasProductosReportesPorFechasProveedor);
            cargarParametros(FechaHoraInicio, FechaHoraFin);

        }

        public void ListarComprasProductosReportesPorFechasTipo(DataTable DTListarComprasProductosReportesPorFechasTipo, DateTime FechaHoraInicio, DateTime FechaHoraFin, bool AgruparPorFactura)
        {
            this.DSReporteGeneral.Tables.Add(DTListarComprasProductosReportesPorFechasTipo);            
            if (AgruparPorFactura)
                fuenteReporteGeneral = new CRListarComprasProductosReportesPorFechasTipo();
            else
                fuenteReporteGeneral = new CRListarComprasProductosReportesPorFechasTipo2();
            fuenteReporteGeneral.SetDataSource(DTListarComprasProductosReportesPorFechasTipo);
            cargarParametros(FechaHoraInicio, FechaHoraFin);

        }



        #region Funciones para los reportes de Ventas
        public void ListarVentasProductosReportesPorFechasCliente(DataTable DTListarVentasProductosReportesPorFechasCliente, DateTime FechaHoraInicio, DateTime FechaHoraFin, bool AgruparPorCliente)
        {
            this.DSReporteGeneral.Tables.Add(DTListarVentasProductosReportesPorFechasCliente);
            if (AgruparPorCliente)
                fuenteReporteGeneral = new CRListarVentasProductosReportesPorFechasCliente();
            else
                fuenteReporteGeneral = new CRListarVentasProductosReportesPorFechasCliente2();
            fuenteReporteGeneral.SetDataSource(DTListarVentasProductosReportesPorFechasCliente);
            cargarParametros(FechaHoraInicio, FechaHoraFin);

        }

        public void ListarVentasProductosReportesPorFechasTipo(DataTable DTListarVentasProductosReportesPorFechasTipo, DateTime FechaHoraInicio, DateTime FechaHoraFin, bool AgruparPorFactura)
        {
            this.DSReporteGeneral.Tables.Add(DTListarVentasProductosReportesPorFechasTipo);
            if (AgruparPorFactura)
                fuenteReporteGeneral = new CRListarVentasProductosReportesPorFechasTipo();
            else
                fuenteReporteGeneral = new CRListarVentasProductosReportesPorFechasTipo2();
            fuenteReporteGeneral.SetDataSource(DTListarVentasProductosReportesPorFechasTipo);
            cargarParametros(FechaHoraInicio, FechaHoraFin);

        }
        #endregion



        

        public void ListarCompraProductoCuentasPorPagarReporte(DataTable DTListarCompraProductoCuentasPorPagarReporte, DateTime FechaInicio, DateTime FechaFin)
        {
            DTListarCompraProductoCuentasPorPagarReporte.TableName = "ListarCompraProductoCuentasPorPagarReporte";
            DTListarCompraProductoCuentasPorPagarReporte.Constraints.Clear();

            this.DSReporteGeneral.Tables.Add(DTListarCompraProductoCuentasPorPagarReporte);
            this.fuenteReporteGeneral = new CRListarCompraProductoCuentasPorPagarReporte();
            fuenteReporteGeneral.SetDataSource(DSReporteGeneral);
            cargarParametros(FechaInicio, FechaFin);
        }

        

        public void ListarCompraProductoCuentasPorPagarReportePorProveedor(DataTable DTListarCompraProductoCuentasPorPagarReporte, DateTime FechaInicio, DateTime FechaFin)
        {
            DTListarCompraProductoCuentasPorPagarReporte.TableName = "ListarCompraProductoCuentasPorPagarReporte";
            DTListarCompraProductoCuentasPorPagarReporte.Constraints.Clear();

            this.DSReporteGeneral.Tables.Add(DTListarCompraProductoCuentasPorPagarReporte);
            this.fuenteReporteGeneral = new CRListarCompraProductoCuentasPorPagarReportePorProveedor();
            fuenteReporteGeneral.SetDataSource(DSReporteGeneral);
            cargarParametros(FechaInicio, FechaFin);
        }


        public void ListarVentaProductoCuentasPorCobrarReporte(DataTable DTListarVentaProductoCuentasPorCobrarReporte, DateTime FechaInicio, DateTime FechaFin)
        {
            DTListarVentaProductoCuentasPorCobrarReporte.TableName = "ListarVentaProductoCuentasPorCobrarReporte";
            DTListarVentaProductoCuentasPorCobrarReporte.Constraints.Clear();

            this.DSReporteGeneral.Tables.Add(DTListarVentaProductoCuentasPorCobrarReporte);
            this.fuenteReporteGeneral = new CRListarVentaProductoCuentasPorCobrarReporte();
            fuenteReporteGeneral.SetDataSource(DSReporteGeneral);
            cargarParametros(FechaInicio, FechaFin);
        }



        public void ListarVentaProductoCuentasPorCobrarReportePorProveedor(DataTable DTListarVentaProductoCuentasPorCobrarReporte, DateTime FechaInicio, DateTime FechaFin)
        {
            DTListarVentaProductoCuentasPorCobrarReporte.TableName = "ListarVentaProductoCuentasPorCobrarReporte";
            DTListarVentaProductoCuentasPorCobrarReporte.Constraints.Clear();

            this.DSReporteGeneral.Tables.Add(DTListarVentaProductoCuentasPorCobrarReporte);
            this.fuenteReporteGeneral = new CRListarVentaProductoCuentasPorCobrarReportePorCliente();
            fuenteReporteGeneral.SetDataSource(DSReporteGeneral);
            cargarParametros(FechaInicio, FechaFin);
        }


        //ListarVentasProductosReportesPorCreditosFechasCliente
        public void ListarVentasProductosReportesPorCreditosFechas(DataTable DTListarCompraProductoCuentasPorPagarReporte, DateTime FechaInicio, DateTime FechaFin)
        {
            DTListarCompraProductoCuentasPorPagarReporte.TableName = "ListarCompraProductoCuentasPorPagarReporte";
            DTListarCompraProductoCuentasPorPagarReporte.Constraints.Clear();

            this.DSReporteGeneral.Tables.Add(DTListarCompraProductoCuentasPorPagarReporte);
            this.fuenteReporteGeneral = new CRListarCompraProductoCuentasPorPagarReporte();
            fuenteReporteGeneral.SetDataSource(DSReporteGeneral);
            cargarParametros(FechaInicio, FechaFin);
        }



        public void ListarVentasProductosReportesPorCreditosFechasCliente(DataTable DTListarCompraProductoCuentasPorPagarReporte, DateTime FechaInicio, DateTime FechaFin)
        {
            DTListarCompraProductoCuentasPorPagarReporte.TableName = "ListarCompraProductoCuentasPorPagarReporte";
            DTListarCompraProductoCuentasPorPagarReporte.Constraints.Clear();

            this.DSReporteGeneral.Tables.Add(DTListarCompraProductoCuentasPorPagarReporte);
            this.fuenteReporteGeneral = new CRListarCompraProductoCuentasPorPagarReportePorProveedor();
            fuenteReporteGeneral.SetDataSource(DSReporteGeneral);
            cargarParametros(FechaInicio, FechaFin);
        }
    }
}
