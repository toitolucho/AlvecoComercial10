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
    public partial class PanelBusquedaProductos : UserControl
    {
        public AlvecoComercial10DataSet.DTProductosSeleccionadosDataTable DTProductosSeleccionados{get; set;}
        private int NumeroAlmacen;
        private string CodigoUsuario;
        public AlvecoComercial10DataSet.BuscarProductoDataTable DTProductosBusqueda { get; set; }
        private BuscarProductoTableAdapter TAProductosBusqueda;
        private ErrorProvider eProviderProductosBusqueda;
        private string TipoTransaccion = "";

        public int? CodigoProveedor { get; set; }
        public decimal? PorcentajeGananciaMayor { get; set; }
        public decimal? PorcentajeGananciaMenor { get; set; }

        public PanelBusquedaProductos(AlvecoComercial10DataSet.DTProductosSeleccionadosDataTable DTProductosSeleccionados,
            int NumeroAlmacen, string CodigoUsuario, string TipoTransaccion)
        {
            InitializeComponent();

            CodigoProveedor = null;
            this.NumeroAlmacen = NumeroAlmacen;
            this.CodigoUsuario = CodigoUsuario;
            this.DTProductosSeleccionados = DTProductosSeleccionados;
            this.TipoTransaccion = TipoTransaccion;

            TAProductosBusqueda = new BuscarProductoTableAdapter();
            TAProductosBusqueda.Connection = Utilidades.DAOUtilidades.conexion;
            DTProductosBusqueda = new AlvecoComercial10DataSet.BuscarProductoDataTable();
            dtGVProductosBusqueda.AutoGenerateColumns = false;
            eProviderProductosBusqueda = new ErrorProvider();
        }

        public PanelBusquedaProductos()
        {
            InitializeComponent();            
            TAProductosBusqueda = new BuscarProductoTableAdapter();
            TAProductosBusqueda.Connection = Utilidades.DAOUtilidades.conexion;
            DTProductosBusqueda = new AlvecoComercial10DataSet.BuscarProductoDataTable();
            eProviderProductosBusqueda = new ErrorProvider();
            dtGVProductosBusqueda.AutoGenerateColumns = false;
        }

        public void cargarParametrosConstructor(AlvecoComercial10DataSet.DTProductosSeleccionadosDataTable DTProductosSeleccionados,
            int NumeroAlmacen, string CodigoUsuario, string TipoTransaccion)
        {
            this.NumeroAlmacen = NumeroAlmacen;
            this.CodigoUsuario = CodigoUsuario;
            this.DTProductosSeleccionados = DTProductosSeleccionados;
            this.TipoTransaccion = TipoTransaccion;

            if (TipoTransaccion == "V")
            {
                txtCantidadExistencia.Text = "0";
            }
        }

        public void btnBuscar_Click(object sender, EventArgs e)
        {
            eProviderProductosBusqueda.Clear();

            int cantidadBusqueda = -100;
            if (String.IsNullOrEmpty(txtTextoBusqueda.Text))
            {
                eProviderProductosBusqueda.SetError(txtTextoBusqueda, "Aún no ha ingresado ningun parametro para buscar");
                return;
            }

            if (txtCantidadExistencia.Visible && !int.TryParse(txtCantidadExistencia.Text, out cantidadBusqueda))
            {
                eProviderProductosBusqueda.SetError(txtCantidadExistencia, "la Cantidad Ingresada se encuentra mal escrita");
                return;
            }

            DTProductosBusqueda = TAProductosBusqueda.GetData(txtTextoBusqueda.Text, cantidadBusqueda, CodigoProveedor, NumeroAlmacen);
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
                    AlvecoComercial10DataSet.DTProductosSeleccionadosRow DRProductoSeleccionado =  DTProductosSeleccionados.AddDTProductosSeleccionadosRow(DTProductosBusqueda[e.RowIndex].CodigoProducto,
                    DTProductosBusqueda[e.RowIndex].NombreProducto, 1,
                    ((TipoTransaccion == "C") || (TipoTransaccion == "T")) ?
                    DTProductosBusqueda[e.RowIndex].PrecioUnitarioCompra : (dtGVProductosBusqueda.Columns[e.ColumnIndex].Name == DGCPrecioUnitarioVentaPorMenor.Name?
                    DTProductosBusqueda[e.RowIndex].PrecioUnitarioVentaPorMenor : DTProductosBusqueda[e.RowIndex].PrecioUnitarioVentaPorMayor),
                    DTProductosBusqueda[e.RowIndex].CantidadExistencia, DTProductosBusqueda[e.RowIndex].TiempoGarantiaProducto,
                    DTProductosBusqueda[e.RowIndex].NombreMarca, 0, int.Parse(DTProductosBusqueda[e.RowIndex]["CodigoProveedor"].ToString() ?? "-1")
                    );
                    DRProductoSeleccionado.PrecioTotal = DRProductoSeleccionado.Cantidad * DRProductoSeleccionado.PrecioUnitario;
                    DTProductosSeleccionados.AcceptChanges();
                    if (TipoTransaccion == "C")
                    {
                        if(!PorcentajeGananciaMenor.HasValue)
                            DRProductoSeleccionado["PorcentajeGananciaVentaPorMenor"] = DTProductosBusqueda[e.RowIndex].PorcentajeGananciaVentaPorMenor;
                        else
                            DRProductoSeleccionado["PorcentajeGananciaVentaPorMenor"] = PorcentajeGananciaMenor;
                        if(!PorcentajeGananciaMayor.HasValue)
                            DRProductoSeleccionado["PorcentajeGananciaVentaPorMayor"] = DTProductosBusqueda[e.RowIndex].PorcentajeGananciaVentaPorMayor;
                        else
                            DRProductoSeleccionado["PorcentajeGananciaVentaPorMayor"] = PorcentajeGananciaMayor;

                        //DRProducto["PrecioUnitarioVentaPorMenor"] = decimal.Round((PrecioCompraCalculado * PorcentajeUtilidad / 100) + PrecioCompraCalculado, 2);
                        if (!CodigoProveedor.HasValue)
                        {
                            DRProductoSeleccionado["PrecioUnitarioVentaPorMayor"] = DTProductosBusqueda[e.RowIndex].PrecioUnitarioVentaPorMayor;
                            DRProductoSeleccionado["PrecioUnitarioVentaPorMenor"] = DTProductosBusqueda[e.RowIndex].PrecioUnitarioVentaPorMenor;
                        }
                        else
                        {
                            DRProductoSeleccionado["PrecioUnitarioVentaPorMayor"] = decimal.Round((DTProductosBusqueda[e.RowIndex].PrecioUnitarioCompra * (PorcentajeGananciaMayor ?? 0) / 100) + DTProductosBusqueda[e.RowIndex].PrecioUnitarioCompra, 2);
                            DRProductoSeleccionado["PrecioUnitarioVentaPorMenor"] = decimal.Round((DTProductosBusqueda[e.RowIndex].PrecioUnitarioCompra * (PorcentajeGananciaMenor ?? 0) / 100) + DTProductosBusqueda[e.RowIndex].PrecioUnitarioCompra, 2);
                            
                        }
                        
                    }
                    
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
            if (TipoTransaccion == "C")
            {
                
                lblExistencia.Visible = false;
                txtCantidadExistencia.Visible = false;
            }
            this.Visible = true;
        }

        public void ocultarPanel()
        {
            this.Visible = false;
        }

        public void limpiarControles()
        {
            txtCantidadExistencia.Text = "1";
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
