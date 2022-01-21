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
    public partial class FComprasProductosRecepcion : Form
    {

        AlvecoComercial10DataSet.ListarProductosRecepcionFaltantesDataTable DTProductosRecepcion;
        AlvecoComercial10DataSet.DTProductosRecepcionEntregaDataTable DTProductosSeleccionados;
        AlvecoComercial10DataSet.DTProductosRecepcionEntregaEspecificosDataTable DTProductosSeleccionadosEspecificos;


        ListarProductosRecepcionFaltantesTableAdapter TAListarProductosRecepcionFaltantes;
        ComprasProductosDetalleEntregaTableAdapter TAComprasProductosDetalleEntrega;        
        ComprasProductosTableAdapter TAComprasProductos;
        InventariosProductosCantidadesTransaccionesHistorialTableAdapter TAInventariosProductosCantidadesTransaccionesHistorial;
        //InventariosProductosEspecificos _InventariosProductosEspecificosCLN;
        //ComprasProductosEspecificosCLN _ComprasProductosEspecificosCLN;

        int NumeroAlmacen;        
        string DIUsuario;
        int NumeroCompraProducto;

        bool llenadoConfirmadoPE = false;
        Font fuenteDefecto;
        Utilidades.FIngresarCantidad formIngresarCantidad;

        public FComprasProductosRecepcion(int NumeroAlmacen, string CodigoUsuario, int NumeroCompraProducto)
        {
            InitializeComponent(); 
            DTProductosRecepcion = new AlvecoComercial10DataSet.ListarProductosRecepcionFaltantesDataTable();
            this.NumeroAlmacen = NumeroAlmacen;            
            this.DIUsuario = CodigoUsuario;
            this.NumeroCompraProducto = NumeroCompraProducto;

            TAComprasProductosDetalleEntrega = new ComprasProductosDetalleEntregaTableAdapter();
            TAComprasProductosDetalleEntrega.Connection = Utilidades.DAOUtilidades.conexion;
            //_ComprasProductosEspecificosCLN = new ComprasProductosEspecificosCLN();
            TAComprasProductos = new ComprasProductosTableAdapter();
            TAComprasProductos.Connection = Utilidades.DAOUtilidades.conexion;
            TAInventariosProductosCantidadesTransaccionesHistorial = new InventariosProductosCantidadesTransaccionesHistorialTableAdapter();
            TAInventariosProductosCantidadesTransaccionesHistorial.Connection = Utilidades.DAOUtilidades.conexion;
            TAListarProductosRecepcionFaltantes = new ListarProductosRecepcionFaltantesTableAdapter();
            TAListarProductosRecepcionFaltantes.Connection = Utilidades.DAOUtilidades.conexion;
            //_InventariosProductosEspecificosCLN = new InventariosProductosEspecificosCLN();
            fuenteDefecto = dtGVProductosCantidadesSeleccionadas.Font;
            DTProductosRecepcion = TAListarProductosRecepcionFaltantes.GetData(NumeroAlmacen, NumeroCompraProducto);
            //DTProductosRecepcion.CantidadRecepcionadaColumn.ReadOnly = false;

            dtGVProductosListado.AutoGenerateColumns = false;
            dtGVProductosListado.DataSource = DTProductosRecepcion;
            DTProductosSeleccionados = new AlvecoComercial10DataSet.DTProductosRecepcionEntregaDataTable();
            DTProductosSeleccionadosEspecificos = new AlvecoComercial10DataSet.DTProductosRecepcionEntregaEspecificosDataTable();
            formIngresarCantidad = new Utilidades.FIngresarCantidad(false);

            dtGVProductosCantidadesSeleccionadas.AutoGenerateColumns = false;
            dtGVProductosCantidadesSeleccionadas.DataSource = DTProductosSeleccionados;

            dtGVProductosEspecificosSeleccionados.AutoGenerateColumns = false;
            dtGVProductosEspecificosSeleccionados.DataSource = DTProductosSeleccionadosEspecificos;

            
            foreach(AlvecoComercial10DataSet.ListarProductosRecepcionFaltantesRow fila in DTProductosRecepcion.Rows)
            {
                DTProductosSeleccionados.AddDTProductosRecepcionEntregaRow(fila.CodigoProducto, fila.NombreProducto, fila.CantidadFaltante);
                DTProductosSeleccionados.AcceptChanges();
                fila.Seleccionar = true;
                //fila.CantidadRecepcionada += fila.CantidadFaltante;
                fila.AcceptChanges();
                
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
                            this.dtGVProductosCantidadesSeleccionadas.Rows[e.RowIndex].ErrorText = "   La Cantidad a recepcionar es necesaria y no puede estar vacia.";
                            e.Cancel = true;
                        }
                        else if (!int.TryParse(e.FormattedValue.ToString(), out CantidadNuevaDeEntrega) || CantidadNuevaDeEntrega < 0)
                        {
                            this.dtGVProductosCantidadesSeleccionadas.Rows[e.RowIndex].ErrorText = "   La Cantidad a recepcionar debe ser un entero positivo.";
                            e.Cancel = true;
                        }

                        if (int.TryParse(e.FormattedValue.ToString(), out CantidadNuevaDeEntrega))
                        {
                            int CantidadFaltante = DTProductosRecepcion.FindByCodigoProducto(DTProductosSeleccionados[e.RowIndex].CodigoProducto).CantidadFaltante;                            
                            if (CantidadNuevaDeEntrega > CantidadFaltante)
                            {
                                this.dtGVProductosCantidadesSeleccionadas.Rows[e.RowIndex].ErrorText = "   No puede recepcionar una cantidad superior a la Cantidad Faltante de Recepción.";
                                e.Cancel = true;
                            }
                        }
                        break;


                }

            }
        }

        private void dtGVProductosCantidadesSeleccionadas_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //if (dtGVProductosCantidadesSeleccionadas.RowCount > 0 
            //    && dtGVProductosCantidadesSeleccionadas.CurrentRow != null && e.RowIndex >= 0)
            //{
            //    string CodigoProducto = DTProductosSeleccionados[e.RowIndex].CodigoProducto;
            //    string NombreProducto = DTProductosSeleccionados[e.RowIndex].NombreProducto ;
            //    bool EsProductoEspecifico = DTProductosRecepcion.FindByCodigoProducto(CodigoProducto).EsProductoEspecifico;


            //    if (e.ColumnIndex == DGCCantidadRecepcionEntrega.Index)
            //    {
            //        if (EsProductoEspecifico)
            //        {

            //            object CantidadProductoRegistrado = DTProductosSeleccionadosEspecificos.Compute("count(CodigoProducto)", "CodigoProducto = '" + CodigoProducto + "'");
            //            int CantidadMaxima = DTProductosRecepcion.FindByCodigoProducto(CodigoProducto).CantidadFaltante;
            //            int CantidadNueva = DTProductosSeleccionados[e.RowIndex].CantidadRecepcionEntrega;

            //            if (CantidadNueva > CantidadMaxima)
            //            {
            //                if (MessageBox.Show(this, "No puede Recepcionar una Cantidad que supera a lo especificado dentro del Compra incluyendo ya las partes recepcionadas para este Producto"
            //                    + Environment.NewLine + " ¿Desea que el Sistema actualize a la Cantidad Recomendable Tope de Recepción? "
            //                    , this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //                {
            //                    DTProductosSeleccionados[e.RowIndex].CantidadRecepcionEntrega = CantidadMaxima;
            //                    DTProductosSeleccionados.Rows[e.RowIndex].AcceptChanges();
            //                    CantidadNueva = CantidadMaxima;
            //                }
            //                else
            //                {
            //                    DTProductosSeleccionados.Rows[e.RowIndex].RejectChanges();
            //                    dtGVProductosCantidadesSeleccionadas.CurrentCell = dtGVProductosCantidadesSeleccionadas["DGCCantidadRecepcionEntrega", e.RowIndex];
            //                    dtGVProductosCantidadesSeleccionadas.BeginEdit(true);
            //                    llenadoConfirmadoPE = false;
            //                }
            //            }

            //            if (CantidadProductoRegistrado.Equals(0))
            //            {

            //                FComprasProductosEspecificosRecepcion formComprasCodigosEspecificos = new FComprasProductosEspecificosRecepcion(CodigoProducto, CantidadNueva, NombreProducto, NumeroAlmacen);
            //                formComprasCodigosEspecificos.ShowDialog();
            //                if (formComprasCodigosEspecificos.OperacionConfirmada)
            //                {
            //                    int indice = 0;
            //                    foreach (DataRow FilaNueva in formComprasCodigosEspecificos.DTProductosEspecificos.Rows)
            //                    {
            //                        CLCAD.AlvecoComercial10DataSet.DTProductosRecepcionEntregaEspecificosRow
            //                            DRNuevoEspecifico = DTProductosSeleccionadosEspecificos.NewDTProductosRecepcionEntregaEspecificosRow();
            //                        if (indice == 0)
            //                            DRNuevoEspecifico.NombreProducto = NombreProducto;
            //                        DRNuevoEspecifico.CodigoProducto = CodigoProducto;
            //                        DRNuevoEspecifico.CodigoProductoEspecifico = FilaNueva["CodigoProductoEspecifico"].ToString();

            //                        DTProductosSeleccionadosEspecificos.Rows.Add(DRNuevoEspecifico);
            //                        DRNuevoEspecifico.AcceptChanges();
            //                        indice++;

            //                    }
            //                    llenadoConfirmadoPE = true;
            //                }
            //                else
            //                {
            //                    DTProductosSeleccionados.Rows[e.RowIndex].RejectChanges();
            //                    dtGVProductosCantidadesSeleccionadas.CurrentCell = dtGVProductosCantidadesSeleccionadas["DGCCantidadRecepcionEntrega", e.RowIndex];
            //                    dtGVProductosCantidadesSeleccionadas.BeginEdit(true);
            //                    llenadoConfirmadoPE = false;
            //                }
            //                //_FCompraProductosCompraEspecificos.Dispose();
            //            }
            //            else
            //            {
            //                FComprasProductosEspecificosRecepcion _FCompraProductosCompraEspecificos = new FComprasProductosEspecificosRecepcion(CodigoProducto, CantidadNueva, NombreProducto, NumeroAlmacen);
            //                DataRow[] DRProductosEspecificosPorProductos = DTProductosSeleccionadosEspecificos.Select("CodigoProducto = '" + CodigoProducto + "'");

            //                foreach (DataRow DRProductoEspecifico in DRProductosEspecificosPorProductos)
            //                {
            //                    DataRow DRNuevoProductoEspecifico = _FCompraProductosCompraEspecificos.DTProductosEspecificos.NewRow();
            //                    DRNuevoProductoEspecifico["CodigoProductoEspecifico"] = DRProductoEspecifico["CodigoProductoEspecifico"];

            //                    _FCompraProductosCompraEspecificos.DTProductosEspecificos.Rows.Add(DRNuevoProductoEspecifico);
            //                    DRNuevoProductoEspecifico.AcceptChanges();
            //                }



            //                int CantidadRegistrada = int.Parse(CantidadProductoRegistrado.ToString());
            //                if (CantidadNueva > CantidadRegistrada) // se Adicionan mas Productos
            //                {
            //                    _FCompraProductosCompraEspecificos.ShowDialog();
            //                    if (_FCompraProductosCompraEspecificos.OperacionConfirmada)
            //                    {
            //                        foreach (DataRow DRProductoNuevo in _FCompraProductosCompraEspecificos.DTProductosEspecificos.Rows)
            //                        {

            //                            if (DTProductosSeleccionadosEspecificos.FindByCodigoProductoCodigoProductoEspecifico
            //                                (CodigoProducto, DRProductoNuevo["CodigoProductoEspecifico"].ToString()) == null)
            //                            {
            //                                DataRow DRNuevoEspecifico = DTProductosSeleccionadosEspecificos.NewRow();
            //                                DRNuevoEspecifico["CodigoProducto"] = CodigoProducto;
            //                                DRNuevoEspecifico["CodigoProductoEspecifico"] = DRProductoNuevo["CodigoProductoEspecifico"];


            //                                DTProductosSeleccionadosEspecificos.Rows.Add(DRNuevoEspecifico);
            //                                DRNuevoEspecifico.AcceptChanges();
            //                            }
            //                        }
            //                        llenadoConfirmadoPE = true;
            //                    }
            //                    else
            //                    {
            //                        DTProductosSeleccionados.Rows[e.RowIndex].RejectChanges();
            //                        dtGVProductosCantidadesSeleccionadas.CurrentCell = dtGVProductosCantidadesSeleccionadas["DGCCantidadRecepcionEntrega", e.RowIndex];
            //                        dtGVProductosCantidadesSeleccionadas.BeginEdit(true);
            //                        llenadoConfirmadoPE = false;
            //                    }
            //                }
            //                else // Cuando disminuyen la cantidad, se debe eliminar codigos especificos
            //                {
            //                    MessageBox.Show("Debe Seleccionar los productos Específicos que desea eliminar");
            //                    _FCompraProductosCompraEspecificos.ShowDialog();
            //                    if (_FCompraProductosCompraEspecificos.OperacionConfirmada && _FCompraProductosCompraEspecificos.ListadoProductosEspecificosEliminados.Count > 0)
            //                    {
            //                        foreach (string codEspecifico in _FCompraProductosCompraEspecificos.ListadoProductosEspecificosEliminados)
            //                        {
            //                            DataRow DRProductoEliminar = DTProductosSeleccionadosEspecificos.Rows.Find(new object[] { CodigoProducto, codEspecifico });
            //                            if (DRProductoEliminar != null)
            //                                DTProductosSeleccionadosEspecificos.Rows[DTProductosSeleccionadosEspecificos.Rows.IndexOf(DRProductoEliminar)].Delete();

            //                        }
            //                        llenadoConfirmadoPE = true;
            //                        if (CantidadNueva > 0)
            //                        {
            //                            DTProductosSeleccionadosEspecificos.Select("CodigoProducto = '" + CodigoProducto +"'")[0]["NombreProducto"] = NombreProducto;
            //                            DTProductosSeleccionadosEspecificos.AcceptChanges();
            //                        }
            //                    }
            //                    else
            //                    {
            //                        DTProductosSeleccionados.Rows[e.RowIndex].RejectChanges();
            //                        dtGVProductosCantidadesSeleccionadas.CurrentCell = dtGVProductosCantidadesSeleccionadas["DGCCantidadRecepcionEntrega", e.RowIndex];
            //                        dtGVProductosCantidadesSeleccionadas.BeginEdit(true);
            //                        llenadoConfirmadoPE = false;
            //                    }
            //                }
            //                //_FCompraProductosCompraEspecificos.Dispose();
            //            }
            //            DTProductosSeleccionados.AcceptChanges();
            //            DTProductosSeleccionadosEspecificos.AcceptChanges();
            //        }
            //    }
            //}
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
            if (DTProductosRecepcion != null && DTProductosRecepcion.Count != 0 
                && dtGVProductosListado.CurrentCell != null && e.ColumnIndex == DGCSeleccionar.Index)
            {
                if (e.ColumnIndex == DGCSeleccionar.Index)
                {
                    string CodigoProducto = "", NombreProducto = "";
                    int CantidadRecepcion = 0, CantidadMaximaEnvio = 0;

                    CodigoProducto = DTProductosRecepcion[e.RowIndex].CodigoProducto;
                    NombreProducto = DTProductosRecepcion[e.RowIndex].NombreProducto;
                    CantidadMaximaEnvio = DTProductosRecepcion[e.RowIndex].CantidadFaltante;

                    //Agrega el producto a la Lista de Productos de Entrega
                    if (dtGVProductosListado[e.ColumnIndex, e.RowIndex].Value.Equals(true))
                    {
                        formIngresarCantidad.MontoTopeMaximo = CantidadMaximaEnvio;
                        formIngresarCantidad.LimpiarControl();

                        if (formIngresarCantidad.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            CantidadRecepcion = formIngresarCantidad.CantidadEnteraIngresada;
                            if (CantidadRecepcion > CantidadMaximaEnvio)
                            {
                                MessageBox.Show(this, "La cantidad ingresada supera a la cantidad maxima posible de recepción para este Compra", "Cantidad no Valida", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                DTProductosRecepcion[e.RowIndex].RejectChanges();
                                DTProductosRecepcion[e.RowIndex].Seleccionar = false;
                                DTProductosRecepcion.AcceptChanges();
                                return;
                            }
                            //SI NO EXISTE EL Producto
                            if (DTProductosSeleccionados.FindByCodigoProducto(CodigoProducto) == null)
                            {
                                //if (DTProductosRecepcion[e.RowIndex].EsProductoEspecifico)
                                //{
                                //    FComprasProductosEspecificosRecepcion formSeleccionEspecificos = new FComprasProductosEspecificosRecepcion(CodigoProducto, CantidadRecepcion, NombreProducto, NumeroAlmacen);
                                //    formSeleccionEspecificos.ShowDialog();
                                //    if (!formSeleccionEspecificos.OperacionConfirmada)
                                //    {
                                //        DTProductosRecepcion[e.RowIndex].RejectChanges();
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
                                //        foreach (DataRow DRProductoEspecifico in formSeleccionEspecificos.DTProductosEspecificos.Rows)
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
                            
                            DTProductosRecepcion[e.RowIndex].RejectChanges();
                            DTProductosRecepcion[e.RowIndex].Seleccionar = false;
                            DTProductosRecepcion.AcceptChanges();
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
            
            if (MessageBox.Show(this, "La recepción total o Parcial de los Productos seleccionados implica la actualización en inventarios de acuerdo a la cantidad?" +
                " selecionada. ¿Desea confirmar la accion?", "Confirmación de recepción", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            Button btnAccion = (Button)sender;
            String tipoRecepcion = "T"; // T-> Total, "P"-> parcial, "X"-> Incompleta
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

            if(btnAccion.Equals(btnConfirmarTotal))
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
            }
            try
            {
                DateTime FechaHoraRegistro = Utilidades.DAOUtilidades.ObtenerFechaHoraServidor();

                foreach (AlvecoComercial10DataSet.DTProductosRecepcionEntregaRow
                    DRProductos in DTProductosSeleccionados.Rows)
                {                    
                    TAComprasProductosDetalleEntrega.Insert(NumeroAlmacen, NumeroCompraProducto, DRProductos.CodigoProducto, DRProductos.CantidadRecepcionEntrega, FechaHoraRegistro);
                    TAInventariosProductosCantidadesTransaccionesHistorial.Insert(
                        NumeroAlmacen, NumeroCompraProducto, DRProductos.CodigoProducto,
                        "C", FechaHoraRegistro, DRProductos.CantidadRecepcionEntrega,
                        DTProductosRecepcion.FindByCodigoProducto(DRProductos.CodigoProducto).PrecioUnitarioCompra);
                        

                    //foreach (AlvecoComercial10DataSet.DTProductosRecepcionEntregaEspecificosRow
                    //    DRProductoEspecifico in DTProductosSeleccionadosEspecificos.Select("CodigoProducto = '" + DRProductos.CodigoProducto + "'"))
                    //{
                    //    _ComprasProductosEspecificosCLN.InsertarCompraProductoEspecifico(NumeroAlmacen,
                    //        NumeroCompraProducto, DRProductoEspecifico.CodigoProducto, DRProductoEspecifico.CodigoProductoEspecifico,
                    //        0, FechaHoraRegistro, FechaHoraRegistro);

                    //    _InventariosProductosEspecificosCLN.InsertarInventarioProductoEspecifico(NumeroAlmacen,
                    //        DRProductos.CodigoProducto, DRProductoEspecifico.CodigoProductoEspecifico,
                    //        0, FechaHoraRegistro, "C", "A");
                    //}
                }

                TAComprasProductos.ActualizarInventarioComprasProductos(NumeroAlmacen, NumeroCompraProducto, FechaHoraRegistro);

                int CantidadTotal = int.Parse(DTProductosRecepcion.Compute("sum(CantidadFaltante)", "").ToString());
                int CantidadRecepcionada = int.Parse(DTProductosSeleccionados.Compute("sum(CantidadRecepcionEntrega)", "").ToString());
                if (btnAccion.Equals(btnConfirmarTotal) || btnAccion.Equals(btnConfirmarParcial))
                {
                    TAComprasProductos.ActualizarCodigoEstadoCompra(NumeroAlmacen, NumeroCompraProducto,
                            CantidadTotal == CantidadRecepcionada ? "F" : "D");
                    this.DialogResult = CantidadTotal == CantidadRecepcionada ?DialogResult.OK : DialogResult.Ignore;
                }
                //else if ()
                //{
                //    _ComprasProductosCLN.ActualizarCompraProducto(NumeroAlmacen, NumeroCompraProducto, "D");
                //    this.DialogResult = DialogResult.Ignore;

                //}
                else
                {
                    TAComprasProductos.ActualizarCodigoEstadoCompra(NumeroAlmacen, NumeroCompraProducto, "X");
                    this.DialogResult = DialogResult.Ignore;
                }

                
                //MessageBox.Show(this, "Se recepcionó correcatmente los Productos Seleccionados", "Recepción de Artículos",
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

        private void FComprasProductosRecepcion_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (this.DialogResult == DialogResult.Cancel
            //    && MessageBox.Show(this,"¿Se encuentra seguro de cancelar la Operación Actual?","Recepción de Productos",
            //    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            //{
            //    e.Cancel = true;
            //    return;
            //}
        }
    }
}
