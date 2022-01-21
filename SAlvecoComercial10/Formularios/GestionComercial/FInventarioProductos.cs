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


namespace SAlvecoComercial10.Formularios.GestionComercial
{
    public partial class FInventarioProductos : Form
    {
        InventariosProductosTableAdapter TAInventariosProductos;
        BuscarProductoTableAdapter TAProductosBusqueda;
        ProductosTableAdapter TAProductos;
        AlmacenesTableAdapter TAAlmaacenes;
        AlvecoComercial10DataSet.BuscarProductoDataTable DTProductosBusqueda;
        int NumeroAlmacen = 1;

        public FInventarioProductos(int NumeroAlmacen)
        {
            InitializeComponent();

            this.NumeroAlmacen = NumeroAlmacen;
            TAInventariosProductos = new InventariosProductosTableAdapter();
            TAInventariosProductos.Connection = Utilidades.DAOUtilidades.conexion;
            TAProductosBusqueda = new BuscarProductoTableAdapter();
            TAProductosBusqueda.Connection = Utilidades.DAOUtilidades.conexion;
            TAAlmaacenes = new AlmacenesTableAdapter();
            TAAlmaacenes.Connection = Utilidades.DAOUtilidades.conexion;
            TAProductos = new ProductosTableAdapter();
            TAProductos.Connection = Utilidades.DAOUtilidades.conexion;
            DTProductosBusqueda = new AlvecoComercial10DataSet.BuscarProductoDataTable();
            cBoxAlmacen.DataSource = TAAlmaacenes.GetData();
            cBoxAlmacen.ValueMember = "NumeroAlmacen";
            cBoxAlmacen.DisplayMember = "NombreAlmacen";

            limpiarControles();
            habilitarControles(false);
        }

        public void limpiarControles()
        {
            txtCantidadExistencia.Text = String.Empty;
            txtCantidadRequerida.Text = String.Empty;
            txtPorcentajeGananciaVentaPorMayor.Text = String.Empty;
            txtPorcentajeGananciaVentaPorMenor.Text = String.Empty;
            txtPrecioUnitarioCompra.Text = String.Empty;
            txtPrecioUnitarioVentaPorMayor.Text = String.Empty;
            txtPrecioUnitarioVentaPorMenor.Text = String.Empty;
            txtStockMinimo.Text = String.Empty;
            txtTiempoGarantiaProducto.Text = String.Empty;
            lblFechaIngreso.Text = String.Empty;
            lblProductoDatos.Text = String.Empty;
            lblStock.Text = String.Empty;
        }

        public void habilitarBotones(bool modificar, bool aceptar, bool cancelar)
        {
            btnEditar.Enabled = modificar;
            btnAceptar.Enabled = aceptar;
            btnCancelar.Enabled = cancelar;
        }

        public void habilitarControles(bool estadoHabilitacion)
        {
            //txtCantidadExistencia.ReadOnly = !estadoHabilitacion;
            txtCantidadRequerida.ReadOnly = !estadoHabilitacion;
            txtPorcentajeGananciaVentaPorMayor.ReadOnly = !estadoHabilitacion;
            txtPorcentajeGananciaVentaPorMenor.ReadOnly = !estadoHabilitacion;
            txtPrecioUnitarioCompra.ReadOnly = !estadoHabilitacion;
            txtPrecioUnitarioVentaPorMayor.ReadOnly = !estadoHabilitacion;
            txtPrecioUnitarioVentaPorMenor.ReadOnly = !estadoHabilitacion;
            txtStockMinimo.ReadOnly = !estadoHabilitacion;
            txtTiempoGarantiaProducto.ReadOnly = !estadoHabilitacion;
        }

        public void cargarDatosInventarioProducto(string CodigoProducto)
        {
            SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSet.BuscarProductoRow DRProducto = DTProductosBusqueda.FindByCodigoProducto(CodigoProducto);
            if (DRProducto == null)
            {
                AlvecoComercial10DataSet.ProductosDataTable DTProductosAux = TAProductos.GetDataBy1(CodigoProducto);
                if (DTProductosAux.Count > 0)
                {
                    AlvecoComercial10DataSet.BuscarProductoDataTable DTBusquedaAux = new AlvecoComercial10DataSet.BuscarProductoDataTable();
                    DTBusquedaAux.ImportRow(DTProductosAux[0]);
                    DTBusquedaAux.AcceptChanges();
                    DRProducto = DTBusquedaAux[0];
                }
                else
                {
                    limpiarControles();
                    habilitarControles(false);
                    habilitarBotones(false, false, false);
                    return;
                }
            }
            else
            {
                txtCantidadExistencia.Text = DRProducto.CantidadExistencia.ToString();
                txtCantidadRequerida.Text = DRProducto.CantidadRequerida.ToString();
                //txtPorcentajeGananciaVentaPorMayor.Text = !DRProducto.IsPorcentajeGananciaVentaPorMayorNull() ?
                //    DRProducto.PorcentajeGananciaVentaPorMayor.ToString() : "0";
                //txtPorcentajeGananciaVentaPorMenor.Text = !DRProducto.IsPorcentajeGananciaVentaPorMenorNull() ?
                //    DRProducto.PorcentajeGananciaVentaPorMenor.ToString() : "0";
                //txtPrecioUnitarioCompra.Text = !DRProducto.IsPrecioUnitarioCompraNull() ?
                //    DRProducto.PrecioUnitarioCompra.ToString() : "0";
                //txtPrecioUnitarioVentaPorMayor.Text = !DRProducto.IsPrecioUnitarioVentaPorMayorNull() ?
                //    DRProducto.PrecioUnitarioVentaPorMayor.ToString() : "0";
                //txtPrecioUnitarioVentaPorMenor.Text = !DRProducto.IsPrecioUnitarioVentaPorMenorNull() ?
                //    DRProducto.PrecioUnitarioVentaPorMenor.ToString() : "0";
                //txtStockMinimo.Text = !DRProducto.IsStockMinimoNull() ? DRProducto.StockMinimo.ToString() : "0";
                //txtTiempoGarantiaProducto.Text = !DRProducto.IsTiempoGarantiaProductoNull() ? 
                //    DRProducto.TiempoGarantiaProducto.ToString() : "0";


                txtPorcentajeGananciaVentaPorMayor.Text = DRProducto.PorcentajeGananciaVentaPorMayor.ToString();
                txtPorcentajeGananciaVentaPorMenor.Text = DRProducto.PorcentajeGananciaVentaPorMenor.ToString();
                txtPrecioUnitarioCompra.Text = DRProducto.PrecioUnitarioCompra.ToString();
                txtPrecioUnitarioVentaPorMayor.Text = DRProducto.PrecioUnitarioVentaPorMayor.ToString();
                txtPrecioUnitarioVentaPorMenor.Text = DRProducto.PrecioUnitarioVentaPorMenor.ToString();
                txtStockMinimo.Text = DRProducto.StockMinimo.ToString();
                txtTiempoGarantiaProducto.Text = DRProducto.TiempoGarantiaProducto.ToString();

                lblFechaIngreso.Text = "Fecha Ingreso " + DRProducto["FechaHoraIngresoInventario"].ToString();
                lblProductoDatos.Text = "Producto : " + DRProducto.NombreProducto + ",    Código : " + DRProducto.CodigoProducto;

                if (DRProducto.CantidadExistencia > 0)
                {
                    lblStock.Text = "STOCK EXISTENTE";
                    lblStock.ForeColor = Color.Azure;
                }
                else
                {
                    lblStock.Text = "STOCK INEXISTENTE";
                    lblStock.ForeColor = Color.Brown;
                }


                habilitarBotones(true, false, false);
                habilitarControles(false);
            }
        }

        public bool validarDatos()
        {
            if (String.IsNullOrEmpty(txtCantidadExistencia.Text.Trim()))
            {
                eProvicerInventario.SetError(txtCantidadExistencia, "Aún no ha ingresado la Cantidad de Existencia");
                txtCantidadExistencia.Focus();
                txtCantidadExistencia.SelectAll();
                return false;
            }
            int CantidadExistencia = -1;
            if (!int.TryParse(txtCantidadExistencia.Text, out CantidadExistencia))
            {
                eProvicerInventario.SetError(txtCantidadExistencia, "La Cantidad de Existencia se encuentra mal Escrita");
                txtCantidadExistencia.Focus();
                txtCantidadExistencia.SelectAll();
                return false;
            }

            if (String.IsNullOrEmpty(txtCantidadRequerida.Text.Trim()))
            {
                eProvicerInventario.SetError(txtCantidadRequerida, "Aún no ha ingresado la Cantidad Requerida");
                txtCantidadRequerida.Focus();
                txtCantidadRequerida.SelectAll();
                return false;
            }
            int CantidadRequerida = -1;
            if (!int.TryParse(txtCantidadRequerida.Text, out CantidadRequerida))
            {
                eProvicerInventario.SetError(txtCantidadRequerida, "La Cantidad de Existencia se encuentra mal Escrita");
                txtCantidadRequerida.Focus();
                txtCantidadRequerida.SelectAll();
                return false;
            }

            if (String.IsNullOrEmpty(txtStockMinimo.Text.Trim()))
            {
                eProvicerInventario.SetError(txtStockMinimo, "Aún no ha ingresado el Stock Minimo que debe tener el Producto en Inventario");
                txtStockMinimo.Focus();
                txtStockMinimo.SelectAll();
                return false;
            }
            int Stock = -1;
            if (!int.TryParse(txtStockMinimo.Text, out Stock))
            {
                eProvicerInventario.SetError(txtStockMinimo, "La El stock Mínimo se encuentra mal Escrita");
                txtStockMinimo.Focus();
                txtStockMinimo.SelectAll();
                return false;
            }


            if (String.IsNullOrEmpty(txtPrecioUnitarioCompra.Text.Trim()))
            {
                eProvicerInventario.SetError(txtPrecioUnitarioCompra, "No puede Dejar en blanco el Precio de Compra");
                txtPrecioUnitarioCompra.Focus();
                txtPrecioUnitarioCompra.SelectAll();
                return false;
            }
            if (String.IsNullOrEmpty(txtPrecioUnitarioVentaPorMayor.Text.Trim()))
            {
                eProvicerInventario.SetError(txtPrecioUnitarioVentaPorMayor, "No puede Dejar en blanco el Precio de Venta Por Mayor");
                txtPrecioUnitarioVentaPorMayor.Focus();
                txtPrecioUnitarioVentaPorMayor.SelectAll();
                return false;
            }
            if (String.IsNullOrEmpty(txtPrecioUnitarioVentaPorMenor.Text.Trim()))
            {
                eProvicerInventario.SetError(txtPrecioUnitarioVentaPorMenor, "No puede Dejar en blanco el Precio de Venta Por Menor");
                txtPrecioUnitarioVentaPorMenor.Focus();
                txtPrecioUnitarioVentaPorMenor.SelectAll();
                return false;
            }
            if (String.IsNullOrEmpty(txtPorcentajeGananciaVentaPorMayor.Text.Trim()))
            {
                eProvicerInventario.SetError(txtPorcentajeGananciaVentaPorMayor, "Aún no ha ingresado El Porcentaje de Ganancia para las Ventas Por Mayor");
                txtPorcentajeGananciaVentaPorMayor.Focus();
                txtPorcentajeGananciaVentaPorMayor.SelectAll();
                return false;
            }
            if (String.IsNullOrEmpty(txtPorcentajeGananciaVentaPorMenor.Text.Trim()))
            {
                eProvicerInventario.SetError(txtPorcentajeGananciaVentaPorMenor, "Aún no ha ingresado El Porcentaje de Ganancia para las Ventas Por Menor");
                txtPorcentajeGananciaVentaPorMenor.Focus();
                txtPorcentajeGananciaVentaPorMenor.SelectAll();
                return false;
            }            
            if (String.IsNullOrEmpty(txtTiempoGarantiaProducto.Text.Trim()))
            {
                eProvicerInventario.SetError(txtTiempoGarantiaProducto, "Aún no ha ingresado el Tiempo de Garantía que debe tener el Producto en Inventario");
                txtTiempoGarantiaProducto.Focus();
                txtTiempoGarantiaProducto.SelectAll();
                return false;
            }
            return true;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            eProvicerInventario.Clear();
            if (String.IsNullOrEmpty(txtTextoBusqueda.Text))
            {
                eProvicerInventario.SetError(txtTextoBusqueda, "Aún no ha ingresado ningún Artículo");
                txtTextoBusqueda.Focus();
                txtTextoBusqueda.SelectAll();
                return;
            }

            if (cBoxAlmacen.SelectedIndex < 0)
            {
                eProvicerInventario.SetError(cBoxAlmacen, "Aún no ha seleccionado el almacen");
                cBoxAlmacen.Focus();
                cBoxAlmacen.SelectAll();
                return;
            }

            DTProductosBusqueda = TAProductosBusqueda.GetData(txtTextoBusqueda.Text, -1, null, int.Parse(cBoxAlmacen.SelectedValue.ToString()));
            bdSourceInventario.DataSource = DTProductosBusqueda;
        }

        private void bdSourceInventario_CurrentChanged(object sender, EventArgs e)
        {
            if (bdSourceInventario.Position >= 0)
            {
                cargarDatosInventarioProducto(DTProductosBusqueda[bdSourceInventario.Position].CodigoProducto);
                
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            habilitarBotones(false, true, true);
            dtGVInventarioProductos.Enabled = false;
            habilitarControles(true);
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            eProvicerInventario.Clear();
            if (!validarDatos())
            {
                MessageBox.Show(this, "Existen algunos problemas al tratar de modificar el registro actual, corrijalos porfavor",
                    this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            TAInventariosProductos.ActualizarInventarioProducto(NumeroAlmacen, DTProductosBusqueda[bdSourceInventario.Position].CodigoProducto,
                int.Parse(txtCantidadExistencia.Text), int.Parse(txtCantidadRequerida.Text), decimal.Parse(txtPrecioUnitarioCompra.Text),
                decimal.Parse(txtPrecioUnitarioVentaPorMayor.Text), decimal.Parse(txtPrecioUnitarioVentaPorMenor.Text),
                decimal.Parse(txtPorcentajeGananciaVentaPorMayor.Text), decimal.Parse(txtPorcentajeGananciaVentaPorMenor.Text),
                int.Parse(txtTiempoGarantiaProducto.Text), int.Parse(txtStockMinimo.Text));

            DTProductosBusqueda[bdSourceInventario.Position].CantidadExistencia = int.Parse(txtCantidadExistencia.Text);
            DTProductosBusqueda[bdSourceInventario.Position].CantidadRequerida = int.Parse(txtCantidadRequerida.Text);
            DTProductosBusqueda[bdSourceInventario.Position].PrecioUnitarioCompra = decimal.Parse(txtPrecioUnitarioCompra.Text);
            DTProductosBusqueda[bdSourceInventario.Position].PrecioUnitarioVentaPorMayor = decimal.Parse(txtPrecioUnitarioVentaPorMayor.Text);
            DTProductosBusqueda[bdSourceInventario.Position].PrecioUnitarioVentaPorMenor = decimal.Parse(txtPrecioUnitarioVentaPorMenor.Text);
            DTProductosBusqueda[bdSourceInventario.Position].PorcentajeGananciaVentaPorMayor = decimal.Parse(txtPorcentajeGananciaVentaPorMayor.Text);
            DTProductosBusqueda[bdSourceInventario.Position].PorcentajeGananciaVentaPorMenor = decimal.Parse(txtPorcentajeGananciaVentaPorMenor.Text);
            DTProductosBusqueda[bdSourceInventario.Position].TiempoGarantiaProducto = int.Parse(txtTiempoGarantiaProducto.Text);
            DTProductosBusqueda[bdSourceInventario.Position].StockMinimo = int.Parse(txtStockMinimo.Text);
            
            DTProductosBusqueda.AcceptChanges();

            habilitarBotones(true, false, false);
            dtGVInventarioProductos.Enabled = true;
            habilitarControles(false);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            habilitarBotones(true, false, false);
            dtGVInventarioProductos.Enabled = true;
            habilitarControles(false);
            //limpiarControles();
            bdSourceInventario_CurrentChanged(bdSourceInventario, e);
            
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtTextoBusqueda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnBuscar_Click(btnBuscar, e as EventArgs);
            }
        }

        private void txtPrecioUnitarioVentaPorMayor_TextChanged(object sender, EventArgs e)
        {
            //if (!String.IsNullOrEmpty(txtPrecioUnitarioVentaPorMayor.Text) &&)
            //{

            //}
        }

        private void txtPorcentajeGananciaVentaPorMayor_TextChanged(object sender, EventArgs e)
        {
            if (btnAceptar.Enabled  && !String.IsNullOrEmpty(txtPorcentajeGananciaVentaPorMayor.Text)
                && !String.IsNullOrEmpty(txtPrecioUnitarioCompra.Text))
            {
                //DRProducto["PrecioUnitarioVentaPorMenor"] = decimal.Round((PrecioCompraCalculado * PorcentajeUtilidad / 100) + PrecioCompraCalculado, 2);
                decimal PrecioCompraCalculado = decimal.Parse(txtPrecioUnitarioCompra.Text);
                decimal PorcentajeUtilidad = decimal.Parse(txtPorcentajeGananciaVentaPorMayor.Text);
                txtPrecioUnitarioVentaPorMayor.Text = decimal.Round((PrecioCompraCalculado * PorcentajeUtilidad / 100) + PrecioCompraCalculado, 2).ToString();
            }
        }

        private void txtPorcentajeGananciaVentaPorMenor_TextChanged(object sender, EventArgs e)
        {
            if (btnAceptar.Enabled && !String.IsNullOrEmpty(txtPorcentajeGananciaVentaPorMenor.Text)
               && !String.IsNullOrEmpty(txtPrecioUnitarioCompra.Text))
            {
                decimal PrecioCompraCalculado = decimal.Parse(txtPrecioUnitarioCompra.Text);
                decimal PorcentajeUtilidad = decimal.Parse(txtPorcentajeGananciaVentaPorMenor.Text);
                txtPrecioUnitarioVentaPorMenor.Text = decimal.Round((PrecioCompraCalculado * PorcentajeUtilidad / 100) + PrecioCompraCalculado, 2).ToString();
            }
        }



    }
}
