using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SAlvecoComercial10.Formularios.ReportesCR;

namespace SAlvecoComercial10.Formularios.Reportes
{
    public partial class FReporteTransaccionesGeneral : SAlvecoComercial10.Formularios.Reportes.FReporteGeneral
    {
        public FReporteTransaccionesGeneral()
        {
            InitializeComponent();
        }

        public void cargarReporteComprasProductos(DataTable DTCompraProducto, DataTable DTCompraProductoDetalle)
        {

            //DTCompraProductoDetalle.TableName = "ListarIngresosArticulosDetalleParaMostrar";
            //DTCompraProducto.TableName = "ListarIngresoArticuloReporte";
            DTCompraProductoDetalle.Constraints.Clear();
            //DTCompraProductoDetalle.Columns["CodigoProducto"].ColumnName = "CodigoArticulo";
            //DTCompraProductoDetalle.Columns["NombreProducto"].ColumnName = "NombreArticulo";
            //DTCompraProductoDetalle.Columns["CantidadCompra"].ColumnName = "CantidadIngreso";
            //DTCompraProductoDetalle.Columns["PrecioUnitarioCompra"].ColumnName = "PrecioUnitarioIngreso";
            //DTCompraProductoDetalle.Columns["TiempoGarantiaCompra"].ColumnName = "TiempoGarantiaIngreso";

            DTCompraProductoDetalle.AcceptChanges();

            this.DSReporteGeneral.Clear();
            this.DSReporteGeneral.Tables.Add(DTCompraProducto);
            this.DSReporteGeneral.Tables.Add(DTCompraProductoDetalle);



            fuenteReporteGeneral = new CRIngresosArticulosGeneral();
            fuenteReporteGeneral.SetDataSource(DSReporteGeneral);
        }

        public void cargarReporteVentasProductos(DataTable DTVentaProducto, DataTable DTVentaProductoDetalle)
        {

            //DTVentaProductoDetalle.TableName = "ListarSalidasArticulosDetalleParaMostrar";
            //DTVentaProducto.TableName = "ListasSalidaArticuloReporte";
            //DTVentaProductoDetalle.Constraints.Clear();
            //DTVentaProductoDetalle.Columns["CodigoProducto"].ColumnName = "CodigoArticulo";
            //DTVentaProductoDetalle.Columns["NombreProducto"].ColumnName = "NombreArticulo";
            //DTVentaProductoDetalle.Columns["CantidadVenta"].ColumnName = "CantidadSalida";
            //DTVentaProductoDetalle.Columns["PrecioUnitarioVenta"].ColumnName = "PrecioUnitarioSalida";
            //DTVentaProductoDetalle.Columns["TiempoGarantiaVenta"].ColumnName = "TiempoGarantiaSalida";

            //DTVentaProductoDetalle.AcceptChanges();

            this.DSReporteGeneral.Clear();
            this.DSReporteGeneral.Tables.Add(DTVentaProducto);
            this.DSReporteGeneral.Tables.Add(DTVentaProductoDetalle);



            fuenteReporteGeneral = new CRSalidasArticulosGeneral();
            fuenteReporteGeneral.SetDataSource(DSReporteGeneral);
        }

        public void cargarReporteTransferenciasProductos(DataTable DTTransferenciaProducto, DataTable DTTransferenciaProductoDetalle)
        {
            this.DSReporteGeneral.Clear();
            this.DSReporteGeneral.Tables.Add(DTTransferenciaProducto);
            this.DSReporteGeneral.Tables.Add(DTTransferenciaProductoDetalle);
            fuenteReporteGeneral = new CRTransferenciaProductosGeneral();
            fuenteReporteGeneral.SetDataSource(DSReporteGeneral);
        }

        public void cargarReporteVentasProductosDevolucion(DataTable DTVentaProductoDevolucion, DataTable DTVentaProductoDevolucionDetalle)
        {
            //ListarSolicitudSalidaArticulosDetalleParaMostrar
            //ListarSalidaArticuloDevolucionReporte
            //ListarSalidasArticuloDevolucionDetalleParaMostrar
            DTVentaProductoDevolucionDetalle.TableName = "ListarSalidasArticuloDevolucionDetalleParaMostrar";
            //DTVentaProductoDevolucionDetalle.TableName = "ListarSolicitudSalidaArticulosDetalleParaMostrar";
            DTVentaProductoDevolucion.TableName = "ListarSalidaArticuloDevolucionReporte";
            DTVentaProductoDevolucionDetalle.Constraints.Clear();
            DTVentaProductoDevolucionDetalle.Columns["CodigoProducto"].ColumnName = "CodigoArticulo";
            DTVentaProductoDevolucionDetalle.Columns["NombreProducto"].ColumnName = "NombreArticulo";
            DTVentaProductoDevolucionDetalle.Columns["CantidadVentaDevolucion"].ColumnName = "CantidadDevolucion";
            DTVentaProductoDevolucionDetalle.Columns["PrecioUnitarioDevolucion"].ColumnName = "PrecioUnitarioRetorno";            

            DTVentaProductoDevolucionDetalle.AcceptChanges();

            this.DSReporteGeneral.Clear();
            this.DSReporteGeneral.Tables.Add(DTVentaProductoDevolucion);
            this.DSReporteGeneral.Tables.Add(DTVentaProductoDevolucionDetalle);


            fuenteReporteGeneral = new CRSalidasArticulosDevolucionGeneral();
            fuenteReporteGeneral.SetDataSource(DSReporteGeneral);
        }

        public void cargarReporteComprasProductosDevolucion(DataTable DTCompraProductoDevolucion, DataTable DTCompraProductoDevolucionDetalle)
        {

            DTCompraProductoDevolucionDetalle.TableName = "ListarSalidasArticuloDevolucionDetalleParaMostrar";
            DTCompraProductoDevolucion.TableName = "ListarSalidaArticuloDevolucionReporte";
            DTCompraProductoDevolucionDetalle.Constraints.Clear();
            DTCompraProductoDevolucionDetalle.Columns["CodigoProducto"].ColumnName = "CodigoArticulo";
            DTCompraProductoDevolucionDetalle.Columns["NombreProducto"].ColumnName = "NombreArticulo";
            DTCompraProductoDevolucionDetalle.Columns["CantidadCompraDevolucion"].ColumnName = "CantidadDevolucion";
            DTCompraProductoDevolucionDetalle.Columns["PrecioUnitarioDevolucion"].ColumnName = "PrecioUnitarioRetorno";

            DTCompraProductoDevolucionDetalle.AcceptChanges();

            this.DSReporteGeneral.Clear();
            this.DSReporteGeneral.Tables.Add(DTCompraProductoDevolucion);
            this.DSReporteGeneral.Tables.Add(DTCompraProductoDevolucionDetalle);

            fuenteReporteGeneral = new CRIngresosArticulosDevolucionGeneral();
            fuenteReporteGeneral.SetDataSource(DSReporteGeneral);
        }
    }
}
