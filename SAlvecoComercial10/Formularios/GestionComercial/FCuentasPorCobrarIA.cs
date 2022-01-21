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
    public partial class FCuentasPorCobrarIA : Form
    {
        private int NumeroAlmacen;
        private string DIUsuario;        
        private CuentasPorCobrarCobrosTableAdapter TACuentasPorCobrarCobros;
        private CuentasPorCobrarTableAdapter TACuentasPorCobrar;
        private ClientesTableAdapter TAClientes;
        private ConceptosTableAdapter TAConceptos;

        AlvecoComercial10DataSet.CuentasPorCobrarCobrosDataTable DTCuentasPorCobrarCobros;
        AlvecoComercial10DataSet.CuentasPorCobrarDataTable DTCuentasPorCobrar;
        AlvecoComercial10DataSet.ClientesDataTable DTClientes;
        AlvecoComercial10DataSet.ConceptosDataTable DTConceptos;

        public int NumeroCuentaPorCobrar { get; set; }
        private string TipoOperacion = "";
        ErrorProvider eProviderCuentasPorCobrar;
        private bool soloInsertarEditar = false;

        public FCuentasPorCobrarIA(int NumeroAlmacen, string DIUsuario)
        {
            InitializeComponent();
            this.NumeroAlmacen = NumeroAlmacen;
            this.DIUsuario = DIUsuario;

            TACuentasPorCobrar = new CuentasPorCobrarTableAdapter();
            TACuentasPorCobrar.Connection = Utilidades.DAOUtilidades.conexion;
            TACuentasPorCobrarCobros = new CuentasPorCobrarCobrosTableAdapter();
            TACuentasPorCobrarCobros.Connection = Utilidades.DAOUtilidades.conexion;
            TAClientes = new ClientesTableAdapter();
            TAClientes.Connection = Utilidades.DAOUtilidades.conexion;
            TAConceptos = new ConceptosTableAdapter();
            TAConceptos.Connection = Utilidades.DAOUtilidades.conexion;

            DTCuentasPorCobrarCobros = new AlvecoComercial10DataSet.CuentasPorCobrarCobrosDataTable();
            DTCuentasPorCobrar = new AlvecoComercial10DataSet.CuentasPorCobrarDataTable();
            DTClientes = new AlvecoComercial10DataSet.ClientesDataTable();
            DTConceptos = new AlvecoComercial10DataSet.ConceptosDataTable();
            eProviderCuentasPorCobrar = new ErrorProvider();


            DTClientes = TAClientes.GetData();
            cBoxCliente.DataSource = DTClientes;
            cBoxCliente.DisplayMember = "NombreCliente";
            cBoxCliente.ValueMember = "CodigoCliente";
            cBoxCliente.SelectedIndex = -1;


            DTConceptos = TAConceptos.GetData();
            cboxConcepto.DataSource = DTConceptos;
            cboxConcepto.DisplayMember = "Concepto";
            cboxConcepto.ValueMember = "NumeroConcepto";
            cboxConcepto.SelectedIndex = -1;

            Utilidades.ObjetoCodigoDescripcion EstadosCuentasPorCobrar = new Utilidades.ObjetoCodigoDescripcion();
            EstadosCuentasPorCobrar.cargarDatosEstadosCuentasPorPagarPorCobrar();
            cboxEstado.DataSource = EstadosCuentasPorCobrar.listaObjetos;
            cboxEstado.DisplayMember = EstadosCuentasPorCobrar.DisplayMember;
            cboxEstado.ValueMember = EstadosCuentasPorCobrar.ValueMember;
            cboxEstado.SelectedIndex = -1;
            cboxEstado.Enabled = false;

            lblMontoPagado.Text = String.Empty;
            DTCuentasPorCobrarCobros.RowChanging += new DataRowChangeEventHandler(DTCuentasPorCobrarCobros_RowChanged);
            DTCuentasPorCobrarCobros.RowDeleting += new DataRowChangeEventHandler(DTCuentasPorCobrarCobros_RowChanged);
            cargarDatosCuentaPorCobrar(-1);
            this.Shown += new EventHandler(FCuentasPorCobrarIA_Shown);

            //btnEliminarPago.Visible = false;
        }

        public void FCuentasPorCobrarIA_Shown(object sender, EventArgs e)
        {
            btnAceptar.Focus();
        }

        void DTCuentasPorCobrarCobros_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            object total = DTCuentasPorCobrarCobros.Compute("Sum(Monto)", "");
            lblMontoPagado.Text = "Cobrado " + (String.IsNullOrEmpty(total.ToString()) ? "0.00" : total.ToString()) + " Bs";
        }

        public void configurarFormularioIA(int? NumeroCuentaPorCobrar)
        {

            cargarDatosCuentaPorCobrar(NumeroCuentaPorCobrar != null ? NumeroCuentaPorCobrar.Value : -1);
            if (NumeroCuentaPorCobrar == null)
            {
                int NumeroCuentaPorCobrarSiguiente = Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("CuentasPorCobrar") == -1
                ? 1 : Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("CuentasPorCobrar") + 1;
                txtNroCuentaPorCobrar.Text = NumeroCuentaPorCobrarSiguiente.ToString();
            }
            TipoOperacion = NumeroCuentaPorCobrar == null ? "I" : "E";
            soloInsertarEditar = true;
            splitContainer1.Panel2Collapsed = true;
            //290
            btnEditar.Visible = btnNuevo.Visible = btnEliminar.Visible = false;
            this.Size = new Size(splitContainer1.SplitterDistance +10, this.Size.Height);
            cboxEstado.SelectedValue = "D";            
            habilitarControles(true);
            habilitarControlesBotones(false, true, false, false, true, false, false);
            this.dateFechaLimite.Value = this.dateFechaLimite.Value.AddDays(10);
        }

        public void cargarDatosMontoTotalVenta(decimal montoTota, int codigoProveedor, string observaciones)
        {
            txtMonto.Text = montoTota.ToString();
            txtMonto.ReadOnly = true;
            txtObservaciones.Text = observaciones;
            cboxConcepto.SelectedValue = "2";
            cboxConcepto.Enabled = false;
            btnAgregarConcepto.Enabled = false;
            cBoxCliente.SelectedValue = codigoProveedor;
            cBoxCliente.Enabled = false;
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
            this.txtNroCuentaPorCobrar.Text = String.Empty;
            this.txtObservaciones.Text = String.Empty;
            cboxConcepto.SelectedIndex = -1;
            cboxEstado.SelectedIndex = -1;
            cBoxCliente.SelectedIndex = -1;
        }

        public void habilitarControles(bool estadoHabilitacion)
        {
            this.txtMonto.ReadOnly = !estadoHabilitacion;
            this.txtObservaciones.ReadOnly = !estadoHabilitacion;
            cboxConcepto.Enabled = estadoHabilitacion;
            cBoxCliente.Enabled = estadoHabilitacion;
            btnAgregarConcepto.Enabled = estadoHabilitacion;
            dateFechaLimite.Enabled = estadoHabilitacion;
        }

        public bool validarControles()
        {

            if (txtMonto.ReadOnly && cBoxCliente.SelectedIndex < 0)
            {
                eProviderCuentasPorCobrar.SetError(cBoxCliente, "Aun no ha seleccionado el  Proveedor");
                cBoxCliente.Focus();
                return false;
            }

            if (cboxConcepto.SelectedIndex < 0)
            {
                eProviderCuentasPorCobrar.SetError(cboxConcepto, "Aun no ha seleccionado el  Concepto");
                cboxConcepto.Focus();
                return false;
            }

            if (String.IsNullOrEmpty(txtMonto.Text.Trim()))
            {
                eProviderCuentasPorCobrar.SetError(txtMonto, "Aún no ha ingresado el Monto");
                txtMonto.Focus();
                txtMonto.SelectAll();
                return false;
            }
            decimal Monto = -1;
            if (!decimal.TryParse(txtMonto.Text, out Monto) || Monto <= 0)
            {
                eProviderCuentasPorCobrar.SetError(txtMonto, "El Monto se encuentra mal escrito");
                txtMonto.Focus();
                txtMonto.SelectAll();
                return false;
            }

            DateTime FechaHoraServidor = Utilidades.DAOUtilidades.ObtenerFechaHoraServidor();
            if (dateFechaLimite.Value < FechaHoraServidor && TipoOperacion == "I")
            {
                eProviderCuentasPorCobrar.SetError(dateFechaLimite, "La Fecha Introducida es Menor a la Actual");
                dateFechaLimite.Focus();
                return false;
            }
            return true;
        }

        public void cargarDatosCuentaPorCobrar(int NumeroCuentaPorCobrar)
        {
            DTCuentasPorCobrar = TACuentasPorCobrar.GetDataBy1(NumeroCuentaPorCobrar);
            if (DTCuentasPorCobrar.Count != 0)
            {
                DTCuentasPorCobrarCobros = TACuentasPorCobrarCobros.GetData(NumeroCuentaPorCobrar);
                bdSourceCuentasPorCobrarCobros.DataSource = DTCuentasPorCobrarCobros;
                DTCuentasPorCobrarCobros.RowChanging += new DataRowChangeEventHandler(DTCuentasPorCobrarCobros_RowChanged);
                DTCuentasPorCobrarCobros.RowDeleting += new DataRowChangeEventHandler(DTCuentasPorCobrarCobros_RowChanged);

                object total = DTCuentasPorCobrarCobros.Compute("Sum(Monto)", "");
                lblMontoPagado.Text = "Cobrado " + (String.IsNullOrEmpty(total.ToString()) ? "0.00" : total.ToString()) + " Bs";

                txtNroCuentaPorCobrar.Text = NumeroCuentaPorCobrar.ToString();
                txtMonto.Text = DTCuentasPorCobrar[0].Monto.ToString();
                txtObservaciones.Text = DTCuentasPorCobrar[0].Observaciones;
                cboxConcepto.SelectedValue = DTCuentasPorCobrar[0].NumeroConcepto;
                cboxEstado.SelectedValue = DTCuentasPorCobrar[0].CodigoEstado;
                if (DTCuentasPorCobrar[0].IsCodigoClienteNull())
                    cBoxCliente.SelectedIndex = -1;
                else
                    cBoxCliente.SelectedValue = DTCuentasPorCobrar[0].CodigoCliente;
                this.NumeroCuentaPorCobrar = NumeroCuentaPorCobrar;
                this.tsLblFechaHoraRegistro.Text = DTCuentasPorCobrar[0].FechaHoraRegistro.ToString();
                this.tsLblNroAgencia.Text = DTCuentasPorCobrar[0].NumeroAlmacen.ToString();

                habilitarControlesBotones(true, false, true, true, false, DTCuentasPorCobrar[0].CodigoEstado != "P", DTCuentasPorCobrar[0].CodigoEstado != "P");
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
            txtNroCuentaPorCobrar.Text = (Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("CuentasPorCobrar") + 1).ToString();
            cboxEstado.SelectedValue = "D";
            this.dateFechaLimite.Value = this.dateFechaLimite.Value.AddDays(10);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            eProviderCuentasPorCobrar.Clear();
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
                    TACuentasPorCobrar.Delete(NumeroCuentaPorCobrar);
                    limpiarControles();
                    DTCuentasPorCobrarCobros.Clear();
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
            eProviderCuentasPorCobrar.Clear();
            if (validarControles())
            {
                try
                {
                    int NumeroCuentaAux = int.Parse(txtNroCuentaPorCobrar.Text);
                    if (TipoOperacion == "I")
                    {
                        TACuentasPorCobrar.Insert(NumeroAlmacen, null, int.Parse(cboxConcepto.SelectedValue.ToString()),
                            cBoxCliente.SelectedIndex >= 0 ? int.Parse(cBoxCliente.SelectedValue.ToString()) : (int?)null, 
                            decimal.Parse(txtMonto.Text),
                            dateFechaLimite.Value, cboxEstado.SelectedValue.ToString(), txtObservaciones.Text, DIUsuario);
                    }
                    else
                    {
                        TACuentasPorCobrar.ActualizarCuentaPorCobrar(NumeroCuentaPorCobrar, NumeroAlmacen, DateTime.Now, int.Parse(cboxConcepto.SelectedValue.ToString()),
                            cBoxCliente.SelectedIndex >= 0 ? int.Parse(cBoxCliente.SelectedValue.ToString()) : (int?)null, 
                            decimal.Parse(txtMonto.Text),
                            dateFechaLimite.Value, cboxEstado.SelectedValue.ToString(), txtObservaciones.Text, DIUsuario);
                    }

                    //MessageBox.Show(this, "Cuenta por Cobrar" + (TipoOperacion == "I" ? "Registrada" : "Actualizada") + " correctamente", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    habilitarControlesBotones(true, false, true, true, false, true, false);
                    if (soloInsertarEditar)
                    {
                        NumeroCuentaPorCobrar = NumeroCuentaAux;
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
            if (string.IsNullOrEmpty(txtNroCuentaPorCobrar.Text))
            {
                MessageBox.Show("No existe ninguna cuenta seleccionada");
                return;
            }
            Utilidades.FIngresarCantidad formCantidad = new Utilidades.FIngresarCantidad(true);
            decimal MontoMaximo = DTCuentasPorCobrarCobros.Count == 0 ? 0 : decimal.Parse(DTCuentasPorCobrarCobros.Compute("Sum(Monto)", "").ToString());

            decimal MontoCuentaPorCobrar = decimal.Parse(txtMonto.Text);
            if (MontoMaximo >= MontoCuentaPorCobrar)
            {
                MessageBox.Show(this, "La cuenta por Cobrar actual ya ha sido completamente pagada", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            formCantidad.MontoTopeMaximo = MontoCuentaPorCobrar - MontoMaximo;
            if (formCantidad.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    TACuentasPorCobrarCobros.Insert(NumeroCuentaPorCobrar, null, formCantidad.MontoDecimalIngresado, DIUsuario);
                    AlvecoComercial10DataSet.CuentasPorCobrarCobrosRow DRCuentaPorCobrarPago = DTCuentasPorCobrarCobros.AddCuentasPorCobrarCobrosRow(NumeroCuentaPorCobrar,
                        Utilidades.DAOUtilidades.ObtenerFechaHoraServidor(), formCantidad.MontoDecimalIngresado, DIUsuario);
                    DRCuentaPorCobrarPago.NumeroCobro = Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("CuentasPorCobrarCobros");
                    DTCuentasPorCobrarCobros.AcceptChanges();
                    btnEliminarPago.Enabled = true;

                    MontoMaximo = DTCuentasPorCobrarCobros.Count == 0 ? 0 : decimal.Parse(DTCuentasPorCobrarCobros.Compute("Sum(Monto)", "").ToString());

                    if (MontoMaximo == MontoCuentaPorCobrar)
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
            if (bdSourceCuentasPorCobrarCobros.Current != null
                && MessageBox.Show(this, "¿Se encuentra seguro de Eliminar el Registro?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    TACuentasPorCobrarCobros.Delete(DTCuentasPorCobrarCobros[bdSourceCuentasPorCobrarCobros.Position].NumeroCobro);
                    DTCuentasPorCobrarCobros.Rows.RemoveAt(bdSourceCuentasPorCobrarCobros.Position);
                    DTCuentasPorCobrarCobros.AcceptChanges();
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

        internal void DeshabilitarBotones()
        {
            flPnlBotones.Visible = false;
        }



        internal void ConfigurarVerCobros()
        {
            splitContainer1.Panel1Collapsed = true;
            pnlBotonesDetalle.Visible = false;
            flPnlBotones.Visible = false;
            this.Size = new Size(splitContainer1.Size.Width - splitContainer1.SplitterDistance + 10, this.Size.Height);
        }

        public void configurarParaCobros()
        {
            splitContainer1.Panel1Collapsed = true;            
            flPnlBotones.Visible = false;
            this.Size = new Size(splitContainer1.Size.Width - splitContainer1.SplitterDistance + 10, this.Size.Height);
        }
    }
}
