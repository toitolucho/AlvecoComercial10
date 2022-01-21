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
    public partial class FMovilidades : Form
    {
        MovilidadesTableAdapter _MovilidadesTableAdapter; 
        MovilidadesModelosTableAdapter _MovilidadesModelosTableAdapter;     
        AlvecoComercial10DataSet.MovilidadesDataTable DTMovilidades;  
        AlvecoComercial10DataSet.MovilidadesModelosDataTable DTMovilidadesModelos;
        ProductosMarcasTableAdapter _ProductosMarcas;
        DataTable DTMarcas;
        string TipoOperacion = "";
        public string CodigoMovilidad = null;
        public FMovilidades()
        {
            InitializeComponent();
            _MovilidadesTableAdapter = new MovilidadesTableAdapter();
            _MovilidadesTableAdapter.Connection = Utilidades.DAOUtilidades.conexion;
            _MovilidadesModelosTableAdapter = new MovilidadesModelosTableAdapter();
            _MovilidadesModelosTableAdapter.Connection = Utilidades.DAOUtilidades.conexion;
            DTMovilidades = (AlvecoComercial10DataSet.MovilidadesDataTable)_MovilidadesTableAdapter.GetData();
            _ProductosMarcas = new ProductosMarcasTableAdapter();
            _ProductosMarcas.Connection = Utilidades.DAOUtilidades.conexion;
            DTMovilidades.CodigoMovilidadColumn.ReadOnly = false;
            dGVGrilla.AutoGenerateColumns = false;
            dGVGrilla.DataSource = DTMovilidades;
            cargarMarcas();
            cargarMovilidadesModelos();
            cargarDatosMovilidades("0");
            cBoxEstadoMovilidad.Enabled = false;
            dGVGrilla.Enabled = true;
        }

        public void cargarMovilidadesModelos()
        {
            DTMovilidadesModelos = (AlvecoComercial10DataSet.MovilidadesModelosDataTable)_MovilidadesModelosTableAdapter.GetData();
            cBModelo.DataSource = DTMovilidadesModelos;
            cBModelo.DisplayMember = DTMovilidadesModelos.NombreModeloColumn.ColumnName;
            cBModelo.ValueMember = DTMovilidadesModelos.CodigoModeloColumn.ColumnName;
        }

        public void cargarMarcas()
        {
            DTMarcas = _ProductosMarcas.GetData();
            DTMarcas.DefaultView.RowFilter = "CodigoTipoMarca = 'M'";
            cBMarca.DataSource = DTMarcas;
            cBMarca.DisplayMember = "NombreMarca";
            cBMarca.ValueMember = "CodigoMarca";
        }


        public void habilitarControles(bool estadoHabilitacion)
        {
            if(TipoOperacion =="E")
                tBCodigo.ReadOnly = true;
            else
                tBCodigo.ReadOnly = !estadoHabilitacion;
            tBNombre.ReadOnly = !estadoHabilitacion;
            tBPlaca.ReadOnly = !estadoHabilitacion;
            tBDescripcion.ReadOnly = !estadoHabilitacion;
            cBMarca.Enabled = estadoHabilitacion;
            cBModelo.Enabled = estadoHabilitacion;
            btnMarcas.Enabled = estadoHabilitacion;
            btnModelos.Enabled = estadoHabilitacion;
        }

        public void cargarDatosMovilidades(string CodigoMovilidad)
        {
            AlvecoComercial10DataSet.MovilidadesDataTable DTMovilidadesAux = (AlvecoComercial10DataSet.MovilidadesDataTable)_MovilidadesTableAdapter.GetDataBy1(CodigoMovilidad);
            if (DTMovilidadesAux.Count > 0)
            {
                tBNombre.Text = DTMovilidadesAux[0].NombreMovilidad;
                tBPlaca.Text = DTMovilidadesAux[0].IsCodigoPlacaNull() ? "" : DTMovilidadesAux[0].CodigoPlaca;
                tBDescripcion.Text = DTMovilidadesAux[0].Descripcion;
                cBMarca.SelectedValue = DTMovilidadesAux[0].CodigoMarca;
                cBModelo.SelectedValue = DTMovilidadesAux[0].CodigoModelo;
                tBCodigo.Text = DTMovilidadesAux[0].CodigoMovilidad.ToString();
                cBoxEstadoMovilidad.SelectedIndex = DTMovilidades[0].CodigoEstadoMovilidad == "A" ? 0 : 1;
                habilitarBotones(true, false, false, true, true);
            }
            else
            {
                limpiarControles();                
                habilitarBotones(true, false, false, false, false);
            }
            habilitarControles(false);
        }

        public void limpiarControles()
        {
            tBCodigo.Text = String.Empty;
            ePMovilidades.Clear();
            tBNombre.Text = string.Empty;
            tBPlaca.Text = string.Empty;
            tBDescripcion.Text = string.Empty;
            cBMarca.SelectedIndex = cBModelo.SelectedIndex = -1;
            TipoOperacion = "";
        }

        public bool validarDatos()
        {
            if (string.IsNullOrEmpty(tBNombre.Text.Trim()))
            {
                ePMovilidades.SetError(tBCodigo, "Aún no ha Ingresado el Codigo de la Movilidad");
                tBCodigo.Focus();
                tBCodigo.SelectAll();
                return false;
            }
            if (string.IsNullOrEmpty(tBNombre.Text.Trim()))
            {
                ePMovilidades.SetError(tBNombre, "Aún no ha Ingresado el Nombre de la Movilidad");
                tBNombre.Focus();
                tBNombre.SelectAll();
                return false;
            }
            if (string.IsNullOrEmpty(tBPlaca.Text.Trim()))
            {
                ePMovilidades.SetError(tBPlaca, "Aún no ha Ingresado la Placa de la Movilidad");
                tBPlaca.Focus();
                tBPlaca.SelectAll();
                return false;
            }
            if (cBMarca.SelectedIndex == -1)
            {
                ePMovilidades.SetError(cBMarca, "Aún no ha seleccionado la Marca de la Movilidad");
                cBMarca.Focus();
                cBMarca.SelectAll();
                return false;
            }
            if (cBModelo.SelectedIndex == -1)
            {
                ePMovilidades.SetError(cBModelo, "Aún no ha seleccionado el Modelo de la Movilidad");
                cBModelo.Focus();
                cBModelo.SelectAll();
                return false;
            }
            return true;
        }

        public void habilitarBotones(bool nuevo, bool cancelar, bool aceptar, bool editar, bool eliminar)
        {
            this.bAceptar.Enabled = aceptar;
            this.bNuevo.Enabled = nuevo;
            this.bCancelar.Enabled = cancelar;
            this.bEditar.Enabled = editar;
            this.bEliminar.Enabled = eliminar;
        }

        private void dGVGrilla_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int fila = 0;
            fila = dGVGrilla.CurrentCell.RowIndex;
            if (fila >= 0)
            {
                cargarDatosMovilidades(DTMovilidades[fila].CodigoMovilidad);
            }
            
        }

        private void dGVGrilla_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int fila = 0;
            fila = dGVGrilla.CurrentCell.RowIndex;
            if (fila >= 0)
            {
                cargarDatosMovilidades(DTMovilidades[fila].CodigoMovilidad);
                tabControl1.SelectedIndex = 0;
            }
        }

        private void bNuevo_Click(object sender, EventArgs e)
        {
            
            habilitarBotones(false, true, true, false, false);
            tabControl1.Controls[1].Enabled = false;
            limpiarControles();
            habilitarControles(true);
            tBCodigo.Text = Utilidades.DAOUtilidades.ObtenerCodigoMovilidadGenerado();
            TipoOperacion = "N";
            cBoxEstadoMovilidad.Enabled = false;
            cBoxEstadoMovilidad.SelectedIndex = 0;//ACTIVO
        }

        private void bEditar_Click(object sender, EventArgs e)
        {
            TipoOperacion = "E";
            habilitarControles(true);            
            habilitarBotones(false, true, true, false, false);
            tBNombre.Focus();
            cBoxEstadoMovilidad.Enabled = true;

        }

        private void bEliminar_Click(object sender, EventArgs e)
        {
            if (tBCodigo.Text.Length <= 0)
            {
                MessageBox.Show("No se puede eliminar puesto que no eligió ningún registro para hacerlo");
                bNuevo.Focus();
                return;
            }

            string Mensaje = "Esta seguro que desea eliminar el registro actual?, recuerde que una vez aceptada la operación es irreversible.";
            string Titulo = "Confimarción eliminación registro";
            MessageBoxButtons Botones = MessageBoxButtons.YesNo;
            MessageBoxIcon Icono = MessageBoxIcon.Exclamation;
            DialogResult result;

            result = MessageBox.Show(Mensaje, Titulo, Botones, Icono);

            if (result == DialogResult.Yes)
            {

                if (!String.IsNullOrEmpty(tBCodigo.Text.Trim()))
                {
                    CodigoMovilidad = tBCodigo.Text.Trim();
                    DTMovilidades.FindByCodigoMovilidad(CodigoMovilidad).Delete();
                    DTMovilidades.AcceptChanges();
                    limpiarControles();

                    _MovilidadesTableAdapter.Delete(CodigoMovilidad);
                }
            }
        }

        private void bAceptar_Click(object sender, EventArgs e)
        {
            if (validarDatos())
            {
                ePMovilidades.Clear();
                try
                {
                    if (TipoOperacion == "E")
                    {
                        if (cBoxEstadoMovilidad.SelectedIndex < 0)
                        {
                            ePMovilidades.SetError(cBoxEstadoMovilidad, "No ha Seleccionado el Estado de la Movilidad");
                            return;
                        }
                        _MovilidadesTableAdapter.ActualizarMovilidad(tBCodigo.Text.Trim(),
                            tBNombre.Text, tBPlaca.Text, int.Parse(cBMarca.SelectedValue.ToString()),
                            int.Parse(cBModelo.SelectedValue.ToString()), cBoxEstadoMovilidad.SelectedIndex == 0 ? "A" :"B", tBDescripcion.Text);

                        AlvecoComercial10DataSet.MovilidadesRow DRMovilidad = DTMovilidades.FindByCodigoMovilidad(tBCodigo.Text.Trim());
                        DRMovilidad.NombreMovilidad = tBNombre.Text;
                        DRMovilidad.CodigoPlaca = tBPlaca.Text;
                        DRMovilidad.CodigoMarca = int.Parse(cBMarca.SelectedValue.ToString());
                        DRMovilidad.CodigoModelo = int.Parse(cBModelo.SelectedValue.ToString());
                        DRMovilidad.Descripcion = tBDescripcion.Text;
                        DRMovilidad.CodigoEstadoMovilidad = cBoxEstadoMovilidad.SelectedIndex == 0 ? "A" : "B";
                        DRMovilidad.AcceptChanges();

                    }
                    else if (TipoOperacion == "N")
                    {
                        _MovilidadesTableAdapter.Insert( tBCodigo.Text.Trim(), tBNombre.Text, tBPlaca.Text, int.Parse(cBMarca.SelectedValue.ToString()),
                            int.Parse(cBModelo.SelectedValue.ToString()), "A", tBDescripcion.Text);
                        

                        DTMovilidades.AddMovilidadesRow(tBCodigo.Text.Trim(), tBNombre.Text, tBPlaca.Text, int.Parse(cBMarca.SelectedValue.ToString()),
                            int.Parse(cBModelo.SelectedValue.ToString()), "A", tBDescripcion.Text);
                        
                        DTMovilidades.AcceptChanges();

                        dGVGrilla.FirstDisplayedScrollingRowIndex = DTMovilidades.Rows.Count - 1;
                    }

                    habilitarBotones(true, false, false, true, true);
                    habilitarControles(false);
                    //MessageBox.Show(this, "Operación realizada correctamente", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //dGVGrilla.ReadOnly = false;
                    //dGVGrilla.Enabled = true;
                    tabControl1.Controls[1].Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Ocurrio la siguiente Excepción " + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    
                }
            }
        }

        private void bCancelar_Click(object sender, EventArgs e)
        {
            cBoxEstadoMovilidad.SelectedIndex = -1;
            limpiarControles();
            habilitarControles(false);
            habilitarBotones(true, false, false, false, false);
            cBoxEstadoMovilidad.Enabled = false;
            tabControl1.Controls[1].Enabled = true;
            
        }

        private void bCerrar_Click(object sender, EventArgs e)
        {
            CodigoMovilidad = string.IsNullOrEmpty(tBCodigo.Text) ? null :tBCodigo.Text.Trim();
            this.Close();
        }

        private void btnMarcas_Click(object sender, EventArgs e)
        {
            FProductosMarcas formMarcas = new FProductosMarcas("M");
            formMarcas.ShowDialog(this);            
            cargarMarcas();            
            cBMarca.SelectedValue = formMarcas.CodigoMarca;
            formMarcas.Dispose();
            cBModelo.Focus();
        }

        private void btnModelos_Click(object sender, EventArgs e)
        {
            FMovilidadesModelos formMovilidadesModelos = new FMovilidadesModelos();
            formMovilidadesModelos.ShowDialog(this);
            DataTable DTModelos = cBModelo.DataSource as DataTable;
            if (DTModelos.Rows.Find(formMovilidadesModelos.CodigoModeloActual) == null)
            {
                cargarMovilidadesModelos();
            }
            cBModelo.SelectedValue = formMovilidadesModelos.CodigoModeloActual;
            formMovilidadesModelos.Dispose();
            cBModelo.Focus();
        }
    }
}
