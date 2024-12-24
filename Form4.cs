using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace АИС_банка_кредитов
{
    public partial class Form4 : Form
    {
        private SQLiteConnection connection;
        public Form4()
        {
            InitializeComponent();
            LoadPlatechData();
            LoadClientsToComboBox();
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
        }

        private void LoadPlatechData()
        {
            string dbPath = "C:\\Users\\KyCyMaMa\\Desktop\\Bank.db";
            string connectionString = $"Data Source={dbPath}";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Платеж";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        DataTable clientsTable = new DataTable();
                        adapter.Fill(clientsTable);
                        dataGridView1.DataSource = clientsTable;
                        dataGridView1.Columns["ID"].Visible = false;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Получаем значения из полей ввода
            string data_plata = textBox1.Text;
            string vid_plata = textBox3.Text;
            string summa_kredita = textBox4.Text;
            string summa_plata = textBox5.Text;
            string fio = comboBox1.Text;
            string cell_credit = textBox8.Text;
            string remainingDebtStr = textBox6.Text;

            // Вставляем данные в базу данных
            InsertPlatechDataToDatabase(fio, data_plata, vid_plata, summa_kredita, summa_plata, cell_credit, remainingDebtStr);
            LoadPlatechData();
            MessageBox.Show("Данные успешно добавлены.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void InsertPlatechDataToDatabase(string fio, string data_plata, string vid_plata, string summa_kredita, string summa_plata, string cell_credit, string remainingDebtStr)
        {
            string dbPath = "C:\\Users\\KyCyMaMa\\Desktop\\Bank.db";
            string connectionString = $"Data Source={dbPath}";
            string currentDate = GetCurrentDate();

            // Преобразуем суммы в числа
            decimal sumaKreditaDecimal = 0;
            decimal sumaPlataDecimal = 0;
            decimal remainingDebt = 0;

            if (!string.IsNullOrWhiteSpace(summa_kredita))
            {
                if (!decimal.TryParse(summa_kredita, out sumaKreditaDecimal))
                {
                    MessageBox.Show("Ошибка при обработке суммы кредита.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (!decimal.TryParse(summa_plata, out sumaPlataDecimal))
            {
                MessageBox.Show("Ошибка при обработке суммы платежа.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!string.IsNullOrWhiteSpace(remainingDebtStr))
            {
                if (!decimal.TryParse(remainingDebtStr, out remainingDebt))
                {
                    MessageBox.Show("Ошибка при обработке остатка долга.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // Вычисляем остаток долга
            if (sumaKreditaDecimal > 0)
            {
                remainingDebt = sumaKreditaDecimal - sumaPlataDecimal;
            }
            else
            {
                remainingDebt -= sumaPlataDecimal;
            }

            // Определяем статус кредита
            string status = remainingDebt == 0 ? "Погашен" : "Активен";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "INSERT INTO Платеж (ФИО, Цель_кредита, Дата_платежа, Вид_платежа, Сумма_кредита, Сумма_платежа, Остаток_долга, Статус_кредита) " +
                                          "VALUES (@fio, @cell_credit, @data_plata, @vid_plata, @summa_kredita, @summa_plata, @remaining_debt, @status)";

                    command.Parameters.AddWithValue("@fio", fio);
                    command.Parameters.AddWithValue("@cell_credit", cell_credit);
                    command.Parameters.AddWithValue("@data_plata", currentDate);
                    command.Parameters.AddWithValue("@vid_plata", vid_plata);
                    command.Parameters.AddWithValue("@summa_kredita", sumaKreditaDecimal > 0 ? sumaKreditaDecimal.ToString("F2") : "");
                    command.Parameters.AddWithValue("@summa_plata", sumaPlataDecimal.ToString("F2"));
                    command.Parameters.AddWithValue("@remaining_debt", remainingDebt.ToString("F2"));
                    command.Parameters.AddWithValue("@status", status);

                    command.ExecuteNonQuery();
                }
            }
        }


        private string GetCurrentDate()
        {
            return DateTime.Now.ToString("dd-MM-yyyy"); // Текущая дата
        }

        // Обработчик изменения выбранного клиента в ComboBox


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) // Поставщик
        {
            if (comboBox1.SelectedItem != null)
            {
                string selectedClient = comboBox1.SelectedItem.ToString();
                string connectionString = @"Data Source=C:\Users\KyCyMaMa\Desktop\Bank.db;Version=3;";

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    // SQL-запрос для объединения данных и получения цели кредита
                    using (var command = new SQLiteCommand(
                        @"SELECT Кредит.Цель_кредита, Кредит.Сумма_кредита
                  FROM Клиент
                  INNER JOIN Кредит ON Клиент.ID = Кредит.ID
                  WHERE Клиент.Фамилия || ' ' || Клиент.Имя || ' ' || Клиент.Отчество = @FullName",
                        connection))
                    {
                        command.Parameters.AddWithValue("@FullName", selectedClient);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                textBox8.Text = reader["Цель_кредита"].ToString();
                                textBox4.Text = reader["Сумма_кредита"].ToString();
                            }
                        }
                    }
                }
            }
        }

        private void LoadClientsToComboBox()
        {
            string connectionString = @"Data Source=C:\Users\KyCyMaMa\Desktop\Bank.db;Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand("SELECT Фамилия || ' ' || Имя || ' ' || Отчество AS FullName FROM Клиент", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBox1.Items.Add(reader["FullName"].ToString());
                        }
                    }
                }
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                textBox1.Text = selectedRow.Cells["Дата_платежа"].Value?.ToString() ?? "";
                textBox3.Text = selectedRow.Cells["Вид_платежа"].Value?.ToString() ?? "";
                textBox5.Text = selectedRow.Cells["Сумма_платежа"].Value?.ToString() ?? "";
                textBox6.Text = selectedRow.Cells["Остаток_долга"].Value?.ToString() ?? "";
                textBox8.Text = selectedRow.Cells["Цель_кредита"].Value?.ToString() ?? "";
                comboBox1.SelectedItem = selectedRow.Cells["ФИО"].Value?.ToString() ?? "";
            }
        }
    }
}
