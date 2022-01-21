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
    public partial class FReporteInventarios : SAlvecoComercial10.Formularios.Reportes.FReporteGeneral
    {
        public FReporteInventarios()
        {
            InitializeComponent();
        }

        public void ListarVentasMotivosDanios(DataTable DtListarVentasMotivosDanios, DateTime FechaInicio, DateTime FechaFin, String NombreAlmacen)
        {
            DSReporteGeneral.Tables.Add(DtListarVentasMotivosDanios);
            fuenteReporteGeneral = new CRListarVentasMotivosDanios();
            this.fuenteReporteGeneral.SetDataSource(DSReporteGeneral);

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


            //------------------Almacen
            crtParamDiscreteValue = new ParameterDiscreteValue();
            crtParamField = new ParameterField();
            crtParamDiscreteValue.Value = String.IsNullOrEmpty(NombreAlmacen) ? "Todos los Almacenes" : "Almacen : " + NombreAlmacen;
            crtParamField.ParameterFieldName = "Almacen";
            crtParamField.CurrentValues.Add(crtParamDiscreteValue);
            crtParamFields.Add(crtParamField);


            CRVReporteGeneralAcceso.ParameterFieldInfo = crtParamFields;
        }




        public void ListarMovimientoTransaccionalProductos(DataTable DtListarVentasMotivosDanios, DateTime FechaInicio, DateTime FechaFin, String NombreAlmacen)
        {
            DSReporteGeneral.Tables.Add(DtListarVentasMotivosDanios);
            fuenteReporteGeneral = new CRListarMovimientoTransaccionalProductos();
            this.fuenteReporteGeneral.SetDataSource(DSReporteGeneral);

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


            //------------------Almacen
            crtParamDiscreteValue = new ParameterDiscreteValue();
            crtParamField = new ParameterField();
            crtParamDiscreteValue.Value = String.IsNullOrEmpty(NombreAlmacen) ? "Todos los Almacenes" : "Almacen : " + NombreAlmacen;
            crtParamField.ParameterFieldName = "Almacen";
            crtParamField.CurrentValues.Add(crtParamDiscreteValue);
            crtParamFields.Add(crtParamField);


            CRVReporteGeneralAcceso.ParameterFieldInfo = crtParamFields;
        }

        public void cargarReporteKardexProductos(DataTable DTKardexArticulos, DateTime FechaInicio, DateTime FechaFin, String NombreAlmacen)
        {
            DTKardexArticulos.TableName = "ListarKardexArticuloDetalladoReporte";
            DTKardexArticulos.Constraints.Clear();

            DTKardexArticulos.Columns["CodigoProducto"].ColumnName = "CodigoArticulo";
            DTKardexArticulos.Columns["NombreProducto"].ColumnName = "NombreArticulo";
            DTKardexArticulos.Columns["NumeroCompraProducto"].ColumnName = "NumeroIngresoArticulo";
            DTKardexArticulos.Columns["CantidadCompra"].ColumnName = "CantidadIngreso";
            DTKardexArticulos.Columns["PrecioUnitarioCompra"].ColumnName = "PrecioUnitarioIngreso";
            DTKardexArticulos.Columns["PrecioValoradoCompra"].ColumnName = "PrecioValoradoIngreso";
            DTKardexArticulos.Columns["CantidadVenta"].ColumnName = "CantidadSalida";
            DTKardexArticulos.Columns["PrecioUnitarioVenta"].ColumnName = "PrecioUnitarioSalida";
            DTKardexArticulos.Columns["PrecioValoradoVenta"].ColumnName = "PrecioValoradoSalida";

            DTKardexArticulos.AcceptChanges();


            this.DSReporteGeneral.Tables.Add(DTKardexArticulos);
            fuenteReporteGeneral = new CRKardexFisicoValoradoInventarioArticulos();
            this.fuenteReporteGeneral.SetDataSource(DSReporteGeneral);

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


            //------------------Almacen
            crtParamDiscreteValue = new ParameterDiscreteValue();
            crtParamField = new ParameterField();
            crtParamDiscreteValue.Value = String.IsNullOrEmpty(NombreAlmacen) ? "Todos los Almacenes" : "Almacen : " + NombreAlmacen;
            crtParamField.ParameterFieldName = "Almacen";
            crtParamField.CurrentValues.Add(crtParamDiscreteValue);
            crtParamFields.Add(crtParamField);

            
            CRVReporteGeneralAcceso.ParameterFieldInfo = crtParamFields;
        }

        public void ListarMovimientoArticuloReporte(DataTable DTListarMovimientoArticuloReporte,
            DateTime FechaInicio, DateTime FechaFin, String NombreAlmacen)
        {
            DTListarMovimientoArticuloReporte.TableName = "ListarMovimientoArticuloReporte";
            DTListarMovimientoArticuloReporte.Constraints.Clear();

            DTListarMovimientoArticuloReporte.Columns["CodigoProducto"].ColumnName = "CodigoArticulo";
            DTListarMovimientoArticuloReporte.Columns["NombreProducto"].ColumnName = "NombreArticulo";
            DTListarMovimientoArticuloReporte.Columns["CantidadInicial"].ColumnName = "CantidadInicial";
            DTListarMovimientoArticuloReporte.Columns["PrecioTotalInicial"].ColumnName = "PrecioTotalInicial";
            DTListarMovimientoArticuloReporte.Columns["PrecioTotalCompras"].ColumnName = "PrecioTotalIngresos";
            DTListarMovimientoArticuloReporte.Columns["CantidadCompras"].ColumnName = "CantidadIngresos";
            DTListarMovimientoArticuloReporte.Columns["CantidadVenta"].ColumnName = "CantidadSalida";
            DTListarMovimientoArticuloReporte.Columns["PrecioTotalVentas"].ColumnName = "PrecioTotalSalidas";
            DTListarMovimientoArticuloReporte.Columns["CantidadSaldo"].ColumnName = "CantidadSaldo";
            DTListarMovimientoArticuloReporte.Columns["PrecioTotalSaldo"].ColumnName = "PrecioTotalSaldo"; 

            this.DSReporteGeneral.Tables.Add(DTListarMovimientoArticuloReporte);
            this.fuenteReporteGeneral = new CRListarMovimientoArticuloReporte();
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

            //------------------Almacen
            crtParamDiscreteValue = new ParameterDiscreteValue();
            crtParamField = new ParameterField();
            crtParamDiscreteValue.Value = String.IsNullOrEmpty(NombreAlmacen) ? "Todos los Almacenes" : "Almacen : " + NombreAlmacen;
            crtParamField.ParameterFieldName = "Almacen";
            crtParamField.CurrentValues.Add(crtParamDiscreteValue);
            crtParamFields.Add(crtParamField);
            this.CRVReporteGeneralAcceso.ParameterFieldInfo = crtParamFields;

        }

        public void ListarMovimientoArticuloPorPartidaReporte(DataTable DTListarMovimientoProductoPorProductoTipoReporte, DateTime FechaInicio, DateTime FechaFin, string Almacen)
        {
            DTListarMovimientoProductoPorProductoTipoReporte.TableName = "ListarMovimientoArticuloPorPartidaReporte";
            DTListarMovimientoProductoPorProductoTipoReporte.Constraints.Clear();

            DTListarMovimientoProductoPorProductoTipoReporte.Columns["NombreProductoTipo"].ColumnName = "NombrePartida";
            DTListarMovimientoProductoPorProductoTipoReporte.Columns["CodigoProductoTipo"].ColumnName = "CodigoPartida";

            DTListarMovimientoProductoPorProductoTipoReporte.Columns["CodigoProducto"].ColumnName = "CodigoArticulo";
            DTListarMovimientoProductoPorProductoTipoReporte.Columns["NombreProducto"].ColumnName = "NombreArticulo";
            DTListarMovimientoProductoPorProductoTipoReporte.Columns["CantidadInicial"].ColumnName = "CantidadInicial";
            DTListarMovimientoProductoPorProductoTipoReporte.Columns["PrecioTotalInicial"].ColumnName = "PrecioTotalInicial";
            DTListarMovimientoProductoPorProductoTipoReporte.Columns["PrecioTotalCompras"].ColumnName = "PrecioTotalIngresos";
            DTListarMovimientoProductoPorProductoTipoReporte.Columns["CantidadCompras"].ColumnName = "CantidadIngresos";
            DTListarMovimientoProductoPorProductoTipoReporte.Columns["CantidadVenta"].ColumnName = "CantidadSalida";
            DTListarMovimientoProductoPorProductoTipoReporte.Columns["PrecioTotalVentas"].ColumnName = "PrecioTotalSalidas";
            DTListarMovimientoProductoPorProductoTipoReporte.Columns["CantidadSaldo"].ColumnName = "CantidadSaldo";
            DTListarMovimientoProductoPorProductoTipoReporte.Columns["PrecioTotalSaldo"].ColumnName = "PrecioTotalSaldo"; 

            this.DSReporteGeneral.Tables.Add(DTListarMovimientoProductoPorProductoTipoReporte);
            this.fuenteReporteGeneral = new CRListarMovimientoArticuloPorPartidaReporte();
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

            //------------------Almacen
            crtParamDiscreteValue = new ParameterDiscreteValue();
            crtParamField = new ParameterField();
            crtParamDiscreteValue.Value = Almacen;
            crtParamField.ParameterFieldName = "Almacen";
            crtParamField.CurrentValues.Add(crtParamDiscreteValue);
            crtParamFields.Add(crtParamField);


            CRVReporteGeneralAcceso.ParameterFieldInfo = crtParamFields;
        }

        public void cargarReporteInventarioGeneral(DataTable DTInventarioArticulos)
        {
            //DTInventarioArticulos.TableName = "ListarInventarioArticulosReportes";
            //DTInventarioArticulos.Constraints.Clear();
            //DTInventarioArticulos.Columns["CodigoProducto"].ColumnName = "CodigoArticulo";
            //DTInventarioArticulos.Columns["NombreProducto"].ColumnName = "NombreArticulo";
            //DTInventarioArticulos.Columns["NombreUnidad"].ColumnName = "NombreUnidad";
            //DTInventarioArticulos.Columns["CantidadExistencia"].ColumnName = "CantidadExistencia";
            //DTInventarioArticulos.Columns["CantidadRequerida"].ColumnName = "CantidadRequerida";
            //DTInventarioArticulos.Columns["StockMinimo"].ColumnName = "StockMinimo";
            //DTInventarioArticulos.Columns["PrecioUnitarioCompra"].ColumnName = "PrecioUnitarioIngreso";

            this.DSReporteGeneral.Tables.Add(DTInventarioArticulos);
            fuenteReporteGeneral = new CRInventarioArticulos();
            fuenteReporteGeneral.SetDataSource(DSReporteGeneral);
            
        }

        public void ListarArticulosRequeridos(DataTable DTListarArticulosRequeridos)
        {
            DTListarArticulosRequeridos.TableName = "ListarArticulosRequeridos";
            DTListarArticulosRequeridos.Constraints.Clear();
            DTListarArticulosRequeridos.Columns["CodigoProducto"].ColumnName = "CodigoArticulo";
            //DTListarArticulosRequeridos.Columns["NombreProducto"].ColumnName = "NombreArticulo1";
            DTListarArticulosRequeridos.Columns["NombreUnidad"].ColumnName = "NombreUnidad";
            DTListarArticulosRequeridos.Columns["CantidadExistencia"].ColumnName = "CantidadExistencia";
            DTListarArticulosRequeridos.Columns["CantidadRequerida"].ColumnName = "CantidadRequerida";
            DTListarArticulosRequeridos.Columns["StockMinimo"].ColumnName = "StockMinimo";
            DTListarArticulosRequeridos.Columns["PrecioUnitarioCompra"].ColumnName = "PrecioUnitarioIngreso";

            this.DSReporteGeneral.Tables.Add(DTListarArticulosRequeridos);
            fuenteReporteGeneral = new CRListarArticulosRequeridos();
            fuenteReporteGeneral.SetDataSource(DSReporteGeneral);
            
        }
    }
}
