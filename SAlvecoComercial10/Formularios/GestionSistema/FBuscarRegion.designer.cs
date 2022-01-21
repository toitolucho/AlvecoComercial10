namespace SAlvecoComercial10.Formularios.GestionSistema
{
    partial class FBuscarRegion
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

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FBuscarRegion));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.tBPaisBusqueda = new System.Windows.Forms.TextBox();
            this.bBuscarP = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.bSPaises = new System.Windows.Forms.BindingSource(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.cBBuscarpor = new System.Windows.Forms.ComboBox();
            this.cBBusquedaExacta = new System.Windows.Forms.CheckBox();
            this.sSBuscarPaises = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.dGVResultadosBusqueda = new System.Windows.Forms.DataGridView();
            this.cBNombrePais = new System.Windows.Forms.ComboBox();
            this.cBNombreDepartamento = new System.Windows.Forms.ComboBox();
            this.cBNombreProvincia = new System.Windows.Forms.ComboBox();
            this.cBUbicaciongeografica = new System.Windows.Forms.CheckBox();
            this.gBUbicacionGeografica = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.bSPaises)).BeginInit();
            this.sSBuscarPaises.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGVResultadosBusqueda)).BeginInit();
            this.gBUbicacionGeografica.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Texto de Busqueda";
            // 
            // tBPaisBusqueda
            // 
            this.tBPaisBusqueda.Location = new System.Drawing.Point(118, 49);
            this.tBPaisBusqueda.Name = "tBPaisBusqueda";
            this.tBPaisBusqueda.Size = new System.Drawing.Size(175, 20);
            this.tBPaisBusqueda.TabIndex = 1;
            // 
            // bBuscarP
            // 
            this.bBuscarP.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bBuscarP.ImageIndex = 0;
            this.bBuscarP.ImageList = this.imageList1;
            this.bBuscarP.Location = new System.Drawing.Point(224, 83);
            this.bBuscarP.Name = "bBuscarP";
            this.bBuscarP.Size = new System.Drawing.Size(69, 32);
            this.bBuscarP.TabIndex = 2;
            this.bBuscarP.Text = "&Buscar";
            this.bBuscarP.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.bBuscarP.UseVisualStyleBackColor = true;
            this.bBuscarP.Click += new System.EventHandler(this.bBuscarP_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "search.ico");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Buscar por";
            // 
            // cBBuscarpor
            // 
            this.cBBuscarpor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBBuscarpor.FormattingEnabled = true;
            this.cBBuscarpor.Items.AddRange(new object[] {
            "Codigo",
            "Nombre"});
            this.cBBuscarpor.Location = new System.Drawing.Point(118, 11);
            this.cBBuscarpor.Name = "cBBuscarpor";
            this.cBBuscarpor.Size = new System.Drawing.Size(175, 21);
            this.cBBuscarpor.TabIndex = 5;
            // 
            // cBBusquedaExacta
            // 
            this.cBBusquedaExacta.AutoSize = true;
            this.cBBusquedaExacta.Location = new System.Drawing.Point(15, 97);
            this.cBBusquedaExacta.Name = "cBBusquedaExacta";
            this.cBBusquedaExacta.Size = new System.Drawing.Size(142, 17);
            this.cBBusquedaExacta.TabIndex = 6;
            this.cBBusquedaExacta.Text = "¿Buscar Texto Identico?";
            this.cBBusquedaExacta.UseVisualStyleBackColor = true;
            // 
            // sSBuscarPaises
            // 
            this.sSBuscarPaises.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.sSBuscarPaises.Location = new System.Drawing.Point(0, 362);
            this.sSBuscarPaises.MinimumSize = new System.Drawing.Size(0, 22);
            this.sSBuscarPaises.Name = "sSBuscarPaises";
            this.sSBuscarPaises.Size = new System.Drawing.Size(644, 22);
            this.sSBuscarPaises.TabIndex = 11;
            this.sSBuscarPaises.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // dGVResultadosBusqueda
            // 
            this.dGVResultadosBusqueda.AllowUserToAddRows = false;
            this.dGVResultadosBusqueda.AllowUserToDeleteRows = false;
            this.dGVResultadosBusqueda.AllowUserToResizeRows = false;
            this.dGVResultadosBusqueda.AutoGenerateColumns = false;
            this.dGVResultadosBusqueda.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dGVResultadosBusqueda.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dGVResultadosBusqueda.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGVResultadosBusqueda.DataSource = this.bSPaises;
            this.dGVResultadosBusqueda.Location = new System.Drawing.Point(15, 157);
            this.dGVResultadosBusqueda.Name = "dGVResultadosBusqueda";
            this.dGVResultadosBusqueda.ReadOnly = true;
            this.dGVResultadosBusqueda.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dGVResultadosBusqueda.Size = new System.Drawing.Size(602, 194);
            this.dGVResultadosBusqueda.TabIndex = 3;
            this.dGVResultadosBusqueda.DoubleClick += new System.EventHandler(this.dGVResultadosBusqueda_DoubleClick);
            // 
            // cBNombrePais
            // 
            this.cBNombrePais.AutoCompleteCustomSource.AddRange(new string[] {
            "todoshhh"});
            this.cBNombrePais.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBNombrePais.FormattingEnabled = true;
            this.cBNombrePais.Items.AddRange(new object[] {
            "Todos"});
            this.cBNombrePais.Location = new System.Drawing.Point(86, 17);
            this.cBNombrePais.Name = "cBNombrePais";
            this.cBNombrePais.Size = new System.Drawing.Size(160, 21);
            this.cBNombrePais.TabIndex = 12;
            this.cBNombrePais.SelectedValueChanged += new System.EventHandler(this.cBNombrePais_SelectedValueChanged);
            // 
            // cBNombreDepartamento
            // 
            this.cBNombreDepartamento.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBNombreDepartamento.FormattingEnabled = true;
            this.cBNombreDepartamento.Items.AddRange(new object[] {
            "Todos"});
            this.cBNombreDepartamento.Location = new System.Drawing.Point(86, 44);
            this.cBNombreDepartamento.Name = "cBNombreDepartamento";
            this.cBNombreDepartamento.Size = new System.Drawing.Size(160, 21);
            this.cBNombreDepartamento.TabIndex = 13;
            this.cBNombreDepartamento.SelectedValueChanged += new System.EventHandler(this.cBNombreDepartamento_SelectedValueChanged);
            // 
            // cBNombreProvincia
            // 
            this.cBNombreProvincia.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBNombreProvincia.FormattingEnabled = true;
            this.cBNombreProvincia.Location = new System.Drawing.Point(86, 69);
            this.cBNombreProvincia.Name = "cBNombreProvincia";
            this.cBNombreProvincia.Size = new System.Drawing.Size(160, 21);
            this.cBNombreProvincia.TabIndex = 14;
            // 
            // cBUbicaciongeografica
            // 
            this.cBUbicaciongeografica.AutoSize = true;
            this.cBUbicaciongeografica.Location = new System.Drawing.Point(380, 8);
            this.cBUbicaciongeografica.Name = "cBUbicaciongeografica";
            this.cBUbicaciongeografica.Size = new System.Drawing.Size(161, 17);
            this.cBUbicaciongeografica.TabIndex = 15;
            this.cBUbicaciongeografica.Text = "Utlizar Ubicacion Geografica";
            this.cBUbicaciongeografica.UseVisualStyleBackColor = true;
            this.cBUbicaciongeografica.CheckedChanged += new System.EventHandler(this.cBUbicaciongeografica_CheckedChanged);
            // 
            // gBUbicacionGeografica
            // 
            this.gBUbicacionGeografica.Controls.Add(this.label5);
            this.gBUbicacionGeografica.Controls.Add(this.label4);
            this.gBUbicacionGeografica.Controls.Add(this.label3);
            this.gBUbicacionGeografica.Controls.Add(this.cBNombrePais);
            this.gBUbicacionGeografica.Controls.Add(this.cBNombreDepartamento);
            this.gBUbicacionGeografica.Controls.Add(this.cBNombreProvincia);
            this.gBUbicacionGeografica.Enabled = false;
            this.gBUbicacionGeografica.Location = new System.Drawing.Point(371, 31);
            this.gBUbicacionGeografica.Name = "gBUbicacionGeografica";
            this.gBUbicacionGeografica.Size = new System.Drawing.Size(249, 105);
            this.gBUbicacionGeografica.TabIndex = 16;
            this.gBUbicacionGeografica.TabStop = false;
            this.gBUbicacionGeografica.Text = "Criterio de Busqueda";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 77);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Provincia";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Departamento";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Pais";
            // 
            // FBuscarRegion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 384);
            this.Controls.Add(this.gBUbicacionGeografica);
            this.Controls.Add(this.cBUbicaciongeografica);
            this.Controls.Add(this.sSBuscarPaises);
            this.Controls.Add(this.cBBusquedaExacta);
            this.Controls.Add(this.cBBuscarpor);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dGVResultadosBusqueda);
            this.Controls.Add(this.bBuscarP);
            this.Controls.Add(this.tBPaisBusqueda);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FBuscarRegion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Buscar Región";
            this.Load += new System.EventHandler(this.FBuscarRegion_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bSPaises)).EndInit();
            this.sSBuscarPaises.ResumeLayout(false);
            this.sSBuscarPaises.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGVResultadosBusqueda)).EndInit();
            this.gBUbicacionGeografica.ResumeLayout(false);
            this.gBUbicacionGeografica.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gBUbicacionGeografica;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cBNombrePais;
        private System.Windows.Forms.ComboBox cBNombreDepartamento;
        private System.Windows.Forms.ComboBox cBNombreProvincia;
        private System.Windows.Forms.CheckBox cBUbicaciongeografica;
        private System.Windows.Forms.CheckBox cBBusquedaExacta;
        private System.Windows.Forms.ComboBox cBBuscarpor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dGVResultadosBusqueda;
        private System.Windows.Forms.Button bBuscarP;
        private System.Windows.Forms.TextBox tBPaisBusqueda;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.BindingSource bSPaises;
        private System.Windows.Forms.StatusStrip sSBuscarPaises;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ImageList imageList1;
    }
}