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
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // Настройка ComboBox
            comboBox1.Items.AddRange(new object[] { "Сбербанк", "Альфа-Банк", "ВТБ", "Тинькофф", "Газпромбанк" });
            comboBox2.Items.AddRange(new object[] { "12 месяцев", "24 месяца", "36 месяцев", "48 месяцев", "60 месяцев" });
            comboBox3.Items.AddRange(new object[] { "5%", "10%", "15%", "20%", "25%" });
            comboBox4.Items.AddRange(new object[] { "Аннуитетный", "Дифференцированный", "Единовременный платёж", "Смешанный", "Процентами сначала" });

            // Загрузка данных в DataGridView
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
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Получение данных из TextBox и ComboBox
            string id = textBox1.Text;
            string fio = textBox2.Text;
            string innClient = textBox3.Text;
            string passportSeries = textBox4.Text;
            string passportNumber = textBox5.Text;
            string address = textBox6.Text;
            string bankName = comboBox1.SelectedItem?.ToString() ?? "Не выбрано";
            string innBank = textBox7.Text;
            string creditAmount = textBox8.Text;
            string creditTerm = comboBox2.SelectedItem?.ToString() ?? "Не выбрано";
            string annualRate = comboBox3.SelectedItem?.ToString() ?? "Не выбрано";
            string repaymentMethod = comboBox4.SelectedItem?.ToString() ?? "Не выбрано";
            string signingDate = textBox9.Text;

            // Проверка заполнения полей
            if (string.IsNullOrWhiteSpace(fio) || string.IsNullOrWhiteSpace(innClient) || string.IsNullOrWhiteSpace(creditAmount))
            {
                MessageBox.Show("Пожалуйста, заполните обязательные поля (ФИО, ИНН клиента, Сумма кредита).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Вставка данных в таблицу "Договор"
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Договор (ID, ФИО_Клиента, ИНН_Клиента, Серия_паспорта, Номер_паспорта, Адрес, Название_банка, ИНН_банка, Сумма_кредита, Срок_кредита, Процентная_ставка, Способ_погашения, Дата_подписания) " +
                               "VALUES (@ID, @FIO, @INNClient, @PassportSeries, @PassportNumber, @Address, @BankName, @INNBank, @CreditAmount, @CreditTerm, @AnnualRate, @RepaymentMethod, @SigningDate)";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    command.Parameters.AddWithValue("@FIO", fio);
                    command.Parameters.AddWithValue("@INNClient", innClient);
                    command.Parameters.AddWithValue("@PassportSeries", passportSeries);
                    command.Parameters.AddWithValue("@PassportNumber", passportNumber);
                    command.Parameters.AddWithValue("@Address", address);
                    command.Parameters.AddWithValue("@BankName", bankName);
                    command.Parameters.AddWithValue("@INNBank", innBank);
                    command.Parameters.AddWithValue("@CreditAmount", creditAmount);
                    command.Parameters.AddWithValue("@CreditTerm", creditTerm);
                    command.Parameters.AddWithValue("@AnnualRate", annualRate);
                    command.Parameters.AddWithValue("@RepaymentMethod", repaymentMethod);
                    command.Parameters.AddWithValue("@SigningDate", signingDate);

                    command.ExecuteNonQuery();
                }
            }
            // Обновление данных в DataGridView
            LoadDogovorData();

            // Очистка полей ввода
            ClearInputs();

            // Вывод сообщения
            MessageBox.Show("Вы успешно оформили кредит", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        
    }
}
