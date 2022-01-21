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
    public partial class FProductosTipos : Form
    {

        ProductosTiposTableAdapter TAProductosTipos;
        SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSet.ProductosTiposDataTable DTProductosTipos;
        SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSet.ProductosTiposDataTable DTProductosTiposCBox;
        ErrorProvider eProviderProductosTipos;
        private string TipoOperacion = "";
        public string CodigoProductoTipo { get; set; }
        private bool soloInsertarEditar = false;
        public FProductosTipos()
        {
            InitializeComponent();

            DTProductosTipos = new AccesoDatos.AlvecoComercial10DataSet.ProductosTiposDataTable();
            TAProductosTipos = new ProductosTiposTableAdapter();
            TAProductosTipos.Connection = Utilidades.DAOUtilidades.conexion;

            eProviderProductosTipos = new ErrorProvider();
            DTProductosTipos = TAProductosTipos.GetData();
            bdSourceProductosTipos.DataSource = DTProductosTipos;
            DialogResult = System.Windows.Forms.DialogResult.None;

            DTProductosTiposCBox = TAProductosTipos.GetData();
            DTProductosTiposCBox.DefaultView.Sort = "NombreProductoTipo ASC";
            cBoxProductoPadre.DataSource = DTProductosTiposCBox;
            cBoxProductoPadre.DisplayMember = "NombreProductoTipo";
            cBoxProductoPadre.ValueMember = "CodigoProductoTipo";
            CargarDatosNombreMarca("-1");
            this.Shown += new EventHandler(FProductosTipos_Shown);
            
        }

        void FProductosTipos_Shown(object sender, EventArgs e)
        {
            this.txtNombre.Focus();
        }

        public void configurarFormularioIA(string CodigoProductoTipo)
        {
            
            CargarDatosNombreMarca(CodigoProductoTipo);
            TipoOperacion = String.IsNullOrEmpty(CodigoProductoTipo) ? "I" : "E";
            soloInsertarEditar = true;
            gBoxGrilla.Visible = false;
            //290
            btnEditar.Visible = btnNuevo.Visible = btnEliminar.Visible = false;
            this.Size = new Size(380, this.Size.Height);
            habilitarControles(true);
            habilitarBotones(false, true, true, false, false);
            txtCodigo.Text = Utilidades.DAOUtilidades.GenerarCodigoProductoTipo();
        }

        public void CargarDatosNombreMarca(string CodigoProductoTipo)
        {

            SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSet.ProductosTiposRow DRProductoTipo = DTProductosTipos.FindByCodigoProductoTipo(CodigoProductoTipo);
            if (DRProductoTipo == null)
            {
                AlvecoComercial10DataSet.ProductosTiposDataTable DTProductosTiposAux = TAProductosTipos.GetDataBy1(CodigoProductoTipo);
                if (DTProductosTiposAux.Count > 0)
                    DRProductoTipo = DTProductosTiposAux[0];
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
                txtCodigo.Text = DRProductoTipo.CodigoProductoTipo.ToString();
                txtNombre.Text = DRProductoTipo.NombreProductoTipo;
                if(DRProductoTipo.IsCodigoProductoTipoPadreNull())
                    cBoxProductoPadre.SelectedIndex = -1;
                else
                    cBoxProductoPadre.SelectedValue = DRProductoTipo.CodigoProductoTipoPadre;
                txtDescripcion.Text = DRProductoTipo.IsDescripcionProductoTipoNull() ? String.Empty : DRProductoTipo.DescripcionProductoTipo;

                habilitarBotones(true, false, false, true, true);
                habilitarControles(false);
            }
        }

        public void habilitarControles(bool estadoHabilitacion)
        {
            txtNombre.ReadOnly = !estadoHabilitacion;
            //txtCodigo.ReadOnly = !estadoHabilitacion;
            txtDescripcion.ReadOnly = !estadoHabilitacion;
            cBoxProductoPadre.Enabled = estadoHabilitacion;
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
            txtCodigo.Text = String.Empty;
            txtDescripcion.Text = String.Empty;
            cBoxProductoPadre.SelectedIndex = -1;
        }

        public bool validarDatos()
        {
            if (String.IsNullOrEmpty(txtNombre.Text.Trim()))
            {
                eProviderProductosTipos.SetError(txtNombre, "Aún no ha ingresado el Nombre del NombreMarca");
                txtNombre.Focus();
                txtNombre.SelectAll();
                return false;
            }
            return true;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            //int NumeroProductosTiposiguiente = Utilidades.DAOUtilidades._QTAUtilidadesFunciones.ObtenerUltimoIndiceTabla("ProductosTipos").Value == null
            //    ? 1 : Utilidades.DAOUtilidades._QTAUtilidadesFunciones.ObtenerUltimoIndiceTabla("ProductosTipos").Value + 1;
            //txtCodigo.Text = "";
            TipoOperacion = "I";
            habilitarControles(true);
            limpiarControles();
            habilitarBotones(false, true, true, false, false);
            txtCodigo.Text = Utilidades.DAOUtilidades.GenerarCodigoProductoTipo();
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
                eProviderProductosTipos.Clear();
                if (validarDatos())
                {
                    if (TipoOperacion == "I")
                    {
                        TAProductosTipos.Insert(txtCodigo.Text, cBoxProductoPadre.SelectedIndex >= 0 ? 
                            cBoxProductoPadre.SelectedValue.ToString() : null, txtNombre.Text.Trim(), txtDescripcion.Text);
                        AlvecoComercial10DataSet.ProductosTiposRow DRNombreMarcaNuevo = DTProductosTipos.AddProductosTiposRow(txtCodigo.Text, 
                            cBoxProductoPadre.SelectedIndex >= 0 ? cBoxProductoPadre.SelectedValue.ToString() : null, txtNombre.Text.Trim(), txtDescripcion.Text);
                        //DRNombreMarcaNuevo.CodigoProductoTipo = txtCodigo.Text;
                        DRNombreMarcaNuevo.AcceptChanges();
                        DTProductosTipos.AcceptChanges();

                        DTProductosTiposCBox.Rows.Add(DRNombreMarcaNuevo.ItemArray);
                        DTProductosTiposCBox.DefaultView.Sort = "NombreProductoTipo ASC";
                        CodigoProductoTipo = txtCodigo.Text;
                    }

                    else
                    {
                        TAProductosTipos.ActualizarProductoTipo(txtCodigo.Text, cBoxProductoPadre.SelectedIndex >= 0 ? 
                            cBoxProductoPadre.SelectedValue.ToString() : null, txtNombre.Text.Trim(), txtDescripcion.Text);
                        int indiceEdicion = DTProductosTipos.Rows.IndexOf(DTProductosTipos.FindByCodigoProductoTipo(txtCodigo.Text));
                        DTProductosTipos[indiceEdicion].NombreProductoTipo = txtNombre.Text;
                        DTProductosTipos[indiceEdicion].CodigoProductoTipoPadre = cBoxProductoPadre.SelectedIndex >= 0 ? cBoxProductoPadre.SelectedValue.ToString() : null;
                        DTProductosTipos[indiceEdicion].DescripcionProductoTipo = txtDescripcion.Text;
                        DTProductosTipos.AcceptChanges();
                    }
                    habilitarBotones(true, false, false, true, true);
                    habilitarControles(false);
                    TipoOperacion = "";
                    //MessageBox.Show(this, "Operación realizada correctamente", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (soloInsertarEditar)
                    {
                        DialogResult = System.Windows.Forms.DialogResult.OK;
                        bdSourceProductosTipos.MoveLast();
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
            eProviderProductosTipos.Clear();
            TipoOperacion = "";
            limpiarControles();
            habilitarControles(false);
            habilitarBotones(true, false, false, false, false);
            bdSourceProductosTipos_CurrentChanged(bdSourceProductosTipos, e);
            if (soloInsertarEditar)
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "¿Se encuentra seguro de Eliminar el registro Actual??", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {

                    
                    TAProductosTipos.Delete(txtCodigo.Text);
                    DTProductosTipos.Rows.RemoveAt(bdSourceProductosTipos.Position);
                    DTProductosTipos.AcceptChanges();
                    limpiarControles();
                    habilitarBotones(true, false, false, false, false);
                }
                catch (Exception)
                {
                    MessageBox.Show(this, "No se pudo culminar la operación actual, seguramente el registro se encuentra en uso en otras operaciones sel sistema", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dtGVProductosTipos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex >= 0 && DTProductosTipos.Count > 0)
            //{
            //    CargarDatosNombreMarca(DTProductosTipos[e.RowIndex].CodigoProductoTipo);
            //}
        }

        private void bdSourceProductosTipos_CurrentChanged(object sender, EventArgs e)
        {
            //if(dtGVProductosTipos.CurrentCell != null)
            //    dtGVProductosTipos_CellContentClick(dtGVProductosTipos, new DataGridViewCellEventArgs(0, bdSourceProductosTipos.Position));
            if (bdSourceProductosTipos.Position >= 0)
            {
                CodigoProductoTipo = DTProductosTipos[bdSourceProductosTipos.Position].CodigoProductoTipo;
                CargarDatosNombreMarca(CodigoProductoTipo);

                if (soloInsertarEditar && (DialogResult == System.Windows.Forms.DialogResult.OK || DialogResult == System.Windows.Forms.DialogResult.Cancel))
                    this.Close();
            }
        }

        private void FProductosTipos_Load(object sender, EventArgs e)
        {
            
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtDescripcion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!String.IsNullOrEmpty(TipoOperacion) && e.KeyChar == (char)Keys.Enter)
            {
                btnAceptar_Click(btnAceptar, e as EventArgs);
            }
        }
    }
}
