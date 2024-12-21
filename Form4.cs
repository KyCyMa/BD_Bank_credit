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
    public partial class Form4 : Form
    {
        private SQLiteConnection connection;
        public Form4()
        {
            InitializeComponent();
            
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
            string tip_plata = textBox2.Text;
            string vid_plata = textBox3.Text;
            string summa_kredita = textBox4.Text;
            string summa_plata = textBox5.Text;
            string ostatok = textBox6.Text;
            string status = textBox7.Text;


            // Вставляем данные в базу данных
            InsertPlatechDataToDatabase(data_plata, tip_plata, vid_plata, summa_kredita, summa_plata, ostatok, status);
            LoadPlatechData();
            MessageBox.Show("Данные успешно добавлены.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Проверяем, что строка в DataGridView выбрана
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите договор для изменения.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Получаем данные из текстовых полей
            string data_plata = textBox1.Text;
            string tip_plata = textBox2.Text;
            string vid_plata = textBox3.Text;
            string summa_kredita = textBox4.Text;
            string summa_plata = textBox5.Text;
            string ostatok = textBox6.Text;
            string status = textBox7.Text;


            // Получаем ID выбранного договора из DataGridView2
            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
            string kreditId = selectedRow.Cells["ID"].Value.ToString();// ID из скрытого столбца


            try
            {
                // Обновляем данные договора
                UpdatePlatechDataInDatabase(kreditId, data_plata, tip_plata, vid_plata, summa_kredita, summa_plata, ostatok, status);
                LoadPlatechData(); // Перезагружаем данные
                MessageBox.Show("Данные договора обновлены.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Получаем выбранную строку в DataGridView
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите кредит для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

            // Удаляем данные из базы данных
            try
            {
                DeletePlatechDataFromDatabase(selectedRow.Cells[0].Value.ToString());
                LoadPlatechData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InsertPlatechDataToDatabase(string data_plata, string tip_plata, string vid_plata, string summa_kredita, string summa_plata, string ostatok, string status)
        {
            string currentDate = GetCurrentDate();

            // Преобразуем суммы в числа для выполнения арифметической операции
            decimal sumaKreditaDecimal = 0;
            decimal sumaPlataDecimal = 0;

            // Преобразуем строковые значения в числа, если они могут быть конвертированы
            if (!decimal.TryParse(summa_kredita, out sumaKreditaDecimal))
            {
                MessageBox.Show("Ошибка при обработке суммы кредита.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(summa_plata, out sumaPlataDecimal))
            {
                MessageBox.Show("Ошибка при обработке суммы платежа.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Вычисляем остаток долга
            decimal remainingDebt = sumaKreditaDecimal - sumaPlataDecimal;

            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = "INSERT INTO Платеж (Дата_платежа, Тип_платежа, Вид_платежа, Сумма_кредита, Сумма_платежа, Остаток_долга) " +
                                      "VALUES (@data_plata, @tip_plata, @vid_plata, @summa_kredita, @summa_plata, @remaining_debt)";

                command.Parameters.AddWithValue("@data_plata", currentDate);
                command.Parameters.AddWithValue("@tip_plata", tip_plata);
                command.Parameters.AddWithValue("@vid_plata", vid_plata);
                command.Parameters.AddWithValue("@summa_kredita", sumaKreditaDecimal.ToString("F2"));
                command.Parameters.AddWithValue("@summa_plata", sumaPlataDecimal.ToString("F2"));
                command.Parameters.AddWithValue("@remaining_debt", remainingDebt.ToString("F2"));

                command.ExecuteNonQuery();
            }
        }

        private void UpdatePlatechDataInDatabase(string id, string data_plata, string tip_plata, string vid_plata, string summa_plata, string summa_kredita, string ostatok, string status)
        {
            // Преобразуем суммы в числа для выполнения арифметической операции
            decimal sumaKreditaDecimal = 0;
            decimal sumaPlataDecimal = 0;

            // Преобразуем строковые значения в числа, если они могут быть конвертированы
            if (!decimal.TryParse(summa_kredita, out sumaKreditaDecimal))
            {
                MessageBox.Show("Ошибка при обработке суммы кредита.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(summa_plata, out sumaPlataDecimal))
            {
                MessageBox.Show("Ошибка при обработке суммы платежа.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Вычисляем остаток долга
            decimal remainingDebt = sumaKreditaDecimal - sumaPlataDecimal;

            string currentDate = GetCurrentDate();
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = "UPDATE Платеж SET Дата_платежа = @data_plata, Тип_платежа = @tip_plata, Вид_платежа = @vid_plata, " +
                              "Сумма_кредита = @summa_kredita, Остаток_долга = @remaining_debt, Сумма_платежа = @summa_plata WHERE ID = @id";

                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@data_plata", currentDate);
                command.Parameters.AddWithValue("@tip_plata", tip_plata);
                command.Parameters.AddWithValue("@vid_kredita", vid_plata);
                command.Parameters.AddWithValue("@summa_kredita", sumaKreditaDecimal.ToString("F2"));
                command.Parameters.AddWithValue("@summa_plata", sumaPlataDecimal.ToString("F2"));
                command.Parameters.AddWithValue("@remaining_debt", remainingDebt.ToString("F2"));
                command.ExecuteNonQuery();
            }
        }

        private string GetCurrentDate()
        {
            return DateTime.Now.ToString("dd-MM-yyyy"); // Текущая дата
        }

        private void DeletePlatechDataFromDatabase(string id)
        {
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = "DELETE FROM Платеж WHERE ID = @id";
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }
    }
}
