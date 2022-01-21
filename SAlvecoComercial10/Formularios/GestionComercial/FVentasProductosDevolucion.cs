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
    public partial class FVentasProductosDevolucion : Form
    {
     
        private string DIUsuario;
        private int NumeroAlmacen;

        
        private VentasProductosTableAdapter TAVentaProductos;
        private VentasProductosDevolucionesTableAdapter TAVentasProductosDevoluciones;
        private VentaProductoDevolucionDetalleTableAdapter TAVentaProductoDevolucionDetalle;
        private ListarVentaProductoDevolucionDetalleParaMostrarTableAdapter TAVentasProductosDevolucionDetalleMostrar;
        private ListasVentaProductoDevolucionReporteTableAdapter TAListasVentaProductoDevolucionReporte;
        private ClientesTableAdapter TAClientes;
        private QTAUtilidadesFunciones TAFuncionesSistema;


        private AlvecoComercial10DataSet.DTProductosSeleccionadosDataTable DTProductosSeleccionados;        
        private AlvecoComercial10DataSet.VentasProductosDataTable DTVentaProductos;
        private AlvecoComercial10DataSet.VentasProductosDevolucionesDataTable DTVentasProductosDevolucion;        
        private AlvecoComercial10DataSet.ListarVentaProductoDevolucionDetalleParaMostrarDataTable DTVentaProductosDevolucionDetalle;        
        private AlvecoComercial10DataSet.ClientesDataTable DTClientes;

        
        public int NumeroVentaProducto { get; set; }
        public int NumeroVentaProductoDevolucion { get; set; }
        private string TipoOperacion = "";

        public FVentasProductosDevolucion(string DIUsuario, int NumeroAlmacen)
        {
            InitializeComponent();

            this.pnlProductosBusqueda = new SAlvecoComercial10.Formularios.Utilidades.PanelBusquedaProductosDevolucion(DTProductosSeleccionados,
                NumeroAlmacen, DIUsuario, "V");
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
            this.Controls.Add(this.gBoxDatosVenta);
            this.Controls.Add(this.toolStrip1);


            this.DIUsuario = DIUsuario;
            this.NumeroAlmacen = NumeroAlmacen;


            DTProductosSeleccionados = new AlvecoComercial10DataSet.DTProductosSeleccionadosDataTable();
            DTVentaProductosDevolucionDetalle = new AlvecoComercial10DataSet.ListarVentaProductoDevolucionDetalleParaMostrarDataTable();

            

            this.pnlProductosBusqueda.cargarParametrosConstructor(DTProductosSeleccionados,
                NumeroAlmacen, DIUsuario, "V", -1);
            this.pnlProductosBusqueda.ocultarPanel();
            TAVentaProductos = new VentasProductosTableAdapter();
            TAVentaProductos.Connection = Utilidades.DAOUtilidades.conexion;
            TAVentasProductosDevoluciones = new VentasProductosDevolucionesTableAdapter();
            TAVentasProductosDevoluciones.Connection = Utilidades.DAOUtilidades.conexion;
            TAVentasProductosDevolucionDetalleMostrar = new ListarVentaProductoDevolucionDetalleParaMostrarTableAdapter();
            TAVentasProductosDevolucionDetalleMostrar.Connection = Utilidades.DAOUtilidades.conexion;
            TAVentaProductoDevolucionDetalle = new VentaProductoDevolucionDetalleTableAdapter();
            TAVentaProductoDevolucionDetalle.Connection = Utilidades.DAOUtilidades.conexion;
            TAClientes = new ClientesTableAdapter();
            TAClientes.Connection = Utilidades.DAOUtilidades.conexion;
            TAListasVentaProductoDevolucionReporte = new ListasVentaProductoDevolucionReporteTableAdapter();
            TAListasVentaProductoDevolucionReporte.Connection = Utilidades.DAOUtilidades.conexion;
            

            DTClientes = TAClientes.GetData();
            cBoxCliente.DataSource = DTClientes;
            cBoxCliente.DisplayMember = "NombreCliente";
            cBoxCliente.ValueMember = "CodigoCliente";
            cBoxCliente.SelectedIndex = -1;

            cargarDatosVentaProducto(-1);


            DTProductosSeleccionados.RowChanged += DTProductosSeleccionados_RowChanged;
            DTProductosSeleccionados.RowDeleted += DTProductosSeleccionados_RowChanged;
        }

        void DTProductosSeleccionados_RowChanged(object sender, DataRowChangeEventArgs e)
        {

            //if (e.Action == DataRowAction.Change)
            //{
            //    e.Row["PrecioTotal"] = int.Parse(e.Row["Cantidad"].ToString()) * decimal.Parse(e.Row["PrecioUnitario"].ToString());
            //    e.Row.AcceptChanges();
            //}

            object PrecioTotal = DTProductosSeleccionados.Compute("Sum(PrecioTotal)", "");
            txtPrecioTotal.Text = PrecioTotal == null ? "0.00 Bs  " : PrecioTotal.ToString() + " Bs  ";
            //DTProductosSeleccionados.AcceptChanges();
        }

        public void adecuarColumnasVentaTemporal()
        {
            
            DGCCantidadVentaDevolucion.DataPropertyName = DTProductosSeleccionados.CantidadColumn.ColumnName;
            DGCCantidadVentaDevolucion.ReadOnly = false;
            DGCPrecioUnitarioDevolucion.DataPropertyName = DTProductosSeleccionados.PrecioUnitarioColumn.ColumnName;
            DGCPrecioUnitarioDevolucion.ReadOnly = false;
            
            DGCPrecioTotal.DataPropertyName = DTProductosSeleccionados.PrecioTotalColumn.ColumnName;
            DGCPrecioTotal.ReadOnly = true;
        }

        public void adecuarColumnasVisualizarVenta()
        {

            DGCCantidadVentaDevolucion.DataPropertyName = "CantidadVentaDevolucion";
            DGCCantidadVentaDevolucion.ReadOnly = true;
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
            this.cBoxCliente.SelectedIndex = -1;            
            this.tsLblFechaHoraRecepcion.Text = String.Empty;
            this.tsLblFechaHoraRegistro.Text = String.Empty;
            this.tsLblNroAgencia.Text = String.Empty;
            this.tsLabelNroVenta.Text = String.Empty;
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

        public void cargarDatosVentaProducto(int NumeroVentaProductoDevolucion)
        {
            DTVentasProductosDevolucion = TAVentasProductosDevoluciones.GetDataBy21(NumeroAlmacen, NumeroVentaProductoDevolucion);
            if (DTVentasProductosDevolucion.Count > 0)
            {
                this.NumeroVentaProductoDevolucion = NumeroVentaProductoDevolucion;
                DTVentaProductosDevolucionDetalle = TAVentasProductosDevolucionDetalleMostrar.GetData(NumeroAlmacen, NumeroVentaProductoDevolucion);
                adecuarColumnasVisualizarVenta();
                bdSourceVentaProductos.DataSource = DTVentaProductosDevolucionDetalle;

                DTVentaProductos = TAVentaProductos.GetDataBy21(NumeroAlmacen, DTVentasProductosDevolucion[0].NumeroVentaProducto);

                lblUsuario.Text = Utilidades.DAOUtilidades.ObtenerNombreCompleto(DTVentasProductosDevolucion[0].DIUsuario);
                cBoxCliente.SelectedValue = DTVentaProductos[0].CodigoCliente;
                gBoxDatosVenta.Text = String.Format("Datos Devolucion Venta Nº {0} , Estado : {1}", NumeroVentaProductoDevolucion, DTVentasProductosDevolucion[0].NumeroVentaProductoDevolucion, 
                    DTVentasProductosDevolucion[0].CodigoEstadoVentaDevolucion.CompareTo ("I") == 0 ? "INICIADO" : 
                    (DTVentasProductosDevolucion[0].CodigoEstadoVentaDevolucion.CompareTo ("A") == 0 ? "ANULADO" : "FINALIZADO") );
                

                txtObservaciones.Text = DTVentaProductos[0].IsObservacionesNull() ?
                    String.Empty : DTVentaProductos[0].Observaciones;

                tsLblNroAgencia.Text = "Nro Almacen :" + NumeroAlmacen.ToString();
                tsLabelNroVenta.Text = "Nro Venta :" + DTVentasProductosDevolucion[0].NumeroVentaProducto;
                tsLblNumeroDevolucion.Text = "Nro Devolucion :" + NumeroVentaProductoDevolucion.ToString();
                tsLblFechaHoraRegistro.Text = "Fecha Registro " + DTVentasProductosDevolucion[0].FechaHoraRegistro.ToString();
                tsLblFechaHoraRecepcion.Text = !DTVentasProductosDevolucion[0].IsFechaHoraDevolucionNull() ? "Fecha Recepcion " + DTVentasProductosDevolucion[0].FechaHoraDevolucion
                    : String.Empty;


                txtPrecioTotal.Text = DTVentasProductosDevolucion.Compute("sum(MontoTotalVentaDevolucion)", "").ToString() + " Bs  ";
                txtPrecioTotal.Text = DTVentasProductosDevolucion[0].MontoTotalPagoEfectivo.ToString() + " Bs  ";



                switch (DTVentasProductosDevolucion[0].CodigoEstadoVentaDevolucion)
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
            FVentasProductosAdministrador formVentasBuscador = new FVentasProductosAdministrador(NumeroAlmacen, DIUsuario);
            formVentasBuscador.formatearParaBusquedasDevoluciones();            
            if (formVentasBuscador.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                NumeroVentaProducto = formVentasBuscador.NumeroTransaccion;
                int NumeroDevolucionAux = Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("VentasProductosDevoluciones") + 1;
                limpiarControles();
                pnlProductosBusqueda.limpiarControles();
                pnlProductosBusqueda.NumeroTransaccionDevolucion = NumeroVentaProducto;
                TipoOperacion = "I";
                pnlProductosBusqueda.visualizarPanel();
                habilitarControles(true);
                habilitarBotonesAcciones(false, true, true, true, false, false, false);
                lblUsuario.Text = "Usuario Responsable " + Utilidades.DAOUtilidades.ObtenerNombreCompleto(DIUsuario);
                gBoxDatosVenta.Text = String.Format("Datos Devolucion Venta Nº {0} , Estado : {1}", NumeroDevolucionAux, "INICIADO");
                adecuarColumnasVentaTemporal();
                bdSourceVentaProductos.DataSource = DTProductosSeleccionados;
                tsLblFechaHoraRegistro.Text = "Fecha Registro " + Utilidades.DAOUtilidades.ObtenerFechaHoraServidor().ToString();
                tsLblNroAgencia.Text = "Nro Almacen " + NumeroAlmacen.ToString();

                DTVentaProductos = TAVentaProductos.GetDataBy21(NumeroAlmacen, NumeroVentaProducto);
                cBoxCliente.SelectedValue = DTVentaProductos[0].CodigoCliente;

                tsLblNumeroDevolucion.Text = "Nro Venta Dev. Producto " + NumeroDevolucionAux.ToString();
                DTVentaProductosDevolucionDetalle.Clear();
            }
            else
            {
                MessageBox.Show(this, "Aún no ha seleccionado ninguna Venta", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                NumeroVentaProducto = -1;
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
            adecuarColumnasVisualizarVenta();
            gBoxDatosVenta.Text = "Datos de Devolución Actual";
            NumeroVentaProducto = -1;
        }


        private void dtGVVentaProductosDevDev_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dtGVVentaProductosDev.CurrentCell != null)
            {
                if (e.ColumnIndex == DGCCantidadVentaDevolucion.Index || e.ColumnIndex == DGCPrecioUnitarioDevolucion.Index)
                {
                    dtGVVentaProductosDev[DGCPrecioTotal.Name, e.RowIndex].Value = DTProductosSeleccionados[e.RowIndex].Cantidad *
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
                DTProductosSeleccionadosXMLAux.TableName = "VentasProductosDevolucionesDetalle";
                DTProductosSeleccionadosXMLAux.AcceptChanges();

                DataSet DSProductosSeleccionados;
                DSProductosSeleccionados = new DataSet("VentasProductosDevoluciones");
                DSProductosSeleccionados.Tables.Add(DTProductosSeleccionadosXMLAux);

                string ProductosDetalleXML = DTProductosSeleccionadosXMLAux.DataSet.GetXml();


                decimal MontoTotalVenta = decimal.Parse(DTProductosSeleccionados.Compute("sum(PrecioTotal)", "").ToString());
                decimal MontoPagoEfectivo = 0;


                try
                {

                    if (TipoOperacion == "I")
                    {
                        
                        TAVentasProductosDevoluciones.InsertarVentaProductoDevolucionXMLDetalle(NumeroAlmacen, "", DIUsuario,
                            DateTime.Now, "I", MontoTotalVenta, MontoTotalVenta, NumeroVentaProducto, NumeroAlmacen, null,
                            txtObservaciones.Text, ProductosDetalleXML);

                        foreach (AlvecoComercial10DataSet.DTProductosSeleccionadosRow DRProductoSeleccionado in DTProductosSeleccionados.Rows)
                        {

                            DTVentaProductosDevolucionDetalle.AddListarVentaProductoDevolucionDetalleParaMostrarRow(DRProductoSeleccionado.CodigoProducto,
                                DRProductoSeleccionado.NombreProducto, DRProductoSeleccionado.Cantidad, DRProductoSeleccionado.PrecioUnitario,
                                DRProductoSeleccionado.PrecioTotal, DRProductoSeleccionado.MarcaProducto, DRProductoSeleccionado.MarcaProducto, 0);
                        }
                        DTVentaProductosDevolucionDetalle.AcceptChanges();
                        NumeroVentaProductoDevolucion = Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("VentasProductosDevoluciones");
                    }
                    else
                    {                        
                        TAVentasProductosDevoluciones.ActualizarVentaProductoDevolucionXMLDetalle(NumeroAlmacen, NumeroVentaProductoDevolucion, "", DIUsuario,
                            DateTime.Now, "I", MontoTotalVenta, MontoTotalVenta, NumeroVentaProducto, NumeroAlmacen, null,
                            txtObservaciones.Text, ProductosDetalleXML);

                        DTVentaProductosDevolucionDetalle.Clear();
                        DTVentaProductosDevolucionDetalle.AcceptChanges();
                        foreach (AlvecoComercial10DataSet.DTProductosSeleccionadosRow DRProductoSeleccionado in DTProductosSeleccionados.Rows)
                        {
                            DTVentaProductosDevolucionDetalle.AddListarVentaProductoDevolucionDetalleParaMostrarRow(DRProductoSeleccionado.CodigoProducto,
                                DRProductoSeleccionado.NombreProducto, DRProductoSeleccionado.Cantidad, DRProductoSeleccionado.PrecioUnitario,
                                DRProductoSeleccionado.PrecioTotal, DRProductoSeleccionado.MarcaProducto, DRProductoSeleccionado.MarcaProducto, 0);
                        }
                        DTVentaProductosDevolucionDetalle.AcceptChanges();
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

        private void dtGVVentaProductosDev_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            int CantidadVentada;

            this.dtGVVentaProductosDev.Rows[e.RowIndex].ErrorText = "";

            // No cell validation for new rows. New rows are validated on Row Validation.
            if (this.dtGVVentaProductosDev.Rows[e.RowIndex].IsNewRow) { return; }

            if (this.dtGVVentaProductosDev.IsCurrentCellDirty)
            {
                switch (this.dtGVVentaProductosDev.Columns[e.ColumnIndex].Name)
                {

                    case "DGCCantidadVentaDevolucion": 
                        if (e.FormattedValue.ToString().Trim().Length < 1)
                        {
                            this.dtGVVentaProductosDev.Rows[e.RowIndex].ErrorText = "   La Cantidad es necesaria y no puede estar vacia.";
                            e.Cancel = true;
                        }
                        else if (!int.TryParse(e.FormattedValue.ToString(), out CantidadVentada) || CantidadVentada <= 0)
                        {
                            this.dtGVVentaProductosDev.Rows[e.RowIndex].ErrorText = "   La Cantidad debe ser un entero positivo Mayor a Cero";
                            e.Cancel = true;
                        }
                        //MessageBox.Show("Valor a Validar " + e.FormattedValue.ToString() + ",  Valor Anterior " + this.dtGVVentaProductosDev.Rows[e.RowIndex].Cells["DGCCantidadExistencia"].Value.ToString());
                        break;

                    case "DGCPrecioUnitarioDevolucion":
                        if (e.FormattedValue.ToString().Trim().Length < 1)
                        {
                            this.dtGVVentaProductosDev.Rows[e.RowIndex].ErrorText = "   El Precio Unitario de Venta es necesario y no puede estar vacio.";
                            e.Cancel = true;
                        }
                        else if (!int.TryParse(e.FormattedValue.ToString(), out CantidadVentada) || CantidadVentada <= 0)
                        {
                            this.dtGVVentaProductosDev.Rows[e.RowIndex].ErrorText = "   El Precio Unitario de Venta debe ser un valor positivo Mayor a Cero";
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
            cargarProductosSeleccionados(DTVentaProductosDevolucionDetalle);
            habilitarBotonesAcciones(false, true, true, false, false, false, false);
            pnlProductosBusqueda.visualizarPanel();
            habilitarControles(true);
            bdSourceVentaProductos.DataSource = DTProductosSeleccionados;
            DTProductosSeleccionados.RowChanged += DTProductosSeleccionados_RowChanged;
            DTProductosSeleccionados.RowDeleted += DTProductosSeleccionados_RowChanged;
            adecuarColumnasVentaTemporal();
        }


        public void cargarProductosSeleccionados(DataTable DTVentasProductosDetalle)
        {
            pnlProductosBusqueda.limpiarControles();
            foreach (DataRow DRProductosDetalle in DTVentasProductosDetalle.Rows)
            {
                DTProductosSeleccionados.AddDTProductosSeleccionadosRow(
                    DRProductosDetalle["CodigoProducto"].ToString(), 
                    DRProductosDetalle["NombreProducto"].ToString(),
                    int.Parse(DRProductosDetalle["CantidadVentaDevolucion"].ToString()),
                    decimal.Parse(DRProductosDetalle["PrecioUnitarioDevolucion"].ToString()),
                    Utilidades.DAOUtilidades.ObtenerCantidadExistenciaProducto(NumeroAlmacen, DRProductosDetalle["CodigoProducto"].ToString()),
                    0,
                    DRProductosDetalle["NombreMarca"].ToString(),
                    int.Parse(DRProductosDetalle["CantidadVentaDevolucion"].ToString()) * decimal.Parse(DRProductosDetalle["PrecioUnitarioDevolucion"].ToString()), -1
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
                    TAVentasProductosDevoluciones.Delete(NumeroAlmacen, NumeroVentaProductoDevolucion);
                    //MessageBox.Show(this, "Registro eliminado correctamente", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cargarDatosVentaProducto(-1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Ocurrió la siguiente Excepcion " + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void tsBtnFinalizar_Click(object sender, EventArgs e)
        {
            string Producto = Utilidades.DAOUtilidades.EsPosibleCulminarDevolucion(NumeroAlmacen, NumeroVentaProductoDevolucion, "V");
            if (!String.IsNullOrEmpty(Producto))
            {
                MessageBox.Show(this, "No puede culminar la devolución total de los productos actuales debido a que al menos existe un"
                    + "Producto que sobrepasa la Cantidad Maxima de Devolucion (" + Producto + ") ", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            if (MessageBox.Show(this, "La Finalización de la Transacción actual implica la devolución de todos los Productos seleccionados por el Cliente"
                + ", para su posterior actualización en Existencia\r\n¿Desea Finalizar la Devolución de Productos?",
                "Finalización de Devolución de Venta de Productos", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {

                try
                {
                    
                    TAVentasProductosDevoluciones.ActualizarCodigoEstadoVentaDevolucion(NumeroAlmacen, NumeroVentaProductoDevolucion, "F");
                    TAVentasProductosDevoluciones.ActualizarInventarioVentasProductosDevolucion(NumeroAlmacen, NumeroVentaProductoDevolucion);                    
                    habilitarBotonesAcciones(true, false, false, false, false, false, true);
                    //MessageBox.Show(this, "Devolución de Venta Culminada y Devuelta Correctamente", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);


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
            ListarVentaProductoDevolucionDetalleParaMostrarTableAdapter TAVentasProductoDevolucionReporte = new ListarVentaProductoDevolucionDetalleParaMostrarTableAdapter();
            TAVentasProductoDevolucionReporte.Connection = Utilidades.DAOUtilidades.conexion;


            DataTable DTVentasProductosDev = TAListasVentaProductoDevolucionReporte.GetData(NumeroAlmacen, NumeroVentaProductoDevolucion);
            DataTable DTVentasProductosDevDetalle = TAVentasProductosDevolucionDetalleMostrar.GetData(NumeroAlmacen, NumeroVentaProductoDevolucion);
            FReporteTransaccionesGeneral formReporte = new FReporteTransaccionesGeneral();
            formReporte.cargarReporteVentasProductosDevolucion(DTVentasProductosDev, DTVentasProductosDevDetalle);
            formReporte.ShowDialog(this);
            formReporte.Dispose();
        }

    }
}
