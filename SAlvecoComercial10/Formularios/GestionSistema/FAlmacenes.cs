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
    public partial class FAlmacenes : Form
    {
        AlmacenesTableAdapter TAAlmacenes;
        SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSet.AlmacenesDataTable DTAlmacenes;        
        ErrorProvider eProviderAlmacenes;
        string TipoOperacion = "";
        public FAlmacenes()
        {
            InitializeComponent();

            DTAlmacenes = new AccesoDatos.AlvecoComercial10DataSet.AlmacenesDataTable();
            TAAlmacenes = new AlmacenesTableAdapter();
            TAAlmacenes.Connection = Utilidades.DAOUtilidades.conexion;
            
            eProviderAlmacenes = new ErrorProvider();
            DTAlmacenes = TAAlmacenes.GetData();
            bdSourceAlmacenes.DataSource = DTAlmacenes;
            CargarDatosAlmacen(-1);
        }

        public void CargarDatosAlmacen(int NumeroAlmacen)
        {
            
            SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSet.AlmacenesRow DRAlmacen = DTAlmacenes.FindByNumeroAlmacen(NumeroAlmacen);
            if (DRAlmacen == null)
            {
                AlvecoComercial10DataSet.AlmacenesDataTable DTAlmacenesAux = TAAlmacenes.GetDataBy1(NumeroAlmacen);
                if (DTAlmacenesAux.Count > 0)
                    DRAlmacen = DTAlmacenesAux[0];
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
                txtCodigo.Text = DRAlmacen.NumeroAlmacen.ToString();
                txtNombre.Text = DRAlmacen.NombreAlmacen;
                txtDescripcion.Text = DRAlmacen.IsDescripcionNull() ? String.Empty : DRAlmacen.Descripcion;

                habilitarBotones(true, false, false, true, true);
                habilitarControles(false);
            }
        }

        public void habilitarControles(bool estadoHabilitacion)
        {
            txtNombre.ReadOnly = !estadoHabilitacion;
            txtDescripcion.ReadOnly = !estadoHabilitacion;
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
            txtDescripcion.Text = string.Empty;
        }

        public bool validarDatos()
        {
            if (String.IsNullOrEmpty(txtNombre.Text.Trim()))
            {
                eProviderAlmacenes.SetError(txtNombre, "Aún no ha ingresado el Nombre del almacen");
                txtNombre.Focus();
                txtNombre.SelectAll();
                return false;
            }
            return true;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            int NumeroAlmacenSiguiente = Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("Almacenes") == -1
                ? 1 : Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("Almacenes") + 1;
            txtCodigo.Text = NumeroAlmacenSiguiente.ToString();
            TipoOperacion = "I";
            habilitarControles(true);
            limpiarControles();
            habilitarBotones(false, true, true, false, false);
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            eProviderAlmacenes.Clear();
            TipoOperacion = "E";
            habilitarControles(true);            
            habilitarBotones(false, true, true, false, false);
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                eProviderAlmacenes.Clear();
                if (validarDatos())
                {
                    int NumeroAlmacenAux = int.Parse(txtCodigo.Text);
                    if (TipoOperacion == "I")
                    {
                        TAAlmacenes.Insert(txtNombre.Text.Trim(), txtDescripcion.Text);
                        AlvecoComercial10DataSet.AlmacenesRow DRAlmacenNuevo = DTAlmacenes.AddAlmacenesRow(txtNombre.Text.Trim(), txtDescripcion.Text);
                        DTAlmacenes.AcceptChanges();
                        DRAlmacenNuevo.NumeroAlmacen = NumeroAlmacenAux;
                        DRAlmacenNuevo.AcceptChanges();
                        DTAlmacenes.AcceptChanges();

                        dtGVAlmacenes.CurrentCell = dtGVAlmacenes[0, DTAlmacenes.Rows.Count - 1];
                        dtGVAlmacenes.CurrentRow.Selected = true;
                    }

                    else
                    {
                        TAAlmacenes.ActualizarAlmacen(NumeroAlmacenAux, txtNombre.Text, txtDescripcion.Text);
                        int indiceEdicion = DTAlmacenes.Rows.IndexOf(DTAlmacenes.FindByNumeroAlmacen(int.Parse(txtCodigo.Text)));
                        DTAlmacenes[indiceEdicion].NombreAlmacen = txtNombre.Text;
                        DTAlmacenes[indiceEdicion].Descripcion = txtDescripcion.Text;
                        DTAlmacenes.AcceptChanges();
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
            eProviderAlmacenes.Clear();
            TipoOperacion = "";
            limpiarControles();
            habilitarControles(false);
            habilitarBotones(true, false, false, false, false);
            bdSourceAlmacenes_CurrentChanged(bdSourceAlmacenes, e);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "¿Se encuentra seguro de Eliminar el registro Actual??", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    limpiarControles();
                    habilitarBotones(true, false, false, false, false);
                    TAAlmacenes.Delete(int.Parse(txtCodigo.Text));
                    DTAlmacenes.Rows.Remove(DTAlmacenes.FindByNumeroAlmacen(int.Parse(txtCodigo.Text)));
                    DTAlmacenes.AcceptChanges();
                }
                catch (Exception)
                {
                    MessageBox.Show(this, "No se pudo culminar la operación actual, seguramente el registro se encuentra en uso en otras operaciones sel sistema", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dtGVAlmacenes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex >= 0 && DTAlmacenes.Count > 0)
            //{
            //    CargarDatosAlmacen(DTAlmacenes[e.RowIndex].NumeroAlmacen);
            //}
        }

        private void bdSourceAlmacenes_CurrentChanged(object sender, EventArgs e)
        {
            //if(dtGVAlmacenes.CurrentCell != null)
            //    dtGVAlmacenes_CellContentClick(dtGVAlmacenes, new DataGridViewCellEventArgs(0, bdSourceAlmacenes.Position));
            if(bdSourceAlmacenes.Position >= 0)
                CargarDatosAlmacen(DTAlmacenes[bdSourceAlmacenes.Position].NumeroAlmacen);
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
