using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSetTableAdapters;
using SAlvecoComercial10.AccesoDatos;


namespace SAlvecoComercial10.Formularios.Utilidades
{
    public partial class PanelBusquedaProductosDevolucion : UserControl
    {
        public AlvecoComercial10DataSet.DTProductosSeleccionadosDataTable DTProductosSeleccionados{get; set;}
        private int NumeroAlmacen;
        private string CodigoUsuario;
        public AlvecoComercial10DataSet.BuscarProductosDevolucionDataTable DTProductosBusqueda { get; set; }
        private BuscarProductosDevolucionTableAdapter TAProductosBusqueda;
        private ErrorProvider eProviderProductosBusqueda;
        public int NumeroTransaccionDevolucion{get; set;}
        /// <summary>
        /// 'C'->Devoluciones por compra, 'V'->Devoluciones por venta
        /// </summary>
        private string TipoTransaccion = "";
        public PanelBusquedaProductosDevolucion(AlvecoComercial10DataSet.DTProductosSeleccionadosDataTable DTProductosSeleccionados,
            int NumeroAlmacen, string CodigoUsuario, string TipoTransaccion)
        {
            InitializeComponent();

            this.NumeroAlmacen = NumeroAlmacen;
            this.CodigoUsuario = CodigoUsuario;
            this.DTProductosSeleccionados = DTProductosSeleccionados;
            this.TipoTransaccion = TipoTransaccion;

            TAProductosBusqueda = new BuscarProductosDevolucionTableAdapter();
            TAProductosBusqueda.Connection = Utilidades.DAOUtilidades.conexion;
            DTProductosBusqueda = new AlvecoComercial10DataSet.BuscarProductosDevolucionDataTable();

            eProviderProductosBusqueda = new ErrorProvider();
        }

        public PanelBusquedaProductosDevolucion()
        {
            InitializeComponent();
            TAProductosBusqueda = new BuscarProductosDevolucionTableAdapter();
            TAProductosBusqueda.Connection = Utilidades.DAOUtilidades.conexion;
            DTProductosBusqueda = new AlvecoComercial10DataSet.BuscarProductosDevolucionDataTable();
            eProviderProductosBusqueda = new ErrorProvider();
        }

        public void cargarParametrosConstructor(AlvecoComercial10DataSet.DTProductosSeleccionadosDataTable DTProductosSeleccionados,
            int NumeroAlmacen, string CodigoUsuario, string TipoTransaccion, int NumeroTransaccionDevolucion)
        {
            this.NumeroAlmacen = NumeroAlmacen;
            this.CodigoUsuario = CodigoUsuario;
            this.DTProductosSeleccionados = DTProductosSeleccionados;
            this.TipoTransaccion = TipoTransaccion;
            this.NumeroTransaccionDevolucion = NumeroTransaccionDevolucion;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            eProviderProductosBusqueda.Clear();
                        
            if (String.IsNullOrEmpty(txtTextoBusqueda.Text))
            {
                eProviderProductosBusqueda.SetError(txtTextoBusqueda, "Aún no ha ingresado ningun parametro para buscar");
                return;
            }


            DTProductosBusqueda = TAProductosBusqueda.GetData(NumeroAlmacen, txtTextoBusqueda.Text, TipoTransaccion, NumeroTransaccionDevolucion);
            dtGVProductosBusqueda.DataSource = DTProductosBusqueda;
            lblCantidadProductos.Text = "Total Registros : " + DTProductosBusqueda.Count.ToString();
            txtTextoBusqueda.Focus();
            txtTextoBusqueda.SelectAll();
        }

        private void dtGVProductosBusqueda_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dtGVProductosBusqueda.CurrentCell != null)
            {
                if (DTProductosSeleccionados.FindByCodigoProducto(DTProductosBusqueda[e.RowIndex].CodigoProducto) == null)
                {
                    AlvecoComercial10DataSet.DTProductosSeleccionadosRow DRProductoSeleccionado =  DTProductosSeleccionados.AddDTProductosSeleccionadosRow(DTProductosBusqueda[e.RowIndex].CodigoProducto.Trim(),
                    DTProductosBusqueda[e.RowIndex].NombreProducto, 1,                    
                    DTProductosBusqueda[e.RowIndex].PrecioUnitarioTransaccion,
                    DTProductosBusqueda[e.RowIndex].CantidadDevolucion - DTProductosBusqueda[e.RowIndex].CantidadEntregada,
                    DTProductosBusqueda[e.RowIndex].TiempoGarantia,
                    DTProductosBusqueda[e.RowIndex].NombreMarca, DTProductosBusqueda[e.RowIndex].PrecioUnitarioTransaccion, -1                    
                    );
                    DRProductoSeleccionado.PrecioTotal = DRProductoSeleccionado.Cantidad * DRProductoSeleccionado.PrecioUnitario;
                    DTProductosSeleccionados.AcceptChanges();
                   
                }
                else
                {
                    MessageBox.Show(this, "El Producto Seleccionado ya se encuentra en la Lista", "Selección de Productos", MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);

                }
                
            }
        }

        public void visualizarPanel()
        {            
            this.Visible = true;
        }

        public void ocultarPanel()
        {
            this.Visible = false;
        }

        public void limpiarControles()
        {            
            txtTextoBusqueda.Text = String.Empty;
            DTProductosBusqueda.Clear();
            DTProductosSeleccionados.Clear();
        }

        private void txtTextoBusqueda_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)Keys.Enter)
            {
                btnBuscar_Click(btnBuscar, e as EventArgs);
            }
        }


    }
}
