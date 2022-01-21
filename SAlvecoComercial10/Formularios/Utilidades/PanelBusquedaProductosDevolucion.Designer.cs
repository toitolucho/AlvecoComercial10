namespace SAlvecoComercial10.Formularios.Utilidades
{
    partial class PanelBusquedaProductosDevolucion
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
            this.dtGVProductosBusqueda = new System.Windows.Forms.DataGridView();
            this.DGCCodigoProducto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGCNombreProducto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGCNombreMarca = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGCCantidadEntregada = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGCCantidadDevolucion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGCPrecioUnitarioTransaccion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.gBoxDetalleProductos = new System.Windows.Forms.GroupBox();
            this.pnlDatosBusqueda = new System.Windows.Forms.Panel();
            this.txtTextoBusqueda = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblCantidadProductos = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dtGVProductosBusqueda)).BeginInit();
            this.gBoxDetalleProductos.SuspendLayout();
            this.pnlDatosBusqueda.SuspendLayout();
            this.SuspendLayout();
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
            this.DGCCantidadEntregada,
            this.DGCCantidadDevolucion,
            this.DGCPrecioUnitarioTransaccion});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtGVProductosBusqueda.DefaultCellStyle = dataGridViewCellStyle2;
            this.dtGVProductosBusqueda.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtGVProductosBusqueda.Location = new System.Drawing.Point(3, 16);
            this.dtGVProductosBusqueda.Name = "dtGVProductosBusqueda";
            this.dtGVProductosBusqueda.ReadOnly = true;
            this.dtGVProductosBusqueda.RowHeadersVisible = false;
            this.dtGVProductosBusqueda.Size = new System.Drawing.Size(546, 197);
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
            // DGCCantidadEntregada
            // 
            this.DGCCantidadEntregada.DataPropertyName = "CantidadEntregada";
            this.DGCCantidadEntregada.HeaderText = "Cantidad Transaccion";
            this.DGCCantidadEntregada.Name = "DGCCantidadEntregada";
            this.DGCCantidadEntregada.ReadOnly = true;
            this.DGCCantidadEntregada.ToolTipText = "Cantidad Vendida o comprada";
            // 
            // DGCCantidadDevolucion
            // 
            this.DGCCantidadDevolucion.DataPropertyName = "CantidadDevolucion";
            this.DGCCantidadDevolucion.HeaderText = "Cantidad Devuelta";
            this.DGCCantidadDevolucion.Name = "DGCCantidadDevolucion";
            this.DGCCantidadDevolucion.ReadOnly = true;
            // 
            // DGCPrecioUnitarioTransaccion
            // 
            this.DGCPrecioUnitarioTransaccion.DataPropertyName = "PrecioUnitarioTransaccion";
            this.DGCPrecioUnitarioTransaccion.HeaderText = "Precio";
            this.DGCPrecioUnitarioTransaccion.Name = "DGCPrecioUnitarioTransaccion";
            this.DGCPrecioUnitarioTransaccion.ReadOnly = true;
            this.DGCPrecioUnitarioTransaccion.ToolTipText = "Precio Unitario de Compra o Venta";
            // 
            // btnBuscar
            // 
            this.btnBuscar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBuscar.Image = global::SAlvecoComercial10.Properties.Resources.FILEFIND;
            this.btnBuscar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBuscar.Location = new System.Drawing.Point(474, 6);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(67, 28);
            this.btnBuscar.TabIndex = 4;
            this.btnBuscar.Text = "&Buscar";
            this.btnBuscar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // gBoxDetalleProductos
            // 
            this.gBoxDetalleProductos.Controls.Add(this.dtGVProductosBusqueda);
            this.gBoxDetalleProductos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gBoxDetalleProductos.Location = new System.Drawing.Point(0, 34);
            this.gBoxDetalleProductos.Name = "gBoxDetalleProductos";
            this.gBoxDetalleProductos.Size = new System.Drawing.Size(552, 216);
            this.gBoxDetalleProductos.TabIndex = 4;
            this.gBoxDetalleProductos.TabStop = false;
            this.gBoxDetalleProductos.Text = "Detalle de Productos Encontrados";
            // 
            // pnlDatosBusqueda
            // 
            this.pnlDatosBusqueda.Controls.Add(this.btnBuscar);
            this.pnlDatosBusqueda.Controls.Add(this.txtTextoBusqueda);
            this.pnlDatosBusqueda.Controls.Add(this.label1);
            this.pnlDatosBusqueda.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDatosBusqueda.Location = new System.Drawing.Point(0, 0);
            this.pnlDatosBusqueda.Name = "pnlDatosBusqueda";
            this.pnlDatosBusqueda.Size = new System.Drawing.Size(552, 34);
            this.pnlDatosBusqueda.TabIndex = 3;
            // 
            // txtTextoBusqueda
            // 
            this.txtTextoBusqueda.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTextoBusqueda.Location = new System.Drawing.Point(109, 7);
            this.txtTextoBusqueda.Name = "txtTextoBusqueda";
            this.txtTextoBusqueda.Size = new System.Drawing.Size(359, 20);
            this.txtTextoBusqueda.TabIndex = 1;
            this.txtTextoBusqueda.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTextoBusqueda_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Texto de Busqueda";
            // 
            // lblCantidadProductos
            // 
            this.lblCantidadProductos.AutoSize = true;
            this.lblCantidadProductos.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblCantidadProductos.Location = new System.Drawing.Point(0, 250);
            this.lblCantidadProductos.Name = "lblCantidadProductos";
            this.lblCantidadProductos.Size = new System.Drawing.Size(174, 13);
            this.lblCantidadProductos.TabIndex = 2;
            this.lblCantidadProductos.Text = "Cantidad de Registros Encontrados";
            // 
            // PanelBusquedaProductosDevolucion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gBoxDetalleProductos);
            this.Controls.Add(this.pnlDatosBusqueda);
            this.Controls.Add(this.lblCantidadProductos);
            this.Name = "PanelBusquedaProductosDevolucion";
            this.Size = new System.Drawing.Size(552, 263);
            ((System.ComponentModel.ISupportInitialize)(this.dtGVProductosBusqueda)).EndInit();
            this.gBoxDetalleProductos.ResumeLayout(false);
            this.pnlDatosBusqueda.ResumeLayout(false);
            this.pnlDatosBusqueda.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dtGVProductosBusqueda;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.GroupBox gBoxDetalleProductos;
        private System.Windows.Forms.Panel pnlDatosBusqueda;
        private System.Windows.Forms.TextBox txtTextoBusqueda;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblCantidadProductos;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGCCodigoProducto;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGCNombreProducto;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGCNombreMarca;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGCCantidadEntregada;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGCCantidadDevolucion;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGCPrecioUnitarioTransaccion;
    }
}
