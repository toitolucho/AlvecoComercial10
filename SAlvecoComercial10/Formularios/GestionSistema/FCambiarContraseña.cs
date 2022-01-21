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
    public partial class FCambiarContraseña : Form
    {
        static private ListarUsuariosPersonasTableAdapter TAUsuarios = new ListarUsuariosPersonasTableAdapter();
        string DIUsuario; string NombreUsuario; string NombreServidor;
        public FCambiarContraseña(string DIUsuario, string NombreUsuario, string NombreServidor)
        {
            InitializeComponent();
            this.DIUsuario = DIUsuario;
            this.NombreUsuario = NombreUsuario;
            this.NombreServidor = NombreServidor;
            TAUsuarios.Connection = SAlvecoComercial10.Formularios.Utilidades.DAOUtilidades.conexion;
        }
        public FCambiarContraseña()
        {
            InitializeComponent();
            TAUsuarios.Connection = SAlvecoComercial10.Formularios.Utilidades.DAOUtilidades.conexion;
        }

        private void bAceptar_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(DAOUtilidades.VerificarUsuario(NombreUsuario, tBContrasenaActual.Text)))
            {
                if (tBContrasenaNueva.Text == tBContrasenaNuevaRepetida.Text)
                {
                    System.Data.SqlClient.SqlConnection coneccionAdministracion = new System.Data.SqlClient.SqlConnection("Data Source=" + NombreServidor + ";Initial Catalog=AlvecoComercial10;User ID=administrador;Password=administrador");
                    coneccionAdministracion.Open();
                    TAUsuarios.Connection = coneccionAdministracion;
                    TAUsuarios.ActualizarContrasenaUsuario(DIUsuario, NombreUsuario, tBContrasenaNueva.Text);
                    MessageBox.Show("Se ha registrado exitosamente su nueva contraseña");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Las contraseña nueva no coindice en los dos cuadros de texto");
                }
            }
            else
            {
                MessageBox.Show("La contraseña actual escrita, no es correcta. Vuelva a escribirla.");
            }
        }

        private void bCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
