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
    public partial class FProductosUnidades : Form
    {

        ProductosUnidadesTableAdapter TAProductosUnidades;
        SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSet.ProductosUnidadesDataTable DTProductosUnidades;
        ErrorProvider eProviderProductosUnidades;
        string TipoOperacion = "";
        public int CodigoUnidad { get; set; }
        private bool soloInsertarEditar = false;
        public FProductosUnidades()
        {
            InitializeComponent();

            DTProductosUnidades = new AccesoDatos.AlvecoComercial10DataSet.ProductosUnidadesDataTable();
            TAProductosUnidades = new ProductosUnidadesTableAdapter();
            TAProductosUnidades.Connection = Utilidades.DAOUtilidades.conexion;

            eProviderProductosUnidades = new ErrorProvider();
            DTProductosUnidades = TAProductosUnidades.GetData();
            bdSourceProductosUnidades.DataSource = DTProductosUnidades;
            CargarDatosNombreUnidad(-1);
            this.Shown += new EventHandler(FProductosUnidades_Shown);
        }

        void FProductosUnidades_Shown(object sender, EventArgs e)
        {
            this.txtNombre.Focus();
        }

        public void configurarFormularioIA(int? CodigoUnidad)
        {

            CargarDatosNombreUnidad(CodigoUnidad != null ? CodigoUnidad.Value : -1);
            if (CodigoUnidad == null)
            {
                int NumeroProductosUnidadesiguiente = Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("ProductosUnidades") == null
                ? 1 : Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("ProductosUnidades") + 1;
                txtCodigo.Text = NumeroProductosUnidadesiguiente.ToString();
            }
            TipoOperacion = CodigoUnidad == null ? "I" : "E";
            soloInsertarEditar = true;
            gBoxGrilla.Visible = false;
            //290
            btnEditar.Visible = btnNuevo.Visible = btnEliminar.Visible = false;
            this.Size = new Size(260, this.Size.Height);
            habilitarControles(true);
            habilitarBotones(false, true, true, false, false);
        }

        public void CargarDatosNombreUnidad(int CodigoUnidad)
        {

            SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSet.ProductosUnidadesRow DRNombreUnidad = DTProductosUnidades.FindByCodigoUnidad(CodigoUnidad);
            if (DRNombreUnidad == null)
            {
                AlvecoComercial10DataSet.ProductosUnidadesDataTable DTProductosUnidadesAux = TAProductosUnidades.GetDataBy1(CodigoUnidad);
                if (DTProductosUnidadesAux.Count > 0)
                    DRNombreUnidad = DTProductosUnidadesAux[0];
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
                txtCodigo.Text = DRNombreUnidad.CodigoUnidad.ToString();
                txtNombre.Text = DRNombreUnidad.NombreUnidad;


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
                eProviderProductosUnidades.SetError(txtNombre, "Aún no ha ingresado el Nombre del NombreUnidad");
                txtNombre.Focus();
                txtNombre.SelectAll();
                return false;
            }
            return true;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            int NumeroProductosUnidadesiguiente = Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("ProductosUnidades") == -1
                ? 1 : Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("ProductosUnidades") + 1;
            txtCodigo.Text = NumeroProductosUnidadesiguiente.ToString();
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
                eProviderProductosUnidades.Clear();
                if (validarDatos())
                {
                    if (TipoOperacion == "I")
                    {
                        int CodigoAuxiliar = int.Parse(txtCodigo.Text);
                        TAProductosUnidades.Insert(txtNombre.Text.Trim());
                        AlvecoComercial10DataSet.ProductosUnidadesRow DRNombreUnidadNuevo = DTProductosUnidades.AddProductosUnidadesRow(txtNombre.Text.Trim());
                        DRNombreUnidadNuevo.CodigoUnidad = CodigoAuxiliar;
                        DRNombreUnidadNuevo.AcceptChanges();
                        DTProductosUnidades.AcceptChanges();

                        CodigoUnidad = CodigoAuxiliar;
                    }

                    else
                    {
                        TAProductosUnidades.ActualizarProductoUnidad(int.Parse(txtCodigo.Text), txtNombre.Text);
                        int indiceEdicion = DTProductosUnidades.Rows.IndexOf(DTProductosUnidades.FindByCodigoUnidad(int.Parse(txtCodigo.Text)));
                        DTProductosUnidades[indiceEdicion].NombreUnidad = txtNombre.Text;
                        DTProductosUnidades.AcceptChanges();
                    }
                    habilitarBotones(true, false, false, true, true);
                    habilitarControles(false);
                    TipoOperacion = "";
                    //MessageBox.Show(this, "Operación realizada correctamente", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (soloInsertarEditar)
                    {
                        DialogResult = System.Windows.Forms.DialogResult.OK;
                        bdSourceProductosUnidades.MoveLast();
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
            eProviderProductosUnidades.Clear();
            TipoOperacion = "";
            limpiarControles();
            habilitarControles(false);
            habilitarBotones(true, false, false, false, false);
            bdSourceProductosUnidades_CurrentChanged(bdSourceProductosUnidades, e);
            if (soloInsertarEditar)
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "¿Se encuentra seguro de Eliminar el registro Actual??", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    TAProductosUnidades.Delete(int.Parse(txtCodigo.Text));
                    DTProductosUnidades.Rows.Remove(DTProductosUnidades.FindByCodigoUnidad(int.Parse(txtCodigo.Text)));
                    DTProductosUnidades.AcceptChanges();
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
            //if(dtGVProductosUnidades.CurrentCell != null)
            //    dtGVProductosUnidades_CellContentClick(dtGVProductosUnidades, new DataGridViewCellEventArgs(0, bdSourceProductosUnidades.Position));
            if (bdSourceProductosUnidades.Position >= 0)
            {
                CargarDatosNombreUnidad(DTProductosUnidades[bdSourceProductosUnidades.Position].CodigoUnidad);
                CodigoUnidad = DTProductosUnidades[bdSourceProductosUnidades.Position].CodigoUnidad;
            }

            if (soloInsertarEditar && (DialogResult == System.Windows.Forms.DialogResult.OK || DialogResult == System.Windows.Forms.DialogResult.Cancel))
                this.Close();
            
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
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
