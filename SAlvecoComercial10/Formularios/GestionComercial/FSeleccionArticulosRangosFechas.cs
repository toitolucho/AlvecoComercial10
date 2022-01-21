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

namespace SAlvecoComercial10.Formularios.GestionComercial
{
    public partial class FSeleccionProductoRangosFechas : Form
    {
        ListarProductosSeleccionTableAdapter TAListarProductosSeleccion;
        AlvecoComercial10DataSet.ListarProductosSeleccionDataTable DTProductos;
        private int NumeroAlmacen;
        public string ListadoProductos{get; set;}
        public DateTime FechaInicio {get;set;}
        public DateTime FechaFin {get; set;}
        public object SelectedValueFiltro
        {
            get { return cBoxFiltro.SelectedValue; }
        }

        public object SelectedItemFiltro
        {
            get { return cBoxFiltro.SelectedItem; }
        }

        DgvFilterPopup.DgvFilterManager DGVFilterManger;
        public FSeleccionProductoRangosFechas(int NumeroAlmacen)
        {
            InitializeComponent();
            DGVFilterManger = new DgvFilterPopup.DgvFilterManager();
            this.NumeroAlmacen = NumeroAlmacen;
            TAListarProductosSeleccion = new ListarProductosSeleccionTableAdapter();
            TAListarProductosSeleccion.Connection = Utilidades.DAOUtilidades.conexion;
            DTProductos = TAListarProductosSeleccion.GetData(NumeroAlmacen);
            dtGVProductos.DataSource = DTProductos;
            DGVFilterManger.DataGridView = dtGVProductos;
            dtGVProductos.CurrentCellDirtyStateChanged += new EventHandler(dtGVProductos_CurrentCellDirtyStateChanged);

            DateTime FechaInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            dateTimePicker1.Value = FechaInicio;
            lblFiltro.Visible = cBoxFiltro.Visible = false;
            
        }

        void dtGVProductos_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dtGVProductos.IsCurrentCellDirty && dtGVProductos.CurrentCell.ColumnIndex == 2)
            {
                dtGVProductos.CommitEdit(DataGridViewDataErrorContexts.Commit);
            } 
        }

        public void CargarFiltro(DataTable DTFiltro, string DisplayMember, string ValueMember, bool visualizar)
        {
            cBoxFiltro.DataSource = DTFiltro;
            cBoxFiltro.DisplayMember = DisplayMember;
            cBoxFiltro.ValueMember = ValueMember;
            cBoxFiltro.SelectedIndex = -1;
            lblFiltro.Visible = cBoxFiltro.Visible = visualizar;
            if (!visualizar)
                cBoxFiltro.SelectedValue = NumeroAlmacen;
        }

        private void checkSeleccionarTodos_CheckedChanged(object sender, EventArgs e)
        {
            foreach (AlvecoComercial10DataSet.ListarProductosSeleccionRow DRProducto
                in DTProductos.Rows)
            {
                DRProducto.Seleccionar = false;
            }


            foreach (DataGridViewRow DRProductos in dtGVProductos.Rows)
            {
                DRProductos.Cells["DGCSeleccionar"].Value = checkSeleccionarTodos.Checked;
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ListadoProductos = null;
            DTProductos.AcceptChanges();
            AlvecoComercial10DataSet.ListarProductosSeleccionRow[] DTProductosSeleccionados = (AlvecoComercial10DataSet.ListarProductosSeleccionRow[])DTProductos.Select("Seleccionar = True");
            if (DTProductosSeleccionados.Length == DTProductos.Count)
            {
                ListadoProductos = null;
            }
            else if (DTProductos.Select("Seleccionar = False").Length == 0)
            {
                if (MessageBox.Show(this, "Aún no ha Seleccionado un Producto para ser visualizado, ¿Desea ver el Reporte de Todos los Productos?", "Productos No Seleccionados", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ListadoProductos = null;
                }
                else
                { return; }
            }
            else
            {
                foreach (AlvecoComercial10DataSet.ListarProductosSeleccionRow DRProductos in DTProductosSeleccionados)
                {
                    ListadoProductos += "'" + DRProductos.CodigoProducto + "', ";
                }
            }
            ListadoProductos = ListadoProductos == null ? null : ListadoProductos.Substring(0, ListadoProductos.LastIndexOf(',')).TrimEnd();
            this.FechaFin = dateTimePicker2.Value;
            this.FechaInicio = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, dateTimePicker1.Value.Day, 0,0,0);
            
            this.DialogResult = DialogResult.OK;

            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
