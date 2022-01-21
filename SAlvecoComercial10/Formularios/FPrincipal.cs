using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SAlvecoComercial10.Formularios.GestionSistema;
using SAlvecoComercial10.Formularios.Utilidades;
using SAlvecoComercial10.Formularios.GestionComercial;
using System.Data.SqlClient;
using SAlvecoComercial10.Formularios.Reportes;
using SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSetTableAdapters;
namespace SAlvecoComercial10.Formularios
{
    public partial class FPrincipal : Form
    {
        string DIUsuario = "-1";
        string NombreUsuario = "";
        int NumeroAlmacen = 1;
        int NumeroAlmacenOriginal = 1;
        string NombreServidor = "";
        string NombreAlmacen = "";
        bool esUsuarioAdministrador = true;
        public FPrincipal()
        {
            InitializeComponent();
        }

        private void FPrincipal_Load(object sender, EventArgs e)
        {
            try
            {
                FSplash fSplash = new FSplash(1);
                fSplash.ShowDialog(this);     // Mostramos el formulario de forma modal.
                fSplash.Dispose();

                FAutenticacion fautenticacion = new FAutenticacion();
                fautenticacion.ShowDialog();
                if (!fautenticacion.OperacionConfirmada)
                {
                    Application.Exit();
                }
                DIUsuario = fautenticacion.DIUsuario;
                NombreUsuario = fautenticacion.NombreUsuario;
                NombreServidor = fautenticacion.Servidor;
                sSPrincipal.Items[0].Text += fautenticacion.Servidor;
                sSPrincipal.Items[1].Text += fautenticacion.BaseDatos;
                sSPrincipal.Items[2].Text += DIUsuario.ToString();
                sSPrincipal.Items[3].Text += fautenticacion.NombreUsuario;
                

                fautenticacion.Dispose();

                DAOUtilidades.conexion = fautenticacion.Coneccion;

                if (!(DIUsuario.Trim().CompareTo("0000000000") == 0 || DIUsuario.Trim().CompareTo("5058785") == 0))
                {
                    cambiarDeAlmacenToolStripMenuItem.Visible = gestionBDToolStripMenuItem.Visible = administraciónToolStripMenuItem.Visible = false;                    
                    esUsuarioAdministrador = false;
                }

                AccesoDatos.AlvecoComercial10DataSetTableAdapters.ListarUsuariosPersonasTableAdapter TAUsuarios = new ListarUsuariosPersonasTableAdapter();
                TAUsuarios.Connection = DAOUtilidades.conexion;
                AccesoDatos.AlvecoComercial10DataSet.ListarUsuariosPersonasDataTable
                DTUsuarios = TAUsuarios.GetData(DIUsuario);
                if (!DTUsuarios[0].IsNumeroAlmacenNull())
                {
                    NumeroAlmacen = DTUsuarios[0].NumeroAlmacen;
                    NumeroAlmacenOriginal = NumeroAlmacen;
                }
                AccesoDatos.AlvecoComercial10DataSetTableAdapters.AlmacenesTableAdapter TAlmacenes = new AlmacenesTableAdapter();
                TAlmacenes.Connection = DAOUtilidades.conexion;
                tslblNumeroAlmacen.Text =  "Almacen :" + TAlmacenes.GetDataBy1(NumeroAlmacen)[0].NombreAlmacen + " Nro:" + NumeroAlmacen.ToString();
                administracionToolStripMenuItem.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Ocurrio la siguiente Excepcion " + ex, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                throw;
            }
            //try
            //{
            //    CLCLN.Sistema.UtilidadesFuncionesCLN _UtilidadesFuncionesCLN = new CLCLN.Sistema.UtilidadesFuncionesCLN(Coneccion);
            //    _UtilidadesFuncionesCLN.ActualizarAutomaticamenteEstadoVigencia();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(this, "Oucirró la Siguiente Excepción " + ex.Message, "Excepción", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            //}
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //FUsuarios formUsuarios = new FUsuarios();
            //formUsuarios.ShowDialog();
            //formUsuarios.Dispose();

            //FAlmacenes formAlmcenes = new FAlmacenes();
            //formAlmcenes.ShowDialog();
            //formAlmcenes.Dispose();

            //FGeografico formAlmcenes = new FGeografico();
            //formAlmcenes.ShowDialog();
            //formAlmcenes.Dispose();

            //FProductosTipos formAlmcenes = new FProductosTipos();
            //formAlmcenes.ShowDialog();
            //formAlmcenes.Dispose();

            //FProductos formProductos = new FProductos();
            //formProductos.ShowDialog();
            //formProductos.Dispose();

            //FInventarioProductos formAlmcenes = new FInventarioProductos();
            //formAlmcenes.ShowDialog();
            //formAlmcenes.Dispose();

            //FComprasProductos formComprasProductos = new FComprasProductos("0000000000", 1);
            //formComprasProductos.cargarDatosCompraProducto(4);
            //formComprasProductos.ShowDialog();
            //formComprasProductos.Dispose();

            //FCuentasPorPagarIA formCuentas = new FCuentasPorPagarIA(1, "0000000000");
            //formCuentas.cargarDatosCuentaPorPagar(1);
            //formCuentas.ShowDialog();
            //formCuentas.Dispose();

            //FComprasProductosAdministrador formAdministradorCompras = new FComprasProductosAdministrador(1, DIUsuario);
            //formAdministradorCompras.formatearParaBusquedasGeneral();
            //formAdministradorCompras.ShowDialog();
            //formAdministradorCompras.Dispose();

            //FComprasProductosCuentasPorPagar formComprasCuentas = new FComprasProductosCuentasPorPagar(1, "0000000000");
            //formComprasCuentas.ShowDialog();
            //formComprasCuentas.Dispose();

            //FVentasProductos formVentasProducto = new FVentasProductos(DIUsuario, 1);
            //formVentasProducto.cargarDatosVentaProducto(2);
            //formVentasProducto.ShowDialog();
            //formVentasProducto.Dispose();


            //FDevolucionesAdministrador formAdministradorCompras = new FDevolucionesAdministrador(1, DIUsuario, "V");
            //formAdministradorCompras.formatearParaBusquedasGeneral();
            //formAdministradorCompras.ShowDialog();
            //formAdministradorCompras.Dispose();

            //FComprasProductosDevolucion formAdministradorCompras = new FComprasProductosDevolucion(DIUsuario, 1);
            //formAdministradorCompras.ShowDialog();
            //formAdministradorCompras.Dispose();

            FVentasProductosDistribucionDatos formAdministrador = new FVentasProductosDistribucionDatos(DIUsuario, NumeroAlmacen, 1);
            formAdministrador.ShowDialog();
            formAdministrador.Dispose();

            //FPersonas formPersonas = new FPersonas();
            //formPersonas.ShowDialog();
            //formPersonas.Dispose();

        }

        private void copiaDeSeguridadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Se encuentra seguro de realizar una Copia de Seguridad de la Base de Datos?", "Copia de Seguridad", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;            
            try
            {
                Utilidades.DAOUtilidades.CrearBackup(Application.StartupPath + "\\Backup");
                MessageBox.Show("Se Realizo Correctamente la copia de seguridad en el directorio " + Application.StartupPath + "\\Backup", "Copia de Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio la siguiente Excepcion " + ex.Message, "Excepción", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void restaurarCopiaDeSeguridadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileBackup = new OpenFileDialog();
            openFileBackup.Title = "Seleccione el Archivo de Respaldo(Backup Base de Datos)";
            openFileBackup.FileName = "Backup Archivo de Respaldo";

            if (openFileBackup.ShowDialog(this) == DialogResult.OK)
            {
                
                if (!System.IO.File.Exists(openFileBackup.FileName))
                {
                    MessageBox.Show(this, "El Archivo que desea restaurar no existe", "Restauración de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                try
                {
                    /*
                    //SqlConnection sqlConnection = new SqlConnection(@"Integrated Security=SSPI; Data Source=(local)\SQLEXPRESS");
                    SqlConnection sqlConnection = Utilidades.DAOUtilidades.conexion;
                    //string consultaSQL = @"RESTORE DATABASE [AlvecoComercial10] FROM  DISK = '" + openFileBackup.FileName + @"' WITH  FILE = 1,  MOVE 'AlvecoComercial10' TO '" + Application.StartupPath + "\\..\\BD\\ONG_1.mdf" + @"', MOVE 'AlvecoComercial10_log' TO '" + Application.StartupPath + "\\..\\BD\\ONG_1.ldf" + "', NOUNLOAD, REPLACE, STATS = 10";
                    string consultaSQL = @"RESTORE DATABASE [AlvecoComercial10] FROM  DISK = '" + openFileBackup.FileName + @"' WITH  FILE = 1, NOUNLOAD, REPLACE, STATS = 10";

                    SqlCommand comand = new SqlCommand(consultaSQL, sqlConnection);
                    comand.ExecuteNonQuery();
                    
                    consultaSQL = "exec dbo.CrearConfiguracionUsuarioInicial";
                    comand = new SqlCommand(consultaSQL, sqlConnection);
                    comand.ExecuteNonQuery();*/


                    SqlConnection sqlConecction = new SqlConnection("Data Source=" + NombreServidor + ";Initial Catalog=master;Persist Security Info=True;User ID=administrador;Password=administrador");
                    sqlConecction.Open();
                    SqlCommand comando = new SqlCommand("RestaurarBaseDatos", sqlConecction);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.Add(new SqlParameter("@NombreArchivoRestaurar", SqlDbType.VarChar, 100));
                    comando.Parameters.Add(new SqlParameter("@DirectorioBackup", SqlDbType.VarChar, 100));
                    comando.Parameters.Add(new SqlParameter("@NombreBaseDatos", SqlDbType.VarChar, 100));

                    string NombreBackup = System.IO.Path.GetFileName(openFileBackup.FileName);
                    string DirectorioBackup = System.IO.Path.GetDirectoryName(openFileBackup.FileName);

                    comando.Parameters["@NombreArchivoRestaurar"].Value = NombreBackup;
                    comando.Parameters["@DirectorioBackup"].Value = DirectorioBackup;
                    comando.Parameters["@NombreBaseDatos"].Value = "AlvecoComercial10";
                    comando.ExecuteNonQuery();

                    //DAOUtilidades.RestaurarBaseDatos(NombreBackup, DirectorioBackup, "AlvecoComercial10");


                    MessageBox.Show("Restauración Correcta");
                    sqlConecction.Close();



                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se Pudo Restaurar Correctamente, seguramente no tiene los permisos suficientes. Ocurrió la siguiente excepción " + ex.Message);
                    return;
                }

                try
                {
                    DAOUtilidades.ObtenerFechaHoraServidor();
                    DAOUtilidades.ObtenerFechaHoraServidor();
                }
                catch (Exception )
                {
                                        
                }
            }
        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FUsuarios formUsuarios = new FUsuarios();
            formUsuarios.ShowDialog();
            formUsuarios.Dispose();
        }

        private void almacenesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FAlmacenes formAlmacenes = new FAlmacenes();
            formAlmacenes.ShowDialog();
            formAlmacenes.Dispose();
        }

        private void salirDelSistemaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void cambiarContraseñaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FCambiarContraseña formCambiarContrasena = new FCambiarContraseña(DIUsuario, NombreUsuario, NombreServidor);
            formCambiarContrasena.ShowDialog();
            formCambiarContrasena.Dispose();
        }

        private void productosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FProductos formProductos = new FProductos(NumeroAlmacen);
            formProductos.ShowDialog();
            formProductos.Dispose();
        }

        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FClientes formClientes = new FClientes();
            formClientes.ShowDialog();
            formClientes.Dispose();
        }

        private void proveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FProveedores formProveedores = new FProveedores();
            formProveedores.ShowDialog();
            formProveedores.Dispose();
        }

        private void administradorDeComprasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FComprasProductosAdministrador formAdministradorCompras = new FComprasProductosAdministrador(NumeroAlmacen, DIUsuario);
            formAdministradorCompras.formatearParaBusquedasGeneral();
            formAdministradorCompras.ShowDialog();
            formAdministradorCompras.Dispose();  
        }

        private void registroDeComprasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FComprasProductos formComrpasProdutos = new FComprasProductos(DIUsuario, NumeroAlmacen);            
            formComrpasProdutos.ShowDialog();
            formComrpasProdutos.Dispose();
        }

        private void cuentasPorPagarPorComprasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FTransaccionesCuentasPorPagarCobrar formCuentasPorPagar = new FTransaccionesCuentasPorPagarCobrar(NumeroAlmacen, DIUsuario, "C");
            formCuentasPorPagar.ShowDialog();
            formCuentasPorPagar.Dispose();
        }

        private void administradorVentasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FVentasProductosAdministrador formVentasAdministrador = new FVentasProductosAdministrador(NumeroAlmacen, DIUsuario);
            formVentasAdministrador.formatearParaBusquedasGeneral();
            formVentasAdministrador.ShowDialog();
            formVentasAdministrador.Dispose();
        }

        private void registroDeVentasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FVentasProductos formVentasProductos = new FVentasProductos(DIUsuario, NumeroAlmacen);
            formVentasProductos.cargarDatosVentaProducto(5);
            formVentasProductos.ShowDialog();
            formVentasProductos.Dispose();
        }

        private void unidadesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FProductosUnidades formUnidades = new FProductosUnidades();
            formUnidades.ShowDialog();
            formUnidades.Dispose();
        }

        private void tiposToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FProductosTipos formTipos = new FProductosTipos();
            formTipos.ShowDialog();
            formTipos.Dispose();
        }

        private void marcasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FProductosMarcas formMarcas = new FProductosMarcas("P");
            formMarcas.ShowDialog();
            formMarcas.Dispose();
        }

        private void kardexDeProductoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FSeleccionProductoRangosFechas formReportes = new FSeleccionProductoRangosFechas(NumeroAlmacen);
            
            AlmacenesTableAdapter TAAlmacenes = new AlmacenesTableAdapter();
            TAAlmacenes.Connection = DAOUtilidades.conexion;
            formReportes.CargarFiltro(TAAlmacenes.GetData(), "NombreAlmacen", "NumeroAlmacen", esUsuarioAdministrador);
            
            if (formReportes.ShowDialog() == DialogResult.OK)
            {
                int NumeroAlmacenSeleccionado = int.Parse(formReportes.SelectedValueFiltro!= null ? formReportes.SelectedValueFiltro.ToString() :  NumeroAlmacen.ToString());
                SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSetTableAdapters.ListarKardexProductoDetalladoReporteTableAdapter TAKardexProducto =
                    new AccesoDatos.AlvecoComercial10DataSetTableAdapters.ListarKardexProductoDetalladoReporteTableAdapter();
                TAKardexProducto.Connection = Utilidades.DAOUtilidades.conexion;
                DataTable DTInventario = TAKardexProducto.GetData(NumeroAlmacenSeleccionado, formReportes.FechaInicio,
                    formReportes.FechaFin, formReportes.ListadoProductos);

                if (DTInventario.Rows.Count == 0)
                {
                    MessageBox.Show(this, "No encontró ningún registro con los datos provistos");
                    return;
                }
                FReporteInventarios formReporteInventario = new FReporteInventarios();
                formReporteInventario.cargarReporteKardexProductos(DTInventario, formReportes.FechaInicio, formReportes.FechaFin, 
                    (formReportes.SelectedValueFiltro != null) ?  (formReportes.SelectedItemFiltro as DataRowView)["NombreAlmacen"].ToString() :"");
                formReporteInventario.ShowDialog();
                formReporteInventario.Dispose();
            }
            formReportes.Dispose();
        }

        private void movimientoDeAlmacenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FSeleccionProductoRangosFechas formReportes = new FSeleccionProductoRangosFechas(NumeroAlmacen);
            
            AlmacenesTableAdapter TAAlmacenes = new AlmacenesTableAdapter();
            TAAlmacenes.Connection = DAOUtilidades.conexion;
            formReportes.CargarFiltro(TAAlmacenes.GetData(), "NombreAlmacen", "NumeroAlmacen", esUsuarioAdministrador);
            
            if (formReportes.ShowDialog() == DialogResult.OK)
            {
                int NumeroAlmacenSeleccionado = int.Parse(formReportes.SelectedValueFiltro != null ? formReportes.SelectedValueFiltro.ToString() : NumeroAlmacen.ToString());
                SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSetTableAdapters.ListarMovimientoProductoReporteTableAdapter TAKardexProducto =
                    new AccesoDatos.AlvecoComercial10DataSetTableAdapters.ListarMovimientoProductoReporteTableAdapter();
                TAKardexProducto.Connection = Utilidades.DAOUtilidades.conexion;
                DataTable DTInventario = TAKardexProducto.GetData(NumeroAlmacenSeleccionado, formReportes.FechaInicio,
                   formReportes.FechaFin, formReportes.ListadoProductos);
                if (DTInventario.Rows.Count == 0)
                {
                    MessageBox.Show(this, "No encontró ningún registro con los datos provistos");
                    return;
                }
                FReporteInventarios formReporteInventario = new FReporteInventarios();
                formReporteInventario.ListarMovimientoArticuloReporte(DTInventario, formReportes.FechaInicio, formReportes.FechaFin,
                     (formReportes.SelectedValueFiltro != null) ? (formReportes.SelectedItemFiltro as DataRowView)["NombreAlmacen"].ToString() : "");
                formReporteInventario.ShowDialog();
                formReporteInventario.Dispose();
            }
            formReportes.Dispose();
        }

        private void movimientoAlmacenPorTipoProductoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FSeleccionProductoRangosFechas formReportes = new FSeleccionProductoRangosFechas(NumeroAlmacen);
            AlmacenesTableAdapter TAAlmacenes = new AlmacenesTableAdapter();
            TAAlmacenes.Connection = DAOUtilidades.conexion;
            formReportes.CargarFiltro(TAAlmacenes.GetData(), "NombreAlmacen", "NumeroAlmacen", esUsuarioAdministrador);

            if (formReportes.ShowDialog() == DialogResult.OK)
            {
                int NumeroAlmacenSeleccionado = int.Parse(formReportes.SelectedValueFiltro != null ? formReportes.SelectedValueFiltro.ToString() : NumeroAlmacen.ToString());

                SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSetTableAdapters.ListarMovimientoProductoPorProductoTipoReporteTableAdapter TAKardexProducto =
                    new AccesoDatos.AlvecoComercial10DataSetTableAdapters.ListarMovimientoProductoPorProductoTipoReporteTableAdapter();
                TAKardexProducto.Connection = Utilidades.DAOUtilidades.conexion;
                DataTable DTInventario = TAKardexProducto.GetData(NumeroAlmacenSeleccionado, formReportes.FechaInicio,
                   formReportes.FechaFin, formReportes.ListadoProductos);
                if (DTInventario.Rows.Count == 0)
                {
                    MessageBox.Show(this, "No encontró ningún registro con los datos provistos");
                    return;
                }
                FReporteInventarios formReporteInventario = new FReporteInventarios();
                formReporteInventario.ListarMovimientoArticuloPorPartidaReporte(DTInventario, formReportes.FechaInicio, formReportes.FechaFin,
                    (formReportes.SelectedValueFiltro != null) ? (formReportes.SelectedItemFiltro as DataRowView)["NombreAlmacen"].ToString() : "");
                formReporteInventario.ShowDialog();
                formReporteInventario.Dispose();
            }
            formReportes.Dispose();
        }

        private void otroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSetTableAdapters.ListarInventarioProductosReportesTableAdapter TAInventarioProductos =
                new AccesoDatos.AlvecoComercial10DataSetTableAdapters.ListarInventarioProductosReportesTableAdapter();
            TAInventarioProductos.Connection = Utilidades.DAOUtilidades.conexion;            
            DataTable DTInventario = TAInventarioProductos.GetData(esUsuarioAdministrador ? (int?)null : NumeroAlmacen);
            if (DTInventario.Rows.Count == 0)
            {
                MessageBox.Show(this, "No existen datos para mostrar en el informe");
                return;
            }
            FReporteInventarios formReporteInventario = new FReporteInventarios();
            formReporteInventario.cargarReporteInventarioGeneral(DTInventario);
            formReporteInventario.ShowDialog();
            formReporteInventario.Dispose();
        }

        private void stockMinimoDeProductosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSetTableAdapters.ListarInventarioProductosRequeridosReportesTableAdapter TAInventarioProductos =
                new AccesoDatos.AlvecoComercial10DataSetTableAdapters.ListarInventarioProductosRequeridosReportesTableAdapter();
            TAInventarioProductos.Connection = Utilidades.DAOUtilidades.conexion;            
            DataTable DTInventario = TAInventarioProductos.GetData( esUsuarioAdministrador ? (int?)null : NumeroAlmacen);
            if (DTInventario.Rows.Count == 0)
            {
                MessageBox.Show(this, "No existen datos para mostrar en el informe");
                return;
            }
            FReporteInventarios formReporteInventario = new FReporteInventarios();
            formReporteInventario.ListarArticulosRequeridos(DTInventario);
            formReporteInventario.ShowDialog();
            formReporteInventario.Dispose();
        }

        private void administradorInventariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FInventarioProductos formInventario = new FInventarioProductos(NumeroAlmacen);
            formInventario.ShowDialog();
            formInventario.Dispose();
        }

        private void aToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FGeografico formGeografico = new FGeografico();
            formGeografico.seleccionarPestaniaPais();
            formGeografico.ShowDialog();
        }

        private void departamentosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FGeografico fgeografico = new FGeografico(Utilidades.DAOUtilidades.conexion);
            fgeografico.cargarAutomaticamente = true;
            fgeografico.seleccionarPestaniaDepartamento();
            fgeografico.CargarPaises();
            
            fgeografico.ShowDialog(this);
            fgeografico.Dispose();
        }

        private void provinciasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FGeografico fgeografico = new FGeografico(Utilidades.DAOUtilidades.conexion);
            fgeografico.cargarAutomaticamente = true;
            fgeografico.seleccionarPestaniaProvincia();
            fgeografico.CargarPaisesP();            
            fgeografico.ShowDialog(this);
            fgeografico.Dispose();
        }

        private void lugaresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FGeografico fgeografico = new FGeografico(Utilidades.DAOUtilidades.conexion);
            fgeografico.cargarAutomaticamente = true;
            fgeografico.seleccionarPestaniaLugar();
            fgeografico.CargarPaisesL();            
            fgeografico.ShowDialog(this);
        }

        private void administracionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FGeografico formGeografico = new FGeografico(Utilidades.DAOUtilidades.conexion);
            formGeografico.ShowDialog();
            formGeografico.Dispose();
        }

        private void administradorDeDevolucionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FDevolucionesAdministrador formAdministradorCompras = new FDevolucionesAdministrador(NumeroAlmacen, DIUsuario, "C");
            formAdministradorCompras.formatearParaBusquedasGeneral();
            formAdministradorCompras.ShowDialog();
            formAdministradorCompras.Dispose();
        }

        private void devolucionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FVentasProductosDevolucion formVentasProductosDevolucion = new FVentasProductosDevolucion(DIUsuario, NumeroAlmacen);
            formVentasProductosDevolucion.ShowDialog();
            formVentasProductosDevolucion.Dispose();
        }

        private void administradorDeDevolucionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FDevolucionesAdministrador formAdministradorVentas = new FDevolucionesAdministrador(NumeroAlmacen, DIUsuario, "V");
            formAdministradorVentas.formatearParaBusquedasGeneral();
            formAdministradorVentas.ShowDialog();
            formAdministradorVentas.Dispose();
        }

        private void devoluciónDeComprasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FComprasProductosDevolucion formComprasProductosDevolucion = new FComprasProductosDevolucion(DIUsuario, NumeroAlmacen);
            formComprasProductosDevolucion.ShowDialog();
            formComprasProductosDevolucion.Dispose();
        }

        private void administradorCuentasPorPagarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FCuentasPorPagarAdministrador formCuentasPorPagarAdministrador = new FCuentasPorPagarAdministrador(NumeroAlmacen, DIUsuario);
            formCuentasPorPagarAdministrador.ShowDialog(this);
            formCuentasPorPagarAdministrador.Dispose();
        }

        private void administradorCuentasPorCobrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FCuentasPorCobrarAdministrador formCuentasPorCobrarAdministrador = new FCuentasPorCobrarAdministrador(NumeroAlmacen, DIUsuario);
            formCuentasPorCobrarAdministrador.ShowDialog(this);
            formCuentasPorCobrarAdministrador.Dispose();
        }

        private void cuentasPorCobrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FTransaccionesCuentasPorPagarCobrar formCuentasPorPagar = new FTransaccionesCuentasPorPagarCobrar(NumeroAlmacen, DIUsuario, "V");
            formCuentasPorPagar.ShowDialog();
            formCuentasPorPagar.Dispose();
        }

        private void movilidadesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FMovilidades formMovilidades = new FMovilidades();
            formMovilidades.ShowDialog();
            formMovilidades.Dispose();
        }

        private void tiposMovilidadesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FMovilidadesModelos formMovilidades = new FMovilidadesModelos();
            formMovilidades.ShowDialog();
            formMovilidades.Dispose();
        }

        private void productosEnEsperaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListarProductosEnTransitoPorPedidoTableAdapter TAListarProductosEnTransitoPorPedido = new ListarProductosEnTransitoPorPedidoTableAdapter();
            TAListarProductosEnTransitoPorPedido.Connection = Utilidades.DAOUtilidades.conexion;
            DataTable DTListarProductosEnTransitoPorPedido = TAListarProductosEnTransitoPorPedido.GetData(esUsuarioAdministrador ? (int?)null : NumeroAlmacen);
            FReporteMovimientoTransacciones formMercaderiaEnTransito = new FReporteMovimientoTransacciones();
            formMercaderiaEnTransito.ListarProductosEnTransitoPorPedido(DTListarProductosEnTransitoPorPedido);
            formMercaderiaEnTransito.ShowDialog();
            formMercaderiaEnTransito.Dispose();
        }

        private void listaEnRangoDeFechasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FParametrosRangoFechas formParametrosFechas = new FParametrosRangoFechas();
            if (formParametrosFechas.ShowDialog() == DialogResult.OK)
            {
                ListarComprasProductosReportesPorFechasTipoTableAdapter TAListarComprasProductosReportesPorFechasTipo = new ListarComprasProductosReportesPorFechasTipoTableAdapter();
                TAListarComprasProductosReportesPorFechasTipo.Connection = Utilidades.DAOUtilidades.conexion;
                DataTable DTListarComprasProductosReportesPorFechasTipo = TAListarComprasProductosReportesPorFechasTipo.GetData(esUsuarioAdministrador ? (int?)null : NumeroAlmacen,
                    formParametrosFechas.FechaHoraInicio, formParametrosFechas.FechaHoraFin, true);
                FReporteMovimientoTransacciones formMercaderiaEnTransito = new FReporteMovimientoTransacciones();
                formMercaderiaEnTransito.ListarComprasProductosReportesPorFechasTipo(DTListarComprasProductosReportesPorFechasTipo,
                    formParametrosFechas.FechaHoraInicio, formParametrosFechas.FechaHoraFin, true);
                formMercaderiaEnTransito.ShowDialog();
                formMercaderiaEnTransito.Dispose();
            }
            formParametrosFechas.Dispose();
        }

        private void listadoPorTiposProductosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FParametrosRangoFechas formParametrosFechas = new FParametrosRangoFechas();
            if (formParametrosFechas.ShowDialog() == DialogResult.OK)
            {
                ListarComprasProductosReportesPorFechasTipoTableAdapter TAListarComprasProductosReportesPorFechasTipo = new ListarComprasProductosReportesPorFechasTipoTableAdapter();
                TAListarComprasProductosReportesPorFechasTipo.Connection = Utilidades.DAOUtilidades.conexion;
                DataTable DTListarComprasProductosReportesPorFechasTipo = TAListarComprasProductosReportesPorFechasTipo.GetData(esUsuarioAdministrador ? (int?)null : NumeroAlmacen,
                    formParametrosFechas.FechaHoraInicio, formParametrosFechas.FechaHoraFin, false);
                FReporteMovimientoTransacciones formMercaderiaEnTransito = new FReporteMovimientoTransacciones();
                formMercaderiaEnTransito.ListarComprasProductosReportesPorFechasTipo(DTListarComprasProductosReportesPorFechasTipo,
                    formParametrosFechas.FechaHoraInicio, formParametrosFechas.FechaHoraFin, false);
                formMercaderiaEnTransito.ShowDialog();
                formMercaderiaEnTransito.Dispose();
            }
            formParametrosFechas.Dispose();
        }

        private void listadoPorProveedorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FParametrosRangoFechas formParametrosFechas = new FParametrosRangoFechas();
            if (formParametrosFechas.ShowDialog() == DialogResult.OK)
            {
                ListarComprasProductosReportesPorFechasProveedorTableAdapter TAListarComprasProductosReportesPorFechasProveedor = new ListarComprasProductosReportesPorFechasProveedorTableAdapter();
                TAListarComprasProductosReportesPorFechasProveedor.Connection = Utilidades.DAOUtilidades.conexion;
                DataTable DTListarComprasProductosReportesPorFechasTipo = TAListarComprasProductosReportesPorFechasProveedor.GetData(esUsuarioAdministrador ? (int?)null : NumeroAlmacen,
                    formParametrosFechas.FechaHoraInicio, formParametrosFechas.FechaHoraFin, true);
                FReporteMovimientoTransacciones formMercaderiaEnTransito = new FReporteMovimientoTransacciones();
                formMercaderiaEnTransito.ListarComprasProductosReportesPorFechasProveedor(DTListarComprasProductosReportesPorFechasTipo,
                    formParametrosFechas.FechaHoraInicio, formParametrosFechas.FechaHoraFin, true);
                formMercaderiaEnTransito.ShowDialog();
                formMercaderiaEnTransito.Dispose();
            }
            formParametrosFechas.Dispose();
        }

        private void listadoPorProveedorYTiposProductosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FParametrosRangoFechas formParametrosFechas = new FParametrosRangoFechas();
            if (formParametrosFechas.ShowDialog() == DialogResult.OK)
            {
                ListarComprasProductosReportesPorFechasProveedorTableAdapter TAListarComprasProductosReportesPorFechasProveedor = new ListarComprasProductosReportesPorFechasProveedorTableAdapter();
                TAListarComprasProductosReportesPorFechasProveedor.Connection = Utilidades.DAOUtilidades.conexion;
                DataTable DTListarComprasProductosReportesPorFechasTipo = TAListarComprasProductosReportesPorFechasProveedor.GetData(esUsuarioAdministrador ? (int?)null : NumeroAlmacen,
                    formParametrosFechas.FechaHoraInicio, formParametrosFechas.FechaHoraFin, false);
                FReporteMovimientoTransacciones formMercaderiaEnTransito = new FReporteMovimientoTransacciones();
                formMercaderiaEnTransito.ListarComprasProductosReportesPorFechasProveedor(DTListarComprasProductosReportesPorFechasTipo,
                    formParametrosFechas.FechaHoraInicio, formParametrosFechas.FechaHoraFin, false);
                formMercaderiaEnTransito.ShowDialog();
                formMercaderiaEnTransito.Dispose();
            }
            formParametrosFechas.Dispose();
        }

        private void cuentasPorPagarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListarCompraProductoCuentasPorPagarReporteTableAdapter TAListarCompraProductoCuentasPorPagarReporte = new ListarCompraProductoCuentasPorPagarReporteTableAdapter();
            TAListarCompraProductoCuentasPorPagarReporte.Connection = Utilidades.DAOUtilidades.conexion;
            if (MessageBox.Show("¿Desea que el Informe agrupe la infomración por algún proveedor?", "Informe de Compras", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.No)
            {
                FParametrosRangoFechas formParametrosFechas = new FParametrosRangoFechas();
                if (formParametrosFechas.ShowDialog() == DialogResult.OK)
                {
                    DataTable ListarCompraProductoCuentasPorPagarReporte = TAListarCompraProductoCuentasPorPagarReporte.GetData(esUsuarioAdministrador ? (int?)null : NumeroAlmacen,
                        null, formParametrosFechas.FechaHoraInicio, formParametrosFechas.FechaHoraFin, null);
                    FReporteMovimientoTransacciones formCuentaPorPagar = new FReporteMovimientoTransacciones();
                    formCuentaPorPagar.ListarCompraProductoCuentasPorPagarReporte(ListarCompraProductoCuentasPorPagarReporte,
                        formParametrosFechas.FechaHoraInicio, formParametrosFechas.FechaHoraFin);
                    formCuentaPorPagar.ShowDialog();
                    formCuentaPorPagar.Dispose();
                }
                formParametrosFechas.Dispose();
            }
            else
            {
                FSeleccionFechasReportes formSeleccionParametros = new FSeleccionFechasReportes();
                ProveedoresTableAdapter TAProveedores = new ProveedoresTableAdapter();
                TAProveedores.Connection = DAOUtilidades.conexion;
                formSeleccionParametros.cargarDatosFiltro(TAProveedores.GetDataByActivos(), "NombreRazonSocial", "CodigoProveedor");
                formSeleccionParametros.setVisibilidadFiltro(true);
                formSeleccionParametros.LabelFiltro.Text = "Proveedores[Opcional]";
                if (formSeleccionParametros.ShowDialog() == DialogResult.OK)
                {
                    DataTable DTListarCompraProductoCuentasPorPagarReporte = TAListarCompraProductoCuentasPorPagarReporte.GetData(esUsuarioAdministrador ? (int?)null : NumeroAlmacen,
                        null, formSeleccionParametros.FechaInicio, formSeleccionParametros.FechaFin, null);
                    DataTable DTDatosFiltro;
                    if (formSeleccionParametros.SelectedValueFiltro != null)
                    {
                        DTDatosFiltro = DTListarCompraProductoCuentasPorPagarReporte.Clone();
                        foreach (DataRow DRDatos in DTListarCompraProductoCuentasPorPagarReporte.Select(" CodigoProveedor = " + formSeleccionParametros.SelectedValueFiltro.ToString(), ""))
                        {
                            DTDatosFiltro.Rows.Add(DRDatos.ItemArray);
                        }
                    }
                    else
                        DTDatosFiltro = DTListarCompraProductoCuentasPorPagarReporte;

                    FReporteMovimientoTransacciones formCuentasPorPagar = new FReporteMovimientoTransacciones();
                    formCuentasPorPagar.ListarCompraProductoCuentasPorPagarReportePorProveedor(DTDatosFiltro,
                        formSeleccionParametros.FechaInicio, formSeleccionParametros.FechaFin);
                    formCuentasPorPagar.ShowDialog();
                    formCuentasPorPagar.Dispose();
                }

            }
        }

        private void listadoEnRangoDeFechasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListarVentasProductosReportesPorFechasTipoTableAdapter TAListarVentasProductosReportesPorFechasTipo = new ListarVentasProductosReportesPorFechasTipoTableAdapter();
            TAListarVentasProductosReportesPorFechasTipo.Connection = Utilidades.DAOUtilidades.conexion;
            FParametrosRangoFechas formParametrosFechas = new FParametrosRangoFechas();
            if (formParametrosFechas.ShowDialog() == DialogResult.OK)
            {
                DataTable DTListarComprasProductosReportesPorFechasTipo = TAListarVentasProductosReportesPorFechasTipo.GetData(esUsuarioAdministrador ? (int?)null : NumeroAlmacen,
                    formParametrosFechas.FechaHoraInicio, formParametrosFechas.FechaHoraFin, true);
                FReporteMovimientoTransacciones formMercaderiaEnTransito = new FReporteMovimientoTransacciones();
                formMercaderiaEnTransito.ListarVentasProductosReportesPorFechasTipo(DTListarComprasProductosReportesPorFechasTipo,
                    formParametrosFechas.FechaHoraInicio, formParametrosFechas.FechaHoraFin, true);
                formMercaderiaEnTransito.ShowDialog();
                formMercaderiaEnTransito.Dispose();
            }
            formParametrosFechas.Dispose();
        }

        private void listadoPorTiposProductosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ListarVentasProductosReportesPorFechasTipoTableAdapter TAListarVentasProductosReportesPorFechasTipo = new ListarVentasProductosReportesPorFechasTipoTableAdapter();
            TAListarVentasProductosReportesPorFechasTipo.Connection = Utilidades.DAOUtilidades.conexion;
            FParametrosRangoFechas formParametrosFechas = new FParametrosRangoFechas();
            if (formParametrosFechas.ShowDialog() == DialogResult.OK)
            {
                DataTable DTListarComprasProductosReportesPorFechasTipo = TAListarVentasProductosReportesPorFechasTipo.GetData(esUsuarioAdministrador ? (int?)null : NumeroAlmacen,
                    formParametrosFechas.FechaHoraInicio, formParametrosFechas.FechaHoraFin, false);
                FReporteMovimientoTransacciones formMercaderiaEnTransito = new FReporteMovimientoTransacciones();
                formMercaderiaEnTransito.ListarVentasProductosReportesPorFechasTipo(DTListarComprasProductosReportesPorFechasTipo,
                    formParametrosFechas.FechaHoraInicio, formParametrosFechas.FechaHoraFin, false);
                formMercaderiaEnTransito.ShowDialog();
                formMercaderiaEnTransito.Dispose();
            }
            formParametrosFechas.Dispose();
        }

        private void listadoPorClientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListarVentasProductosReportesPorFechasClienteTableAdapter TAListarVentasProductosReportesPorFechasCliente = new ListarVentasProductosReportesPorFechasClienteTableAdapter();
            TAListarVentasProductosReportesPorFechasCliente.Connection = Utilidades.DAOUtilidades.conexion;
            FParametrosRangoFechas formParametrosFechas = new FParametrosRangoFechas();
            if (formParametrosFechas.ShowDialog() == DialogResult.OK)
            {
                DataTable DTListarComprasProductosReportesPorFechasTipo = TAListarVentasProductosReportesPorFechasCliente.GetData(esUsuarioAdministrador ? (int?)null : NumeroAlmacen,
                    formParametrosFechas.FechaHoraInicio, formParametrosFechas.FechaHoraFin, true);
                FReporteMovimientoTransacciones formMercaderiaEnTransito = new FReporteMovimientoTransacciones();
                formMercaderiaEnTransito.ListarVentasProductosReportesPorFechasCliente(DTListarComprasProductosReportesPorFechasTipo,
                    formParametrosFechas.FechaHoraInicio, formParametrosFechas.FechaHoraFin, true);
                formMercaderiaEnTransito.ShowDialog();
                formMercaderiaEnTransito.Dispose();
            }
            formParametrosFechas.Dispose();
        }

        private void listadoPorClientesYTiposProductosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListarVentasProductosReportesPorFechasClienteTableAdapter TAListarVentasProductosReportesPorFechasCliente = new ListarVentasProductosReportesPorFechasClienteTableAdapter();
            TAListarVentasProductosReportesPorFechasCliente.Connection = Utilidades.DAOUtilidades.conexion;
            FParametrosRangoFechas formParametrosFechas = new FParametrosRangoFechas();
            if (formParametrosFechas.ShowDialog() == DialogResult.OK)
            {
                DataTable DTListarComprasProductosReportesPorFechasTipo = TAListarVentasProductosReportesPorFechasCliente.GetData(esUsuarioAdministrador ? (int?)null : NumeroAlmacen,
                    formParametrosFechas.FechaHoraInicio, formParametrosFechas.FechaHoraFin, false);
                FReporteMovimientoTransacciones formMercaderiaEnTransito = new FReporteMovimientoTransacciones();
                formMercaderiaEnTransito.ListarVentasProductosReportesPorFechasCliente(DTListarComprasProductosReportesPorFechasTipo,
                    formParametrosFechas.FechaHoraInicio, formParametrosFechas.FechaHoraFin, false);
                formMercaderiaEnTransito.ShowDialog();
                formMercaderiaEnTransito.Dispose();
            }
            formParametrosFechas.Dispose();
        }

        private void personasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FPersonas formPersonas = new FPersonas();
            formPersonas.ShowDialog();
            formPersonas.Dispose();
        }

        private void distribucionDeProductosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FSeleccionFechasReportes formSeleccionParametros = new FSeleccionFechasReportes();
            ListarUsuariosPersonasTableAdapter TAUsuarios = new ListarUsuariosPersonasTableAdapter();
            ListarVentasProductosPersonasDistribucionTableAdapter TAListarVentasProductosPersonasDistribucion = new ListarVentasProductosPersonasDistribucionTableAdapter();
            TAUsuarios.Connection = DAOUtilidades.conexion;
            TAListarVentasProductosPersonasDistribucion.Connection = DAOUtilidades.conexion;
            DataTable DTUsuario = TAUsuarios.GetData(null);
            formSeleccionParametros.cargarDatosFiltro(DTUsuario, "NombreCompleto", "DIPersona");
            formSeleccionParametros.setVisibilidadFiltro(true);
            formSeleccionParametros.LabelFiltro.Text = "Usuario[Opcional]";
            if (formSeleccionParametros.ShowDialog() == DialogResult.OK)
            {
                string CodigoUsuario = formSeleccionParametros.SelectedValueFiltro != null ? formSeleccionParametros.SelectedValueFiltro.ToString() : null;
                DataTable DTListarVentasProductosPersonasDistribucion = TAListarVentasProductosPersonasDistribucion.GetData(esUsuarioAdministrador ? (int?)null : NumeroAlmacen,
                    formSeleccionParametros.FechaInicio, formSeleccionParametros.FechaFin, CodigoUsuario, null);
               
                FReporteMovimientoTransacciones formCuentasPorPagar = new FReporteMovimientoTransacciones();
                formCuentasPorPagar.ListarVentasProductosPersonasDistribucion(DTListarVentasProductosPersonasDistribucion,
                    formSeleccionParametros.FechaInicio, formSeleccionParametros.FechaFin, true);
                formCuentasPorPagar.ShowDialog();
                formCuentasPorPagar.Dispose();
            }
            formSeleccionParametros.Dispose();
        }

        private void distribucionDeProductosPorClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FSeleccionFechasReportes formSeleccionParametros = new FSeleccionFechasReportes();
            ClientesTableAdapter TAUsuarios = new ClientesTableAdapter();
            ListarVentasProductosPersonasDistribucionTableAdapter TAListarVentasProductosPersonasDistribucion = new ListarVentasProductosPersonasDistribucionTableAdapter();
            TAUsuarios.Connection = DAOUtilidades.conexion;
            TAListarVentasProductosPersonasDistribucion.Connection = DAOUtilidades.conexion;
            DataTable DTUsuario = TAUsuarios.GetData();
            formSeleccionParametros.cargarDatosFiltro(DTUsuario, "NombreCliente", "CodigoCliente");
            formSeleccionParametros.setVisibilidadFiltro(true);
            formSeleccionParametros.LabelFiltro.Text = "Cliente[Opcional]";
            if (formSeleccionParametros.ShowDialog() == DialogResult.OK)
            {
                int? CodigoCliente = formSeleccionParametros.SelectedValueFiltro != null ? int.Parse(formSeleccionParametros.SelectedValueFiltro.ToString()) : (int?)null;
                DataTable DTListarVentasProductosPersonasDistribucion = TAListarVentasProductosPersonasDistribucion.GetData(esUsuarioAdministrador ? (int?)null : NumeroAlmacen,
                    formSeleccionParametros.FechaInicio, formSeleccionParametros.FechaFin, null, CodigoCliente);

                FReporteMovimientoTransacciones formCuentasPorPagar = new FReporteMovimientoTransacciones();
                formCuentasPorPagar.ListarVentasProductosPersonasDistribucion(DTListarVentasProductosPersonasDistribucion,
                    formSeleccionParametros.FechaInicio, formSeleccionParametros.FechaFin, false);
                formCuentasPorPagar.ShowDialog();
                formCuentasPorPagar.Dispose();
            }
            formSeleccionParametros.Dispose();
        }

        private void listadoPorUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FSeleccionFechasReportes formSeleccionParametros = new FSeleccionFechasReportes();
            ListarUsuariosPersonasTableAdapter TAUsuarios = new ListarUsuariosPersonasTableAdapter();
            ListarVentasProductosUsuariosTableAdapter TAListarVentasProductosPersonasDistribucion = new ListarVentasProductosUsuariosTableAdapter();
            TAUsuarios.Connection = DAOUtilidades.conexion;
            TAListarVentasProductosPersonasDistribucion.Connection = DAOUtilidades.conexion;
            DataTable DTUsuario = TAUsuarios.GetData(null);
            formSeleccionParametros.cargarDatosFiltro(DTUsuario, "NombreCompleto", "DIPersona");
            formSeleccionParametros.setVisibilidadFiltro(true);
            formSeleccionParametros.LabelFiltro.Text = "Usuario[Opcional]";
            if (formSeleccionParametros.ShowDialog() == DialogResult.OK)
            {
                string CodigoUsuario = formSeleccionParametros.SelectedValueFiltro != null ? formSeleccionParametros.SelectedValueFiltro.ToString() : null;
                DataTable DTListarVentasProductosPersonasDistribucion = TAListarVentasProductosPersonasDistribucion.GetData(esUsuarioAdministrador ? (int?)null : NumeroAlmacen,
                    formSeleccionParametros.FechaInicio, formSeleccionParametros.FechaFin, CodigoUsuario);

                FReporteMovimientoTransacciones formCuentasPorPagar = new FReporteMovimientoTransacciones();
                formCuentasPorPagar.ListarVentasProductosUsuarios(DTListarVentasProductosPersonasDistribucion,
                    formSeleccionParametros.FechaInicio, formSeleccionParametros.FechaFin, true);
                formCuentasPorPagar.ShowDialog();
                formCuentasPorPagar.Dispose();
            }
            formSeleccionParametros.Dispose();
        }

        private void resumenMovimientoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ListarVentasProductosUsuariosTableAdapter TAListarVentasProductosPersonasDistribucion = new ListarVentasProductosUsuariosTableAdapter();
            TAListarVentasProductosPersonasDistribucion.Connection = Utilidades.DAOUtilidades.conexion;
            FParametrosRangoFechas formParametrosFechas = new FParametrosRangoFechas();
            if (formParametrosFechas.ShowDialog() == DialogResult.OK)
            {
                DataTable DTListarComprasProductosReportesPorFechasTipo = TAListarVentasProductosPersonasDistribucion.GetData(esUsuarioAdministrador ? (int?)null : NumeroAlmacen,
                    formParametrosFechas.FechaHoraInicio, formParametrosFechas.FechaHoraFin, null);
                FReporteMovimientoTransacciones formMercaderiaEnTransito = new FReporteMovimientoTransacciones();
                formMercaderiaEnTransito.ListarVentasProductosUsuarios(DTListarComprasProductosReportesPorFechasTipo,
                    formParametrosFechas.FechaHoraInicio, formParametrosFechas.FechaHoraFin, false);
                formMercaderiaEnTransito.ShowDialog();
                formMercaderiaEnTransito.Dispose();
            }
            formParametrosFechas.Dispose();


        }

        private void listadoPorUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FSeleccionFechasReportes formSeleccionParametros = new FSeleccionFechasReportes();
            ListarUsuariosPersonasTableAdapter TAUsuarios = new ListarUsuariosPersonasTableAdapter();
            ListarComprasProductosUsuariosTableAdapter TAListarComprasProductosPersonasDistribucion = new ListarComprasProductosUsuariosTableAdapter();
            TAUsuarios.Connection = DAOUtilidades.conexion;
            TAListarComprasProductosPersonasDistribucion.Connection = DAOUtilidades.conexion;
            DataTable DTUsuario = TAUsuarios.GetData(null);
            formSeleccionParametros.cargarDatosFiltro(DTUsuario, "NombreCompleto", "DIPersona");
            formSeleccionParametros.setVisibilidadFiltro(true);
            formSeleccionParametros.LabelFiltro.Text = "Usuario[Opcional]";
            if (formSeleccionParametros.ShowDialog() == DialogResult.OK)
            {
                string CodigoUsuario = formSeleccionParametros.SelectedValueFiltro != null ? formSeleccionParametros.SelectedValueFiltro.ToString() : null;
                DataTable DTListarComprasProductosPersonasDistribucion = TAListarComprasProductosPersonasDistribucion.GetData(esUsuarioAdministrador ? (int?)null : NumeroAlmacen,
                    formSeleccionParametros.FechaInicio, formSeleccionParametros.FechaFin, CodigoUsuario);

                FReporteMovimientoTransacciones formCuentasPorPagar = new FReporteMovimientoTransacciones();
                formCuentasPorPagar.ListarComprasProductosUsuarios(DTListarComprasProductosPersonasDistribucion,
                    formSeleccionParametros.FechaInicio, formSeleccionParametros.FechaFin, true);
                formCuentasPorPagar.ShowDialog();
                formCuentasPorPagar.Dispose();
            }
            formSeleccionParametros.Dispose();
        }

        private void resumenMovimientoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListarComprasProductosUsuariosTableAdapter TAListarComprasProductosPersonasDistribucion = new ListarComprasProductosUsuariosTableAdapter();
            TAListarComprasProductosPersonasDistribucion.Connection = Utilidades.DAOUtilidades.conexion;
            FParametrosRangoFechas formParametrosFechas = new FParametrosRangoFechas();
            if (formParametrosFechas.ShowDialog() == DialogResult.OK)
            {
                DataTable DTListarComprasProductosReportesPorFechasTipo = TAListarComprasProductosPersonasDistribucion.GetData(esUsuarioAdministrador ? (int?)null : NumeroAlmacen,
                    formParametrosFechas.FechaHoraInicio, formParametrosFechas.FechaHoraFin, null);
                FReporteMovimientoTransacciones formMercaderiaEnTransito = new FReporteMovimientoTransacciones();
                formMercaderiaEnTransito.ListarComprasProductosUsuarios(DTListarComprasProductosReportesPorFechasTipo,
                    formParametrosFechas.FechaHoraInicio, formParametrosFechas.FechaHoraFin, false);
                formMercaderiaEnTransito.ShowDialog();
                formMercaderiaEnTransito.Dispose();
            }
            formParametrosFechas.Dispose();
        }

        private void porUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FSeleccionFechasReportes formSeleccionParametros = new FSeleccionFechasReportes();
            ListarUsuariosPersonasTableAdapter TAUsuarios = new ListarUsuariosPersonasTableAdapter();
            ListarMovimientoMonetarioTableAdapter TAListarMovimientoMonetario = new ListarMovimientoMonetarioTableAdapter();
            TAUsuarios.Connection = DAOUtilidades.conexion;
            TAListarMovimientoMonetario.Connection = DAOUtilidades.conexion;
            DataTable DTUsuario = TAUsuarios.GetData(null);
            formSeleccionParametros.cargarDatosFiltro(DTUsuario, "NombreCompleto", "DIPersona");
            formSeleccionParametros.setVisibilidadFiltro(true);
            formSeleccionParametros.LabelFiltro.Text = "Usuario[Opcional]";
            if (formSeleccionParametros.ShowDialog() == DialogResult.OK)
            {
                string CodigoUsuario = formSeleccionParametros.SelectedValueFiltro != null ? formSeleccionParametros.SelectedValueFiltro.ToString() : null;
                DataTable DTTAListarMovimientoMonetario = TAListarMovimientoMonetario.GetData( esUsuarioAdministrador ? (int?)null : NumeroAlmacen, CodigoUsuario, 
                    formSeleccionParametros.FechaInicio, formSeleccionParametros.FechaFin);

                FReporteCuentas formCuentasPorPagar = new FReporteCuentas();
                formCuentasPorPagar.ListarMovimientoMonetario(DTTAListarMovimientoMonetario,
                    formSeleccionParametros.FechaInicio, formSeleccionParametros.FechaFin, true);
                formCuentasPorPagar.ShowDialog();
                formCuentasPorPagar.Dispose();
            }
            formSeleccionParametros.Dispose();
        }

        private void generalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListarMovimientoMonetarioTableAdapter TAListarMovimientoMonetario = new ListarMovimientoMonetarioTableAdapter();
            TAListarMovimientoMonetario.Connection = Utilidades.DAOUtilidades.conexion;
            FParametrosRangoFechas formParametrosFechas = new FParametrosRangoFechas();
            if (formParametrosFechas.ShowDialog() == DialogResult.OK)
            {
                DataTable DTListarComprasProductosReportesPorFechasTipo = TAListarMovimientoMonetario.GetData(NumeroAlmacen,
                    null, formParametrosFechas.FechaHoraInicio, formParametrosFechas.FechaHoraFin);
                FReporteCuentas formMercaderiaEnTransito = new FReporteCuentas();
                formMercaderiaEnTransito.ListarMovimientoMonetario(DTListarComprasProductosReportesPorFechasTipo,
                    formParametrosFechas.FechaHoraInicio, formParametrosFechas.FechaHoraFin, false);
                formMercaderiaEnTransito.ShowDialog();
                formMercaderiaEnTransito.Dispose();
            }
            formParametrosFechas.Dispose();
        }

        private void tiposDeProductoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListarProductosPorTipoTableAdapter TAListarProductosPorTipo = new ListarProductosPorTipoTableAdapter();
            TAListarProductosPorTipo.Connection = Utilidades.DAOUtilidades.conexion;
            DataTable DTListarProductosPorTipo = TAListarProductosPorTipo.GetData(NumeroAlmacen, null);
            FReporteDatosAdministracion formMercaderiaEnTransito = new FReporteDatosAdministracion();
            formMercaderiaEnTransito.ListarProductosPorTipo(DTListarProductosPorTipo);
            formMercaderiaEnTransito.ShowDialog();
            formMercaderiaEnTransito.Dispose();
        }

        private void conceptosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FConceptos formConceptos = new FConceptos();
            formConceptos.ShowDialog();
            formConceptos.Dispose();
        }  

        private void porDistribuidorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FSeleccionFechasReportes formSeleccionParametros = new FSeleccionFechasReportes();
            PersonasTableAdapter TAUsuarios = new PersonasTableAdapter();
            ListarMovimientoMonetarioVentasDistribuiblesTableAdapter TAListarMovimientoMonetarioVentasDistribuibles = new ListarMovimientoMonetarioVentasDistribuiblesTableAdapter();
            TAUsuarios.Connection = DAOUtilidades.conexion;
            TAListarMovimientoMonetarioVentasDistribuibles.Connection = DAOUtilidades.conexion;
            DataTable DTUsuario = TAUsuarios.GetDataByParticulares(null);
            formSeleccionParametros.cargarDatosFiltro(DTUsuario, "NombreCompleto", "DIPersona");
            formSeleccionParametros.setVisibilidadFiltro(true);
            formSeleccionParametros.LabelFiltro.Text = "Distribuidor[Opcional]";
            if (formSeleccionParametros.ShowDialog() == DialogResult.OK)
            {
                string CodigoUsuario = formSeleccionParametros.SelectedValueFiltro != null ? formSeleccionParametros.SelectedValueFiltro.ToString() : null;
                DataTable DTListarMovimientoMonetarioVentasDistribuibles = TAListarMovimientoMonetarioVentasDistribuibles.GetData(NumeroAlmacen, CodigoUsuario,
                    formSeleccionParametros.FechaInicio, formSeleccionParametros.FechaFin);

                FReporteCuentas formCuentasPorPagar = new FReporteCuentas();
                formCuentasPorPagar.ListarMovimientoMonetarioVentasDistribuibles(DTListarMovimientoMonetarioVentasDistribuibles,
                    formSeleccionParametros.FechaInicio, formSeleccionParametros.FechaFin);
                formCuentasPorPagar.ShowDialog();
                formCuentasPorPagar.Dispose();
            }
            formSeleccionParametros.Dispose();
        }

        private void devolucionesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FSeleccionFechasReportes formSeleccionParametros = new FSeleccionFechasReportes();
            ListarUsuariosPersonasTableAdapter TAUsuarios = new ListarUsuariosPersonasTableAdapter();
            ListarMovimientoDevolucionesTableAdapter TAListarMovimientoMonetarioVentasDistribuibles = new ListarMovimientoDevolucionesTableAdapter();
            TAUsuarios.Connection = DAOUtilidades.conexion;
            TAListarMovimientoMonetarioVentasDistribuibles.Connection = DAOUtilidades.conexion;
            DataTable DTUsuario = TAUsuarios.GetData(null);
            formSeleccionParametros.cargarDatosFiltro(DTUsuario, "NombreCompleto", "DIPersona");
            formSeleccionParametros.setVisibilidadFiltro(true);
            formSeleccionParametros.LabelFiltro.Text = "Usuario[Opcional]";
            if (formSeleccionParametros.ShowDialog() == DialogResult.OK)
            {
                string CodigoUsuario = formSeleccionParametros.SelectedValueFiltro != null ? formSeleccionParametros.SelectedValueFiltro.ToString() : null;
                DataTable DTListarMovimientoDevoluciones = TAListarMovimientoMonetarioVentasDistribuibles.GetData(NumeroAlmacen, CodigoUsuario,
                    formSeleccionParametros.FechaInicio, formSeleccionParametros.FechaFin);

                FReporteDatosAdministracion formCuentasPorPagar = new FReporteDatosAdministracion();
                formCuentasPorPagar.ListarMovimientoDevoluciones(DTListarMovimientoDevoluciones,
                    formSeleccionParametros.FechaInicio, formSeleccionParametros.FechaFin);
                formCuentasPorPagar.ShowDialog();
                formCuentasPorPagar.Dispose();
            }
            formSeleccionParametros.Dispose();
        }

        private void bitacoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FSeleccionFechasReportes formSeleccionParametros = new FSeleccionFechasReportes();
            ListarUsuariosPersonasTableAdapter TAUsuarios = new ListarUsuariosPersonasTableAdapter();
            ListarBitacoraReporteTableAdapter TAListarBitacoraReporte = new ListarBitacoraReporteTableAdapter();
            TAUsuarios.Connection = DAOUtilidades.conexion;
            TAListarBitacoraReporte.Connection = DAOUtilidades.conexion;
            DataTable DTUsuario = TAUsuarios.GetData(null);
            formSeleccionParametros.cargarDatosFiltro(DTUsuario, "NombreCompleto", "DIPersona");
            formSeleccionParametros.setVisibilidadFiltro(true);
            formSeleccionParametros.LabelFiltro.Text = "Usuario[Opcional]";
            if (formSeleccionParametros.ShowDialog() == DialogResult.OK)
            {
                string CodigoUsuario = formSeleccionParametros.SelectedValueFiltro != null ? formSeleccionParametros.SelectedValueFiltro.ToString() : null;
                DataTable DTListarBitacoraReporte = TAListarBitacoraReporte.GetData(NumeroAlmacen, CodigoUsuario,
                    formSeleccionParametros.FechaInicio, formSeleccionParametros.FechaFin, "AlvecoComercial10");

                FReporteDatosAdministracion formCuentasPorPagar = new FReporteDatosAdministracion();
                formCuentasPorPagar.ListarBitacoraReporte(DTListarBitacoraReporte,
                    formSeleccionParametros.FechaInicio, formSeleccionParametros.FechaFin);
                formCuentasPorPagar.ShowDialog();
                formCuentasPorPagar.Dispose();
            }
            formSeleccionParametros.Dispose();
        }

        private void cuentasPorCobrarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ListarVentaProductoCuentasPorCobrarReporteTableAdapter TAListarVentaProductoCuentasPorCobrarReporte = new ListarVentaProductoCuentasPorCobrarReporteTableAdapter();
            TAListarVentaProductoCuentasPorCobrarReporte.Connection = Utilidades.DAOUtilidades.conexion;
            if (MessageBox.Show("¿Desea que el Informe agrupe la infomración por algún cliente?", "Informe de Ventas", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.No)
            {
                FParametrosRangoFechas formParametrosFechas = new FParametrosRangoFechas();
                if (formParametrosFechas.ShowDialog() == DialogResult.OK)
                {
                    DataTable ListarVentaProductoCuentasPorCobrarReporte = TAListarVentaProductoCuentasPorCobrarReporte.GetData(NumeroAlmacen,
                        null, formParametrosFechas.FechaHoraInicio, formParametrosFechas.FechaHoraFin, null);
                    FReporteMovimientoTransacciones formCuentaPorCobrar = new FReporteMovimientoTransacciones();
                    
                    formCuentaPorCobrar.ListarVentaProductoCuentasPorCobrarReporte(ListarVentaProductoCuentasPorCobrarReporte,
                        formParametrosFechas.FechaHoraInicio, formParametrosFechas.FechaHoraFin);
                    
                    formCuentaPorCobrar.ShowDialog();
                    formCuentaPorCobrar.Dispose();
                }
                formParametrosFechas.Dispose();
            }
            else
            {
                FSeleccionFechasReportes formSeleccionParametros = new FSeleccionFechasReportes();
                ClientesTableAdapter TAClientes = new ClientesTableAdapter();
                TAClientes.Connection = DAOUtilidades.conexion;
                formSeleccionParametros.cargarDatosFiltro(TAClientes.GetDataByActivos(), "NombreCliente", "CodigoCliente");
                formSeleccionParametros.setVisibilidadFiltro(true);
                formSeleccionParametros.LabelFiltro.Text = "Clientes[Opcional]";
                if (formSeleccionParametros.ShowDialog() == DialogResult.OK)
                {
                    DataTable DTListarVentaProductoCuentasPorCobrarReporte = TAListarVentaProductoCuentasPorCobrarReporte.GetData(NumeroAlmacen,
                        null, formSeleccionParametros.FechaInicio, formSeleccionParametros.FechaFin, null);
                    DataTable DTDatosFiltro;
                    if (formSeleccionParametros.SelectedValueFiltro != null)
                    {
                        DTDatosFiltro = DTListarVentaProductoCuentasPorCobrarReporte.Clone();
                        foreach (DataRow DRDatos in DTListarVentaProductoCuentasPorCobrarReporte.Select(" CodigoCliente = " + formSeleccionParametros.SelectedValueFiltro.ToString(), ""))
                        {
                            DTDatosFiltro.Rows.Add(DRDatos.ItemArray);
                        }
                    }
                    else
                        DTDatosFiltro = DTListarVentaProductoCuentasPorCobrarReporte;

                    FReporteMovimientoTransacciones formCuentasPorCobrar = new FReporteMovimientoTransacciones();
                    formCuentasPorCobrar.ListarVentaProductoCuentasPorCobrarReportePorProveedor(DTDatosFiltro,
                        formSeleccionParametros.FechaInicio, formSeleccionParametros.FechaFin);
                    formCuentasPorCobrar.ShowDialog();
                    formCuentasPorCobrar.Dispose();
                }

            }
        }

        private void toolStripMenuItem37_Click(object sender, EventArgs e)
        {
            FInventarioProductos formInventario = new FInventarioProductos(NumeroAlmacen);
            formInventario.ShowDialog();
            formInventario.Dispose();
        }

        private void registroDeTransferenciaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FTransferenciasProductos formTransferencia = new FTransferenciasProductos(DIUsuario, NumeroAlmacen);
            formTransferencia.ShowDialog();
            formTransferencia.Dispose();
        }

        private void administradorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FTransferenciasProductosAdministrador formTransferencia = new FTransferenciasProductosAdministrador(NumeroAlmacen, DIUsuario);
            formTransferencia.formatearParaBusquedasGeneral();
            formTransferencia.ShowDialog();
            formTransferencia.Dispose();
        }

        private void cambiarDeAlmacenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FSeleccionFechasReportes formSeleccionParametros = new FSeleccionFechasReportes();
            
            AlmacenesTableAdapter TAlmacenes = new AlmacenesTableAdapter();
            TAlmacenes.Connection = DAOUtilidades.conexion;
            formSeleccionParametros.cargarDatosFiltro(TAlmacenes.GetData(), "NombreAlmacen", "NumeroAlmacen");            
            formSeleccionParametros.LabelFiltro.Text = "Almacen";
            formSeleccionParametros.configurarSoloFiltro("Seleccione Almacen");
            if (formSeleccionParametros.ShowDialog() == DialogResult.OK)
            {
                if (formSeleccionParametros.SelectedValueFiltro != null)
                {
                    int NumeroAlmacenTemp = int.Parse(formSeleccionParametros.SelectedValueFiltro.ToString());
                    NumeroAlmacen = NumeroAlmacenTemp;
                    if (NumeroAlmacenOriginal == NumeroAlmacenTemp) // si el almacen es el original
                    {
                        habilitarMenusAdministrador(true);
                        esUsuarioAdministrador = true;
                    }
                    else
                    {
                        habilitarMenusAdministrador(false);
                        esUsuarioAdministrador = false;
                    }
                    tslblNumeroAlmacen.Text = "Almacen :" + TAlmacenes.GetDataBy1(NumeroAlmacen)[0].NombreAlmacen + " Nro:" + NumeroAlmacen.ToString();
                }
            }
        }

        public void habilitarMenusAdministrador(bool estadoHabilitacion)
        {
            toolStripMenuItem37.Visible = herramientasGestiónToolStripMenuItem.Visible = estadoHabilitacion;
        }

        private void movimientoAlmacenPorProductoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FSeleccionFechasReportes formSeleccionParametros = new FSeleccionFechasReportes();
            AlmacenesTableAdapter TAAlmacenes = new AlmacenesTableAdapter();
            ListarMovimientoTransaccionalProductosTableAdapter TAListarVentasProductosPersonasDistribucion = new ListarMovimientoTransaccionalProductosTableAdapter();
            TAAlmacenes.Connection = DAOUtilidades.conexion;
            TAListarVentasProductosPersonasDistribucion.Connection = DAOUtilidades.conexion;
            DataTable DTUsuario = TAAlmacenes.GetData();
            formSeleccionParametros.cargarDatosFiltro(DTUsuario, "NombreAlmacen", "NumeroAlmacen");
            formSeleccionParametros.setVisibilidadFiltro(true);
            formSeleccionParametros.LabelFiltro.Text = "Almacen";
            if (formSeleccionParametros.ShowDialog() == DialogResult.OK)
            {
                int NumeroAlmacenAux = formSeleccionParametros.SelectedValueFiltro != null ? int.Parse(formSeleccionParametros.SelectedValueFiltro.ToString()) : NumeroAlmacen;
                DataTable DTListarVentasProductosPersonasDistribucion = TAListarVentasProductosPersonasDistribucion.GetData(NumeroAlmacenAux,
                    formSeleccionParametros.FechaInicio, formSeleccionParametros.FechaFin);

                FReporteInventarios formCuentasPorPagar = new FReporteInventarios();
                formCuentasPorPagar.ListarMovimientoTransaccionalProductos(DTListarVentasProductosPersonasDistribucion,
                    formSeleccionParametros.FechaInicio, formSeleccionParametros.FechaFin, TAAlmacenes.GetDataBy1(NumeroAlmacenAux)[0].NombreAlmacen);
                formCuentasPorPagar.ShowDialog();
                formCuentasPorPagar.Dispose();
            }
            formSeleccionParametros.Dispose();
        }

        private void ventasConObservacionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FSeleccionFechasReportes formSeleccionParametros = new FSeleccionFechasReportes();
            AlmacenesTableAdapter TAAlmacenes = new AlmacenesTableAdapter();
            ListarVentasMotivosDaniosTableAdapter TAListarVentasProductosPersonasDistribucion = new ListarVentasMotivosDaniosTableAdapter();
            TAAlmacenes.Connection = DAOUtilidades.conexion;
            TAListarVentasProductosPersonasDistribucion.Connection = DAOUtilidades.conexion;
            DataTable DTUsuario = TAAlmacenes.GetData();
            formSeleccionParametros.cargarDatosFiltro(DTUsuario, "NombreAlmacen", "NumeroAlmacen");
            formSeleccionParametros.setVisibilidadFiltro(true);
            formSeleccionParametros.LabelFiltro.Text = "Almacen";
            if (formSeleccionParametros.ShowDialog() == DialogResult.OK)
            {
                int NumeroAlmacenAux = formSeleccionParametros.SelectedValueFiltro != null ? int.Parse(formSeleccionParametros.SelectedValueFiltro.ToString()) : NumeroAlmacen;
                DataTable DTListarVentasProductosPersonasDistribucion = TAListarVentasProductosPersonasDistribucion.GetData(NumeroAlmacenAux,
                    formSeleccionParametros.FechaInicio, formSeleccionParametros.FechaFin);

                FReporteInventarios formCuentasPorPagar = new FReporteInventarios();
                formCuentasPorPagar.ListarVentasMotivosDanios(DTListarVentasProductosPersonasDistribucion,
                    formSeleccionParametros.FechaInicio, formSeleccionParametros.FechaFin, TAAlmacenes.GetDataBy1(NumeroAlmacenAux)[0].NombreAlmacen);
                formCuentasPorPagar.ShowDialog();
                formCuentasPorPagar.Dispose();
            }
            formSeleccionParametros.Dispose();
        }

        






    }
}
