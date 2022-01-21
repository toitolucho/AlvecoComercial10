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
    public partial class FCuentasPorCobrarAdministrador : Form
    {
        private string DIUsuario;
        private int NumeroAlmacen;
        public int NumeroCuentaPorCobrarActual { get; set; }


        ListarCuentasPorCobrarPagarTableAdapter TAListarCuentasPorCobrar;
        AlvecoComercial10DataSet.ListarCuentasPorCobrarPagarDataTable DTListarCuentasPorCobrar;
        CuentasPorCobrarTableAdapter TACuentasPorCobrar;
        string TipoOperacion;

        DgvFilterPopup.DgvFilterManager DGVFilterManagerCuentasPorCobrar;

        public FCuentasPorCobrarAdministrador(int NumeroAlmacen, string DIUsuario)
        {
            InitializeComponent();

            this.NumeroAlmacen = NumeroAlmacen;
            this.DIUsuario = DIUsuario;
            TAListarCuentasPorCobrar = new ListarCuentasPorCobrarPagarTableAdapter();
            TAListarCuentasPorCobrar.Connection = Utilidades.DAOUtilidades.conexion;
            TACuentasPorCobrar = new CuentasPorCobrarTableAdapter();
            TACuentasPorCobrar.Connection = Utilidades.DAOUtilidades.conexion;


            DTListarCuentasPorCobrar = new AlvecoComercial10DataSet.ListarCuentasPorCobrarPagarDataTable();
            DTListarCuentasPorCobrar = TAListarCuentasPorCobrar.GetData(NumeroAlmacen, null, null, null, null, "C");
            dtGVCuentas.AutoGenerateColumns = false;
            bdSourceCuentas.DataSource = DTListarCuentasPorCobrar;
            dtGVCuentas.DataSource = bdSourceCuentas;

            Utilidades.ObjetoCodigoDescripcion EstadosCuentas = new Utilidades.ObjetoCodigoDescripcion();
            EstadosCuentas.cargarDatosEstadosCuentasPorPagarPorCobrar();
            cBoxEstadoCuenta.DataSource = EstadosCuentas.listaObjetos;
            cBoxEstadoCuenta.DisplayMember = EstadosCuentas.DisplayMember;
            cBoxEstadoCuenta.ValueMember = EstadosCuentas.ValueMember;
            cBoxEstadoCuenta.SelectedIndex = -1;

            dateFechaFin.Checked = dateFechaInicio.Checked = false;

            DGVFilterManagerCuentasPorCobrar = new DgvFilterPopup.DgvFilterManager();
            DGVFilterManagerCuentasPorCobrar.DataGridView = this.dtGVCuentas;
            DTListarCuentasPorCobrar.DefaultView.ListChanged += new ListChangedEventHandler(DefaultView_ListChanged);
            CargarResumenCuentasPorCobrar();
            StartPosition = FormStartPosition.CenterScreen;
        }

        void DefaultView_ListChanged(object sender, ListChangedEventArgs e)
        {
            CargarResumenCuentasPorCobrar();
        }

        public void CargarResumenCuentasPorCobrar()
        {
            if (DGVFilterManagerCuentasPorCobrar != null)
            {

                DataView DVCuentasPorCobrar = ((DataTable)(DGVFilterManagerCuentasPorCobrar.DataGridView.DataSource as BindingSource).DataSource).DefaultView;

                string MontoSumatoria = DVCuentasPorCobrar.Table.Compute("Sum(MontoCuenta)", DVCuentasPorCobrar.RowFilter).ToString();
                txtMontoCuenta.Text = String.IsNullOrEmpty(MontoSumatoria) ? "0.00" : MontoSumatoria;
                MontoSumatoria = DVCuentasPorCobrar.Table.Compute("Sum(MontoAmortiguado)", DVCuentasPorCobrar.RowFilter).ToString();
                txtMontoAmortiguado.Text = String.IsNullOrEmpty(MontoSumatoria) ? "0.00" : MontoSumatoria;
                MontoSumatoria = DVCuentasPorCobrar.Table.Compute("Sum(MontoPendiente)", DVCuentasPorCobrar.RowFilter).ToString();
                txtMontoPendiente.Text = String.IsNullOrEmpty(MontoSumatoria) ? "0.00" : MontoSumatoria;
            }
        }

        public void habilitarBotones(bool nuevo, bool editar, bool eliminar, bool verPagos, bool Cobrar, bool reportes)
        {
            this.tsBtnNuevo.Enabled = nuevo;
            this.tsBtnModificar.Enabled = editar;
            this.tsBtnAnular.Enabled = eliminar;
            this.tsBtnVerPagos.Enabled = verPagos;
            this.tsBtnPagar.Enabled = Cobrar;
            this.tsBtnReportes.Enabled = reportes;
        }
        private void tsBtnNuevo_Click(object sender, EventArgs e)
        {
            FCuentasPorCobrarIA formCuentasPorCobrar = new FCuentasPorCobrarIA(NumeroAlmacen, DIUsuario);
            formCuentasPorCobrar.configurarFormularioIA(null);
            if (formCuentasPorCobrar.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                checkListartodos.Checked = true;
                btnBuscar_Click(btnBuscar, e);
            }
            formCuentasPorCobrar.Dispose();
        }

        private void tsBtnModificar_Click(object sender, EventArgs e)
        {
            if (bdSourceCuentas.Position >= 0 && dtGVCuentas.CurrentCell != null)
            {

                int NumeroTransaccion = -1;
                if (int.TryParse(dtGVCuentas.CurrentRow.Cells["DGCNumeroTransaccion"].Value.ToString(), out NumeroTransaccion))
                {
                    MessageBox.Show("No se puede modificar una Cuenta por Cobrar correspondiente a una trasaccion");
                    return;
                }

                FCuentasPorCobrarIA formCuentasPorCobrar = new FCuentasPorCobrarIA(NumeroAlmacen, DIUsuario);
                formCuentasPorCobrar.configurarFormularioIA(int.Parse(dtGVCuentas.CurrentRow.Cells["DGCNumeroCuenta"].Value.ToString()));
                if (formCuentasPorCobrar.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    checkListartodos.Checked = true;
                    btnBuscar_Click(btnBuscar, e);
                }
                formCuentasPorCobrar.Dispose();
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
                if (int.TryParse(dtGVCuentas.CurrentRow.Cells["DGCNumeroTransaccion"].Value.ToString(), out NumeroTransaccion))
                {
                    MessageBox.Show("No se puede modificar una Cuenta por Cobrar correspondiente a una trasaccion");
                    return;
                }

                TACuentasPorCobrar.Delete(int.Parse(dtGVCuentas.CurrentRow.Cells["DGCNumeroCuenta"].Value.ToString()));

                DTListarCuentasPorCobrar.Rows.RemoveAt(
                    DTListarCuentasPorCobrar.Rows.IndexOf(
                    DTListarCuentasPorCobrar.Select("NumeroCuenta = " + dtGVCuentas.CurrentRow.Cells["DGCNumeroCuenta"].Value.ToString())[0]));
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
                FCuentasPorCobrarIA formCuentasPorCobrar = new FCuentasPorCobrarIA(NumeroAlmacen, DIUsuario);
                formCuentasPorCobrar.cargarDatosCuentaPorCobrar(int.Parse(dtGVCuentas.CurrentRow.Cells["DGCNumeroCuenta"].Value.ToString()));
                formCuentasPorCobrar.ConfigurarVerCobros();
                formCuentasPorCobrar.ShowDialog();
                formCuentasPorCobrar.Dispose();
                checkListartodos.Checked = true;
                btnBuscar_Click(btnBuscar, e);
            }
            else
            {
                MessageBox.Show(this, "Aún no ha seleccionado ninguna Cuenta", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void tsBtnCobrar_Click(object sender, EventArgs e)
        {
            if (dtGVCuentas.CurrentRow != null)
            {
                FCuentasPorCobrarIA formCuentasPorCobrar = new FCuentasPorCobrarIA(NumeroAlmacen, DIUsuario);
                formCuentasPorCobrar.cargarDatosCuentaPorCobrar(int.Parse(dtGVCuentas.CurrentRow.Cells["DGCNumeroCuenta"].Value.ToString()));
                formCuentasPorCobrar.configurarParaCobros();
                formCuentasPorCobrar.ShowDialog();
                formCuentasPorCobrar.Dispose();
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
            DGVFilterManagerCuentasPorCobrar.ActivateAllFilters(false);
            if (checkListartodos.Checked)
            {
                DTListarCuentasPorCobrar = TAListarCuentasPorCobrar.GetData(NumeroAlmacen, null, null, null, null, "C");
            }
            else
            {
                DTListarCuentasPorCobrar = TAListarCuentasPorCobrar.GetData(NumeroAlmacen, String.IsNullOrEmpty(txtNumeroCuenta.Text) ? (int?)null : int.Parse(txtNumeroCuenta.Text),
                    cBoxEstadoCuenta.SelectedIndex >= 0 ? cBoxEstadoCuenta.SelectedValue.ToString() : null,
                    dateFechaInicio.Checked ? dateFechaInicio.Value : (DateTime?)null,
                    dateFechaFin.Checked ? dateFechaFin.Value : (DateTime?)null, "C");
            }
            bdSourceCuentas.DataSource = DTListarCuentasPorCobrar;
            dtGVCuentas.DataSource = bdSourceCuentas;
            DGVFilterManagerCuentasPorCobrar.DataGridView = dtGVCuentas;
            DTListarCuentasPorCobrar.DefaultView.ListChanged += new ListChangedEventHandler(DefaultView_ListChanged);
            CargarResumenCuentasPorCobrar();
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
            formCuentas.ListarCuentasPorCobrarPagar(DTListarCuentasPorCobrar, dateFechaInicio.Value, dateFechaFin.Value, "CUENTAS POR COBRAR");
            formCuentas.ShowDialog();
            formCuentas.Dispose();
        }


    }
}
