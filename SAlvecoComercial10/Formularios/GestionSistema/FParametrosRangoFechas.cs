using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SAlvecoComercial10.Formularios.GestionSistema
{
    public partial class FParametrosRangoFechas : Form
    {
        public DateTime FechaHoraInicio { get { return dateTimePickerFechaInicio.Value; } }
        public DateTime FechaHoraFin { get { return dateTimePickerFechaFin.Value; } }
        public FParametrosRangoFechas()
        {
            InitializeComponent();
        }

        private void FParametrosRangoFechas_Load(object sender, EventArgs e)
        {
            dateTimePickerFechaInicio.Value = DateTime.Parse("01/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString());
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
