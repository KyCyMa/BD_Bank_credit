using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace АИС_банка_кредитов
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
            textBox2.PasswordChar = '*';
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "admin" && textBox2.Text == "admin")
            {
                Form3 adminForm = new Form3();
                adminForm.ShowDialog();
                this.Close(); // Закрыть текущую форму после открытия формы администратора
            }
            else
            {
                MessageBox.Show("Неправильный логин или пароль. Пожалуйста, попробуйте снова.");
            }

        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // Заменяем каждый символ вводимого пароля на '*'
            textBox2.PasswordChar = '*';
        }
    }
}
