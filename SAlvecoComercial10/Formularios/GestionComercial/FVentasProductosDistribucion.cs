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
using SAlvecoComercial10.Formularios.Utilidades;

namespace SAlvecoComercial10.Formularios.GestionComercial
{
    public partial class FVentasProductosDistribucion : Form
    {
        AlvecoComercial10DataSet.ListarProductosDistribucionFaltantesDataTable DTProductosDistribucion;
        AlvecoComercial10DataSet.DTProductosRecepcionEntregaDataTable DTProductosSeleccionados;
        AlvecoComercial10DataSet.DTProductosRecepcionEntregaEspecificosDataTable DTProductosSeleccionadosEspecificos;


        VentasProductosDetalleEntregaTableAdapter TAVentasProductosDetalleEntrega;
        ListarProductosDistribucionFaltantesTableAdapter TAListarProductosDistribucionFaltantes;
        VentasProductosTableAdapter TAVentasProductos;
        InventariosProductosCantidadesTransaccionesHistorialTableAdapter TAInventariosProductosCantidadesTransaccionesHistorial;
        ListarProductosCantidadSuperaStockMinimoXMLTableAdapter TAListarProductosCantidadSuperaStockMinimo;
        ListarProductosExistenciaInsuficienteXMLTableAdapter TAListarProductosExistenciaInsuficiente;

        ListarProductosDistribucionFaltantesTableAdapter TAListarProductosRecepcionFaltantes;
         
        

        int NumeroAlmacen;
        int NumeroEquipo;
        string DIUsuario;
        int NumeroVentaProducto;
        
        public DateTime FechaHoraEntrega = DateTime.Now;

        bool llenadoConfirmadoPE = false;
        Font fuenteDefecto;
        FIngresarCantidad formIngresarCantidad;

        public FVentasProductosDistribucion(int NumeroAlmacen, int NumeroEquipo, string CodigoUsuario, int NumeroVentaProducto)
        {
            InitializeComponent();
            DTProductosDistribucion = new AlvecoComercial10DataSet.ListarProductosDistribucionFaltantesDataTable();
            this.NumeroAlmacen = NumeroAlmacen;
            this.NumeroEquipo = NumeroEquipo;
            this.DIUsuario = CodigoUsuario;
            this.NumeroVentaProducto = NumeroVentaProducto;

            TAVentasProductosDetalleEntrega = new VentasProductosDetalleEntregaTableAdapter();
            TAVentasProductosDetalleEntrega.Connection = Utilidades.DAOUtilidades.conexion;
            TAListarProductosDistribucionFaltantes = new ListarProductosDistribucionFaltantesTableAdapter();
            TAListarProductosDistribucionFaltantes.Connection = Utilidades.DAOUtilidades.conexion;
            TAVentasProductos = new VentasProductosTableAdapter();
            TAVentasProductos.Connection = Utilidades.DAOUtilidades.conexion;
            TAInventariosProductosCantidadesTransaccionesHistorial = new InventariosProductosCantidadesTransaccionesHistorialTableAdapter();
            TAInventariosProductosCantidadesTransaccionesHistorial.Connection = Utilidades.DAOUtilidades.conexion;
            TAListarProductosCantidadSuperaStockMinimo = new ListarProductosCantidadSuperaStockMinimoXMLTableAdapter();
            TAListarProductosCantidadSuperaStockMinimo.Connection = Utilidades.DAOUtilidades.conexion;
            TAListarProductosExistenciaInsuficiente = new ListarProductosExistenciaInsuficienteXMLTableAdapter();
            TAListarProductosExistenciaInsuficiente.Connection = Utilidades.DAOUtilidades.conexion;
            TAListarProductosRecepcionFaltantes = new ListarProductosDistribucionFaltantesTableAdapter();
            TAListarProductosRecepcionFaltantes.Connection = Utilidades.DAOUtilidades.conexion;
            
            fuenteDefecto = dtGVProductosCantidadesSeleccionadas.Font;
            DTProductosDistribucion = TAListarProductosDistribucionFaltantes.GetData(NumeroAlmacen, NumeroVentaProducto);

            dtGVProductosListado.AutoGenerateColumns = false;
            dtGVProductosListado.DataSource = DTProductosDistribucion;
            DTProductosSeleccionados = new AlvecoComercial10DataSet.DTProductosRecepcionEntregaDataTable();
            DTProductosSeleccionadosEspecificos = new AlvecoComercial10DataSet.DTProductosRecepcionEntregaEspecificosDataTable();            
            formIngresarCantidad = new FIngresarCantidad(false, "Ingrese la Cantidad a Distribuir", false);
            
            
            dtGVProductosCantidadesSeleccionadas.AutoGenerateColumns = false;
            dtGVProductosCantidadesSeleccionadas.DataSource = DTProductosSeleccionados;

            dtGVProductosEspecificosSeleccionados.AutoGenerateColumns = false;
            dtGVProductosEspecificosSeleccionados.DataSource = DTProductosSeleccionadosEspecificos;

            //agregado 08/05/2011
            gBoxListadoProductosEspecificosSeleccionados.Visible = false;
            DGCEsProductoEspecifico.Visible = false;

            if (DTProductosDistribucion.Count == 0)
            {
                btnConfirmarTotal.Enabled = false;
            }
        }

        private void dtGVProductosCantidadesSeleccionadas_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            int CantidadNuevaDeEntrega;

            this.dtGVProductosCantidadesSeleccionadas.Rows[e.RowIndex].ErrorText = "";

            // No cell validation for new rows. New rows are validated on Row Validation.
            if (this.dtGVProductosCantidadesSeleccionadas.Rows[e.RowIndex].IsNewRow) { return; }

            if (this.dtGVProductosCantidadesSeleccionadas.IsCurrentCellDirty)
            {
                switch (this.dtGVProductosCantidadesSeleccionadas.Columns[e.ColumnIndex].Name)
                {

                    case "DGCCantidadRecepcionEntrega": 
                        if (e.FormattedValue.ToString().Trim().Length < 1)
                        {
                            this.dtGVProductosCantidadesSeleccionadas.Rows[e.RowIndex].ErrorText = "   La Cantidad a entregar es necesaria y no puede estar vacia.";
                            e.Cancel = true;
                        }
                        else if (!int.TryParse(e.FormattedValue.ToString(), out CantidadNuevaDeEntrega) || CantidadNuevaDeEntrega <= 0)
                        {
                            this.dtGVProductosCantidadesSeleccionadas.Rows[e.RowIndex].ErrorText = "   La Cantidad a entregar debe ser un entero positivo.";
                            e.Cancel = true;
                        }

                        if (int.TryParse(e.FormattedValue.ToString(), out CantidadNuevaDeEntrega))
                        {
                            int CantidadMaximaEntrega = DTProductosDistribucion.FindByCodigoProducto(DTProductosSeleccionados[e.RowIndex].CodigoProducto).CantidadFaltante;
                            int existenciaInventario = SAlvecoComercial10.Formularios.Utilidades.DAOUtilidades.ObtenerExistenciaActualInventarios(NumeroAlmacen, DTProductosSeleccionados[e.RowIndex].CodigoProducto);                            
                            if (CantidadNuevaDeEntrega <= CantidadMaximaEntrega)
                            {

                                if (CantidadNuevaDeEntrega > existenciaInventario)
                                    {
                                        this.dtGVProductosCantidadesSeleccionadas.Rows[e.RowIndex].ErrorText = "   No puede entregar una cantidad que no puede Ser abastecida por Almacenes";
                                        e.Cancel = true;
                                    }
                                

                            }
                            else
                            {
                                this.dtGVProductosCantidadesSeleccionadas.Rows[e.RowIndex].ErrorText = "   No puede entregar una cantidad superior a la Cantidad Vendida.";
                                e.Cancel = true;
                            }
                        }

                        //MessageBox.Show("Valor a Validar " + e.FormattedValue.ToString() + ",  Valor Anterior " + this.dtGVProductosCantidadesSeleccionadas.Rows[e.RowIndex].Cells["DGCCantidadExistencia"].Value.ToString());
                        break;


                }

            }
        }

        private void dtGVProductosCantidadesSeleccionadas_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dtGVProductosCantidadesSeleccionadas.RowCount > 0 
                && dtGVProductosCantidadesSeleccionadas.CurrentRow != null && e.RowIndex >= 0)
            {
                string CodigoProducto = DTProductosSeleccionados[e.RowIndex].CodigoProducto;
                string NombreProducto = DTProductosSeleccionados[e.RowIndex].NombreProducto ;
                bool EsProductoEspecifico = DTProductosDistribucion.FindByCodigoProducto(CodigoProducto).EsProductoEspecifico;


                if (e.ColumnIndex == DGCCantidadRecepcionEntrega.Index)
                {
                    //if (EsProductoEspecifico)
                    //{

                    //    object CantidadProductoRegistrado = DTProductosSeleccionadosEspecificos.Compute("count(CodigoProducto)", "CodigoProducto = '" + CodigoProducto + "'");
                    //    int CantidadMaxima = DTProductosDistribucion.FindByCodigoProducto(CodigoProducto).CantidadFaltante;
                    //    int CantidadNueva = DTProductosSeleccionados[e.RowIndex].CantidadRecepcionEntrega;

                    //    if (CantidadNueva > CantidadMaxima)
                    //    {
                    //        if (MessageBox.Show(this, "No puede entregar una Cantidad que supera a lo especificado dentro de la Venta incluyendo ya las partes entregadas para este Producto"
                    //            + Environment.NewLine + " ¿Desea que el Sistema actualize a la Cantidad Recomendable Tope de Recepción? "
                    //            , this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    //        {
                    //            DTProductosSeleccionados[e.RowIndex].CantidadRecepcionEntrega = CantidadMaxima;
                    //            DTProductosSeleccionados.Rows[e.RowIndex].AcceptChanges();
                    //            CantidadNueva = CantidadMaxima;
                    //        }
                    //        else
                    //        {
                    //            DTProductosSeleccionados.Rows[e.RowIndex].RejectChanges();
                    //            dtGVProductosCantidadesSeleccionadas.CurrentCell = dtGVProductosCantidadesSeleccionadas["DGCCantidadRecepcionEntrega", e.RowIndex];
                    //            dtGVProductosCantidadesSeleccionadas.BeginEdit(true);
                    //            llenadoConfirmadoPE = false;
                    //        }
                    //    }

                    //    if (CantidadProductoRegistrado.Equals(0))
                    //    {
                    //        //FVentaProductosEspecificos formSeleccionCodigosEspecificos = new FVentaProductosEspecificos(NumeroAlmacen, NumeroVentaProducto, CodigoProducto, "E", NombreProducto, CantidadNueva);
                    //        FVentaProductosEspecificos formVentaProductosEspecificos = new FVentaProductosEspecificos(NumeroAlmacen,
                    //            NumeroVentaProducto, CodigoProducto, "E", NombreProducto, CantidadNueva);
                    //        formVentaProductosEspecificos.ShowDialog(this);
                    //        //FIngresosProductosEspecificosRecepcion formIngresosCodigosEspecificos = new FIngresosProductosEspecificosRecepcion(CodigoProducto, CantidadNueva, NombreProducto, NumeroAlmacen);
                    //        //formIngresosCodigosEspecificos.ShowDialog();
                    //        if (formVentaProductosEspecificos.OperacionConfirmada)
                    //        {
                    //            int indice = 0;
                    //            foreach (DataRow FilaNueva in formVentaProductosEspecificos.DTProductosEspecificos.Select("Seleccionar = True"))
                    //            {
                    //                CLCAD.AlvecoComercial10DataSet.DTProductosRecepcionEntregaEspecificosRow
                    //                    DRNuevoEspecifico = DTProductosSeleccionadosEspecificos.NewDTProductosRecepcionEntregaEspecificosRow();
                    //                if (indice == 0)
                    //                    DRNuevoEspecifico.NombreProducto = NombreProducto;
                    //                DRNuevoEspecifico.CodigoProducto = CodigoProducto;
                    //                DRNuevoEspecifico.CodigoProductoEspecifico = FilaNueva["CodigoProductoEspecifico"].ToString();

                    //                DTProductosSeleccionadosEspecificos.Rows.Add(DRNuevoEspecifico);
                    //                DRNuevoEspecifico.AcceptChanges();
                    //                indice++;

                    //            }
                    //            llenadoConfirmadoPE = true;
                    //        }
                    //        else
                    //        {
                    //            DTProductosSeleccionados.Rows[e.RowIndex].RejectChanges();
                    //            dtGVProductosCantidadesSeleccionadas.CurrentCell = dtGVProductosCantidadesSeleccionadas["DGCCantidadRecepcionEntrega", e.RowIndex];
                    //            dtGVProductosCantidadesSeleccionadas.BeginEdit(true);
                    //            llenadoConfirmadoPE = false;
                    //        }
                    //        //_FCompraProductosIngresoEspecificos.Dispose();
                    //    }
                    //    else
                    //    {
                    //        FVentaProductosEspecificos formSeleccionProductosEspecficiso = new FVentaProductosEspecificos(
                    //            NumeroAlmacen, NumeroVentaProducto, CodigoProducto, "E", NombreProducto, CantidadNueva);
                    //        //DataRow[] DRProductosEspecificosPorProductos = DTProductosSeleccionadosEspecificos.Select("CodigoProducto = '" + CodigoProducto + "'");

                    //        //foreach (DataRow DRProductoEspecifico in DRProductosEspecificosPorProductos)
                    //        //{
                    //        //    DataRow DRNuevoProductoEspecifico = _FCompraProductosIngresoEspecificos.DTProductosEspecificos.NewRow();
                    //        //    DRNuevoProductoEspecifico["CodigoProductoEspecifico"] = DRProductoEspecifico["CodigoProductoEspecifico"];

                    //        //    _FCompraProductosIngresoEspecificos.DTProductosEspecificos.Rows.Add(DRNuevoProductoEspecifico);
                    //        //    DRNuevoProductoEspecifico.AcceptChanges();
                    //        //}

                    //        formSeleccionProductosEspecficiso.cargarDatosAnterior(DTProductosSeleccionadosEspecificos);


                    //        int CantidadRegistrada = int.Parse(CantidadProductoRegistrado.ToString());
                    //        if (CantidadNueva > CantidadRegistrada) // se Adicionan mas Productos
                    //        {
                    //            formSeleccionProductosEspecficiso.ShowDialog();
                    //            if (formSeleccionProductosEspecficiso.OperacionConfirmada)
                    //            {
                    //                foreach (DataRow DRProductoNuevo in formSeleccionProductosEspecficiso.DTProductosEspecificos.Select("Seleccionar = True"))
                    //                {

                    //                    if (DTProductosSeleccionadosEspecificos.FindByCodigoProductoCodigoProductoEspecifico
                    //                        (CodigoProducto, DRProductoNuevo["CodigoProductoEspecifico"].ToString()) == null)
                    //                    {
                    //                        DataRow DRNuevoEspecifico = DTProductosSeleccionadosEspecificos.NewRow();
                    //                        DRNuevoEspecifico["CodigoProducto"] = CodigoProducto;
                    //                        DRNuevoEspecifico["CodigoProductoEspecifico"] = DRProductoNuevo["CodigoProductoEspecifico"];


                    //                        DTProductosSeleccionadosEspecificos.Rows.Add(DRNuevoEspecifico);
                    //                        DRNuevoEspecifico.AcceptChanges();
                    //                    }
                    //                }
                    //                llenadoConfirmadoPE = true;
                    //            }
                    //            else
                    //            {
                    //                DTProductosSeleccionados.Rows[e.RowIndex].RejectChanges();
                    //                dtGVProductosCantidadesSeleccionadas.CurrentCell = dtGVProductosCantidadesSeleccionadas["DGCCantidadRecepcionEntrega", e.RowIndex];
                    //                dtGVProductosCantidadesSeleccionadas.BeginEdit(true);
                    //                llenadoConfirmadoPE = false;
                    //            }
                    //        }
                    //        else // Cuando disminuyen la cantidad, se debe eliminar codigos especificos
                    //        {
                    //            MessageBox.Show("Debe Seleccionar los productos Específicos que ya no desea incluir en la lista");
                    //            formSeleccionProductosEspecficiso.ShowDialog();
                    //            if (formSeleccionProductosEspecficiso.OperacionConfirmada)
                    //            {
                    //                foreach (AlvecoComercial10DataSet.ListarCodigosProductosEspecificosRow DREspecifico in formSeleccionProductosEspecficiso.DTProductosEspecificos.Select("Seleccionar = False"))
                    //                {
                    //                    DataRow DRProductoEliminar = DTProductosSeleccionadosEspecificos.Rows.Find(new object[] { CodigoProducto, DREspecifico.CodigoProductoEspecifico });
                    //                    if (DRProductoEliminar != null)
                    //                        DTProductosSeleccionadosEspecificos.Rows[DTProductosSeleccionadosEspecificos.Rows.IndexOf(DRProductoEliminar)].Delete();

                    //                }
                    //                llenadoConfirmadoPE = true;
                    //                if (CantidadNueva > 0)
                    //                {
                    //                    DTProductosSeleccionadosEspecificos.Select("CodigoProducto = '" + CodigoProducto +"'")[0]["NombreProducto"] = NombreProducto;
                    //                    DTProductosSeleccionadosEspecificos.AcceptChanges();
                    //                }
                    //            }
                    //            else
                    //            {
                    //                DTProductosSeleccionados.Rows[e.RowIndex].RejectChanges();
                    //                dtGVProductosCantidadesSeleccionadas.CurrentCell = dtGVProductosCantidadesSeleccionadas["DGCCantidadRecepcionEntrega", e.RowIndex];
                    //                dtGVProductosCantidadesSeleccionadas.BeginEdit(true);
                    //                llenadoConfirmadoPE = false;
                    //            }
                    //        }
                    //        //_FCompraProductosIngresoEspecificos.Dispose();
                    //    }
                    //    DTProductosSeleccionados.AcceptChanges();
                    //    DTProductosSeleccionadosEspecificos.AcceptChanges();
                    //}
                }
            }
        }

        private void dtGVProductosEspecificosSeleccionados_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (DTProductosSeleccionadosEspecificos.Rows.Count > 0)
            {

                if (dtGVProductosEspecificosSeleccionados.Rows[e.RowIndex].Cells[0].Value != null && !dtGVProductosEspecificosSeleccionados.Rows[e.RowIndex].Cells[0].Value.ToString().Trim().Equals(""))
                {
                    dtGVProductosEspecificosSeleccionados.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Bisque;
                    dtGVProductosEspecificosSeleccionados.Rows[e.RowIndex].Cells["DGCNombreProductoAE"].Style.Font = new Font(fuenteDefecto.Name, fuenteDefecto.Size, FontStyle.Bold);
                }
            }
        }

        private void dtGVProductosListado_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dtGVProductosListado.IsCurrentCellDirty && dtGVProductosListado.CurrentCell.ColumnIndex == DGCSeleccionar.Index)
            {
                dtGVProductosListado.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dtGVProductosListado_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (DTProductosDistribucion != null && DTProductosDistribucion.Count != 0 
                && dtGVProductosListado.CurrentCell != null && e.ColumnIndex == DGCSeleccionar.Index)
            {
                if (e.ColumnIndex == DGCSeleccionar.Index)
                {
                    string CodigoProducto = "", NombreProducto = "";
                    int CantidadRecepcion = 0, CantidadMaximaEnvio = 0;

                    CodigoProducto = DTProductosDistribucion[e.RowIndex].CodigoProducto;
                    NombreProducto = DTProductosDistribucion[e.RowIndex].NombreProducto;
                    CantidadMaximaEnvio = DTProductosDistribucion[e.RowIndex].CantidadFaltante;

                    //Agrega el producto a la Lista de Productos de Entrega
                    if (dtGVProductosListado[e.ColumnIndex, e.RowIndex].Value.Equals(true))
                    {
                        formIngresarCantidad.MontoTopeMaximo = CantidadMaximaEnvio;                        
                        if (formIngresarCantidad.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            CantidadRecepcion = formIngresarCantidad.CantidadEnteraIngresada;
                            if (CantidadRecepcion > CantidadMaximaEnvio)
                            {
                                MessageBox.Show(this, "La cantidad ingresada supera a la cantidad maxima posible de recepción para este Ingreso", "Cantidad no Valida", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                DTProductosDistribucion[e.RowIndex].RejectChanges();
                                DTProductosDistribucion[e.RowIndex].Seleccionar = false;
                                DTProductosDistribucion.AcceptChanges();
                                return;
                            }
                            //SI NO EXISTE EL Producto
                            if (DTProductosSeleccionados.FindByCodigoProducto(CodigoProducto) == null)
                            {
                                //if (DTProductosDistribucion[e.RowIndex].EsProductoEspecifico)
                                //{
                                //    FVentaProductosEspecificos formSeleccionEspecificos = new FVentaProductosEspecificos(NumeroAlmacen,
                                //        NumeroVentaProducto, CodigoProducto, "E", NombreProducto, CantidadRecepcion);
                                //    //FIngresosProductosEspecificosRecepcion formSeleccionEspecificos = new FIngresosProductosEspecificosRecepcion(CodigoProducto, CantidadRecepcion, NombreProducto, NumeroAlmacen);
                                //    formSeleccionEspecificos.ShowDialog();
                                //    if (!formSeleccionEspecificos.OperacionConfirmada)
                                //    {
                                //        DTProductosDistribucion[e.RowIndex].RejectChanges();
                                //        return;
                                //    }
                                //    else
                                //    {
                                //        int i = 0;
                                //        //foreach (DSDoblones20GestionComercial2.ListarCodigosProductosEspecificosTransferenciasRow FilaEspecifico
                                //        //    in (DSDoblones20GestionComercial2.ListarCodigosProductosEspecificosTransferenciasRow[])
                                //        //    _FTransferenciaProductosRecepcionEnvioPE.DTProductosEspecificos.Select("Seleccionar = true"))
                                //        //{
                                //        //    _DTProductosEspecificosTemporal.Rows.Add(new object[] { CodigoProducto, ((i == 0) ? NombreProducto : String.Empty), FilaEspecifico.CodigoProductoEspecifico, DTVentasProductosDetalle[e.RowIndex]["TiempoGarantiaVenta"] });
                                //        //    i++;
                                //        //}
                                //        //_DTProductosEspecificosTemporal.AcceptChanges();
                                //        foreach (DataRow DRProductoEspecifico in formSeleccionEspecificos.DTProductosEspecificos.Select("Seleccionar = True"))
                                //        {
                                //            DTProductosSeleccionadosEspecificos.AddDTProductosRecepcionEntregaEspecificosRow(CodigoProducto, DRProductoEspecifico["CodigoProductoEspecifico"].ToString(), ((i == 0) ? NombreProducto : String.Empty));
                                //            i++;
                                //        }
                                //    }

                                //}

                                DTProductosSeleccionados.AddDTProductosRecepcionEntregaRow(CodigoProducto, NombreProducto, CantidadRecepcion);
                                DTProductosSeleccionados.AcceptChanges();
                                                                
                            }
                        }
                        else
                        {
                            
                            DTProductosDistribucion[e.RowIndex].RejectChanges();
                            DTProductosDistribucion[e.RowIndex].Seleccionar = false;
                            DTProductosDistribucion.AcceptChanges();
                        }

                    }
                    else//quitar de la lista
                    {
                        AlvecoComercial10DataSet.DTProductosRecepcionEntregaRow DRProductoEliminar = DTProductosSeleccionados.FindByCodigoProducto(CodigoProducto);
                        //DataRow DRProductoEliminar = _DTProductosDetalleEntrega.Rows.Find(CodigoProducto);
                        if (DRProductoEliminar != null)
                        {
                            //if (MessageBox.Show(this, "¿Esta seguro de Cancelar el Envio del Producto " + NombreProducto + "?", "Elimiar Producto", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            //{
                            //    //DTTransferenciaProductosDetalle[e.RowIndex].RejectChanges();
                            //    dtGVProductosListado[e.ColumnIndex, e.RowIndex].Value = true;
                            //    return;
                            //}

                            DataRow[] DRProductosEspecificosEliminar = DTProductosSeleccionadosEspecificos.Select("CodigoProducto = '" + CodigoProducto + "'");
                            if (DRProductosEspecificosEliminar != null)
                                foreach (DataRow DRProductoEspecifico in DRProductosEspecificosEliminar)
                                    DRProductoEspecifico.Delete();
                            DRProductoEliminar.Delete();

                            DTProductosSeleccionadosEspecificos.AcceptChanges();
                            DTProductosSeleccionados.AcceptChanges();
                        }
                    }
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();

        }

        private void btnConfirmarTotal_Click(object sender, EventArgs e)
        {
            
            //if (MessageBox.Show(this, "La recepción total o Parcial de los Productos seleccionados implica la actualización en inventarios de acuerdo a la cantidad?" +
            //    " selecionada. ¿Desea confirmar la accion?", "Confirmación de recepción", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            //    return;
            Button btnAccion = (Button)sender;            
            if (btnAccion.Equals(btnConfirmarParcial) && DTProductosSeleccionados.Count == 0)
            {
                MessageBox.Show(this, "Aún no ha seleccionado ningún artículo, seeleccioné que artículo desea recepcionar en la Columna 'Seleccionar'", "Confirmación de recepción",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            

            if (btnAccion.Equals(btnConfirmarForzada) && 
                MessageBox.Show(this,"Ha decidido forzar la recepción de Productos en un estado Incompleto ¿Desea continuar?", "Confirmación de recepción", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            /*if(btnAccion.Equals(btnConfirmarTotal))
            {
                DTProductosSeleccionados.Clear();
                DTProductosSeleccionados.AcceptChanges();
                DTProductosSeleccionadosEspecificos.Clear();
                DTProductosSeleccionadosEspecificos.AcceptChanges();

                foreach (DataGridViewRow DGRProductoLista in dtGVProductosListado.Rows)
                {
                    DGRProductoLista.Cells["DGCSeleccionar"].Value = false;
                    dtGVProductosListado.CurrentCell = dtGVProductosListado[0, DGRProductoLista.Index];
                    dtGVProductosListado.CurrentRow.Selected = true;
                    dtGVProductosListado.FirstDisplayedScrollingRowIndex = DGRProductoLista.Index;                    
                    DGRProductoLista.Cells["DGCSeleccionar"].Value = true;
                }
            }*/
            try
            {
                DataSet DSVentasProductos = new DataSet("VentasProductos");
                DataTable DTVentasProductosDetalleXML = DTProductosSeleccionados.Copy();
                DTVentasProductosDetalleXML.TableName = "VentasProductosDetalle";
                DTVentasProductosDetalleXML.Columns.Remove(DTVentasProductosDetalleXML.Columns["NombreProducto"]);
                DSVentasProductos.Tables.Add(DTVentasProductosDetalleXML);
                string VentasDetalleXML = DTVentasProductosDetalleXML.DataSet.GetXml();

                string ListadoProductos = "";
                AlvecoComercial10DataSet.ListarProductosCantidadSuperaStockMinimoXMLDataTable DTListarProductosCantidadSuperaStockMinimo =
                    TAListarProductosCantidadSuperaStockMinimo.GetData(NumeroAlmacen, VentasDetalleXML);
                if (DTListarProductosCantidadSuperaStockMinimo.Count > 0)
                {

                    foreach (AlvecoComercial10DataSet.ListarProductosCantidadSuperaStockMinimoRow
                        DRProducto in DTListarProductosCantidadSuperaStockMinimo.Rows)
                        ListadoProductos += "\r\n" + DRProducto.NombreProducto;

                    if (MessageBox.Show(this, "La siguiente Lista de Artículos supera la cantidad de Stock Minimo en inventarios" +
                        ListadoProductos + "\r\n¿Desea Continuar la transacción?",
                        "Stock Mínimo en Posible riesgo de ser Superado",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return;
                }
                ListadoProductos = string.Empty;
                AlvecoComercial10DataSet.ListarProductosExistenciaInsuficienteXMLDataTable DTListarProductosExistenciaInsuficiente
                    = TAListarProductosExistenciaInsuficiente.GetData(NumeroAlmacen, VentasDetalleXML);
                if (DTListarProductosExistenciaInsuficiente.Count > 0)
                {
                    foreach (AlvecoComercial10DataSet.ListarProductosExistenciaInsuficienteRow
                        DRProducto in DTListarProductosExistenciaInsuficiente.Rows)
                    {
                        ListadoProductos += "\r\n" + DRProducto.NombreProducto;
                    }

                    MessageBox.Show(this, "La Siguiente Lista de Productos supera la Cantidad de Existencia en Almacenes " +
                        ListadoProductos + "\r\nLa Operación actual no puede continuar bajo la situación de existencia actual en almacenes",
                        "Existencia Insuficiente en Inventarios para Abastecer Solicitud",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //DateTime FechaHoraRegistro = SAlvecoComercial10.Formularios.Utilidades.DAOUtilidades.ObtenerFechaHoraServidor();
                DateTime FechaHoraRegistro = FechaHoraEntrega;

                foreach (AlvecoComercial10DataSet.DTProductosRecepcionEntregaRow
                    DRProductos in DTProductosSeleccionados.Rows)
                {   /**
                     * 
                     * CORREGIR
                     * CORREGIR
                     * CORREGIR
                     * */
                    DateTime FechaHoraCompraInventario = Utilidades.DAOUtilidades.ObtenerFechaHoraServidor();
                    TAVentasProductosDetalleEntrega.Insert(NumeroAlmacen, NumeroVentaProducto, DRProductos.CodigoProducto, FechaHoraRegistro, FechaHoraCompraInventario, DRProductos.CantidadRecepcionEntrega, -1);
                    //foreach (AlvecoComercial10DataSet.DTProductosRecepcionEntregaEspecificosRow
                    //    DRProductoEspecifico in DTProductosSeleccionadosEspecificos.Select("CodigoProducto = '" + DRProductos.CodigoProducto + "'"))
                    //{
                    //    TAListarProductosDistribucionFaltantes.InsertarVentaProductoEspecifico(NumeroAlmacen,
                    //        NumeroVentaProducto, DRProductoEspecifico.CodigoProducto, DRProductoEspecifico.CodigoProductoEspecifico,
                    //        0, false, FechaHoraRegistro);                        
                    //}
                }

                //TAVentasProductos.ActualizarInventarioIngresosProductos(NumeroAlmacen, NumeroVentaProducto, FechaHoraRegistro);
                TAVentasProductos.ActualizarInventarioVentasProductos(NumeroAlmacen, NumeroVentaProducto, FechaHoraRegistro, true);

                int CantidadTotal = int.Parse(DTProductosDistribucion.Compute("sum(CantidadFaltante)", "").ToString());
                int CantidadRecepcionada = int.Parse(DTProductosSeleccionados.Compute("sum(CantidadRecepcionEntrega)", "").ToString());
                if (btnAccion.Equals(btnConfirmarTotal) || btnAccion.Equals(btnConfirmarParcial))
                {
                    TAVentasProductos.ActualizarCodigoEstadoVenta(NumeroAlmacen, NumeroVentaProducto,
                            CantidadTotal == CantidadRecepcionada ? "F" : "D");
                    this.DialogResult = CantidadTotal == CantidadRecepcionada ?DialogResult.OK : DialogResult.Ignore;

                   


                }
                //else if ()
                //{
                //    _IngresosProductosCLN.ActualizarIngresoProducto(NumeroAlmacen, NumeroIngresoProducto, "D");
                //    this.DialogResult = DialogResult.Ignore;

                //}
                else
                {
                    TAVentasProductos.ActualizarCodigoEstadoVenta(NumeroAlmacen, NumeroVentaProducto, "X");
                    this.DialogResult = DialogResult.Ignore;
                }

                
                //MessageBox.Show(this, "Se distribuyó correcatmente los Productos Seleccionados", "Distribución de Productos",
                //    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Ocurrio la Siguiente Excepción " + ex.Message, "Recepción de Productos",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void FIngresosProductosRecepcion_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (DTProductosDistribucion != null && DTProductosDistribucion.Count > 0 && this.DialogResult == DialogResult.Cancel
            //    && MessageBox.Show(this,"¿Se encuentra seguro de cancelar la Operación Actual?","Recepción de Productos",
            //    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            //{
            //    e.Cancel = true;
            //    return;
            //}
        }

    }
}
