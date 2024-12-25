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
            connectionString = $"Data Source={dbPath}";
            InitializeFormComponents();
            InitializeComponent();
            ConnectToDatabase();
            LoadClientData();
            LoadDogovorData();
            LoadKreditData();
            LoadPlatechData();
            LoadSearchCriteria();
            LoadComboBox1FromBank();
            LoadComboBox2FromBank();
            LoadClientsToComboBox();
            dataGridView2.SelectionChanged += dataGridView2_SelectionChanged;
            dataGridView3.SelectionChanged += dataGridView3_SelectionChanged;
            dataGridView4.SelectionChanged += dataGridView4_SelectionChanged;
            dataGridView1.ReadOnly = true;
            dataGridView2.ReadOnly = true;
            dataGridView3.ReadOnly = true;
            dataGridView4.ReadOnly = true;
            comboBox11.Items.AddRange(new object[] { "Потребительские нужды", "Жилищные цели", "Автокредит", "Образование", "Личные цели", "Бизнес" });
            comboBox2.Items.AddRange(new object[] { "Потребительские нужды", "Жилищные цели", "Автокредит", "Образование", "Личные цели", "Бизнес" });
            comboBox9.Items.AddRange(new object[] { "12 месяцев", "24 месяца", "36 месяцев", "48 месяцев", "60 месяцев" });
            comboBox3.Items.AddRange(new object[] { "12 месяцев", "24 месяца", "36 месяцев", "48 месяцев", "60 месяцев" });
            comboBox4.Items.AddRange(new object[] { "Активен", "Погашен" });
            comboBox10.Items.AddRange(new object[] { "5.3%", "6.7%", "12%", "6.2%", "5.7%" });
            comboBox13.Items.AddRange(new object[] { "5.3%", "6.7%", "12%", "6.2%", "5.7%" });


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
                        dataGridView2.Columns["ID_Сотрудника"].Visible = false;

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
                        dataGridView4.DataSource = clientsTable;
                        dataGridView4.Columns["ID"].Visible = false;
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

            // Проверка на пустые поля
            if (string.IsNullOrWhiteSpace(surname) || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(data_roda) || string.IsNullOrWhiteSpace(seria_pasporta) || string.IsNullOrWhiteSpace(number_pasport) ||
                string.IsNullOrWhiteSpace(INN) || string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(number_phone) || string.IsNullOrWhiteSpace(data_reg))
            {
                MessageBox.Show("Все поля должны быть заполнены.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Валидация ИНН (должен состоять из 8 цифр)
            if (!IsValidINN(INN))
            {
                MessageBox.Show("ИНН должен состоять из 8 цифр.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Валидация серии паспорта (должна состоять из 4 цифр)
            if (!IsValidSeriaPasporta(seria_pasporta))
            {
                MessageBox.Show("Серия паспорта должна состоять из 4 цифр.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Валидация номера паспорта (должен состоять из 6 цифр)
            if (!IsValidNumberPasporta(number_pasport))
            {
                MessageBox.Show("Номер паспорта должен состоять из 6 цифр.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Валидация телефона (начинается с +7 или 8 и 10 цифр)
            if (!IsValidPhoneNumber(number_phone))
            {
                MessageBox.Show("Номер телефона должен начинаться с +7 или 8 и содержать 10 цифр.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Валидация ФИО (только буквы)
            if (!IsValidName(surname) || !IsValidName(name) || !IsValidName(lastName))
            {
                MessageBox.Show("ФИО должно содержать только буквы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Валидация адреса (только буквы)
            if (!IsValidAddress(address))
            {
                MessageBox.Show("Адрес должен содержать только буквы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Валидация даты рождения
            if (!IsValidDate(data_roda))
            {
                MessageBox.Show("Некорректная дата рождения.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Валидация даты регистрации
            if (!IsValidDate(data_reg))
            {
                MessageBox.Show("Некорректная дата регистрации.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Вставляем данные в базу данных
            try
            {
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

            // Проверка на пустые поля
            if (string.IsNullOrWhiteSpace(surname) || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(data_roda) || string.IsNullOrWhiteSpace(seria_pasporta) || string.IsNullOrWhiteSpace(number_pasporta) ||
                string.IsNullOrWhiteSpace(INN) || string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(number_phone) || string.IsNullOrWhiteSpace(data_reg))
            {
                MessageBox.Show("Все поля должны быть заполнены.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Валидация ИНН
            if (!IsValidINN(INN))
            {
                MessageBox.Show("ИНН должен состоять из 8 цифр.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Валидация серии паспорта
            if (!IsValidSeriaPasporta(seria_pasporta))
            {
                MessageBox.Show("Серия паспорта должна состоять из 4 цифр.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Валидация номера паспорта
            if (!IsValidNumberPasporta(number_pasporta))
            {
                MessageBox.Show("Номер паспорта должен состоять из 6 цифр.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Валидация телефона
            if (!IsValidPhoneNumber(number_phone))
            {
                MessageBox.Show("Номер телефона должен начинаться с +7 или 8 и содержать 10 цифр.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Валидация ФИО
            if (!IsValidName(surname) || !IsValidName(name) || !IsValidName(lastName))
            {
                MessageBox.Show("ФИО должно содержать только буквы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Валидация адреса
            if (!IsValidAddress(address))
            {
                MessageBox.Show("Адрес должен содержать только буквы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Валидация даты рождения
            if (!IsValidDate(data_roda))
            {
                MessageBox.Show("Некорректная дата рождения.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Валидация даты регистрации
            if (!IsValidDate(data_reg))
            {
                MessageBox.Show("Некорректная дата регистрации.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Обновляем данные клиента в базе
            try
            {
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
            MessageBox.Show("Данные успешно добавлены", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

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

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView2.SelectedRows[0];

                textBox11.Text = selectedRow.Cells["Фамилия"].Value?.ToString() ?? "";
                textBox12.Text = selectedRow.Cells["Имя"].Value?.ToString() ?? "";
                textBox13.Text = selectedRow.Cells["Отчество"].Value?.ToString() ?? "";
                textBox14.Text = selectedRow.Cells["Дата_рождения"].Value?.ToString() ?? "";
                textBox15.Text = selectedRow.Cells["Серия_паспорта"].Value?.ToString() ?? "";
                textBox16.Text = selectedRow.Cells["Номер_паспорта"].Value?.ToString() ?? "";
                textBox17.Text = selectedRow.Cells["ИНН"].Value?.ToString() ?? "";
                textBox18.Text = selectedRow.Cells["Адрес_проживания"].Value?.ToString() ?? "";
                textBox19.Text = selectedRow.Cells["Номер_телефона"].Value?.ToString() ?? "";
                textBox20.Text = selectedRow.Cells["Сумма_кредита"].Value?.ToString() ?? "";
                comboBox1.SelectedItem = selectedRow.Cells["Банк"].Value?.ToString() ?? "";
                comboBox9.SelectedItem = selectedRow.Cells["Срок_кредита"].Value?.ToString() ?? "";
                comboBox10.SelectedItem = selectedRow.Cells["Процентная_ставка"].Value?.ToString() ?? "";
                comboBox11.SelectedItem = selectedRow.Cells["Цель_кредита"].Value?.ToString() ?? "";
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
            string suma = textBox22.Text;
            string proc = comboBox13.Text;

            // Вставляем данные в базу данных
            InsertKreditDataToDatabase(cell_credit, srok, status, data_vidachi, suma, proc);

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
            string suma = textBox22.Text;
            string proc = comboBox13.Text;

            // Получаем ID выбранного договора из DataGridView2
            DataGridViewRow selectedRow = dataGridView3.SelectedRows[0];
            string kreditId = selectedRow.Cells["ID"].Value.ToString();// ID из скрытого столбца
            

            try
            {
                // Обновляем данные договора
                UpdateKreditDataInDatabase(kreditId, cell_credit, srok, status, data_vidachi, suma, proc);
                LoadKreditData(); // Перезагружаем данные
                MessageBox.Show("Данные договора обновлены.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ClearInputs();
        }

        private void dataGridView3_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView3.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView3.SelectedRows[0];

                textBox22.Text = selectedRow.Cells["Сумма_кредита"].Value?.ToString() ?? "";
                textBox21.Text = selectedRow.Cells["Дата_выдачи_кредита"].Value?.ToString() ?? "";
                comboBox2.SelectedItem = selectedRow.Cells["Цель_кредита"].Value?.ToString() ?? "";
                comboBox3.SelectedItem = selectedRow.Cells["Срок_кредита"].Value?.ToString() ?? "";
                comboBox4.SelectedItem = selectedRow.Cells["Статус_кредита"].Value?.ToString() ?? "";
                comboBox13.SelectedItem = selectedRow.Cells["Процентная_ставка"].Value?.ToString() ?? "";
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

            // Удаляем данные из базы данных и заносим их в таблицу "Удаленные_Договоры"
            try
            {
                string lastName = selectedRow.Cells["Фамилия"].Value.ToString();
                string firstName = selectedRow.Cells["Имя"].Value.ToString();
                string middleName = selectedRow.Cells["Отчество"].Value.ToString();
                string birthDate = selectedRow.Cells["Дата_рождения"].Value.ToString();
                int passportSeries = Convert.ToInt32(selectedRow.Cells["Серия_паспорта"].Value);
                int passportNumber = Convert.ToInt32(selectedRow.Cells["Номер_паспорта"].Value);
                int inn = Convert.ToInt32(selectedRow.Cells["ИНН"].Value);
                string address = selectedRow.Cells["Адрес_проживания"].Value.ToString();
                long phoneNumber = Convert.ToInt64(selectedRow.Cells["Номер_телефона"].Value);
                string bank = selectedRow.Cells["Банк"].Value.ToString();
                int loanAmount = Convert.ToInt32(selectedRow.Cells["Сумма_кредита"].Value);
                string loanTerm = selectedRow.Cells["Срок_кредита"].Value.ToString();
                string interestRate = selectedRow.Cells["Процентная_ставка"].Value.ToString();
                string loanPurpose = selectedRow.Cells["Цель_кредита"].Value.ToString();

                ArchiveAndDeleteKreditData(lastName, firstName, middleName, birthDate, passportSeries, passportNumber, inn, address, phoneNumber, bank, loanAmount, loanTerm, interestRate, loanPurpose);
                LoadKreditData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ArchiveAndDeleteKreditData(string lastName, string firstName, string middleName, string birthDate, int passportSeries, int passportNumber, int inn, string address, long phoneNumber, string bank, int loanAmount, string loanTerm, string interestRate, string loanPurpose)
        {
            string dbPath = "C:\\Users\\KyCyMaMa\\Desktop\\Bank.db";
            string connectionString = $"Data Source={dbPath};Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Добавляем данные в таблицу "Удаленные_Договоры"
                        string insertQuery = @"INSERT INTO Удаленные_Договоры (Фамилия, Имя, Отчество, Дата_рождения, Серия_паспорта, Номер_паспорта, ИНН, Адрес_проживания, Номер_телефона, Банк, Сумма_кредита, Срок_кредита, Процентная_ставка, Цель_кредита) 
                                       VALUES (@LastName, @FirstName, @MiddleName, @BirthDate, @PassportSeries, @PassportNumber, @Inn, @Address, @PhoneNumber, @Bank, @LoanAmount, @LoanTerm, @InterestRate, @LoanPurpose)";
                        using (SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, connection, transaction))
                        {
                            insertCommand.Parameters.AddWithValue("@LastName", lastName);
                            insertCommand.Parameters.AddWithValue("@FirstName", firstName);
                            insertCommand.Parameters.AddWithValue("@MiddleName", middleName);
                            insertCommand.Parameters.AddWithValue("@BirthDate", birthDate);
                            insertCommand.Parameters.AddWithValue("@PassportSeries", passportSeries);
                            insertCommand.Parameters.AddWithValue("@PassportNumber", passportNumber);
                            insertCommand.Parameters.AddWithValue("@Inn", inn);
                            insertCommand.Parameters.AddWithValue("@Address", address);
                            insertCommand.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                            insertCommand.Parameters.AddWithValue("@Bank", bank);
                            insertCommand.Parameters.AddWithValue("@LoanAmount", loanAmount);
                            insertCommand.Parameters.AddWithValue("@LoanTerm", loanTerm);
                            insertCommand.Parameters.AddWithValue("@InterestRate", interestRate);
                            insertCommand.Parameters.AddWithValue("@LoanPurpose", loanPurpose);

                            insertCommand.ExecuteNonQuery();
                        }

                        // Удаляем данные из основной таблицы
                        string deleteQuery = @"DELETE FROM Договор 
                                       WHERE Фамилия = @LastName AND Имя = @FirstName AND Отчество = @MiddleName AND Серия_паспорта = @PassportSeries AND Номер_паспорта = @PassportNumber";
                        using (SQLiteCommand deleteCommand = new SQLiteCommand(deleteQuery, connection, transaction))
                        {
                            deleteCommand.Parameters.AddWithValue("@LastName", lastName);
                            deleteCommand.Parameters.AddWithValue("@FirstName", firstName);
                            deleteCommand.Parameters.AddWithValue("@MiddleName", middleName);
                            deleteCommand.Parameters.AddWithValue("@PassportSeries", passportSeries);
                            deleteCommand.Parameters.AddWithValue("@PassportNumber", passportNumber);

                            deleteCommand.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private void InsertKreditDataToDatabase(string cell_credit, string srok, string status, string data_vidachi, string suma, string proc)
        {
            string currentDate = GetCurrentDate(); // Получение текущей даты
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = "INSERT INTO Кредит (Цель_кредита, Срок_кредита, Статус_кредита, Дата_выдачи_кредита, Сумма_кредита, Процентная_ставка) " +
                                      "VALUES (@cell_credit, @srok, @status, @data_vidachi, @suma, @data_platezha, @proc)";
                command.Parameters.AddWithValue("@cell_credit", cell_credit);
                command.Parameters.AddWithValue("@srok", srok);
                command.Parameters.AddWithValue("@status", status);
                command.Parameters.AddWithValue("@data_vidachi", currentDate);
                command.Parameters.AddWithValue("@suma", suma);
                command.Parameters.AddWithValue("proc", proc);
                command.ExecuteNonQuery();
            }
        }

        private void UpdateKreditDataInDatabase(string id, string cell_credit, string srok, string status, string data_vidachi, string suma, string proc)
        {
            string currentDate = GetCurrentDate();
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = "UPDATE Кредит SET Цель_кредита = @cell_credit, Срок_кредита = @srok, Статус_кредита = @status, Дата_выдачи_кредита = @data_vidachi, Сумма_кредита = @suma, Процентная_ставка = @proc WHERE ID = @id";
                command.Parameters.AddWithValue("id", id);
                command.Parameters.AddWithValue("@cell_credit", cell_credit);
                command.Parameters.AddWithValue("@srok", srok);
                command.Parameters.AddWithValue("@status", status);
                command.Parameters.AddWithValue("@data_vidachi", data_vidachi);
                command.Parameters.AddWithValue("@suma", suma);
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
            string cell_credit = textBox23.Text;
            string data_plata = textBox24.Text;
            string vid_plata = textBox25.Text;
            string summa_kredita = textBox27.Text;
            string summa_plata = textBox28.Text;
            string fio = comboBox12.Text;
            string remainingDebtStr = textBox29.Text;


        if (string.IsNullOrWhiteSpace(cell_credit) || string.IsNullOrWhiteSpace(data_plata) || string.IsNullOrWhiteSpace(vid_plata) ||
        string.IsNullOrWhiteSpace(summa_kredita) || string.IsNullOrWhiteSpace(summa_plata) || string.IsNullOrWhiteSpace(fio) || string.IsNullOrWhiteSpace(remainingDebtStr))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Прерываем выполнение метода, если есть пустые поля
            }

            // Вставляем данные в базу данных
            InsertPlatechDataToDatabase(fio, data_plata, vid_plata, summa_kredita, summa_plata, cell_credit, remainingDebtStr);
            LoadPlatechData();
            ClearInputs();
            MessageBox.Show("Данные успешно добавлены.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button14_Click_1(object sender, EventArgs e)
        {
            // Проверяем, что строка в DataGridView выбрана
            if (dataGridView4.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите договор для изменения.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Получаем данные из текстовых полей
            string cell_credit = textBox23.Text;
            string data_plata = textBox24.Text;
            string vid_plata = textBox25.Text;
            string summa_kredita = textBox27.Text;
            string summa_plata = textBox28.Text;
            string fio = comboBox12.Text;
            string remainingDebtStr = textBox29.Text;


            // Получаем ID выбранного договора из DataGridView2
            DataGridViewRow selectedRow = dataGridView4.SelectedRows[0];
            string kreditId = selectedRow.Cells["ID"].Value.ToString();// ID из скрытого столбца


            try
            {
                // Обновляем данные договора
                UpdatePlatechDataInDatabase(kreditId, fio, data_plata, vid_plata, summa_kredita, summa_plata, cell_credit, remainingDebtStr);
                LoadPlatechData(); 
                ClearInputs();
                MessageBox.Show("Данные договора обновлены.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ClearInputs();

        }

        private void dataGridView4_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView4.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView4.SelectedRows[0];

                textBox24.Text = selectedRow.Cells["Дата_платежа"].Value?.ToString() ?? "";
                textBox25.Text = selectedRow.Cells["Вид_платежа"].Value?.ToString() ?? "";
                textBox28.Text = selectedRow.Cells["Сумма_платежа"].Value?.ToString() ?? "";
                textBox29.Text = selectedRow.Cells["Остаток_долга"].Value?.ToString() ?? "";
                textBox23.Text = selectedRow.Cells["Цель_кредита"].Value?.ToString() ?? "";
                comboBox12.SelectedItem = selectedRow.Cells["ФИО"].Value?.ToString() ?? "";
            }
        }

        private void button15_Click_1(object sender, EventArgs e)
        {
            // Получаем выбранную строку в DataGridView
            if (dataGridView4.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите платеж для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = dataGridView4.SelectedRows[0];

            // Удаляем данные из базы данных
            try
            {
                DeletePlatechDataFromDatabase(selectedRow.Cells[0].Value.ToString());
                LoadPlatechData();
                MessageBox.Show("Платеж успешно удален.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void UpdatePlatechDataInDatabase(string id, string fio, string data_plata, string vid_plata, string summa_kredita, string summa_plata, string cell_credit, string remainingDebtStr)
        {
            // Преобразуем суммы в числа для выполнения арифметической операции
            decimal sumaKreditaDecimal = 0;
            decimal sumaPlataDecimal = 0;
            decimal remainingDebt = 0;

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

            string currentDate = GetCurrentDate();

            using (var connection = new SQLiteConnection("Data Source=C:\\Users\\KyCyMaMa\\Desktop\\Bank.db"))
            {
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "UPDATE Платеж SET ФИО = @fio, Цель_кредита = @cell_credit, Дата_платежа = @data_plata, Вид_платежа = @vid_plata, " +
                                          "Сумма_кредита = @summa_kredita, Сумма_платежа = @summa_plata, Остаток_долга = @remaining_debt, Статус_кредита = @status " +
                                          "WHERE ID = @id";

                    command.Parameters.AddWithValue("@fio", fio);
                    command.Parameters.AddWithValue("@cell_credit", cell_credit);
                    command.Parameters.AddWithValue("@data_plata", currentDate);
                    command.Parameters.AddWithValue("@vid_plata", vid_plata);
                    command.Parameters.AddWithValue("@summa_kredita", sumaKreditaDecimal > 0 ? sumaKreditaDecimal.ToString("F2") : "");
                    command.Parameters.AddWithValue("@summa_plata", sumaPlataDecimal.ToString("F2"));
                    command.Parameters.AddWithValue("@remaining_debt", remainingDebt.ToString("F2"));
                    command.Parameters.AddWithValue("@status", status);
                    command.Parameters.AddWithValue("@id", id);

                    command.ExecuteNonQuery();
                }
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

        private void comboBox12_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox12.SelectedItem != null)
            {
                string selectedClient = comboBox12.SelectedItem.ToString();
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
                                textBox23.Text = reader["Цель_кредита"].ToString();
                                textBox27.Text = reader["Сумма_кредита"].ToString();
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
                            comboBox12.Items.Add(reader["FullName"].ToString());
                        }
                    }
                }
            }
        }

        private void InitializeFormComponents()
        {
            // Создаем объект MenuStrip
            MenuStrip menuStrip = new MenuStrip();

            // Создаем элементы меню
            ToolStripMenuItem refreshMenuItem = new ToolStripMenuItem("Обновить");
            ToolStripMenuItem exitMenuItem = new ToolStripMenuItem("Выход");

            // Привязываем обработчики событий к пунктам меню
            refreshMenuItem.Click += RefreshMenuItem_Click;
            exitMenuItem.Click += ExitMenuItem_Click;

            // Добавляем элементы меню в MenuStrip
            menuStrip.Items.Add(refreshMenuItem);
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

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите выйти?", "Выход", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Form1 form1 = new Form1();  // Создаем новый экземпляр Form1
                form1.Show();  // Открываем Form1
                this.Hide();  // Скрываем текущую форму
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
                            string columnName = reader["name"].ToString();

                            // Пропускаем столбец с именем "ID"
                            if (columnName != "ID")
                            {
                                comboBox6.Items.Add(columnName);
                            }
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
                            string columnName1 = reader["name"].ToString();

                            // Пропускаем столбец с именем "ID"
                            if (columnName1 != "ID" && columnName1 != "ID_Клиента" && columnName1 != "ID_Кредита" && columnName1 != "ID_Сотрудника")
                            {
                                comboBox7.Items.Add(columnName1);
                            }
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
                            string columnName2 = reader["name"].ToString();

                            // Пропускаем столбец с именем "ID"
                            if (columnName2 != "ID" && columnName2 != "ID_Платежа")
                            {
                                comboBox8.Items.Add(columnName2);
                            }
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
                            string columnName3 = reader["name"].ToString();

                            // Пропускаем столбец с именем "ID"
                            if (columnName3 != "ID")
                            {
                                comboBox14.Items.Add(columnName3);
                            }
                        }
                    }
                }

                // Установить значение по умолчанию, если список не пуст
                if (comboBox6.Items.Count > 0 || comboBox7.Items.Count > 0 || comboBox8.Items.Count > 0 || comboBox14.Items.Count > 0)
                {
                    comboBox6.SelectedIndex = 0;
                    comboBox7.SelectedIndex = 0;
                    comboBox8.SelectedIndex = 0;
                    comboBox14.SelectedIndex = 0;
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
            string selectedCriteria = comboBox14.SelectedItem?.ToString();
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
                        dataGridView4.DataSource = searchResults;
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
            comboBox9.SelectedIndex = -1;
            comboBox10.SelectedIndex = -1;
            comboBox11.SelectedIndex = -1;
        }

        // Валидация ИНН (должен состоять из 8 цифр)
        private bool IsValidINN(string inn)
        {
            return inn.Length == 8 && inn.All(char.IsDigit);
        }

        // Валидация серии паспорта (должна состоять из 4 цифр)
        private bool IsValidSeriaPasporta(string seria)
        {
            return seria.Length == 4 && seria.All(char.IsDigit);
        }

        // Валидация номера паспорта (должен состоять из 6 цифр)
        private bool IsValidNumberPasporta(string number)
        {
            return number.Length == 6 && number.All(char.IsDigit);
        }

        // Валидация телефона (начинается с +7 или 8 и 10 цифр)
        private bool IsValidPhoneNumber(string phone)
        {
            return (phone.StartsWith("+7") && phone.Length == 12 && phone.Substring(2).All(char.IsDigit)) ||
                   (phone.StartsWith("8") && phone.Length == 11 && phone.Substring(1).All(char.IsDigit));
        }

        // Валидация имени/фамилии (только буквы)
        private bool IsValidName(string name)
        {
            return name.All(c => char.IsLetter(c) || c == ' ');
        }

        // Валидация адреса (только буквы)
        private bool IsValidAddress(string address)
        {
            return address.All(c => char.IsLetter(c) || c == ' ');
        }

        // Валидация даты (формат: ДД.ММ.ГГГГ)
        private bool IsValidDate(string date)
        {
            DateTime result;
            return DateTime.TryParseExact(date, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out result);
        }

        private void button17_Click(object sender, EventArgs e)//Сброс поиска клиент
        {
            // Очищаем поле поиска и сбрасываем выбранный критерий
            textBox30.Clear();
            comboBox6.SelectedIndex = -1; // Сбрасываем выбор в ComboBox

            try
            {
                // Формируем запрос для получения всех данных из таблицы Клиент
                string query = "SELECT * FROM Клиент";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        DataTable allData = new DataTable();
                        adapter.Fill(allData);

                        // Обновляем DataGridView с полными данными
                        dataGridView1.DataSource = allData;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сбросе фильтра: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            // Очищаем поле поиска и сбрасываем выбранный критерий
            textBox31.Clear();
            comboBox7.SelectedIndex = -1; // Сбрасываем выбор в ComboBox

            try
            {
                // Формируем запрос для получения всех данных из таблицы Клиент
                string query = "SELECT * FROM Договор";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        DataTable allData = new DataTable();
                        adapter.Fill(allData);

                        // Обновляем DataGridView с полными данными
                        dataGridView2.DataSource = allData;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сбросе фильтра: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            // Очищаем поле поиска и сбрасываем выбранный критерий
            textBox32.Clear();
            comboBox8.SelectedIndex = -1; // Сбрасываем выбор в ComboBox

            try
            {
                // Формируем запрос для получения всех данных из таблицы Клиент
                string query = "SELECT * FROM Кредит";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        DataTable allData = new DataTable();
                        adapter.Fill(allData);

                        // Обновляем DataGridView с полными данными
                        dataGridView3.DataSource = allData;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сбросе фильтра: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            // Очищаем поле поиска и сбрасываем выбранный критерий
            textBox33.Clear();
            comboBox14.SelectedIndex = -1; // Сбрасываем выбор в ComboBox

            try
            {
                // Формируем запрос для получения всех данных из таблицы Клиент
                string query = "SELECT * FROM Платеж";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        DataTable allData = new DataTable();
                        adapter.Fill(allData);

                        // Обновляем DataGridView с полными данными
                        dataGridView4.DataSource = allData;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сбросе фильтра: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadComboBox1FromBank()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    
                    string query = "SELECT DISTINCT Название FROM Банк";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        comboBox1.Items.Clear(); 
                        while (reader.Read())
                        {
                            comboBox1.Items.Add(reader.GetString(0));
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке данных в ComboBox1: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadComboBox2FromBank()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Запрос для получения данных из столбца "ФИО" таблицы "Сотрудник"
                    string query = "SELECT DISTINCT Название FROM [Цели кредита]";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        comboBox2.Items.Clear();
                        comboBox11.Items.Clear();// Очистить ComboBox перед загрузкой данных
                        while (reader.Read())
                        {
                            
                            comboBox2.Items.Add(reader.GetString(0));
                            comboBox11.Items.Add(reader.GetString(1));
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке данных в ComboBox1: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
       



    }
}
    

