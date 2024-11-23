using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace АИС_банка_кредитов
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Form4 adminForm = new Form4();
            adminForm.ShowDialog();
            this.Close(); // Закрыть текущую форму после открытия формы пользователя
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 userForm = new Form2();
            userForm.ShowDialog();
            this.Close(); // Закрыть текущую форму после открытия формы пользователя
        }
    }
}
