using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SAlvecoComercial10.Formularios.GestionComercial
{
    public partial class FSeleccionFechasReportes : Form
    {
        public object SelectedValueFiltro
        {
            get {return cBoxFiltro.SelectedValue; }
        }
        public DateTime FechaInicio
        {
            get { return dateTimePicker1.Value; }
        }
        public DateTime FechaFin
        {
            get { return dateTimePicker2.Value; }
        }
        public FSeleccionFechasReportes()
        {
            InitializeComponent();
            this.dateTimePicker1.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        }
        public void setVisibilidadFiltro(bool estadoVisible)
        {
            lblFiltro.Visible = gBoxFiltro.Visible = cBoxFiltro.Visible = estadoVisible;
        }
        public Label LabelFiltro
        {
            get { return this.lblFiltro; }
        }

        public void configurarSoloFiltro(String tituloVentana)
        {
            setVisibilidadFiltro(true);
            gBoxRangoFechas.Visible = label1.Visible = label2.Visible = dateTimePicker1.Visible = dateTimePicker2.Visible = false;
            this.Size = new Size(this.Size.Width, 200);
            gBoxFiltro.Location = gBoxRangoFechas.Location;
            btnAceptar.Location =  new Point(btnAceptar.Location.X, btnAceptar.Location.Y - 100);
            btnCancelar.Location = new Point(btnCancelar.Location.X, btnCancelar.Location.Y - 100);
            this.Text = tituloVentana;
        }
        public void cargarDatosFiltro(DataTable DTFiltro, string DisplayMember, string ValueMember)
        {
            cBoxFiltro.DataSource = DTFiltro;
            cBoxFiltro.DisplayMember = DisplayMember;
            cBoxFiltro.ValueMember = ValueMember;
            cBoxFiltro.SelectedIndex = -1;     
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Button btnAccion = sender as Button;
            this.DialogResult = btnAccion.Equals(btnAceptar) ? DialogResult.OK : DialogResult.Cancel;
        }
    }

}
