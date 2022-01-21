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
    public partial class FComprasProductos : Form
    {
        private string DIUsuario;
        private int NumeroAlmacen;
        private AlvecoComercial10DataSet.DTProductosSeleccionadosDataTable DTProductosSeleccionados;
        private ComprasProductosTableAdapter TACompraProductos;
        private AlvecoComercial10DataSet.ComprasProductosDataTable DTCompraProductos;
        private ComprasProductosDetalleTableAdapter TAComprasProductosDetalle;
        private ListarComprasProductosDetalleParaMostrarTableAdapter TAComprasProductosDetalleMostrar;
        private AlvecoComercial10DataSet.ListarComprasProductosDetalleParaMostrarDataTable DTCompraProductosDetalle;
        private ProveedoresTableAdapter TAProveedores;
        private AlvecoComercial10DataSet.ProveedoresDataTable DTProveedores;
        private Utilidades.ObjetoCodigoDescripcion ComprasTipos;
        private Utilidades.ObjetoCodigoDescripcion ComprasEstado;
        int? NumeroCuentaPorPagar = null;

        public int NumeroCompraProducto { get; set; }
        private string TipoOperacion = "";

        public FComprasProductos(string DIUsuario, int NumeroAlmacen)
        {
            InitializeComponent();
            this.DIUsuario = DIUsuario;
            this.NumeroAlmacen = NumeroAlmacen;

            
            DTProductosSeleccionados = new AlvecoComercial10DataSet.DTProductosSeleccionadosDataTable();
            DTCompraProductosDetalle = new AlvecoComercial10DataSet.ListarComprasProductosDetalleParaMostrarDataTable();



            //DataColumn DCColumnaCalculo = new DataColumn("PrecioUnitarioCompra", Type.GetType("System.Decimal"));
            //DTProductosSeleccionados.Columns.Add(DCColumnaCalculo);

            DataColumn DCColumnaCalculo = new DataColumn("PorcentajeGananciaVentaPorMayor", Type.GetType("System.Decimal"));
            DCColumnaCalculo.DefaultValue = 0;
            DTProductosSeleccionados.Columns.Add(DCColumnaCalculo);

            DCColumnaCalculo = new DataColumn("PorcentajeGananciaVentaPorMenor", Type.GetType("System.Decimal"));
            DCColumnaCalculo.DefaultValue = 0;
            DTProductosSeleccionados.Columns.Add(DCColumnaCalculo);

            DCColumnaCalculo = new DataColumn("PrecioUnitarioVentaPorMayor", Type.GetType("System.Decimal"));
            DCColumnaCalculo.DefaultValue = 0;
            //DCColumnaCalculo.Expression = "PrecioUnitario + (PrecioUnitario * PorcentajeGananciaVentaPorMayor / 100) ";
            DTProductosSeleccionados.Columns.Add(DCColumnaCalculo);

            DCColumnaCalculo = new DataColumn("PrecioUnitarioVentaPorMenor", Type.GetType("System.Decimal"));
            DCColumnaCalculo.DefaultValue = 0;
            //DCColumnaCalculo.Expression = "PrecioUnitario + (PrecioUnitario * PorcentajeGananciaVentaPorMenor / 100)";
            DTProductosSeleccionados.Columns.Add(DCColumnaCalculo);

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

            dtGVCompraProductos.AutoGenerateColumns = false;
            this.pnlProductosBusqueda.cargarParametrosConstructor(DTProductosSeleccionados,
                NumeroAlmacen, DIUsuario, "C");
            this.pnlProductosBusqueda.ocultarPanel();
            TACompraProductos = new ComprasProductosTableAdapter();
            TACompraProductos.Connection = Utilidades.DAOUtilidades.conexion;
            TAComprasProductosDetalle = new ComprasProductosDetalleTableAdapter();
            TAComprasProductosDetalle.Connection = Utilidades.DAOUtilidades.conexion;
            TAComprasProductosDetalleMostrar = new ListarComprasProductosDetalleParaMostrarTableAdapter();
            TAComprasProductosDetalleMostrar.Connection = Utilidades.DAOUtilidades.conexion;
            TAProveedores = new ProveedoresTableAdapter();
            TAProveedores.Connection = Utilidades.DAOUtilidades.conexion;
            ComprasTipos = new Utilidades.ObjetoCodigoDescripcion();
            ComprasEstado = new Utilidades.ObjetoCodigoDescripcion();

            ComprasEstado.cargarDatosEstadosComprasVentas();
            cBoxEstado.DataSource = ComprasEstado.listaObjetos;
            cBoxEstado.DisplayMember = ComprasEstado.DisplayMember;
            cBoxEstado.ValueMember = ComprasEstado.ValueMember;

            ComprasTipos.cargarDatosTiposComprasVentas();
            cBoxTipoCompra.DataSource = ComprasTipos.listaObjetos;
            cBoxTipoCompra.DisplayMember = ComprasTipos.DisplayMember;
            cBoxTipoCompra.ValueMember = ComprasTipos.ValueMember;

            DTProveedores = TAProveedores.GetDataByActivos();
            cBoxProveedor.DataSource = DTProveedores;
            cBoxProveedor.DisplayMember = "NombreRazonSocial";
            cBoxProveedor.ValueMember = "CodigoProveedor";
            

            cargarDatosCompraProducto(-1);


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

            object PrecioTotal = DTProductosSeleccionados.Compute("Sum(PrecioTotal)","");
            txtPrecioTotal.Text = PrecioTotal == null ? "0.00 Bs  " : PrecioTotal.ToString() + " Bs  ";
            //DTProductosSeleccionados.AcceptChanges();
        }

        public void adecuarColumnasCompraTemporal()
        {
            DGCCantidadEntregada.Visible = false;
            DGCCantidadCompra.DataPropertyName = DTProductosSeleccionados.CantidadColumn.ColumnName;
            DGCCantidadCompra.ReadOnly = false;
            DGCPrecioUnitarioCompra.DataPropertyName = DTProductosSeleccionados.PrecioUnitarioColumn.ColumnName;
            DGCPrecioUnitarioCompra.ReadOnly = false;
            DGCTiempoGarantiaCompra.DataPropertyName = DTProductosSeleccionados.TiempoGarantiaColumn.ColumnName;
            DGCTiempoGarantiaCompra.ReadOnly = false;
            DGCPrecioTotal.DataPropertyName = DTProductosSeleccionados.PrecioTotalColumn.ColumnName;
            DGCPrecioTotal.ReadOnly = true;
        }

        public void adecuarColumnasVisualizarCompra()
        {
            DGCCantidadEntregada.Visible = true;
            DGCCantidadCompra.DataPropertyName = "CantidadCompra";
            DGCCantidadCompra.ReadOnly = true;
            DGCPrecioUnitarioCompra.DataPropertyName = "PrecioUnitarioCompra";
            DGCPrecioUnitarioCompra.ReadOnly = true;
            DGCTiempoGarantiaCompra.DataPropertyName = "TiempoGarantiaCompra";
            DGCTiempoGarantiaCompra.ReadOnly = true;
            DGCPrecioTotal.DataPropertyName = "PrecioTotal";
        }


        public void habilitarControles(bool estadoHabilitacion)
        {
            this.txtNroComprobante.ReadOnly = !estadoHabilitacion;
            this.txtNroFactura.ReadOnly = !estadoHabilitacion;
            this.txtObservaciones.ReadOnly = !estadoHabilitacion;
            this.cBoxProveedor.Enabled = estadoHabilitacion;
            this.cBoxTipoCompra.Enabled = estadoHabilitacion;
            this.btnAgregarProveedor.Enabled = estadoHabilitacion;
            //this.dtGVCompraProductos.ReadOnly = !estadoHabilitacion;
            DGCPrecioUnitarioVentaPorMayor.Visible = DGCPrecioUnitarioVentaPorMenor.Visible = DGCPorcentajeGananciaVentaPorMayor.Visible = DGCPorcentajeGananciaVentaPorMenor.Visible = estadoHabilitacion;
            bindingNavigatorDeleteItem.Visible = estadoHabilitacion;
        }

        public void limpiarControles()
        {
            this.txtNroComprobante.Text = String.Empty;
            this.txtNroFactura.Text = String.Empty;
            this.txtObservaciones.Text = String.Empty;
            this.cBoxProveedor.SelectedIndex = -1;
            this.cBoxTipoCompra.SelectedIndex = -1;
            this.tsLblFechaHoraRecepcion.Text = String.Empty;
            this.tsLblFechaHoraRegistro.Text = String.Empty;
            this.tsLblNroAgencia.Text = String.Empty;
            this.tsLblNroCompra.Text = String.Empty;
            this.lblUsuario.Text = String.Empty;
            DTProductosSeleccionados.Clear();
            txtPrecioTotal.Text = "0.00 BS";
            checkActualizarPorcentajeGanancia.Checked = false;
        }

        public void habilitarBotonesAcciones(bool nuevo, bool aceptar, bool cancelar, bool modificar, bool anular, bool finalizar, bool eliminar, bool reportes)
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
                return false;

            }
            if (cBoxProveedor.SelectedIndex < 00)
            {
                errorProvider1.SetError(cBoxProveedor, "Aún no ha seleccionado el Proveedor");
                cBoxProveedor.Focus();
                return false;
            }
            if (cBoxTipoCompra.SelectedIndex < 00)
            {
                errorProvider1.SetError(cBoxTipoCompra, "Aún no ha seleccionado El tipo de Compra");
                cBoxTipoCompra.Focus();
                return false;
            }

            //DTProductosSeleccionados.AcceptChanges();
            if (DTProductosSeleccionados.Select("PrecioUnitario = 0").Length > 0)
            {
                
                errorProvider1.SetError(txtPrecioTotal,"No puede adquirir productos con precios nulos");
                txtPrecioTotal.Focus();
                return false;
            }
            return true;
        }

        public void cargarDatosCompraProducto(int NumeroCompraProducto)
        {
            DTCompraProductos = TACompraProductos.GetDataByNumeroCompraProducto(NumeroAlmacen, NumeroCompraProducto);
            if (DTCompraProductos.Count > 0)
            {
                this.NumeroCompraProducto = NumeroCompraProducto;
                DTCompraProductosDetalle = TAComprasProductosDetalleMostrar.GetData(NumeroAlmacen, NumeroCompraProducto);
                adecuarColumnasVisualizarCompra();
                bdSourceCompraProductos.DataSource = DTCompraProductosDetalle;


                lblUsuario.Text = Utilidades.DAOUtilidades.ObtenerNombreCompleto(DTCompraProductos[0].DIUsuario);
                cBoxProveedor.SelectedValue = DTCompraProductos[0].CodigoProveedor;
                cBoxTipoCompra.SelectedValue = DTCompraProductos[0].CodigoTipoCompra;
                cBoxEstado.SelectedValue = DTCompraProductos[0].CodigoEstadoCompra;
                txtNroComprobante.Text = DTCompraProductos[0].IsNumeroComprobanteNull() ?
                    String.Empty : DTCompraProductos[0].NumeroComprobante;
                txtNroFactura.Text = DTCompraProductos[0].IsNumeroFacturaNull() ?
                    String.Empty : DTCompraProductos[0].NumeroFactura;
                txtObservaciones.Text = DTCompraProductos[0].IsObservacionesNull() ?
                    String.Empty : DTCompraProductos[0].Observaciones;

                tsLblNroAgencia.Text = "Nro almacen :" + NumeroAlmacen.ToString();
                tsLblNroCompra.Text = "Nro Compra :" + NumeroCompraProducto.ToString();
                tsLblFechaHoraRegistro.Text = "Fecha Registro " + DTCompraProductos[0].FechaHoraRegistro.ToString();
                tsLblFechaHoraRecepcion.Text = !DTCompraProductos[0].IsFechaHoraRecepcionNull() ? "Fecha Recepcion " + DTCompraProductos[0].FechaHoraRecepcion
                    : String.Empty;

                NumeroCuentaPorPagar = DTCompraProductos[0].IsNumeroCuentaPorPagarNull() ? null : (int?)DTCompraProductos[0].NumeroCuentaPorPagar;
                txtPrecioTotal.Text = DTCompraProductosDetalle.Compute("sum(PrecioTotal)", "").ToString() + " Bs  ";
                txtMontoCancelado.Text = DTCompraProductos[0].MontoTotalPagoEfectivo.ToString() + " Bs  ";

                txtMontoCancelado.Visible = !DTCompraProductos[0].IsNumeroCuentaPorPagarNull();
                lblMontoCancelado.Visible = !DTCompraProductos[0].IsNumeroCuentaPorPagarNull();

                switch (DTCompraProductos[0].CodigoEstadoCompra)
                {
                    case"I":
                        habilitarBotonesAcciones(true, false, false, true, true, true, true, true);
                        break;
                    case "F":
                        habilitarBotonesAcciones(true, false, false, false, false, false, false, true);
                        break;
                    case "A":
                        habilitarBotonesAcciones(true, false, false, true, true, true, true, true);
                        break;
                    case "D":
                        habilitarBotonesAcciones(true, false, false, false, false, true, false, true);
                        break;
                    case "X":
                        habilitarBotonesAcciones(true, false, false, false, false, false, false, false);
                        break;
                    default :
                        habilitarBotonesAcciones(true, false, false, false, false, false, false, false);
                        break;
                }
            }
            else
            {
                limpiarControles();
                habilitarBotonesAcciones(true, false, false, false, false, false, false, false);
                NumeroCuentaPorPagar = null;
                DTCompraProductosDetalle.Clear();
            }
            habilitarControles(false);
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
            cBoxTipoCompra.SelectedValue = "C";
            adecuarColumnasCompraTemporal();
            bdSourceCompraProductos.DataSource = DTProductosSeleccionados;
            tsLblFechaHoraRegistro.Text = "Fecha Registro " + Utilidades.DAOUtilidades.ObtenerFechaHoraServidor().ToString();
            tsLblNroAgencia.Text = "Nro Almacen " + NumeroAlmacen.ToString();
            tsLblNroCompra.Text = "Nro Compra Producto " + (Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("ComprasProductos") + 1).ToString();
            DTCompraProductosDetalle.Clear();
            NumeroCuentaPorPagar = null;
            lblMontoCancelado.Visible = txtMontoCancelado.Visible = false;
            checkActualizarPorcentajeGanancia.Visible = true;

        }

        private void tsBtnCancelar_Click(object sender, EventArgs e)
        {
            TipoOperacion = "";
            errorProvider1.Clear();
            pnlProductosBusqueda.ocultarPanel();
            limpiarControles();
            habilitarControles(false);
            habilitarBotonesAcciones(true, false, false, false, false, false, false, false);
            adecuarColumnasVisualizarCompra();
            checkActualizarPorcentajeGanancia.Visible = false;
        }

        private void btnAgregarProveedor_Click(object sender, EventArgs e)
        {
            Formularios.GestionSistema.FProveedores formProveedores = new GestionSistema.FProveedores();
            formProveedores.configurarFormularioIA(-1);
            if (formProveedores.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
                DTProveedores.FindByCodigoProveedor(formProveedores.CodigoProveedor) == null
                && TAProveedores.GetDataBy1(formProveedores.CodigoProveedor).Count > 0)
            {
                DTProveedores.Rows.Add(TAProveedores.GetDataBy1(formProveedores.CodigoProveedor)[0].ItemArray);
                DTProveedores.DefaultView.Sort = "NombreRazonSocial ASC";
                cBoxProveedor.SelectedValue = formProveedores.CodigoProveedor;
            }
            formProveedores.Dispose();
        }

        private void dtGVCompraProductos_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dtGVCompraProductos.CurrentCell != null)
            {
                if (e.ColumnIndex == DGCCantidadCompra.Index || e.ColumnIndex == DGCPrecioUnitarioCompra.Index)
                {
                    dtGVCompraProductos[DGCPrecioTotal.Name, e.RowIndex].Value = DTProductosSeleccionados[e.RowIndex].Cantidad *
                        DTProductosSeleccionados[e.RowIndex].PrecioUnitario;

                    DTProductosSeleccionados.AcceptChanges();
                }

                string CodigoProducto = dtGVCompraProductos.CurrentRow.Cells["DGCCodigoProducto"].Value.ToString();
                string NombreProducto = dtGVCompraProductos.CurrentRow.Cells["DGCNombreProducto"].Value.ToString(); 

                AlvecoComercial10DataSet.DTProductosSeleccionadosRow DRProducto = DTProductosSeleccionados.FindByCodigoProducto(CodigoProducto);

                decimal PrecioCompraCalculado = 0, PorcentajeUtilidad = 0, PrecioUnitarioVenta = 0;
                PrecioCompraCalculado = decimal.Parse(dtGVCompraProductos.CurrentRow.Cells["DGCPrecioUnitarioCompra"].Value.ToString()); 
                switch (dtGVCompraProductos.Columns[e.ColumnIndex].Name) 
                {
                    case "DGCPorcentajeGananciaVentaPorMenor":
                    case "DGCPorcentajeGananciaVentaPorMayor":
                        PorcentajeUtilidad = decimal.Parse(dtGVCompraProductos.CurrentRow.Cells[dtGVCompraProductos.Columns[e.ColumnIndex].Name].Value.ToString());
                        break;
                    case "DGCPrecioUnitarioVentaPorMayor":
                    case "DGCPrecioUnitarioVentaPorMenor":
                        PrecioUnitarioVenta = decimal.Parse(dtGVCompraProductos.CurrentRow.Cells[dtGVCompraProductos.Columns[e.ColumnIndex].Name].Value.ToString());
                        break;

                }

                if (e.ColumnIndex == DGCPorcentajeGananciaVentaPorMenor.Index)
                {
                    
                    DRProducto["PrecioUnitarioVentaPorMenor"] = decimal.Round((PrecioCompraCalculado * PorcentajeUtilidad / 100) + PrecioCompraCalculado, 2);
                    DRProducto.AcceptChanges();

                    //dtGVCompraProductos_CellValueChanged(dtGVCompraProductos, new DataGridViewCellEventArgs(DGCPrecioUnitarioVentaPorMenor.Index, e.RowIndex));
                    
                    
                }
                if (e.ColumnIndex == DGCPorcentajeGananciaVentaPorMayor.Index)
                {
                    DRProducto["PrecioUnitarioVentaPorMayor"] = decimal.Round((PrecioCompraCalculado * PorcentajeUtilidad / 100) + PrecioCompraCalculado, 2);
                    DRProducto.AcceptChanges();

                    //dtGVCompraProductos_CellValueChanged(dtGVCompraProductos, new DataGridViewCellEventArgs(DGCPrecioUnitarioVentaPorMayor.Index, e.RowIndex));
                    
                }

                if (e.ColumnIndex == DGCPrecioUnitarioVentaPorMenor.Index)
                {
                    DRProducto["PorcentajeGananciaVentaPorMenor"] = decimal.Round((PrecioUnitarioVenta / PrecioCompraCalculado - 1) * 100, 2);
                    //dtGVCompraProductos.CurrentRow.Cells["DGCPorcentajeGananciaVentaPorMenor"].Value = DRProducto["PorcentajeGananciaVentaPorMenor"];
                }
                if (e.ColumnIndex == DGCPrecioUnitarioVentaPorMayor.Index)
                {
                    DRProducto["PorcentajeGananciaVentaPorMayor"] = decimal.Round((PrecioUnitarioVenta / PrecioCompraCalculado - 1) * 100, 2);
                    //dtGVCompraProductos.CurrentRow.Cells["DGCPrecioUnitarioVentaPorMayor"].Value = DRProducto["PrecioUnitarioVentaPorMayor"];
                }

                if (e.ColumnIndex == DGCPrecioUnitarioCompra.Index)
                {

                    dtGVCompraProductos_CellValueChanged(dtGVCompraProductos, new DataGridViewCellEventArgs(DGCPorcentajeGananciaVentaPorMayor.Index, e.RowIndex));
                    dtGVCompraProductos_CellValueChanged(dtGVCompraProductos, new DataGridViewCellEventArgs(DGCPorcentajeGananciaVentaPorMenor.Index, e.RowIndex));                    
                }

                DRProducto.AcceptChanges();
                DTProductosSeleccionados.AcceptChanges();
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
                DTProductosSeleccionadosXMLAux.TableName = "ComprasProductosDetalle";
                DTProductosSeleccionadosXMLAux.AcceptChanges();

                DataSet DSProductosSeleccionados;
                DSProductosSeleccionados = new DataSet("ComprasProductos");
                DSProductosSeleccionados.Tables.Add(DTProductosSeleccionadosXMLAux);

                string ProductosDetalleXML = DTProductosSeleccionadosXMLAux.DataSet.GetXml();

                
                decimal MontoTotalCompra = decimal.Parse(DTProductosSeleccionados.Compute("sum(PrecioTotal)", "").ToString());
                decimal MontoPagoEfectivo = MontoTotalCompra;

                if (DTProductosSeleccionados.Select("PrecioTotal = 0").Length > 0
                    && MessageBox.Show(this, "Existen articulos cuyos precios son equivalentes a cero. ¿Se encuentra seguro de continuar en este estado con la Transacción?",
                    this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                if (MontoTotalCompra <= 0 && MessageBox.Show(this, "¿Se encuentra seguro de realizar la Transaccion con un Precio Total equivalente a cero?",
                    this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                {
                    errorProvider1.SetError(lblTotalTransaccion, "El Precio Total Equivalente es Cero");
                    return;
                }

                try
                {
                    if (cBoxTipoCompra.SelectedValue.ToString() == "R")//Si la compra es a credito
                    {
                        Utilidades.FIngresarCantidad formIngresarMonto = new Utilidades.FIngresarCantidad(true, "Ingrese el Monto de Pago", true);
                        formIngresarMonto.MontoTopeMaximo = MontoTotalCompra;

                        if (formIngresarMonto.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                        {
                            MessageBox.Show(this, "No ha ingresado el Monto que desea pagar en el Credito");
                            return;
                        }
                        MontoPagoEfectivo = formIngresarMonto.MontoDecimalIngresado;
                        lblMontoCancelado.Visible = txtMontoCancelado.Visible = true;
                        txtMontoCancelado.Text = MontoPagoEfectivo.ToString() + " Bs  ";

                        if (NumeroCuentaPorPagar == null)
                        {
                            FCuentasPorPagarIA formCuentasPorPagar = new FCuentasPorPagarIA(NumeroAlmacen, DIUsuario);
                            formCuentasPorPagar.configurarFormularioIA(null);
                            formCuentasPorPagar.cargarDatosMontoTotalCompra(MontoTotalCompra - MontoPagoEfectivo, int.Parse(cBoxProveedor.SelectedValue.ToString()),
                                "La Cuenta Por Pagar corresponde a la Orden de Compra de Productos " + (Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("ComprasProductos") + 1).ToString());
                            if (formCuentasPorPagar.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                            {
                                MessageBox.Show(this, "No puede Ingresar una Compra a Credito sin primero haber registrado la Cuenta por Pagar");
                                return;
                            }
                            NumeroCuentaPorPagar = formCuentasPorPagar.NumeroCuentaPorPagar;
                            formCuentasPorPagar.Dispose();
                        }
                    }

                    if (TipoOperacion == "I")
                    {
                        TACompraProductos.InsertarCompraProductoXMLDetalle(NumeroAlmacen, int.Parse(cBoxProveedor.SelectedValue.ToString()),
                        DIUsuario, cBoxTipoCompra.SelectedValue.ToString(), "I", txtNroFactura.Text, txtNroComprobante.Text,
                        MontoTotalCompra, MontoPagoEfectivo, NumeroCuentaPorPagar, txtObservaciones.Text, ProductosDetalleXML);

                        foreach (AlvecoComercial10DataSet.DTProductosSeleccionadosRow DRProductoSeleccionado in DTProductosSeleccionados.Rows)
                        {
                            DTCompraProductosDetalle.AddListarComprasProductosDetalleParaMostrarRow(DRProductoSeleccionado.CodigoProducto,
                                DRProductoSeleccionado.NombreProducto, DRProductoSeleccionado.Cantidad, 0, DRProductoSeleccionado.PrecioUnitario,
                                DRProductoSeleccionado.PrecioTotal, DRProductoSeleccionado.TiempoGarantia, DRProductoSeleccionado.MarcaProducto, DRProductoSeleccionado.MarcaProducto);
                        }
                        DTCompraProductosDetalle.AcceptChanges();
                        NumeroCompraProducto = Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("ComprasProductos");
                    }
                    else
                    {
                        NumeroCuentaPorPagar = cBoxTipoCompra.SelectedValue.ToString() == "E" ? null : NumeroCuentaPorPagar;

                        TACompraProductos.ActualizarCompraProductoXMLDetalle(NumeroAlmacen, NumeroCompraProducto, int.Parse(cBoxProveedor.SelectedValue.ToString()),
                        DIUsuario, cBoxTipoCompra.SelectedValue.ToString(), "I", txtNroFactura.Text, txtNroComprobante.Text,
                        MontoTotalCompra, MontoPagoEfectivo, NumeroCuentaPorPagar, txtObservaciones.Text, ProductosDetalleXML);

                        DTCompraProductosDetalle.Clear();
                        DTCompraProductosDetalle.AcceptChanges();
                        foreach (AlvecoComercial10DataSet.DTProductosSeleccionadosRow DRProductoSeleccionado in DTProductosSeleccionados.Rows)
                        {
                            DTCompraProductosDetalle.AddListarComprasProductosDetalleParaMostrarRow(DRProductoSeleccionado.CodigoProducto,
                                DRProductoSeleccionado.NombreProducto, DRProductoSeleccionado.Cantidad, 0, DRProductoSeleccionado.PrecioUnitario,
                                DRProductoSeleccionado.PrecioTotal, DRProductoSeleccionado.TiempoGarantia, DRProductoSeleccionado.MarcaProducto, DRProductoSeleccionado.MarcaProducto);
                        }
                        DTCompraProductosDetalle.AcceptChanges();
                    }

                    //MessageBox.Show(this, "La operación actual fue registrada correctamente", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    habilitarControles(false);
                    habilitarBotonesAcciones(true, false, false, true, true, true, true, true);
                    TipoOperacion = "";
                    pnlProductosBusqueda.ocultarPanel();
                    checkActualizarPorcentajeGanancia.Visible = false;
                    tsBtnFinalizar_Click(tsBtnFinalizar, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Ocurrio la siguiente Excepción " + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dtGVCompraProductos_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            int CantidadComprada; decimal precioCompra;

            this.dtGVCompraProductos.Rows[e.RowIndex].ErrorText = "";

            // No cell validation for new rows. New rows are validated on Row Validation.
            if (this.dtGVCompraProductos.Rows[e.RowIndex].IsNewRow) { return; }

            if (this.dtGVCompraProductos.IsCurrentCellDirty)
            {
                switch (this.dtGVCompraProductos.Columns[e.ColumnIndex].Name)
                {

                    case "DGCCantidadCompra":  
                        if (e.FormattedValue.ToString().Trim().Length < 1)
                        {
                            this.dtGVCompraProductos.Rows[e.RowIndex].ErrorText = "   La Cantidad es necesaria y no puede estar vacia.";
                            e.Cancel = true;
                        }
                        else if (!int.TryParse(e.FormattedValue.ToString(), out CantidadComprada) || CantidadComprada <= 0)
                        {
                            this.dtGVCompraProductos.Rows[e.RowIndex].ErrorText = "   La Cantidad debe ser un entero positivo Mayor a Cero";
                            e.Cancel = true;
                        }
                        //MessageBox.Show("Valor a Validar " + e.FormattedValue.ToString() + ",  Valor Anterior " + this.dtGVCompraProductos.Rows[e.RowIndex].Cells["DGCCantidadExistencia"].Value.ToString());
                        break;

                    case "DGCPrecioUnitarioCompra": 
                        if (e.FormattedValue.ToString().Trim().Length < 1)
                        {
                            this.dtGVCompraProductos.Rows[e.RowIndex].ErrorText = "   El Precio Unitario de Compra es necesario y no puede estar vacio.";
                            e.Cancel = true;
                        }
                        else if (!decimal.TryParse(e.FormattedValue.ToString(), out precioCompra) || precioCompra <= 0)
                        {
                            this.dtGVCompraProductos.Rows[e.RowIndex].ErrorText = "   El Precio Unitario de Compra debe ser un valor positivo Mayor a Cero";
                            e.Cancel = true;
                        }
                        //MessageBox.Show("Valor a Validar " + e.FormattedValue.ToString() + ",  Valor Anterior " + this.dtGVCompraProductos.Rows[e.RowIndex].Cells["DGCCantidadExistencia"].Value.ToString());
                        break;
                    case "DGCPorcentajeGananciaVentaPorMayor":
                    case "DGCPorcentajeGananciaVentaPorMenor":
                    case "DGCPrecioUnitarioVentaPorMayor":
                    case "DGCPrecioUnitarioVentaPorMenor":
                        if (e.FormattedValue.ToString().Trim().Length < 1)
                        {
                            this.dtGVCompraProductos.Rows[e.RowIndex].ErrorText = "   El monto ingresado es necesario y no puede estar vacio.";
                            e.Cancel = true;
                        }
                        else if (!decimal.TryParse(e.FormattedValue.ToString(), out precioCompra) || precioCompra < 0)
                        {
                            this.dtGVCompraProductos.Rows[e.RowIndex].ErrorText = "   El monto ingresado debe ser un valor positivo";
                            e.Cancel = true;
                        }
                        //MessageBox.Show("Valor a Validar " + e.FormattedValue.ToString() + ",  Valor Anterior " + this.dtGVCompraProductos.Rows[e.RowIndex].Cells["DGCCantidadExistencia"].Value.ToString());
                        break;
                    //DGCTiempoGarantiaCompra
                        case "DGCTiempoGarantiaCompra":
                        if (e.FormattedValue.ToString().Trim().Length < 1)
                        {
                            this.dtGVCompraProductos.Rows[e.RowIndex].ErrorText = "   La Garantia es necesaria y no puede estar vacia.";
                            e.Cancel = true;
                        }
                        else if (!int.TryParse(e.FormattedValue.ToString(), out CantidadComprada) || CantidadComprada < 0)
                        {
                            this.dtGVCompraProductos.Rows[e.RowIndex].ErrorText = "   La Garantia debe ser un entero positivo";
                            e.Cancel = true;
                        }
                        //MessageBox.Show("Valor a Validar " + e.FormattedValue.ToString() + ",  Valor Anterior " + this.dtGVCompraProductos.Rows[e.RowIndex].Cells["DGCCantidadExistencia"].Value.ToString());
                        break;

                }

            }
        }

        private void tsBtnModificar_Click(object sender, EventArgs e)
        {
            TipoOperacion = "E";
            cargarProductosSeleccionados(DTCompraProductosDetalle);
            habilitarBotonesAcciones(false, true, true, false, false, false, false, false);
            pnlProductosBusqueda.visualizarPanel();            
            habilitarControles(true);
            bdSourceCompraProductos.DataSource = DTProductosSeleccionados;
            DTProductosSeleccionados.RowChanged += DTProductosSeleccionados_RowChanged;
            DTProductosSeleccionados.RowDeleted += DTProductosSeleccionados_RowChanged;
            adecuarColumnasCompraTemporal();
            checkActualizarPorcentajeGanancia.Visible = true;
        }


        public void cargarProductosSeleccionados(DataTable DTComprasProductosDetalle)
        {
            ProductosTableAdapter TAProductos = new ProductosTableAdapter();
            TAProductos.Connection = Utilidades.DAOUtilidades.conexion;
            AlvecoComercial10DataSet.ProductosDataTable DTProductosAux;

            pnlProductosBusqueda.limpiarControles();
            foreach(DataRow DRProductosDetalle in DTComprasProductosDetalle.Rows)
            {
                DTProductosAux = TAProductos.GetDataBy1(DRProductosDetalle["CodigoProducto"].ToString());
                DTProductosSeleccionados.AddDTProductosSeleccionadosRow(
                    DRProductosDetalle["CodigoProducto"].ToString(),
                    DRProductosDetalle["NombreProducto"].ToString(),
                    int.Parse(DRProductosDetalle["CantidadCompra"].ToString()),
                    decimal.Parse(DRProductosDetalle["PrecioUnitarioCompra"].ToString()),
                    Utilidades.DAOUtilidades.ObtenerCantidadExistenciaProducto(NumeroAlmacen, DRProductosDetalle["CodigoProducto"].ToString()),
                    int.Parse(DRProductosDetalle["TiempoGarantiaCompra"].ToString()),
                    DRProductosDetalle["NombreMarca"].ToString(),
                    int.Parse(DRProductosDetalle["CantidadCompra"].ToString()) * decimal.Parse(DRProductosDetalle["PrecioUnitarioCompra"].ToString()),
                    DTProductosAux[0].IsCodigoProveedorNull() ? -1 : DTProductosAux[0].CodigoProveedor
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
                    //TACompraProductos.Delete(NumeroAlmacen, NumeroCompraProducto);
                    ////MessageBox.Show(this, "Registro eliminado correctamente", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //cargarDatosCompraProducto(-1);

                    TACompraProductos.ActualizarCodigoEstadoCompra(NumeroAlmacen, NumeroCompraProducto, "A");
                    cBoxEstado.SelectedValue = DTCompraProductos[0].CodigoEstadoCompra = "A";
                    habilitarBotonesAcciones(true, false, false, false, false, false, true, false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Ocurrió la siguiente Excepcion " + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void tsBtnFinalizar_Click(object sender, EventArgs e)
        {
            //if (MessageBox.Show(this, "¿Se encuentra seguro de Culminar completamente la Compra de Productos?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            //    == System.Windows.Forms.DialogResult.Yes)
            //{
            //    TACompraProductos.ActualizarCodigoEstadoCompra(NumeroAlmacen, NumeroCompraProducto, "F");
            //    cBoxEstado.SelectedValue = "F";
            //    habilitarBotonesAcciones(true, false, false, false, false, false, true);
            //    MessageBox.Show(this, "Compra Culminada y Recepcionada Correctamente", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}

            //if (MessageBox.Show(this, "La Finalización de la Transacción actual implica la recepción Total de todos los Productos Solicitados al Proveedor"
            //    + "en almacenes, para su posterior actualización en Existencia\r\n¿Desea Finalizar la Compra de Productos?",
            //    "Finalización de Compra de Productos", MessageBoxButtons.YesNo,
            //    MessageBoxIcon.Question) == DialogResult.Yes)
            //{
                try
                {
                    // habilitar este sector de código cuando se necesita habilitar la recepción en una ventana diferente
                    FComprasProductosRecepcion formRecepcion = new FComprasProductosRecepcion(this.NumeroAlmacen,
                    DIUsuario, NumeroCompraProducto);
                    DialogResult respuesta = formRecepcion.ShowDialog(this);
                    if (respuesta == DialogResult.OK)
                    {
                        if (DTCompraProductos.Count > 0)
                        {
                            DTCompraProductos[0].CodigoEstadoCompra = "F";
                            DTCompraProductos[0].FechaHoraRecepcion = Utilidades.DAOUtilidades.ObtenerFechaHoraServidor();
                            DTCompraProductos.AcceptChanges();
                        }

                        cBoxEstado.SelectedValue = "F";
                        habilitarBotonesAcciones(true, false, false, false, false, false, false, true);
                    }
                    else if (respuesta == DialogResult.Ignore)
                    {
                        if (DTCompraProductos.Count > 0)
                        {
                            DTCompraProductos[0].CodigoEstadoCompra = "D";
                            DTCompraProductos.AcceptChanges();
                        }
                        cBoxEstado.SelectedValue = "D";
                        habilitarBotonesAcciones(true, false, false, false, false, true, false, true);
                    }
                    formRecepcion.Dispose();



                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "No se Pudo Realizar la Operación Actual, ocurrió la Siguiente Excepción \r\n" + ex.Message,
                        "Error en Finalización de Ingresos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            //}
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
            ListarCompraProductoReporteTableAdapter TAComprasProductoReporte = new ListarCompraProductoReporteTableAdapter();
            TAComprasProductoReporte.Connection = Utilidades.DAOUtilidades.conexion;
            DataTable DTIngresoArticuloR = TAComprasProductoReporte.GetData(NumeroAlmacen, NumeroCompraProducto);
            DataTable DTIngresoArticuloDetalleR = TAComprasProductosDetalleMostrar.GetData(NumeroAlmacen, NumeroCompraProducto);
            FReporteTransaccionesGeneral formReporte = new FReporteTransaccionesGeneral();
            formReporte.cargarReporteComprasProductos(DTIngresoArticuloR, DTIngresoArticuloDetalleR);
            formReporte.ShowDialog(this);
            formReporte.Dispose();
        }

        private void pnlProductosBusqueda_Load(object sender, EventArgs e)
        {

        }

        private void cBoxProveedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(TipoOperacion))
            {
                int? CodigoProveedorSeleccionado = cBoxProveedor.SelectedIndex >= 0 ? int.Parse(cBoxProveedor.SelectedValue.ToString()) : (int?)null;

                pnlProductosBusqueda.CodigoProveedor = CodigoProveedorSeleccionado;
                if (CodigoProveedorSeleccionado.HasValue)
                {
                    pnlProductosBusqueda.PorcentajeGananciaMayor = DTProveedores.FindByCodigoProveedor(CodigoProveedorSeleccionado.Value).PorcentajeGananciaVentaPorMayor;
                    pnlProductosBusqueda.PorcentajeGananciaMenor = DTProveedores.FindByCodigoProveedor(CodigoProveedorSeleccionado.Value).PorcentajeGananciaVentaPorMenor;
                }
                pnlProductosBusqueda.txtTextoBusqueda.Text = " ";
                pnlProductosBusqueda.btnBuscar_Click(sender, e);
                actualizarPorcentajePorProveedor();
                
            }
        }

        public void actualizarPorcentajePorProveedor()
        {
            int? CodigoProveedorSeleccionado = cBoxProveedor.SelectedIndex >= 0 ? int.Parse(cBoxProveedor.SelectedValue.ToString()) : (int?)null;
            if (checkActualizarPorcentajeGanancia.Checked && CodigoProveedorSeleccionado != null)
            {
                decimal? PorcentajeGananciaVentaPorMayor = decimal.Parse(DTProveedores.FindByCodigoProveedor(CodigoProveedorSeleccionado.Value)["PorcentajeGananciaVentaPorMayor"].ToString() ?? "0");
                decimal? PorcentajeGananciaVentaPorMenor = decimal.Parse(DTProveedores.FindByCodigoProveedor(CodigoProveedorSeleccionado.Value)["PorcentajeGananciaVentaPorMenor"].ToString() ?? "0");
                foreach (AlvecoComercial10DataSet.DTProductosSeleccionadosRow DRProductosSeleccionados in DTProductosSeleccionados.Select("CodigoProveedor = " + CodigoProveedorSeleccionado))
                {
                    DRProductosSeleccionados["PorcentajeGananciaVentaPorMayor"] = PorcentajeGananciaVentaPorMayor ?? 0;
                    DRProductosSeleccionados["PorcentajeGananciaVentaPorMenor"] = PorcentajeGananciaVentaPorMenor ?? 0;

                    DRProductosSeleccionados["PrecioUnitarioVentaPorMenor"] = decimal.Round((DRProductosSeleccionados.PrecioUnitario * PorcentajeGananciaVentaPorMenor.Value / 100) + DRProductosSeleccionados.PrecioUnitario, 2);
                    DRProductosSeleccionados["PrecioUnitarioVentaPorMayor"] = decimal.Round((DRProductosSeleccionados.PrecioUnitario * PorcentajeGananciaVentaPorMayor.Value / 100) + DRProductosSeleccionados.PrecioUnitario, 2);
                    
                }
                dtGVCompraProductos.RefreshEdit();

                //DTProductosSeleccionados.AcceptChanges();
            }
            else
            {
                DTProductosSeleccionados.RejectChanges();
            }
        }

        private void cBoxProveedor_KeyDown(object sender, KeyEventArgs e)
        {
            if (!String.IsNullOrEmpty(TipoOperacion) && e.KeyCode == Keys.Escape)
            {
                cBoxProveedor.SelectedIndex = -1;
            }
        }

        private void checkActualizarPorcentajeGanancia_CheckedChanged(object sender, EventArgs e)
        {
            actualizarPorcentajePorProveedor();
        }

        private void tsBtnEliminar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Se encuentra seguro de eliminar el Registro Actual?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    TACompraProductos.Delete(NumeroAlmacen, NumeroCompraProducto);
                    //MessageBox.Show(this, "Registro eliminado correctamente", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cargarDatosCompraProducto(-1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Ocurrió la siguiente Excepcion " + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}
