using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace АИС_банка_кредитов
{
    public partial class Form2 : Form
    {
        private SQLiteConnection connection;
        private string dbPath = "C:\\Users\\KyCyMaMa\\Desktop\\Bank.db";
        private string connectionString;

        public Form2()
        {
            InitializeComponent();
            connectionString = $"Data Source={dbPath}";
            textBox1.Validating += textBox1_Validating;
            textBox2.Validating += textBox2_Validating;
            textBox3.Validating += textBox3_Validating;
            textBox5.Validating += textBox5_Validating;
            textBox6.Validating += textBox6_Validating;
            textBox9.Validating += textBox9_Validating;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            comboBox1.Items.AddRange(new object[] { "Сбербанк", "Альфа-Банк", "ВТБ", "Тинькофф", "Газпромбанк" });
            comboBox2.Items.AddRange(new object[] { "12 месяцев", "24 месяца", "36 месяцев", "48 месяцев", "60 месяцев" });
            comboBox3.Items.AddRange(new object[] { "5%", "10%", "15%", "20%", "25%" });
            comboBox4.Items.AddRange(new object[] { "Потребительские нужды", "Жилищные цели", "Автокредит", "Образование", "Личные цели", "Бизнес" });

            LoadDogovorData();
        }

        private void LoadDogovorData()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Договор";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        DataTable clientsTable = new DataTable();
                        adapter.Fill(clientsTable);
                        dataGridView1.DataSource = clientsTable;
                        dataGridView1.Columns["ID"].Visible = false;
                        dataGridView1.Columns["ID_Клиента"].Visible = false;
                        dataGridView1.Columns["ID_Кредита"].Visible = false;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string lastName = textBox1.Text;
            string firstName = textBox2.Text;
            string middleName = textBox3.Text;
            string birthDate = textBox4.Text;
            string passportSeries = textBox5.Text;
            string passportNumber = textBox6.Text;
            string innClient = textBox7.Text;
            string address = textBox8.Text;
            string phoneNumber = textBox9.Text;
            string bankName = comboBox1.SelectedItem?.ToString() ?? "Не выбрано";
            string creditAmount = textBox10.Text;
            string creditTerm = comboBox2.SelectedItem?.ToString() ?? "Не выбрано";
            string annualRate = comboBox3.SelectedItem?.ToString() ?? "Не выбрано";
            string creditPurpose = comboBox4.SelectedItem?.ToString() ?? "Не выбрано";

            if (string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(innClient) || string.IsNullOrWhiteSpace(creditAmount))
            {
                MessageBox.Show("Пожалуйста, заполните обязательные поля (Фамилия, Имя, ИНН, Сумма кредита).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Договор (Фамилия, Имя, Отчество, Дата_рождения, Серия_паспорта, Номер_паспорта, ИНН, Адрес_проживания, Номер_телефона, Банк, Сумма_кредита, Срок_кредита, Процентная_ставка, Цель_кредита) " +
                               "VALUES (@LastName, @FirstName, @MiddleName, @BirthDate, @PassportSeries, @PassportNumber, @INN, @Address, @PhoneNumber, @BankName, @CreditAmount, @CreditTerm, @AnnualRate, @CreditPurpose)";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@MiddleName", middleName);
                    command.Parameters.AddWithValue("@BirthDate", birthDate);
                    command.Parameters.AddWithValue("@PassportSeries", passportSeries);
                    command.Parameters.AddWithValue("@PassportNumber", passportNumber);
                    command.Parameters.AddWithValue("@INN", innClient);
                    command.Parameters.AddWithValue("@Address", address);
                    command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                    command.Parameters.AddWithValue("@BankName", bankName);
                    command.Parameters.AddWithValue("@CreditAmount", creditAmount);
                    command.Parameters.AddWithValue("@CreditTerm", creditTerm);
                    command.Parameters.AddWithValue("@AnnualRate", annualRate);
                    command.Parameters.AddWithValue("@CreditPurpose", creditPurpose);

                    command.ExecuteNonQuery();
                }
            }

            LoadDogovorData();
            ClearInputs();
            MessageBox.Show("Вы успешно оформили кредит", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM Договор WHERE ID = @ID";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", selectedID);
                        command.ExecuteNonQuery();
                    }
                }

                LoadDogovorData();
                MessageBox.Show("Запись удалена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Выберите строку для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Договор SET Фамилия = @LastName, Имя = @FirstName, Отчество = @MiddleName, Дата_рождения = @BirthDate, Серия_паспорта = @PassportSeries, Номер_паспорта = @PassportNumber, ИНН = @INN, Адрес_проживания = @Address, Номер_телефона = @PhoneNumber, Банк = @BankName, Сумма_кредита = @CreditAmount, Срок_кредита = @CreditTerm, Процентная_ставка = @AnnualRate, Цель_кредита = @CreditPurpose WHERE ID = @ID";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", selectedID);
                        command.Parameters.AddWithValue("@LastName", textBox1.Text);
                        command.Parameters.AddWithValue("@FirstName", textBox2.Text);
                        command.Parameters.AddWithValue("@MiddleName", textBox3.Text);
                        command.Parameters.AddWithValue("@BirthDate", textBox4.Text);
                        command.Parameters.AddWithValue("@PassportSeries", textBox5.Text);
                        command.Parameters.AddWithValue("@PassportNumber", textBox6.Text);
                        command.Parameters.AddWithValue("@INN", textBox7.Text);
                        command.Parameters.AddWithValue("@Address", textBox8.Text);
                        command.Parameters.AddWithValue("@PhoneNumber", textBox9.Text);
                        command.Parameters.AddWithValue("@BankName", comboBox1.SelectedItem?.ToString() ?? "Не выбрано");
                        command.Parameters.AddWithValue("@CreditAmount", textBox10.Text);
                        command.Parameters.AddWithValue("@CreditTerm", comboBox2.SelectedItem?.ToString() ?? "Не выбрано");
                        command.Parameters.AddWithValue("@AnnualRate", comboBox3.SelectedItem?.ToString() ?? "Не выбрано");
                        command.Parameters.AddWithValue("@CreditPurpose", comboBox4.SelectedItem?.ToString() ?? "Не выбрано");

                        command.ExecuteNonQuery();
                    }
                }

                LoadDogovorData();
                ClearInputs();
                MessageBox.Show("Данные успешно обновлены.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Выберите строку для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                textBox1.Text = row.Cells["Фамилия"].Value.ToString();
                textBox2.Text = row.Cells["Имя"].Value.ToString();
                textBox3.Text = row.Cells["Отчество"].Value.ToString();
                textBox4.Text = row.Cells["Дата_рождения"].Value.ToString();
                textBox5.Text = row.Cells["Серия_паспорта"].Value.ToString();
                textBox6.Text = row.Cells["Номер_паспорта"].Value.ToString();
                textBox7.Text = row.Cells["ИНН"].Value.ToString();
                textBox8.Text = row.Cells["Адрес_проживания"].Value.ToString();
                textBox9.Text = row.Cells["Номер_телефона"].Value.ToString();
                textBox10.Text = row.Cells["Сумма_кредита"].Value.ToString();
                comboBox1.SelectedItem = row.Cells["Банк"].Value.ToString();
                comboBox2.SelectedItem = row.Cells["Срок_кредита"].Value.ToString();
                comboBox3.SelectedItem = row.Cells["Процентная_ставка"].Value.ToString();
                comboBox4.SelectedItem = row.Cells["Цель_кредита"].Value.ToString();
            }
        }

        private void ClearInputs()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox10.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (!IsValidName(textBox1.Text))
            {
                MessageBox.Show("Неправильно введена фамилия.");
                e.Cancel = true;
            }
        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            if (!IsValidName(textBox2.Text))
            {
                MessageBox.Show("Неправильно введено имя.");
                e.Cancel = true;
            }
        }

        private void textBox3_Validating(object sender, CancelEventArgs e)
        {
            if (!IsValidName(textBox3.Text))
            {
                MessageBox.Show("Неправильно введено отчество.");
                e.Cancel = true;
            }
        }

        private bool IsValidName(string name)
        {
            return !string.IsNullOrEmpty(name) && name.All(char.IsLetter);
        }

        private void textBox5_Validating(object sender, CancelEventArgs e)
        {
            if (textBox5.Text.Length != 4 || !IsNumeric(textBox5.Text))
            {
                MessageBox.Show("Серия паспорта должна содержать 4 цифры.");
                e.Cancel = true;
            }
        }

        private void textBox6_Validating(object sender, CancelEventArgs e)
        {
            if (textBox6.Text.Length != 6 || !IsNumeric(textBox6.Text))
            {
                MessageBox.Show("Номер паспорта должен содержать 6 цифр.");
                e.Cancel = true;
            }
        }

        private bool IsNumeric(string value)
        {
            return int.TryParse(value, out _);
        }

        private void textBox9_Validating(object sender, CancelEventArgs e)
        {
            string phonePattern = @"^(\+7|8)\d{10}$";
            if (!Regex.IsMatch(textBox9.Text, phonePattern))
            {
                MessageBox.Show("Неверный формат номера телефона.");
                e.Cancel = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
    }
}
