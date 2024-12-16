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
                // Если логин и пароль администратора верны, открыть форму администратора
                Form3 adminForm = new Form3();
                adminForm.ShowDialog();
                this.Close(); // Закрыть текущую форму
            }
            else if (textBox1.Text == "user") // Проверка для пользователя
            {
                // Если логин и пароль пользователя верны, открыть форму пользователя
                Form2 userForm = new Form2();
                userForm.ShowDialog();
                this.Close(); // Закрыть текущую форму
            }
            else
            {
                // Вывести сообщение об ошибке, если логин или пароль неверны
                MessageBox.Show("Неправильный логин или пароль. Пожалуйста, попробуйте снова.");
            }

        }
    }
}
