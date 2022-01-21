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

namespace SAlvecoComercial10.Formularios.GestionSistema
{
    public partial class FProductos : Form
    {
        ProductosMarcasTableAdapter TAProductosMarcas;
        ProductosTableAdapter TAProductos;
        ProductosTiposTableAdapter TAProductosTipos;
        ProductosUnidadesTableAdapter TAProductosUnidades;
        BuscarProductoTableAdapter TAProductosBusqueda;
        ProveedoresTableAdapter TAProveedores;
        ErrorProvider eProviderProductos;
        Utilidades.ObjetoCodigoDescripcion TipoCalculoInventario = new Utilidades.ObjetoCodigoDescripcion();


        AlvecoComercial10DataSet.ProductosDataTable DTProductos;
        AlvecoComercial10DataSet.ProductosMarcasDataTable DTProductosMarcas;
        AlvecoComercial10DataSet.ProductosTiposDataTable DTProductosTipos;
        AlvecoComercial10DataSet.ProductosUnidadesDataTable DTProductosUnidades;
        AlvecoComercial10DataSet.BuscarProductoDataTable DTProductosBusqueda;
        AlvecoComercial10DataSet.ProveedoresDataTable DTProveedores;
        string TipoOperacion = "";
        int NumeroAlmacen;
        public FProductos(int NumeroAlmacen)
        {
            InitializeComponent();

            this.NumeroAlmacen = NumeroAlmacen;
            TAProductos = new ProductosTableAdapter();
            TAProductos.Connection = Utilidades.DAOUtilidades.conexion;
            TAProductosBusqueda = new BuscarProductoTableAdapter();
            TAProductosBusqueda.Connection = Utilidades.DAOUtilidades.conexion;
            TAProductosMarcas = new ProductosMarcasTableAdapter();
            TAProductosMarcas.Connection = Utilidades.DAOUtilidades.conexion;
            TAProductosTipos = new ProductosTiposTableAdapter();
            TAProductosTipos.Connection = Utilidades.DAOUtilidades.conexion;
            TAProductosUnidades = new ProductosUnidadesTableAdapter();
            TAProductosUnidades.Connection = Utilidades.DAOUtilidades.conexion;
            TAProveedores = new ProveedoresTableAdapter();
            TAProveedores.Connection = Utilidades.DAOUtilidades.conexion;
                 

            DTProductos = new AlvecoComercial10DataSet.ProductosDataTable();
            DTProductosBusqueda = new AlvecoComercial10DataSet.BuscarProductoDataTable();
            DTProductosMarcas = new AlvecoComercial10DataSet.ProductosMarcasDataTable();
            DTProductosTipos = new AlvecoComercial10DataSet.ProductosTiposDataTable();
            DTProductosUnidades = new AlvecoComercial10DataSet.ProductosUnidadesDataTable();

            eProviderProductos = new ErrorProvider();

            TipoCalculoInventario.cargarDatosTipoCalculoInventario();
            cBoxTipoCalculoInventario.DataSource = TipoCalculoInventario.listaObjetos;
            cBoxTipoCalculoInventario.DisplayMember = TipoCalculoInventario.DisplayMember;
            cBoxTipoCalculoInventario.ValueMember = TipoCalculoInventario.ValueMember;


            CargarDatosProductos("-23");

        }

        public void CargarDatosProductos(string CodigoProducto)
        {

            SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSet.BuscarProductoRow DRProducto = DTProductosBusqueda.FindByCodigoProducto(CodigoProducto);
            if (DRProducto == null)
            {
                AlvecoComercial10DataSet.ProductosDataTable DTProductosAux = TAProductos.GetDataBy1(CodigoProducto);
                if (DTProductosAux.Count > 0)
                {
                    AlvecoComercial10DataSet.BuscarProductoDataTable DTBusquedaAux = new AlvecoComercial10DataSet.BuscarProductoDataTable();
                    DTBusquedaAux.Rows.Add(DTProductosAux[0].ItemArray);
                    DTBusquedaAux.AcceptChanges();
                    DRProducto = DTBusquedaAux[0];
                }
                else
                {
                    limpiarControles();
                    habilitarControles(false);
                    habilitarBotones(true, false, false, false, false);
                    return;
                }
            }
            else
            {
                txtCodigoProducto.Text = DRProducto.CodigoProducto;
                txtCodigoFabricante.Text = DRProducto.CodigoProductoFabricante;
                txtNombreProducto.Text = DRProducto.NombreProducto;
                txtNombreAlternativo.Text = DRProducto.NombreProductoAlternativo;
                cBoxMarcaProducto.SelectedValue = DRProducto.CodigoMarcaProducto;
                cBoxTipoCalculoInventario.SelectedValue = DRProducto.CodigoTipoCalculoInventario;
                cBoxTipoProducto.SelectedValue = DRProducto.CodigoProductoTipo;
                cBoxUnidadProducto.SelectedValue = DRProducto.CodigoUnidadProducto;

                checkActualizarPrecios.Checked = DRProducto.ActualizarPrecioVenta;
                txtDescripcion.Text = DRProducto.IsDescripcionNull() ? String.Empty : DRProducto.Descripcion;
                txtObservaciones.Text = DRProducto.IsObservacionesNull() ? string.Empty : DRProducto.Observaciones;
                if (DRProducto.IsCodigoProveedorNull())
                    cBoxProveedor.SelectedIndex = -1;
                else
                    cBoxProveedor.SelectedValue = DRProducto.CodigoProveedor;

                habilitarBotones(true, false, false, true, true);
                habilitarControles(false);
            }
        }

        public void habilitarControles(bool estadoHabilitacion)
        {
            //txtCodigoProducto.ReadOnly = !estadoHabilitacion;
            txtCodigoFabricante.ReadOnly = !estadoHabilitacion;
            txtNombreAlternativo.ReadOnly = !estadoHabilitacion;
            txtNombreProducto.ReadOnly = !estadoHabilitacion;
            txtObservaciones.ReadOnly = !estadoHabilitacion;
            txtDescripcion.ReadOnly = !estadoHabilitacion;
            cBoxMarcaProducto.Enabled = estadoHabilitacion;
            cBoxTipoCalculoInventario.Enabled = estadoHabilitacion;
            cBoxTipoProducto.Enabled = estadoHabilitacion;
            cBoxUnidadProducto.Enabled = estadoHabilitacion;
            cBoxProveedor.Enabled = estadoHabilitacion;
            checkActualizarPrecios.Enabled = estadoHabilitacion;
            btnAgregarMarca.Enabled = estadoHabilitacion;
            btnAgregarTipo.Enabled = estadoHabilitacion;
            btnAgregarUnidad.Enabled = estadoHabilitacion;
            btnAgregarProveedor.Enabled = estadoHabilitacion;
            
        }

        public void habilitarBotones(bool nuevo, bool aceptar, bool cancelar, bool editar, bool eliminar)
        {
            this.btnNuevo.Enabled = nuevo;
            this.btnAceptar.Enabled = aceptar;
            this.btnCancelar.Enabled = cancelar;
            this.btnEditar.Enabled = editar;
            this.btnEliminar.Enabled = eliminar;
        }

        public void limpiarControles()
        {
            txtCodigoFabricante.Text = String.Empty;
            txtCodigoProducto.Text = String.Empty;
            txtNombreProducto.Text = String.Empty;
            txtNombreAlternativo.Text = String.Empty;
            txtDescripcion.Text = String.Empty;
            txtObservaciones.Text = String.Empty;
            checkActualizarPrecios.Checked = false;
            cBoxMarcaProducto.SelectedIndex = -1;
            cBoxTipoCalculoInventario.SelectedIndex = -1;
            cBoxTipoProducto.SelectedIndex = -1;
            cBoxUnidadProducto.SelectedIndex = -1;
        }

        public bool validarDatos()
        {
            if (String.IsNullOrEmpty(txtNombreProducto.Text.Trim()))
            {

                eProviderProductos.SetError(txtNombreProducto, "Aún no ha ingresado el Nombre Producto");
                txtNombreProducto.Focus();
                txtNombreProducto.SelectAll();
                return false;
            }
            if (String.IsNullOrEmpty(txtNombreAlternativo.Text.Trim()) && 
                MessageBox.Show(this,"¿Se encuentra seguro de dejar el Nombre Alternativo del Producto en Blanco?",
                this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
            {

                eProviderProductos.SetError(txtNombreAlternativo, "Aún no ha ingresado el Nombre Alternativo del Producto");
                txtNombreAlternativo.Focus();
                txtNombreAlternativo.SelectAll();
                return false;
            }

            if (cBoxTipoProducto.SelectedIndex < 0)
            {

                eProviderProductos.SetError(cBoxTipoProducto, "Aún no ha seleccionado el tipo de Producto");
                cBoxTipoProducto.Focus();
                cBoxTipoProducto.SelectAll();
                return false;
            }

            if (cBoxMarcaProducto.SelectedIndex < 0)
            {

                eProviderProductos.SetError(cBoxMarcaProducto, "Aún no ha seleccionado la marca del Producto");
                cBoxMarcaProducto.Focus();
                cBoxMarcaProducto.SelectAll();
                return false;
            }

            if (cBoxUnidadProducto.SelectedIndex < 0)
            {

                eProviderProductos.SetError(cBoxUnidadProducto, "Aún no ha seleccionado la unidad del Producto");
                cBoxUnidadProducto.Focus();
                cBoxUnidadProducto.SelectAll();
                return false;
            }

            if (cBoxTipoCalculoInventario.SelectedIndex < 0)
            {

                eProviderProductos.SetError(cBoxTipoCalculoInventario, "Aún no ha seleccionado el Tipo de Calculo en Inventario para el Producto");
                cBoxTipoCalculoInventario.Focus();
                cBoxTipoCalculoInventario.SelectAll();
                return false;
            }
            return true;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            //int NumeroProductosUnidadesiguiente = Utilidades.DAOUtilidades._QTAUtilidadesFunciones.ObtenerUltimoIndiceTabla("ProductosUnidades").Value == null
            //    ? 1 : Utilidades.DAOUtilidades._QTAUtilidadesFunciones.ObtenerUltimoIndiceTabla("ProductosUnidades").Value + 1;
            //txtCodigoProducto.Text = NumeroProductosUnidadesiguiente.ToString();
            tabControlProductos.SelectedTab = tabPageDatos;
            TipoOperacion = "I";
            habilitarControles(true);
            limpiarControles();
            habilitarBotones(false, true, true, false, false);
            txtCodigoProducto.Text = txtCodigoFabricante.Text = Utilidades.DAOUtilidades.ObtenerCodigoProductoGenerado();
            txtNombreProducto.Focus();
            cBoxTipoCalculoInventario.SelectedValue = "O";
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            tabControlProductos.SelectedTab = tabPageDatos;
            TipoOperacion = "E";
            habilitarControles(true);
            habilitarBotones(false, true, true, false, false);
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                eProviderProductos.Clear();
                if (validarDatos())
                {
                    if (TipoOperacion == "I")
                    {
                        TAProductos.Insert(txtCodigoProducto.Text, txtCodigoFabricante.Text, txtNombreProducto.Text.Trim(), txtNombreAlternativo.Text.Trim(),
                            cBoxTipoProducto.SelectedValue.ToString(), int.Parse(cBoxMarcaProducto.SelectedValue.ToString()), int.Parse(cBoxUnidadProducto.SelectedValue.ToString()),
                            cBoxTipoCalculoInventario.SelectedValue.ToString(),  checkActualizarPrecios.Checked, 
                            cBoxProveedor.SelectedIndex >= 0 ? int.Parse(cBoxProveedor.SelectedValue.ToString()) : (int?)null,
                            txtDescripcion.Text, txtObservaciones.Text);
                        AlvecoComercial10DataSet.ProductosRow DRProductoNuevo = DTProductos.AddProductosRow(txtCodigoProducto.Text, 
                            txtCodigoFabricante.Text, txtNombreProducto.Text.Trim(), txtNombreAlternativo.Text.Trim(),
                            cBoxTipoProducto.SelectedValue.ToString(), int.Parse(cBoxMarcaProducto.SelectedValue.ToString()), int.Parse(cBoxUnidadProducto.SelectedValue.ToString()),
                            cBoxTipoCalculoInventario.SelectedValue.ToString(), checkActualizarPrecios.Checked,                             
                            txtDescripcion.Text, txtObservaciones.Text, 
                            -1);
                        if (cBoxProveedor.SelectedIndex >= 0)
                            DRProductoNuevo.CodigoProveedor = int.Parse(cBoxProveedor.SelectedValue.ToString());
                        else
                            DRProductoNuevo.SetCodigoProveedorNull();
                        DRProductoNuevo.AcceptChanges();
                        DTProductos.AcceptChanges();
                    }

                    else
                    {
                        TAProductos.ActualizarProducto(txtCodigoProducto.Text, txtCodigoFabricante.Text, txtNombreProducto.Text.Trim(), txtNombreAlternativo.Text.Trim(),
                            cBoxTipoProducto.SelectedValue.ToString(), int.Parse(cBoxMarcaProducto.SelectedValue.ToString()), int.Parse(cBoxUnidadProducto.SelectedValue.ToString()),
                            cBoxTipoCalculoInventario.SelectedValue.ToString(), checkActualizarPrecios.Checked,
                            cBoxProveedor.SelectedIndex >= 0 ? int.Parse(cBoxProveedor.SelectedValue.ToString()) : (int?)null,
                            txtDescripcion.Text, txtObservaciones.Text);

                        int indiceEdicion = DTProductosBusqueda.Rows.IndexOf(DTProductosBusqueda.FindByCodigoProducto(txtCodigoProducto.Text));
                        if (indiceEdicion >= 0)
                        {
                            DTProductosBusqueda[indiceEdicion].NombreProducto = txtNombreProducto.Text;
                            DTProductosBusqueda[indiceEdicion].NombreProductoAlternativo = txtNombreAlternativo.Text;
                            DTProductosBusqueda[indiceEdicion].Descripcion = txtDescripcion.Text;
                            DTProductosBusqueda[indiceEdicion].Observaciones = txtObservaciones.Text;
                            DTProductosBusqueda[indiceEdicion].ActualizarPrecioVenta = checkActualizarPrecios.Checked;
                            DTProductosBusqueda[indiceEdicion].CodigoMarcaProducto = int.Parse(cBoxMarcaProducto.SelectedValue.ToString());
                            DTProductosBusqueda[indiceEdicion].CodigoProductoTipo = cBoxTipoProducto.SelectedValue.ToString();
                            DTProductosBusqueda[indiceEdicion].CodigoTipoCalculoInventario = cBoxTipoCalculoInventario.SelectedValue.ToString();
                            DTProductosBusqueda[indiceEdicion].CodigoUnidadProducto = int.Parse(cBoxUnidadProducto.SelectedValue.ToString());
                            DTProductosBusqueda[indiceEdicion].NombreMarca = (cBoxMarcaProducto.SelectedItem as DataRowView)["NombreMarca"].ToString();
                            DTProductosBusqueda[indiceEdicion].NombreUnidad = (cBoxUnidadProducto.SelectedItem as DataRowView)["NombreUnidad"].ToString();
                            DTProductosBusqueda[indiceEdicion].NombreProductoTipo = (cBoxTipoProducto.SelectedItem as DataRowView)["NombreProductoTipo"].ToString();
                            if (cBoxProveedor.SelectedIndex >= 0)
                                DTProductosBusqueda[indiceEdicion].CodigoProveedor = int.Parse(cBoxProveedor.SelectedValue.ToString());
                            else
                                DTProductosBusqueda[indiceEdicion].SetCodigoProveedorNull();
                            DTProductosBusqueda.AcceptChanges();
                        }
                    }
                    habilitarBotones(true, false, false, true, true);
                    habilitarControles(false);
                    TipoOperacion = "";
                    //MessageBox.Show(this, "Operación realizada correctamente", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(this, "Falta completar el llenado de alguno campos qu eson necesarios, revise su datos", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "No se pudo culminar satisfactoriamente la operación actual, debido a que ocurrió la siguiente excepcion " +
                    ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            eProviderProductos.Clear();
            TipoOperacion = "";
            limpiarControles();
            habilitarControles(false);
            habilitarBotones(true, false, false, false, false);
            bdSourceProductosUnidades_CurrentChanged(bdSourceProductos, e);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "¿Se encuentra seguro de Eliminar el registro Actual??", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    TAProductos.Delete(txtCodigoProducto.Text);
                    if (DTProductosBusqueda.FindByCodigoProducto(txtCodigoProducto.Text) != null)
                    {
                        DTProductosBusqueda.RemoveBuscarProductoRow(DTProductosBusqueda.FindByCodigoProducto(txtCodigoProducto.Text));
                        DTProductosBusqueda.AcceptChanges();
                    }

                    if (DTProductos.FindByCodigoProducto(txtCodigoProducto.Text) != null)
                    {
                        DTProductos.RemoveProductosRow(DTProductos.FindByCodigoProducto(txtCodigoProducto.Text));
                        DTProductos.AcceptChanges();
                    }

                    limpiarControles();
                    habilitarBotones(true, false, false, false, false);
                }
                catch (Exception)
                {
                    MessageBox.Show(this, "No se pudo culminar la operación actual, seguramente el registro se encuentra en uso en otras operaciones sel sistema", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void dtGVProductosUnidades_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex >= 0 && DTProductosUnidades.Count > 0)
            //{
            //    CargarDatosNombreUnidad(DTProductosUnidades[e.RowIndex].CodigoUnidad);
            //}
        }

        private void bdSourceProductosUnidades_CurrentChanged(object sender, EventArgs e)
        {
            
            //if (bdSourceProductos.Position >= 0)
            //    CargarDatosProductos(DTProductosBusqueda[bdSourceProductos.Position].CodigoProducto);
        }

        private void FProductos_Load(object sender, EventArgs e)
        {
            DTProductosMarcas = TAProductosMarcas.GetData();
            DTProductosMarcas.DefaultView.RowFilter = "CodigoTipoMarca = 'P'";
            cBoxMarcaProducto.DataSource = DTProductosMarcas;
            cBoxMarcaProducto.DisplayMember = "NombreMarca";
            cBoxMarcaProducto.ValueMember = "CodigoMarca";
            cBoxMarcaProducto.SelectedIndex = -1;

            DTProductosTipos = TAProductosTipos.GetData();
            cBoxTipoProducto.DataSource = DTProductosTipos;
            cBoxTipoProducto.DisplayMember = "NombreProductoTipo";
            cBoxTipoProducto.ValueMember = "CodigoProductoTipo";
            cBoxTipoProducto.SelectedIndex = -1;

            DTProductosUnidades = TAProductosUnidades.GetData();
            cBoxUnidadProducto.DataSource = DTProductosUnidades;
            cBoxUnidadProducto.DisplayMember = "NombreUnidad";
            cBoxUnidadProducto.ValueMember = "CodigoUnidad";
            cBoxUnidadProducto.SelectedIndex = -1;

            cBoxTipoCalculoInventario.DataSource = TipoCalculoInventario.listaObjetos;
            cBoxTipoCalculoInventario.DisplayMember = TipoCalculoInventario.DisplayMember;
            cBoxTipoCalculoInventario.ValueMember = TipoCalculoInventario.ValueMember;
            cBoxTipoCalculoInventario.SelectedIndex = -1;

            DTProveedores = TAProveedores.GetData();
            cBoxProveedor.DataSource = DTProveedores;
            cBoxProveedor.DisplayMember = "NombreRazonSocial";
            cBoxProveedor.ValueMember = "CodigoProveedor";
            cBoxProveedor.SelectedIndex = -1;
                
        }

        private void btnAgregarTipo_Click(object sender, EventArgs e)
        {
            GestionSistema.FProductosTipos formTipos = new FProductosTipos();
            formTipos.configurarFormularioIA(null);
            if (formTipos.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
                DTProductosTipos.FindByCodigoProductoTipo(formTipos.CodigoProductoTipo) == null)
            {
                DTProductosTipos.Rows.Add(TAProductosTipos.GetDataBy1(formTipos.CodigoProductoTipo)[0].ItemArray);
                DTProductosTipos.DefaultView.Sort = "NombreProductoTipo ASC";
                cBoxTipoProducto.SelectedValue = formTipos.CodigoProductoTipo;                
            }
            formTipos.Dispose();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            eProviderProductos.Clear();
            if (String.IsNullOrEmpty(txtTextoBusqueda.Text))
            {
                eProviderProductos.SetError(txtTextoBusqueda, "Aún no ha ingresado un texto de busqueda");
                txtTextoBusqueda.Focus();
                txtTextoBusqueda.SelectAll();
                return;
            }

            DTProductosBusqueda = TAProductosBusqueda.GetData(txtTextoBusqueda.Text, -100, null, NumeroAlmacen);
            bdSourceProductos.DataSource = DTProductosBusqueda;
        }

        private void btnAgregarMarca_Click(object sender, EventArgs e)
        {
            GestionSistema.FProductosMarcas formMarcas = new FProductosMarcas("P");
            formMarcas.configurarFormularioIA(null);
            if (formMarcas.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
                DTProductosMarcas.FindByCodigoMarca(formMarcas.CodigoMarca) == null)
            {
                DTProductosMarcas.Rows.Add(TAProductosMarcas.GetDataBy1(formMarcas.CodigoMarca)[0].ItemArray);
                DTProductosMarcas.DefaultView.Sort = "NombreMarca ASC";
                cBoxMarcaProducto.SelectedValue = formMarcas.CodigoMarca;
            }
            formMarcas.Dispose();
        }

        private void btnAgregarUnidad_Click(object sender, EventArgs e)
        {
            GestionSistema.FProductosUnidades formUnidades = new FProductosUnidades();
            formUnidades.configurarFormularioIA(null);
            if (formUnidades.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
                DTProductosUnidades.FindByCodigoUnidad(formUnidades.CodigoUnidad) == null)
            {
                DTProductosUnidades.Rows.Add(TAProductosUnidades.GetDataBy1(formUnidades.CodigoUnidad)[0].ItemArray);
                DTProductosUnidades.DefaultView.Sort = "NombreUnidad ASC";
                cBoxUnidadProducto.SelectedValue = formUnidades.CodigoUnidad;
            }
            formUnidades.Dispose();
        }

        private void txtTextoBusqueda_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnBuscar_Click(btnBuscar, e as EventArgs);
            }
        }

        private void dtGVProductos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dtGVProductos.CurrentCell != null && DTProductosBusqueda.Count >= 0)
            {
                tabControlProductos.SelectedTab = tabPageDatos;
            }
        }

        private void dtGVProductos_SelectionChanged(object sender, EventArgs e)
        {
            if (bdSourceProductos.Position >= 0 && dtGVProductos.CurrentCell != null)
                CargarDatosProductos(dtGVProductos.CurrentRow.Cells["DGCCodigoProducto"].Value.ToString());
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAgregarProveedor_Click(object sender, EventArgs e)
        {
            Formularios.GestionSistema.FProveedores formProveedores = new GestionSistema.FProveedores();
            formProveedores.configurarFormularioIA(-1);
            if (formProveedores.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
                DTProveedores.FindByCodigoProveedor(formProveedores.CodigoProveedor) == null
                && TAProveedores.GetDataBy1(formProveedores.CodigoProveedor).Count > 0)
            {
                DTProveedores.Rows.Add(TAProveedores.GetDataBy1(formProveedores.CodigoProveedor)[0].ItemArray);
                DTProveedores.DefaultView.Sort = "NombreRazonSocial ASC";
                cBoxProveedor.SelectedValue = formProveedores.CodigoProveedor;
            }
            formProveedores.Dispose();
        }


    }
}
