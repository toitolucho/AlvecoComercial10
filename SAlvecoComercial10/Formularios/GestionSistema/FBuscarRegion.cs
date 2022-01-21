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
    public partial class FBuscarRegion : Form
    {
        private PaisesCLN Paises = new PaisesCLN();
        private DepartamentosCLN Departamentos = new DepartamentosCLN();
        private ProvinciasCLN Provincias = new ProvinciasCLN();
        private LugaresCLN Lugares = new LugaresCLN();
        private DataTable BRegiones = new DataTable();
        public string CodigoDepartamento = "";
        public string CodigoPais = "";
        public string CodigoProvincia = "";
        public string CodigoLugar = "";
        public string vRegion = "";


        public FBuscarRegion()
        {
            InitializeComponent();
        }

        private void CargarPaises()
        {
            DataTable DTPaises = new DataTable();
            DTPaises = Paises.ListarPaises();
            DataRow RPais = DTPaises.NewRow();
            RPais["CodigoPais"] = "01";
            RPais["NombrePais"] = "TODOS";
            RPais["Nacionalidad"] = "TODOS";
            DTPaises.Rows.Add(RPais);
            cBNombrePais.DataSource = DTPaises.DefaultView;
            cBNombrePais.DisplayMember = "NombrePais";
            cBNombrePais.ValueMember = "CodigoPais";

        }
        private void CargarDepartamentos(string CodigoPais)
        {
            DataTable DTDepartamentos = new DataTable();
            DTDepartamentos = Departamentos.ObtenerDepartamentosPorPais(CodigoPais);
            DataRow RDepartamento = DTDepartamentos.NewRow();
            RDepartamento["CodigoPais"] = "01";
            RDepartamento["CodigoDepartamento"] = "01";
            RDepartamento["NombreDepartamento"] = "TODOS";
            DTDepartamentos.Rows.Add(RDepartamento);
            cBNombreDepartamento.DataSource = DTDepartamentos.DefaultView;
            cBNombreDepartamento.DisplayMember = "NombreDepartamento";
            cBNombreDepartamento.ValueMember = "CodigoDepartamento";

        }

        private void CargarProvincias(string CodigoPais, string CodigoDepartamento)
        {
            DataTable DTProvincias = new DataTable();
            DTProvincias = Provincias.ObtenerProvinciasPorDepartamento(CodigoPais, CodigoDepartamento);
            DataRow RProvincia = DTProvincias.NewRow();
            RProvincia["CodigoPais"] = "01";
            RProvincia["CodigoDepartamento"] = "01";
            RProvincia["CodigoProvincia"] = "01";
            RProvincia["NombreProvincia"] = "TODOS";
            DTProvincias.Rows.Add(RProvincia);
            cBNombreProvincia.DataSource = DTProvincias.DefaultView;
            cBNombreProvincia.DisplayMember = "NombreProvincia";
            cBNombreProvincia.ValueMember = "CodigoProvincia";
        }


        private void bBuscarP_Click(object sender, EventArgs e)
        {

        }

        private void dGVResultadosBusqueda_DoubleClick(object sender, EventArgs e)
        {

        }

        private void FBuscarRegion_Load(object sender, EventArgs e)
        {
            cBBuscarpor.SelectedIndex = 1;
            CargarPaises();
            cBNombrePais.SelectedValue = "01";

        }

        private void cBNombrePais_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void cBNombreDepartamento_SelectedValueChanged(object sender, EventArgs e)
        {

        }


        private void cBUbicaciongeografica_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
