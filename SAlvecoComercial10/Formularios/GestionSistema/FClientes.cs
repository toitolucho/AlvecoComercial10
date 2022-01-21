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
    public partial class FClientes : Form
    {
        ClientesTableAdapter TAClientes;
        PaisesTableAdapter TAPaises;
        DepartamentosTableAdapter TADepartamentos;
        ProvinciasTableAdapter TAProvincias;
        LugaresTableAdapter TALugares;


        SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSet.ClientesDataTable DTClientes;
        SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSet.PaisesDataTable DTPaises;
        SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSet.DepartamentosDataTable DTDepartamentos;
        SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSet.ProvinciasDataTable DTProvincias;
        SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSet.LugaresDataTable DTLugares;

        ErrorProvider eProviderClientes;
        string TipoOperacion = "";
        Utilidades.ObjetoCodigoDescripcion TiposClientes = new Utilidades.ObjetoCodigoDescripcion();
        private bool soloInsertarEditar = false;
        public int CodigoCliente { get; set; }
        public FClientes()
        {
            InitializeComponent();

            TAClientes = new ClientesTableAdapter();
            TAClientes.Connection = Utilidades.DAOUtilidades.conexion;
            TAPaises = new PaisesTableAdapter();
            TAPaises.Connection = Utilidades.DAOUtilidades.conexion;
            TADepartamentos = new DepartamentosTableAdapter();
            TADepartamentos.Connection = Utilidades.DAOUtilidades.conexion;
            TAProvincias = new ProvinciasTableAdapter();
            TAProvincias.Connection = Utilidades.DAOUtilidades.conexion;
            TALugares = new LugaresTableAdapter();
            TALugares.Connection = Utilidades.DAOUtilidades.conexion;

            DTClientes = new AccesoDatos.AlvecoComercial10DataSet.ClientesDataTable();
            DTPaises = new AlvecoComercial10DataSet.PaisesDataTable();
            DTDepartamentos = new AlvecoComercial10DataSet.DepartamentosDataTable();
            DTProvincias = new AlvecoComercial10DataSet.ProvinciasDataTable();
            DTLugares = new AlvecoComercial10DataSet.LugaresDataTable();

            cargarProcedencia();

            eProviderClientes = new ErrorProvider();
            DTClientes = TAClientes.GetData();
            bdSourceClientes.DataSource = DTClientes;

            txtNombre.KeyPress += new KeyPressEventHandler(Utilidades.EventosValidacionTexto.TextBoxCaracteres_KeyPress);
            txtRepresentante.KeyPress += new KeyPressEventHandler(Utilidades.EventosValidacionTexto.TextBoxCaracteres_KeyPress);
            txtTelefono.KeyPress += new KeyPressEventHandler(Utilidades.EventosValidacionTexto.TextBoxEnteros_KeyPress);
            txtNombre.CharacterCasing = txtRepresentante.CharacterCasing = CharacterCasing.Upper;
            CargarDatosCliente(-1);
        }

        private void FClientes_Load(object sender, EventArgs e)
        {
            TiposClientes.cargarDatosTiposClientesProveedores();
            cBoxCliente.DataSource = TiposClientes.listaObjetos;
            cBoxCliente.DisplayMember = TiposClientes.DisplayMember;
            cBoxCliente.ValueMember = TiposClientes.ValueMember;
            cBoxCliente.SelectedIndex = -1;
        }

        public void cargarProcedencia()
        {
            DTPaises = TAPaises.GetData();
            cBoxPais.DataSource = DTPaises;
            cBoxPais.ValueMember = "CodigoPais";
            cBoxPais.DisplayMember = "NombrePais";
            cBoxPais.SelectedValue = "BO";

        }

        private void CargarPaises()
        {
            DTPaises = TAPaises.GetData();
            cBoxPais.DataSource = DTPaises.DefaultView;
            cBoxPais.DisplayMember = "NombrePais";
            cBoxPais.ValueMember = "CodigoPais";
            if (DTPaises.Count == 0)
            {
                if (DTDepartamentos != null)
                    DTDepartamentos.Clear();
                if (DTProvincias != null)
                    DTProvincias.Clear();
                if (DTLugares != null)
                    DTLugares.Clear();
            }
        }

        private void CargarDepartamentos(string CodigoPais)
        {
            DTDepartamentos = TADepartamentos.GetDataByPais(CodigoPais);
            cBoxDepartamento.DataSource = DTDepartamentos.DefaultView;
            cBoxDepartamento.DisplayMember = "NombreDepartamento";
            cBoxDepartamento.ValueMember = "CodigoDepartamento";
            if (DTDepartamentos.Count == 0)
            {
                if (DTProvincias != null)
                    DTProvincias.Clear();
                if (DTLugares != null)
                    DTLugares.Clear();
            }
        }

        private void CargarProvincias(string CodigoPais, string CodigoDepartamento)
        {
            DTProvincias = TAProvincias.GetDataByDepartamento(CodigoPais, CodigoDepartamento);
            cBoxProvincia.DataSource = DTProvincias.DefaultView;
            cBoxProvincia.DisplayMember = "NombreProvincia";
            cBoxProvincia.ValueMember = "CodigoProvincia";
            if (DTProvincias.Count == 0)
            {
                if (DTLugares != null)
                    DTLugares.Clear();
            }
        }

        private void CargarLugares(string CodigoPais, string CodigoDepartamento, string CodigoProvincia)
        {
            DTLugares = TALugares.GetDataByProvincia(CodigoPais, CodigoDepartamento, CodigoProvincia);
            cBoxLugar.DataSource = DTLugares.DefaultView;
            cBoxLugar.DisplayMember = "NombreLugar";
            cBoxLugar.ValueMember = "CodigoLugar";
        }


        public void CargarDatosCliente(int CodigoCliente)
        {

            SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSet.ClientesRow DRCliente = DTClientes.FindByCodigoCliente(CodigoCliente);
            if (DRCliente == null)
            {
                AlvecoComercial10DataSet.ClientesDataTable DTClientesAux = TAClientes.GetDataBy1(CodigoCliente);
                if (DTClientesAux.Count > 0)
                    DRCliente = DTClientesAux[0];
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
                txtCodigo.Text = DRCliente.CodigoCliente.ToString();
                txtNombre.Text = DRCliente.NombreCliente;
                txtRepresentante.Text = DRCliente.NombreRepresentante;
                cBoxCliente.SelectedValue = DRCliente.CodigoTipoCliente;
                txtNIT.Text = DRCliente.IsNITClienteNull() ? String.Empty : DRCliente.NITCliente;

                if (!DRCliente.IsCodigoPaisNull())
                    cBoxPais.SelectedValue = DRCliente.CodigoPais;
                else
                    cBoxPais.SelectedIndex = -1;

                if (!DRCliente.IsCodigoDepartamentoNull())
                    cBoxDepartamento.SelectedValue = DRCliente.CodigoDepartamento;
                else
                    cBoxDepartamento.SelectedIndex = -1;
                if (!DRCliente.IsCodigoProvinciaNull())
                    cBoxProvincia.SelectedValue = DRCliente.CodigoProvincia;
                else
                    cBoxProvincia.SelectedIndex = -1;
                if (!DRCliente.IsCodigoLugarNull())
                {
                    CargarLugares(DRCliente.CodigoPais, DRCliente.CodigoDepartamento, DRCliente.CodigoProvincia);
                    cBoxLugar.SelectedValue = DRCliente.CodigoLugar;
                }
                else
                    cBoxLugar.SelectedIndex = -1;

                //cBoxPais.SelectedValue = DRCliente.IsCodigoPaisNull() ? null : DRCliente.CodigoPais;
                //cBoxDepartamento.SelectedValue = DRCliente.IsCodigoDepartamentoNull() ? null : DRCliente.CodigoDepartamento;
                //cBoxProvincia.SelectedValue = DRCliente.IsCodigoProvinciaNull() ? null : DRCliente.CodigoProvincia;
                //cBoxLugar.SelectedValue = DRCliente.IsCodigoLugarNull() ? null : DRCliente.CodigoLugar;
                txtDireccion.Text = DRCliente.IsDireccionNull() ? String.Empty : DRCliente.Direccion;
                txtTelefono.Text = DRCliente.IsTelefonoNull() ? String.Empty : DRCliente.Telefono;
                txtEmail.Text = DRCliente.IsEmailNull() ? String.Empty : DRCliente.Email;
                txtDescripcion.Text = DRCliente.IsObservacionesNull() ? String.Empty : DRCliente.Observaciones;
                this.CodigoCliente = CodigoCliente;

                habilitarBotones(true, false, false, true, true);
                habilitarControles(false);
            }
        }

        public void habilitarControles(bool estadoHabilitacion)
        {
            txtNombre.ReadOnly = !estadoHabilitacion;
            txtRepresentante.ReadOnly = !estadoHabilitacion;
            cBoxCliente.Enabled = estadoHabilitacion;
            txtNIT.ReadOnly = !estadoHabilitacion;
            cBoxPais.Enabled = estadoHabilitacion;
            cBoxDepartamento.Enabled = estadoHabilitacion;
            cBoxProvincia.Enabled = estadoHabilitacion;
            cBoxLugar.Enabled = estadoHabilitacion;
            txtTelefono.ReadOnly = !estadoHabilitacion;
            txtDireccion.ReadOnly = !estadoHabilitacion;
            txtEmail.ReadOnly = !estadoHabilitacion;            
            txtDescripcion.ReadOnly = !estadoHabilitacion;

            btnAgregarComunidad.Enabled = estadoHabilitacion;
            btnAgregarDepartamento.Enabled = estadoHabilitacion;
            btnAgregarPais.Enabled = estadoHabilitacion;
            btnAgregarProvincia.Enabled = estadoHabilitacion;
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
            txtRepresentante.Text = String.Empty;
            cBoxCliente.SelectedIndex = -1;
            txtNIT.Text = String.Empty;
            cBoxPais.SelectedIndex = -1;
            cBoxDepartamento.SelectedIndex = -1;
            cBoxProvincia.SelectedIndex = -1;
            cBoxLugar.SelectedIndex = -1;
            txtTelefono.Text = String.Empty;
            txtDireccion.Text = String.Empty;
            txtEmail.Text = String.Empty;
            txtDescripcion.Text = string.Empty;
            txtCodigo.Text = String.Empty;
            eProviderClientes.Clear();
        }

        public bool validarDatos()
        {
            if (String.IsNullOrEmpty(txtNombre.Text.Trim()))
            {
                eProviderClientes.SetError(txtNombre, "Aún no ha ingresado el Nombre del Cliente");
                txtNombre.Focus();
                txtNombre.SelectAll();
                return false;
            }
            if (String.IsNullOrEmpty(txtRepresentante.Text.Trim()))
            {
                eProviderClientes.SetError(txtRepresentante, "Aún no ha ingresado el Nombre del Representante del cliente");
                txtRepresentante.Focus();
                txtRepresentante.SelectAll();
                return false;
            }
            if (cBoxCliente.SelectedIndex < 0)
            {
                eProviderClientes.SetError(cBoxCliente, "Aún no ha seleccionado el tipo del Cliente");
                cBoxCliente.Focus();
                cBoxCliente.SelectAll();
                return false;
            }
            return true;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            limpiarControles();
            int CodigoClienteSiguiente = Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("Clientes") == -1
                ? 1 : Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("Clientes") + 1;
            txtCodigo.Text = CodigoClienteSiguiente.ToString();
            TipoOperacion = "I";
            habilitarControles(true);            
            cBoxPais.SelectedValue = "BO";
            cBoxDepartamento.SelectedValue = "06";
            cBoxProvincia.SelectedValue = "03";
            habilitarBotones(false, true, true, false, false);
            txtNombre.Focus();
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
                eProviderClientes.Clear();
                if (validarDatos())
                {
                    if (TipoOperacion == "I")
                    {
                        int Codigocliente = int.Parse(txtCodigo.Text);
                        TAClientes.Insert(null, txtNombre.Text, txtRepresentante.Text, cBoxCliente.SelectedValue.ToString(), txtNIT.Text,
                            cBoxPais.SelectedIndex >= 0 ? cBoxPais.SelectedValue.ToString() : null, cBoxDepartamento.SelectedIndex >= 0 ? cBoxDepartamento.SelectedValue.ToString() : null,
                            cBoxProvincia.SelectedIndex >= 0 ? cBoxProvincia.SelectedValue.ToString() : null, 
                            cBoxLugar.SelectedIndex >= 0 ? cBoxLugar.SelectedValue.ToString() : null, txtDireccion.Text,
                            txtTelefono.Text, txtEmail.Text, txtDescripcion.Text, "H");
                        AlvecoComercial10DataSet.ClientesRow DRAlmacenNuevo = DTClientes.AddClientesRow(txtNombre.Text, txtRepresentante.Text, cBoxCliente.SelectedValue.ToString(), txtNIT.Text,
                            cBoxPais.SelectedIndex >= 0 ? cBoxPais.SelectedValue.ToString() : null, cBoxDepartamento.SelectedIndex >= 0 ? cBoxDepartamento.SelectedValue.ToString() : null,
                            cBoxProvincia.SelectedIndex >= 0 ? cBoxProvincia.SelectedValue.ToString() : null,
                            cBoxLugar.SelectedIndex >= 0 ? cBoxLugar.SelectedValue.ToString() : null, txtDireccion.Text,
                            txtTelefono.Text, txtEmail.Text, txtDescripcion.Text, "H");

                        DRAlmacenNuevo.CodigoCliente = Codigocliente;
                        DRAlmacenNuevo.AcceptChanges();
                        DTClientes.AcceptChanges();

                        this.CodigoCliente = Codigocliente;
                        
                        //DTClientes.AcceptChanges();
                    }

                    else
                    {
                        TAClientes.ActualizarCliente(int.Parse(txtCodigo.Text), txtNombre.Text, txtRepresentante.Text, cBoxCliente.SelectedValue.ToString(), txtNIT.Text,
                            cBoxPais.SelectedIndex >= 0 ? cBoxPais.SelectedValue.ToString() : null, cBoxDepartamento.SelectedIndex >= 0 ? cBoxDepartamento.SelectedValue.ToString() : null,
                            cBoxProvincia.SelectedIndex >= 0 ? cBoxProvincia.SelectedValue.ToString() : null,
                            cBoxLugar.SelectedIndex >= 0 ? cBoxLugar.SelectedValue.ToString() : null, txtDireccion.Text,
                            txtTelefono.Text, txtEmail.Text, txtDescripcion.Text, "H");
                        int indiceEdicion = DTClientes.Rows.IndexOf(DTClientes.FindByCodigoCliente(int.Parse(txtCodigo.Text)));

                        DTClientes[indiceEdicion].NombreCliente= txtNombre.Text;
                        DTClientes[indiceEdicion].NombreRepresentante = txtRepresentante.Text;
                        DTClientes[indiceEdicion].NITCliente = txtNIT.Text;
                        DTClientes[indiceEdicion].CodigoTipoCliente = cBoxCliente.SelectedValue.ToString();
                        DTClientes[indiceEdicion].CodigoPais = cBoxPais.SelectedIndex >= 0 ? cBoxPais.SelectedValue.ToString() : String.Empty;
                        DTClientes[indiceEdicion].CodigoDepartamento = cBoxDepartamento.SelectedIndex >= 0 ? cBoxDepartamento.SelectedValue.ToString() : String.Empty;
                        DTClientes[indiceEdicion].CodigoProvincia = cBoxProvincia.SelectedIndex >= 0 ? cBoxProvincia.SelectedValue.ToString() : String.Empty;
                        DTClientes[indiceEdicion].CodigoLugar = cBoxLugar.SelectedIndex >= 0 ? cBoxLugar.SelectedValue.ToString() : String.Empty;
                        DTClientes[indiceEdicion].Observaciones = txtDescripcion.Text;
                        DTClientes[indiceEdicion].Direccion = txtDireccion.Text;
                        DTClientes[indiceEdicion].Telefono = txtTelefono.Text;
                        DTClientes[indiceEdicion].Email = txtEmail.Text;
                        DTClientes[indiceEdicion].Observaciones = txtDescripcion.Text;
                        DTClientes.AcceptChanges();
                    }
                    habilitarBotones(true, false, false, true, true);
                    habilitarControles(false);
                    TipoOperacion = "";
                    //MessageBox.Show(this, "Operación realizada correctamente", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (soloInsertarEditar)
                    {
                        DialogResult = System.Windows.Forms.DialogResult.OK;
                        bdSourceClientes.MoveLast();
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
            eProviderClientes.Clear();
            TipoOperacion = "";
            limpiarControles();
            habilitarControles(false);
            habilitarBotones(true, false, false, false, false);
            bdSourceClientes_CurrentChanged(bdSourceClientes, e);
            if (soloInsertarEditar)
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "¿Se encuentra seguro de Eliminar el registro Actual??", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    
                    habilitarBotones(true, false, false, false, false);
                    TAClientes.Delete(int.Parse(txtCodigo.Text));
                    DTClientes.Rows.Remove(DTClientes.FindByCodigoCliente(int.Parse(txtCodigo.Text)));
                    DTClientes.AcceptChanges();
                    limpiarControles();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "No se pudo culminar la operación actual, seguramente el registro se encuentra en uso en otras operaciones sel sistema", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dtGVClientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex >= 0 && DTClientes.Count > 0)
            //{
            //    CargarDatosAlmacen(DTClientes[e.RowIndex].CodigoCliente);
            //}
        }

        private void bdSourceClientes_CurrentChanged(object sender, EventArgs e)
        {
            //if(dtGVClientes.CurrentCell != null)
            //    dtGVClientes_CellContentClick(dtGVClientes, new DataGridViewCellEventArgs(0, bdSourceClientes.Position));
            if (bdSourceClientes.Position >= 0)
            {
                CodigoCliente = DTClientes[bdSourceClientes.Position].CodigoCliente;
                CargarDatosCliente(DTClientes[bdSourceClientes.Position].CodigoCliente);

                if (soloInsertarEditar && (DialogResult == System.Windows.Forms.DialogResult.OK || DialogResult == System.Windows.Forms.DialogResult.Cancel))
                    this.Close();
            }
        }

        private void cBoxPais_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cBoxPais.SelectedIndex >= 0)
                CargarDepartamentos(cBoxPais.SelectedValue.ToString());
        }

        private void cBoxDepartamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cBoxDepartamento.SelectedIndex >= 0)
                CargarProvincias(cBoxPais.SelectedValue.ToString(), cBoxDepartamento.SelectedValue.ToString());
        }

        private void cBoxProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cBoxProvincia.SelectedIndex >= 0)
                CargarLugares(cBoxPais.SelectedValue.ToString(), cBoxDepartamento.SelectedValue.ToString(), cBoxProvincia.SelectedValue.ToString());
        }

        private void btnAgregarPais_Click(object sender, EventArgs e)
        {
            FGeografico fgeografico = new FGeografico(Utilidades.DAOUtilidades.conexion);
            fgeografico.seleccionarPestaniaPais();
            fgeografico.ShowDialog(this);
            CargarPaises();
            if(!String.IsNullOrEmpty(fgeografico.CodigoPais))
            {
                cBoxPais.SelectedValue = fgeografico.CodigoPais;
            }
            fgeografico.Dispose();
            
        }

        private void btnAgregarDepartamento_Click(object sender, EventArgs e)
        {
            FGeografico fgeografico = new FGeografico(Utilidades.DAOUtilidades.conexion);
            fgeografico.cargarAutomaticamente = false;
            fgeografico.seleccionarPestaniaDepartamento();
            fgeografico.CargarPaises();
            fgeografico.seleccionarPais(cBoxPais.SelectedValue.ToString());
            fgeografico.ShowDialog(this);
            CargarDepartamentos(cBoxPais.SelectedValue.ToString());
            if (!String.IsNullOrEmpty(fgeografico.CodigoDepartamento))
                cBoxDepartamento.SelectedValue = fgeografico.CodigoDepartamento;
        }

        private void btnAgregarProvincia_Click(object sender, EventArgs e)
        {
            FGeografico fgeografico = new FGeografico(Utilidades.DAOUtilidades.conexion);
            fgeografico.cargarAutomaticamente = false;
            fgeografico.seleccionarPestaniaProvincia();
            fgeografico.CargarPaisesP();
            fgeografico.CargarDepartamentos(cBoxPais.SelectedValue.ToString());
            fgeografico.seleccionarPaisDepartamento(cBoxPais.SelectedValue.ToString(), cBoxDepartamento.SelectedValue != null ? cBoxDepartamento.SelectedValue.ToString() : null);
            fgeografico.ShowDialog(this);
            CargarProvincias(cBoxPais.SelectedValue.ToString(), cBoxDepartamento.SelectedValue != null ? cBoxDepartamento.SelectedValue.ToString() : null);
            if (!String.IsNullOrEmpty(fgeografico.CodigoProvincia))
                cBoxProvincia.SelectedValue = fgeografico.CodigoProvincia;
        }

        private void btnAgregarComunidad_Click(object sender, EventArgs e)
        {
            FGeografico fgeografico = new FGeografico(Utilidades.DAOUtilidades.conexion);
            fgeografico.cargarAutomaticamente = false;
            fgeografico.seleccionarPestaniaLugar();
            fgeografico.CargarPaisesL();
            fgeografico.CargarDepartamentosL(cBoxPais.SelectedValue.ToString());
            fgeografico.CargarProvincias(cBoxPais.SelectedValue.ToString(), cBoxDepartamento.SelectedValue != null ? cBoxDepartamento.SelectedValue.ToString() : null);
            fgeografico.seleccionarPaisDepartamentoProvincia(cBoxPais.SelectedValue.ToString(), cBoxDepartamento.SelectedValue != null ? cBoxDepartamento.SelectedValue.ToString() : "", cBoxProvincia.SelectedValue != null ? cBoxProvincia.SelectedValue.ToString() : null);
            fgeografico.ShowDialog(this);
            CargarLugares(cBoxPais.SelectedValue.ToString(), cBoxDepartamento.SelectedValue != null ? cBoxDepartamento.SelectedValue.ToString() : null, cBoxProvincia.SelectedValue != null ? cBoxProvincia.SelectedValue.ToString() : null);
            if (!String.IsNullOrEmpty(fgeografico.CodigoLugar))
                cBoxLugar.SelectedValue = fgeografico.CodigoLugar;
        }

        public void configurarFormularioIA(int CodigoCliente)
        {

            CargarDatosCliente(CodigoCliente);
            TipoOperacion = CodigoCliente < 0 ? "I" : "E";
            if (CodigoCliente < 0)
            {
                int NumeroProveedorSiguiente = Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("Clientes") == -1
                ? 1 : Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("Clientes") + 1;
                txtCodigo.Text = NumeroProveedorSiguiente.ToString();
            }
            soloInsertarEditar = true;
            gBoxGrilla.Visible = false;
            //290
            btnEditar.Visible = btnNuevo.Visible = btnEliminar.Visible = false;
            this.Size = new Size(gBoxDatos.Size.Width + 20, this.Size.Height);
            habilitarControles(true);
            habilitarBotones(false, true, true, false, false);
            cBoxPais.SelectedValue = "BO";
            cBoxDepartamento.SelectedValue = "06";
            cBoxProvincia.SelectedValue = "03";
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
