namespace SAlvecoComercial10.Formularios.GestionComercial
{
    partial class FVentasProductosDistribucionDatos
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
            this.btnAceptar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cboxPersonas = new System.Windows.Forms.ComboBox();
            this.cBoxMovilidades = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsLblNroVenta = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnAgregarPersonas = new System.Windows.Forms.Button();
            this.btnAgregarMovilidad = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAceptar
            // 
            this.btnAceptar.Image = global::SAlvecoComercial10.Properties.Resources.accept;
            this.btnAceptar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAceptar.Location = new System.Drawing.Point(291, 68);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(70, 23);
            this.btnAceptar.TabIndex = 0;
            this.btnAceptar.Text = "&Aceptar";
            this.btnAceptar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Responsable";
            // 
            // cboxPersonas
            // 
            this.cboxPersonas.FormattingEnabled = true;
            this.cboxPersonas.Location = new System.Drawing.Point(81, 10);
            this.cboxPersonas.Name = "cboxPersonas";
            this.cboxPersonas.Size = new System.Drawing.Size(248, 21);
            this.cboxPersonas.TabIndex = 3;
            // 
            // cBoxMovilidades
            // 
            this.cBoxMovilidades.FormattingEnabled = true;
            this.cBoxMovilidades.Location = new System.Drawing.Point(83, 38);
            this.cBoxMovilidades.Name = "cBoxMovilidades";
            this.cBoxMovilidades.Size = new System.Drawing.Size(246, 21);
            this.cBoxMovilidades.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Movilidad";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Enabled = false;
            this.checkBox1.Location = new System.Drawing.Point(83, 72);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(108, 17);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "Venta Distribuible";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.tsLblNroVenta});
            this.statusStrip1.Location = new System.Drawing.Point(0, 98);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(371, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(77, 17);
            this.toolStripStatusLabel1.Text = "Nro de Venta :";
            // 
            // tsLblNroVenta
            // 
            this.tsLblNroVenta.Name = "tsLblNroVenta";
            this.tsLblNroVenta.Size = new System.Drawing.Size(13, 17);
            this.tsLblNroVenta.Text = "  ";
            // 
            // btnAgregarPersonas
            // 
            this.btnAgregarPersonas.Image = global::SAlvecoComercial10.Properties.Resources.add1;
            this.btnAgregarPersonas.Location = new System.Drawing.Point(335, 8);
            this.btnAgregarPersonas.Name = "btnAgregarPersonas";
            this.btnAgregarPersonas.Size = new System.Drawing.Size(24, 23);
            this.btnAgregarPersonas.TabIndex = 8;
            this.btnAgregarPersonas.UseVisualStyleBackColor = true;
            this.btnAgregarPersonas.Click += new System.EventHandler(this.btnAgregarPersonas_Click);
            // 
            // btnAgregarMovilidad
            // 
            this.btnAgregarMovilidad.Image = global::SAlvecoComercial10.Properties.Resources.add1;
            this.btnAgregarMovilidad.Location = new System.Drawing.Point(335, 37);
            this.btnAgregarMovilidad.Name = "btnAgregarMovilidad";
            this.btnAgregarMovilidad.Size = new System.Drawing.Size(24, 23);
            this.btnAgregarMovilidad.TabIndex = 9;
            this.btnAgregarMovilidad.UseVisualStyleBackColor = true;
            this.btnAgregarMovilidad.Click += new System.EventHandler(this.btnAgregarMovilidad_Click);
            // 
            // FVentasProductosDistribucionDatos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(371, 120);
            this.Controls.Add(this.btnAgregarMovilidad);
            this.Controls.Add(this.btnAgregarPersonas);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.cBoxMovilidades);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboxPersonas);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnAceptar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FVentasProductosDistribucionDatos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Distribucion de Productos";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboxPersonas;
        private System.Windows.Forms.ComboBox cBoxMovilidades;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel tsLblNroVenta;
        private System.Windows.Forms.Button btnAgregarPersonas;
        private System.Windows.Forms.Button btnAgregarMovilidad;
    }
}