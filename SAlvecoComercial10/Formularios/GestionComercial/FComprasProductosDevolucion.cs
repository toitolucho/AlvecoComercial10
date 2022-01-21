using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSetTableAdapters;
using SAlvecoComercial10.AccesoDatos;
using SAlvecoComercial10.Formularios.Reportes;

namespace SAlvecoComercial10.Formularios.GestionComercial
{
    public partial class FComprasProductosDevolucion : Form
    {

        private string DIUsuario;
        private int NumeroAlmacen;


        private ComprasProductosTableAdapter TACompraProductos;
        
        private ComprasProductosDevolucionTableAdapter TAComprasProductosDevoluciones;
        private ComprasProductosDetalleDevolucionTableAdapter TACompraProductoDevolucionDetalle;
        private ListarComprasProductosDevolucionDetalleParaMostrarTableAdapter TAComprasProductosDevolucionDetalleMostrar;
        private ListarCompraProductoDevolucionReporteTableAdapter TAListasCompraProductoDevolucionReporte;
        private ProveedoresTableAdapter TAProveedores;
        private QTAUtilidadesFunciones TAFuncionesSistema;


        private AlvecoComercial10DataSet.DTProductosSeleccionadosDataTable DTProductosSeleccionados;
        private AlvecoComercial10DataSet.ComprasProductosDataTable DTCompraProductos;
        private AlvecoComercial10DataSet.ComprasProductosDevolucionDataTable DTComprasProductosDevolucion;
        private AlvecoComercial10DataSet.ListarComprasProductosDevolucionDetalleParaMostrarDataTable DTCompraProductosDevolucionDetalle;
        private AlvecoComercial10DataSet.ProveedoresDataTable DTProveedores;


        public int NumeroCompraProducto { get; set; }
        public int NumeroCompraProductoDevolucion { get; set; }
        private string TipoOperacion = "";

        public FComprasProductosDevolucion(string DIUsuario, int NumeroAlmacen)
        {
            InitializeComponent();

            this.pnlProductosBusqueda = new SAlvecoComercial10.Formularios.Utilidades.PanelBusquedaProductosDevolucion(DTProductosSeleccionados,
                NumeroAlmacen, DIUsuario, "C");
            this.pnlProductosBusqueda.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlProductosBusqueda.DTProductosSeleccionados = null;
            this.pnlProductosBusqueda.Location = new System.Drawing.Point(69, 365);
            this.pnlProductosBusqueda.Name = "pnlProductosBusqueda";
            this.pnlProductosBusqueda.Size = new System.Drawing.Size(712, 171);
            this.pnlProductosBusqueda.TabIndex = 9;


            this.Controls.Clear();
            this.Controls.Add(this.gBoxGrilla);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.pnlProductosBusqueda);
            this.Controls.Add(this.gBoxDatosCompra);
            this.Controls.Add(this.toolStrip1);


            this.DIUsuario = DIUsuario;
            this.NumeroAlmacen = NumeroAlmacen;


            DTProductosSeleccionados = new AlvecoComercial10DataSet.DTProductosSeleccionadosDataTable();
            DTCompraProductosDevolucionDetalle = new AlvecoComercial10DataSet.ListarComprasProductosDevolucionDetalleParaMostrarDataTable();



            this.pnlProductosBusqueda.cargarParametrosConstructor(DTProductosSeleccionados,
                NumeroAlmacen, DIUsuario, "C", -1);
            this.pnlProductosBusqueda.ocultarPanel();
            TACompraProductos = new ComprasProductosTableAdapter();
            TACompraProductos.Connection = Utilidades.DAOUtilidades.conexion;
            TAComprasProductosDevoluciones = new ComprasProductosDevolucionTableAdapter();
            TAComprasProductosDevoluciones.Connection = Utilidades.DAOUtilidades.conexion;
            TAComprasProductosDevolucionDetalleMostrar = new ListarComprasProductosDevolucionDetalleParaMostrarTableAdapter();
            TAComprasProductosDevolucionDetalleMostrar.Connection = Utilidades.DAOUtilidades.conexion;
            TACompraProductoDevolucionDetalle = new ComprasProductosDetalleDevolucionTableAdapter();
            TACompraProductoDevolucionDetalle.Connection = Utilidades.DAOUtilidades.conexion;
            TAProveedores = new ProveedoresTableAdapter();
            TAProveedores.Connection = Utilidades.DAOUtilidades.conexion;
            TAListasCompraProductoDevolucionReporte = new ListarCompraProductoDevolucionReporteTableAdapter();
            TAListasCompraProductoDevolucionReporte.Connection = Utilidades.DAOUtilidades.conexion;


            DTProveedores = TAProveedores.GetData();
            cBoxProveedor.DataSource = DTProveedores;
            cBoxProveedor.DisplayMember = "NombreRazonSocial";
            cBoxProveedor.ValueMember = "CodigoProveedor";
            cBoxProveedor.SelectedIndex = -1;

            cargarDatosCompraProducto(-1);


            DTProductosSeleccionados.RowChanged += DTProductosSeleccionados_RowChanged;
            DTProductosSeleccionados.RowDeleted += DTProductosSeleccionados_RowChanged;
        }

        void DTProductosSeleccionados_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            object PrecioTotal = DTProductosSeleccionados.Compute("Sum(PrecioTotal)", "");
            txtPrecioTotal.Text = PrecioTotal == null ? "0.00 Bs  " : PrecioTotal.ToString() + " Bs  ";            
        }

        public void adecuarColumnasCompraTemporal()
        {

            DGCCantidadCompraDevolucion.DataPropertyName = DTProductosSeleccionados.CantidadColumn.ColumnName;
            DGCCantidadCompraDevolucion.ReadOnly = false;
            DGCPrecioUnitarioDevolucion.DataPropertyName = DTProductosSeleccionados.PrecioUnitarioColumn.ColumnName;
            DGCPrecioUnitarioDevolucion.ReadOnly = false;

            DGCPrecioTotal.DataPropertyName = DTProductosSeleccionados.PrecioTotalColumn.ColumnName;
            DGCPrecioTotal.ReadOnly = true;
        }

        public void adecuarColumnasVisualizarCompra()
        {

            DGCCantidadCompraDevolucion.DataPropertyName = "CantidadCompraDevolucion";
            DGCCantidadCompraDevolucion.ReadOnly = true;
            DGCPrecioUnitarioDevolucion.DataPropertyName = "PrecioUnitarioDevolucion";
            DGCPrecioUnitarioDevolucion.ReadOnly = true;
            DGCPrecioTotal.DataPropertyName = "PrecioTotal";
        }


        public void habilitarControles(bool estadoHabilitacion)
        {
            this.txtObservaciones.ReadOnly = !estadoHabilitacion;
        }

        public void limpiarControles()
        {
            this.txtObservaciones.Text = String.Empty;
            this.cBoxProveedor.SelectedIndex = -1;
            this.tsLblFechaHoraRecepcion.Text = String.Empty;
            this.tsLblFechaHoraRegistro.Text = String.Empty;
            this.tsLblNroAgencia.Text = String.Empty;
            this.tsLabelNroCompra.Text = String.Empty;
            this.tsLblNumeroDevolucion.Text = String.Empty;
            this.lblUsuario.Text = String.Empty;
            DTProductosSeleccionados.Clear();
            txtPrecioTotal.Text = "0.00 BS";
        }

        public void habilitarBotonesAcciones(bool nuevo, bool aceptar, bool cancelar, bool modificar, bool anular, bool finalizar, bool reportes)
        {
            this.tsBtnNuevo.Enabled = nuevo;
            this.tsBtnAceptar.Enabled = aceptar;
            this.tsBtnCancelar.Enabled = cancelar;
            this.tsBtnModificar.Enabled = modificar;
            this.tsBtnAnular.Enabled = anular;
            this.tsBtnFinalizar.Enabled = finalizar;
            this.tsBtnReportes.Enabled = reportes;
        }

        public bool validarDatos()
        {
            errorProvider1.Clear();
            if (DTProductosSeleccionados.Count == 0)
            {
                MessageBox.Show(this, "Aún no ha seleccionado ningún producto", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                errorProvider1.SetError(txtPrecioTotal, "Aún no ha seleccionado ningún producto, El Monto es Cero");
                return false;

            }
            return true;
        }

        public void cargarDatosCompraProducto(int NumeroCompraProductoDevolucion)
        {
            DTComprasProductosDevolucion = TAComprasProductosDevoluciones.GetDataByNumeroDevolucion(NumeroAlmacen, NumeroCompraProductoDevolucion);
            if (DTComprasProductosDevolucion.Count > 0)
            {
                this.NumeroCompraProductoDevolucion = NumeroCompraProductoDevolucion;
                DTCompraProductosDevolucionDetalle = TAComprasProductosDevolucionDetalleMostrar.GetData(NumeroAlmacen, NumeroCompraProductoDevolucion);
                adecuarColumnasVisualizarCompra();
                bdSourceVentaProductos.DataSource = DTCompraProductosDevolucionDetalle;
                
                DTCompraProductos = TACompraProductos.GetDataByNumeroCompraProducto(NumeroAlmacen, DTComprasProductosDevolucion[0].NumeroCompraProducto);

                lblUsuario.Text = Utilidades.DAOUtilidades.ObtenerNombreCompleto(DTComprasProductosDevolucion[0].DIUsuario);
                cBoxProveedor.SelectedValue = DTCompraProductos[0].CodigoProveedor;
                gBoxDatosCompra.Text = String.Format("Datos Devolucion Compra Nº {0} , Estado : {1}", NumeroCompraProductoDevolucion, DTComprasProductosDevolucion[0].NumeroCompraProductoDevolucion,
                    DTComprasProductosDevolucion[0].CodigoEstadoCompraDevolucion.CompareTo("I") == 0 ? "INICIADO" :
                    (DTComprasProductosDevolucion[0].CodigoEstadoCompraDevolucion.CompareTo("A") == 0 ? "ANULADO" : "FINALIZADO"));


                txtObservaciones.Text = DTCompraProductos[0].IsObservacionesNull() ?
                    String.Empty : DTCompraProductos[0].Observaciones;

                tsLblNroAgencia.Text = "Nro Almacen :" + NumeroAlmacen.ToString();
                tsLabelNroCompra.Text = "Nro Compra :" + DTComprasProductosDevolucion[0].NumeroCompraProducto;
                tsLblNumeroDevolucion.Text = "Nro Devolucion :" + NumeroCompraProductoDevolucion.ToString();
                tsLblFechaHoraRegistro.Text = "Fecha Registro " + DTComprasProductosDevolucion[0].FechaHoraRegistro.ToString();
                tsLblFechaHoraRecepcion.Text = !DTComprasProductosDevolucion[0].IsFechaHoraDevolucionNull() ? "Fecha Recepcion " + DTComprasProductosDevolucion[0].FechaHoraDevolucion
                    : String.Empty;


                txtPrecioTotal.Text = DTComprasProductosDevolucion.Compute("sum(MontoTotalCompraDevolucion)", "").ToString() + " Bs  ";
                txtPrecioTotal.Text = DTComprasProductosDevolucion[0].MontoTotalPagoEfectivo.ToString() + " Bs  ";



                switch (DTComprasProductosDevolucion[0].CodigoEstadoCompraDevolucion)
                {
                    case "I":
                        habilitarBotonesAcciones(true, false, false, true, true, true, true);
                        break;
                    case "F":
                        habilitarBotonesAcciones(true, false, false, false, false, false, true);
                        break;
                    case "A":
                        habilitarBotonesAcciones(true, false, false, true, true, true, true);
                        break;
                    case "D":
                        habilitarBotonesAcciones(true, false, false, false, false, true, true);
                        break;
                    case "X":
                        habilitarBotonesAcciones(true, false, false, false, false, false, false);
                        break;
                    default:
                        habilitarBotonesAcciones(true, false, false, false, false, false, false);
                        break;
                }
            }
            else
            {
                limpiarControles();
                habilitarBotonesAcciones(true, false, false, false, false, false, false);
            }
            habilitarControles(false);
        }

        private void tsBtnNuevo_Click(object sender, EventArgs e)
        {
            FComprasProductosAdministrador formComprasBuscador = new FComprasProductosAdministrador(NumeroAlmacen, DIUsuario);
            formComprasBuscador.formatearParaBusquedasDevoluciones();
            if (formComprasBuscador.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                NumeroCompraProducto = formComprasBuscador.NumeroTransaccion;
                int NumeroDevolucionAux = Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("ComprasProductosDevoluciones") + 1;
                limpiarControles();
                pnlProductosBusqueda.limpiarControles();
                pnlProductosBusqueda.NumeroTransaccionDevolucion = NumeroCompraProducto;
                TipoOperacion = "I";
                pnlProductosBusqueda.visualizarPanel();
                habilitarControles(true);
                habilitarBotonesAcciones(false, true, true, true, false, false, false);
                lblUsuario.Text = "Usuario Responsable " + Utilidades.DAOUtilidades.ObtenerNombreCompleto(DIUsuario);
                gBoxDatosCompra.Text = String.Format("Datos Devolucion Compra Nº {0} , Estado : {1}", NumeroDevolucionAux, "INICIADO");
                adecuarColumnasCompraTemporal();
                bdSourceVentaProductos.DataSource = DTProductosSeleccionados;
                tsLblFechaHoraRegistro.Text = "Fecha Registro " + Utilidades.DAOUtilidades.ObtenerFechaHoraServidor().ToString();
                tsLblNroAgencia.Text = "Nro Almacen " + NumeroAlmacen.ToString();
                DTCompraProductos = TACompraProductos.GetDataByNumeroCompraProducto(NumeroAlmacen, NumeroCompraProducto);
                cBoxProveedor.SelectedValue = DTCompraProductos[0].CodigoProveedor;
                tsLblNumeroDevolucion.Text = "Nro Compra Dev. Producto " + NumeroDevolucionAux.ToString();
                DTCompraProductosDevolucionDetalle.Clear();
            }
            else
            {
                MessageBox.Show(this, "Aún no ha seleccionado ninguna Compra", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                NumeroCompraProducto = -1;
            }
        }

        private void tsBtnCancelar_Click(object sender, EventArgs e)
        {
            TipoOperacion = "";
            errorProvider1.Clear();
            pnlProductosBusqueda.ocultarPanel();
            limpiarControles();
            habilitarControles(false);
            habilitarBotonesAcciones(true, false, false, false, false, false, false);
            adecuarColumnasVisualizarCompra();
            gBoxDatosCompra.Text = "Datos de Devolución Actual";
            NumeroCompraProducto = -1;
        }


        private void dtGVCompraProductosDevDev_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dtGVCompraProductosDev.CurrentCell != null)
            {
                if (e.ColumnIndex == DGCCantidadCompraDevolucion.Index || e.ColumnIndex == DGCPrecioUnitarioDevolucion.Index)
                {
                    dtGVCompraProductosDev[DGCPrecioTotal.Name, e.RowIndex].Value = DTProductosSeleccionados[e.RowIndex].Cantidad *
                        DTProductosSeleccionados[e.RowIndex].PrecioUnitario;

                    DTProductosSeleccionados.AcceptChanges();
                }
            }
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            DTProductosSeleccionados.AcceptChanges();
        }

        private void tsBtnAceptar_Click(object sender, EventArgs e)
        {
            DTProductosSeleccionados.AcceptChanges();

            if (validarDatos())
            {
                DataTable DTProductosSeleccionadosXMLAux = DTProductosSeleccionados.Copy();
                DTProductosSeleccionadosXMLAux.Constraints.Clear();
                DTProductosSeleccionadosXMLAux.Columns.Remove(DTProductosSeleccionadosXMLAux.Columns["NombreProducto"]);
                DTProductosSeleccionadosXMLAux.TableName = "ComprasProductosDevolucionesDetalle";
                DTProductosSeleccionadosXMLAux.AcceptChanges();

                DataSet DSProductosSeleccionados;
                DSProductosSeleccionados = new DataSet("ComprasProductosDevoluciones");
                DSProductosSeleccionados.Tables.Add(DTProductosSeleccionadosXMLAux);

                string ProductosDetalleXML = DTProductosSeleccionadosXMLAux.DataSet.GetXml();


                decimal MontoTotalCompra = decimal.Parse(DTProductosSeleccionados.Compute("sum(PrecioTotal)", "").ToString());
                decimal MontoPagoEfectivo = 0;


                try
                {

                    if (TipoOperacion == "I")
                    {

                        TAComprasProductosDevoluciones.InsertarCompraProductoDevolucionXMLDetalle(NumeroAlmacen, "", DIUsuario,
                            DateTime.Now, "I", MontoTotalCompra, MontoTotalCompra, NumeroCompraProducto, NumeroAlmacen, null,
                            txtObservaciones.Text, ProductosDetalleXML);

                        foreach (AlvecoComercial10DataSet.DTProductosSeleccionadosRow DRProductoSeleccionado in DTProductosSeleccionados.Rows)
                        {

                            DTCompraProductosDevolucionDetalle.AddListarComprasProductosDevolucionDetalleParaMostrarRow(DRProductoSeleccionado.CodigoProducto,
                                DRProductoSeleccionado.NombreProducto, DRProductoSeleccionado.Cantidad, DRProductoSeleccionado.PrecioUnitario,
                                DRProductoSeleccionado.PrecioTotal, DRProductoSeleccionado.MarcaProducto, DRProductoSeleccionado.MarcaProducto, 0);
                        }
                        DTCompraProductosDevolucionDetalle.AcceptChanges();
                        NumeroCompraProductoDevolucion = Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("ComprasProductosDevoluciones");
                    }
                    else
                    {
                        TAComprasProductosDevoluciones.ActualizarCompraProductoDevolucionXMLDetalle(NumeroAlmacen, NumeroCompraProductoDevolucion, "", DIUsuario,
                            DateTime.Now, "I", MontoTotalCompra, MontoTotalCompra, NumeroCompraProducto, NumeroAlmacen, null,
                            txtObservaciones.Text, ProductosDetalleXML);

                        DTCompraProductosDevolucionDetalle.Clear();
                        DTCompraProductosDevolucionDetalle.AcceptChanges();
                        foreach (AlvecoComercial10DataSet.DTProductosSeleccionadosRow DRProductoSeleccionado in DTProductosSeleccionados.Rows)
                        {
                            DTCompraProductosDevolucionDetalle.AddListarComprasProductosDevolucionDetalleParaMostrarRow(DRProductoSeleccionado.CodigoProducto,
                                DRProductoSeleccionado.NombreProducto, DRProductoSeleccionado.Cantidad, DRProductoSeleccionado.PrecioUnitario,
                                DRProductoSeleccionado.PrecioTotal, DRProductoSeleccionado.MarcaProducto, DRProductoSeleccionado.MarcaProducto, 0);
                        }
                        DTCompraProductosDevolucionDetalle.AcceptChanges();
                    }

                    //MessageBox.Show(this, "La operación actual fue registrada correctamente", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    habilitarControles(false);
                    habilitarBotonesAcciones(true, false, false, true, true, true, true);
                    TipoOperacion = "";
                    pnlProductosBusqueda.ocultarPanel();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Ocurrio la siguiente Excepción " + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dtGVCompraProductosDev_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            int CantidadComprada;

            this.dtGVCompraProductosDev.Rows[e.RowIndex].ErrorText = "";

            // No cell validation for new rows. New rows are validated on Row Validation.
            if (this.dtGVCompraProductosDev.Rows[e.RowIndex].IsNewRow) { return; }

            if (this.dtGVCompraProductosDev.IsCurrentCellDirty)
            {
                switch (this.dtGVCompraProductosDev.Columns[e.ColumnIndex].Name)
                {
                        
                    case "DGCCantidadCompraDevolucion":
                        if (e.FormattedValue.ToString().Trim().Length < 1)
                        {
                            this.dtGVCompraProductosDev.Rows[e.RowIndex].ErrorText = "   La Cantidad es necesaria y no puede estar vacia.";
                            e.Cancel = true;
                        }
                        else if (!int.TryParse(e.FormattedValue.ToString(), out CantidadComprada) || CantidadComprada <= 0)
                        {
                            this.dtGVCompraProductosDev.Rows[e.RowIndex].ErrorText = "   La Cantidad debe ser un entero positivo Mayor a Cero";
                            e.Cancel = true;
                        }
                        //MessageBox.Show("Valor a Validar " + e.FormattedValue.ToString() + ",  Valor Anterior " + this.dtGVVentaProductosDev.Rows[e.RowIndex].Cells["DGCCantidadExistencia"].Value.ToString());
                        break;

                    case "DGCPrecioUnitarioDevolucion":
                        if (e.FormattedValue.ToString().Trim().Length < 1)
                        {
                            this.dtGVCompraProductosDev.Rows[e.RowIndex].ErrorText = "   El Precio Unitario de Compra es necesario y no puede estar vacio.";
                            e.Cancel = true;
                        }
                        else if (!int.TryParse(e.FormattedValue.ToString(), out CantidadComprada) || CantidadComprada <= 0)
                        {
                            this.dtGVCompraProductosDev.Rows[e.RowIndex].ErrorText = "   El Precio Unitario de Compra debe ser un valor positivo Mayor a Cero";
                            e.Cancel = true;
                        }
                        //MessageBox.Show("Valor a Validar " + e.FormattedValue.ToString() + ",  Valor Anterior " + this.dtGVVentaProductosDev.Rows[e.RowIndex].Cells["DGCCantidadExistencia"].Value.ToString());
                        break;


                }

            }
        }

        private void tsBtnModificar_Click(object sender, EventArgs e)
        {
            TipoOperacion = "E";
            cargarProductosSeleccionados(DTCompraProductosDevolucionDetalle);
            habilitarBotonesAcciones(false, true, true, false, false, false, false);
            pnlProductosBusqueda.visualizarPanel();
            habilitarControles(true);
            bdSourceVentaProductos.DataSource = DTProductosSeleccionados;
            DTProductosSeleccionados.RowChanged += DTProductosSeleccionados_RowChanged;
            DTProductosSeleccionados.RowDeleted += DTProductosSeleccionados_RowChanged;
            adecuarColumnasCompraTemporal();
        }


        public void cargarProductosSeleccionados(DataTable DTComprasProductosDetalle)
        {
            pnlProductosBusqueda.limpiarControles();
            foreach (DataRow DRProductosDetalle in DTComprasProductosDetalle.Rows)
            {
                DTProductosSeleccionados.AddDTProductosSeleccionadosRow(
                    DRProductosDetalle["CodigoProducto"].ToString(),
                    DRProductosDetalle["NombreProducto"].ToString(),
                    int.Parse(DRProductosDetalle["CantidadCompraDevolucion"].ToString()),
                    decimal.Parse(DRProductosDetalle["PrecioUnitarioDevolucion"].ToString()),
                    Utilidades.DAOUtilidades.ObtenerCantidadExistenciaProducto(NumeroAlmacen, DRProductosDetalle["CodigoProducto"].ToString()),
                    0,
                    DRProductosDetalle["NombreMarca"].ToString(),
                    int.Parse(DRProductosDetalle["CantidadCompraDevolucion"].ToString()) * decimal.Parse(DRProductosDetalle["PrecioUnitarioDevolucion"].ToString()),
                    -1
                    );
            }
            DTProductosSeleccionados.AcceptChanges();
        }

        private void tsBtnAnular_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Se encuentra seguro de eliminar el Registro Actual?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    TAComprasProductosDevoluciones.Delete(NumeroAlmacen, NumeroCompraProductoDevolucion);
                    //MessageBox.Show(this, "Registro eliminado correctamente", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cargarDatosCompraProducto(-1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Ocurrió la siguiente Excepcion " + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void tsBtnFinalizar_Click(object sender, EventArgs e)
        {
            string Producto = Utilidades.DAOUtilidades.EsPosibleCulminarDevolucion(NumeroAlmacen, NumeroCompraProductoDevolucion, "C");
            if (!String.IsNullOrEmpty(Producto))
            {
                MessageBox.Show(this, "No puede culminar la devolución total de los productos actuales debido a que al menos existe un"
                    + "Producto que sobrepasa la Cantidad Maxima de Devolucion (" + Producto + ") ", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            if (MessageBox.Show(this, "La Finalización de la Transacción actual implica la devolución de todos los Productos seleccionados por el Proveedore"
                + ", para su posterior actualización en Existencia\r\n¿Desea Finalizar la Devolución de Productos?",
                "Finalización de Devolución de Compra de Productos", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {

                try
                {

                    TAComprasProductosDevoluciones.ActualizarCodigoEstadoCompraDevolucion(NumeroAlmacen, NumeroCompraProductoDevolucion, "F");
                    TAComprasProductosDevoluciones.ActualizarInventarioComprasProductosDevolucion(NumeroAlmacen, NumeroCompraProductoDevolucion);
                    habilitarBotonesAcciones(true, false, false, false, false, false, true);
                    //MessageBox.Show(this, "Devolución de Compra Culminada y Devuelta Correctamente", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);


                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "No se Pudo Realizar la Operación Actual, ocurrió la Siguiente Excepción \r\n" + ex.Message,
                        "Error en Finalización de Ingresos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }


        public void emitirPermisos(bool permitirNuevo, bool permitirModificar, bool permitirAnular, bool permitirFinalizar)
        {
            this.tsBtnNuevo.Visible = permitirNuevo;
            this.tsBtnModificar.Visible = permitirModificar;
            this.tsBtnAnular.Visible = permitirAnular;
            this.tsBtnFinalizar.Visible = permitirFinalizar;
        }

        private void tsBtnReportes_Click(object sender, EventArgs e)
        {
            ListarCompraProductoDevolucionReporteTableAdapter TAComprasProductoDevolucionReporte = new ListarCompraProductoDevolucionReporteTableAdapter();
            TAComprasProductoDevolucionReporte.Connection = Utilidades.DAOUtilidades.conexion;
            DataTable DTComprasProductosDev = TAComprasProductoDevolucionReporte.GetData(NumeroAlmacen, NumeroCompraProductoDevolucion);
            DataTable DTComprasProductosDevDetalle = TAComprasProductosDevolucionDetalleMostrar.GetData(NumeroAlmacen, NumeroCompraProductoDevolucion);
            FReporteTransaccionesGeneral formReporte = new FReporteTransaccionesGeneral();
            formReporte.cargarReporteComprasProductosDevolucion(DTComprasProductosDev, DTComprasProductosDevDetalle);
            formReporte.ShowDialog(this);
            formReporte.Dispose();
        }

    }
}
