using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAlvecoComercial10.Formularios.Utilidades
{
    class DAOUtilidades
    {
        public static System.Data.SqlClient.SqlConnection conexion;
        private static DateTime _fechaHoraSeridor;
        private static SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSetTableAdapters.QTAUtilidadesFunciones qtAUtilidadesFunciones;

        public static DateTime FechaHoraServidor
        {
            get
            {
                if(_fechaHoraSeridor == null)
                    _fechaHoraSeridor = _QTAUtilidadesFunciones.ObtenerFechaHoraServidor().Value;
                return _fechaHoraSeridor;
            }
            set { _fechaHoraSeridor = value; }
        }
        public static SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSetTableAdapters.QTAUtilidadesFunciones _QTAUtilidadesFunciones
        {
            get{
                if (qtAUtilidadesFunciones == null)
                    qtAUtilidadesFunciones = new AccesoDatos.AlvecoComercial10DataSetTableAdapters.QTAUtilidadesFunciones();
                return qtAUtilidadesFunciones;
            }
            set
            {
                qtAUtilidadesFunciones = value;
            }
        }


        public static void cargarConeccion(System.Data.SqlClient.SqlDataAdapter TableAdapter, System.Data.SqlClient.SqlConnection conexion)
        {
        }

        public static void cargarConeccion(ref SAlvecoComercial10.AccesoDatos.AlvecoComercial10DataSetTableAdapters.QTAUtilidadesFunciones _QTAUtilidadesFunciones,
            System.Data.SqlClient.SqlConnection conexion)
        {
            foreach (System.Data.SqlClient.SqlCommand comando in _QTAUtilidadesFunciones.CommandCollection)
            {
                comando.Connection = conexion;
            }
        }

        public static void cargarConeccion(System.Data.SqlClient.SqlConnection conexion)
        {
            foreach (System.Data.SqlClient.SqlCommand comando in _QTAUtilidadesFunciones.CommandCollection)
            {
                comando.Connection = conexion;
            }
        }

        public static string GenerarCodigoDepartamento(string CodigoPais)
        {
            return _QTAUtilidadesFunciones.GenerarCodigoDepartamento(CodigoPais);
        }

        public static string GenerarCodigoProvincia(string CodigoPais, string CodigoDepartamento)
        {
            return _QTAUtilidadesFunciones.GenerarCodigoProvincia(CodigoPais, CodigoDepartamento);
        }

        public static string GenerarCodigoLugar(string CodigoPais, string CodigoDepartamento, string CodigoProvincia)
        {
            return _QTAUtilidadesFunciones.GenerarCodigoLugar(CodigoPais, CodigoDepartamento, CodigoProvincia);
        }

        public static int ObtenerCantidadExistenciaProducto(int NumeroAlmacen, string CodigoProducto)
        {
            return _QTAUtilidadesFunciones.ObtenerCantidadExistenciaProducto(NumeroAlmacen, CodigoProducto).Value;
        }


        public static int ObtenerEdad(DateTime FechaNacimiento, DateTime FechaActual)
        {
            return _QTAUtilidadesFunciones.ObtenerEdad(FechaNacimiento, FechaActual).Value;
        }

        public static DateTime ObtenerFechaHoraServidor()
        {
            _fechaHoraSeridor = _QTAUtilidadesFunciones.ObtenerFechaHoraServidor().Value;
            return _fechaHoraSeridor;
        }

        public static string ObtenerNombreCompleto(string DIPersona)
        {
            return _QTAUtilidadesFunciones.ObtenerNombreCompleto(DIPersona);
        }

        public static int ObtenerUltimoIndiceTabla(string NombreTabla)
        {
            int? indice = 0;
            _QTAUtilidadesFunciones.ObtenerUltimoIndiceTabla(NombreTabla, ref indice);
            return indice == null ? -1 : indice.Value;
        }

        public static string VerificarUsuario(string nombreLogin, string password)
        {
            return _QTAUtilidadesFunciones.VerificarUsuario(nombreLogin, password);
        }

        public static string ObtenerCodigoEstadoActualTransacciones(int NumeroAlmacen, int NumeroTransaccion, string TipoTransaccion)
        {
            return _QTAUtilidadesFunciones.ObtenerCodigoEstadoActualTransacciones(NumeroAlmacen, NumeroTransaccion, TipoTransaccion);
        }

        public static void CrearBackup(string Path)
        {
            _QTAUtilidadesFunciones.CrearBackup(Path);
        }

        public static string ObtenerCodigoProductoGenerado()
        {
            return _QTAUtilidadesFunciones.ObtenerCodigoProductoGenerado();
        }

        public static string EsPosibleCulminarDevolucion(
            int NumeroAlmacenDevolucion,
            int NumeroTransaccionDev,
            string TipoTransaccionDev
            )
        {
            return _QTAUtilidadesFunciones.EsPosibleCulminarDevolucion(NumeroAlmacenDevolucion, NumeroTransaccionDev, TipoTransaccionDev);
        }

        public static string obtenerSignificadoEstadoCompra(string estadoCompra)
        {
            //CASE(CodigoEstadoCompra) WHEN ''I'' THEN ''INICIADA'' WHEN ''A'' THEN ''ANULADA'' WHEN ''P'' THEN ''PAGADA'' WHEN ''D'' THEN ''PENDIENTE''  WHEN ''F'' THEN ''FINALIZADO Y RECIBIDO''
            switch (estadoCompra)
            {
                case "I":
                    return "INICIADA";
                case "A":
                    return "ANULADA";
                case "P":
                    return "EN TRANSITO";
                case "D":
                    return "PENDIENTE";
                case "F":
                    return "FINALIZADA Y DISTRIBUIDA";
                case "X":
                    return "FINALIZADA INCOMPLETA";
                default:
                    return "INDETERMINADO";
            }
        }

        public static string GenerarCodigoPais()
        {
            return _QTAUtilidadesFunciones.GenerarCodigoPais();
        }

        public static string ObtenerCodigoMovilidadGenerado()
        {
            return _QTAUtilidadesFunciones.ObtenerCodigoMovilidadGenerado();
        }

        public static int ObtenerExistenciaActualInventarios(int NumeroAlmacen, string CodigoProducto)
        {
            return _QTAUtilidadesFunciones.ObtenerCantidadExistenciaProducto(NumeroAlmacen, CodigoProducto).Value;
        }

        public static string GenerarCodigoProductoTipo()
        {
            return _QTAUtilidadesFunciones.GenerarCodigoProductoTipo();
        }

        public static bool EsPosibleModificarMontoPagoVenta(int NumeroAlmacen, int NumeroVentaProducto, decimal MontoNuevo)
        {
            return _QTAUtilidadesFunciones.EsPosibleModificarMontoPagoVenta(NumeroAlmacen, NumeroVentaProducto, MontoNuevo).Value;
        }

        public static void RestaurarBaseDatos(string NombreArchivoRestaurar, string DirectorioBackup, string NombreBaseDatos)
        {
            _QTAUtilidadesFunciones.RestaurarBaseDatos(NombreArchivoRestaurar, DirectorioBackup, NombreBaseDatos);
        }

        public static DateTime FormatearFecha(bool esInicio)
        {
            DateTime FechaHoraServidor = ObtenerFechaHoraServidor();
            if (esInicio)
                return new DateTime(FechaHoraServidor.Year, FechaHoraServidor.Month, FechaHoraServidor.Day);
            else
            {
                return new DateTime(FechaHoraServidor.Year, FechaHoraServidor.Month, FechaHoraServidor.Day, 23, 59, 59);
            }

        }
    }
}
