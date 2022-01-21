using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSetTableAdapters;
using SAlvecoComercial10.Formularios.Utilidades;
using SAlvecoComercial10.Formularios.Reportes;

namespace SAlvecoComercial10.Formularios.GestionSistema
{
    public partial class FPersonas : Form
    {
        
        PersonasTableAdapter TAPersonas;
        AccesoDatos.AlvecoComercial10DataSet.PersonasDataTable DTPersonas;        
        string TipoOperacion = "";
        public string DIPersona{get;set;}
        public FPersonas()
        {
            InitializeComponent();
            
            TAPersonas = new PersonasTableAdapter();
            TAPersonas.Connection = DAOUtilidades.conexion;

            DTPersonas = new AccesoDatos.AlvecoComercial10DataSet.PersonasDataTable();
            DTPersonas = TAPersonas.GetData(null);
            bdSourceUsuarios.DataSource = DTPersonas;
            DTPersonas.NombreCompletoColumn.ReadOnly = false;
            DTPersonas.SexoCadenaColumn.ReadOnly = DTPersonas.NombreEstadoCivilColumn.ReadOnly = DTPersonas.EstadoCivilColumn.ReadOnly = false;

            ObjetoCodigoDescripcion listaSexo = new ObjetoCodigoDescripcion();
            listaSexo.cargarDatosSexo();
            cBoxSexo.DataSource = listaSexo.listaObjetos;
            cBoxSexo.ValueMember = listaSexo.ValueMember;
            cBoxSexo.DisplayMember = listaSexo.DisplayMember;
            

            ObjetoCodigoDescripcion listaEstadpCivil = new ObjetoCodigoDescripcion();
            listaEstadpCivil.cargarDatosEstadoCivil();
            cBoxEstadoCivil.DataSource = listaEstadpCivil.listaObjetos;
            cBoxEstadoCivil.ValueMember = listaEstadpCivil.ValueMember;
            cBoxEstadoCivil.DisplayMember = listaEstadpCivil.DisplayMember;
            
            
            CargarDatosPerona("");
            
            txtNombres.KeyPress += new KeyPressEventHandler(Utilidades.EventosValidacionTexto.TextBoxCaracteres_KeyPress);
            txtMaterno.KeyPress += new KeyPressEventHandler(Utilidades.EventosValidacionTexto.TextBoxCaracteres_KeyPress);
            txtPaterno.KeyPress += new KeyPressEventHandler(Utilidades.EventosValidacionTexto.TextBoxCaracteres_KeyPress);
            txtTelefono.KeyPress += new KeyPressEventHandler(Utilidades.EventosValidacionTexto.TextBoxEnteros_KeyPress);
            txtCelular.KeyPress += new KeyPressEventHandler(Utilidades.EventosValidacionTexto.TextBoxEnteros_KeyPress);

            this.StartPosition = FormStartPosition.CenterScreen;

        }


        public void limpiarCampos()
        {
            txtCelular.Text = String.Empty;
            txtCI.Text = String.Empty;
            txtDireccion.Text = String.Empty;
            txtEmail.Text = String.Empty;
            txtMaterno.Text = String.Empty;
            txtNombres.Text = String.Empty;
            txtPaterno.Text = String.Empty;
            txtTelefono.Text = String.Empty;
            txtCargo.Text = String.Empty;
            txtObservaciones.Text = String.Empty;
            cBoxEstadoCivil.SelectedIndex = -1;
            cBoxSexo.SelectedIndex = -1;


        }

        public void habilitarCampos(bool estadoHabilitacion)
        {
            txtCelular.ReadOnly = !estadoHabilitacion;
            txtCI.ReadOnly = !estadoHabilitacion;
            txtDireccion.ReadOnly = !estadoHabilitacion;
            txtEmail.ReadOnly = !estadoHabilitacion;
            txtMaterno.ReadOnly = !estadoHabilitacion;
            txtNombres.ReadOnly = !estadoHabilitacion;
            txtPaterno.ReadOnly = !estadoHabilitacion;
            txtTelefono.ReadOnly = !estadoHabilitacion;
            txtCargo.ReadOnly = !estadoHabilitacion;
            txtObservaciones.ReadOnly = !estadoHabilitacion;
            cBoxEstadoCivil.Enabled = estadoHabilitacion;
            cBoxSexo.Enabled = estadoHabilitacion;
            dateFechaNacimiento.Enabled = estadoHabilitacion;

        }

        public void habilitarBotones(bool nuevo, bool cancelar, bool editar, bool aceptar, bool eliminar)
        {
            btnNuevo.Enabled = nuevo;
            btnCancelar.Enabled = cancelar;
            btnEditar.Enabled = editar;
            btnAceptar.Enabled = aceptar;
            btnEliminar.Enabled = eliminar;
        }

        public void CargarDatosPerona(string DIPersona)
        {
            AccesoDatos.AlvecoComercial10DataSet.PersonasRow DRPersonas = DTPersonas.FindByDIPersona(DIPersona);
            if (DRPersonas == null)
            {
                AccesoDatos.AlvecoComercial10DataSet.PersonasDataTable DTUsuariosAux = TAPersonas.GetData(DIPersona);
                if (DTUsuariosAux.Count > 0)
                    DRPersonas = DTUsuariosAux[0];
                else
                {
                    limpiarCampos();
                    habilitarCampos(false);
                    habilitarBotones(true, false, false, false, false);
                    tabControl1.SelectedTab = tPageDatos;
                    return;
                }
            }
            else
            {
                txtCelular.Text = DRPersonas.IsCelularNull() ? String.Empty : DRPersonas.Celular;
                txtCI.Text = DRPersonas.DIPersona;
                txtDireccion.Text = DRPersonas.IsDireccionDNull() ? String.Empty : DRPersonas.DireccionD;
                txtEmail.Text = DRPersonas.IsEmailNull() ? String.Empty : DRPersonas.Email;
                txtMaterno.Text = DRPersonas.IsMaternoNull() ? String.Empty : DRPersonas.Materno;
                txtNombres.Text = DRPersonas.Nombres;
                txtPaterno.Text = DRPersonas.IsPaternoNull() ? String.Empty : DRPersonas.Paterno;
                txtTelefono.Text = DRPersonas.IsTelefonoDNull() ? String.Empty : DRPersonas.TelefonoD;
                txtCargo.Text = DRPersonas.IsNombreCargoNull() ? "" : DRPersonas.NombreCargo;
                dateFechaNacimiento.Value = DRPersonas.IsFechaNacimientoNull() ? DateTime.Now : DRPersonas.FechaNacimiento;
                cBoxEstadoCivil.SelectedValue = DRPersonas["EstadoCivil"];
                cBoxSexo.SelectedValue = DRPersonas.Sexo;

                habilitarBotones(true, false, true, false, true);
                habilitarCampos(false);
            }

        }

        public bool estanLosDatosCorrectos()
        {
            if (String.IsNullOrEmpty(txtCI.Text.Trim()))
            {
                eProviderUsuarios.SetError(txtCI, "Aún no ha ingresado el CI del usuario");
                txtCI.Focus();
                txtCI.SelectAll();
                return false;
            }
            if (String.IsNullOrEmpty(txtNombres.Text.Trim()))
            {
                eProviderUsuarios.SetError(txtNombres, "Aún no ha ingresado el Nombre del usuario");
                txtNombres.Focus();
                txtNombres.SelectAll();
                return false;
            }
            if (cBoxEstadoCivil.SelectedIndex < 0)
            {
                eProviderUsuarios.SetError(cBoxEstadoCivil, "Aún no ha seleccionado el Estado Civil del Usuario");
                cBoxEstadoCivil.Focus();
                cBoxEstadoCivil.SelectAll();
                return false;
            }
            if (cBoxSexo.SelectedIndex < 0)
            {
                eProviderUsuarios.SetError(cBoxSexo, "Aún no ha seleccionado el Sexo del Usuario");
                cBoxSexo.Focus();
                cBoxSexo.SelectAll();
                return false;
            }
            return true;
        }

        private void dtGVUsuarios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dtGVUsuarios.CurrentCell != null && e.RowIndex >= 0 && DTPersonas.Count > 0)
            {
                CargarDatosPerona(DTPersonas[e.RowIndex].DIPersona);
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tPageDatos;
            limpiarCampos();
            habilitarCampos(true);
            habilitarBotones(false, true, false, true, false);
            TipoOperacion = "N";
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            habilitarCampos(true);
            habilitarBotones(false, true, false, true, false);
            TipoOperacion = "E";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            eProviderUsuarios.Clear();
            limpiarCampos();
            habilitarCampos(false);
            habilitarBotones(true, false, false, false, false);
            TipoOperacion = "";
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            eProviderUsuarios.Clear();
            if (!estanLosDatosCorrectos())
            {
                MessageBox.Show(this, "Existen algunos campos erróneos", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (TipoOperacion == "N")
                {
                    //TAPersonas.Insert(txtCI.Text, txtPaterno.Text, txtMaterno.Text, txtNombres.Text, dateFechaNacimiento.Value,
                    //    cBoxSexo.SelectedValue.ToString(), cBoxEstadoCivil.SelectedValue.ToString(), txtCelular.Text, txtEmail.Text,
                    //    null, null, null, null, txtDireccion.Text, txtTelefono.Text, txtObservaciones.Text);
                    //TAUsuarios.Insert(txtLogin.Text, txtPassword.Text, txtCI.Text, null, txtObservaciones.Text);


                    TAPersonas.InsertarPersona(txtCI.Text, txtPaterno.Text, txtMaterno.Text, txtNombres.Text, dateFechaNacimiento.Value,
                        cBoxSexo.SelectedValue.ToString(), cBoxEstadoCivil.SelectedValue.ToString(), txtCargo.Text,  txtCelular.Text, txtEmail.Text,
                        null, null, null, null, txtDireccion.Text, txtTelefono.Text, txtObservaciones.Text);


                    DTPersonas.AddPersonasRow(txtCI.Text, txtPaterno.Text, txtMaterno.Text, txtNombres.Text, dateFechaNacimiento.Value,
                        cBoxSexo.SelectedValue.ToString(), cBoxEstadoCivil.SelectedValue.ToString(), 
                        txtCelular.Text, txtEmail.Text, null,null, null, null, txtDireccion.Text, txtTelefono.Text, txtObservaciones.Text,
                        txtNombres.Text + " "+txtPaterno.Text +" "+ txtMaterno.Text,
                        cBoxSexo.SelectedItem.ToString(), cBoxEstadoCivil.SelectedItem.ToString(), null, null,null, null, txtCargo.Text);
                       

                }
                else
                {
                    TAPersonas.ActualizarPersona(txtCI.Text, txtPaterno.Text, txtMaterno.Text, txtNombres.Text, dateFechaNacimiento.Value,
                        cBoxSexo.SelectedValue.ToString(), cBoxEstadoCivil.SelectedValue.ToString(), txtCargo.Text, txtCelular.Text, txtEmail.Text,
                        null, null, null, null, txtDireccion.Text, txtTelefono.Text, txtObservaciones.Text);

                    
                    AccesoDatos.AlvecoComercial10DataSet.PersonasRow DRUsuarios =
                        (AccesoDatos.AlvecoComercial10DataSet.PersonasRow)DTPersonas.FindByDIPersona(txtCI.Text);

                    DRUsuarios.DIPersona = txtCI.Text;
                    DRUsuarios.Paterno = txtPaterno.Text;
                    DRUsuarios.Materno = txtMaterno.Text;
                    DRUsuarios.NombreCompleto = txtNombres.Text +" " + txtPaterno.Text + " " +txtMaterno.Text;
                    DRUsuarios.Nombres = txtNombres.Text;
                    DRUsuarios.FechaNacimiento = dateFechaNacimiento.Value;
                    DRUsuarios.Sexo = cBoxSexo.SelectedValue.ToString();
                    DRUsuarios.SexoCadena = cBoxSexo.SelectedItem.ToString().Trim();
                    DRUsuarios.Celular = txtCelular.Text;
                    DRUsuarios.Email = txtEmail.Text;
                    DRUsuarios.DireccionD = txtDireccion.Text;
                    DRUsuarios.TelefonoD = txtTelefono.Text;
                    DRUsuarios.Observaciones = txtObservaciones.Text;
                    DRUsuarios.EstadoCivil = cBoxEstadoCivil.SelectedValue.ToString();
                    DRUsuarios.NombreEstadoCivil = cBoxEstadoCivil.SelectedItem.ToString();
                    DRUsuarios.NombreCargo = txtCargo.Text;
                    
                    DRUsuarios.AcceptChanges();
                }
                DIPersona = txtCI.Text;
                TipoOperacion = "";
                CargarDatosPerona(txtCI.Text);
                habilitarBotones(true, false, true, false, true);
                habilitarCampos(false);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "No se pudo realizar correctamente la operación actual, ocurrió la siguiente excepción " + ex.Message,
                    this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dtGVUsuarios_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            tabControl1.SelectedTab = tPageDatos;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "¿Se encuentra seguro de Eliminar el Registro Actual?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    TAPersonas.Delete(txtCI.Text);
                    DTPersonas.Rows.Remove(DTPersonas.FindByDIPersona(txtCI.Text));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Ocurrio la siguiente Excepción " + ex.Message +
                        ". Seguramente el Usuario ya realizo ciertas tareas en el Sistema, motivo por el cual no puede ser eliminardo.");
                }
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnReporte_Click(object sender, EventArgs e)
        {
            
            
            FReporteCuentas formReportes = new FReporteCuentas();
            formReportes.ListarUsuariosPersonas(TAPersonas.GetData(null));
            formReportes.ShowDialog();
        }


    }
}
