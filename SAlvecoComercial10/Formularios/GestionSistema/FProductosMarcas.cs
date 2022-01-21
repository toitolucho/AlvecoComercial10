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
    public partial class FProductosMarcas : Form
    {

        ProductosMarcasTableAdapter TAProductosMarcas;
        SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSet.ProductosMarcasDataTable DTProductosMarcas;
        ErrorProvider eProviderProductosMarcas;
        string TipoOperacion = "";
        public int CodigoMarca { get; set; }
        private bool soloInsertarEditar = false;
        public string CodigoTipoMarca { get; set; }
        public FProductosMarcas(string CodigoTipoMarca)
        {
            InitializeComponent();
            this.CodigoTipoMarca = CodigoTipoMarca;
            DTProductosMarcas = new AccesoDatos.AlvecoComercial10DataSet.ProductosMarcasDataTable();
            TAProductosMarcas = new ProductosMarcasTableAdapter();
            TAProductosMarcas.Connection = Utilidades.DAOUtilidades.conexion;

            eProviderProductosMarcas = new ErrorProvider();
            DTProductosMarcas = TAProductosMarcas.GetData();
            bdSourceProductosMarcas.DataSource = DTProductosMarcas;
            CargarDatosNombreMarca(-1);
            
        }

        public void configurarFormularioIA(int? CodigoProductoTipo)
        {

            CargarDatosNombreMarca(CodigoProductoTipo != null ? CodigoProductoTipo.Value : -1);
            if (CodigoProductoTipo == null)
            {
                int NumeroProductosMarcasiguiente = Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("ProductosMarcas") == -1
                ? 1 : Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("ProductosMarcas") + 1;
                txtCodigo.Text = NumeroProductosMarcasiguiente.ToString();
            }
            TipoOperacion = CodigoProductoTipo == null ? "I" : "E";
            soloInsertarEditar = true;
            gBoxGrilla.Visible = false;
            //290
            btnEditar.Visible = btnNuevo.Visible = btnEliminar.Visible = false;
            this.Size = new Size(270, this.Size.Height);
            habilitarControles(true);
            habilitarBotones(false, true, true, false, false);
        }

        public void CargarDatosNombreMarca(int CodigoMarca)
        {

            SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSet.ProductosMarcasRow DRNombreMarca = DTProductosMarcas.FindByCodigoMarca(CodigoMarca);
            if (DRNombreMarca == null)
            {
                AlvecoComercial10DataSet.ProductosMarcasDataTable DTProductosMarcasAux = TAProductosMarcas.GetDataBy1(CodigoMarca);
                if (DTProductosMarcasAux.Count > 0)
                    DRNombreMarca = DTProductosMarcasAux[0];
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
                txtCodigo.Text = DRNombreMarca.CodigoMarca.ToString();
                txtNombre.Text = DRNombreMarca.NombreMarca;


                habilitarBotones(true, false, false, true, true);
                habilitarControles(false);
            }
        }

        public void habilitarControles(bool estadoHabilitacion)
        {
            txtNombre.ReadOnly = !estadoHabilitacion;
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
            txtNombre.Text = String.Empty;
        }

        public bool validarDatos()
        {
            if (String.IsNullOrEmpty(txtNombre.Text.Trim()))
            {
                eProviderProductosMarcas.SetError(txtNombre, "Aún no ha ingresado el Nombre del NombreMarca");
                txtNombre.Focus();
                txtNombre.SelectAll();
                return false;
            }
            return true;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            int NumeroProductosMarcasiguiente = Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("ProductosMarcas") == -1
                ? 1 : Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("ProductosMarcas") + 1;
            txtCodigo.Text = NumeroProductosMarcasiguiente.ToString();
            TipoOperacion = "I";
            habilitarControles(true);
            limpiarControles();
            habilitarBotones(false, true, true, false, false);
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            TipoOperacion = "E";
            habilitarControles(true);
            habilitarBotones(false, true, true, false, false);
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                eProviderProductosMarcas.Clear();
                if (validarDatos())
                {
                    if (TipoOperacion == "I")
                    {
                        int CodigoAuxiliar = int.Parse(txtCodigo.Text);
                        TAProductosMarcas.Insert(txtNombre.Text.Trim(), CodigoTipoMarca);
                        AlvecoComercial10DataSet.ProductosMarcasRow DRNombreMarcaNuevo = DTProductosMarcas.AddProductosMarcasRow(txtNombre.Text.Trim(), CodigoTipoMarca);
                        DRNombreMarcaNuevo.CodigoMarca = CodigoAuxiliar;
                        DRNombreMarcaNuevo.AcceptChanges();
                        DTProductosMarcas.AcceptChanges();
                        
                        CodigoMarca = CodigoMarca;
                    }

                    else
                    {
                        TAProductosMarcas.ActualizarProductoMarca(int.Parse(txtCodigo.Text), txtNombre.Text, CodigoTipoMarca);
                        int indiceEdicion = DTProductosMarcas.Rows.IndexOf(DTProductosMarcas.FindByCodigoMarca(int.Parse(txtCodigo.Text)));
                        DTProductosMarcas[indiceEdicion].NombreMarca = txtNombre.Text;
                        DTProductosMarcas.AcceptChanges();
                    }
                    habilitarBotones(true, false, false, true, true);
                    habilitarControles(false);
                    TipoOperacion = "";
                    //MessageBox.Show(this, "Operación realizada correctamente", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (soloInsertarEditar)
                    {
                        DialogResult = System.Windows.Forms.DialogResult.OK;
                        bdSourceProductosMarcas.MoveLast();
                    }
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
            eProviderProductosMarcas.Clear();
            TipoOperacion = "";
            limpiarControles();
            habilitarControles(false);
            habilitarBotones(true, false, false, false, false);
            bdSourceProductosMarcas_CurrentChanged(bdSourceProductosMarcas, e);
            if (soloInsertarEditar)
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "¿Se encuentra seguro de Eliminar el registro Actual??", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    TAProductosMarcas.Delete(int.Parse(txtCodigo.Text));
                    DTProductosMarcas.Rows.Remove(DTProductosMarcas.FindByCodigoMarca(int.Parse(txtCodigo.Text)));
                    DTProductosMarcas.AcceptChanges();
                    limpiarControles();
                    habilitarBotones(true, false, false, false, false);

                }
                catch (Exception)
                {
                    MessageBox.Show(this, "No se pudo culminar la operación actual, seguramente el registro se encuentra en uso en otras operaciones sel sistema", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }           
            }
        }

        private void dtGVProductosMarcas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex >= 0 && DTProductosMarcas.Count > 0)
            //{
            //    CargarDatosNombreMarca(DTProductosMarcas[e.RowIndex].CodigoMarca);
            //}
        }

        private void bdSourceProductosMarcas_CurrentChanged(object sender, EventArgs e)
        {
            //if(dtGVProductosMarcas.CurrentCell != null)
            //    dtGVProductosMarcas_CellContentClick(dtGVProductosMarcas, new DataGridViewCellEventArgs(0, bdSourceProductosMarcas.Position));
            if (bdSourceProductosMarcas.Position >= 0)
            {
                CargarDatosNombreMarca(DTProductosMarcas[bdSourceProductosMarcas.Position].CodigoMarca);
                CodigoMarca = DTProductosMarcas[bdSourceProductosMarcas.Position].CodigoMarca;
            }
            if (soloInsertarEditar && (DialogResult == System.Windows.Forms.DialogResult.OK || DialogResult == System.Windows.Forms.DialogResult.Cancel))
                this.Close();
        }

        private void FProductosMarcas_Shown(object sender, EventArgs e)
        {
            this.txtNombre.Focus();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!String.IsNullOrEmpty(TipoOperacion) && e.KeyChar == (char)Keys.Enter)
            {
                btnAceptar_Click(btnAceptar, e as EventArgs);
            }
        }
    }
}
