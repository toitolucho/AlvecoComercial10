using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSetTableAdapters;
using SAlvecoComercial10.AccesoDatos;
using SAlvecoComercial10.Formularios.Reportes;

namespace SAlvecoComercial10.Formularios.GestionComercial
{
    public partial class FVentasProductosAdministrador : Form
    {
        AlvecoComercial10DataSet.BuscarTransaccionGCDataTable DTBusquedaCompraProducto;
        DataTable VariablesConfiguracionSistemaGC;
        BuscarTransaccionGCTableAdapter TABuscarTransaccionGC;
        ComprasProductosDetalleTableAdapter TAComprasProductosDetalle;


        ArrayList ListaCodigosEstadoCompra = new ArrayList();

        public int NumeroAlmacen { get; set; }
        private int NumeroPC = 0;
        public string DIUsuario { get; set; }
        public int NumeroTransaccion { get; set; }
        public string TipoOperacion = "";



        public FVentasProductosAdministrador(int NumeroAlmacen, string DIUsuario)
        {
            InitializeComponent();
            this.NumeroAlmacen = NumeroAlmacen;
            this.DIUsuario = DIUsuario;

            DTBusquedaCompraProducto = new AlvecoComercial10DataSet.BuscarTransaccionGCDataTable();

            this.dateTimePicker1.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            txtBoxNumeroTransaccion.Clear();
            txtBoxTextoBusqueda.Clear();
            this.lblNroTransaccion.Text = "Numero de Venta";
            this.cBoxBuscarPor.Items.Clear();
            this.cBoxBuscarPor.Items.Add("Nombre Cliente");
            this.cBoxBuscarPor.Items.Add("NIT Cliente");
            this.cBoxBuscarPor.Items.Add("Nombre Producto");
            this.cBoxBuscarPor.Items.Add("Observaciones");
            this.cBoxBuscarPor.SelectedIndex = 0;

            TABuscarTransaccionGC = new BuscarTransaccionGCTableAdapter();
            TABuscarTransaccionGC.Connection = Utilidades.DAOUtilidades.conexion;
            TAComprasProductosDetalle = new ComprasProductosDetalleTableAdapter();
            TAComprasProductosDetalle.Connection = Utilidades.DAOUtilidades.conexion;



            DGCNumeroAgencia.Width = 75;
            DGCNumeroCompraProducto.Width = 100;
            DGCFecha.Width = 165;
            DGCObservaciones.Width = 250;
            dtGVTransacciones.CellDoubleClick += new DataGridViewCellEventHandler(dtGVTransacciones_CellDoubleClick);
            dtGVTransacciones.CellFormatting += new DataGridViewCellFormattingEventHandler(dtGVTransacciones_CellFormatting);
            this.StartPosition = FormStartPosition.CenterScreen;
            detalleDePagosToolStripMenuItem.Visible = false;
        }

        void dtGVTransacciones_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((e.Value != null))
            {
                if ((e.ColumnIndex == DGCTipoCompra.Index))
                {
                    if (dtGVTransacciones["DGCTipoCompra", e.RowIndex].Value.ToString().CompareTo("EFECTIVO") == 0)
                        dtGVTransacciones.Rows[e.RowIndex].Cells["DGCTipoCompra"].Style.BackColor = Color.LightCoral;
                    else
                        dtGVTransacciones.Rows[e.RowIndex].Cells["DGCTipoCompra"].Style.BackColor = Color.Cyan;
                }

                if ((e.ColumnIndex == DGCEstadoCompra.Index))
                {
                    switch (dtGVTransacciones["DGCEstadoCompra", e.RowIndex].Value.ToString())
                    {
                        case "INICIADA":
                            dtGVTransacciones.Rows[e.RowIndex].Cells["DGCEstadoCompra"].Style.BackColor = Color.Tomato;
                            break;
                        case "ANULADA":
                            dtGVTransacciones.Rows[e.RowIndex].Cells["DGCEstadoCompra"].Style.BackColor = Color.Firebrick;
                            break;
                        case "PAGADA EN TRANSITO":
                            dtGVTransacciones.Rows[e.RowIndex].Cells["DGCEstadoCompra"].Style.BackColor = Color.SteelBlue;
                            break;
                        case "A CREDITO EN TRANSITO":
                            dtGVTransacciones.Rows[e.RowIndex].Cells["DGCEstadoCompra"].Style.BackColor = Color.Gold;
                            break;
                        case "PENDIENTE":
                            dtGVTransacciones.Rows[e.RowIndex].Cells["DGCEstadoCompra"].Style.BackColor = Color.LawnGreen;
                            break;
                        case "FINALIZADA Y DISTRIBUIDA":
                            dtGVTransacciones.Rows[e.RowIndex].Cells["DGCEstadoCompra"].Style.BackColor = Color.WhiteSmoke;
                            break;
                        case "FINALIZADO INCOMPLETO":
                            dtGVTransacciones.Rows[e.RowIndex].Cells["DGCEstadoCompra"].Style.BackColor = Color.Moccasin;
                            break;
                    }

                }


            }
        }

        void dtGVTransacciones_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                if (dtGVTransacciones.RowCount > 0 && dtGVTransacciones.CurrentRow != null)
                {

                    this.NumeroTransaccion = int.Parse(DTBusquedaCompraProducto.DefaultView[e.RowIndex][DTBusquedaCompraProducto.NumeroTransaccionColumn.ColumnName].ToString());
                    string CodigoEstadoCompra = DTBusquedaCompraProducto.DefaultView[e.RowIndex][DTBusquedaCompraProducto.CodigoEstadoTransaccionColumn.ColumnName].ToString();

                    switch (TipoOperacion)
                    {
                        case "A": //Recepción en Almacenes
                            FComprasProductosRecepcion _FCompraProductosAdministradorIngresoInventarios = new FComprasProductosRecepcion(NumeroAlmacen, DIUsuario, NumeroTransaccion);
                            if (_FCompraProductosAdministradorIngresoInventarios.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                DTBusquedaCompraProducto[e.RowIndex].CodigoEstadoTransaccion = Utilidades.DAOUtilidades.ObtenerCodigoEstadoActualTransacciones(NumeroAlmacen, NumeroTransaccion, "C");
                            }
                            _FCompraProductosAdministradorIngresoInventarios.Dispose();
                            break;
                        case "C": //Cancelacion y pago para el Contador compras a CREDITO
                            if (DTBusquedaCompraProducto[e.RowIndex].CodigoTipoTransaccion == "R")
                            {
                                FCuentasPorCobrarIA formCuentasPorPagar = new FCuentasPorCobrarIA(NumeroAlmacen, DIUsuario);
                                formCuentasPorPagar.cargarDatosCuentaPorCobrar(DTBusquedaCompraProducto[e.RowIndex].NumeroCuentaCreditoTransaccion);
                                formCuentasPorPagar.ShowDialog();
                                formCuentasPorPagar.Dispose();
                            }
                            else
                            {
                                MessageBox.Show("La Transacción actual ha sido pagada en Efectivo");
                            }
                            break;
                        case "N": //Navegación
                            FVentasProductos _FComprasProductos = new FVentasProductos(DIUsuario, NumeroAlmacen);
                            _FComprasProductos.emitirPermisos(false, true, true, true, true);
                            _FComprasProductos.cargarDatosVentaProducto(NumeroTransaccion);
                            _FComprasProductos.ShowDialog(this);
                            _FComprasProductos.Dispose();
                            break;
                        case "B":
                            if (CodigoEstadoCompra.CompareTo("F") == 0)
                            {
                                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                                this.Close();
                                return;
                            }
                            else
                            {
                                MessageBox.Show(this, "Los productos de la Venta Seleccionada no puede ser devueltos, debido a que la misma no ha sido concretada aún");
                            }
                            break;
                        default:
                            break;
                    }

                    CodigoEstadoCompra = Utilidades.DAOUtilidades.ObtenerCodigoEstadoActualTransacciones(NumeroAlmacen, NumeroTransaccion, "V");
                    if (!String.IsNullOrEmpty(CodigoEstadoCompra))
                    {
                        DTBusquedaCompraProducto[e.RowIndex].CodigoEstadoTransaccion = CodigoEstadoCompra;
                        DTBusquedaCompraProducto[e.RowIndex].EstadoTransaccion = Utilidades.DAOUtilidades.obtenerSignificadoEstadoCompra(CodigoEstadoCompra);
                    }
                    else
                    {
                        DTBusquedaCompraProducto.Rows.Remove(DTBusquedaCompraProducto.FindByNumeroAlmacenNumeroTransaccion(NumeroAlmacen, NumeroTransaccion));
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("No ha Seleccionado aún una Compra " + ex.Message);
            }

        }

        private void FVentaProductosBuscador_Load(object sender, EventArgs e)
        {

            DTBusquedaCompraProducto = TABuscarTransaccionGC.GetData("0", " ", NumeroAlmacen,
                null, "S", dateTimePicker1.Value, DateTime.Now, false, null);

            bdSourceTransacciones.DataSource = DTBusquedaCompraProducto;
            dtGVTransacciones.DataSource = bdSourceTransacciones;
            statusStrip1.Items[0].Text = "Numero de registros encontrados: " + bdSourceTransacciones.Count.ToString();
            DGCNumeroAgencia.Width = 85;
            DGCNombreRazonSocial.Width = 200;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtBoxNumeroTransaccion.Text.Trim()) && String.IsNullOrEmpty(txtBoxTextoBusqueda.Text))
            {
                MessageBox.Show(this, "Aun no ha ingresado un Texto a Buscar", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                DTBusquedaCompraProducto.Clear();
                int NumeroTransaccion = -1;
                if (txtBoxNumeroTransaccion.Text.Trim().Length > 0 && txtBoxNumeroTransaccion.Text != null)
                {
                    NumeroTransaccion = Int32.Parse(txtBoxNumeroTransaccion.Text);
                }

                DTBusquedaCompraProducto = TABuscarTransaccionGC.GetData(cBoxBuscarPor.SelectedIndex.ToString(),
                    txtBoxTextoBusqueda.Text.Trim(), NumeroAlmacen, NumeroTransaccion == -1 ? null : (int?)NumeroTransaccion, "S",
                        dateTimePicker1.Value, dateTimePicker2.Value, checkTextoIdentico.Checked,
                        cBoxCodigoEstadoVenta.SelectedValue.Equals("T") ? null : cBoxCodigoEstadoVenta.SelectedValue.ToString());
                bdSourceTransacciones.DataSource = DTBusquedaCompraProducto;
                dtGVTransacciones.DataSource = bdSourceTransacciones;

                if (DTBusquedaCompraProducto.Rows.Count == 0)
                {
                    MessageBox.Show(this,"No se encontró ningun registro con la información provista", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                    
                statusStrip1.Items[0].Text = "Numero de registros encontrados: " + bdSourceTransacciones.Count.ToString();
                if (txtBoxNumeroTransaccion.Focused)
                    txtBoxNumeroTransaccion.SelectAll();
                else if (txtBoxTextoBusqueda.Focused)
                    txtBoxTextoBusqueda.SelectAll();
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtBoxNumeroTransaccion.Clear();
            txtBoxTextoBusqueda.Clear();
            this.DTBusquedaCompraProducto.Clear();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtBoxTextoBusqueda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e as EventArgs);
            }
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                if (dtGVTransacciones.RowCount > 0)
                {
                    dtGVTransacciones.Focus();
                    dtGVTransacciones.Columns[3].Selected = true;
                    dtGVTransacciones.CurrentCell = dtGVTransacciones.Rows[0].Cells[3];
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.txtBoxTextoBusqueda.Clear();
                this.txtBoxNumeroTransaccion.Clear();
                this.txtBoxTextoBusqueda.Focus();
            }
        }




        public void formatearParaBusquedasGeneral()
        {


            Utilidades.ObjetoCodigoDescripcion EstadosCompras = new Utilidades.ObjetoCodigoDescripcion();
            EstadosCompras.cargarDatosEstadosComprasVentasBusqueda();

            this.cBoxCodigoEstadoVenta.DataSource = EstadosCompras.listaObjetos;
            this.cBoxCodigoEstadoVenta.ValueMember = EstadosCompras.ValueMember;
            this.cBoxCodigoEstadoVenta.DisplayMember = EstadosCompras.DisplayMember;
            this.cBoxCodigoEstadoVenta.SelectedValue = "T";

            TipoOperacion = "N";


        }

        public void formatearParaBusquedasDevoluciones()
        {


            Utilidades.ObjetoCodigoDescripcion EstadosCompras = new Utilidades.ObjetoCodigoDescripcion();
            EstadosCompras.cargarDatosEstadosTransaccionesDevoluciones();

            this.cBoxCodigoEstadoVenta.DataSource = EstadosCompras.listaObjetos;
            this.cBoxCodigoEstadoVenta.ValueMember = EstadosCompras.ValueMember;
            this.cBoxCodigoEstadoVenta.DisplayMember = EstadosCompras.DisplayMember;
            this.cBoxCodigoEstadoVenta.SelectedValue = "F";

            TipoOperacion = "B";
            this.Text = "Buscador de Transacciones por Ventas";

        }



        private void txtBoxNumeroTransaccion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsNumber(e.KeyChar) & (Keys)e.KeyChar != Keys.Back & (Keys)e.KeyChar != Keys.Enter)
            {
                e.Handled = true;
                txtBoxNumeroTransaccion.SelectionStart = 0;
                txtBoxNumeroTransaccion.SelectionLength = txtBoxNumeroTransaccion.Text.Length;
                System.Media.SystemSounds.Beep.Play();
                return;
            }
        }



        private void verDetalleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int NumeroCompraProducto = -1;
            if (dtGVTransacciones.RowCount > 0 && dtGVTransacciones.CurrentCell != null
                && int.TryParse(dtGVTransacciones.CurrentRow.Cells[DGCNumeroCompraProducto.Index].Value.ToString(), out NumeroCompraProducto))
            {
                FVentasProductos _FComprasProductos = new FVentasProductos(DIUsuario, NumeroAlmacen);
                _FComprasProductos.emitirPermisos(false, true, true, true, true);
                _FComprasProductos.cargarDatosVentaProducto(NumeroTransaccion);
                _FComprasProductos.ShowDialog(this);
                _FComprasProductos.Dispose();
            }
        }

        private void detalleDePagosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //columna NumeroCuentaPorPagar
        }

        private void importaciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int NumeroCompraProducto = -1;
            if (dtGVTransacciones.RowCount > 0 && dtGVTransacciones.CurrentCell != null
                && int.TryParse(dtGVTransacciones.CurrentRow.Cells[DGCNumeroCompraProducto.Index].Value.ToString(), out NumeroCompraProducto))
            {

                //DataTable DTCompraProductosReporte = TABuscarTransaccionGC.ListarCompraProductoReporte(NumeroAlmacen, NumeroCompraProducto);
                //DataTable DTCompraProductosDetalleReporte = TAComprasProductosDetalle.ListarCompraProductoDetalleReporte(NumeroAlmacen, NumeroCompraProducto);
                //FReporteCompraProductosGeneral ReporteCompraproductosForm = new FReporteCompraProductosGeneral();
                //ReporteCompraproductosForm.ListarReporteComprasProductosImportacion(DTCompraProductosReporte, DTCompraProductosDetalleReporte);
                //ReporteCompraproductosForm.Show();
            }
        }

        private void reportesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int NumeroVentaProducto = -1;
            if (dtGVTransacciones.RowCount > 0 && dtGVTransacciones.CurrentCell != null
                && int.TryParse(dtGVTransacciones.CurrentRow.Cells[DGCNumeroCompraProducto.Index].Value.ToString(), out NumeroVentaProducto))
            {
                ListasVentaProductoReporteTableAdapter TAVentasProductoReporte = new ListasVentaProductoReporteTableAdapter();
                TAVentasProductoReporte.Connection = Utilidades.DAOUtilidades.conexion;
                ListarVentasProductosDetalleParaMostrarTableAdapter TAVentasProductosDetalleMostrar = new ListarVentasProductosDetalleParaMostrarTableAdapter();
                TAVentasProductosDetalleMostrar.Connection = Utilidades.DAOUtilidades.conexion;

                DataTable DTIngresoArticuloR = TAVentasProductoReporte.GetData(NumeroAlmacen, NumeroVentaProducto);
                DataTable DTIngresoArticuloDetalleR = TAVentasProductosDetalleMostrar.GetData(NumeroAlmacen, NumeroVentaProducto);
                FReporteTransaccionesGeneral formReporte = new FReporteTransaccionesGeneral();
                formReporte.cargarReporteVentasProductos(DTIngresoArticuloR, DTIngresoArticuloDetalleR);
                formReporte.ShowDialog(this);
                formReporte.Dispose();
            }
        }

    }


}
