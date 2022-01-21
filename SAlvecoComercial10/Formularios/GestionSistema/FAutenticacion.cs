using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSetTableAdapters;
using SAlvecoComercial10.Formularios.Utilidades;

namespace SAlvecoComercial10.Formularios.GestionSistema
{
    public partial class FAutenticacion : Form
    {
        private QTAUtilidadesFunciones _QTAUtilidadesFunciones;
        private UsuariosTableAdapter _UsuariosTableAdapter; 
        public string Servidor = "";
        public string BaseDatos = "";
        public string DIUsuario = "0";
        public string NombreUsuario = "";
        public System.Data.SqlClient.SqlConnection Coneccion;
        public bool OperacionConfirmada = false;
        public FAutenticacion()
        {
            InitializeComponent();
        }

        private void FAutenticacion_Load(object sender, EventArgs e)
        {
            string Servidor = System.Configuration.ConfigurationSettings.AppSettings["Servidor"];
            string BaseDatos = System.Configuration.ConfigurationSettings.AppSettings["BaseDatos"];
            string NombreUsuario = System.Configuration.ConfigurationSettings.AppSettings["NombreUsuario"];

            tBServidor.Text = Servidor;
            tBBaseDatos.Text = BaseDatos;

            splitContainer1.Panel2Collapsed = true;
        }

        private void label5_Click(object sender, EventArgs e)
        {
            
        }

        private void bAceptar_Click(object sender, EventArgs e)
        {
            String connectionString = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}", tBServidor.Text, tBBaseDatos.Text, tBNombreUsuario.Text, tBContrasena.Text);
            //SqlConnection sqlConnection = new SqlConnection(@"Integrated Security=SSPI; Data Source=(local)\SQLEXPRESS");
            //String connectionString = string.Format("Data Source={0};Initial Catalog={1};Integrated Security=SSPI;", tBServidor.Text, tBBaseDatos.Text);
            this.Coneccion = new System.Data.SqlClient.SqlConnection(connectionString);

            try
            {
                Coneccion.Open();
            }
            catch (Exception)
            {
                MessageBox.Show(this, "No puede conectarse con el Servidor, probablemente sus datos de Autenticación al sistema se encuentra escritos incorrectamente, proceda a revisarlos", "Conección no Valida", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;                
            }

            DAOUtilidades.cargarConeccion(this.Coneccion);
            
            DIUsuario = DAOUtilidades.VerificarUsuario(tBNombreUsuario.Text, tBContrasena.Text);

            if ( String.IsNullOrEmpty(DIUsuario))
            {
                MessageBox.Show("Los datos proporcionados en la autenticacion no son validos, verifique y vuelva a intentar.");
            }
            else
            {
                Servidor = tBServidor.Text;
                BaseDatos = tBBaseDatos.Text;
                NombreUsuario = tBNombreUsuario.Text;

                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                

                if (config.AppSettings.Settings["Servidor"].Value.ToString().CompareTo(Servidor) != 0)
                {
                    config.AppSettings.Settings["Servidor"].Value = Servidor;
                }

                if (config.AppSettings.Settings["BaseDatos"].Value.ToString().CompareTo(BaseDatos) != 0)
                {
                    config.AppSettings.Settings["BaseDatos"].Value = BaseDatos;
                }
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
                OperacionConfirmada = true;
                this.Close();
            }
               
        }

        private void bCancelar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void FAutenticacion_Shown(object sender, EventArgs e)
        {
            tBNombreUsuario.Focus();
        }

        private void FAutenticacion_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!OperacionConfirmada && MessageBox.Show(this, "¿Se Encuentra seguro de cancelar el ingreso a la aplicación?", "Autenticación de Usuario", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                e.Cancel = true;            
        }

        private void tBContrasena_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnConfirgurar_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2Collapsed = !splitContainer1.Panel2Collapsed;
        }
    }
}
