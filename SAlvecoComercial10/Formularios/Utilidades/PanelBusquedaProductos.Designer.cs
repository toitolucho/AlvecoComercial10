namespace SAlvecoComercial10.Formularios.Utilidades
{
    partial class PanelBusquedaProductos
    {
        /// <summary> 
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar 
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlDatosBusqueda = new System.Windows.Forms.Panel();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.txtCantidadExistencia = new System.Windows.Forms.TextBox();
            this.lblExistencia = new System.Windows.Forms.Label();
            this.txtTextoBusqueda = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gBoxDetalleProductos = new System.Windows.Forms.GroupBox();
            this.dtGVProductosBusqueda = new System.Windows.Forms.DataGridView();
            this.DGCCodigoProducto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGCNombreProducto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGCNombreMarca = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGCCantidadExistencia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGCPrecioUnitarioCompra = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGCPrecioUnitarioVentaPorMayor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGCPrecioUnitarioVentaPorMenor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGCTiempoGarantiaProducto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblCantidadProductos = new System.Windows.Forms.Label();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlDatosBusqueda.SuspendLayout();
            this.gBoxDetalleProductos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtGVProductosBusqueda)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDatosBusqueda
            // 
            this.pnlDatosBusqueda.Controls.Add(this.btnBuscar);
            this.pnlDatosBusqueda.Controls.Add(this.txtCantidadExistencia);
            this.pnlDatosBusqueda.Controls.Add(this.lblExistencia);
            this.pnlDatosBusqueda.Controls.Add(this.txtTextoBusqueda);
            this.pnlDatosBusqueda.Controls.Add(this.label1);
            this.pnlDatosBusqueda.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDatosBusqueda.Location = new System.Drawing.Point(0, 0);
            this.pnlDatosBusqueda.Margin = new System.Windows.Forms.Padding(4);
            this.pnlDatosBusqueda.Name = "pnlDatosBusqueda";
            this.pnlDatosBusqueda.Size = new System.Drawing.Size(748, 42);
            this.pnlDatosBusqueda.TabIndex = 0;
            // 
            // btnBuscar
            // 
            this.btnBuscar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBuscar.Image = global::SAlvecoComercial10.Properties.Resources.FILEFIND;
            this.btnBuscar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBuscar.Location = new System.Drawing.Point(644, 7);
            this.btnBuscar.Margin = new System.Windows.Forms.Padding(4);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(87, 34);
            this.btnBuscar.TabIndex = 4;
            this.btnBuscar.Text = "&Buscar";
            this.btnBuscar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // txtCantidadExistencia
            // 
            this.txtCantidadExistencia.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCantidadExistencia.Location = new System.Drawing.Point(503, 9);
            this.txtCantidadExistencia.Margin = new System.Windows.Forms.Padding(4);
            this.txtCantidadExistencia.Name = "txtCantidadExistencia";
            this.txtCantidadExistencia.Size = new System.Drawing.Size(132, 22);
            this.txtCantidadExistencia.TabIndex = 3;
            // 
            // lblExistencia
            // 
            this.lblExistencia.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblExistencia.AutoSize = true;
            this.lblExistencia.Location = new System.Drawing.Point(401, 14);
            this.lblExistencia.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblExistencia.Name = "lblExistencia";
            this.lblExistencia.Size = new System.Drawing.Size(91, 17);
            this.lblExistencia.TabIndex = 2;
            this.lblExistencia.Text = "Existencia >=";
            // 
            // txtTextoBusqueda
            // 
            this.txtTextoBusqueda.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTextoBusqueda.Location = new System.Drawing.Point(145, 9);
            this.txtTextoBusqueda.Margin = new System.Windows.Forms.Padding(4);
            this.txtTextoBusqueda.Name = "txtTextoBusqueda";
            this.txtTextoBusqueda.Size = new System.Drawing.Size(247, 22);
            this.txtTextoBusqueda.TabIndex = 1;
            this.txtTextoBusqueda.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTextoBusqueda_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Texto de Busqueda";
            // 
            // gBoxDetalleProductos
            // 
            this.gBoxDetalleProductos.Controls.Add(this.dtGVProductosBusqueda);
            this.gBoxDetalleProductos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gBoxDetalleProductos.Location = new System.Drawing.Point(0, 42);
            this.gBoxDetalleProductos.Margin = new System.Windows.Forms.Padding(4);
            this.gBoxDetalleProductos.Name = "gBoxDetalleProductos";
            this.gBoxDetalleProductos.Padding = new System.Windows.Forms.Padding(4);
            this.gBoxDetalleProductos.Size = new System.Drawing.Size(748, 225);
            this.gBoxDetalleProductos.TabIndex = 1;
            this.gBoxDetalleProductos.TabStop = false;
            this.gBoxDetalleProductos.Text = "Detalle de Productos Encontrados";
            // 
            // dtGVProductosBusqueda
            // 
            this.dtGVProductosBusqueda.AllowUserToAddRows = false;
            this.dtGVProductosBusqueda.AllowUserToDeleteRows = false;
            this.dtGVProductosBusqueda.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtGVProductosBusqueda.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dtGVProductosBusqueda.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtGVProductosBusqueda.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DGCCodigoProducto,
            this.DGCNombreProducto,
            this.DGCNombreMarca,
            this.DGCCantidadExistencia,
            this.DGCPrecioUnitarioCompra,
            this.DGCPrecioUnitarioVentaPorMayor,
            this.DGCPrecioUnitarioVentaPorMenor,
            this.DGCTiempoGarantiaProducto});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtGVProductosBusqueda.DefaultCellStyle = dataGridViewCellStyle2;
            this.dtGVProductosBusqueda.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtGVProductosBusqueda.Location = new System.Drawing.Point(4, 19);
            this.dtGVProductosBusqueda.Margin = new System.Windows.Forms.Padding(4);
            this.dtGVProductosBusqueda.Name = "dtGVProductosBusqueda";
            this.dtGVProductosBusqueda.ReadOnly = true;
            this.dtGVProductosBusqueda.RowHeadersVisible = false;
            this.dtGVProductosBusqueda.RowTemplate.Height = 24;
            this.dtGVProductosBusqueda.Size = new System.Drawing.Size(740, 202);
            this.dtGVProductosBusqueda.TabIndex = 0;
            this.dtGVProductosBusqueda.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtGVProductosBusqueda_CellDoubleClick);
            // 
            // DGCCodigoProducto
            // 
            this.DGCCodigoProducto.DataPropertyName = "CodigoProducto";
            this.DGCCodigoProducto.HeaderText = "Código";
            this.DGCCodigoProducto.Name = "DGCCodigoProducto";
            this.DGCCodigoProducto.ReadOnly = true;
            this.DGCCodigoProducto.ToolTipText = "Código del Producto";
            // 
            // DGCNombreProducto
            // 
            this.DGCNombreProducto.DataPropertyName = "NombreProducto";
            this.DGCNombreProducto.HeaderText = "Producto";
            this.DGCNombreProducto.Name = "DGCNombreProducto";
            this.DGCNombreProducto.ReadOnly = true;
            this.DGCNombreProducto.ToolTipText = "Nombre o Descripción del Producto";
            // 
            // DGCNombreMarca
            // 
            this.DGCNombreMarca.DataPropertyName = "NombreMarca";
            this.DGCNombreMarca.HeaderText = "Marca";
            this.DGCNombreMarca.Name = "DGCNombreMarca";
            this.DGCNombreMarca.ReadOnly = true;
            this.DGCNombreMarca.ToolTipText = "Marca del Producto";
            // 
            // DGCCantidadExistencia
            // 
            this.DGCCantidadExistencia.DataPropertyName = "CantidadExistencia";
            this.DGCCantidadExistencia.HeaderText = "Existencia";
            this.DGCCantidadExistencia.Name = "DGCCantidadExistencia";
            this.DGCCantidadExistencia.ReadOnly = true;
            this.DGCCantidadExistencia.ToolTipText = "Cantidad de Existencia en Inventario";
            // 
            // DGCPrecioUnitarioCompra
            // 
            this.DGCPrecioUnitarioCompra.DataPropertyName = "PrecioUnitarioCompra";
            this.DGCPrecioUnitarioCompra.HeaderText = "P. Compra";
            this.DGCPrecioUnitarioCompra.Name = "DGCPrecioUnitarioCompra";
            this.DGCPrecioUnitarioCompra.ReadOnly = true;
            this.DGCPrecioUnitarioCompra.ToolTipText = "Precio Unitario de Compra";
            // 
            // DGCPrecioUnitarioVentaPorMayor
            // 
            this.DGCPrecioUnitarioVentaPorMayor.DataPropertyName = "PrecioUnitarioVentaPorMayor";
            this.DGCPrecioUnitarioVentaPorMayor.HeaderText = "P. Venta Por Mayor";
            this.DGCPrecioUnitarioVentaPorMayor.Name = "DGCPrecioUnitarioVentaPorMayor";
            this.DGCPrecioUnitarioVentaPorMayor.ReadOnly = true;
            this.DGCPrecioUnitarioVentaPorMayor.ToolTipText = "Precio Unitario de Venta Por Mayor";
            // 
            // DGCPrecioUnitarioVentaPorMenor
            // 
            this.DGCPrecioUnitarioVentaPorMenor.DataPropertyName = "PrecioUnitarioVentaPorMenor";
            this.DGCPrecioUnitarioVentaPorMenor.HeaderText = "P. Venta Por Menor";
            this.DGCPrecioUnitarioVentaPorMenor.Name = "DGCPrecioUnitarioVentaPorMenor";
            this.DGCPrecioUnitarioVentaPorMenor.ReadOnly = true;
            this.DGCPrecioUnitarioVentaPorMenor.ToolTipText = "Precio Unitario de Venta por Menor";
            // 
            // DGCTiempoGarantiaProducto
            // 
            this.DGCTiempoGarantiaProducto.DataPropertyName = "TiempoGarantiaProducto";
            this.DGCTiempoGarantiaProducto.HeaderText = "Garantia Dias";
            this.DGCTiempoGarantiaProducto.Name = "DGCTiempoGarantiaProducto";
            this.DGCTiempoGarantiaProducto.ReadOnly = true;
            this.DGCTiempoGarantiaProducto.ToolTipText = "Garantia del Producto en Días";
            // 
            // lblCantidadProductos
            // 
            this.lblCantidadProductos.AutoSize = true;
            this.lblCantidadProductos.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblCantidadProductos.Location = new System.Drawing.Point(0, 267);
            this.lblCantidadProductos.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCantidadProductos.Name = "lblCantidadProductos";
            this.lblCantidadProductos.Size = new System.Drawing.Size(232, 17);
            this.lblCantidadProductos.TabIndex = 0;
            this.lblCantidadProductos.Text = "Cantidad de Registros Encontrados";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "CodigoProducto";
            this.dataGridViewTextBoxColumn1.HeaderText = "Código";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.ToolTipText = "Código del Producto";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "NombreProducto";
            this.dataGridViewTextBoxColumn2.HeaderText = "Producto";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.ToolTipText = "Nombre o Descripción del Producto";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "NombreMarca";
            this.dataGridViewTextBoxColumn3.HeaderText = "Marca";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.ToolTipText = "Marca del Producto";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "CantidadExistencia";
            this.dataGridViewTextBoxColumn4.HeaderText = "Existencia";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.ToolTipText = "Cantidad de Existencia en Inventario";
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "PrecioUnitarioCompra";
            this.dataGridViewTextBoxColumn5.HeaderText = "P. Compra";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.ToolTipText = "Precio Unitario de Compra";
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "PrecioUnitarioVentaPorMayor";
            this.dataGridViewTextBoxColumn6.HeaderText = "P. Venta Por Mayor";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.ToolTipText = "Precio Unitario de Venta Por Mayor";
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "PrecioUnitarioVentaPorMenor";
            this.dataGridViewTextBoxColumn7.HeaderText = "P. Venta Por Menor";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.ToolTipText = "Precio Unitario de Venta por Menor";
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "TiempoGarantiaProducto";
            this.dataGridViewTextBoxColumn8.HeaderText = "Garantia Dias";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            this.dataGridViewTextBoxColumn8.ToolTipText = "Garantia del Producto en Días";
            // 
            // PanelBusquedaProductos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gBoxDetalleProductos);
            this.Controls.Add(this.lblCantidadProductos);
            this.Controls.Add(this.pnlDatosBusqueda);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PanelBusquedaProductos";
            this.Size = new System.Drawing.Size(748, 284);
            this.pnlDatosBusqueda.ResumeLayout(false);
            this.pnlDatosBusqueda.PerformLayout();
            this.gBoxDetalleProductos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtGVProductosBusqueda)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlDatosBusqueda;
        private System.Windows.Forms.GroupBox gBoxDetalleProductos;
        private System.Windows.Forms.DataGridView dtGVProductosBusqueda;
        private System.Windows.Forms.Label lblCantidadProductos;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.TextBox txtCantidadExistencia;
        private System.Windows.Forms.Label lblExistencia;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGCCodigoProducto;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGCNombreProducto;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGCNombreMarca;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGCCantidadExistencia;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGCPrecioUnitarioCompra;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGCPrecioUnitarioVentaPorMayor;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGCPrecioUnitarioVentaPorMenor;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGCTiempoGarantiaProducto;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        public System.Windows.Forms.TextBox txtTextoBusqueda;
    }
}
