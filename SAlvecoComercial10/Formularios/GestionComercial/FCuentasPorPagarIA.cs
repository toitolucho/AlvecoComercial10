using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SAlvecoComercial10.AccesoDatos;
using SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSetTableAdapters;
using SAlvecoComercial10.Formularios.GestionSistema;


namespace SAlvecoComercial10.Formularios.GestionComercial
{
    public partial class FCuentasPorPagarIA : Form
    {
        private int NumeroAlmacen;
        private string DIUsuario;
        private CuentasPorPagarPagosTableAdapter TACuentasPorPagarPagos;
        private CuentasPorPagarTableAdapter TACuentasPorPagar;
        private ProveedoresTableAdapter TAProveedores;
        private ConceptosTableAdapter TAConceptos;

        AlvecoComercial10DataSet.CuentasPorPagarPagosDataTable DTCuentasPorPagarPagos;
        AlvecoComercial10DataSet.CuentasPorPagarDataTable DTCuentasPorPagar;
        AlvecoComercial10DataSet.ProveedoresDataTable DTProveedores;
        AlvecoComercial10DataSet.ConceptosDataTable DTConceptos;

        public int NumeroCuentaPorPagar { get; set; }
        private string TipoOperacion ="";
        ErrorProvider eProviderCuentasPorPagar;
        private bool soloInsertarEditar = false;

        public FCuentasPorPagarIA(int NumeroAlmacen, string DIUsuario)
        {
            InitializeComponent();
            this.NumeroAlmacen = NumeroAlmacen;
            this.DIUsuario = DIUsuario;

            TACuentasPorPagar = new CuentasPorPagarTableAdapter();
            TACuentasPorPagar.Connection = Utilidades.DAOUtilidades.conexion;
            TACuentasPorPagarPagos = new CuentasPorPagarPagosTableAdapter();
            TACuentasPorPagarPagos.Connection = Utilidades.DAOUtilidades.conexion;
            TAProveedores = new ProveedoresTableAdapter();
            TAProveedores.Connection = Utilidades.DAOUtilidades.conexion;
            TAConceptos = new ConceptosTableAdapter();
            TAConceptos.Connection = Utilidades.DAOUtilidades.conexion;

            DTCuentasPorPagarPagos = new AlvecoComercial10DataSet.CuentasPorPagarPagosDataTable();
            DTCuentasPorPagar = new AlvecoComercial10DataSet.CuentasPorPagarDataTable();
            DTProveedores = new AlvecoComercial10DataSet.ProveedoresDataTable();
            DTConceptos = new AlvecoComercial10DataSet.ConceptosDataTable();
            eProviderCuentasPorPagar = new ErrorProvider();


            DTProveedores = TAProveedores.GetData();
            cBoxProveedor.DataSource = DTProveedores;
            cBoxProveedor.DisplayMember = "NombreRazonSocial";
            cBoxProveedor.ValueMember = "CodigoProveedor";
            cBoxProveedor.SelectedIndex = -1;


            DTConceptos = TAConceptos.GetData();
            cboxConcepto.DataSource = DTConceptos;
            cboxConcepto.DisplayMember = "Concepto";
            cboxConcepto.ValueMember = "NumeroConcepto";
            cboxConcepto.SelectedIndex = -1;

            Utilidades.ObjetoCodigoDescripcion EstadosCuentasPorPagar = new Utilidades.ObjetoCodigoDescripcion();
            EstadosCuentasPorPagar.cargarDatosEstadosCuentasPorPagarPorCobrar();
            cboxEstado.DataSource = EstadosCuentasPorPagar.listaObjetos;
            cboxEstado.DisplayMember = EstadosCuentasPorPagar.DisplayMember;
            cboxEstado.ValueMember = EstadosCuentasPorPagar.ValueMember;
            cboxEstado.SelectedIndex = -1;
            cboxEstado.Enabled = false;

            lblMontoPagado.Text = String.Empty;
            DTCuentasPorPagarPagos.RowChanging += new DataRowChangeEventHandler(DTCuentasPorPagarPagos_RowChanged);
            DTCuentasPorPagarPagos.RowDeleting += new DataRowChangeEventHandler(DTCuentasPorPagarPagos_RowChanged);
            Shown += new EventHandler(FCuentasPorPagarIA_Shown);
            cargarDatosCuentaPorPagar(-1);

            //btnEliminarPago.Visible = false;
        }

        void FCuentasPorPagarIA_Shown(object sender, EventArgs e)
        {
            btnAceptar.Focus();
        }

        void DTCuentasPorPagarPagos_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            object total = DTCuentasPorPagarPagos.Compute("Sum(Monto)","");
            lblMontoPagado.Text = "Total Pagado " + (String.IsNullOrEmpty(total.ToString()) ? "0.00" : total.ToString()) + " Bs";
        }

        public void configurarFormularioIA(int? NumeroCuentaPorPagar)
        {

            cargarDatosCuentaPorPagar(NumeroCuentaPorPagar != null ? NumeroCuentaPorPagar.Value : -1);
            if (NumeroCuentaPorPagar == null)
            {
                int NumeroCuentaPorPagarSiguiente = Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("CuentasPorPagar") == -1
                ? 1 : Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("CuentasPorPagar") + 1;
                txtNroCuentaPorPagar.Text = NumeroCuentaPorPagarSiguiente.ToString();
            }
            TipoOperacion = NumeroCuentaPorPagar == null ? "I" : "E";
            soloInsertarEditar = true;            
            splitContainer1.Panel2Collapsed = true;
            //290
            btnEditar.Visible = btnNuevo.Visible = btnEliminar.Visible = false;
            this.Size = new Size(splitContainer1.SplitterDistance + 10, this.Size.Height);
            cboxEstado.SelectedValue = "D";
            habilitarControles(true);
            habilitarControlesBotones(false, true, false, false, true, false, false);
            this.dateFechaLimite.Value = this.dateFechaLimite.Value.AddDays(10);
        }

        public void cargarDatosMontoTotalCompra(decimal montoTota, int codigoProveedor, string observaciones)
        {
            txtMonto.Text = montoTota.ToString();
            txtMonto.ReadOnly = true;
            cBoxProveedor.SelectedValue = codigoProveedor;
            cBoxProveedor.Enabled = false;
            cboxConcepto.SelectedValue = "1";
            cboxConcepto.Enabled = false;
            btnAgregarConcepto.Enabled = false;
            txtObservaciones.Text = observaciones;
            this.dateFechaLimite.Value = this.dateFechaLimite.Value.AddDays(10);
            btnAceptar.Focus();
        }

        public void habilitarControlesBotones(bool nuevo, bool cancelar, bool editar, bool eliminar, bool aceptar, bool nuevoPago, bool eliminarPago)
        {
            this.btnAceptar.Enabled = aceptar;
            this.btnCancelar.Enabled = cancelar;
            this.btnEditar.Enabled = editar;
            this.btnEliminar.Enabled = eliminar;
            this.btnNuevo.Enabled = nuevo;
            this.btnNuevoPago.Enabled = nuevoPago;
            this.btnEliminarPago.Enabled = eliminarPago;
        }

        public void limpiarControles()
        {
            this.txtMonto.Text = String.Empty;
            this.txtNroCuentaPorPagar.Text = String.Empty;
            this.txtObservaciones.Text = String.Empty;
            cboxConcepto.SelectedIndex = -1;
            cboxEstado.SelectedIndex = -1;
            cBoxProveedor.SelectedIndex = -1;
        }

        public void habilitarControles(bool estadoHabilitacion)
        {
            this.txtMonto.ReadOnly = !estadoHabilitacion;
            this.txtObservaciones.ReadOnly = !estadoHabilitacion;
            cboxConcepto.Enabled = estadoHabilitacion;            
            cBoxProveedor.Enabled = estadoHabilitacion;
            btnAgregarConcepto.Enabled = estadoHabilitacion;
            dateFechaLimite.Enabled = estadoHabilitacion;
        }

        public bool validarControles()
        {

            if (txtMonto.ReadOnly && cBoxProveedor.SelectedIndex < 0)
            {
                eProviderCuentasPorPagar.SetError(cBoxProveedor, "Aun no ha seleccionado el  Proveedor");
                cBoxProveedor.Focus();
                return false;
            }

            if (cboxConcepto.SelectedIndex < 0)
            {
                eProviderCuentasPorPagar.SetError(cboxConcepto, "Aun no ha seleccionado el  Concepto");
                cboxConcepto.Focus();
                return false;
            }

            if (String.IsNullOrEmpty(txtMonto.Text.Trim()))
            {
                eProviderCuentasPorPagar.SetError(txtMonto, "Aún no ha ingresado el Monto");
                txtMonto.Focus();
                txtMonto.SelectAll();
                return false;
            }
            decimal Monto = -1;
            if (!decimal.TryParse(txtMonto.Text, out Monto) || Monto <= 0)
            {
                eProviderCuentasPorPagar.SetError(txtMonto, "El Monto se encuentra mal escrito");
                txtMonto.Focus();
                txtMonto.SelectAll();
                return false;
            }

            DateTime FechaHoraServidor = Utilidades.DAOUtilidades.ObtenerFechaHoraServidor();
            if (dateFechaLimite.Value < FechaHoraServidor  && TipoOperacion == "I")
            {
                eProviderCuentasPorPagar.SetError(dateFechaLimite, "La Fecha Introducida es Menor a la Actual");
                dateFechaLimite.Focus();
                return false;
            }
            return true;
        }

        public void DeshabilitarBotones()
        {
            flPnlBotones.Visible = false;
        }

        public void cargarDatosCuentaPorPagar(int NumeroCuentaPorPagar)
        {
            DTCuentasPorPagar = TACuentasPorPagar.GetDataBy1(NumeroCuentaPorPagar);
            if (DTCuentasPorPagar.Count != 0)
            {
                DTCuentasPorPagarPagos = TACuentasPorPagarPagos.GetData(NumeroCuentaPorPagar);
                bdSourceCuentasPorPagarPagos.DataSource = DTCuentasPorPagarPagos;
                DTCuentasPorPagarPagos.RowChanging += new DataRowChangeEventHandler(DTCuentasPorPagarPagos_RowChanged);
                DTCuentasPorPagarPagos.RowDeleting += new DataRowChangeEventHandler(DTCuentasPorPagarPagos_RowChanged);

                object total = DTCuentasPorPagarPagos.Compute("Sum(Monto)", "");
                lblMontoPagado.Text = "Total Pagado " + (String.IsNullOrEmpty(total.ToString()) ? "0.00" : total.ToString()) + " Bs";

                txtNroCuentaPorPagar.Text = NumeroCuentaPorPagar.ToString();
                txtMonto.Text = DTCuentasPorPagar[0].Monto.ToString();
                txtObservaciones.Text = DTCuentasPorPagar[0].Observaciones;
                cboxConcepto.SelectedValue = DTCuentasPorPagar[0].NumeroConcepto;
                cboxEstado.SelectedValue = DTCuentasPorPagar[0].CodigoEstado;
                if (DTCuentasPorPagar[0].IsCodigoProveedorNull())
                    cBoxProveedor.SelectedIndex = -1;
                else
                    cBoxProveedor.SelectedValue = DTCuentasPorPagar[0].CodigoProveedor;
                this.NumeroCuentaPorPagar = NumeroCuentaPorPagar;
                this.tsLblFechaHoraRegistro.Text = DTCuentasPorPagar[0].FechaHoraRegistro.ToString();
                this.tsLblNroAgencia.Text = DTCuentasPorPagar[0].NumeroAlmacen.ToString();

                habilitarControlesBotones(true, false, true, true, false, DTCuentasPorPagar[0].CodigoEstado != "P", DTCuentasPorPagar[0].CodigoEstado != "P");
            }
            else
            {
                habilitarControles(false);
                habilitarControlesBotones(true, false, false, false, true, false, false);
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            TipoOperacion = "I";
            habilitarControles(true);
            habilitarControlesBotones(false, true, false, false, true, false, false);
            limpiarControles();
            txtNroCuentaPorPagar.Text = (Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("CuentasPorPagar") + 1).ToString();
            cboxEstado.SelectedValue = "D";
            this.dateFechaLimite.Value = this.dateFechaLimite.Value.AddDays(10);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            eProviderCuentasPorPagar.Clear();
            TipoOperacion = "";
            habilitarControles(false);
            limpiarControles();
            habilitarControlesBotones(true, false, false, false, false, false, false);
            if (soloInsertarEditar)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                Close();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            TipoOperacion = "E";
            habilitarControles(true);
            habilitarControlesBotones(false, true, false, false, true, false, false);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "¿Se encuentra seguro de eliminar el registro Actual?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    TACuentasPorPagar.Delete(NumeroCuentaPorPagar);
                    limpiarControles();
                    DTCuentasPorPagarPagos.Clear();
                    habilitarControlesBotones(true, false, false, false, false, false, false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "No se pudo eliminar el registro actual debido a que ocurrio la siguiente excepcion " + ex.Message,
                        this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            eProviderCuentasPorPagar.Clear();
            if (validarControles())
            {
                try
                {
                    int NumeroCuentaAux = int.Parse(txtNroCuentaPorPagar.Text);
                    if (TipoOperacion == "I")
                    {
                        TACuentasPorPagar.Insert(NumeroAlmacen, null, int.Parse(cboxConcepto.SelectedValue.ToString()),
                            cBoxProveedor.SelectedIndex >= 0 ? int.Parse(cBoxProveedor.SelectedValue.ToString()) : (int?)null, 
                            decimal.Parse(txtMonto.Text),
                            dateFechaLimite.Value, cboxEstado.SelectedValue.ToString(), txtObservaciones.Text, DIUsuario);
                    }
                    else
                    {
                        TACuentasPorPagar.ActualizarCuentaPorPagar(NumeroCuentaPorPagar, NumeroAlmacen, DateTime.Now, int.Parse(cboxConcepto.SelectedValue.ToString()),
                            cBoxProveedor.SelectedIndex >= 0 ? int.Parse(cBoxProveedor.SelectedValue.ToString()) : (int?)null, 
                            decimal.Parse(txtMonto.Text), dateFechaLimite.Value, 
                            cboxEstado.SelectedValue.ToString(), txtObservaciones.Text, DIUsuario);
                    }

                    //MessageBox.Show(this, "Cuenta por Pagar" + (TipoOperacion == "I" ? "Registrada" : "Actualizada") + " correctamente", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    habilitarControlesBotones(true, false, true, true, false, true, false);
                    if (soloInsertarEditar)
                    {
                        NumeroCuentaPorPagar = NumeroCuentaAux;
                        DialogResult = System.Windows.Forms.DialogResult.OK;
                        this.Close();

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Ocurrió la siguiente Excepción " + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);                    
                }
            }
        }

        private void btnNuevoPago_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNroCuentaPorPagar.Text))
            {
                MessageBox.Show("No existe ninguna cuenta seleccionada");
                return;
            }
            Utilidades.FIngresarCantidad formCantidad = new Utilidades.FIngresarCantidad(true);
            decimal MontoMaximo = DTCuentasPorPagarPagos.Count == 0 ? 0 : decimal.Parse(DTCuentasPorPagarPagos.Compute("Sum(Monto)", "").ToString());
            
            decimal MontoCuentaPorPagar = decimal.Parse(txtMonto.Text);
            if (MontoMaximo >= MontoCuentaPorPagar)
            {
                MessageBox.Show(this, "La cuenta por Pagar actual ya ha sido completamente pagada", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            formCantidad.MontoTopeMaximo = MontoCuentaPorPagar - MontoMaximo;
            if (formCantidad.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    TACuentasPorPagarPagos.Insert(NumeroCuentaPorPagar, null, formCantidad.MontoDecimalIngresado, DIUsuario);
                    AlvecoComercial10DataSet.CuentasPorPagarPagosRow DRCuentaPorPagarPago = DTCuentasPorPagarPagos.AddCuentasPorPagarPagosRow(NumeroCuentaPorPagar,
                        Utilidades.DAOUtilidades.ObtenerFechaHoraServidor(), formCantidad.MontoDecimalIngresado, DIUsuario);
                    DRCuentaPorPagarPago.NumeroPago = Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("CuentasPorPagarPagos");
                    DTCuentasPorPagarPagos.AcceptChanges();
                    btnEliminarPago.Enabled = true;

                    MontoMaximo = DTCuentasPorPagarPagos.Count == 0 ? 0 : decimal.Parse(DTCuentasPorPagarPagos.Compute("Sum(Monto)", "").ToString());

                    if (MontoMaximo == MontoCuentaPorPagar)
                    {
                        btnNuevoPago.Enabled = false;
                        btnEliminarPago.Enabled = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrio la siguiente excepcion " + ex.Message);
                }
            }
        }

        private void btnEliminarPago_Click(object sender, EventArgs e)
        {
            if (bdSourceCuentasPorPagarPagos.Current != null
                && MessageBox.Show(this, "¿Se encuentra seguro de Eliminar el Registro?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    TACuentasPorPagarPagos.Delete(DTCuentasPorPagarPagos[bdSourceCuentasPorPagarPagos.Position].NumeroPago);
                    DTCuentasPorPagarPagos.Rows.RemoveAt(bdSourceCuentasPorPagarPagos.Position);
                    DTCuentasPorPagarPagos.AcceptChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "No se pudo realizar la operación actual, ocurrió la siguiente Excepcion " + ex.Message,
                        this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No existe ningún pago seleccionado");
            }
        }

        private void btnAgregarConcepto_Click(object sender, EventArgs e)
        {
            FConceptos formConceptos = new FConceptos();
            formConceptos.configurarFormularioIA(null);
            if (formConceptos.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (DTConceptos.FindByNumeroConcepto(formConceptos.NumeroConcepto) == null)
                {
                    DataTable DTConceptosAux = TAConceptos.GetDataBy1(formConceptos.NumeroConcepto);
                    if (DTConceptosAux.Rows.Count > 0)
                    {
                        DTConceptos.Rows.Add(TAConceptos.GetDataBy1(formConceptos.NumeroConcepto)[0].ItemArray);
                        DTConceptos.DefaultView.Sort = "Concepto ASC";
                        cboxConcepto.SelectedValue = formConceptos.NumeroConcepto;
                    }
                }
            }
            formConceptos.Dispose();
        }

        internal void ConfigurarVerPagos()
        {
            splitContainer1.Panel1Collapsed = true;
            pnlBotonesDetalle.Visible = false;
            flPnlBotones.Visible = false;
            this.Size = new Size(splitContainer1.Size.Width - splitContainer1.SplitterDistance + 10, this.Size.Height);
        }

        public void configurarParaPagos()
        {
            splitContainer1.Panel1Collapsed = true;
            flPnlBotones.Visible = false;
            this.Size = new Size(splitContainer1.Size.Width - splitContainer1.SplitterDistance + 10, this.Size.Height);
        }
    }
}
