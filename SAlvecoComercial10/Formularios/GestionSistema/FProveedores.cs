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
    public partial class FProveedores : Form
    {
        ProveedoresTableAdapter TAProveedores;
        PaisesTableAdapter TAPaises;
        DepartamentosTableAdapter TADepartamentos;
        ProvinciasTableAdapter TAProvincias;
        LugaresTableAdapter TALugares;


        SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSet.ProveedoresDataTable DTProveedores;
        SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSet.PaisesDataTable DTPaises;
        SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSet.DepartamentosDataTable DTDepartamentos;
        SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSet.ProvinciasDataTable DTProvincias;
        SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSet.LugaresDataTable DTLugares;

        ErrorProvider eProviderProveedores;
        string TipoOperacion = "";
        Utilidades.ObjetoCodigoDescripcion TiposProveedores = new Utilidades.ObjetoCodigoDescripcion();

        public int CodigoProveedor { get; set; }
        private bool soloInsertarEditar = false;
        public FProveedores()
        {
            InitializeComponent();

            TAProveedores = new ProveedoresTableAdapter();
            TAProveedores.Connection = Utilidades.DAOUtilidades.conexion;
            TAPaises = new PaisesTableAdapter();
            TAPaises.Connection = Utilidades.DAOUtilidades.conexion;
            TADepartamentos = new DepartamentosTableAdapter();
            TADepartamentos.Connection = Utilidades.DAOUtilidades.conexion;
            TAProvincias = new ProvinciasTableAdapter();
            TAProvincias.Connection = Utilidades.DAOUtilidades.conexion;
            TALugares = new LugaresTableAdapter();
            TALugares.Connection = Utilidades.DAOUtilidades.conexion;

            DTProveedores = new AccesoDatos.AlvecoComercial10DataSet.ProveedoresDataTable();
            DTPaises = new AlvecoComercial10DataSet.PaisesDataTable();
            DTDepartamentos = new AlvecoComercial10DataSet.DepartamentosDataTable();
            DTProvincias = new AlvecoComercial10DataSet.ProvinciasDataTable();
            DTLugares = new AlvecoComercial10DataSet.LugaresDataTable();

            cargarProcedencia();

            eProviderProveedores = new ErrorProvider();
            DTProveedores = TAProveedores.GetData();
            bdSourceProveedores.DataSource = DTProveedores;
            Utilidades.ObjetoCodigoDescripcion TiposProveedores = new Utilidades.ObjetoCodigoDescripcion();
            TiposProveedores.cargarDatosTiposClientesProveedores();
            cBoxTiposProveedores.DataSource = TiposProveedores.listaObjetos;
            cBoxTiposProveedores.DisplayMember = TiposProveedores.DisplayMember;
            cBoxTiposProveedores.ValueMember = TiposProveedores.ValueMember;

            Utilidades.ObjetoCodigoDescripcion EstadoProveedores = new Utilidades.ObjetoCodigoDescripcion();
            EstadoProveedores.cargarDatosEstadoProveedores();
            cBoxEstadoProveedor.DataSource = EstadoProveedores.listaObjetos;
            cBoxEstadoProveedor.DisplayMember = EstadoProveedores.DisplayMember;
            cBoxEstadoProveedor.ValueMember = EstadoProveedores.ValueMember;

            txtNombre.KeyPress += new KeyPressEventHandler(Utilidades.EventosValidacionTexto.TextBoxCaracteres_KeyPress);
            txtRepresentante.KeyPress += new KeyPressEventHandler(Utilidades.EventosValidacionTexto.TextBoxCaracteres_KeyPress);
            txtTelefono.KeyPress += new KeyPressEventHandler(Utilidades.EventosValidacionTexto.TextBoxEnteros_KeyPress);
            txtFax.KeyPress += new KeyPressEventHandler(Utilidades.EventosValidacionTexto.TextBoxEnteros_KeyPress);
            txtPorcentajeGananciaVentaPorMayor.KeyPress += new KeyPressEventHandler(Utilidades.EventosValidacionTexto.TextBoxFlotantes_KeyPress);
            txtPorcentajeGananciaVentaPorMenor.KeyPress += new KeyPressEventHandler(Utilidades.EventosValidacionTexto.TextBoxFlotantes_KeyPress);
            CargarDatosProveedores(-1);
        }

        private void FProveedores_Load(object sender, EventArgs e)
        {
            //TiposProveedores.cargarDatosTiposProveedores();
            //cBoxCliente.DataSource = TiposProveedores.listaObjetos;
            //cBoxCliente.DisplayMember = TiposProveedores.DisplayMember;
            //cBoxCliente.ValueMember = TiposProveedores.ValueMember;
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


        public void CargarDatosProveedores(int CodigoProveedor)
        {

            SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSet.ProveedoresRow DRProveedor = DTProveedores.FindByCodigoProveedor(CodigoProveedor);
            if (DRProveedor == null)
            {
                AlvecoComercial10DataSet.ProveedoresDataTable DTProveedoresAux = TAProveedores.GetDataBy1(CodigoProveedor);
                if (DTProveedoresAux.Count > 0)
                    DRProveedor = DTProveedoresAux[0];
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
                txtCodigo.Text = DRProveedor.CodigoProveedor.ToString();
                txtNombre.Text = DRProveedor.NombreRazonSocial;
                txtRepresentante.Text = DRProveedor.NombreRepresentante;
                cBoxTiposProveedores.SelectedValue = DRProveedor.CodigoTipoProveedor;
                txtNIT.Text = DRProveedor.IsNITProveedorNull() ? String.Empty : DRProveedor.NITProveedor;

                if (!DRProveedor.IsCodigoPaisNull())
                    cBoxPais.SelectedValue = DRProveedor.CodigoPais;
                else
                    cBoxPais.SelectedIndex = -1;

                if (!DRProveedor.IsCodigoDepartamentoNull())
                    cBoxDepartamento.SelectedValue = DRProveedor.CodigoDepartamento;
                else
                    cBoxDepartamento.SelectedIndex = -1;
                if (!DRProveedor.IsCodigoProvinciaNull())
                    cBoxProvincia.SelectedValue = DRProveedor.CodigoProvincia;
                else
                    cBoxProvincia.SelectedIndex = -1;
                if (!DRProveedor.IsCodigoLugarNull())
                {
                    CargarLugares(DRProveedor.CodigoPais, DRProveedor.CodigoDepartamento, DRProveedor.CodigoProvincia);
                    cBoxLugar.SelectedValue = DRProveedor.CodigoLugar;
                }
                else
                    cBoxLugar.SelectedIndex = -1;

                //cBoxPais.SelectedValue = DRCliente.IsCodigoPaisNull() ? null : DRCliente.CodigoPais;
                //cBoxDepartamento.SelectedValue = DRCliente.IsCodigoDepartamentoNull() ? null : DRCliente.CodigoDepartamento;
                //cBoxProvincia.SelectedValue = DRCliente.IsCodigoProvinciaNull() ? null : DRCliente.CodigoProvincia;
                //cBoxLugar.SelectedValue = DRCliente.IsCodigoLugarNull() ? null : DRCliente.CodigoLugar;
                txtDireccion.Text = DRProveedor.IsDireccionNull() ? String.Empty : DRProveedor.Direccion;
                txtTelefono.Text = DRProveedor.IsTelefonoNull() ? String.Empty : DRProveedor.Telefono;
                txtEmail.Text = DRProveedor.IsEmailNull() ? String.Empty : DRProveedor.Email;
                txtDescripcion.Text = DRProveedor.IsObservacionesNull() ? String.Empty : DRProveedor.Observaciones;
                cBoxEstadoProveedor.SelectedValue = DRProveedor.ProveedorActivo ? "A" : "B";
                txtCasilla.Text = DRProveedor.IsCasillaNull() ? String.Empty : DRProveedor.Casilla;
                txtFax.Text = DRProveedor.IsFaxNull() ? String.Empty : DRProveedor.Fax;
                txtRubro.Text = DRProveedor.IsRubroNull() ? "" : DRProveedor.Rubro;
                txtPorcentajeGananciaVentaPorMayor.Text = !DRProveedor.IsPorcentajeGananciaVentaPorMayorNull() ? DRProveedor.PorcentajeGananciaVentaPorMayor.ToString() : String.Empty;
                txtPorcentajeGananciaVentaPorMenor.Text = !DRProveedor.IsPorcentajeGananciaVentaPorMenorNull() ? DRProveedor.PorcentajeGananciaVentaPorMenor.ToString() : String.Empty;

                habilitarBotones(true, false, false, true, true);
                habilitarControles(false);
            }
        }

        public void habilitarControles(bool estadoHabilitacion)
        {
            txtNombre.ReadOnly = !estadoHabilitacion;
            txtRepresentante.ReadOnly = !estadoHabilitacion;
            cBoxTiposProveedores.Enabled = estadoHabilitacion;
            txtNIT.ReadOnly = !estadoHabilitacion;
            cBoxPais.Enabled = estadoHabilitacion;
            cBoxDepartamento.Enabled = estadoHabilitacion;
            cBoxProvincia.Enabled = estadoHabilitacion;
            cBoxLugar.Enabled = estadoHabilitacion;
            txtTelefono.ReadOnly = !estadoHabilitacion;
            txtDireccion.ReadOnly = !estadoHabilitacion;
            txtEmail.ReadOnly = !estadoHabilitacion;
            txtDescripcion.ReadOnly = !estadoHabilitacion;
            txtCasilla.ReadOnly = !estadoHabilitacion;
            txtFax.ReadOnly = !estadoHabilitacion;
            txtPorcentajeGananciaVentaPorMayor.ReadOnly = txtPorcentajeGananciaVentaPorMenor.ReadOnly = !estadoHabilitacion;
            txtRubro.ReadOnly = !estadoHabilitacion;
            btnAgregarComunidad.Enabled = estadoHabilitacion;
            btnAgregarDepartamento.Enabled = estadoHabilitacion;
            btnAgregarPais.Enabled = estadoHabilitacion;
            btnAgregarProvincia.Enabled = estadoHabilitacion;
            cBoxEstadoProveedor.Enabled = estadoHabilitacion;
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
            cBoxTiposProveedores.SelectedIndex = -1;
            txtNIT.Text = String.Empty;
            cBoxPais.SelectedIndex = -1;
            cBoxDepartamento.SelectedIndex = -1;
            cBoxProvincia.SelectedIndex = -1;
            cBoxLugar.SelectedIndex = -1;
            txtTelefono.Text = String.Empty;
            txtDireccion.Text = String.Empty;
            txtEmail.Text = String.Empty;
            txtDescripcion.Text = string.Empty;
            cBoxEstadoProveedor.SelectedIndex = -1;
            txtCasilla.Text = txtFax.Text = string.Empty;
            txtPorcentajeGananciaVentaPorMayor.Text = txtPorcentajeGananciaVentaPorMenor.Text = txtRubro.Text = String.Empty;
            eProviderProveedores.Clear();
        }

        public bool validarDatos()
        {
            if (String.IsNullOrEmpty(txtNombre.Text.Trim()))
            {
                eProviderProveedores.SetError(txtNombre, "Aún no ha ingresado el Nombre del Proveedor");
                txtNombre.Focus();
                txtNombre.SelectAll();
                return false;
            }
            if (String.IsNullOrEmpty(txtRepresentante.Text.Trim()))
            {
                eProviderProveedores.SetError(txtRepresentante, "Aún no ha ingresado el Nombre del Representante del Proveedor");
                txtRepresentante.Focus();
                txtRepresentante.SelectAll();
                return false;
            }
            if (cBoxTiposProveedores.SelectedIndex < 0)
            {
                eProviderProveedores.SetError(cBoxTiposProveedores, "Aún no ha seleccionado el tipo de Proveedor");
                cBoxTiposProveedores.Focus();
                cBoxTiposProveedores.SelectAll();
                return false;
            }
            if (cBoxEstadoProveedor.SelectedIndex < 0)
            {
                eProviderProveedores.SetError(cBoxEstadoProveedor, "Aún no ha seleccionado el Estado del proveedor");
                cBoxEstadoProveedor.Focus();
                cBoxEstadoProveedor.SelectAll();
                cBoxEstadoProveedor.SelectedValue = "A";
                return false;
            }

            if (TipoOperacion == "I" && cBoxEstadoProveedor.SelectedValue.Equals("B") )
            {
                eProviderProveedores.SetError(cBoxEstadoProveedor, "No puede registrar a un proveedor con el estado DE BAJA");
                cBoxEstadoProveedor.Focus();
                cBoxEstadoProveedor.SelectAll();
                return false;
            }

            decimal PorcentajeGananciaVentaPorMayor = -1;
            decimal PorcentajeGananciaVentaPorMenor = -1;

            if (!string.IsNullOrEmpty(txtPorcentajeGananciaVentaPorMayor.Text)
                && !decimal.TryParse(txtPorcentajeGananciaVentaPorMayor.Text, out PorcentajeGananciaVentaPorMayor))
            {
                eProviderProveedores.SetError(txtPorcentajeGananciaVentaPorMayor, "El % de Ganancia de Venta al Por Mayor se encuentra mal Escrito");
                txtPorcentajeGananciaVentaPorMayor.Focus();
                txtPorcentajeGananciaVentaPorMayor.SelectAll();
                return false;
            }

            if (!string.IsNullOrEmpty(txtPorcentajeGananciaVentaPorMenor.Text)
                && !decimal.TryParse(txtPorcentajeGananciaVentaPorMenor.Text, out PorcentajeGananciaVentaPorMenor))
            {
                eProviderProveedores.SetError(txtPorcentajeGananciaVentaPorMenor, "El % de Ganancia de Venta al Por Menor se encuentra mal Escrito");
                txtPorcentajeGananciaVentaPorMenor.Focus();
                txtPorcentajeGananciaVentaPorMenor.SelectAll();
                return false;
            }
            return true;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            limpiarControles();
            int CodigoProveedoresiguiente = Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("Proveedores") == -1
                ? 1 : Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("Proveedores") + 1;
            txtCodigo.Text = CodigoProveedoresiguiente.ToString();
            TipoOperacion = "I";
            habilitarControles(true);            
            habilitarBotones(false, true, true, false, false);
            cBoxEstadoProveedor.SelectedValue = "A";
            cBoxEstadoProveedor.Enabled = false;
            cBoxPais.SelectedValue = "BO";
            cBoxDepartamento.SelectedValue = "02";
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
                eProviderProveedores.Clear();
                if (validarDatos())
                {
                    if (TipoOperacion == "I")
                    {
                        int CodigoProveedorAux = int.Parse(txtCodigo.Text);                       


                        TAProveedores.Insert(txtNombre.Text, txtRepresentante.Text, cBoxTiposProveedores.SelectedValue.ToString(), txtRubro.Text, txtNIT.Text,
                            cBoxPais.SelectedIndex >= 0 ? cBoxPais.SelectedValue.ToString() : null, cBoxDepartamento.SelectedIndex >= 0 ? cBoxDepartamento.SelectedValue.ToString() : null,
                            cBoxProvincia.SelectedIndex >= 0 ? cBoxProvincia.SelectedValue.ToString() : null,
                            cBoxLugar.SelectedIndex >= 0 ? cBoxLugar.SelectedValue.ToString() : null, txtDireccion.Text,
                            txtTelefono.Text, txtFax.Text, txtCasilla.Text, 
                            !String.IsNullOrEmpty(txtPorcentajeGananciaVentaPorMayor.Text) ? decimal.Parse(txtPorcentajeGananciaVentaPorMayor.Text) : (decimal?)null,
                            !String.IsNullOrEmpty(txtPorcentajeGananciaVentaPorMenor.Text) ? decimal.Parse(txtPorcentajeGananciaVentaPorMenor.Text) : (decimal?)null,
                            txtEmail.Text, txtDescripcion.Text, true);
                        AlvecoComercial10DataSet.ProveedoresRow DRAlmacenNuevo = DTProveedores.AddProveedoresRow(txtNombre.Text, txtRepresentante.Text, cBoxTiposProveedores.SelectedValue.ToString(), txtNIT.Text,
                            cBoxPais.SelectedIndex >= 0 ? cBoxPais.SelectedValue.ToString() : null, cBoxDepartamento.SelectedIndex >= 0 ? cBoxDepartamento.SelectedValue.ToString() : null,
                            cBoxProvincia.SelectedIndex >= 0 ? cBoxProvincia.SelectedValue.ToString() : null,
                            cBoxLugar.SelectedIndex >= 0 ? cBoxLugar.SelectedValue.ToString() : null, txtDireccion.Text,
                            txtTelefono.Text, txtFax.Text, txtCasilla.Text,                             
                            txtEmail.Text, txtDescripcion.Text, true,
                            !String.IsNullOrEmpty(txtPorcentajeGananciaVentaPorMayor.Text) ? decimal.Parse(txtPorcentajeGananciaVentaPorMayor.Text) : 0,
                            !String.IsNullOrEmpty(txtPorcentajeGananciaVentaPorMenor.Text) ? decimal.Parse(txtPorcentajeGananciaVentaPorMenor.Text) : 0,
                            txtRubro.Text);
                        if(!String.IsNullOrEmpty(txtPorcentajeGananciaVentaPorMayor.Text))
                            DRAlmacenNuevo.PorcentajeGananciaVentaPorMayor = decimal.Parse(txtPorcentajeGananciaVentaPorMayor.Text);
                        else
                            DRAlmacenNuevo.SetPorcentajeGananciaVentaPorMayorNull();

                        if (!String.IsNullOrEmpty(txtPorcentajeGananciaVentaPorMenor.Text))
                            DRAlmacenNuevo.PorcentajeGananciaVentaPorMayor = decimal.Parse(txtPorcentajeGananciaVentaPorMayor.Text);
                        else
                            DRAlmacenNuevo.SetPorcentajeGananciaVentaPorMayorNull();


                        DRAlmacenNuevo.CodigoProveedor = CodigoProveedorAux;
                        DRAlmacenNuevo.AcceptChanges();
                        DTProveedores.AcceptChanges();

                        this.CodigoProveedor = CodigoProveedorAux;
                        
                        //DTProveedores.AcceptChanges();
                    }

                    else
                    {
                        TAProveedores.ActualizarProveedor(int.Parse(txtCodigo.Text), txtNombre.Text, txtRepresentante.Text, cBoxTiposProveedores.SelectedValue.ToString(), txtRubro.Text, txtNIT.Text,
                            cBoxPais.SelectedIndex >= 0 ? cBoxPais.SelectedValue.ToString() : null, cBoxDepartamento.SelectedIndex >= 0 ? cBoxDepartamento.SelectedValue.ToString() : null,
                            cBoxProvincia.SelectedIndex >= 0 ? cBoxProvincia.SelectedValue.ToString() : null,
                            cBoxLugar.SelectedIndex >= 0 ? cBoxLugar.SelectedValue.ToString() : null, txtDireccion.Text,
                            txtTelefono.Text, txtFax.Text, txtCasilla.Text,
                            !String.IsNullOrEmpty(txtPorcentajeGananciaVentaPorMayor.Text) ? decimal.Parse(txtPorcentajeGananciaVentaPorMayor.Text) : (decimal?)null,
                            !String.IsNullOrEmpty(txtPorcentajeGananciaVentaPorMenor.Text) ? decimal.Parse(txtPorcentajeGananciaVentaPorMenor.Text) : (decimal?)null,
                            txtEmail.Text, txtDescripcion.Text, true);
                        int indiceEdicion = DTProveedores.Rows.IndexOf(DTProveedores.FindByCodigoProveedor(int.Parse(txtCodigo.Text)));

                        DTProveedores[indiceEdicion].NombreRazonSocial = txtNombre.Text;
                        DTProveedores[indiceEdicion].NombreRepresentante = txtRepresentante.Text;
                        DTProveedores[indiceEdicion].NITProveedor = txtNIT.Text;
                        DTProveedores[indiceEdicion].CodigoTipoProveedor = cBoxTiposProveedores.SelectedValue.ToString();
                        DTProveedores[indiceEdicion].CodigoPais = cBoxPais.SelectedIndex >= 0 ? cBoxPais.SelectedValue.ToString() : String.Empty;
                        DTProveedores[indiceEdicion].CodigoDepartamento = cBoxDepartamento.SelectedIndex >= 0 ? cBoxDepartamento.SelectedValue.ToString() : String.Empty;
                        DTProveedores[indiceEdicion].CodigoProvincia = cBoxProvincia.SelectedIndex >= 0 ? cBoxProvincia.SelectedValue.ToString() : String.Empty;
                        DTProveedores[indiceEdicion].CodigoLugar = cBoxLugar.SelectedIndex >= 0 ? cBoxLugar.SelectedValue.ToString() : String.Empty;
                        DTProveedores[indiceEdicion].Observaciones = txtDescripcion.Text;
                        DTProveedores[indiceEdicion].Direccion = txtDireccion.Text;
                        DTProveedores[indiceEdicion].Telefono = txtTelefono.Text;
                        DTProveedores[indiceEdicion].Fax = txtFax.Text;
                        DTProveedores[indiceEdicion].Casilla = txtCasilla.Text;
                        DTProveedores[indiceEdicion].Email = txtEmail.Text;
                        DTProveedores[indiceEdicion].Rubro = txtRubro.Text;
                        DTProveedores[indiceEdicion].Observaciones = txtDescripcion.Text;
                        if (!String.IsNullOrEmpty(txtPorcentajeGananciaVentaPorMayor.Text))
                            DTProveedores[indiceEdicion].PorcentajeGananciaVentaPorMayor = decimal.Parse(txtPorcentajeGananciaVentaPorMayor.Text);
                        else
                            DTProveedores[indiceEdicion].SetPorcentajeGananciaVentaPorMayorNull();

                        if (!String.IsNullOrEmpty(txtPorcentajeGananciaVentaPorMenor.Text))
                            DTProveedores[indiceEdicion].PorcentajeGananciaVentaPorMenor= decimal.Parse(txtPorcentajeGananciaVentaPorMenor.Text);
                        else
                            DTProveedores[indiceEdicion].SetPorcentajeGananciaVentaPorMenorNull();

                        DTProveedores.AcceptChanges();
                    }
                    habilitarBotones(true, false, false, true, true);
                    habilitarControles(false);
                    TipoOperacion = "";
                    //MessageBox.Show(this, "Operación realizada correctamente", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (soloInsertarEditar)
                    {
                        DialogResult = System.Windows.Forms.DialogResult.OK;
                        bdSourceProveedores.MoveLast();
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
            eProviderProveedores.Clear();
            TipoOperacion = "";
            limpiarControles();
            habilitarControles(false);
            habilitarBotones(true, false, false, false, false);
            bdSourceProveedores_CurrentChanged(bdSourceProveedores, e);
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
                    TAProveedores.Delete(int.Parse(txtCodigo.Text));
                    DTProveedores.Rows.Remove(DTProveedores.FindByCodigoProveedor(int.Parse(txtCodigo.Text)));
                    DTProveedores.AcceptChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "No se pudo culminar la operación actual, seguramente el registro se encuentra en uso en otras operaciones sel sistema", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dtGVProveedores_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex >= 0 && DTProveedores.Count > 0)
            //{
            //    CargarDatosAlmacen(DTProveedores[e.RowIndex].CodigoProveedor);
            //}
        }

        private void bdSourceProveedores_CurrentChanged(object sender, EventArgs e)
        {
            //if(dtGVProveedores.CurrentCell != null)
            //    dtGVProveedores_CellContentClick(dtGVProveedores, new DataGridViewCellEventArgs(0, bdSourceProveedores.Position));
            //if (bdSourceProveedores.Position >= 0)
            //    CargarDatosProveedores(DTProveedores[bdSourceProveedores.Position].CodigoProveedor);

            if (bdSourceProveedores.Position >= 0)
            {
                CodigoProveedor = DTProveedores[bdSourceProveedores.Position].CodigoProveedor;
                CargarDatosProveedores(CodigoProveedor);

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
            if (!String.IsNullOrEmpty(fgeografico.CodigoPais))
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

        public void configurarFormularioIA(int CodigoProveedor)
        {

            CargarDatosProveedores(CodigoProveedor);
            TipoOperacion = CodigoProveedor < 0 ? "I" : "E";
            if (CodigoProveedor < 0)
            {
                int NumeroProveedorSiguiente = Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("Proveedores")  == -1
                ? 1 : Utilidades.DAOUtilidades.ObtenerUltimoIndiceTabla("Proveedores") + 1;
                txtCodigo.Text = NumeroProveedorSiguiente.ToString();
            }
            soloInsertarEditar = true;
            gBoxGrilla.Visible = false;
            //290
            btnEditar.Visible = btnNuevo.Visible = btnEliminar.Visible = false;
            this.Size = new Size(gBoxDatos.Size.Width +20 , this.Size.Height);
            habilitarControles(true);
            habilitarBotones(false, true, true, false, false);
            cBoxEstadoProveedor.SelectedValue = "A";
            cBoxEstadoProveedor.Enabled = false;
            cBoxPais.SelectedValue = "BO";
            cBoxDepartamento.SelectedValue = "02";
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
