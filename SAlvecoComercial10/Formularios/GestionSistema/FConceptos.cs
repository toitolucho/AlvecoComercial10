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
    public partial class FConceptos : Form
    {
        
        ConceptosTableAdapter TAConceptos;
        SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSet.ConceptosDataTable DTConceptos;
        ErrorProvider eProviderConceptos;
        string TipoOperacion = "";
        private bool soloInsertarEditar = false;
        public int NumeroConcepto { get; set; }
        public FConceptos()
        {
            InitializeComponent();

            DTConceptos = new AccesoDatos.AlvecoComercial10DataSet.ConceptosDataTable();
            TAConceptos = new ConceptosTableAdapter();
            TAConceptos.Connection = Utilidades.DAOUtilidades.conexion;

            eProviderConceptos = new ErrorProvider();
            DTConceptos = TAConceptos.GetData();
            bdSourceConceptos.DataSource = DTConceptos;
            CargarDatosConcepto(-1);
        }

        public void CargarDatosConcepto(int NumeroConcepto)
        {

            SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSet.ConceptosRow DRConcepto = DTConceptos.FindByNumeroConcepto(NumeroConcepto);
            if (DRConcepto == null)
            {
                AlvecoComercial10DataSet.ConceptosDataTable DTConceptosAux = TAConceptos.GetDataBy1(NumeroConcepto);
                if (DTConceptosAux.Count > 0)
                    DRConcepto = DTConceptosAux[0];
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
                txtCodigo.Text = DRConcepto.NumeroConcepto.ToString();
                txtNombre.Text = DRConcepto.Concepto;


                habilitarBotones(true, false, false, DRConcepto.NumeroConcepto > 2, DRConcepto.NumeroConcepto > 2);
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
                eProviderConceptos.SetError(txtNombre, "Aún no ha ingresado el Nombre del Concepto");
                txtNombre.Focus();
                txtNombre.SelectAll();
                return false;
            }
            return true;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            int NumeroConceptoSiguiente = Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("Conceptos") == -1
                ? 1 : Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("Conceptos") + 1;
            txtCodigo.Text = NumeroConceptoSiguiente.ToString();
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
                eProviderConceptos.Clear();
                if (validarDatos())
                {
                    if (TipoOperacion == "I")
                    {
                        TAConceptos.Insert(txtNombre.Text.Trim());
                        AlvecoComercial10DataSet.ConceptosRow DRConceptoNuevo = DTConceptos.AddConceptosRow(txtNombre.Text.Trim());                        
                        DRConceptoNuevo.NumeroConcepto = int.Parse(txtCodigo.Text);
                        DRConceptoNuevo.AcceptChanges();
                        DTConceptos.AcceptChanges();
                        dtGVConceptos.FirstDisplayedScrollingRowIndex = dtGVConceptos.RowCount - 1;
                    }

                    else
                    {
                        TAConceptos.ActualizarConcepto(int.Parse(txtCodigo.Text), txtNombre.Text);
                        int indiceEdicion = DTConceptos.Rows.IndexOf(DTConceptos.FindByNumeroConcepto(int.Parse(txtCodigo.Text)));
                        DTConceptos[indiceEdicion].Concepto= txtNombre.Text;
                        DTConceptos.AcceptChanges();
                    }
                    habilitarBotones(true, false, false, true, true);
                    habilitarControles(false);
                    TipoOperacion = "";
                    //MessageBox.Show(this, "Operación realizada correctamente", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (soloInsertarEditar)
                    {
                        DialogResult = System.Windows.Forms.DialogResult.OK;
                        bdSourceConceptos.MoveLast();
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
            eProviderConceptos.Clear();
            TipoOperacion = "";
            limpiarControles();
            habilitarControles(false);
            habilitarBotones(true, false, false, false, false);
            bdSourceConceptos_CurrentChanged(bdSourceConceptos, e);
            if (soloInsertarEditar)
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "¿Se encuentra seguro de Eliminar el registro Actual??", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    limpiarControles();
                    habilitarBotones(true, false, false, false, false);
                    TAConceptos.Delete(int.Parse(txtCodigo.Text));
                    DTConceptos.Rows.Remove(DTConceptos.FindByNumeroConcepto(int.Parse(txtCodigo.Text)));
                    DTConceptos.AcceptChanges();
                }
                catch (Exception)
                {
                    MessageBox.Show(this, "No se pudo culminar la operación actual, seguramente el registro se encuentra en uso en otras operaciones sel sistema", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dtGVConceptos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex >= 0 && DTConceptos.Count > 0)
            //{
            //    CargarDatosConcepto(DTConceptos[e.RowIndex].NumeroConcepto);
            //}
        }

        private void bdSourceConceptos_CurrentChanged(object sender, EventArgs e)
        {
            //if(dtGVConceptos.CurrentCell != null)
            //    dtGVConceptos_CellContentClick(dtGVConceptos, new DataGridViewCellEventArgs(0, bdSourceConceptos.Position));
            if (bdSourceConceptos.Position >= 0)
            {
                CargarDatosConcepto(DTConceptos[bdSourceConceptos.Position].NumeroConcepto);

                if (soloInsertarEditar && (DialogResult == System.Windows.Forms.DialogResult.OK || DialogResult == System.Windows.Forms.DialogResult.Cancel))
                {
                    NumeroConcepto = DTConceptos[bdSourceConceptos.Position].NumeroConcepto;
                    this.Close();
                }
            }
            
        }

        public void configurarFormularioIA(int? NumeroConcepto)
        {
            CargarDatosConcepto(NumeroConcepto != null ? NumeroConcepto.Value : -1);
            if (NumeroConcepto == null)
            {
                int NumeroCuentaPorPagarSiguiente = Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("Conceptos") == -1
                ? 1 : Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("Conceptos") + 1;
                txtCodigo.Text = NumeroCuentaPorPagarSiguiente.ToString();
            }
            TipoOperacion = NumeroConcepto == null ? "I" : "E";
            soloInsertarEditar = true;
            gBoxGrilla.Visible = false;
            //290
            btnEditar.Visible = btnNuevo.Visible = btnEliminar.Visible = false;
            this.Size = new Size(290, this.Size.Height);
            habilitarControles(true);
            habilitarBotones(false, true, true, false, false);
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }

}
