using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SAlvecoComercial10.Formularios.Utilidades
{
    public partial class FIngresarCantidad : Form
    {
        public int CantidadEnteraIngresada { get; set; }
        public decimal MontoDecimalIngresado { get; set; }
        public decimal MontoTopeMaximo { get; set; }
        private bool esDecimal = false;
        private bool permitirCeros = false;

        public FIngresarCantidad(bool esDecimal)
        {
            InitializeComponent();
            this.esDecimal = esDecimal;
        }

        public FIngresarCantidad(bool esDecimal, string MensajeTitulo, bool permitirCeros)
        {
            InitializeComponent();
            this.esDecimal = esDecimal;
            this.Text = MensajeTitulo;
            this.permitirCeros = permitirCeros;
        }

        private void FIngresarCantidad_Shown(object sender, EventArgs e)
        {
            txtMontoCantidad.Text = permitirCeros ? "0" : MontoTopeMaximo.ToString();
            txtMontoCantidad.Focus();
            txtMontoCantidad.SelectAll();            
        }

        public void LimpiarControl()
        {
            this.txtMontoCantidad.Clear();
            this.CantidadEnteraIngresada = -1;
            this.MontoDecimalIngresado = -1;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtMontoCantidad.Text.Trim()))
            {
                errorProvider1.SetError(txtMontoCantidad, "Aún no ha ingresado el Monto o la Cantidad");
                txtMontoCantidad.Focus();
                txtMontoCantidad.SelectAll();
                return;
            }

            if (esDecimal)
            {
                decimal monto = -1;
                if (!decimal.TryParse(txtMontoCantidad.Text, out monto))
                {
                    errorProvider1.SetError(txtMontoCantidad, "El Monto Ingresado es Incorrecto o esta mal escrito");
                    txtMontoCantidad.Focus();
                    txtMontoCantidad.SelectAll();
                    return;
                }
                if (!permitirCeros && monto <= 0)
                {
                    errorProvider1.SetError(txtMontoCantidad, "El Monto ingresado no puede ser Meno a cero");
                    txtMontoCantidad.Focus();
                    txtMontoCantidad.SelectAll();
                    return;
                }

                if (monto < 0)
                {
                    errorProvider1.SetError(txtMontoCantidad, "El Monto ingresado no puede ser Meno a cero");
                    txtMontoCantidad.Focus();
                    txtMontoCantidad.SelectAll();
                    return;
                }

                if (MontoTopeMaximo > 0 && monto > MontoTopeMaximo)
                {
                    errorProvider1.SetError(txtMontoCantidad, "La Monto ingresado no puede superar el Maximo de " + MontoTopeMaximo.ToString());
                    txtMontoCantidad.Focus();
                    txtMontoCantidad.SelectAll();
                    return;
                }
                MontoDecimalIngresado = monto;
            }
            else
            {
                int cantidad = -1;
                if (!int.TryParse(txtMontoCantidad.Text, out cantidad))
                {
                    errorProvider1.SetError(txtMontoCantidad, "La Cantidad Ingresada es Incorrecto o esta mal escrito");
                    txtMontoCantidad.Focus();
                    txtMontoCantidad.SelectAll();
                    return;
                }
                if (!permitirCeros && cantidad <= 0)
                {
                    errorProvider1.SetError(txtMontoCantidad, "La Cantidad ingresada no puede ser Meno a cero");
                    txtMontoCantidad.Focus();
                    txtMontoCantidad.SelectAll();
                    return;
                }
                if (MontoTopeMaximo > 0 && cantidad > MontoTopeMaximo)
                {
                    errorProvider1.SetError(txtMontoCantidad, "La Cantidad ingresada no puede superar el Maximo de " + MontoTopeMaximo.ToString());
                    txtMontoCantidad.Focus();
                    txtMontoCantidad.SelectAll();
                    return;
                }
                CantidadEnteraIngresada = cantidad;
            }
            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void txtMontoCantidad_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnAceptar_Click(btnAceptar, e as EventArgs);
        }
    }
}
