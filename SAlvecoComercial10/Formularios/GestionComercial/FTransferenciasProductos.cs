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
    public partial class FTransferenciasProductos : Form
    {
        private string DIUsuario;
        private int NumeroAlmacen;
        private AlvecoComercial10DataSet.DTProductosSeleccionadosDataTable DTProductosSeleccionados;
        private TransferenciasProductosTableAdapter TATransferenciaProductos;
        private AlvecoComercial10DataSet.TransferenciasProductosDataTable DTTransferenciaProductos;
        private VentasProductosDetalleTableAdapter TATransferenciaProductosDetalle;
        private ListarTransferenciaProductosDetalleReporteTableAdapter TATransferenciasProductosDetalleMostrar;
        private ListarProductosTransferidosSuperaStockMinimoTableAdapter TAListarProductosCantidadSuperaStockMinimo;
        private ListarProductosTransferenciaInsuficienteTableAdapter TAListarProductosExistenciaInsuficiente;        
        private ListarProductosCantidadSuperaStockMinimoXMLTableAdapter TAListarProductosCantidadSuperaStockMinimoXML;
        private ListarProductosExistenciaInsuficienteXMLTableAdapter TAListarProductosExistenciaInsuficienteXML;
        private AccesoDatos.AlvecoComercial10DataSet.PersonasDataTable DTPersonas;
        private AccesoDatos.AlvecoComercial10DataSet.AlmacenesDataTable DTAlmacenesEmisor;


        

        
        private AlvecoComercial10DataSet.ListarTransferenciaProductosDetalleReporteDataTable DTTransferenciaProductosDetalle;
        private AlmacenesTableAdapter TAAlmacenes;
        private AlvecoComercial10DataSet.AlmacenesDataTable DTAlmacenesReceptor;
        
        private PersonasTableAdapter TAPersonas;
        private Utilidades.ObjetoCodigoDescripcion TransferenciaEstado;
        
        public int NumeroTransferenciaProducto { get; set; }
        private string TipoOperacion = "";

        public FTransferenciasProductos(string DIUsuario, int NumeroAlmacen)
        {
            InitializeComponent();

            this.pnlProductosBusqueda = new SAlvecoComercial10.Formularios.Utilidades.PanelBusquedaProductos(DTProductosSeleccionados,
                NumeroAlmacen, DIUsuario, "T");
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
            
            this.pnlProductosBusqueda.cargarParametrosConstructor(DTProductosSeleccionados,
                NumeroAlmacen, DIUsuario, "T");
            this.pnlProductosBusqueda.ocultarPanel();
            TATransferenciaProductos = new TransferenciasProductosTableAdapter();
            TATransferenciaProductos.Connection = Utilidades.DAOUtilidades.conexion;
            TATransferenciaProductosDetalle = new VentasProductosDetalleTableAdapter();
            TATransferenciaProductosDetalle.Connection = Utilidades.DAOUtilidades.conexion;
            TATransferenciasProductosDetalleMostrar = new ListarTransferenciaProductosDetalleReporteTableAdapter();
            TATransferenciasProductosDetalleMostrar.Connection = Utilidades.DAOUtilidades.conexion;
            TAListarProductosCantidadSuperaStockMinimo = new ListarProductosTransferidosSuperaStockMinimoTableAdapter();
            TAListarProductosCantidadSuperaStockMinimo.Connection = Utilidades.DAOUtilidades.conexion;
            TAListarProductosExistenciaInsuficiente = new ListarProductosTransferenciaInsuficienteTableAdapter();
            TAListarProductosExistenciaInsuficiente.Connection = Utilidades.DAOUtilidades.conexion;
            TAListarProductosCantidadSuperaStockMinimoXML = new ListarProductosCantidadSuperaStockMinimoXMLTableAdapter();
            TAListarProductosCantidadSuperaStockMinimoXML.Connection = Utilidades.DAOUtilidades.conexion;
            TAListarProductosExistenciaInsuficienteXML = new ListarProductosExistenciaInsuficienteXMLTableAdapter();
            TAListarProductosExistenciaInsuficienteXML.Connection = Utilidades.DAOUtilidades.conexion;
            TAPersonas = new PersonasTableAdapter();
            TAPersonas.Connection = Utilidades.DAOUtilidades.conexion;

            TAAlmacenes = new AlmacenesTableAdapter();
            TAAlmacenes.Connection = Utilidades.DAOUtilidades.conexion;
            
            TransferenciaEstado = new Utilidades.ObjetoCodigoDescripcion();

            TransferenciaEstado.cargarDatosEstadosComprasVentas();
            cBoxEstado.DataSource = TransferenciaEstado.listaObjetos;
            cBoxEstado.DisplayMember = TransferenciaEstado.DisplayMember;
            cBoxEstado.ValueMember = TransferenciaEstado.ValueMember;

            
            DTAlmacenesReceptor = TAAlmacenes.GetData();
            DTAlmacenesReceptor.Rows.Remove(DTAlmacenesReceptor.FindByNumeroAlmacen(NumeroAlmacen));
            DTAlmacenesReceptor.AcceptChanges();
            cBoxAlmacenReceptor.DataSource = DTAlmacenesReceptor;
            cBoxAlmacenReceptor.DisplayMember = "NombreAlmacen";
            cBoxAlmacenReceptor.ValueMember = "NumeroAlmacen";
            cBoxAlmacenReceptor.SelectedIndex = -1;

            DTAlmacenesEmisor = TAAlmacenes.GetData();            
            this.cBoxAlmacenEmisor.DataSource = DTAlmacenesEmisor;
            this.cBoxAlmacenEmisor.DisplayMember = "NombreAlmacen";
            this.cBoxAlmacenEmisor.ValueMember = "NumeroAlmacen";
            this.cBoxAlmacenEmisor.SelectedIndex = -1;

            DTTransferenciaProductosDetalle = new AlvecoComercial10DataSet.ListarTransferenciaProductosDetalleReporteDataTable();
            
            cargarDatosTransferenciaProducto(-1, -1);
            DGCCodigoProducto.ReadOnly = DGCNombreProducto.ReadOnly = true;
            DTProductosSeleccionados.RowChanged += DTProductosSeleccionados_RowChanged;
            DTProductosSeleccionados.RowDeleted += DTProductosSeleccionados_RowChanged;            
            dtGVVentaProductos.CellValidating += dtGVVentaProductos_CellValidating;
            this.cBoxAlmacenEmisor.Enabled = false;
            
            
        }

        void DTProductosSeleccionados_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            object PrecioTotal = DTProductosSeleccionados.Compute("Sum(PrecioTotal)", "");
            txtPrecioTotal.Text = PrecioTotal == null ? "0.00 Bs  " : PrecioTotal.ToString() + " Bs  ";
            //DTProductosSeleccionados.AcceptChanges();
        }

        public void adecuarColumnasVentaTemporal()
        {
            
            DGCCantidadVenta.DataPropertyName = DTProductosSeleccionados.CantidadColumn.ColumnName;
            DGCCantidadVenta.ReadOnly = false;
            DGCPrecioUnitarioVenta.DataPropertyName = DTProductosSeleccionados.PrecioUnitarioColumn.ColumnName;
            DGCPrecioUnitarioVenta.ReadOnly = false;
            DGCTiempoGarantiaVenta.DataPropertyName = DTProductosSeleccionados.TiempoGarantiaColumn.ColumnName;
            DGCTiempoGarantiaVenta.ReadOnly = false;
            DGCPrecioTotal.DataPropertyName = DTProductosSeleccionados.PrecioTotalColumn.ColumnName;
            DGCPrecioTotal.ReadOnly = true;
        }

        public void adecuarColumnasVisualizarVenta()
        {
            
            DGCCantidadVenta.DataPropertyName = "CantidadTransferencia";
            DGCCantidadVenta.ReadOnly = true;
            DGCPrecioUnitarioVenta.DataPropertyName = "PrecioUnitarioTransferencia";
            DGCPrecioUnitarioVenta.ReadOnly = true;
            DGCTiempoGarantiaVenta.DataPropertyName = "TiempoGarantiaVenta";
            DGCTiempoGarantiaVenta.ReadOnly = true;
            DGCPrecioTotal.DataPropertyName = "PrecioTotal";
        }


        public void habilitarControles(bool estadoHabilitacion)
        {            
            this.txtObservaciones.ReadOnly = !estadoHabilitacion;
            this.cBoxAlmacenReceptor.Enabled = estadoHabilitacion;                        
            bindingNavigatorDeleteItem.Visible = estadoHabilitacion;
            this.dtGVVentaProductos.ReadOnly = !estadoHabilitacion;
        }

        public void limpiarControles()
        {            
            this.txtObservaciones.Text = String.Empty;
            this.cBoxAlmacenReceptor.SelectedIndex = -1;
            this.cBoxAlmacenEmisor.SelectedIndex = -1;
            this.tsLblFechaHoraRegistro.Text = String.Empty;
            this.tsLblNroAgencia.Text = String.Empty;
            this.tsLblNroTransferencia.Text = String.Empty;
            this.lblUsuario.Text = String.Empty;
            DTProductosSeleccionados.Clear();
            txtPrecioTotal.Text = "0.00 BS";
            cBoxEstado.SelectedIndex = -1;            
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
            if (DTProductosSeleccionados.Count == 0)
            {
                MessageBox.Show(this, "Aún no ha seleccionado ningún producto", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                errorProvider1.SetError(txtPrecioTotal, "Aún no ha seleccionado ningún producto, El Monto es Cero");
                tabControlVentas.SelectedTab = tabPageVentaDetalle;
                return false;

            }
            if (cBoxAlmacenReceptor.SelectedIndex < 00)
            {
                errorProvider1.SetError(cBoxAlmacenReceptor, "Aún no ha seleccionado el Almacen al cual desea enviar productos");
                cBoxAlmacenReceptor.Focus();
                tabControlVentas.SelectedTab = tabPageVentaDetalle;
                return false;
            }
            if (cBoxAlmacenEmisor.SelectedIndex < 00)
            {
                errorProvider1.SetError(cBoxAlmacenEmisor, "Aún no ha seleccionado el almacén que debe enviar productos");
                cBoxAlmacenEmisor.Focus();
                tabControlVentas.SelectedTab = tabPageVentaDetalle;
                return false;
            }
            return true;
        }

        public void cargarDatosTransferenciaProducto(int NumeroAlmacenEmisor, int NumeroTransfeProducto)
        {
            DTTransferenciaProductos = TATransferenciaProductos.GetDataBy3(NumeroAlmacenEmisor, NumeroTransfeProducto);
            habilitarControles(false);
            if (DTTransferenciaProductos.Count > 0)
            {
                this.NumeroTransferenciaProducto = NumeroTransfeProducto;
                DTTransferenciaProductosDetalle = TATransferenciasProductosDetalleMostrar.GetData(NumeroAlmacenEmisor, NumeroTransfeProducto);
                adecuarColumnasVisualizarVenta();
                bdSourceVentaProductos.DataSource = DTTransferenciaProductosDetalle;


                lblUsuario.Text = Utilidades.DAOUtilidades.ObtenerNombreCompleto(DTTransferenciaProductos[0].DIUsuario);
                cBoxAlmacenReceptor.SelectedValue = DTTransferenciaProductos[0].NumeroAlmacenRecepctor;
                cBoxAlmacenEmisor.SelectedValue = DTTransferenciaProductos[0].NumeroAlmacenEmisor;
                cBoxEstado.SelectedValue = DTTransferenciaProductos[0].CodigoEstadoTransferencia;                
                txtObservaciones.Text = DTTransferenciaProductos[0].IsObservacionesNull() ?
                    String.Empty : DTTransferenciaProductos[0].Observaciones;

                tsLblNroAgencia.Text = "Nro Almacen Emisor :" + NumeroAlmacenEmisor.ToString();
                tsLblNroTransferencia.Text = "Nro Transferencia :" + NumeroTransfeProducto.ToString();
                tsLblFechaHoraRegistro.Text = "Fecha Registro " + DTTransferenciaProductos[0].FechaHoraTransferencia.ToString();
                
                txtPrecioTotal.Text = DTTransferenciaProductosDetalle.Compute("sum(PrecioTotal)", "").ToString() + " Bs  ";
                txtMontoCancelado.Text = txtPrecioTotal.Text;
                
                
                //txtMontoCancelado.Visible = !DTTransferenciaProductos[0].IsNumeroCuentaPorCobrarNull();
                //lblMontoCancelado.Visible = !DTTransferenciaProductos[0].IsNumeroCuentaPorCobrarNull();

                switch (DTTransferenciaProductos[0].CodigoEstadoTransferencia)
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
                DTTransferenciaProductosDetalle.Clear();
            }
            
        }

        private void tsBtnNuevo_Click(object sender, EventArgs e)
        {
            limpiarControles();
            pnlProductosBusqueda.limpiarControles();
            TipoOperacion = "I";
            pnlProductosBusqueda.visualizarPanel();
            habilitarControles(true);
            habilitarBotonesAcciones(false, true, true, true, false, false, false);
            lblUsuario.Text = "Usuario Responsable " + Utilidades.DAOUtilidades.ObtenerNombreCompleto(DIUsuario);
            cBoxEstado.SelectedValue = "I";
            cBoxAlmacenEmisor.SelectedValue = NumeroAlmacen;
            adecuarColumnasVentaTemporal();
            bdSourceVentaProductos.DataSource = DTProductosSeleccionados;
            tsLblFechaHoraRegistro.Text = "Fecha Registro " + Utilidades.DAOUtilidades.ObtenerFechaHoraServidor().ToString();
            tsLblNroAgencia.Text = "Nro Almacen Emisor " + NumeroAlmacen.ToString();
            tsLblNroTransferencia.Text = "Nro Transferencia Producto " + (Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("TransferenciasProductos") + 1).ToString();
            DTTransferenciaProductosDetalle.Clear();
            lblMontoCancelado.Visible = txtMontoCancelado.Visible = false;
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
        }

        private void dtGVVentaProductos_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dtGVVentaProductos.CurrentCell != null)
            {
                if (e.ColumnIndex == DGCCantidadVenta.Index || e.ColumnIndex == DGCPrecioUnitarioVenta.Index)
                {
                    dtGVVentaProductos[DGCPrecioTotal.Name, e.RowIndex].Value = DTProductosSeleccionados[e.RowIndex].Cantidad *
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
            errorProvider1.Clear();
            if (validarDatos())
            {
                DataTable DTProductosSeleccionadosXMLAux = DTProductosSeleccionados.Copy();
                DTProductosSeleccionadosXMLAux.Constraints.Clear();
                DTProductosSeleccionadosXMLAux.Columns.Remove(DTProductosSeleccionadosXMLAux.Columns["NombreProducto"]);
                DTProductosSeleccionadosXMLAux.TableName = "TransferenciasProductosDetalle";
                DTProductosSeleccionadosXMLAux.AcceptChanges();

                DataSet DSProductosSeleccionados;
                DSProductosSeleccionados = new DataSet("TransferenciasProductos");
                DSProductosSeleccionados.Tables.Add(DTProductosSeleccionadosXMLAux);

                string ProductosDetalleXML = DTProductosSeleccionadosXMLAux.DataSet.GetXml();


                decimal MontoTotalVenta = decimal.Parse(DTProductosSeleccionados.Compute("sum(PrecioTotal)", "").ToString());
                decimal MontoPagoEfectivo = 0;


                try
                {                    

                    if (TipoOperacion == "I")
                    {
                        TATransferenciaProductos.InsertarTransferenciaProductoXMLDetalle(int.Parse(cBoxAlmacenEmisor.SelectedValue.ToString()), 
                            int.Parse(cBoxAlmacenReceptor.SelectedValue.ToString()),
                        DIUsuario, null, "I", MontoTotalVenta, txtObservaciones.Text, ProductosDetalleXML);

                        foreach (AlvecoComercial10DataSet.DTProductosSeleccionadosRow DRProductoSeleccionado in DTProductosSeleccionados.Rows)
                        {                            
                            DTTransferenciaProductosDetalle.AddListarTransferenciaProductosDetalleReporteRow(DRProductoSeleccionado.CodigoProducto,
                                DRProductoSeleccionado.NombreProducto, DRProductoSeleccionado.Cantidad, DRProductoSeleccionado.PrecioUnitario,
                                DRProductoSeleccionado.PrecioTotal);
                        }
                        DTTransferenciaProductosDetalle.AcceptChanges();
                        NumeroTransferenciaProducto = Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("TransferenciasProductos");
                    }
                    else
                    {
                        TATransferenciaProductos.ActualizarTransferenciaProductoXMLDetalle(int.Parse(cBoxAlmacenEmisor.SelectedValue.ToString()),
                            NumeroTransferenciaProducto, int.Parse(cBoxAlmacenReceptor.SelectedValue.ToString()),
                        DIUsuario, null, "I", MontoTotalVenta, txtObservaciones.Text, ProductosDetalleXML);


                        DTTransferenciaProductosDetalle.Clear();
                        DTTransferenciaProductosDetalle.AcceptChanges();
                        foreach (AlvecoComercial10DataSet.DTProductosSeleccionadosRow DRProductoSeleccionado in DTProductosSeleccionados.Rows)
                        {
                            DTTransferenciaProductosDetalle.AddListarTransferenciaProductosDetalleReporteRow(DRProductoSeleccionado.CodigoProducto,
                                DRProductoSeleccionado.NombreProducto, DRProductoSeleccionado.Cantidad, DRProductoSeleccionado.PrecioUnitario,
                                DRProductoSeleccionado.PrecioTotal);
                        }
                        DTTransferenciaProductosDetalle.AcceptChanges();
                    }

                    //MessageBox.Show(this, "La operación actual fue registrada correctamente", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    habilitarControles(false);
                    habilitarBotonesAcciones(true, false, false, true, true, true, true);
                    TipoOperacion = "";
                    pnlProductosBusqueda.ocultarPanel();                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Ocurrio la siguiente Excepción: " + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dtGVVentaProductos_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            int CantidadVentada; decimal precioVenta;

            this.dtGVVentaProductos.Rows[e.RowIndex].ErrorText = "";

            // No cell validation for new rows. New rows are validated on Row Validation.
            if (this.dtGVVentaProductos.Rows[e.RowIndex].IsNewRow) { return; }

            if (this.dtGVVentaProductos.IsCurrentCellDirty)
            {
                switch (this.dtGVVentaProductos.Columns[e.ColumnIndex].Name)
                {

                    case "DGCCantidadVenta":
                        if (e.FormattedValue.ToString().Trim().Length < 1)
                        {
                            this.dtGVVentaProductos.Rows[e.RowIndex].ErrorText = "   La Cantidad es necesaria y no puede estar vacia.";
                            e.Cancel = true;
                        }
                        else if (!int.TryParse(e.FormattedValue.ToString(), out CantidadVentada) || CantidadVentada <= 0)
                        {
                            this.dtGVVentaProductos.Rows[e.RowIndex].ErrorText = "   La Cantidad debe ser un entero positivo Mayor a Cero";
                            e.Cancel = true;
                        }
                        //MessageBox.Show("Valor a Validar " + e.FormattedValue.ToString() + ",  Valor Anterior " + this.dtGVVentaProductos.Rows[e.RowIndex].Cells["DGCCantidadExistencia"].Value.ToString());
                        break;

                    case "DGCPrecioUnitarioVenta":
                        if (e.FormattedValue.ToString().Trim().Length < 1)
                        {
                            this.dtGVVentaProductos.Rows[e.RowIndex].ErrorText = "   El Precio Unitario de Venta es necesario y no puede estar vacio.";
                            e.Cancel = true;
                        }
                        else if (!decimal.TryParse(e.FormattedValue.ToString(), out precioVenta) || precioVenta <= 0)
                        {
                            this.dtGVVentaProductos.Rows[e.RowIndex].ErrorText = "   El Precio Unitario de Venta debe ser un valor positivo Mayor a Cero";
                            e.Cancel = true;
                        }
                        //MessageBox.Show("Valor a Validar " + e.FormattedValue.ToString() + ",  Valor Anterior " + this.dtGVVentaProductos.Rows[e.RowIndex].Cells["DGCCantidadExistencia"].Value.ToString());
                        break;


                }

            }
        }

        private void tsBtnModificar_Click(object sender, EventArgs e)
        {
            TipoOperacion = "E";
            cargarProductosSeleccionados(DTTransferenciaProductosDetalle);
            habilitarBotonesAcciones(false, true, true, false, false, false, false);
            pnlProductosBusqueda.visualizarPanel();
            habilitarControles(true);
            bdSourceVentaProductos.DataSource = DTProductosSeleccionados;
            DTProductosSeleccionados.RowChanged += DTProductosSeleccionados_RowChanged;
            DTProductosSeleccionados.RowDeleted += DTProductosSeleccionados_RowChanged;
            adecuarColumnasVentaTemporal();            
        }


        public void cargarProductosSeleccionados(DataTable DTTransferenciasProductosDetalle)
        {
            pnlProductosBusqueda.limpiarControles();
            foreach (DataRow DRProductosDetalle in DTTransferenciasProductosDetalle.Rows)
            {
                DTProductosSeleccionados.AddDTProductosSeleccionadosRow(
                    DRProductosDetalle["CodigoProducto"].ToString(),
                    DRProductosDetalle["NombreProducto"].ToString(),
                    int.Parse(DRProductosDetalle["CantidadTransferencia"].ToString()),
                    decimal.Parse(DRProductosDetalle["PrecioUnitarioTransferencia"].ToString()),
                    Utilidades.DAOUtilidades.ObtenerCantidadExistenciaProducto(NumeroAlmacen, DRProductosDetalle["CodigoProducto"].ToString()),
                    0, " ",
                    //int.Parse(DRProductosDetalle["CantidadVenta"].ToString()) * decimal.Parse(DRProductosDetalle["PrecioUnitarioVenta"].ToString()),
                    decimal.Parse(DRProductosDetalle["PrecioTotal"].ToString()),
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
                    TATransferenciaProductos.Delete(NumeroAlmacen, NumeroTransferenciaProducto);
                    //MessageBox.Show(this, "Registro eliminado correctamente", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cargarDatosTransferenciaProducto(NumeroAlmacen, -1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Ocurrió la siguiente Excepcion " + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void tsBtnFinalizar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "¿Se encuentra seguro de Culminar completamente la Transferencia de Productos?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == System.Windows.Forms.DialogResult.No)
            {
                //TAVentaProductos.ActualizarCodigoEstadoVenta(NumeroAlmacen, NumeroVentaProducto, "F");
                //cBoxEstado.SelectedValue = "F";
                //habilitarBotonesAcciones(true, false, false, false, false, false, true);
                //MessageBox.Show(this, "Venta Culminada y Recepcionada Correctamente", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }
            try
            {
                    

                if (cBoxEstado.SelectedValue != null && cBoxEstado.SelectedValue.Equals("I"))
                {
                    
                        //INICIO
                        if (MessageBox.Show(this, "La Finalización de la Transacción actual implica la Entrega Total de todos los Productos Solicitados por almacen correspondiente "
                            + "del almacen "+ (cBoxAlmacenEmisor.SelectedItem as DataRowView)["NombreAlmacen"].ToString() +", para su posterior actualización en Existencia\r\n¿Desea Finalizar la Venta de Productos?",
                            "Finalización de Transferencia de Productos", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question) == DialogResult.Yes)
                        {

                            if(!validarFinalizacionVenta())
                            {
                                TATransferenciaProductos.ActualizarCodigoEstadoTransferencia(int.Parse(cBoxAlmacenEmisor.SelectedValue.ToString()), NumeroTransferenciaProducto, "F");
                                TATransferenciaProductos.ActualizarInventarioTransferenciasProductos(int.Parse(cBoxAlmacenEmisor.SelectedValue.ToString()), NumeroTransferenciaProducto);

                                cBoxEstado.SelectedValue = "F";
                                habilitarBotonesAcciones(true, false, false, false, false, false, true);
                                habilitarControles(false);
                            }
                        }

                        //MessageBox.Show(this, "Venta Culminada y Entregada Correctamente", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "No se Pudo Realizar la Operación Actual, ocurrió la Siguiente Excepción \r\n" + ex.Message,
                    "Error en Finalización de Ingresos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            
        }

        public bool validarFinalizacionVenta()
        {
           
                string ListadoProductos = "";
                int NumeroAlmacenEmisor = int.Parse(cBoxAlmacenEmisor.SelectedValue.ToString());
                AlvecoComercial10DataSet.ListarProductosTransferidosSuperaStockMinimoDataTable DTListarProductosCantidadSuperaStockMinimo =
                    TAListarProductosCantidadSuperaStockMinimo.GetData(NumeroAlmacenEmisor, NumeroTransferenciaProducto);
                if (DTListarProductosCantidadSuperaStockMinimo.Count > 0)
                {

                    foreach (AlvecoComercial10DataSet.ListarProductosTransferidosSuperaStockMinimoRow
                        DRArticulo in DTListarProductosCantidadSuperaStockMinimo.Rows)
                        ListadoProductos += "\r\n" + DRArticulo.NombreProducto;

                    if (MessageBox.Show(this, "La siguiente Lista de Productos supera la cantidad de Stock Minimo en inventarios" +
                        ListadoProductos + "\r\n¿Desea Continuar la transacción?",
                        "Stock Mínimo en Posible riesgo de ser Superado",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return true;
                }


                ListadoProductos = string.Empty;
                AlvecoComercial10DataSet.ListarProductosTransferenciaInsuficienteDataTable DTListarProductosExistenciaInsuficiente
                    = TAListarProductosExistenciaInsuficiente.GetData(NumeroAlmacenEmisor, NumeroTransferenciaProducto);
                if (DTListarProductosExistenciaInsuficiente.Count > 0)
                {
                    foreach (AlvecoComercial10DataSet.ListarProductosExistenciaInsuficienteRow
                        DRArticulo in DTListarProductosExistenciaInsuficiente.Rows)
                    {
                        ListadoProductos += "\r\n" + DRArticulo.NombreProducto;
                    }

                    MessageBox.Show(this, "La Siguiente Lista de Productos supera la Cantidad de Existencia en Almacenes " +
                        ListadoProductos + "\r\nLa Operación actual no puede continuar, debido a la existencia insuficiente de los productos solicitados dentro Almacenes",
                        "Existencia Insuficiente en Inventarios para Abastecer Solicitud",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }
            
            return false;
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
            int NumeroAlmacenEmisor = int.Parse(cBoxAlmacenEmisor.SelectedValue.ToString());
            ListarTransferenciaProductosReporteTableAdapter TATransferenciasProductoReporte = new ListarTransferenciaProductosReporteTableAdapter();
            TATransferenciasProductoReporte.Connection = Utilidades.DAOUtilidades.conexion;
            DataTable DTIngresoArticuloR = TATransferenciasProductoReporte.GetData(NumeroAlmacenEmisor, NumeroTransferenciaProducto);
            DataTable DTIngresoArticuloDetalleR = TATransferenciasProductosDetalleMostrar.GetData(NumeroAlmacenEmisor, NumeroTransferenciaProducto);
            FReporteTransaccionesGeneral formReporte = new FReporteTransaccionesGeneral();
            formReporte.cargarReporteTransferenciasProductos(DTIngresoArticuloR, DTIngresoArticuloDetalleR);
            formReporte.ShowDialog(this);
            formReporte.Dispose();
        }

    }
}
