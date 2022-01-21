namespace SAlvecoComercial10.Formularios.GestionSistema
{
    partial class FAutenticacion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FAutenticacion));
            this.label5 = new System.Windows.Forms.Label();
            this.bCancelar = new System.Windows.Forms.Button();
            this.bAceptar = new System.Windows.Forms.Button();
            this.tBContrasena = new System.Windows.Forms.TextBox();
            this.tBNombreUsuario = new System.Windows.Forms.TextBox();
            this.tBBaseDatos = new System.Windows.Forms.TextBox();
            this.tBServidor = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnConfirgurar = new System.Windows.Forms.Button();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(410, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Ingrese la información solicitada para autenticarse ante el sistema e ingresar al" +
                " mismo.\r\n";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // bCancelar
            // 
            this.bCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancelar.Image = global::SAlvecoComercial10.Properties.Resources.arrow_rotate_clockwise;
            this.bCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bCancelar.Location = new System.Drawing.Point(342, 6);
            this.bCancelar.Name = "bCancelar";
            this.bCancelar.Size = new System.Drawing.Size(75, 23);
            this.bCancelar.TabIndex = 20;
            this.bCancelar.Text = "&Cancelar";
            this.bCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.bCancelar.UseVisualStyleBackColor = true;
            this.bCancelar.Click += new System.EventHandler(this.bCancelar_Click);
            // 
            // bAceptar
            // 
            this.bAceptar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bAceptar.Image = global::SAlvecoComercial10.Properties.Resources.accept;
            this.bAceptar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bAceptar.Location = new System.Drawing.Point(189, 6);
            this.bAceptar.Name = "bAceptar";
            this.bAceptar.Size = new System.Drawing.Size(70, 23);
            this.bAceptar.TabIndex = 19;
            this.bAceptar.Text = "&Aceptar";
            this.bAceptar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.bAceptar.UseVisualStyleBackColor = true;
            this.bAceptar.Click += new System.EventHandler(this.bAceptar_Click);
            // 
            // tBContrasena
            // 
            this.tBContrasena.Location = new System.Drawing.Point(203, 61);
            this.tBContrasena.Name = "tBContrasena";
            this.tBContrasena.Size = new System.Drawing.Size(176, 20);
            this.tBContrasena.TabIndex = 18;
            this.tBContrasena.UseSystemPasswordChar = true;
            this.tBContrasena.TextChanged += new System.EventHandler(this.tBContrasena_TextChanged);
            // 
            // tBNombreUsuario
            // 
            this.tBNombreUsuario.Location = new System.Drawing.Point(203, 37);
            this.tBNombreUsuario.Name = "tBNombreUsuario";
            this.tBNombreUsuario.Size = new System.Drawing.Size(176, 20);
            this.tBNombreUsuario.TabIndex = 17;
            this.tBNombreUsuario.Text = "roxana";
            // 
            // tBBaseDatos
            // 
            this.tBBaseDatos.Location = new System.Drawing.Point(203, 34);
            this.tBBaseDatos.Name = "tBBaseDatos";
            this.tBBaseDatos.Size = new System.Drawing.Size(202, 20);
            this.tBBaseDatos.TabIndex = 16;
            // 
            // tBServidor
            // 
            this.tBServidor.Location = new System.Drawing.Point(203, 8);
            this.tBServidor.Name = "tBServidor";
            this.tBServidor.Size = new System.Drawing.Size(202, 20);
            this.tBServidor.TabIndex = 15;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(122, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Base de datos";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(151, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Servidor";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(136, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Contraseña";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(101, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Nombre de usuario";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.pictureBox1);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.tBNombreUsuario);
            this.splitContainer1.Panel1.Controls.Add(this.tBContrasena);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Panel2.Controls.Add(this.tBServidor);
            this.splitContainer1.Panel2.Controls.Add(this.tBBaseDatos);
            this.splitContainer1.Size = new System.Drawing.Size(423, 169);
            this.splitContainer1.SplitterDistance = 95;
            this.splitContainer1.TabIndex = 23;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::SAlvecoComercial10.Properties.Resources.businessman1;
            this.pictureBox1.Location = new System.Drawing.Point(12, 29);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(86, 66);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 22;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnConfirgurar);
            this.panel1.Controls.Add(this.bAceptar);
            this.panel1.Controls.Add(this.bCancelar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 169);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(423, 33);
            this.panel1.TabIndex = 24;
            // 
            // btnConfirgurar
            // 
            this.btnConfirgurar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirgurar.Location = new System.Drawing.Point(263, 6);
            this.btnConfirgurar.Name = "btnConfirgurar";
            this.btnConfirgurar.Size = new System.Drawing.Size(75, 23);
            this.btnConfirgurar.TabIndex = 21;
            this.btnConfirgurar.Text = "Con&figurar";
            this.btnConfirgurar.UseVisualStyleBackColor = true;
            this.btnConfirgurar.Click += new System.EventHandler(this.btnConfirgurar_Click);
            // 
            // FAutenticacion
            // 
            this.AcceptButton = this.bAceptar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.CancelButton = this.bCancelar;
            this.ClientSize = new System.Drawing.Size(423, 202);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FAutenticacion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Autenticación";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FAutenticacion_FormClosing);
            this.Load += new System.EventHandler(this.FAutenticacion_Load);
            this.Shown += new System.EventHandler(this.FAutenticacion_Shown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button bCancelar;
        private System.Windows.Forms.Button bAceptar;
        private System.Windows.Forms.TextBox tBContrasena;
        private System.Windows.Forms.TextBox tBNombreUsuario;
        private System.Windows.Forms.TextBox tBBaseDatos;
        private System.Windows.Forms.TextBox tBServidor;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnConfirgurar;
    }
}