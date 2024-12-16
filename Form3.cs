using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
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
    public partial class Form3 : Form
    {
        private SQLiteConnection connection;
        private string dbPath = "C:\\Users\\KyCyMaMa\\Desktop\\Bank.db";
        private string connectionString;

        public Form3()
        {
            InitializeFormComponents();
            InitializeComponent();
            ConnectToDatabase();
            LoadClientData();
            LoadDogovorData();
            LoadKreditData();
            LoadPlatechData();
            LoadSearchCriteria();
            dataGridView1.ReadOnly = true;
            dataGridView2.ReadOnly = true;
            dataGridView3.ReadOnly = true;
            dataGridView5.ReadOnly = true;
            comboBox1.Items.AddRange(new object[] { "Сбербанк", "Альфа-Банк", "ВТБ", "Тинькофф", "Газпромбанк" });
            comboBox2.Items.AddRange(new object[] { "Потребительские нужды", "Жилищные цели", "Автокредит", "Образование", "Личные цели", "Бизнес" });
            comboBox9.Items.AddRange(new object[] { "12 месяцев", "24 месяца", "36 месяцев", "48 месяцев", "60 месяцев" });
            comboBox3.Items.AddRange(new object[] { "12 месяцев", "24 месяца", "36 месяцев", "48 месяцев", "60 месяцев" });
            comboBox4.Items.AddRange(new object[] { "Активен", "Погашен" });
            comboBox5.Items.AddRange(new object[] { "Рубли", "Доллары" });
            comboBox10.Items.AddRange(new object[] { "5%", "10%", "15%", "20%", "25%" });
            comboBox13.Items.AddRange(new object[] { "5%", "10%", "15%", "20%", "25%" });
            comboBox11.Items.AddRange(new object[] { "Потребительские нужды", "Жилищные цели", "Автокредит", "Образование", "Личные цели", "Бизнес" });


        }

        private void Form3_Load(object sender, EventArgs e)
        {
            LoadClientData();
            LoadDogovorData();
            LoadKreditData();
            LoadPlatechData();
            LoadSearchCriteria();
        }

        private string GetCurrentDate()
        {
            return DateTime.Now.ToString("dd-MM-yyyy"); // Текущая дата
        }

        private void ConnectToDatabase()
        {
            string dbPath = "C:\\Users\\KyCyMaMa\\Desktop\\Bank.db";
            connection = new SQLiteConnection($"Data Source={dbPath}");
            connection.Open();
        }

        private void LoadClientData()
        {
            string dbPath = "C:\\Users\\KyCyMaMa\\Desktop\\Bank.db";
            string connectionString = $"Data Source={dbPath}";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Клиент";

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

        private void LoadDogovorData()
        {
            string dbPath = "C:\\Users\\KyCyMaMa\\Desktop\\Bank.db";
            string connectionString = $"Data Source={dbPath}";

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
                        dataGridView2.DataSource = clientsTable;
                        dataGridView2.Columns["ID"].Visible=false;
                        dataGridView2.Columns["ID_Кредита"].Visible = false;
                        dataGridView2.Columns["ID_Клиента"].Visible = false;

                    }
                }
            }
        }

        private void LoadKreditData()
        {
            string dbPath = "C:\\Users\\KyCyMaMa\\Desktop\\Bank.db";
            string connectionString = $"Data Source={dbPath}";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Кредит";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        DataTable clientsTable = new DataTable();
                        adapter.Fill(clientsTable);
                        dataGridView3.DataSource = clientsTable;
                        dataGridView3.Columns["ID"].Visible = false;
                        dataGridView3.Columns["ID_Платежа"].Visible = false;
                    }
                }
            }
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
                        dataGridView5.DataSource = clientsTable;
                        dataGridView5.Columns["ID"].Visible = false;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            // Получаем значения из полей ввода
            string surname = textBox1.Text;
            string name = textBox2.Text;
            string lastName = textBox3.Text;
            string data_roda = textBox4.Text;
            string seria_pasporta = textBox5.Text;
            string number_pasport = textBox6.Text;
            string INN = textBox7.Text;
            string address = textBox8.Text;
            string number_phone = textBox9.Text;
            string data_reg = textBox10.Text;

            // Валидация данных
            try
            {
                // Вставляем данные в базу данных
                InsertKlientDataToDatabase(surname, name, lastName, INN, data_roda, seria_pasporta, number_pasport, address, number_phone, data_reg);
                LoadClientData();
                LoadDogovorData();
                ClearInputs();
                MessageBox.Show("Данные успешно добавлены.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Проверяем, что строка в DataGridView выбрана
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите клиента для изменения.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Получаем данные из текстовых полей
            string surname = textBox1.Text;
            string name = textBox2.Text;
            string lastName = textBox3.Text;
            string data_roda = textBox4.Text;
            string seria_pasporta = textBox5.Text;
            string number_pasporta = textBox6.Text;
            string INN = textBox7.Text;
            string address = textBox8.Text;
            string number_phone = textBox9.Text;
            string data_reg = textBox10.Text;

            // Получаем ID выбранного клиента из DataGridView
            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
            string clientId = selectedRow.Cells["ID"].Value.ToString();  // ID из скрытого столбца

            // Валидация данных
            try
            {
                // Обновляем данные клиента в базе
                UpdateKlientDataInDatabase(clientId, surname, name, lastName, INN, data_roda, seria_pasporta, number_pasporta, address, number_phone, data_reg);
                LoadClientData();  // Перезагружаем данные в DataGridView
                LoadDogovorData();
                ClearInputs();
                MessageBox.Show("Данные клиента обновлены.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Проверяем, что выбрана строка (индекс строки больше или равен 0)
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                // Заполняем текстовые поля данными из выбранной строки
                textBox1.Text = selectedRow.Cells[1].Value?.ToString() ?? string.Empty;
                textBox2.Text = selectedRow.Cells[2].Value?.ToString() ?? string.Empty;
                textBox3.Text = selectedRow.Cells[3].Value?.ToString() ?? string.Empty;
                textBox4.Text = selectedRow.Cells[4].Value?.ToString() ?? string.Empty;
                textBox5.Text = selectedRow.Cells[5].Value?.ToString() ?? string.Empty;
                textBox6.Text = selectedRow.Cells[6].Value?.ToString() ?? string.Empty;
                textBox7.Text = selectedRow.Cells[7].Value?.ToString() ?? string.Empty;
                textBox8.Text = selectedRow.Cells[8].Value?.ToString() ?? string.Empty;
                textBox9.Text = selectedRow.Cells[9].Value?.ToString() ?? string.Empty;
                textBox10.Text = selectedRow.Cells[10].Value?.ToString() ?? string.Empty;

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Получаем выбранную строку в DataGridView
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите клиента для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

            // Удаляем данные из базы данных
            try
            {
                DeleteKlientDataFromDatabase(selectedRow.Cells[0].Value.ToString());
                LoadClientData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InsertKlientDataToDatabase(string surname, string name, string lastName, string INN, string data_roda, string seria_pasporta, string number_pasporta, string address, string number_phone, string data_reg)
        {
            string currentDate = GetCurrentDate();
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = "INSERT INTO Клиент (Фамилия, Имя, Отчество, ИНН, Дата_рождения, Серия_паспорта, Номер_паспорта, Адрес_проживания, Номер_телефона, Дата_регистрации) VALUES (@surname, @name, @lastName, @INN, @data_roda, @seria_pasporta, @number_pasporta, @address, @number_phone, @data_reg)";
                command.Parameters.AddWithValue("@surname", surname);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@lastName", lastName);
                command.Parameters.AddWithValue("@INN", INN);
                command.Parameters.AddWithValue("@data_roda", data_roda);
                command.Parameters.AddWithValue("@seria_pasporta", seria_pasporta);
                command.Parameters.AddWithValue("@number_pasporta", number_pasporta);
                command.Parameters.AddWithValue("@address", address);
                command.Parameters.AddWithValue("@number_phone", number_phone);
                command.Parameters.AddWithValue("@data_reg", currentDate);
                command.ExecuteNonQuery();
            }
        }

        private void UpdateKlientDataInDatabase(string id, string surname, string name, string lastName, string INN, string data_roda, string seria_pasporta, string number_pasporta, string address, string number_phone, string data_reg)
        {
            string currentDate = GetCurrentDate();
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = "UPDATE Клиент SET Фамилия = @surname, Имя = @name, Отчество = @lastName, ИНН = @INN, Дата_рождения = @data_roda, Серия_паспорта = @seria_pasporta, Номер_паспорта = @number_pasporta, Адрес_проживания = @address, Номер_телефона = @number_phone, Дата_регистрации = @data_reg WHERE ID = @id";

                // Проверка и добавление параметров
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@surname", surname);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@lastName", lastName);
                command.Parameters.AddWithValue("@INN", INN);
                command.Parameters.AddWithValue("@data_roda", data_roda);
                command.Parameters.AddWithValue("@seria_pasporta", seria_pasporta);
                command.Parameters.AddWithValue("@number_pasporta", number_pasporta);
                command.Parameters.AddWithValue("@address", address);
                command.Parameters.AddWithValue("@number_phone", number_phone);
                command.Parameters.AddWithValue("@data_reg", data_reg);

                // Выполнение запроса
                command.ExecuteNonQuery();
            }
        }

        private void DeleteKlientDataFromDatabase(string id)
        {
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = "DELETE FROM Клиент WHERE ID = @id";
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string lastName = textBox11.Text;
            string firstName = textBox12.Text;
            string middleName = textBox13.Text;
            string birthDate = textBox14.Text;
            string passportSeries = textBox15.Text;
            string passportNumber = textBox16.Text;
            string innClient = textBox17.Text;
            string address = textBox18.Text;
            string phoneNumber = textBox19.Text;
            string bankName = comboBox1.SelectedItem?.ToString() ?? "Не выбрано";
            string creditAmount = textBox20.Text;
            string creditTerm = comboBox9.SelectedItem?.ToString() ?? "Не выбрано";
            string annualRate = comboBox10.SelectedItem?.ToString() ?? "Не выбрано";
            string creditPurpose = comboBox11.SelectedItem?.ToString() ?? "Не выбрано";

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
        }

        private void button6_Click(object sender, EventArgs e)
        {

            if (dataGridView2.SelectedRows.Count > 0)
            {
                int selectedID = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells["ID"].Value);
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Договор SET Фамилия = @LastName, Имя = @FirstName, Отчество = @MiddleName, Дата_рождения = @BirthDate, Серия_паспорта = @PassportSeries, Номер_паспорта = @PassportNumber, ИНН = @INN, Адрес_проживания = @Address, Номер_телефона = @PhoneNumber, Банк = @BankName, Сумма_кредита = @CreditAmount, Срок_кредита = @CreditTerm, Процентная_ставка = @AnnualRate, Цель_кредита = @CreditPurpose WHERE ID = @ID";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", selectedID);
                        command.Parameters.AddWithValue("@LastName", textBox11.Text);
                        command.Parameters.AddWithValue("@FirstName", textBox12.Text);
                        command.Parameters.AddWithValue("@MiddleName", textBox13.Text);
                        command.Parameters.AddWithValue("@BirthDate", textBox14.Text);
                        command.Parameters.AddWithValue("@PassportSeries", textBox15.Text);
                        command.Parameters.AddWithValue("@PassportNumber", textBox16.Text);
                        command.Parameters.AddWithValue("@INN", textBox17.Text);
                        command.Parameters.AddWithValue("@Address", textBox18.Text);
                        command.Parameters.AddWithValue("@PhoneNumber", textBox19.Text);
                        command.Parameters.AddWithValue("@BankName", comboBox1.SelectedItem?.ToString() ?? "Не выбрано");
                        command.Parameters.AddWithValue("@CreditAmount", textBox20.Text);
                        command.Parameters.AddWithValue("@CreditTerm", comboBox9.SelectedItem?.ToString() ?? "Не выбрано");
                        command.Parameters.AddWithValue("@AnnualRate", comboBox10.SelectedItem?.ToString() ?? "Не выбрано");
                        command.Parameters.AddWithValue("@CreditPurpose", comboBox11.SelectedItem?.ToString() ?? "Не выбрано");

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

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView2.Rows[e.RowIndex];

                textBox11.Text = row.Cells["Фамилия"].Value.ToString();
                textBox12.Text = row.Cells["Имя"].Value.ToString();
                textBox13.Text = row.Cells["Отчество"].Value.ToString();
                textBox14.Text = row.Cells["Дата_рождения"].Value.ToString();
                textBox15.Text = row.Cells["Серия_паспорта"].Value.ToString();
                textBox16.Text = row.Cells["Номер_паспорта"].Value.ToString();
                textBox17.Text = row.Cells["ИНН"].Value.ToString();
                textBox18.Text = row.Cells["Адрес_проживания"].Value.ToString();
                textBox19.Text = row.Cells["Номер_телефона"].Value.ToString();
                textBox20.Text = row.Cells["Сумма_кредита"].Value.ToString();
                comboBox1.SelectedItem = row.Cells["Банк"].Value.ToString();
                comboBox9.SelectedItem = row.Cells["Срок_кредита"].Value.ToString();
                comboBox10.SelectedItem = row.Cells["Процентная_ставка"].Value.ToString();
                comboBox11.SelectedItem = row.Cells["Цель_кредита"].Value.ToString();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                int selectedID = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells["ID"].Value);
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

        private void button3_Click_1(object sender, EventArgs e)
        {
            // Получаем значения из полей ввода
            string cell_credit = comboBox2.SelectedItem?.ToString() ?? "Не выбрано";
            string srok = comboBox3.SelectedItem?.ToString() ?? "Не выбрано";
            string status = comboBox4.SelectedItem?.ToString() ?? "Не выбрано";
            string data_vidachi = textBox21.Text;
            string valuta = comboBox5.SelectedItem?.ToString() ?? "Не выбрано";
            string suma = textBox22.Text;
            string data_platezha = textBox23.Text;
            string proc = comboBox13.Text;

            // Вставляем данные в базу данных
            InsertKreditDataToDatabase(cell_credit, srok, status, data_vidachi, valuta, suma, data_platezha, proc);

            // Обновляем DataGridView
            LoadKreditData();
            ClearInputs();
            MessageBox.Show("Данные успешно добавлены.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            // Проверяем, что строка в DataGridView выбрана
            if (dataGridView3.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите договор для изменения.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Получаем данные из текстовых полей
            string cell_credit = comboBox2.SelectedItem?.ToString() ?? "Не выбрано";
            string srok = comboBox3.SelectedItem?.ToString() ?? "Не выбрано";
            string status = comboBox4.SelectedItem?.ToString() ?? "Не выбрано";
            string data_vidachi = textBox21.Text;
            string valuta = comboBox5.SelectedItem?.ToString() ?? "Не выбрано";
            string suma = textBox22.Text;
            string data_platezha = textBox23.Text;
            string proc = comboBox13.Text;

            // Получаем ID выбранного договора из DataGridView2
            DataGridViewRow selectedRow = dataGridView3.SelectedRows[0];
            string kreditId = selectedRow.Cells["ID"].Value.ToString();// ID из скрытого столбца
            

            try
            {
                // Обновляем данные договора
                UpdateKreditDataInDatabase(kreditId, cell_credit, srok, status, data_vidachi, valuta, suma, data_platezha, proc);
                LoadKreditData(); // Перезагружаем данные
                MessageBox.Show("Данные договора обновлены.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ClearInputs();
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Проверяем, что выбрана строка (индекс строки больше или равен 0)
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView3.Rows[e.RowIndex];

                // Заполняем текстовые поля данными из выбранной строки
                comboBox2.Text = selectedRow.Cells[2].Value?.ToString() ?? string.Empty;
                comboBox3.Text = selectedRow.Cells[3].Value?.ToString() ?? string.Empty;
                comboBox4.Text = selectedRow.Cells[4].Value?.ToString() ?? string.Empty;
                textBox21.Text = selectedRow.Cells[5].Value?.ToString() ?? string.Empty;
                comboBox5.Text = selectedRow.Cells[6].Value?.ToString() ?? string.Empty;
                textBox22.Text = selectedRow.Cells[7].Value?.ToString() ?? string.Empty;
                textBox23.Text = selectedRow.Cells[8].Value?.ToString() ?? string.Empty;
                comboBox13.Text = selectedRow.Cells[9].Value.ToString() ?? string.Empty;        
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            // Получаем выбранную строку в DataGridView
            if (dataGridView3.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите кредит для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = dataGridView3.SelectedRows[0];

            // Удаляем данные из базы данных
            try
            {
                DeleteKreditDataFromDatabase(selectedRow.Cells[0].Value.ToString());
                LoadKreditData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InsertKreditDataToDatabase(string cell_credit, string srok, string status, string data_vidachi, string valuta, string suma, string data_platezha, string proc)
        {
            string currentDate = GetCurrentDate(); // Получение текущей даты
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = "INSERT INTO Кредит (Цель_кредита, Срок_кредита, Статус_кредита, Дата_выдачи_кредита, Валюта_кредита, Сумма_кредита, Дата_платежа, Процентная_ставка) " +
                                      "VALUES (@cell_credit, @srok, @status, @data_vidachi, @valuta, @suma, @data_platezha, @proc)";
                command.Parameters.AddWithValue("@cell_credit", cell_credit);
                command.Parameters.AddWithValue("@srok", srok);
                command.Parameters.AddWithValue("@status", status);
                command.Parameters.AddWithValue("@data_vidachi", currentDate);
                command.Parameters.AddWithValue("@valuta", valuta);
                command.Parameters.AddWithValue("@suma", suma);
                command.Parameters.AddWithValue("@data_platezha", currentDate);
                command.Parameters.AddWithValue("proc", proc);
                command.ExecuteNonQuery();
            }
        }

        private void UpdateKreditDataInDatabase(string id, string cell_credit, string srok, string status, string data_vidachi, string valuta, string suma, string data_platezha, string proc)
        {
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = "UPDATE Кредит SET Цель_кредита = @cell_credit, Срок_кредита = @srok, Статус_кредита = @status, Дата_выдачи_кредита = @data_vidachi, Валюта_кредита = @valuta, Сумма_кредита = @suma, Дата_платежа = @data_platezha, Процентная_ставка = @proc WHERE ID = @id";
                command.Parameters.AddWithValue("id", id);
                command.Parameters.AddWithValue("@cell_credit", cell_credit);
                command.Parameters.AddWithValue("@srok", srok);
                command.Parameters.AddWithValue("@status", status);
                command.Parameters.AddWithValue("@data_vidachi", data_vidachi);
                command.Parameters.AddWithValue("@valuta", valuta);
                command.Parameters.AddWithValue("@suma", suma);
                command.Parameters.AddWithValue("@data_platezha", data_platezha);
                command.Parameters.AddWithValue("proc", proc);
                command.ExecuteNonQuery();
            }
            LoadKreditData();
            ClearInputs();
        }

        private void DeleteKreditDataFromDatabase(string id)
        {
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = "DELETE FROM Кредит WHERE ID = @id";
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            // Получаем значения из полей ввода
            string data_plata = textBox25.Text;
            string tip_plata = textBox26.Text;
            string vid_plata = textBox27.Text;
            string summa_kredita = textBox28.Text;
            string summa_plata = textBox29.Text;
             

            // Вставляем данные в базу данных
            InsertPlatechDataToDatabase(data_plata, tip_plata, vid_plata, summa_kredita, summa_plata);
            LoadPlatechData();
            ClearInputs();
            MessageBox.Show("Данные успешно добавлены.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button14_Click_1(object sender, EventArgs e)
        {
            // Проверяем, что строка в DataGridView выбрана
            if (dataGridView5.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите договор для изменения.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Получаем данные из текстовых полей
            string data_plata = textBox25.Text;
            string tip_plata = textBox26.Text;
            string vid_plata = textBox27.Text;
            string summa_kredita = textBox28.Text;
            string summa_plata = textBox29.Text;
            

            // Получаем ID выбранного договора из DataGridView2
            DataGridViewRow selectedRow = dataGridView5.SelectedRows[0];
            string kreditId = selectedRow.Cells["ID"].Value.ToString();// ID из скрытого столбца


            try
            {
                // Обновляем данные договора
                UpdatePlatechDataInDatabase(kreditId, data_plata, tip_plata, vid_plata, summa_plata, summa_kredita);
                LoadPlatechData(); // Перезагружаем данные
                MessageBox.Show("Данные договора обновлены.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ClearInputs();

        }

        private void dataGridView5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Проверяем, что выбрана строка (индекс строки больше или равен 0)
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView5.Rows[e.RowIndex];

                // Заполняем текстовые поля данными из выбранной строки
                textBox25.Text = selectedRow.Cells[1].Value?.ToString() ?? string.Empty;
                textBox26.Text = selectedRow.Cells[2].Value?.ToString() ?? string.Empty;
                textBox27.Text = selectedRow.Cells[3].Value?.ToString() ?? string.Empty;
                textBox28.Text = selectedRow.Cells[4].Value?.ToString() ?? string.Empty;
                textBox29.Text = selectedRow.Cells[5].Value?.ToString() ?? string.Empty;

                
            }
        }

        private void button15_Click_1(object sender, EventArgs e)
        {
            // Получаем выбранную строку в DataGridView
            if (dataGridView5.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите кредит для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = dataGridView5.SelectedRows[0];

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

        private void InsertPlatechDataToDatabase(string data_plata, string tip_plata, string vid_plata, string summa_kredita, string summa_plata)
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

        private void UpdatePlatechDataInDatabase(string id, string data_plata, string tip_plata, string vid_plata, string summa_plata, string summa_kredita)
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

        private void DeletePlatechDataFromDatabase(string id)
        {
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = "DELETE FROM Платеж WHERE ID = @id";
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        private void InitializeFormComponents()
        {
            // Создаем объект MenuStrip
            MenuStrip menuStrip = new MenuStrip();

            // Создаем элементы меню
            ToolStripMenuItem refreshMenuItem = new ToolStripMenuItem("Обновить");
            ToolStripMenuItem historyMenuItem = new ToolStripMenuItem("История");
            ToolStripMenuItem exitMenuItem = new ToolStripMenuItem("Выход");

            // Привязываем обработчики событий к пунктам меню
            refreshMenuItem.Click += RefreshMenuItem_Click;
            historyMenuItem.Click += HistoryMenuItem_Click;
            exitMenuItem.Click += ExitMenuItem_Click;

            // Добавляем элементы меню в MenuStrip
            menuStrip.Items.Add(refreshMenuItem);
            menuStrip.Items.Add(historyMenuItem);
            menuStrip.Items.Add(exitMenuItem);

            // Устанавливаем MenuStrip для формы
            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);

            // Настройки формы
            this.Text = "Form3";
            this.Width = 600;
            this.Height = 300;
        }

        private void RefreshMenuItem_Click(object sender, EventArgs e)
        {
            // Логика для кнопки "Обновить"
            LoadClientData();
            LoadDogovorData();
            LoadKreditData();
            LoadPlatechData();
            MessageBox.Show("Данные обновлены", "Обновление");
        }

        private void HistoryMenuItem_Click(object sender, EventArgs e)
        {
            // Логика для кнопки "История"
            MessageBox.Show("История действий", "История");
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            // Логика для кнопки "Выход"
            var result = MessageBox.Show("Вы уверены, что хотите выйти?", "Выход", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            // Получаем выбранный критерий поиска
            string selectedCriteria = comboBox6.SelectedItem?.ToString();
            string searchValue = textBox30.Text.Trim();

            if (string.IsNullOrEmpty(selectedCriteria) || string.IsNullOrEmpty(searchValue))
            {
                MessageBox.Show("Выберите критерий и введите значение для поиска.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Формируем запрос поиска
                string query = $"SELECT * FROM Клиент WHERE {selectedCriteria} LIKE @searchValue";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@searchValue", $"%{searchValue}%");
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        DataTable searchResults = new DataTable();
                        adapter.Fill(searchResults);

                        // Обновляем DataGridView
                        dataGridView1.DataSource = searchResults;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSearchCriteria()
        {
            try
            {
                // SQL-запрос для получения имен столбцов таблицы Клиент
                string query0 = "PRAGMA table_info(Клиент)"; // SQLite: возвращает информацию о столбцах таблицы
                string query1 = "PRAGMA table_info(Договор)";
                string query2 = "PRAGMA table_info(Кредит)";
                string query3 = "PRAGMA table_info(Платеж)";

                using (SQLiteCommand command = new SQLiteCommand(query0, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Добавляем имена столбцов в ComboBox
                            comboBox6.Items.Add(reader["name"].ToString());
                        }
                    }
                }

                using (SQLiteCommand command = new SQLiteCommand(query1, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Добавляем имена столбцов в ComboBox
                            comboBox7.Items.Add(reader["name"].ToString());
                        }
                    }
                }

                using (SQLiteCommand command = new SQLiteCommand(query2, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Добавляем имена столбцов в ComboBox
                            comboBox8.Items.Add(reader["name"].ToString());
                        }
                    }
                }

                using (SQLiteCommand command = new SQLiteCommand(query3, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Добавляем имена столбцов в ComboBox
                            comboBox12.Items.Add(reader["name"].ToString());
                        }
                    }
                }

                // Установить значение по умолчанию, если список не пуст
                if (comboBox6.Items.Count > 0)
                {
                    comboBox6.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("Не удалось загрузить критерии поиска. Таблица не содержит столбцов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке критериев: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            // Получаем выбранный критерий поиска
            string selectedCriteria = comboBox7.SelectedItem?.ToString();
            string searchValue = textBox31.Text.Trim();

            if (string.IsNullOrEmpty(selectedCriteria) || string.IsNullOrEmpty(searchValue))
            {
                MessageBox.Show("Выберите критерий и введите значение для поиска.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Формируем запрос поиска
                string query = $"SELECT * FROM Договор WHERE {selectedCriteria} LIKE @searchValue";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@searchValue", $"%{searchValue}%");
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        DataTable searchResults = new DataTable();
                        adapter.Fill(searchResults);

                        // Обновляем DataGridView
                        dataGridView2.DataSource = searchResults;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            // Получаем выбранный критерий поиска
            string selectedCriteria = comboBox8.SelectedItem?.ToString();
            string searchValue = textBox32.Text.Trim();

            if (string.IsNullOrEmpty(selectedCriteria) || string.IsNullOrEmpty(searchValue))
            {
                MessageBox.Show("Выберите критерий и введите значение для поиска.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Формируем запрос поиска
                string query = $"SELECT * FROM Кредит WHERE {selectedCriteria} LIKE @searchValue";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@searchValue", $"%{searchValue}%");
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        DataTable searchResults = new DataTable();
                        adapter.Fill(searchResults);

                        // Обновляем DataGridView
                        dataGridView3.DataSource = searchResults;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            // Получаем выбранный критерий поиска
            string selectedCriteria = comboBox12.SelectedItem?.ToString();
            string searchValue = textBox33.Text.Trim();

            if (string.IsNullOrEmpty(selectedCriteria) || string.IsNullOrEmpty(searchValue))
            {
                MessageBox.Show("Выберите критерий и введите значение для поиска.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Формируем запрос поиска
                string query = $"SELECT * FROM Платеж WHERE {selectedCriteria} LIKE @searchValue";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@searchValue", $"%{searchValue}%");
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        DataTable searchResults = new DataTable();
                        adapter.Fill(searchResults);

                        // Обновляем DataGridView
                        dataGridView5.DataSource = searchResults;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            textBox11.Clear();
            textBox12.Clear();
            textBox13.Clear();
            textBox14.Clear();
            textBox15.Clear();
            textBox16.Clear();
            textBox17.Clear();
            textBox18.Clear();
            textBox19.Clear();
            textBox20.Clear();
            textBox21.Clear();
            textBox22.Clear();
            textBox23.Clear();
            textBox25.Clear();
            textBox26.Clear();
            textBox27.Clear();
            textBox28.Clear();
            textBox29.Clear();
            textBox30.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
            comboBox5.SelectedIndex = -1;
            comboBox9.SelectedIndex = -1;
            comboBox10.SelectedIndex = -1;
            comboBox11.SelectedIndex = -1;
        }
    }
}
    

