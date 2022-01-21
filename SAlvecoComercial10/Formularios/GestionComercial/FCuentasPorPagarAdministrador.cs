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
    public partial class FCuentasPorPagarAdministrador : Form
    {
        private string DIUsuario;
        private int NumeroAlmacen;
        public int NumeroCuentaPorPagarActual { get; set; }


        ListarCuentasPorCobrarPagarTableAdapter TAListarCuentasPorPagar;
        ListarCuentasPorCobrarPagarDetalleTableAdapter TAListarCuentasPorPagarDetalle;
        AlvecoComercial10DataSet.ListarCuentasPorCobrarPagarDataTable DTListarCuentasPorPagar;
        CuentasPorPagarTableAdapter TACuentasPorPagar;
        string TipoOperacion;
        
        DgvFilterPopup.DgvFilterManager DGVFilterManagerCuentasPorPagar;

        public FCuentasPorPagarAdministrador(int NumeroAlmacen, string DIUsuario)
        {
            InitializeComponent();

            this.NumeroAlmacen = NumeroAlmacen;
            this.DIUsuario = DIUsuario;
            TAListarCuentasPorPagar = new ListarCuentasPorCobrarPagarTableAdapter();
            TAListarCuentasPorPagar.Connection = Utilidades.DAOUtilidades.conexion;
            TACuentasPorPagar = new CuentasPorPagarTableAdapter();
            TACuentasPorPagar.Connection = Utilidades.DAOUtilidades.conexion;
            TAListarCuentasPorPagarDetalle = new ListarCuentasPorCobrarPagarDetalleTableAdapter();
            TAListarCuentasPorPagarDetalle.Connection = Utilidades.DAOUtilidades.conexion;

            DTListarCuentasPorPagar = new AlvecoComercial10DataSet.ListarCuentasPorCobrarPagarDataTable();
            DTListarCuentasPorPagar = TAListarCuentasPorPagar.GetData(NumeroAlmacen, null, null, null, null, "P");
            dtGVCuentas.AutoGenerateColumns = false;
            bdSourceCuentas.DataSource = DTListarCuentasPorPagar;
            dtGVCuentas.DataSource = bdSourceCuentas;

            Utilidades.ObjetoCodigoDescripcion EstadosCuentas = new Utilidades.ObjetoCodigoDescripcion();
            EstadosCuentas.cargarDatosEstadosCuentasPorPagarPorCobrar();
            cBoxEstadoCuenta.DataSource = EstadosCuentas.listaObjetos;
            cBoxEstadoCuenta.DisplayMember = EstadosCuentas.DisplayMember;
            cBoxEstadoCuenta.ValueMember = EstadosCuentas.ValueMember;
            cBoxEstadoCuenta.SelectedIndex = -1;

            dateFechaFin.Checked = dateFechaInicio.Checked = false;

            DGVFilterManagerCuentasPorPagar = new DgvFilterPopup.DgvFilterManager();
            DGVFilterManagerCuentasPorPagar.DataGridView = this.dtGVCuentas;
            DTListarCuentasPorPagar.DefaultView.ListChanged += new ListChangedEventHandler(DefaultView_ListChanged);                                    
            CargarResumenCuentasPorPagar();
            StartPosition = FormStartPosition.CenterScreen;
        }

        void DefaultView_ListChanged(object sender, ListChangedEventArgs e)
        {
            CargarResumenCuentasPorPagar();
        }

        public void CargarResumenCuentasPorPagar()
        {
            if (DGVFilterManagerCuentasPorPagar != null)
            {

                DataView DVCuentasPorPagar = ((DataTable)(DGVFilterManagerCuentasPorPagar.DataGridView.DataSource as BindingSource).DataSource).DefaultView;
                
                string MontoSumatoria = DVCuentasPorPagar.Table.Compute("Sum(MontoCuenta)", DVCuentasPorPagar.RowFilter).ToString();
                txtMontoCuenta.Text = String.IsNullOrEmpty(MontoSumatoria) ? "0.00" : MontoSumatoria;
                MontoSumatoria = DVCuentasPorPagar.Table.Compute("Sum(MontoAmortiguado)", DVCuentasPorPagar.RowFilter).ToString();
                txtMontoAmortiguado.Text = String.IsNullOrEmpty(MontoSumatoria) ? "0.00" : MontoSumatoria;
                MontoSumatoria = DVCuentasPorPagar.Table.Compute("Sum(MontoPendiente)", DVCuentasPorPagar.RowFilter).ToString();
                txtMontoPendiente.Text = String.IsNullOrEmpty(MontoSumatoria) ? "0.00" : MontoSumatoria;                
            }
        }

        public void habilitarBotones(bool nuevo, bool editar, bool eliminar, bool verPagos, bool pagar, bool reportes)
        {
            this.tsBtnNuevo.Enabled = nuevo;
            this.tsBtnModificar.Enabled = editar;
            this.tsBtnAnular.Enabled = eliminar;
            this.tsBtnVerPagos.Enabled = verPagos;
            this.tsBtnPagar.Enabled = pagar;
            this.tsBtnReportes.Enabled = reportes;
        }
        private void tsBtnNuevo_Click(object sender, EventArgs e)
        {
            FCuentasPorPagarIA formCuentasPorPagar = new FCuentasPorPagarIA(NumeroAlmacen, DIUsuario);
            formCuentasPorPagar.configurarFormularioIA(null);
            if (formCuentasPorPagar.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                checkListartodos.Checked = true;
                btnBuscar_Click(btnBuscar, e);
            }
            formCuentasPorPagar.Dispose();
        }

        private void tsBtnModificar_Click(object sender, EventArgs e)
        {
            if (bdSourceCuentas.Position >= 0 && dtGVCuentas.CurrentCell != null)
            {

                int NumeroTransaccion = -1;
                if (int.TryParse(dtGVCuentas.CurrentRow.Cells["DGCNumeroTransaccion"].Value.ToString(), out NumeroTransaccion) && NumeroTransaccion > 0)
                {
                    MessageBox.Show("No se puede modificar una Cuenta por Pagar correspondiente a una trasaccion");
                    return;
                }

                FCuentasPorPagarIA formCuentasPorPagar = new FCuentasPorPagarIA(NumeroAlmacen, DIUsuario);
                formCuentasPorPagar.configurarFormularioIA(int.Parse(dtGVCuentas.CurrentRow.Cells["DGCNumeroCuenta"].Value.ToString()));
                if (formCuentasPorPagar.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    checkListartodos.Checked = true;
                    btnBuscar_Click(btnBuscar, e);
                }

                formCuentasPorPagar.Dispose();
            }
            else
            {
                MessageBox.Show(this, "Aún no ha seleccionado ninguna Cuenta", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void tsBtnAnular_Click(object sender, EventArgs e)
        {
            if (bdSourceCuentas.Position >= 0 && dtGVCuentas.CurrentCell != null
                && MessageBox.Show(this, "¿Se encuentra seguro de Eliminar el Registro Actual?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == System.Windows.Forms.DialogResult.Yes)
            {

                int NumeroTransaccion = -1;
                if (int.TryParse(dtGVCuentas.CurrentRow.Cells["DGCNumeroTransaccion"].Value.ToString(), out NumeroTransaccion) || NumeroTransaccion > 0)
                {
                    MessageBox.Show("No se puede modificar una Cuenta por Pagar correspondiente a una trasaccion");
                    return;
                }

                TACuentasPorPagar.Delete(int.Parse(dtGVCuentas.CurrentRow.Cells["DGCNumeroCuenta"].Value.ToString()));

                DTListarCuentasPorPagar.Rows.RemoveAt(
                    DTListarCuentasPorPagar.Rows.IndexOf(
                    DTListarCuentasPorPagar.Select("NumeroCuenta = " + dtGVCuentas.CurrentRow.Cells["DGCNumeroCuenta"].Value.ToString())[0]));
            }
            else
            {
                MessageBox.Show(this, "Aún no ha seleccionado ninguna Cuenta", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void tsBtnVerPagos_Click(object sender, EventArgs e)
        {
            if (dtGVCuentas.CurrentRow != null)
            {
                FCuentasPorPagarIA formCuentasPorPagar = new FCuentasPorPagarIA(NumeroAlmacen, DIUsuario);
                formCuentasPorPagar.cargarDatosCuentaPorPagar(int.Parse(dtGVCuentas.CurrentRow.Cells["DGCNumeroCuenta"].Value.ToString()));
                formCuentasPorPagar.ConfigurarVerPagos();
                formCuentasPorPagar.ShowDialog();
                formCuentasPorPagar.Dispose();
                checkListartodos.Checked = true;
                btnBuscar_Click(btnBuscar, e);
            }
            else
            {
                MessageBox.Show(this, "Aún no ha seleccionado ninguna Cuenta", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void tsBtnPagar_Click(object sender, EventArgs e)
        {
            if (dtGVCuentas.CurrentRow != null)
            {
                FCuentasPorPagarIA formCuentasPorPagar = new FCuentasPorPagarIA(NumeroAlmacen, DIUsuario);
                formCuentasPorPagar.cargarDatosCuentaPorPagar(int.Parse(dtGVCuentas.CurrentRow.Cells["DGCNumeroCuenta"].Value.ToString()));
                formCuentasPorPagar.configurarParaPagos();
                formCuentasPorPagar.ShowDialog();
                formCuentasPorPagar.Dispose();
                checkListartodos.Checked = true;
                btnBuscar_Click(btnBuscar, e);
            }
            else
            {
                MessageBox.Show(this, "Aún no ha seleccionado ninguna Cuenta", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            DGVFilterManagerCuentasPorPagar.ActivateAllFilters(false);
            if (checkListartodos.Checked)
            {
                DTListarCuentasPorPagar = TAListarCuentasPorPagar.GetData(NumeroAlmacen, null, null, null, null, "P");
            }
            else
            {
                DTListarCuentasPorPagar = TAListarCuentasPorPagar.GetData(NumeroAlmacen, String.IsNullOrEmpty(txtNumeroCuenta.Text) ? (int?)null : int.Parse(txtNumeroCuenta.Text),
                    cBoxEstadoCuenta.SelectedIndex >= 0 ? cBoxEstadoCuenta.SelectedValue.ToString() : null, 
                    dateFechaInicio.Checked ? dateFechaInicio.Value : (DateTime?) null,
                    dateFechaFin.Checked ? dateFechaFin.Value : (DateTime?)null, "P");
            }
            bdSourceCuentas.DataSource = DTListarCuentasPorPagar;
            dtGVCuentas.DataSource = bdSourceCuentas;
            DGVFilterManagerCuentasPorPagar.DataGridView = dtGVCuentas;
            DTListarCuentasPorPagar.DefaultView.ListChanged += new ListChangedEventHandler(DefaultView_ListChanged);
            CargarResumenCuentasPorPagar();
        }

        private void checkListartodos_CheckedChanged(object sender, EventArgs e)
        {
            if (checkListartodos.Checked)
            {
                txtNumeroCuenta.Text = String.Empty;
                cBoxEstadoCuenta.SelectedIndex = -1;
            }
            
            txtNumeroCuenta.Enabled = !checkListartodos.Checked;
            dateFechaFin.Enabled = dateFechaInicio.Enabled = cBoxEstadoCuenta.Enabled = !checkListartodos.Checked;
        }

        private void cBoxEstadoCuenta_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                cBoxEstadoCuenta.SelectedIndex = -1;
        }

        private void dtGVCuentas_SelectionChanged(object sender, EventArgs e)
        {
            if (dtGVCuentas.CurrentCell != null &&
                dtGVCuentas.CurrentRow != null)
            {
                tsLblEstado.Text = dtGVCuentas.CurrentRow.Cells["DGCEstado"].Value.ToString();
                tsLblFechaRegistro.Text = dtGVCuentas.CurrentRow.Cells["DGCFechaHoraRegistro"].Value.ToString();
                tsLblNroCuenta.Text = dtGVCuentas.CurrentRow.Cells["DGCNumeroCuenta"].Value.ToString();
            }
            else
            {
                tsLblEstado.Text = String.Empty;
                tsLblFechaRegistro.Text = String.Empty;
                tsLblNroCuenta.Text = String.Empty;
            }
        }

        private void tsBtnReportes_Click(object sender, EventArgs e)
        {

            FReporteCuentas formCuentas = new FReporteCuentas();
            formCuentas.ListarCuentasPorCobrarPagar(DTListarCuentasPorPagar,
                dateFechaInicio.Value, dateFechaFin.Value, "CUENTAS POR PAGAR");
            formCuentas.ShowDialog();
            formCuentas.Dispose();

            //FReporteCuentas formCuentas = new FReporteCuentas();
            //formCuentas.ListarCuentasPorCobrarPagarDetallePagos(TAListarCuentasPorPagarDetalle.GetData(NumeroAlmacen, null, null, null, null, "P"),
            //    dateFechaInicio.Value, dateFechaFin.Value, "DETALLE DE PAGOS POR CUENTAS POR PAGAR");
            //formCuentas.ShowDialog();
            //formCuentas.Dispose();
        }

        
    }
}
