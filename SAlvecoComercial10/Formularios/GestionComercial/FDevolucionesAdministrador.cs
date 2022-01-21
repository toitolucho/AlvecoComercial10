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
using System.Collections;
using SAlvecoComercial10.Formularios.Reportes;

namespace SAlvecoComercial10.Formularios.GestionComercial
{
    public partial class FDevolucionesAdministrador : Form
    {       

        AlvecoComercial10DataSet.BuscarTransaccionGCDataTable DTBusquedaCompraProducto;
        DataTable VariablesConfiguracionSistemaGC;
        BuscarTransaccionGCTableAdapter TABuscarTransaccionGC;
        ComprasProductosDetalleTableAdapter TAComprasProductosDetalle;
        


        ArrayList ListaCodigosEstadoDevolucion = new ArrayList();

        public int NumeroAlmacen { get; set; }
        private int NumeroPC = 0;
        public string DIUsuario { get; set; }
        public int NumeroTransaccion { get; set; }
        public string TipoOperacion = "";
        public string TipoDevolucion { get; set; }


        public FDevolucionesAdministrador(int NumeroAlmacen, string DIUsuario, string TipoDevolucion)
        {
            InitializeComponent();
            this.NumeroAlmacen = NumeroAlmacen;
            this.DIUsuario = DIUsuario;
            this.TipoDevolucion = TipoDevolucion;

            DTBusquedaCompraProducto = new AlvecoComercial10DataSet.BuscarTransaccionGCDataTable();

            this.dateTimePicker1.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            txtBoxNumeroTransaccion.Clear();
            txtBoxTextoBusqueda.Clear();
            this.lblNroTransaccion.Text = TipoDevolucion == "C" ? "Numero de Dev Compra" : "Numero de Dev Venta";
            this.cBoxBuscarPor.Items.Clear();
            this.cBoxBuscarPor.Items.Add(TipoDevolucion == "C" ? "Nombre Proveedor" : "Nombre Cliente");
            this.cBoxBuscarPor.Items.Add(TipoDevolucion == "C" ? "NIT Proveedor" : "NIT Cliente");
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
        }

        void dtGVTransacciones_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((e.Value != null))
            {               

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
                        case "FINALIZADO Y RECIBIDO":
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
                        case "N": //Navegación
                            if (TipoDevolucion == "V")
                            {
                                FVentasProductosDevolucion formVentasProductosDevolucion = new FVentasProductosDevolucion(DIUsuario, NumeroAlmacen);
                                formVentasProductosDevolucion.emitirPermisos(false, true, true, true);
                                formVentasProductosDevolucion.cargarDatosVentaProducto(NumeroTransaccion);
                                formVentasProductosDevolucion.ShowDialog(this);
                                formVentasProductosDevolucion.Dispose();
                            }
                            else
                            {
                                FComprasProductosDevolucion formComprasProductosDevolucion = new FComprasProductosDevolucion(DIUsuario, NumeroAlmacen);
                                formComprasProductosDevolucion.emitirPermisos(false, true, true, true);
                                formComprasProductosDevolucion.cargarDatosCompraProducto(NumeroTransaccion);
                                formComprasProductosDevolucion.ShowDialog(this);
                                formComprasProductosDevolucion.Dispose();
                            }
                            break;
                        case "B":                            
                            this.DialogResult = System.Windows.Forms.DialogResult.OK;
                            this.Close();                                                        
                            return;
                        default:
                            break;
                    }

                    //CodigoEstadoCompra = Utilidades.DAOUtilidades.ObtenerCodigoEstadoActualTransacciones(NumeroAlmacen, NumeroTransaccion, "C");
                    //DTBusquedaCompraProducto[e.RowIndex].CodigoEstadoTransaccion = CodigoEstadoCompra;
                    //DTBusquedaCompraProducto[e.RowIndex].EstadoTransaccion = Utilidades.DAOUtilidades.obtenerSignificadoEstadoCompra(CodigoEstadoCompra);


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
                null, TipoDevolucion == "C" ? "D" : "V", dateTimePicker1.Value, DateTime.Now, false, null);

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
                    txtBoxTextoBusqueda.Text.Trim(), NumeroAlmacen, NumeroTransaccion == -1 ? null : (int?)NumeroTransaccion, TipoDevolucion == "C" ? "D" : "V",
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
            EstadosCompras.cargarDatosEstadosDevoluciones();

            this.cBoxCodigoEstadoVenta.DataSource = EstadosCompras.listaObjetos;
            this.cBoxCodigoEstadoVenta.ValueMember = EstadosCompras.ValueMember;
            this.cBoxCodigoEstadoVenta.DisplayMember = EstadosCompras.DisplayMember;
            this.cBoxCodigoEstadoVenta.SelectedValue = "T";

            TipoOperacion = "N";


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


        private void verDetalleToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            int NumeroTransaccion = -1;
            if (dtGVTransacciones.RowCount > 0 && dtGVTransacciones.CurrentCell != null
                && int.TryParse(dtGVTransacciones.CurrentRow.Cells[DGCNumeroCompraProducto.Index].Value.ToString(), out NumeroTransaccion))
            {

                if (TipoDevolucion == "V")
                {
                    FVentasProductosDevolucion formVentasProductosDevolucion = new FVentasProductosDevolucion(DIUsuario, NumeroAlmacen);
                    formVentasProductosDevolucion.emitirPermisos(false, false, true, true);
                    formVentasProductosDevolucion.cargarDatosVentaProducto(NumeroTransaccion);
                    formVentasProductosDevolucion.ShowDialog(this);
                    formVentasProductosDevolucion.Dispose();
                }
                else
                {
                    FComprasProductosDevolucion formComprasProductosDevolucion = new FComprasProductosDevolucion(DIUsuario, NumeroAlmacen);
                    formComprasProductosDevolucion.emitirPermisos(false, false, true, true);
                    formComprasProductosDevolucion.cargarDatosCompraProducto(NumeroTransaccion);
                    formComprasProductosDevolucion.ShowDialog(this);
                    formComprasProductosDevolucion.Dispose();
                }
            }
        }

        private void reportesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int NumeroTransaccion = -1;
            if (dtGVTransacciones.RowCount > 0 && dtGVTransacciones.CurrentCell != null
                && int.TryParse(dtGVTransacciones.CurrentRow.Cells[DGCNumeroCompraProducto.Index].Value.ToString(), out NumeroTransaccion))
            {

                if (TipoDevolucion == "V")
                {
                    ListasVentaProductoDevolucionReporteTableAdapter TAListasVentaProductoDevolucionReporte = new ListasVentaProductoDevolucionReporteTableAdapter();
                    TAListasVentaProductoDevolucionReporte.Connection = Utilidades.DAOUtilidades.conexion;

                    ListarVentaProductoDevolucionDetalleParaMostrarTableAdapter TAVentasProductosDevolucionDetalleMostrar = new ListarVentaProductoDevolucionDetalleParaMostrarTableAdapter();
                    TAVentasProductosDevolucionDetalleMostrar.Connection = Utilidades.DAOUtilidades.conexion;

                    DataTable DTVentasProductosDev = TAListasVentaProductoDevolucionReporte.GetData(NumeroAlmacen, NumeroTransaccion);
                    DataTable DTVentasProductosDevDetalle = TAVentasProductosDevolucionDetalleMostrar.GetData(NumeroAlmacen, NumeroTransaccion);
                    FReporteTransaccionesGeneral formReporte = new FReporteTransaccionesGeneral();
                    formReporte.cargarReporteVentasProductosDevolucion(DTVentasProductosDev, DTVentasProductosDevDetalle);
                    formReporte.ShowDialog(this);
                    formReporte.Dispose();
                }
                else
                {
                    ListarCompraProductoDevolucionReporteTableAdapter TAListasCompraProductoDevolucionReporte = new ListarCompraProductoDevolucionReporteTableAdapter();
                    TAListasCompraProductoDevolucionReporte.Connection = Utilidades.DAOUtilidades.conexion;

                    ListarComprasProductosDevolucionDetalleParaMostrarTableAdapter TAComprasProductosDevolucionDetalleMostrar = new ListarComprasProductosDevolucionDetalleParaMostrarTableAdapter();
                    TAComprasProductosDevolucionDetalleMostrar.Connection = Utilidades.DAOUtilidades.conexion;

                    DataTable DTComprasProductosDev = TAListasCompraProductoDevolucionReporte.GetData(NumeroAlmacen, NumeroTransaccion);
                    DataTable DTComprasProductosDevDetalle = TAComprasProductosDevolucionDetalleMostrar.GetData(NumeroAlmacen, NumeroTransaccion);
                    FReporteTransaccionesGeneral formReporte = new FReporteTransaccionesGeneral();
                    formReporte.cargarReporteComprasProductosDevolucion(DTComprasProductosDev, DTComprasProductosDevDetalle);
                    formReporte.ShowDialog(this);
                    formReporte.Dispose();
                }
            }
        }
    }
}
