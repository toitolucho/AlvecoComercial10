using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSetTableAdapters;

namespace SAlvecoComercial10.Formularios.GestionComercial
{
    public partial class FVentasProductosDistribucionDatos : Form
    {

        VentasProductosTableAdapter TAVentasProductos;
        PersonasTableAdapter TAPersonas;
        MovilidadesTableAdapter TAMovilidades;

        AccesoDatos.AlvecoComercial10DataSet.PersonasDataTable DTPersonas;
        AccesoDatos.AlvecoComercial10DataSet.MovilidadesDataTable DTMovilidades;

        private string DIUsuario;
        private int NumeroVentaProducto;
        private int NumeroAlmacen;
        public FVentasProductosDistribucionDatos(string DIUsuario, int NumeroAlmacen, int NumeroVentaProducto)
        {
            InitializeComponent();

            TAVentasProductos = new VentasProductosTableAdapter();
            TAVentasProductos.Connection = Utilidades.DAOUtilidades.conexion;
            TAPersonas = new PersonasTableAdapter();
            TAPersonas.Connection = Utilidades.DAOUtilidades.conexion;
            TAMovilidades = new MovilidadesTableAdapter();
            TAMovilidades.Connection = Utilidades.DAOUtilidades.conexion;

            this.DIUsuario = DIUsuario;
            this.NumeroVentaProducto = NumeroVentaProducto;
            this.NumeroAlmacen = NumeroAlmacen;



            DTPersonas = TAPersonas.GetDataByParticulares(null);
            this.cboxPersonas.DataSource = DTPersonas;
            this.cboxPersonas.DisplayMember = "NombreCompleto";
            this.cboxPersonas.ValueMember = "DIPersona";
            this.cboxPersonas.SelectedIndex = -1;

            DTMovilidades = TAMovilidades.GetData();
            this.cBoxMovilidades.DataSource = DTMovilidades;
            this.cBoxMovilidades.DisplayMember = "NombreMovilidad";
            this.cBoxMovilidades.ValueMember = "CodigoMovilidad";
            this.cBoxMovilidades.SelectedIndex = -1;

            tsLblNroVenta.Text = NumeroVentaProducto.ToString();

        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (cboxPersonas.SelectedIndex < 0)
            {
                MessageBox.Show(this, "Aún no ha Seleccionado al Responsable para la Distribución de Productos de Esta Venta",
                    this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                cboxPersonas.Focus();
                return;
            }
            if (cBoxMovilidades.SelectedIndex < 0)
            {
                MessageBox.Show(this, "Aún no ha Seleccionado la Movilidad para la Distribución de Productos de Esta Venta",
                    this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                cBoxMovilidades.Focus();
                return;
            }

            TAVentasProductos.ActualizarVentaDistribucionDatos(NumeroAlmacen, NumeroVentaProducto, Utilidades.DAOUtilidades.ObtenerCodigoEstadoActualTransacciones(NumeroAlmacen, NumeroVentaProducto, "V"),
                cboxPersonas.SelectedValue.ToString(), true, cBoxMovilidades.SelectedValue.ToString());
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void btnAgregarPersonas_Click(object sender, EventArgs e)
        {
            Formularios.GestionSistema.FPersonas formPersonas = new GestionSistema.FPersonas();
            formPersonas.ShowDialog();            

            DTPersonas = TAPersonas.GetData(null);
            this.cboxPersonas.DataSource = DTPersonas;
            this.cboxPersonas.DisplayMember = "NombreCompleto";
            this.cboxPersonas.ValueMember = "DIPersona";
            if (!String.IsNullOrEmpty(formPersonas.DIPersona))
                this.cboxPersonas.SelectedValue = formPersonas.DIPersona;
            else
                this.cboxPersonas.SelectedIndex = -1;
            formPersonas.Dispose();
        }

        private void btnAgregarMovilidad_Click(object sender, EventArgs e)
        {
            Formularios.GestionSistema.FMovilidades formMovilidades = new GestionSistema.FMovilidades();
            formMovilidades.ShowDialog();

            DTMovilidades = TAMovilidades.GetData();
            this.cBoxMovilidades.DataSource = DTMovilidades;
            this.cBoxMovilidades.DisplayMember = "NombreMovilidad";
            this.cBoxMovilidades.ValueMember = "CodigoMovilidad";
            if (!String.IsNullOrEmpty(formMovilidades.CodigoMovilidad))
                this.cBoxMovilidades.SelectedValue = formMovilidades.CodigoMovilidad;
            else
                this.cBoxMovilidades.SelectedIndex = -1;
            formMovilidades.Dispose();
        }
    }
}
