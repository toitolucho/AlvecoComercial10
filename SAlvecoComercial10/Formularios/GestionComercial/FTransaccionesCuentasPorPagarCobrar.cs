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
using SAlvecoComercial10.Formularios.Utilidades;

namespace SAlvecoComercial10.Formularios.GestionComercial
{
    public partial class FTransaccionesCuentasPorPagarCobrar : Form
    {
        ListarCompraProductoCuentasPorPagarReporteTableAdapter TAListarCompraProductoCuentasPorPagarReporte;
        ListarVentaProductoCuentasPorCobrarReporteTableAdapter TAListarVentaProductoCuentasPorCobrarReporte;
        ProveedoresTableAdapter TAProveedores;
        ClientesTableAdapter TAClientes;
        AlvecoComercial10DataSet.ListarCompraProductoCuentasPorPagarReporteDataTable DTListarCompraProductoCuentasPorPagarReporte;
        AlvecoComercial10DataSet.ListarVentaProductoCuentasPorCobrarReporteDataTable DTListarVentaProductoCuentasPorCobrarReporte;
        string DIUsuario; int NumeroAlmacen;
        DgvFilterPopup.DgvFilterManager DGVFilterManagerCuentasPorCobrar;
        DgvFilterPopup.DgvFilterManager DGVFilterManagerCuentasPorPagar;
        DataTable DTClientes, DTProveedoresCP;
        string TipoTransaccion;
        public FTransaccionesCuentasPorPagarCobrar(int NumeroAlmacen, string DIUsuario, string TipoTransaccion)
        {
            InitializeComponent();
            this.NumeroAlmacen = NumeroAlmacen;
            this.DIUsuario = DIUsuario;
            this.TipoTransaccion = TipoTransaccion;

            if (TipoTransaccion == "V")
            {
                tabControl1.TabPages.Remove(tabPage1);
                this.Text = "Transacciones Pendientes por Ventas";
            }
            else
                tabControl1.TabPages.Remove(tabPage2);

            TAListarCompraProductoCuentasPorPagarReporte = new ListarCompraProductoCuentasPorPagarReporteTableAdapter();
            TAListarCompraProductoCuentasPorPagarReporte.Connection = DAOUtilidades.conexion;
            TAListarVentaProductoCuentasPorCobrarReporte = new ListarVentaProductoCuentasPorCobrarReporteTableAdapter();
            TAListarVentaProductoCuentasPorCobrarReporte.Connection = DAOUtilidades.conexion;
            TAClientes = new ClientesTableAdapter();
            TAClientes.Connection = DAOUtilidades.conexion;
            TAProveedores = new ProveedoresTableAdapter();
            TAProveedores.Connection = DAOUtilidades.conexion;

            DTListarVentaProductoCuentasPorCobrarReporte = new AlvecoComercial10DataSet.ListarVentaProductoCuentasPorCobrarReporteDataTable();
            DTListarCompraProductoCuentasPorPagarReporte = new AlvecoComercial10DataSet.ListarCompraProductoCuentasPorPagarReporteDataTable();

            dtGVCuentasPorCobrar.AutoGenerateColumns = false;
            dtGVCuentasPorPagar.AutoGenerateColumns = false;

            DTListarVentaProductoCuentasPorCobrarReporte = TAListarVentaProductoCuentasPorCobrarReporte.GetData(NumeroAlmacen, null, null, null, null);
            DTListarCompraProductoCuentasPorPagarReporte = TAListarCompraProductoCuentasPorPagarReporte.GetData(NumeroAlmacen, null, null, null, null);
            DTClientes = TAClientes.GetDataByActivos();
            DTProveedoresCP = TAProveedores.GetDataByActivos();

            


            dtGVCuentasPorCobrar.DataSource = DTListarVentaProductoCuentasPorCobrarReporte;
            dtGVCuentasPorPagar.DataSource = DTListarCompraProductoCuentasPorPagarReporte;

            DGCNombreRazonSocial.ValueMember = "CodigoProveedor";
            DGCNombreRazonSocial.DisplayMember = "NombreRazonSocial";
            DGCNombreRazonSocial.DataPropertyName = "CodigoProveedor";
            DGCNombreRazonSocial.DataSource = DTProveedoresCP;

            DGCNombreRazonSocial2.ValueMember = "CodigoCliente";
            DGCNombreRazonSocial2.DisplayMember = "NombreCliente";
            DGCNombreRazonSocial2.DataPropertyName = "CodigoCliente";
            DGCNombreRazonSocial2.DataSource = DTClientes;


            

            //new DgvFilterPopup.DgvFilterManager(dtGVProductosRequeridos);
            checkListartodos.Checked = true;
            DGVFilterManagerCuentasPorCobrar = new DgvFilterPopup.DgvFilterManager();
            DGVFilterManagerCuentasPorPagar = new DgvFilterPopup.DgvFilterManager();

            DGVFilterManagerCuentasPorCobrar.DataGridView = this.dtGVCuentasPorCobrar;
            DGVFilterManagerCuentasPorPagar.DataGridView = this.dtGVCuentasPorPagar;

            DTListarVentaProductoCuentasPorCobrarReporte.DefaultView.ListChanged += new ListChangedEventHandler(DefaultView_ListChanged);
            DTListarCompraProductoCuentasPorPagarReporte.DefaultView.ListChanged +=new ListChangedEventHandler(DefaultView_ListChanged);

            CargarResumenCuentasPorCobrar();
            CargarResumenCuentasPorPagar();

            StartPosition = FormStartPosition.CenterScreen;
        }

        void DefaultView_ListChanged(object sender, ListChangedEventArgs e)
        {
            if(sender.Equals(DTListarVentaProductoCuentasPorCobrarReporte.DefaultView))
                CargarResumenCuentasPorCobrar();
            else
                CargarResumenCuentasPorPagar();
        }

        public void CargarResumenCuentasPorPagar()
        {
            if (DGVFilterManagerCuentasPorPagar != null)
            {
                
                DataView DVCuentasPorPagar = ((DataTable)DGVFilterManagerCuentasPorPagar.DataGridView.DataSource).DefaultView;

                string MontoSumatoria = DVCuentasPorPagar.Table.Compute("Sum(MontoTotalCompra)", DVCuentasPorPagar.RowFilter).ToString();
                txtBoxTCompraCP.Text = String.IsNullOrEmpty(MontoSumatoria) ? "0.00" : MontoSumatoria;
                MontoSumatoria = DVCuentasPorPagar.Table.Compute("Sum(MontoCuentaPorPagar)", DVCuentasPorPagar.RowFilter).ToString();
                txtBoxTCuentaCP.Text = String.IsNullOrEmpty(MontoSumatoria) ? "0.00" : MontoSumatoria;
                MontoSumatoria = DVCuentasPorPagar.Table.Compute("Sum(MontoPagado)", DVCuentasPorPagar.RowFilter).ToString();
                txtBoxTPagadoCP.Text = String.IsNullOrEmpty(MontoSumatoria) ? "0.00" : MontoSumatoria;
                MontoSumatoria = DVCuentasPorPagar.Table.Compute("Sum(MontoDiferencia)", DVCuentasPorPagar.RowFilter).ToString();
                txtBoxTDiferenciaCP.Text = String.IsNullOrEmpty(MontoSumatoria) ? "0.00" : MontoSumatoria;
            }
        }

        public void CargarResumenCuentasPorCobrar()
        {
            if (DGVFilterManagerCuentasPorCobrar != null)
            {
                DataView DVCuentasPorCobrar = ((DataTable)DGVFilterManagerCuentasPorCobrar.DataGridView.DataSource).DefaultView;

                string MontoSumatoria = DVCuentasPorCobrar.Table.Compute("Sum(MontoTotalVenta)", DVCuentasPorCobrar.RowFilter).ToString();
                txtBoxTCompraCC.Text = String.IsNullOrEmpty(MontoSumatoria) ? "0.00" : MontoSumatoria;
                MontoSumatoria = DVCuentasPorCobrar.Table.Compute("Sum(MontoCuentaPorCobrar)", DVCuentasPorCobrar.RowFilter).ToString();
                txtBoxTCuentaCC.Text = String.IsNullOrEmpty(MontoSumatoria) ? "0.00" : MontoSumatoria;
                MontoSumatoria = DVCuentasPorCobrar.Table.Compute("Sum(MontoPagado)", DVCuentasPorCobrar.RowFilter).ToString();
                txtBoxTCobradoCC.Text = String.IsNullOrEmpty(MontoSumatoria) ? "0.00" : MontoSumatoria;
                MontoSumatoria = DVCuentasPorCobrar.Table.Compute("Sum(MontoDiferencia)", DVCuentasPorCobrar.RowFilter).ToString();
                txtBoxTDiferenciaCC.Text = String.IsNullOrEmpty(MontoSumatoria) ? "0.00" : MontoSumatoria;
            }
        }

        private void rBtnDetalleCompra_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnDetalle_Click(object sender, EventArgs e)
        {
            if ((dtGVCuentasPorPagar.CurrentCell == null && tabControl1.SelectedTab == tabPage1)
                || (dtGVCuentasPorCobrar.CurrentCell == null && tabControl1.SelectedTab == tabPage2))
            {
                MessageBox.Show("Aún no ha seleccionado ninguna Fila");
                return;
            }
            if (rBtnDetalleCompra.Checked)
            {
                if (tabControl1.SelectedTab == tabPage1)
                {
                    FComprasProductos formComprasProductos = new FComprasProductos(DIUsuario, NumeroAlmacen);
                    formComprasProductos.emitirPermisos(false, false, false, false, false);
                    formComprasProductos.cargarDatosCompraProducto(tabControl1.SelectedIndex == 0 ?
                        int.Parse(dtGVCuentasPorPagar.CurrentRow.Cells[DGCNumeroCompraProducto.Name].Value.ToString())
                        : int.Parse(dtGVCuentasPorCobrar.CurrentRow.Cells[DGCNumeroCompraProducto2.Name].Value.ToString()));
                    formComprasProductos.ShowDialog(this);
                    formComprasProductos.Dispose();
                }
                else
                {
                    FVentasProductos formComprasProductos = new FVentasProductos(DIUsuario, NumeroAlmacen);
                    formComprasProductos.emitirPermisos(false, false, false, false, false);
                    formComprasProductos.cargarDatosVentaProducto(tabControl1.SelectedTab == tabPage1 ?
                        int.Parse(dtGVCuentasPorPagar.CurrentRow.Cells[DGCNumeroCompraProducto.Name].Value.ToString())
                        : int.Parse(dtGVCuentasPorCobrar.CurrentRow.Cells[DGCNumeroCompraProducto2.Name].Value.ToString()));
                    formComprasProductos.ShowDialog(this);
                    formComprasProductos.Dispose();
                }
            }
            else
            {
                if (tabControl1.SelectedTab == tabPage1)
                {
                    FCuentasPorPagarIA formCuentasPorPagar = new FCuentasPorPagarIA(NumeroAlmacen, DIUsuario);
                    formCuentasPorPagar.cargarDatosCuentaPorPagar(int.Parse(dtGVCuentasPorPagar.CurrentRow.Cells[DGCNumeroCuentaPorPagar.Name].Value.ToString()));
                    formCuentasPorPagar.DeshabilitarBotones();
                    formCuentasPorPagar.ShowDialog(this);
                    formCuentasPorPagar.Dispose();
                }
                else if (tabControl1.SelectedTab == tabPage2)
                {
                    FCuentasPorCobrarIA formCuentasPorCobrar = new FCuentasPorCobrarIA(NumeroAlmacen, DIUsuario);
                    formCuentasPorCobrar.cargarDatosCuentaPorCobrar(int.Parse(dtGVCuentasPorCobrar.CurrentRow.Cells[DGCNumeroCuentaPorCobrar.Name].Value.ToString() ));
                    formCuentasPorCobrar.DeshabilitarBotones();
                    formCuentasPorCobrar.ShowDialog(this);
                    formCuentasPorCobrar.Dispose();
                }

                btnFiltrar_Click(sender, e);
            }
        }

        private void dtGVCuentasPorPagar_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
                btnDetalle_Click(btnDetalle, e as EventArgs);
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void FComprasProductosCuentasPorPagar_Load(object sender, EventArgs e)
        {
            dPickeFechaHoraInicio.Value = DateTime.Parse("01/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString());
        }


        private void dtGVCuentasPorPagar_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage1)
            {
                DGVFilterManagerCuentasPorPagar.ActivateAllFilters(false);
                if (checkListartodos.Checked)
                {
                    DTListarCompraProductoCuentasPorPagarReporte = TAListarCompraProductoCuentasPorPagarReporte.GetData(NumeroAlmacen, null, null, null, null);
                }
                else
                {
                    DTListarCompraProductoCuentasPorPagarReporte = TAListarCompraProductoCuentasPorPagarReporte.GetData(NumeroAlmacen,
                        String.IsNullOrEmpty(textBox1.Text.Trim()) ? (int?)null : int.Parse(textBox1.Text),
                        !String.IsNullOrEmpty(textBox1.Text.Trim()) ? (DateTime?)null : dPickeFechaHoraInicio.Value,
                        !String.IsNullOrEmpty(textBox1.Text.Trim()) ? (DateTime?)null : dPickerFechaHoraFin.Value,
                        null);
                }
                dtGVCuentasPorPagar.DataSource = DTListarCompraProductoCuentasPorPagarReporte;
                DGVFilterManagerCuentasPorPagar.DataGridView = dtGVCuentasPorPagar;                
                DTListarCompraProductoCuentasPorPagarReporte.DefaultView.ListChanged += new ListChangedEventHandler(DefaultView_ListChanged);
                CargarResumenCuentasPorPagar();
            }
            else
            {
                {
                    DGVFilterManagerCuentasPorCobrar.ActivateAllFilters(false);
                    if (checkListartodos.Checked)
                    {
                        DTListarVentaProductoCuentasPorCobrarReporte = TAListarVentaProductoCuentasPorCobrarReporte.GetData(NumeroAlmacen, null, null, null, null);
                    }
                    else
                    {
                        DTListarVentaProductoCuentasPorCobrarReporte = TAListarVentaProductoCuentasPorCobrarReporte.GetData(NumeroAlmacen,
                            String.IsNullOrEmpty(textBox1.Text.Trim()) ? (int?)null : int.Parse(textBox1.Text),
                            !String.IsNullOrEmpty(textBox1.Text.Trim()) ? (DateTime?)null : dPickeFechaHoraInicio.Value,
                            !String.IsNullOrEmpty(textBox1.Text.Trim()) ? (DateTime?)null : dPickerFechaHoraFin.Value,
                            null);
                    }
                    dtGVCuentasPorCobrar.DataSource = DTListarVentaProductoCuentasPorCobrarReporte;
                    DGVFilterManagerCuentasPorCobrar.DataGridView = dtGVCuentasPorCobrar;
                    DTListarVentaProductoCuentasPorCobrarReporte.DefaultView.ListChanged += new ListChangedEventHandler(DefaultView_ListChanged);
                    CargarResumenCuentasPorCobrar();
                }
            }
            
        }
    }
}
