using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SAlvecoComercial10.Formularios.Utilidades
{
    public static class EventosValidacionTexto
    {
        public static void TextBoxEnteros_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsNumber(e.KeyChar) && (((Keys)e.KeyChar)) != Keys.Back)
            {
                e.Handled = true;
                System.Media.SystemSounds.Beep.Play();
            }
        }

        public static void TextBoxFlotantes_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsNumber(e.KeyChar) && (((Keys)e.KeyChar)) != Keys.Back
                && (((Keys)e.KeyChar)) != (Keys) ',')
            {
                e.Handled = true;
                System.Media.SystemSounds.Beep.Play();
            }
        }

        public static void TextBoxCaracteres_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsLetter(e.KeyChar) && (((Keys)e.KeyChar)) != Keys.Back
                && (((Keys)e.KeyChar)) != Keys.Space)
            {
                e.Handled = true;
                System.Media.SystemSounds.Beep.Play();
            }
        }
    }
}
