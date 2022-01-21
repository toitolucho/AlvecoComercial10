namespace SAlvecoComercial10.Formularios.GestionComercial
{
    partial class FComprasProductosRecepcion
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gBoxListadoRecepcion = new System.Windows.Forms.GroupBox();
            this.dtGVProductosListado = new System.Windows.Forms.DataGridView();
            this.gBoxListadoProductosSeleccionados = new System.Windows.Forms.GroupBox();
            this.dtGVProductosCantidadesSeleccionadas = new System.Windows.Forms.DataGridView();
            this.DGCCodigoProductoN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGCNombreProductoN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGCCantidadRecepcionEntrega = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gBoxListadoProductosEspecificosSeleccionados = new System.Windows.Forms.GroupBox();
            this.dtGVProductosEspecificosSeleccionados = new System.Windows.Forms.DataGridView();
            this.DGCNombreProductoAE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGCCodigoProductoAE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGCCodigoEspecifico = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnConfirmarForzada = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnConfirmarTotal = new System.Windows.Forms.Button();
            this.btnConfirmarParcial = new System.Windows.Forms.Button();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGCCodigoProducto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGCNombreProducto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGCCantidadIngreso = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGCCantidadRecepcionada = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGCEsProductoEspecifico = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.DGCSeleccionar = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.gBoxListadoRecepcion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtGVProductosListado)).BeginInit();
            this.gBoxListadoProductosSeleccionados.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtGVProductosCantidadesSeleccionadas)).BeginInit();
            this.gBoxListadoProductosEspecificosSeleccionados.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtGVProductosEspecificosSeleccionados)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gBoxListadoRecepcion
            // 
            this.gBoxListadoRecepcion.Controls.Add(this.dtGVProductosListado);
            this.gBoxListadoRecepcion.Dock = System.Windows.Forms.DockStyle.Top;
            this.gBoxListadoRecepcion.Location = new System.Drawing.Point(0, 0);
            this.gBoxListadoRecepcion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gBoxListadoRecepcion.Name = "gBoxListadoRecepcion";
            this.gBoxListadoRecepcion.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gBoxListadoRecepcion.Size = new System.Drawing.Size(993, 123);
            this.gBoxListadoRecepcion.TabIndex = 0;
            this.gBoxListadoRecepcion.TabStop = false;
            this.gBoxListadoRecepcion.Text = "Listado de Productos a Recepcionar";
            // 
            // dtGVProductosListado
            // 
            this.dtGVProductosListado.AllowUserToAddRows = false;
            this.dtGVProductosListado.AllowUserToDeleteRows = false;
            this.dtGVProductosListado.AllowUserToResizeRows = false;
            this.dtGVProductosListado.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtGVProductosListado.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dtGVProductosListado.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtGVProductosListado.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DGCCodigoProducto,
            this.DGCNombreProducto,
            this.DGCCantidadIngreso,
            this.DGCCantidadRecepcionada,
            this.DGCEsProductoEspecifico,
            this.DGCSeleccionar});
            this.dtGVProductosListado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtGVProductosListado.Location = new System.Drawing.Point(4, 20);
            this.dtGVProductosListado.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dtGVProductosListado.Name = "dtGVProductosListado";
            this.dtGVProductosListado.RowHeadersVisible = false;
            this.dtGVProductosListado.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dtGVProductosListado.Size = new System.Drawing.Size(985, 100);
            this.dtGVProductosListado.TabIndex = 0;
            this.dtGVProductosListado.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtGVProductosListado_CellValueChanged);
            this.dtGVProductosListado.CurrentCellDirtyStateChanged += new System.EventHandler(this.dtGVProductosListado_CurrentCellDirtyStateChanged);
            // 
            // gBoxListadoProductosSeleccionados
            // 
            this.gBoxListadoProductosSeleccionados.Controls.Add(this.dtGVProductosCantidadesSeleccionadas);
            this.gBoxListadoProductosSeleccionados.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gBoxListadoProductosSeleccionados.Location = new System.Drawing.Point(0, 123);
            this.gBoxListadoProductosSeleccionados.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gBoxListadoProductosSeleccionados.Name = "gBoxListadoProductosSeleccionados";
            this.gBoxListadoProductosSeleccionados.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gBoxListadoProductosSeleccionados.Size = new System.Drawing.Size(993, 260);
            this.gBoxListadoProductosSeleccionados.TabIndex = 1;
            this.gBoxListadoProductosSeleccionados.TabStop = false;
            this.gBoxListadoProductosSeleccionados.Text = "Productos Seleccionados";
            // 
            // dtGVProductosCantidadesSeleccionadas
            // 
            this.dtGVProductosCantidadesSeleccionadas.AllowUserToAddRows = false;
            this.dtGVProductosCantidadesSeleccionadas.AllowUserToDeleteRows = false;
            this.dtGVProductosCantidadesSeleccionadas.AllowUserToResizeRows = false;
            this.dtGVProductosCantidadesSeleccionadas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtGVProductosCantidadesSeleccionadas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dtGVProductosCantidadesSeleccionadas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtGVProductosCantidadesSeleccionadas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DGCCodigoProductoN,
            this.DGCNombreProductoN,
            this.DGCCantidadRecepcionEntrega});
            this.dtGVProductosCantidadesSeleccionadas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtGVProductosCantidadesSeleccionadas.Location = new System.Drawing.Point(4, 20);
            this.dtGVProductosCantidadesSeleccionadas.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dtGVProductosCantidadesSeleccionadas.Name = "dtGVProductosCantidadesSeleccionadas";
            this.dtGVProductosCantidadesSeleccionadas.Size = new System.Drawing.Size(985, 236);
            this.dtGVProductosCantidadesSeleccionadas.TabIndex = 0;
            this.dtGVProductosCantidadesSeleccionadas.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dtGVProductosCantidadesSeleccionadas_CellValidating);
            this.dtGVProductosCantidadesSeleccionadas.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtGVProductosCantidadesSeleccionadas_CellValueChanged);
            // 
            // DGCCodigoProductoN
            // 
            this.DGCCodigoProductoN.DataPropertyName = "CodigoProducto";
            this.DGCCodigoProductoN.HeaderText = "Código";
            this.DGCCodigoProductoN.Name = "DGCCodigoProductoN";
            this.DGCCodigoProductoN.ReadOnly = true;
            // 
            // DGCNombreProductoN
            // 
            this.DGCNombreProductoN.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.DGCNombreProductoN.DataPropertyName = "NombreProducto";
            this.DGCNombreProductoN.HeaderText = "Artículo";
            this.DGCNombreProductoN.Name = "DGCNombreProductoN";
            this.DGCNombreProductoN.ReadOnly = true;
            this.DGCNombreProductoN.Width = 80;
            // 
            // DGCCantidadRecepcionEntrega
            // 
            this.DGCCantidadRecepcionEntrega.DataPropertyName = "CantidadRecepcionEntrega";
            this.DGCCantidadRecepcionEntrega.HeaderText = "Cantidad";
            this.DGCCantidadRecepcionEntrega.Name = "DGCCantidadRecepcionEntrega";
            // 
            // gBoxListadoProductosEspecificosSeleccionados
            // 
            this.gBoxListadoProductosEspecificosSeleccionados.Controls.Add(this.dtGVProductosEspecificosSeleccionados);
            this.gBoxListadoProductosEspecificosSeleccionados.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gBoxListadoProductosEspecificosSeleccionados.Location = new System.Drawing.Point(0, 383);
            this.gBoxListadoProductosEspecificosSeleccionados.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gBoxListadoProductosEspecificosSeleccionados.Name = "gBoxListadoProductosEspecificosSeleccionados";
            this.gBoxListadoProductosEspecificosSeleccionados.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gBoxListadoProductosEspecificosSeleccionados.Size = new System.Drawing.Size(993, 123);
            this.gBoxListadoProductosEspecificosSeleccionados.TabIndex = 2;
            this.gBoxListadoProductosEspecificosSeleccionados.TabStop = false;
            this.gBoxListadoProductosEspecificosSeleccionados.Text = "Listado Nros de Series";
            this.gBoxListadoProductosEspecificosSeleccionados.Visible = false;
            // 
            // dtGVProductosEspecificosSeleccionados
            // 
            this.dtGVProductosEspecificosSeleccionados.AllowUserToAddRows = false;
            this.dtGVProductosEspecificosSeleccionados.AllowUserToDeleteRows = false;
            this.dtGVProductosEspecificosSeleccionados.AllowUserToResizeRows = false;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtGVProductosEspecificosSeleccionados.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dtGVProductosEspecificosSeleccionados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtGVProductosEspecificosSeleccionados.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DGCNombreProductoAE,
            this.DGCCodigoProductoAE,
            this.DGCCodigoEspecifico});
            this.dtGVProductosEspecificosSeleccionados.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtGVProductosEspecificosSeleccionados.Location = new System.Drawing.Point(4, 20);
            this.dtGVProductosEspecificosSeleccionados.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dtGVProductosEspecificosSeleccionados.MultiSelect = false;
            this.dtGVProductosEspecificosSeleccionados.Name = "dtGVProductosEspecificosSeleccionados";
            this.dtGVProductosEspecificosSeleccionados.ReadOnly = true;
            this.dtGVProductosEspecificosSeleccionados.RowHeadersVisible = false;
            this.dtGVProductosEspecificosSeleccionados.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dtGVProductosEspecificosSeleccionados.Size = new System.Drawing.Size(985, 100);
            this.dtGVProductosEspecificosSeleccionados.TabIndex = 0;
            this.dtGVProductosEspecificosSeleccionados.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dtGVProductosEspecificosSeleccionados_CellFormatting);
            // 
            // DGCNombreProductoAE
            // 
            this.DGCNombreProductoAE.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DGCNombreProductoAE.DataPropertyName = "NombreProducto";
            this.DGCNombreProductoAE.HeaderText = "Artículo";
            this.DGCNombreProductoAE.Name = "DGCNombreProductoAE";
            this.DGCNombreProductoAE.ReadOnly = true;
            // 
            // DGCCodigoProductoAE
            // 
            this.DGCCodigoProductoAE.DataPropertyName = "CodigoProducto";
            this.DGCCodigoProductoAE.FillWeight = 50F;
            this.DGCCodigoProductoAE.HeaderText = "Código";
            this.DGCCodigoProductoAE.Name = "DGCCodigoProductoAE";
            this.DGCCodigoProductoAE.ReadOnly = true;
            // 
            // DGCCodigoEspecifico
            // 
            this.DGCCodigoEspecifico.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.DGCCodigoEspecifico.DataPropertyName = "CodigoProductoEspecifico";
            this.DGCCodigoEspecifico.FillWeight = 70F;
            this.DGCCodigoEspecifico.HeaderText = "Nro Serie";
            this.DGCCodigoEspecifico.Name = "DGCCodigoEspecifico";
            this.DGCCodigoEspecifico.ReadOnly = true;
            this.DGCCodigoEspecifico.Width = 93;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnConfirmarForzada);
            this.panel1.Controls.Add(this.btnCancelar);
            this.panel1.Controls.Add(this.btnConfirmarTotal);
            this.panel1.Controls.Add(this.btnConfirmarParcial);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 506);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(993, 59);
            this.panel1.TabIndex = 3;
            // 
            // btnConfirmarForzada
            // 
            this.btnConfirmarForzada.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirmarForzada.Image = global::SAlvecoComercial10.Properties.Resources.accept;
            this.btnConfirmarForzada.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnConfirmarForzada.Location = new System.Drawing.Point(585, 16);
            this.btnConfirmarForzada.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnConfirmarForzada.Name = "btnConfirmarForzada";
            this.btnConfirmarForzada.Size = new System.Drawing.Size(163, 28);
            this.btnConfirmarForzada.TabIndex = 3;
            this.btnConfirmarForzada.Text = "&Forzar Confirmacion";
            this.btnConfirmarForzada.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnConfirmarForzada.UseVisualStyleBackColor = true;
            this.btnConfirmarForzada.Visible = false;
            this.btnConfirmarForzada.Click += new System.EventHandler(this.btnConfirmarTotal_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelar.Image = global::SAlvecoComercial10.Properties.Resources.arrow_rotate_clockwise;
            this.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancelar.Location = new System.Drawing.Point(877, 16);
            this.btnCancelar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(100, 28);
            this.btnCancelar.TabIndex = 2;
            this.btnCancelar.Text = "&Cancelar";
            this.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnConfirmarTotal
            // 
            this.btnConfirmarTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirmarTotal.Location = new System.Drawing.Point(207, 16);
            this.btnConfirmarTotal.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnConfirmarTotal.Name = "btnConfirmarTotal";
            this.btnConfirmarTotal.Size = new System.Drawing.Size(129, 28);
            this.btnConfirmarTotal.TabIndex = 1;
            this.btnConfirmarTotal.Text = "&Confirmar Todo";
            this.btnConfirmarTotal.UseVisualStyleBackColor = true;
            this.btnConfirmarTotal.Visible = false;
            this.btnConfirmarTotal.Click += new System.EventHandler(this.btnConfirmarTotal_Click);
            // 
            // btnConfirmarParcial
            // 
            this.btnConfirmarParcial.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirmarParcial.Image = global::SAlvecoComercial10.Properties.Resources.accept;
            this.btnConfirmarParcial.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnConfirmarParcial.Location = new System.Drawing.Point(760, 16);
            this.btnConfirmarParcial.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnConfirmarParcial.Name = "btnConfirmarParcial";
            this.btnConfirmarParcial.Size = new System.Drawing.Size(99, 28);
            this.btnConfirmarParcial.TabIndex = 0;
            this.btnConfirmarParcial.Text = "Confirmar";
            this.btnConfirmarParcial.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnConfirmarParcial.UseVisualStyleBackColor = true;
            this.btnConfirmarParcial.Click += new System.EventHandler(this.btnConfirmarTotal_Click);
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 379);
            this.splitter1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(993, 4);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter2.Location = new System.Drawing.Point(0, 123);
            this.splitter2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(993, 4);
            this.splitter2.TabIndex = 2;
            this.splitter2.TabStop = false;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "CodigoProducto";
            this.dataGridViewTextBoxColumn1.FillWeight = 50F;
            this.dataGridViewTextBoxColumn1.HeaderText = "Código";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "NombreProducto";
            this.dataGridViewTextBoxColumn2.FillWeight = 50F;
            this.dataGridViewTextBoxColumn2.HeaderText = "Artículo";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridViewTextBoxColumn3.DataPropertyName = "CodigoEspecifico";
            this.dataGridViewTextBoxColumn3.FillWeight = 50F;
            this.dataGridViewTextBoxColumn3.HeaderText = "Nro Serie";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "CodigoProducto";
            this.dataGridViewTextBoxColumn4.HeaderText = "Código";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.ToolTipText = "Código Identificador de Artículo";
            this.dataGridViewTextBoxColumn4.Width = 116;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridViewTextBoxColumn5.DataPropertyName = "NombreProducto";
            this.dataGridViewTextBoxColumn5.HeaderText = "Artículo";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.ToolTipText = "Nombre o Descripción del Artículo";
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "CantidadIngreso";
            this.dataGridViewTextBoxColumn6.HeaderText = "Cant Solicitada";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.ToolTipText = "Cantidad solicitada en el Ingreso";
            this.dataGridViewTextBoxColumn6.Width = 115;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "CantidadRecepcionada";
            this.dataGridViewTextBoxColumn7.HeaderText = "Recepcionados";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.ToolTipText = "Cantidad Recepcionada Actualmente";
            this.dataGridViewTextBoxColumn7.Width = 116;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "CodigoProducto";
            this.dataGridViewTextBoxColumn8.HeaderText = "Código";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            this.dataGridViewTextBoxColumn8.Width = 269;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridViewTextBoxColumn9.DataPropertyName = "NombreProducto";
            this.dataGridViewTextBoxColumn9.HeaderText = "Artículo";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.DataPropertyName = "CantidadRecepcionEntrega";
            this.dataGridViewTextBoxColumn10.HeaderText = "Cantidad";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.Width = 269;
            // 
            // DGCCodigoProducto
            // 
            this.DGCCodigoProducto.DataPropertyName = "CodigoProducto";
            this.DGCCodigoProducto.HeaderText = "Código";
            this.DGCCodigoProducto.Name = "DGCCodigoProducto";
            this.DGCCodigoProducto.ReadOnly = true;
            this.DGCCodigoProducto.ToolTipText = "Código Identificador de Artículo";
            // 
            // DGCNombreProducto
            // 
            this.DGCNombreProducto.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.DGCNombreProducto.DataPropertyName = "NombreProducto";
            this.DGCNombreProducto.HeaderText = "Artículo";
            this.DGCNombreProducto.Name = "DGCNombreProducto";
            this.DGCNombreProducto.ReadOnly = true;
            this.DGCNombreProducto.ToolTipText = "Nombre o Descripción del Artículo";
            this.DGCNombreProducto.Width = 80;
            // 
            // DGCCantidadIngreso
            // 
            this.DGCCantidadIngreso.DataPropertyName = "CantidadCompra";
            this.DGCCantidadIngreso.HeaderText = "Cant Solicitada";
            this.DGCCantidadIngreso.Name = "DGCCantidadIngreso";
            this.DGCCantidadIngreso.ReadOnly = true;
            this.DGCCantidadIngreso.ToolTipText = "Cantidad solicitada en el Ingreso";
            // 
            // DGCCantidadRecepcionada
            // 
            this.DGCCantidadRecepcionada.DataPropertyName = "CantidadRecepcionada";
            this.DGCCantidadRecepcionada.HeaderText = "Recepcionados";
            this.DGCCantidadRecepcionada.Name = "DGCCantidadRecepcionada";
            this.DGCCantidadRecepcionada.ReadOnly = true;
            this.DGCCantidadRecepcionada.ToolTipText = "Cantidad Recepcionada Actualmente";
            // 
            // DGCEsProductoEspecifico
            // 
            this.DGCEsProductoEspecifico.DataPropertyName = "EsProductoEspecifico";
            this.DGCEsProductoEspecifico.HeaderText = "Específico?";
            this.DGCEsProductoEspecifico.Name = "DGCEsProductoEspecifico";
            this.DGCEsProductoEspecifico.ToolTipText = "Es Considerado Artículo Especifico con Series";
            this.DGCEsProductoEspecifico.Visible = false;
            // 
            // DGCSeleccionar
            // 
            this.DGCSeleccionar.DataPropertyName = "Seleccionar";
            this.DGCSeleccionar.HeaderText = "Seleccionar";
            this.DGCSeleccionar.Name = "DGCSeleccionar";
            // 
            // FComprasProductosRecepcion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(993, 565);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.gBoxListadoProductosSeleccionados);
            this.Controls.Add(this.gBoxListadoRecepcion);
            this.Controls.Add(this.gBoxListadoProductosEspecificosSeleccionados);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FComprasProductosRecepcion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Selección de Productos a ser Recepcionados";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FComprasProductosRecepcion_FormClosing);
            this.gBoxListadoRecepcion.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtGVProductosListado)).EndInit();
            this.gBoxListadoProductosSeleccionados.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtGVProductosCantidadesSeleccionadas)).EndInit();
            this.gBoxListadoProductosEspecificosSeleccionados.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtGVProductosEspecificosSeleccionados)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gBoxListadoRecepcion;
        private System.Windows.Forms.GroupBox gBoxListadoProductosSeleccionados;
        private System.Windows.Forms.GroupBox gBoxListadoProductosEspecificosSeleccionados;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dtGVProductosListado;
        private System.Windows.Forms.DataGridView dtGVProductosCantidadesSeleccionadas;
        private System.Windows.Forms.DataGridView dtGVProductosEspecificosSeleccionados;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnConfirmarTotal;
        private System.Windows.Forms.Button btnConfirmarParcial;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Button btnConfirmarForzada;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGCCodigoProductoN;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGCNombreProductoN;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGCCantidadRecepcionEntrega;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGCNombreProductoAE;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGCCodigoProductoAE;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGCCodigoEspecifico;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGCCodigoProducto;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGCNombreProducto;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGCCantidadIngreso;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGCCantidadRecepcionada;
        private System.Windows.Forms.DataGridViewCheckBoxColumn DGCEsProductoEspecifico;
        private System.Windows.Forms.DataGridViewCheckBoxColumn DGCSeleccionar;
    }
}