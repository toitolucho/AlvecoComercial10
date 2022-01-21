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
    public partial class FVentasProductos : Form
    {
        private string DIUsuario;
        private int NumeroAlmacen;
        private AlvecoComercial10DataSet.DTProductosSeleccionadosDataTable DTProductosSeleccionados;
        private VentasProductosTableAdapter TAVentaProductos;
        private AlvecoComercial10DataSet.VentasProductosDataTable DTVentaProductos;
        private VentasProductosDetalleTableAdapter TAVentasProductosDetalle;
        private ListarVentasProductosDetalleParaMostrarTableAdapter TAVentasProductosDetalleMostrar;        
        private ListarProductosDistribucionFaltantesTableAdapter TAListarProductosDistribucionFaltantes;
        private ListarProductosCantidadSuperaStockMinimoTableAdapter TAListarProductosCantidadSuperaStockMinimo;
        private ListarProductosExistenciaInsuficienteTableAdapter TAListarProductosExistenciaInsuficiente;
        private VentasProductosDetalleEntregaTableAdapter TAVentasProductosDetalleEntrega;
        private ListarProductosCantidadSuperaStockMinimoXMLTableAdapter TAListarProductosCantidadSuperaStockMinimoXML;
        private ListarProductosExistenciaInsuficienteXMLTableAdapter TAListarProductosExistenciaInsuficienteXML;
        private AccesoDatos.AlvecoComercial10DataSet.PersonasDataTable DTPersonas;
        private AccesoDatos.AlvecoComercial10DataSet.MovilidadesDataTable DTMovilidades;


        

        
        private AlvecoComercial10DataSet.ListarVentasProductosDetalleParaMostrarDataTable DTVentaProductosDetalle;
        private ClientesTableAdapter TAClientes;
        private AlvecoComercial10DataSet.ClientesDataTable DTClientes;
        private AlvecoComercial10DataSet.ListarProductosDistribucionFaltantesDataTable DTProductosDistribucion;
        private PersonasTableAdapter TAPersonas;
        private MovilidadesTableAdapter TAMovilidades;
        private Utilidades.ObjetoCodigoDescripcion VentasTipos;
        private Utilidades.ObjetoCodigoDescripcion VentasEstado;
        private Utilidades.ObjetoCodigoDescripcion MotivosVentas;
        int? NumeroCuentaPorCobrar = null;

        public int NumeroVentaProducto { get; set; }
        private string TipoOperacion = "";

        public FVentasProductos(string DIUsuario, int NumeroAlmacen)
        {
            InitializeComponent();

            this.pnlProductosBusqueda = new SAlvecoComercial10.Formularios.Utilidades.PanelBusquedaProductos(DTProductosSeleccionados,
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
            DTVentaProductosDetalle = new AlvecoComercial10DataSet.ListarVentasProductosDetalleParaMostrarDataTable();
            DTProductosDistribucion = new AlvecoComercial10DataSet.ListarProductosDistribucionFaltantesDataTable();
            //this.pnlProductosBusqueda = new SAlvecoComercial10.Formularios.Utilidades.PanelBusquedaProductos(DTProductosSeleccionados,
            //    NumeroAlmacen, DIUsuario, "C");
            //// 
            //// pnlProductosBusqueda
            //// 
            //this.pnlProductosBusqueda.Dock = System.Windows.Forms.DockStyle.Bottom;
            //this.pnlProductosBusqueda.Location = new System.Drawing.Point(69, 334);
            //this.pnlProductosBusqueda.Name = "pnlProductosBusqueda";
            //this.pnlProductosBusqueda.Size = new System.Drawing.Size(655, 172);
            //this.pnlProductosBusqueda.TabIndex = 3;
            //this.Controls.Add(this.pnlProductosBusqueda);

            this.pnlProductosBusqueda.cargarParametrosConstructor(DTProductosSeleccionados,
                NumeroAlmacen, DIUsuario, "V");
            this.pnlProductosBusqueda.ocultarPanel();
            TAVentaProductos = new VentasProductosTableAdapter();
            TAVentaProductos.Connection = Utilidades.DAOUtilidades.conexion;
            TAVentasProductosDetalle = new VentasProductosDetalleTableAdapter();
            TAVentasProductosDetalle.Connection = Utilidades.DAOUtilidades.conexion;
            TAVentasProductosDetalleMostrar = new ListarVentasProductosDetalleParaMostrarTableAdapter();
            TAVentasProductosDetalleMostrar.Connection = Utilidades.DAOUtilidades.conexion;
            TAListarProductosDistribucionFaltantes = new ListarProductosDistribucionFaltantesTableAdapter();
            TAListarProductosDistribucionFaltantes.Connection = Utilidades.DAOUtilidades.conexion;
            TAListarProductosCantidadSuperaStockMinimo = new ListarProductosCantidadSuperaStockMinimoTableAdapter();
            TAListarProductosCantidadSuperaStockMinimo.Connection = Utilidades.DAOUtilidades.conexion;
            TAListarProductosExistenciaInsuficiente = new ListarProductosExistenciaInsuficienteTableAdapter();
            TAListarProductosExistenciaInsuficiente.Connection = Utilidades.DAOUtilidades.conexion;
            TAVentasProductosDetalleEntrega = new VentasProductosDetalleEntregaTableAdapter();
            TAVentasProductosDetalleEntrega.Connection = Utilidades.DAOUtilidades.conexion;
            TAListarProductosCantidadSuperaStockMinimoXML = new ListarProductosCantidadSuperaStockMinimoXMLTableAdapter();
            TAListarProductosCantidadSuperaStockMinimoXML.Connection = Utilidades.DAOUtilidades.conexion;
            TAListarProductosExistenciaInsuficienteXML = new ListarProductosExistenciaInsuficienteXMLTableAdapter();
            TAListarProductosExistenciaInsuficienteXML.Connection = Utilidades.DAOUtilidades.conexion;
            TAPersonas = new PersonasTableAdapter();
            TAPersonas.Connection = Utilidades.DAOUtilidades.conexion;
            TAMovilidades = new MovilidadesTableAdapter();
            TAMovilidades.Connection = Utilidades.DAOUtilidades.conexion;

            TAClientes = new ClientesTableAdapter();
            TAClientes.Connection = Utilidades.DAOUtilidades.conexion;
            VentasTipos = new Utilidades.ObjetoCodigoDescripcion();
            VentasEstado = new Utilidades.ObjetoCodigoDescripcion();
            MotivosVentas = new Utilidades.ObjetoCodigoDescripcion();

            VentasEstado.cargarDatosEstadosComprasVentas();
            cBoxEstado.DataSource = VentasEstado.listaObjetos;
            cBoxEstado.DisplayMember = VentasEstado.DisplayMember;
            cBoxEstado.ValueMember = VentasEstado.ValueMember;

            VentasTipos.cargarDatosTiposComprasVentas();
            cBoxTipoVenta.DataSource = VentasTipos.listaObjetos;
            cBoxTipoVenta.DisplayMember = VentasTipos.DisplayMember;
            cBoxTipoVenta.ValueMember = VentasTipos.ValueMember;

            MotivosVentas.cargarMotivosVentas();
            cBoxMotivoVenta.DataSource = MotivosVentas.listaObjetos;
            cBoxMotivoVenta.DisplayMember = MotivosVentas.DisplayMember;
            cBoxMotivoVenta.ValueMember = MotivosVentas.ValueMember;

            DTClientes = TAClientes.GetDataByActivos();
            cBoxCliente.DataSource = DTClientes;
            cBoxCliente.DisplayMember = "NombreCliente";
            cBoxCliente.ValueMember = "CodigoCliente";

            DTPersonas = TAPersonas.GetDataByParticulares(null);
            this.cboxPersonas.DataSource = DTPersonas;
            this.cboxPersonas.DisplayMember = "NombreCompleto";
            this.cboxPersonas.ValueMember = "DIPersona";
            this.cboxPersonas.SelectedIndex = -1;

            DTMovilidades = TAMovilidades.GetData();
            this.cBoxMovilidades.DataSource = DTMovilidades;
            this.cBoxMovilidades.DisplayMember = "NombreMovilidad";
            this.cBoxMovilidades.ValueMember = "CodigoMovilidad";
            this.cBoxMovilidades.SelectedIndex = -1;

            tabControlVentas.TabPages.Remove(tabPageVentaDistribuible);

            cargarDatosVentaProducto(-1);


            DTProductosSeleccionados.RowChanged += DTProductosSeleccionados_RowChanged;
            DTProductosSeleccionados.RowDeleted += DTProductosSeleccionados_RowChanged;
            dtGVProductosListado.CellValidating += new DataGridViewCellValidatingEventHandler(dtGVProductosListado_CellValidating);
            dtGVVentaProductos.CellValidating += dtGVVentaProductos_CellValidating;
            dtGVProductosListado.ReadOnly = false;
            
        }

        void dtGVProductosListado_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            int CantidadNuevaDeEntrega;

            this.dtGVProductosListado.Rows[e.RowIndex].ErrorText = "";

            // No cell validation for new rows. New rows are validated on Row Validation.
            if (this.dtGVProductosListado.Rows[e.RowIndex].IsNewRow) { return; }

            if (this.dtGVProductosListado.IsCurrentCellDirty)
            {
                switch (this.dtGVProductosListado.Columns[e.ColumnIndex].Name)
                {

                    case "DGCCantidadRecepcionada":
                        if (e.FormattedValue.ToString().Trim().Length < 1)
                        {
                            this.dtGVProductosListado.Rows[e.RowIndex].ErrorText = "   La Cantidad a Distribuir es necesaria y no puede estar vacia.";
                            e.Cancel = true;
                        }
                        else if (!int.TryParse(e.FormattedValue.ToString(), out CantidadNuevaDeEntrega) || CantidadNuevaDeEntrega <= 0)
                        {
                            this.dtGVProductosListado.Rows[e.RowIndex].ErrorText = "   La Cantidad a Distribuir debe ser un entero positivo.";
                            e.Cancel = true;
                        }

                        if (int.TryParse(e.FormattedValue.ToString(), out CantidadNuevaDeEntrega))
                        {
                            int CantidadMaximaEntrega = DTProductosDistribucion[e.RowIndex].CantidadFaltante;
                            int existenciaInventario = SAlvecoComercial10.Formularios.Utilidades.DAOUtilidades.ObtenerExistenciaActualInventarios(NumeroAlmacen, DTProductosDistribucion[e.RowIndex].CodigoProducto);
                            if (CantidadNuevaDeEntrega <= CantidadMaximaEntrega)
                            {

                                if (CantidadNuevaDeEntrega > existenciaInventario)
                                {
                                    this.dtGVProductosListado.Rows[e.RowIndex].ErrorText = "   No puede entregar una cantidad que no puede Ser abastecida por Almacenes";
                                    e.Cancel = true;
                                }


                            }
                            else
                            {
                                this.dtGVProductosListado.Rows[e.RowIndex].ErrorText = "   No puede entregar una cantidad superior a la Cantidad Vendida.";
                                e.Cancel = true;
                            }
                        }

                        //MessageBox.Show("Valor a Validar " + e.FormattedValue.ToString() + ",  Valor Anterior " + this.dtGVProductosListado.Rows[e.RowIndex].Cells["DGCCantidadExistencia"].Value.ToString());
                        break;


                }

            }
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
            DGCCantidadEntregada.Visible = false;
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
            DGCCantidadEntregada.Visible = true;
            DGCCantidadVenta.DataPropertyName = "CantidadVenta";
            DGCCantidadVenta.ReadOnly = true;
            DGCPrecioUnitarioVenta.DataPropertyName = "PrecioUnitarioVenta";
            DGCPrecioUnitarioVenta.ReadOnly = true;
            DGCTiempoGarantiaVenta.DataPropertyName = "TiempoGarantiaVenta";
            DGCTiempoGarantiaVenta.ReadOnly = true;
            DGCPrecioTotal.DataPropertyName = "PrecioTotal";
        }


        public void habilitarControles(bool estadoHabilitacion)
        {
            this.txtNroComprobante.ReadOnly = !estadoHabilitacion;
            this.txtNroFactura.ReadOnly = !estadoHabilitacion;
            this.txtObservaciones.ReadOnly = !estadoHabilitacion;
            this.cBoxCliente.Enabled = estadoHabilitacion;
            this.cBoxTipoVenta.Enabled = estadoHabilitacion;
            this.btnAgregarProveedor.Enabled = estadoHabilitacion;
            this.checkDistribuible.Enabled = estadoHabilitacion;
            this.cboxPersonas.Enabled = this.cBoxMovilidades.Enabled = estadoHabilitacion;
            btnAgregarPersonas.Enabled = btnAgregarMovilidad.Enabled = estadoHabilitacion;
            bindingNavigatorDeleteItem.Visible = estadoHabilitacion;
            cBoxMotivoVenta.Enabled = estadoHabilitacion;
            //this.dtGVVentaProductos.ReadOnly = !estadoHabilitacion;
        }

        public void limpiarControles()
        {
            this.txtNroComprobante.Text = String.Empty;
            this.txtNroFactura.Text = String.Empty;
            this.txtObservaciones.Text = String.Empty;
            this.cBoxCliente.SelectedIndex = -1;
            this.cBoxTipoVenta.SelectedIndex = -1;
            this.tsLblFechaHoraRecepcion.Text = String.Empty;
            this.tsLblFechaHoraRegistro.Text = String.Empty;
            this.tsLblNroAgencia.Text = String.Empty;
            this.tsLblNroVenta.Text = String.Empty;
            this.lblUsuario.Text = String.Empty;
            cBoxMotivoVenta.SelectedIndex = -1;
            DTProductosSeleccionados.Clear();
            DTProductosDistribucion.Clear();
            txtPrecioTotal.Text = "0.00 BS";
            checkDistribuible.Checked = false;
            cBoxEstado.SelectedIndex = -1;
            cboxPersonas.SelectedIndex = cBoxMovilidades.SelectedIndex = -1;
        }

        public void habilitarBotonesAcciones(bool nuevo, bool aceptar, bool cancelar, bool modificar, bool anular, bool finalizar, bool reportes, bool eliminar)
        {
            this.tsBtnNuevo.Enabled = nuevo;
            this.tsBtnAceptar.Enabled = aceptar;
            this.tsBtnCancelar.Enabled = cancelar;
            this.tsBtnModificar.Enabled = modificar;
            this.tsBtnAnular.Enabled = anular;
            this.tsBtnFinalizar.Enabled = finalizar;
            this.tsBtnReportes.Enabled = reportes;
            this.tsBtnEliminar.Enabled = eliminar;
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
            if (cBoxCliente.SelectedIndex < 00)
            {
                errorProvider1.SetError(cBoxCliente, "Aún no ha seleccionado el Proveedor");
                cBoxCliente.Focus();
                tabControlVentas.SelectedTab = tabPageVentaDetalle;
                return false;
            }
            if (cBoxTipoVenta.SelectedIndex < 00)
            {
                errorProvider1.SetError(cBoxTipoVenta, "Aún no ha seleccionado El tipo de Venta");
                cBoxTipoVenta.Focus();
                tabControlVentas.SelectedTab = tabPageVentaDetalle;
                return false;
            }
            return true;
        }

        public void cargarDatosVentaProducto(int NumeroVentaProducto)
        {
            DTVentaProductos = TAVentaProductos.GetDataBy21(NumeroAlmacen, NumeroVentaProducto);
            habilitarControles(false);
            if (DTVentaProductos.Count > 0)
            {
                this.NumeroVentaProducto = NumeroVentaProducto;
                DTVentaProductosDetalle = TAVentasProductosDetalleMostrar.GetData(NumeroAlmacen, NumeroVentaProducto);
                adecuarColumnasVisualizarVenta();
                bdSourceVentaProductos.DataSource = DTVentaProductosDetalle;


                lblUsuario.Text = Utilidades.DAOUtilidades.ObtenerNombreCompleto(DTVentaProductos[0].DIUsuario);
                cBoxCliente.SelectedValue = DTVentaProductos[0].CodigoCliente;
                cBoxTipoVenta.SelectedValue = DTVentaProductos[0].CodigoTipoVenta;
                cBoxEstado.SelectedValue = DTVentaProductos[0].CodigoEstadoVenta;
                txtNroComprobante.Text = DTVentaProductos[0].IsNumeroComprobanteNull() ?
                    String.Empty : DTVentaProductos[0].NumeroComprobante;
                txtNroFactura.Text = DTVentaProductos[0].IsNumeroFacturaNull() ?
                    String.Empty : DTVentaProductos[0].NumeroFactura;
                txtObservaciones.Text = DTVentaProductos[0].IsObservacionesNull() ?
                    String.Empty : DTVentaProductos[0].Observaciones;

                tsLblNroAgencia.Text = "Nro almacen :" + NumeroAlmacen.ToString();
                tsLblNroVenta.Text = "Nro Venta :" + NumeroVentaProducto.ToString();
                tsLblFechaHoraRegistro.Text = "Fecha Registro " + DTVentaProductos[0].FechaHoraVenta.ToString();
                tsLblFechaHoraRecepcion.Text = !DTVentaProductos[0].IsFechaHoraEntregaNull() ? "Fecha Recepcion " + DTVentaProductos[0].FechaHoraEntrega
                    : String.Empty;

                NumeroCuentaPorCobrar = DTVentaProductos[0].IsNumeroCuentaPorCobrarNull() ? null : (int?)DTVentaProductos[0].NumeroCuentaPorCobrar;
                txtPrecioTotal.Text = DTVentaProductosDetalle.Compute("sum(PrecioTotal)", "").ToString() + " Bs  ";
                txtMontoCancelado.Text = DTVentaProductos[0].MontoTotalPagoEfectivo.ToString() + " Bs  ";
                checkDistribuible.Checked = !DTVentaProductos[0].IsVentaParaDistribuirNull() && DTVentaProductos[0].VentaParaDistribuir;
                if (DTVentaProductos[0].IsCodigoMovilidadNull())
                    cBoxMovilidades.SelectedIndex = -1;
                else
                    cBoxMovilidades.SelectedValue = DTVentaProductos[0].CodigoMovilidad;
                if (DTVentaProductos[0].IsDIPersonaDistribuidorNull())
                    cboxPersonas.SelectedIndex = -1;
                else
                    cboxPersonas.SelectedValue = DTVentaProductos[0].DIPersonaDistribuidor;

                txtMontoCancelado.Visible = !DTVentaProductos[0].IsNumeroCuentaPorCobrarNull();
                lblMontoCancelado.Visible = !DTVentaProductos[0].IsNumeroCuentaPorCobrarNull();
                if (!DTVentaProductos[0].IsCodigoMotivoVentaNull())
                    cBoxMotivoVenta.SelectedValue = DTVentaProductos[0].CodigoMotivoVenta;
                else
                    cBoxMotivoVenta.SelectedIndex = -1;

                switch (DTVentaProductos[0].CodigoEstadoVenta)
                {
                    case "I":
                        habilitarBotonesAcciones(true, false, false, true, true, true, true, true);
                        break;
                    case "F":
                        habilitarBotonesAcciones(true, false, false, false, false, false, true, false);
                        break;
                    case "A":
                        habilitarBotonesAcciones(true, false, false, false, false, false, true, false);
                        break;
                    case "D":
                        habilitarBotonesAcciones(true, false, false, false, false, true, true, false);
                        break;
                    case "X":
                        habilitarBotonesAcciones(true, false, false, false, false, false, false, false);
                        break;
                    default:
                        habilitarBotonesAcciones(true, false, false, false, false, false, false, false);
                        break;
                }


                if (checkDistribuible.Checked)
                {
                    DTProductosDistribucion = TAListarProductosDistribucionFaltantes.GetData(NumeroAlmacen, NumeroVentaProducto);
                    dtGVProductosListado.AutoGenerateColumns = false;
                    dtGVProductosListado.DataSource = DTProductosDistribucion;

                    if (!tabControlVentas.Contains(tabPageVentaDistribuible))
                    {
                        tabControlVentas.TabPages.Add(tabPageVentaDistribuible);
                    }
                    
                    DGCCantidadRecepcionada.ReadOnly = cBoxEstado.SelectedValue.ToString().CompareTo("I") != 0;
                    cboxPersonas.Enabled = cBoxMovilidades.Enabled = btnAgregarMovilidad.Enabled = btnAgregarPersonas.Enabled=
                        cBoxEstado.SelectedValue.ToString().CompareTo("I") == 0;
                }
                else
                {
                    if (tabControlVentas.Contains(tabPageVentaDistribuible))
                    {
                        tabControlVentas.TabPages.Remove(tabPageVentaDistribuible);
                    }
                }

                
            }
            else
            {
                limpiarControles();
                habilitarBotonesAcciones(true, false, false, false, false, false, false, false);
                NumeroCuentaPorCobrar = null;
                DTVentaProductosDetalle.Clear();
            }
            
        }

        private void tsBtnNuevo_Click(object sender, EventArgs e)
        {
            limpiarControles();
            pnlProductosBusqueda.limpiarControles();
            TipoOperacion = "I";
            pnlProductosBusqueda.visualizarPanel();
            habilitarControles(true);
            habilitarBotonesAcciones(false, true, true, true, false, false, false, false);
            lblUsuario.Text = "Usuario Responsable " + Utilidades.DAOUtilidades.ObtenerNombreCompleto(DIUsuario);
            cBoxEstado.SelectedValue = "I";
            cBoxTipoVenta.SelectedValue = "C";
            adecuarColumnasVentaTemporal();
            bdSourceVentaProductos.DataSource = DTProductosSeleccionados;
            tsLblFechaHoraRegistro.Text = "Fecha Registro " + Utilidades.DAOUtilidades.ObtenerFechaHoraServidor().ToString();
            tsLblNroAgencia.Text = "Nro Almacen " + NumeroAlmacen.ToString();
            tsLblNroVenta.Text = "Nro Venta Producto " + (Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("VentasProductos") + 1).ToString();
            DTVentaProductosDetalle.Clear();
            NumeroCuentaPorCobrar = null;
            lblMontoCancelado.Visible = txtMontoCancelado.Visible = false;
            tabControlVentas.TabPages.Remove(tabPageVentaDistribuible);
            cBoxMotivoVenta.SelectedIndex = 0;
                 
        }

        private void tsBtnCancelar_Click(object sender, EventArgs e)
        {
            TipoOperacion = "";
            errorProvider1.Clear();
            pnlProductosBusqueda.ocultarPanel();
            limpiarControles();
            habilitarControles(false);
            habilitarBotonesAcciones(true, false, false, false, false, false, false, false);
            adecuarColumnasVisualizarVenta();
        }

        private void btnAgregarProveedor_Click(object sender, EventArgs e)
        {
            Formularios.GestionSistema.FClientes formClientes = new GestionSistema.FClientes();
            formClientes.configurarFormularioIA(-1);
            if (formClientes.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
                DTClientes.FindByCodigoCliente(formClientes.CodigoCliente) == null
                && TAClientes.GetDataBy1(formClientes.CodigoCliente).Count > 0)
            {
                DTClientes.Rows.Add(TAClientes.GetDataBy1(formClientes.CodigoCliente)[0].ItemArray);
                DTClientes.DefaultView.Sort = "NombreCliente ASC";
                cBoxCliente.SelectedValue = formClientes.CodigoCliente;
            }
            formClientes.Dispose();
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
                DTProductosSeleccionadosXMLAux.TableName = "VentasProductosDetalle";
                DTProductosSeleccionadosXMLAux.AcceptChanges();

                DataSet DSProductosSeleccionados;
                DSProductosSeleccionados = new DataSet("VentasProductos");
                DSProductosSeleccionados.Tables.Add(DTProductosSeleccionadosXMLAux);

                string ProductosDetalleXML = DTProductosSeleccionadosXMLAux.DataSet.GetXml();


                decimal MontoTotalVenta = decimal.Parse(DTProductosSeleccionados.Compute("sum(PrecioTotal)", "").ToString());
                decimal MontoPagoEfectivo = 0;

                if (DTProductosSeleccionados.Select("PrecioTotal = 0").Length > 0
                    && MessageBox.Show(this, "Existen articulos cuyos precios son equivalentes a cero. ¿Se encuentra seguro de continuar en este estado con la Transacción?",
                    this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                if (MontoTotalVenta <= 0 && MessageBox.Show(this,"¿Se encuentra seguro de realizar la Transaccion con un Precio Total equivalente a cero?",
                    this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                {
                    errorProvider1.SetError(lblTotalTransaccion, "El Precio Total Equivalente es Cero");
                    return;
                }

                try
                {
                    //en una venta distribuible no se pueden registrar cantidades que superan la existencia en inventario
                    if (checkDistribuible.Checked)                    
                    {
                        DTProductosDistribucion.Clear();
                        foreach (AlvecoComercial10DataSet.DTProductosSeleccionadosRow DRProducto in DTProductosSeleccionados.Rows)
                            DTProductosDistribucion.Rows.Add(new Object[] { DRProducto.CodigoProducto, DRProducto.NombreProducto, DRProducto.Cantidad, DRProducto.Cantidad, 0, false, DRProducto.PrecioUnitario });
                        if(validarFinalizacionVenta())
                            return;

                        DTProductosDistribucion.Clear();
                    }


                    if (cBoxTipoVenta.SelectedValue.ToString() == "R")//Si la Venta es a credito
                    {
                        Utilidades.FIngresarCantidad formIngresarMonto = new Utilidades.FIngresarCantidad(true, "Ingrese el Monto de Pago", true);
                        formIngresarMonto.MontoTopeMaximo = MontoTotalVenta;

                        if (formIngresarMonto.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                        {
                            MessageBox.Show(this, "No ha ingresado el Monto que desea Cobrar en el Credito");
                            return;
                        }
                        MontoPagoEfectivo = formIngresarMonto.MontoDecimalIngresado;
                        lblMontoCancelado.Visible = txtMontoCancelado.Visible = true;
                        txtMontoCancelado.Text = MontoPagoEfectivo.ToString() + " Bs  ";

                        if (NumeroCuentaPorCobrar == null)
                        {
                            FCuentasPorCobrarIA formCuentasPorCobrar = new FCuentasPorCobrarIA(NumeroAlmacen, DIUsuario);
                            formCuentasPorCobrar.configurarFormularioIA(null);
                            formCuentasPorCobrar.cargarDatosMontoTotalVenta(MontoTotalVenta - MontoPagoEfectivo, int.Parse(cBoxCliente.SelectedValue.ToString()),
                                "La Cuenta Por Cobrar corresponde a la Orden de Venta de Productos " + (Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("VentasProductos") + 1).ToString());
                            if (formCuentasPorCobrar.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                            {
                                MessageBox.Show(this, "No puede Ingresar una Venta a Credito sin primero haber registrado la Cuenta por Cobrar");
                                //lblMontoCancelado.Visible = txtMontoCancelado.Visible = false;
                                txtMontoCancelado.Text = MontoTotalVenta.ToString() + " Bs  ";
                                return;
                            }
                            NumeroCuentaPorCobrar = formCuentasPorCobrar.NumeroCuentaPorCobrar;
                            formCuentasPorCobrar.Dispose();
                        }
                    }

                    

                    if (TipoOperacion == "I")
                    {
                        TAVentaProductos.InsertarVentaProductoXMLDetalle(NumeroAlmacen, int.Parse(cBoxCliente.SelectedValue.ToString()),
                        DIUsuario, null, null, txtNroComprobante.Text, "I", cBoxTipoVenta.SelectedValue.ToString(), 
                        cBoxMotivoVenta.SelectedValue.ToString(), txtNroFactura.Text, 
                        MontoTotalVenta, MontoPagoEfectivo, 0, NumeroCuentaPorCobrar, checkDistribuible.Checked, 
                        cBoxMovilidades.SelectedIndex >= 0 ? cBoxMovilidades.SelectedValue.ToString() :null,
                        cboxPersonas.SelectedIndex >= 0 ? cboxPersonas.SelectedValue.ToString() : null,
                        txtObservaciones.Text, ProductosDetalleXML);

                        foreach (AlvecoComercial10DataSet.DTProductosSeleccionadosRow DRProductoSeleccionado in DTProductosSeleccionados.Rows)
                        {
                            DTVentaProductosDetalle.AddListarVentasProductosDetalleParaMostrarRow(DRProductoSeleccionado.CodigoProducto,
                                DRProductoSeleccionado.NombreProducto, DRProductoSeleccionado.Cantidad, DRProductoSeleccionado.PrecioUnitario,
                                DRProductoSeleccionado.PrecioTotal, DRProductoSeleccionado.TiempoGarantia, 0, DRProductoSeleccionado.MarcaProducto, DRProductoSeleccionado.MarcaProducto, 0);
                        }
                        DTVentaProductosDetalle.AcceptChanges();
                        NumeroVentaProducto = Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("VentasProductos");
                    }
                    else
                    {
                        NumeroCuentaPorCobrar = cBoxTipoVenta.SelectedValue.ToString() == "E" ? null : NumeroCuentaPorCobrar;

                        TAVentaProductos.ActualizarVentaProductoXMLDetalle(NumeroAlmacen, NumeroVentaProducto, int.Parse(cBoxCliente.SelectedValue.ToString()),
                        DIUsuario, null, null, txtNroComprobante.Text, "I", cBoxTipoVenta.SelectedValue.ToString(), 
                        cBoxMotivoVenta.SelectedValue.ToString(),txtNroFactura.Text,
                        MontoTotalVenta, MontoPagoEfectivo, 0, NumeroCuentaPorCobrar, checkDistribuible.Checked,
                        cBoxMovilidades.SelectedIndex >= 0 ? cBoxMovilidades.SelectedValue.ToString() : null,
                        cboxPersonas.SelectedIndex >= 0 ? cboxPersonas.SelectedValue.ToString() : null,
                        txtObservaciones.Text, ProductosDetalleXML);

                        DTVentaProductosDetalle.Clear();
                        DTVentaProductosDetalle.AcceptChanges();
                        foreach (AlvecoComercial10DataSet.DTProductosSeleccionadosRow DRProductoSeleccionado in DTProductosSeleccionados.Rows)
                        {
                            DTVentaProductosDetalle.AddListarVentasProductosDetalleParaMostrarRow(DRProductoSeleccionado.CodigoProducto,
                                DRProductoSeleccionado.NombreProducto, DRProductoSeleccionado.Cantidad, DRProductoSeleccionado.PrecioUnitario,
                                DRProductoSeleccionado.PrecioTotal, DRProductoSeleccionado.TiempoGarantia, 0, DRProductoSeleccionado.MarcaProducto, DRProductoSeleccionado.MarcaProducto, 0);
                        }
                        DTVentaProductosDetalle.AcceptChanges();
                    }

                    //MessageBox.Show(this, "La operación actual fue registrada correctamente", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    habilitarControles(false);
                    habilitarBotonesAcciones(true, false, false, true, true, true, true, true);
                    TipoOperacion = "";
                    pnlProductosBusqueda.ocultarPanel();
                    if (checkDistribuible.Checked)
                        cargarDatosVentaProducto(NumeroVentaProducto);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Ocurrio la siguiente Excepción " + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                            e.Cancel = false;
                        }
                        //MessageBox.Show("Valor a Validar " + e.FormattedValue.ToString() + ",  Valor Anterior " + this.dtGVVentaProductos.Rows[e.RowIndex].Cells["DGCCantidadExistencia"].Value.ToString());
                        break;


                }

            }
        }

        private void tsBtnModificar_Click(object sender, EventArgs e)
        {
            TipoOperacion = "E";
            cargarProductosSeleccionados(DTVentaProductosDetalle);
            habilitarBotonesAcciones(false, true, true, false, false, false, false, false);
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
                    int.Parse(DRProductosDetalle["CantidadVenta"].ToString()),
                    decimal.Parse(DRProductosDetalle["PrecioUnitarioVenta"].ToString()),
                    Utilidades.DAOUtilidades.ObtenerCantidadExistenciaProducto(NumeroAlmacen, DRProductosDetalle["CodigoProducto"].ToString()),
                    int.Parse(DRProductosDetalle["TiempoGarantiaVenta"].ToString()),
                    DRProductosDetalle["NombreMarca"].ToString(),
                    int.Parse(DRProductosDetalle["CantidadVenta"].ToString()) * decimal.Parse(DRProductosDetalle["PrecioUnitarioVenta"].ToString()),
                    -1
                    );
            }
            DTProductosSeleccionados.AcceptChanges();
        }

        private void tsBtnAnular_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Se encuentra seguro de Anular el Registro Actual?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    //TAVentaProductos.Delete(NumeroAlmacen, NumeroVentaProducto);
                    //MessageBox.Show(this, "Registro eliminado correctamente", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //cargarDatosVentaProducto(-1);
                    TAVentaProductos.ActualizarCodigoEstadoVenta(NumeroAlmacen, NumeroVentaProducto, "A");
                    cBoxEstado.SelectedValue = DTVentaProductos[0].CodigoEstadoVenta = "A";
                    habilitarBotonesAcciones(true, false, false, false, false, false, false, false);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Ocurrió la siguiente Excepcion " + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void tsBtnFinalizar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "¿Se encuentra seguro de Culminar completamente la Venta de Productos?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
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
                    if (!checkDistribuible.Checked)
                    {
                        //INICIO
                        //if (MessageBox.Show(this, "La Finalización de la Transacción actual implica la Entrega Total de todos los Productos Solicitados del Cliente"
                        //    + "de almacenes, para su posterior actualización en Existencia\r\n¿Desea Finalizar la Venta de Productos?",
                        //    "Finalización de Venta de Productos", MessageBoxButtons.YesNo,
                        //    MessageBoxIcon.Question) == DialogResult.Yes)
                        //{

                            if(!validarFinalizacionVenta())
                            {
                                TAVentaProductos.ActualizarCodigoEstadoVenta(NumeroAlmacen, NumeroVentaProducto, "F");
                                TAVentaProductos.ActualizarInventarioVentasProductos(NumeroAlmacen, NumeroVentaProducto, Utilidades.DAOUtilidades.ObtenerFechaHoraServidor(), false);

                                cBoxEstado.SelectedValue = "F";
                                habilitarBotonesAcciones(true, false, false, false, false, false, true, false);
                                habilitarControles(false);
                            }
                        //}

                        //MessageBox.Show(this, "Venta Culminada y Entregada Correctamente", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        //FVentasProductosDistribucion formDistribucion = new FVentasProductosDistribucion(NumeroAlmacen, 1, DIUsuario, NumeroVentaProducto);
                        //if (formDistribucion.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        //{
                        //    MessageBox.Show(this, "Ingrese los datos necesarios para la Distribución de los Productos de la venta actual",
                        //        this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //    FVentasProductosDistribucionDatos formVentaDistribucionDatos = new FVentasProductosDistribucionDatos(DIUsuario, NumeroAlmacen, NumeroVentaProducto);
                        //    formVentaDistribucionDatos.ShowDialog();
                        //    formVentaDistribucionDatos.Dispose();

                        //    string estado = Utilidades.DAOUtilidades.ObtenerCodigoEstadoActualTransacciones(NumeroAlmacen, NumeroVentaProducto, "V");
                        //    cBoxEstado.SelectedValue = estado;
                        //    habilitarBotonesAcciones(true, false, false, false, false, (estado.Equals("D") || estado.Equals("I")), true);
                        //    //MessageBox.Show(this, "Operación Realizada Correctamente", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //}
                        FinalizarVentaDistribuible();
                    }
                    
                }

                //else if (cBoxEstado.SelectedValue != null && cBoxEstado.SelectedValue.Equals("D"))
                //{
                //    //MessageBox.Show(this, "Proceda a completar la Distribución de los Articulos correspondientes a esta venta", this.Text,
                //    //    MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    FVentasProductosDistribucion formDistribucion = new FVentasProductosDistribucion(NumeroAlmacen, 1, DIUsuario, NumeroVentaProducto);
                //    if (formDistribucion.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                //    {
                //        cBoxEstado.SelectedValue = Utilidades.DAOUtilidades.ObtenerCodigoEstadoActualTransacciones(NumeroAlmacen, NumeroVentaProducto, "V");
                //        habilitarBotonesAcciones(true, false, false, false, false, (cBoxEstado.SelectedValue.Equals("D")), true);
                //        //MessageBox.Show(this, "Operación Realizada Correctamente", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    }
                //}


            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "No se Pudo Realizar la Operación Actual, ocurrió la Siguiente Excepción \r\n" + ex.Message,
                    "Error en Finalización de Ingresos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            
        }

        public bool validarFinalizacionVenta()
        {
            if (!checkDistribuible.Checked)
            {
                string ListadoProductos = "";
                AlvecoComercial10DataSet.ListarProductosCantidadSuperaStockMinimoDataTable DTListarProductosCantidadSuperaStockMinimo =
                    TAListarProductosCantidadSuperaStockMinimo.GetData(NumeroAlmacen, NumeroVentaProducto);
                if (DTListarProductosCantidadSuperaStockMinimo.Count > 0)
                {

                    foreach (AlvecoComercial10DataSet.ListarProductosCantidadSuperaStockMinimoRow
                        DRArticulo in DTListarProductosCantidadSuperaStockMinimo.Rows)
                        ListadoProductos += "\r\n" + DRArticulo.NombreProducto;

                    if (MessageBox.Show(this, "La siguiente Lista de Productos supera la cantidad de Stock Minimo en inventarios" +
                        ListadoProductos + "\r\n¿Desea Continuar la transacción?",
                        "Stock Mínimo en Posible riesgo de ser Superado",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return true;
                }


                ListadoProductos = string.Empty;
                AlvecoComercial10DataSet.ListarProductosExistenciaInsuficienteDataTable DTListarProductosExistenciaInsuficiente
                    = TAListarProductosExistenciaInsuficiente.GetData(NumeroAlmacen, NumeroVentaProducto);
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
            }
            else
            {
                DataSet DSVentasProductos = new DataSet("VentasProductos");
                DataTable DTVentasProductosDetalleXML = DTProductosDistribucion.Copy();
                DTVentasProductosDetalleXML.TableName = "VentasProductosDetalle";
                DTVentasProductosDetalleXML.Columns.Remove(DTVentasProductosDetalleXML.Columns["NombreProducto"]);
                DTVentasProductosDetalleXML.Columns["CantidadDistribuida"].ColumnName = "Cantidad";
                DSVentasProductos.Tables.Add(DTVentasProductosDetalleXML);
                string VentasDetalleXML = DTVentasProductosDetalleXML.DataSet.GetXml();

                string ListadoProductos = "";
                AlvecoComercial10DataSet.ListarProductosCantidadSuperaStockMinimoXMLDataTable DTListarProductosCantidadSuperaStockMinimo =
                    TAListarProductosCantidadSuperaStockMinimoXML.GetData(NumeroAlmacen, VentasDetalleXML);
                if (DTListarProductosCantidadSuperaStockMinimo.Count > 0)
                {

                    foreach (AlvecoComercial10DataSet.ListarProductosCantidadSuperaStockMinimoXMLRow
                        DRProducto in DTListarProductosCantidadSuperaStockMinimo.Rows)
                        ListadoProductos += "\r\n" + DRProducto.NombreProducto;

                    if (MessageBox.Show(this, "La siguiente Lista de Artículos supera la cantidad de Stock Minimo en inventarios" +
                        ListadoProductos + "\r\n¿Desea Continuar la transacción?",
                        "Stock Mínimo en Posible riesgo de ser Superado",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return true;
                }
                ListadoProductos = string.Empty;
                AlvecoComercial10DataSet.ListarProductosExistenciaInsuficienteXMLDataTable DTListarProductosExistenciaInsuficiente
                    = TAListarProductosExistenciaInsuficienteXML.GetData(NumeroAlmacen, VentasDetalleXML);
                if (DTListarProductosExistenciaInsuficiente.Count > 0)
                {
                    foreach (AlvecoComercial10DataSet.ListarProductosExistenciaInsuficienteXMLRow
                        DRProducto in DTListarProductosExistenciaInsuficiente.Rows)
                    {
                        ListadoProductos += "\r\n" + DRProducto.NombreProducto;
                    }

                    MessageBox.Show(this, "La Siguiente Lista de Productos supera la Cantidad de Existencia en Almacenes " +
                        ListadoProductos + "\r\nLa Operación actual no puede continuar bajo la situación de existencia actual en almacenes",
                        "Existencia Insuficiente en Inventarios para Abastecer Solicitud",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }
            }
            return false;
        }

        public void FinalizarVentaDistribuible()
        {
            try
            {

                if (cboxPersonas.SelectedIndex < 0)
                {
                    MessageBox.Show(this, "Aún no ha Seleccionado al Responsable para la Distribución de Productos de Esta Venta",
                        this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cboxPersonas.Focus();
                    tabControlVentas.SelectedTab = tabPageVentaDistribuible;
                    return;
                }
                if (cBoxMovilidades.SelectedIndex < 0)
                {
                    MessageBox.Show(this, "Aún no ha Seleccionado la Movilidad para la Distribución de Productos de Esta Venta",
                        this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cBoxMovilidades.Focus();
                    tabControlVentas.SelectedTab = tabPageVentaDistribuible;
                    return;
                }
                DTProductosDistribucion.AcceptChanges();
                DataTable DTTemporal = DTProductosDistribucion.Copy();
                DTTemporal.Columns.Add("PrecioTotal", Type.GetType("System.Decimal"));
                DTTemporal.Columns["PrecioTotal"].Expression = "PrecioUnitarioVenta * CantidadDistribuida";
                DTTemporal.Columns.Remove(DTTemporal.Columns["NombreProducto"]);
                object montoNuevo = DTTemporal.Compute("SUM(PrecioTotal)", "");
                decimal montoNuevoPago = String.IsNullOrEmpty(montoNuevo.ToString()) ? 0 : decimal.Parse(montoNuevo.ToString());
                DTTemporal.Dispose();

                if (montoNuevoPago == 0)
                {
                    MessageBox.Show(this,"Aún no ha ingresado ninguna cantidad para su Distribución", this.Text,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tabControlVentas.SelectedTab = tabPageVentaDistribuible;
                    return;
                }

                if(!Utilidades.DAOUtilidades.EsPosibleModificarMontoPagoVenta(NumeroAlmacen, NumeroVentaProducto, montoNuevoPago))
                {
                    MessageBox.Show(this,"No puede Distribuir la Venta en el estado actual, debido a que la Diferencia entre el monto Cancelado y el nuevo"
                        +"Monto a Pagar no puede modificarse, probablemente la Venta es a Credito y ya realizo algun pago, o quizas el Monto Pagado en Efetivo supera el monto actual de los productos distribuidos"+ 
                        "TRATE DE ANULAR LA VENTA y vuelvala a registrarla, o trate de modificar la venta",
                        this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (!validarFinalizacionVenta())
                {
                    DateTime FechaHoraRegistro = Utilidades.DAOUtilidades.ObtenerFechaHoraServidor();
                    foreach (AlvecoComercial10DataSet.ListarProductosDistribucionFaltantesRow
                        DRProductos in DTProductosDistribucion.Rows)
                    {
                        DateTime FechaHoraCompraInventario = Utilidades.DAOUtilidades.ObtenerFechaHoraServidor();
                        TAVentasProductosDetalleEntrega.Insert(NumeroAlmacen, NumeroVentaProducto, DRProductos.CodigoProducto, FechaHoraRegistro, FechaHoraCompraInventario, DRProductos.CantidadDistribuida, -1);

                    }

                    TAVentaProductos.ActualizarInventarioVentasProductos(NumeroAlmacen, NumeroVentaProducto, FechaHoraRegistro, true);
                    TAVentaProductos.ActualizarVentaDistribucionDatos(NumeroAlmacen, NumeroVentaProducto, "F",
                    cboxPersonas.SelectedValue.ToString(), true, cBoxMovilidades.SelectedValue.ToString());

                    //TAVentaProductos.ActualizarCodigoEstadoVenta(NumeroAlmacen, NumeroVentaProducto, "F");
                    cBoxEstado.SelectedValue = "F";
                    habilitarBotonesAcciones(true, false, false, false, false, false, true, false);
                    habilitarControles(false);
                }
                else
                {
                    MessageBox.Show(this, "No se pudo culminar la venta", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, "No se pudo culminar la operación actual debido a que ocurrió la siguiente Excepción " + ex.Message,
                    this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public void emitirPermisos(bool permitirNuevo, bool permitirModificar, bool permitirAnular, bool permitirFinalizar, bool permitirEliminar)
        {
            this.tsBtnNuevo.Visible = permitirNuevo;
            this.tsBtnModificar.Visible = permitirModificar;
            this.tsBtnAnular.Visible = permitirAnular;
            this.tsBtnFinalizar.Visible = permitirFinalizar;
            this.tsBtnEliminar.Visible = permitirEliminar;
        }

        private void tsBtnReportes_Click(object sender, EventArgs e)
        {
            ListasVentaProductoReporteTableAdapter TAVentasProductoReporte = new ListasVentaProductoReporteTableAdapter();
            TAVentasProductoReporte.Connection = Utilidades.DAOUtilidades.conexion;
            DataTable DTIngresoArticuloR = TAVentasProductoReporte.GetData(NumeroAlmacen, NumeroVentaProducto);
            DataTable DTIngresoArticuloDetalleR = TAVentasProductosDetalleMostrar.GetData(NumeroAlmacen, NumeroVentaProducto);
            FReporteTransaccionesGeneral formReporte = new FReporteTransaccionesGeneral();
            formReporte.cargarReporteVentasProductos(DTIngresoArticuloR, DTIngresoArticuloDetalleR);
            formReporte.ShowDialog(this);
            formReporte.Dispose();
        }

        private void btnAgregarPersonas_Click(object sender, EventArgs e)
        {
            Formularios.GestionSistema.FPersonas formPersonas = new GestionSistema.FPersonas();
            formPersonas.ShowDialog();

            DTPersonas = TAPersonas.GetData(null);
            this.cboxPersonas.DataSource = DTPersonas;
            this.cboxPersonas.DisplayMember = "NombreCompleto";
            this.cboxPersonas.ValueMember = "DIPersona";
            if (!String.IsNullOrEmpty(formPersonas.DIPersona))
                this.cboxPersonas.SelectedValue = formPersonas.DIPersona;
            else
                this.cboxPersonas.SelectedIndex = -1;
            formPersonas.Dispose();
        }

        private void btnAgregarMovilidad_Click(object sender, EventArgs e)
        {
            Formularios.GestionSistema.FMovilidades formMovilidades = new GestionSistema.FMovilidades();
            formMovilidades.ShowDialog();

            DTMovilidades = TAMovilidades.GetData();
            this.cBoxMovilidades.DataSource = DTMovilidades;
            this.cBoxMovilidades.DisplayMember = "NombreMovilidad";
            this.cBoxMovilidades.ValueMember = "CodigoMovilidad";
            if (!String.IsNullOrEmpty(formMovilidades.CodigoMovilidad))
                this.cBoxMovilidades.SelectedValue = formMovilidades.CodigoMovilidad;
            else
                this.cBoxMovilidades.SelectedIndex = -1;
            formMovilidades.Dispose();
        }

        private void checkDistribuible_CheckedChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(TipoOperacion))
            {
                if (checkDistribuible.Checked)
                {
                    if (!tabControlVentas.Contains(tabPageVentaDistribuible))
                    {
                        tabControlVentas.TabPages.Add(tabPageVentaDistribuible);
                        cboxPersonas.SelectedIndex = cBoxMovilidades.SelectedIndex = -1;
                    }

                }
                else
                {
                    if (tabControlVentas.Contains(tabPageVentaDistribuible))
                    {
                        tabControlVentas.TabPages.Remove(tabPageVentaDistribuible);
                    }
                }
            }
        }

        private void tsBtnEliminar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Se encuentra seguro de eliminar el Registro Actual?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    TAVentaProductos.Delete(NumeroAlmacen, NumeroVentaProducto);
                    //MessageBox.Show(this, "Registro eliminado correctamente", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cargarDatosVentaProducto(-1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Ocurrió la siguiente Excepcion " + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
    }
}
