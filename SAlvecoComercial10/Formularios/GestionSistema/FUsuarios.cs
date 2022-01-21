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

namespace SAlvecoComercial10.Formularios.GestionSistema
{
    public partial class FUsuarios : Form
    {
        UsuariosTableAdapter TAUsuarios;
        PersonasTableAdapter TAPersonas;
        ListarUsuariosPersonasTableAdapter TAUsuariosPersonas;
        AlmacenesTableAdapter TAlmacenes;

        AccesoDatos.AlvecoComercial10DataSet.ListarUsuariosPersonasDataTable DTUsuarios;
        string TipoOperacion = "";

        public FUsuarios()
        {
            InitializeComponent();
            TAUsuarios = new UsuariosTableAdapter();
            TAUsuarios.Connection = DAOUtilidades.conexion;
            
            
            TAUsuariosPersonas = new ListarUsuariosPersonasTableAdapter();
            TAUsuariosPersonas.Connection = DAOUtilidades.conexion;

            TAPersonas = new PersonasTableAdapter();
            TAPersonas.Connection = DAOUtilidades.conexion;

            TAlmacenes = new AlmacenesTableAdapter();
            TAlmacenes.Connection = DAOUtilidades.conexion;
            cBoxAlmacen.DataSource = TAlmacenes.GetData();
            cBoxAlmacen.DisplayMember = "NombreAlmacen";
            cBoxAlmacen.ValueMember = "NumeroAlmacen";
            cBoxAlmacen.SelectedIndex = -1;



            DTUsuarios = new AccesoDatos.AlvecoComercial10DataSet.ListarUsuariosPersonasDataTable();
            DTUsuarios = TAUsuariosPersonas.GetData(null);
            bdSourceUsuarios.DataSource = DTUsuarios;
            DTUsuarios.NombreCompletoColumn.ReadOnly = false;
            DTUsuarios.SexoCadenaColumn.ReadOnly = DTUsuarios.NombreEstadoCivilColumn.ReadOnly = DTUsuarios.EstadoCivilColumn.ReadOnly = false;

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

            ObjetoCodigoDescripcion listaTipoUsuario = new ObjetoCodigoDescripcion();
            listaTipoUsuario.cargarTiposUsuarios();
            cBoxTipoUsurio.DataSource = listaTipoUsuario.listaObjetos;
            cBoxTipoUsurio.ValueMember = listaTipoUsuario.ValueMember;
            cBoxTipoUsurio.DisplayMember = listaTipoUsuario.DisplayMember;
            


            CargarDatosPerona("");
            
            txtNombres.KeyPress += new KeyPressEventHandler(Utilidades.EventosValidacionTexto.TextBoxCaracteres_KeyPress);
            txtMaterno.KeyPress += new KeyPressEventHandler(Utilidades.EventosValidacionTexto.TextBoxCaracteres_KeyPress);
            txtPaterno.KeyPress += new KeyPressEventHandler(Utilidades.EventosValidacionTexto.TextBoxCaracteres_KeyPress);
            txtTelefono.KeyPress += new KeyPressEventHandler(Utilidades.EventosValidacionTexto.TextBoxEnteros_KeyPress);
            txtCelular.KeyPress += new KeyPressEventHandler(Utilidades.EventosValidacionTexto.TextBoxEnteros_KeyPress);

            this.StartPosition = FormStartPosition.CenterScreen;

        }

        public void configurarDatosUsuario(bool mostrarDatos)
        {
            lblLogin.Visible  = lblPassword.Visible = mostrarDatos;
            txtLogin.Visible = txtPassword.Visible = mostrarDatos;

        }

        public void limpiarCampos()
        {
            txtCelular.Text = String.Empty;
            txtCI.Text = String.Empty;
            txtDireccion.Text = String.Empty;
            txtEmail.Text = String.Empty;
            txtLogin.Text = String.Empty;
            txtMaterno.Text = String.Empty;
            txtNombres.Text = String.Empty;
            txtObservaciones.Text = String.Empty;
            txtPassword.Text = String.Empty;
            txtPaterno.Text = String.Empty;
            txtTelefono.Text = String.Empty;
            cBoxEstadoCivil.SelectedIndex = -1;
            cBoxSexo.SelectedIndex = -1;
            txtObservaciones.Text = String.Empty;
            cBoxAlmacen.SelectedIndex = cBoxTipoUsurio.SelectedIndex = -1;

        }

        public void habilitarCampos(bool estadoHabilitacion)
        {
            txtCelular.ReadOnly = !estadoHabilitacion;
            txtCI.ReadOnly = !estadoHabilitacion;
            txtDireccion.ReadOnly = !estadoHabilitacion;
            txtEmail.ReadOnly = !estadoHabilitacion;
            txtLogin.ReadOnly = !estadoHabilitacion;
            txtMaterno.ReadOnly = !estadoHabilitacion;
            txtNombres.ReadOnly = !estadoHabilitacion;
            txtObservaciones.ReadOnly = !estadoHabilitacion;
            txtPassword.ReadOnly = !estadoHabilitacion;
            txtPaterno.ReadOnly = !estadoHabilitacion;
            txtTelefono.ReadOnly = !estadoHabilitacion;
            cBoxEstadoCivil.Enabled = estadoHabilitacion;
            cBoxSexo.Enabled = estadoHabilitacion;
            dateFechaNacimiento.Enabled = estadoHabilitacion;
            txtObservaciones.ReadOnly = !estadoHabilitacion;
            cBoxTipoUsurio.Enabled = cBoxAlmacen.Enabled = estadoHabilitacion;
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
            AccesoDatos.AlvecoComercial10DataSet.ListarUsuariosPersonasRow DRPersonas = DTUsuarios.FindByDIPersona(DIPersona);
            if (DRPersonas == null)
            {
                AccesoDatos.AlvecoComercial10DataSet.ListarUsuariosPersonasDataTable DTUsuariosAux = TAUsuariosPersonas.GetData(DIPersona);
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
                txtLogin.Text = DRPersonas.NombreUsuario;
                txtMaterno.Text = DRPersonas.IsMaternoNull() ? String.Empty : DRPersonas.Materno;
                txtNombres.Text = DRPersonas.Nombres;
                txtObservaciones.Text = DRPersonas.IsObservacionesNull() ? String.Empty : DRPersonas.Observaciones;
                txtPassword.Text = DRPersonas["Contrasena"].ToString();
                txtPaterno.Text = DRPersonas.IsPaternoNull() ? String.Empty : DRPersonas.Paterno;
                txtTelefono.Text = DRPersonas.IsTelefonoDNull() ? String.Empty : DRPersonas.TelefonoD;

                dateFechaNacimiento.Value = DRPersonas.IsFechaNacimientoNull() ? DateTime.Now : DRPersonas.FechaNacimiento;
                cBoxEstadoCivil.SelectedValue = DRPersonas["EstadoCivil"];
                cBoxSexo.SelectedValue = DRPersonas.Sexo;

                if (DRPersonas.IsNumeroAlmacenNull())
                    cBoxAlmacen.SelectedIndex = -1;
                else
                    cBoxAlmacen.SelectedValue = DRPersonas.NumeroAlmacen;

                if (DRPersonas.IsCodigoTipoUsuarioNull())
                    cBoxTipoUsurio.SelectedIndex = -1;
                else
                    cBoxTipoUsurio.SelectedValue = DRPersonas.CodigoTipoUsuario;

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
            if (String.IsNullOrEmpty(txtLogin.Text.Trim()))
            {
                eProviderUsuarios.SetError(txtLogin, "Aún no ha ingresado el nombre de la Cuenta del usuario");
                txtLogin.Focus();
                txtLogin.SelectAll();
                return false;
            }
            if (String.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                eProviderUsuarios.SetError(txtPassword, "Aún no ha ingresado la contraseña del usuario");
                txtPassword.Focus();
                txtPassword.SelectAll();
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
            if (cBoxTipoUsurio.SelectedIndex < 0)
            {
                eProviderUsuarios.SetError(cBoxTipoUsurio, "Aún no ha seleccionado el Tipo de Usuario");
                cBoxTipoUsurio.Focus();
                cBoxTipoUsurio.SelectAll();
                return false;
            }
            if (cBoxAlmacen.SelectedIndex < 0)
            {
                eProviderUsuarios.SetError(cBoxAlmacen, "Aún no ha seleccionado el Almacen del Usuario");
                cBoxAlmacen.Focus();
                cBoxAlmacen.SelectAll();
                return false;
            }
            
            return true;
        }

        private void dtGVUsuarios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dtGVUsuarios.CurrentCell != null && e.RowIndex >= 0 && DTUsuarios.Count > 0)
            {
                string DIPersonaSeleccionada = dtGVUsuarios.CurrentRow.Cells["DGCDIPersona"].Value.ToString();
                CargarDatosPerona(DIPersonaSeleccionada);
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
            txtPassword.ReadOnly =  txtCI.ReadOnly =  true;
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


                    TAUsuariosPersonas.InsertarUsuario(txtCI.Text, txtPaterno.Text, txtMaterno.Text, txtNombres.Text, dateFechaNacimiento.Value,
                        cBoxSexo.SelectedValue.ToString(), cBoxEstadoCivil.SelectedValue.ToString(), txtCelular.Text, txtEmail.Text,
                        null, null, null, null, txtDireccion.Text, txtTelefono.Text, txtObservaciones.Text, txtLogin.Text, txtPassword.Text, null,
                        cBoxTipoUsurio.SelectedValue.ToString(), int.Parse(cBoxAlmacen.SelectedValue.ToString()));
                    

                    DTUsuarios.AddListarUsuariosPersonasRow( txtCI.Text, txtNombres.Text, txtPaterno.Text, txtMaterno.Text, dateFechaNacimiento.Value,
                        cBoxSexo.SelectedValue.ToString(), cBoxSexo.SelectedItem.ToString(), cBoxEstadoCivil.SelectedItem.ToString(), null, null, null, null,
                        txtCelular.Text, txtEmail.Text, txtDireccion.Text, txtTelefono.Text, txtObservaciones.Text,
                        string.Empty, string.Empty, string.Empty, string.Empty, txtLogin.Text, dateFechaNacimiento.Value,
                        txtNombres.Text + " " + txtPaterno.Text + " " + txtMaterno.Text, cBoxEstadoCivil.SelectedValue.ToString(), txtPassword.Text, null,
                        cBoxTipoUsurio.SelectedValue.ToString(), int.Parse(cBoxAlmacen.SelectedValue.ToString()));

                }
                else
                {
                    TAUsuariosPersonas.ActualizarUsuario(txtCI.Text, txtPaterno.Text, txtMaterno.Text, txtNombres.Text, dateFechaNacimiento.Value,
                        cBoxSexo.SelectedValue.ToString(), cBoxEstadoCivil.SelectedValue.ToString(), txtCelular.Text, txtEmail.Text,
                        null, null, null, null, txtDireccion.Text, txtTelefono.Text, txtObservaciones.Text, txtLogin.Text, txtPassword.Text, null,
                        cBoxTipoUsurio.SelectedValue.ToString(), int.Parse(cBoxAlmacen.SelectedValue.ToString()));

                    
                    AccesoDatos.AlvecoComercial10DataSet.ListarUsuariosPersonasRow DRUsuarios =
                        (AccesoDatos.AlvecoComercial10DataSet.ListarUsuariosPersonasRow)DTUsuarios.FindByDIPersona(txtCI.Text);
                    if (DRUsuarios == null)
                        DRUsuarios = (AccesoDatos.AlvecoComercial10DataSet.ListarUsuariosPersonasRow) DTUsuarios.Rows[dtGVUsuarios.CurrentRow.Index];

                    if (DRUsuarios != null)
                    {
                        DRUsuarios.DIPersona = txtCI.Text;
                        DRUsuarios.Paterno = txtPaterno.Text;
                        DRUsuarios.Materno = txtMaterno.Text;
                        DRUsuarios.NombreCompleto = txtNombres.Text + " " + txtPaterno.Text + " " + txtMaterno.Text;
                        DRUsuarios.Nombres = txtNombres.Text;
                        DRUsuarios.FechaNacimiento = dateFechaNacimiento.Value;
                        DRUsuarios.Sexo = cBoxSexo.SelectedValue.ToString();
                        DRUsuarios.SexoCadena = cBoxSexo.SelectedItem.ToString().Trim();
                        DRUsuarios.Celular = txtCelular.Text;
                        DRUsuarios.Email = txtEmail.Text;
                        DRUsuarios.DireccionD = txtDireccion.Text;
                        DRUsuarios.TelefonoD = txtTelefono.Text;
                        DRUsuarios.Observaciones = txtObservaciones.Text;
                        DRUsuarios.NombreUsuario = txtLogin.Text;
                        DRUsuarios.EstadoCivil = cBoxEstadoCivil.SelectedValue.ToString();
                        DRUsuarios.NombreEstadoCivil = cBoxEstadoCivil.SelectedItem.ToString();
                        DRUsuarios.NumeroAlmacen = int.Parse(cBoxAlmacen.SelectedValue.ToString());
                        DRUsuarios.CodigoTipoUsuario = cBoxTipoUsurio.SelectedValue.ToString();
                        //DRUsuarios.Contrasena = txtPassword.Text;

                        DRUsuarios.AcceptChanges();
                    }
                    //SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSet.PersonasRow DRPersonas;
                    //SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSet.PersonasDataTable DTPersonasAux = new AccesoDatos.AlvecoComercial10DataSet.PersonasDataTable();
                    //DRPersonas = DTPersonasAux.NewPersonasRow();
                    //DRPersonas.DIPersona = txtCI.Text;
                    //DRPersonas.Paterno = txtPaterno.Text;
                    //DRPersonas.Materno = txtMaterno.Text;
                    //DRPersonas.Nombres = txtNombres.Text;
                    //DRPersonas.FechaNacimiento = dateFechaNacimiento.Value;
                    //DRPersonas.Sexo = cBoxSexo.SelectedValue.ToString();
                    //DRPersonas.EstadoCivil = cBoxEstadoCivil.SelectedValue.ToString();
                    //DRPersonas.Celular = txtCelular.Text;
                    //DRPersonas.Email = txtEmail.Text;
                    //DRPersonas.DireccionD = txtDireccion.Text;
                    //DRPersonas.TelefonoD = txtTelefono.Text;
                    //DRPersonas.Observaciones = txtObservaciones.Text;

                    //DTPersonasAux.AddPersonasRow(DRPersonas);
                    //DTPersonasAux.AcceptChanges();
                    //TAPersonas.Update(DTPersonasAux);

                    //SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSet.UsuariosRow DRUsuarios;
                    //SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSet.UsuariosDataTable DTUsuariosAux = new AccesoDatos.AlvecoComercial10DataSet.UsuariosDataTable();
                    //DRUsuarios = DTUsuariosAux.NewUsuariosRow();
                    //DRUsuarios.CodigoUsuario = 1;
                    //DRUsuarios.DIUsuario = txtCI.Text;
                    //DRUsuarios.NombreUsuario = txtLogin.Text;
                    //DRUsuarios.Contrasena = txtPassword.Text;
                    //DRUsuarios.Observaciones = txtObservaciones.Text;

                    //DTUsuariosAux.AddUsuariosRow(DRUsuarios);
                    //TAUsuarios.Update(DTUsuariosAux);

                    
                }
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
                    TAUsuarios.EliminarUsuario(txtCI.Text);
                    DTUsuarios.Rows.Remove(DTUsuarios.FindByDIPersona(txtCI.Text));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, " Ocurrio la siguiente Excepción " + ex.Message +
                        ". Seguramente el Usuario ya realizo ciertas tareas en el Sistema, motivo por el cual no puede ser eliminardo.");
                }
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }


    }
}
