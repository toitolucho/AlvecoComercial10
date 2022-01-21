using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAlvecoComercial10.Formularios.Utilidades
{
    class ObjetoCodigoDescripcion
    {
        public string ValueMember{get; set;}
        public string DisplayMember { get; set; }

        public string codigoObjeto { get; set; }
        public string descripcionObjeto { get; set; }

        public List<ObjetoCodigoDescripcion> listaObjetos { get; set; }

        public ObjetoCodigoDescripcion()
        {
            this.ValueMember = "codigoObjeto";
            this.DisplayMember = "descripcionObjeto";
            listaObjetos = new List<ObjetoCodigoDescripcion>();
        }

        public ObjetoCodigoDescripcion(string codigo, string descripcion)
        {
            this.codigoObjeto = codigo;
            this.descripcionObjeto = descripcion;
        }

        public void cargarDatosSexo()
        {
            listaObjetos.Clear();
            listaObjetos.Add(new ObjetoCodigoDescripcion("M","MASCULINO"));
            listaObjetos.Add(new ObjetoCodigoDescripcion("F", "FEMENINO"));
        }

        public void cargarMotivosVentas()
        {
            listaObjetos.Clear();
            listaObjetos.Add(new ObjetoCodigoDescripcion("N", "NORMAL"));
            listaObjetos.Add(new ObjetoCodigoDescripcion("D", "DAÑADOS"));
            listaObjetos.Add(new ObjetoCodigoDescripcion("P", "PERDIDOS"));
            listaObjetos.Add(new ObjetoCodigoDescripcion("V", "VENCIDOS"));
            listaObjetos.Add(new ObjetoCodigoDescripcion("O", "OTROS"));
        }

        public void cargarDatosEstadoCivil()
        {
            listaObjetos.Clear();
            listaObjetos.Add(new ObjetoCodigoDescripcion("S","SOLTERO(A)"));
            listaObjetos.Add(new ObjetoCodigoDescripcion("C", "CASADO(A)"));
            listaObjetos.Add(new ObjetoCodigoDescripcion("D", "DIVORCIADO(A)"));
            listaObjetos.Add(new ObjetoCodigoDescripcion("V", "VIUDO(A)"));
        }

        public void cargarDatosTipoCalculoInventario()
        {
            listaObjetos.Clear();
            //listaObjetos.Add(new ObjetoCodigoDescripcion("P", "PEPS"));
            //listaObjetos.Add(new ObjetoCodigoDescripcion("U", "UEPS"));
            listaObjetos.Add(new ObjetoCodigoDescripcion("O", "PONDERADO"));
            //listaObjetos.Add(new ObjetoCodigoDescripcion("B", "PRECIO MAS BAJO"));
            //listaObjetos.Add(new ObjetoCodigoDescripcion("A", "PRECIO MAS ALTO"));
            
        }

        public void cargarDatosEstadoProveedores()
        {
            listaObjetos.Clear();
            listaObjetos.Add(new ObjetoCodigoDescripcion("A", "ACTIVO"));
            listaObjetos.Add(new ObjetoCodigoDescripcion("B", "DE BAJA"));
        }

        public void cargarDatosTiposClientesProveedores()
        {
            listaObjetos.Clear();
            listaObjetos.Add(new ObjetoCodigoDescripcion("P", "PERSONA"));
            listaObjetos.Add(new ObjetoCodigoDescripcion("E", "EMPRESA"));
        }

        public void cargarDatosTiposComprasVentas()
        {
            listaObjetos.Clear();
            listaObjetos.Add(new ObjetoCodigoDescripcion("R", "CREDITO"));
            listaObjetos.Add(new ObjetoCodigoDescripcion("E", "EFECTIVO"));
        }

        public void cargarDatosEstadosCuentasPorPagarPorCobrar()
        {
            listaObjetos.Clear();
            listaObjetos.Add(new ObjetoCodigoDescripcion("P", "PAGADO"));
            listaObjetos.Add(new ObjetoCodigoDescripcion("D", "PENDIENTE"));
        }

        public void cargarDatosEstadosComprasVentas()
        {
            listaObjetos.Clear();
            listaObjetos.Add(new ObjetoCodigoDescripcion("I", "INICIADA"));
            listaObjetos.Add(new ObjetoCodigoDescripcion("F", "FINALIZADA"));
            listaObjetos.Add(new ObjetoCodigoDescripcion("D", "PENDIENTE"));
            listaObjetos.Add(new ObjetoCodigoDescripcion("A", "ANULADA"));
            listaObjetos.Add(new ObjetoCodigoDescripcion("X", "FINALIZADA INCOMPLETA"));
        }

        public void cargarDatosEstadosComprasVentasBusqueda()
        {

            listaObjetos.Clear();
            listaObjetos.Add(new ObjetoCodigoDescripcion("I", "INICIADA"));
            listaObjetos.Add(new ObjetoCodigoDescripcion("F", "FINALIZADA"));
            listaObjetos.Add(new ObjetoCodigoDescripcion("D", "PENDIENTE"));
            listaObjetos.Add(new ObjetoCodigoDescripcion("A", "ANULADA"));
            listaObjetos.Add(new ObjetoCodigoDescripcion("X", "FINALIZADA INCOMPLETA"));
            listaObjetos.Add(new ObjetoCodigoDescripcion("T", "TODAS"));
        }

        public void cargarDatosEstadosDevoluciones()
        {

            listaObjetos.Clear();
            listaObjetos.Add(new ObjetoCodigoDescripcion("I", "INICIADA"));
            listaObjetos.Add(new ObjetoCodigoDescripcion("F", "FINALIZADA"));            
            listaObjetos.Add(new ObjetoCodigoDescripcion("A", "ANULADA"));            
            listaObjetos.Add(new ObjetoCodigoDescripcion("T", "TODAS"));
        }

        public void cargarDatosEstadosTransaccionesDevoluciones()
        {
            listaObjetos.Clear();
            listaObjetos.Add(new ObjetoCodigoDescripcion("F", "FINALIZADA"));
            listaObjetos.Add(new ObjetoCodigoDescripcion("X", "FINALIZADA INCOMPLETA"));
        }

        public void cargarTiposUsuarios()
        {
            listaObjetos.Clear();
            listaObjetos.Add(new ObjetoCodigoDescripcion("A", "ADMINISTRADOR"));
            listaObjetos.Add(new ObjetoCodigoDescripcion("V", "ENCARGADO DE VENTAS"));
            listaObjetos.Add(new ObjetoCodigoDescripcion("M", "ENCARGADO DE ALMACENES"));
            //listaObjetos.Add(new ObjetoCodigoDescripcion("O", "OTRO TIPO DE USUARIO"));
        }

        public override string ToString()
        {
            return descripcionObjeto;
        }
    
    }
}
