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

        public Form3()
        {
            InitializeFormComponents();
            InitializeComponent();
            ConnectToDatabase();
            LoadClientData();
            LoadDogovorData();
            LoadKreditData();
            LoadPlatechData();
            LoadPersonalData();
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
        }

        private void ConnectToDatabase()
        {
            string dbPath = "C:\\Users\\KyCyMaMa\\Desktop\\Bank.db";
            connection = new SQLiteConnection($"Data Source={dbPath}");
            connection.Open();
        }

        private void LoadClientData(string columnName = "Фамилия")
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

                        // Заполняем comboBox1 значениями из выбранного столбца
                        comboBox1.Items.Clear();
                        if (clientsTable.Columns.Contains(columnName))
                        {
                            foreach (DataRow row in clientsTable.Rows)
                            {
                                if (row[columnName] != DBNull.Value)
                                    comboBox1.Items.Add(row[columnName].ToString());
                            }
                        }
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
                    }
                }
            }
        }

        private void LoadPersonalData()
        {
            string dbPath = "C:\\Users\\KyCyMaMa\\Desktop\\Bank.db";
            string connectionString = $"Data Source={dbPath}";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Сотрудник";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        DataTable clientsTable = new DataTable();
                        adapter.Fill(clientsTable);
                        dataGridView4.DataSource = clientsTable;
                    }
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Получаем выбранное значение
            string selectedColumn = comboBox2.SelectedItem.ToString();

            // Загружаем данные из выбранного столбца
            LoadClientData(selectedColumn);
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            // Добавляем типы данных
            comboBox2.Items.Add("ID");
            comboBox2.Items.Add("Фамилия");
            comboBox2.Items.Add("Имя");
            comboBox2.Items.Add("Отчество");
            comboBox2.Items.Add("ИНН");
            comboBox2.Items.Add("Дата_рождения");
            comboBox2.Items.Add("Серия_паспорта");
            comboBox2.Items.Add("Номер_паспорта");
            comboBox2.Items.Add("Гражданство");
            comboBox2.Items.Add("Адрес");
            comboBox2.Items.Add("Номер_телефона");
            comboBox2.Items.Add("Email");
            comboBox2.Items.Add("Дата_регистрации");
            comboBox2.SelectedIndex = 0; // Устанавливаем начальное значение
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Получаем значения из полей ввода
            string id = textBox1.Text;
            string surname = textBox2.Text;
            string name = textBox3.Text;
            string lastName = textBox4.Text;
            string INN = textBox5.Text;
            string data_roda = textBox6.Text;
            string seria_pasporta = textBox7.Text;
            string number_pasport = textBox8.Text;
            string gr = textBox9.Text;
            string address = textBox10.Text;
            string number_phone = textBox11.Text;
            string email = textBox12.Text;
            string data_reg = textBox13.Text;



            // Валидация данных
            try
            {
                // Вставляем данные в базу данных
                InsertKlientDataToDatabase(id, surname, name, lastName, INN, data_roda, seria_pasporta, number_pasport, gr, address, number_phone, email, data_reg);
                LoadClientData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            // Получаем выбранную строку в DataGridView
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите клиента для обновления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Получаем выбранную строку в DataGridView
            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

            // Обновляем данные в базе данных
            try
            {
                UpdateKlientDataInDatabase(
                    selectedRow.Cells[0].Value.ToString(),
                    textBox2.Text,
                    textBox3.Text,
                    textBox4.Text,
                    textBox5.Text,
                    textBox6.Text,
                    textBox7.Text,
                    textBox8.Text,
                    textBox9.Text,
                    textBox10.Text,
                    textBox11.Text,
                    textBox12.Text,
                    textBox13.Text
                );

                LoadClientData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void InsertKlientDataToDatabase(string id, string surname, string name, string lastName, string INN, string data_roda, string seria_pasporta, string number_pasporta, string gr, string address, string number_phone, string email, string data_reg)
        {
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = "INSERT INTO Клиент (ID, Фамилия, Имя, Отчество, [ИНН], [Дата_рождения], [Серия_паспорта], [Номер_паспорта], Гражданство, Адрес, Номер_телефона, Email, Дата_регистрации) VALUES (@id, @surname, @name, @lastName, @INN, @data_roda, @seria_pasporta, @number_pasporta, @gr, @address, @number_phone, @email, @data_reg)";
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@surname", surname);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@lastName", lastName);
                command.Parameters.AddWithValue("@INN", INN);
                command.Parameters.AddWithValue("@data_roda", data_roda);
                command.Parameters.AddWithValue("@seria_pasporta", seria_pasporta);
                command.Parameters.AddWithValue("@number_pasporta", number_pasporta);
                command.Parameters.AddWithValue("@gr", gr);
                command.Parameters.AddWithValue("@address", address);
                command.Parameters.AddWithValue("@number_phone", number_phone);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@data_reg", data_reg);
                command.ExecuteNonQuery();
            }
        }

        private void UpdateKlientDataInDatabase(string id, string surname, string name, string lastName, string INN, string data_roda, string seria_pasporta, string number_pasporta, string gr, string address, string number_phone, string email, string data_reg)
        {
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = "UPDATE Клиент SET Фамилия = @surname, Имя = @name, Отчество = @lastName, ИНН = @INN, Дата_рождения = @data_roda, Серия_паспорта = @seria_pasporta, Номер_паспорта = @number_pasport, Гражданство = @gr, Адрес = @address, Номер_телефона = @number_phone, Email = @email, Дата_регистрации = @data_reg WHERE ID = @id";
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@surname", surname);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@lastName", lastName);
                command.Parameters.AddWithValue("@INN", INN);
                command.Parameters.AddWithValue("@data_roda", data_roda);
                command.Parameters.AddWithValue("@seria_pasporta", seria_pasporta);
                command.Parameters.AddWithValue("@number_pasporta", number_pasporta);
                command.Parameters.AddWithValue("@gr", gr);
                command.Parameters.AddWithValue("@address", address);
                command.Parameters.AddWithValue("@number_phone", number_phone);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@data_reg", data_reg);
                command.ExecuteNonQuery();
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                textBox2.Text = selectedRow.Cells["Фамилия"].Value.ToString();
                textBox3.Text = selectedRow.Cells["Имя"].Value.ToString();
                textBox4.Text = selectedRow.Cells["Отчество"].Value.ToString();
                textBox5.Text = selectedRow.Cells["ИНН"].Value.ToString();
                textBox6.Text = selectedRow.Cells["Дата_рождения"].Value.ToString();
                textBox7.Text = selectedRow.Cells["Серия_паспорта"].Value.ToString();
                textBox8.Text = selectedRow.Cells["Номер_паспорта"].Value.ToString();
                textBox9.Text = selectedRow.Cells["Гражданство"].Value.ToString();
                textBox10.Text = selectedRow.Cells["Адрес"].Value.ToString();
                textBox11.Text = selectedRow.Cells["Номер_телефона"].Value.ToString();
                textBox12.Text = selectedRow.Cells["Email"].Value.ToString();
                textBox13.Text = selectedRow.Cells["Дата_регистрации"].Value.ToString();
            }
        }

        private void DeleteKlientDataFromDatabase(string id)
        {
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = "DELETE FROM Клиент WHERE id = @id";
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

     

        // Применение обработчика к нужным TextBox


        private void button5_Click(object sender, EventArgs e)
        {
            // Получаем значения из полей ввода
            string id = textBox14.Text;
            string FIO = textBox15.Text;
            string INN_client = textBox16.Text;
            string seria_pasporta = textBox17.Text;
            string number_pasporta = textBox18.Text;
            string address = textBox19.Text;
            string name_bank = textBox20.Text;
            string INN_bank = textBox21.Text;
            string summa = textBox22.Text;
            string srok = textBox23.Text;
            string proc = textBox24.Text;
            string sposob = textBox25.Text;
            string data_reg = textBox26.Text;

            // Вставляем данные в базу данных
            InsertDogovorDataToDatabase(id, FIO, INN_client, seria_pasporta, number_pasporta, address, name_bank, INN_bank, summa, srok, proc, sposob, data_reg);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Получаем выбранную строку в DataGridView
            DataGridViewRow selectedRow = dataGridView2.SelectedRows[0];

            // Обновляем данные в базе данных
            UpdateDogovorDataInDatabase(
                selectedRow.Cells[0].Value.ToString(),
                textBox15.Text,
                textBox16.Text,
                textBox17.Text,
                textBox18.Text,
                textBox19.Text,
                textBox20.Text,
                textBox21.Text,
                textBox22.Text,
                textBox23.Text,
                textBox24.Text,
                textBox25.Text,
                textBox26.Text
            );

            // Обновляем DataGridView
            LoadDogovorData();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            // Получаем выбранную строку в DataGridView
            if (dataGridView2.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите клиента для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = dataGridView2.SelectedRows[0];

            // Удаляем данные из базы данных
            try
            {
                DeleteDogovorDataFromDatabase(selectedRow.Cells[0].Value.ToString());
                LoadDogovorData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InsertDogovorDataToDatabase( string id,string FIO,string INN_client,string seria_pasporta,string number_pasporta,string address,string name_bank,string INN_bank,string summa,string srok,string proc,string sposob,string data_reg)
        {
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = "INSERT INTO Договор (ID, ФИО_Клиента, ИНН_Клиента, Серия_паспорта, Номер_паспорта, Адрес, Название_банка, ИНН_банка, Сумма_кредита, Срок_кредита, Процентная_ставка, Способ_погашения, Дата_подписания) VALUES (@id, @FIO, @INN_client, @seria_pasporta, @number_pasporta, @address, @name_bank, @INN_bank, @summa, @srok, @proc, @sposob, @data_reg)";
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@FIO", FIO);
                command.Parameters.AddWithValue("@INN_client", INN_client);
                command.Parameters.AddWithValue("@seria_pasporta", seria_pasporta);
                command.Parameters.AddWithValue("@number_pasporta", number_pasporta);
                command.Parameters.AddWithValue("@address", address);
                command.Parameters.AddWithValue("@name_bank", name_bank);
                command.Parameters.AddWithValue("@INN_bank", INN_bank);
                command.Parameters.AddWithValue("@summa", summa);
                command.Parameters.AddWithValue("@srok", srok);
                command.Parameters.AddWithValue("@proc", proc);
                command.Parameters.AddWithValue("@sposob", sposob);
                command.Parameters.AddWithValue("@data_reg", data_reg);
                command.ExecuteNonQuery();
            }
        }

        private void UpdateDogovorDataInDatabase(string id, string FIO, string INN_client, string seria_pasporta, string number_pasporta, string address, string name_bank, string INN_bank, string summa, string srok, string proc, string sposob, string data_reg)
        {
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = "UPDATE Договор SET id = @id, FIO = @FIO, INN_client = @INN_client, seria_pasporta = @seria_pasporta, number_pasporta = @number_pasporta, address = @address, name_bank = @name_bank, INN_bank = @INN_bank, summa = @summa, srok = @srok, proc = @proc, sposob = @sposob, data_reg = @data_reg WHERE ID = @id";
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@FIO", FIO);
                command.Parameters.AddWithValue("@INN_client", INN_client);
                command.Parameters.AddWithValue("@seria_pasporta", seria_pasporta);
                command.Parameters.AddWithValue("@number_pasporta", number_pasporta);
                command.Parameters.AddWithValue("@address", address);
                command.Parameters.AddWithValue("@name_bank", name_bank);
                command.Parameters.AddWithValue("@INN_bank", INN_bank);
                command.Parameters.AddWithValue("@summa", summa);
                command.Parameters.AddWithValue("@srok", srok);
                command.Parameters.AddWithValue("@proc", proc);
                command.Parameters.AddWithValue("@sposob", sposob);
                command.Parameters.AddWithValue("@data_reg", data_reg);
                command.ExecuteNonQuery();
            }
        }

        private void DeleteDogovorDataFromDatabase(string id)
        {
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = "DELETE FROM Договор WHERE ID = @id";
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {

            // Получаем значения из полей ввода
            string id_credit = textBox27.Text;
            string id_client = textBox28.Text;
            string id_dogovor = textBox29.Text;
            string summa = textBox30.Text;
            string cell_credit = textBox31.Text;
            string vid_credita = textBox32.Text;
            string srok = textBox33.Text;
            string status = textBox34.Text;
            string data_vidachi = textBox35.Text;
            string valuta = textBox36.Text;
            string sposob = textBox37.Text;


            // Вставляем данные в базу данных
            InsertKreditDataToDatabase(id_credit, id_client, id_dogovor, summa, cell_credit, vid_credita, srok, status, data_vidachi,valuta,sposob);

            // Обновляем DataGridView
            LoadKreditData();
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            // Получаем выбранную строку в DataGridView
            DataGridViewRow selectedRow = dataGridView3.SelectedRows[0];

            // Обновляем данные в базе данных
            UpdateKreditDataInDatabase(
                selectedRow.Cells[0].Value.ToString(),
                textBox28.Text,
                textBox29.Text,
                textBox30.Text,
                textBox31.Text,
                textBox32.Text,
                textBox33.Text,
                textBox34.Text,
                textBox35.Text,
                textBox36.Text,
                textBox37.Text
            );

            // Обновляем DataGridView
            LoadKreditData();
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

        private void InsertKreditDataToDatabase(string id_credit, string id_client, string id_dogovor, string summa, string cell_credit, string vid_credita, string srok, string status, string data_vidachi, string valuta, string sposob)
        {
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = "INSERT INTO Кредит (ID_Кредита, ID_Клиента, ID_Договора, Сумма_кредита, Цель_кредита, Вид_кредита, Срок_кредита, Статус_кредита, Дата_выдачи_кредита, Валюта_кредита, Способ_погашения) VALUES (@id_credit, @id_client, @id_dogovor, @summa, @cell_credit, @vid_credita, @srok, @status, @data_vidachi, @valuta, @sposob)";
                command.Parameters.AddWithValue("@id_credit", id_credit);
                command.Parameters.AddWithValue("@id_client", id_client);
                command.Parameters.AddWithValue("@id_dogovor", id_dogovor);
                command.Parameters.AddWithValue("@summa", summa);
                command.Parameters.AddWithValue("@cell_credit", cell_credit);
                command.Parameters.AddWithValue("@vid_credita", vid_credita);
                command.Parameters.AddWithValue("@srok", srok);
                command.Parameters.AddWithValue("@status", status);
                command.Parameters.AddWithValue("@data_vidachi", data_vidachi);
                command.Parameters.AddWithValue("@valuta", valuta);
                command.Parameters.AddWithValue("@sposob", sposob);
                command.ExecuteNonQuery();
            }
        }

        private void UpdateKreditDataInDatabase(string id_credit, string id_client, string id_dogovor, string summa, string cell_credit, string vid_credita, string srok, string status, string data_vidachi, string valuta, string sposob)
        {
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = "UPDATE Кредит SET id_credit = @id_credit, id_client = @id_client, id_dogovor = @id_dogovor, summa = @summa, cell_credit = @cell_credit, vid_credita = @vid_credita, srok = @srok, status = @status, data_vidachi = @data_vidachi, valuta = @valuta, sposob = @sposob WHERE id = @id";
                command.Parameters.AddWithValue("@id_credit", id_credit);
                command.Parameters.AddWithValue("@id_client", id_client);
                command.Parameters.AddWithValue("@id_dogovor", id_dogovor);
                command.Parameters.AddWithValue("@summa", summa);
                command.Parameters.AddWithValue("@cell_credit", cell_credit);
                command.Parameters.AddWithValue("@vid_credita", vid_credita);
                command.Parameters.AddWithValue("@srok", srok);
                command.Parameters.AddWithValue("@status", status);
                command.Parameters.AddWithValue("@data_vidachi", data_vidachi);
                command.Parameters.AddWithValue("@valuta", valuta);
                command.Parameters.AddWithValue("@sposob", sposob);
                command.ExecuteNonQuery();
            }
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

        private void button10_Click_1(object sender, EventArgs e)
        {
            // Получаем значения из полей ввода
            string id_sotrudnik = textBox38.Text;
            string familia = textBox39.Text;
            string name = textBox40.Text;
            string lastname = textBox41.Text;
            string data_roda = textBox42.Text;
            string seria_pasport = textBox43.Text;
            string namber_pasport = textBox44.Text;
            string grash = textBox45.Text;
            string address = textBox46.Text;
            string number_phone = textBox47.Text;
            string email = textBox48.Text;
            string dolg = textBox49.Text;
            string data_rabota = textBox50.Text;
            string zp = textBox51.Text;


            // Вставляем данные в базу данных
            InsertPersonalDataToDatabase(id_sotrudnik, familia, name, lastname, data_roda, seria_pasport, namber_pasport, grash, address, number_phone, email, dolg, data_rabota, zp);
        }

        private void button11_Click_1(object sender, EventArgs e)
        {
            // Получаем выбранную строку в DataGridView
            DataGridViewRow selectedRow = dataGridView4.SelectedRows[0];

            // Обновляем данные в базе данных
            UpdatePersonalDataInDatabase(
                selectedRow.Cells[0].Value.ToString(),
                textBox39.Text,
                textBox40.Text,
                textBox41.Text,
                textBox42.Text,
                textBox43.Text,
                textBox44.Text,
                textBox45.Text,
                textBox46.Text,
                textBox47.Text,
                textBox48.Text,
                textBox49.Text,
                textBox50.Text,
                textBox51.Text
            );

            // Обновляем DataGridView
            LoadPersonalData();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            // Получаем выбранную строку в DataGridView
            if (dataGridView4.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите сотрудника для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = dataGridView4.SelectedRows[0];

            // Удаляем данные из базы данных
            try
            {
                DeletePersonalDataFromDatabase(selectedRow.Cells[0].Value.ToString());
                LoadPersonalData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InsertPersonalDataToDatabase(string id_sotrudnik,string familia,string name,string lastname,string data_roda,string seria_pasport,string namber_pasport,string grash,string address,string number_phone,string email,string dolg,string data_rabota,string zp)
        {
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = "INSERT INTO Сотрудник (ID, Фамилия, Имя, Отчество, Дата_рождения, Серия_паспорта, Номер_паспорта, Гражданство, Адрес, Номер_телефона, Электронная_почта, Должность, Дата_найма, Заработная_плата) VALUES (@id_sotrudnik, @familia, @name, @lastname, @data_roda, @seria_pasport, @namber_pasport, @grash, @address, @number_phone, @email, @dolg, @data_rabota, @zp)";
                command.Parameters.AddWithValue("@id_sotrudnik", id_sotrudnik);
                command.Parameters.AddWithValue("@familia", familia);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@lastname", lastname);
                command.Parameters.AddWithValue("@data_roda", data_roda);
                command.Parameters.AddWithValue("@seria_pasport", seria_pasport);
                command.Parameters.AddWithValue("@namber_pasport", namber_pasport);
                command.Parameters.AddWithValue("@grash", grash);
                command.Parameters.AddWithValue("@address", address);
                command.Parameters.AddWithValue("@number_phone", number_phone);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@dolg", dolg);
                command.Parameters.AddWithValue("@data_rabota", data_rabota);
                command.Parameters.AddWithValue("@zp", zp);
                command.ExecuteNonQuery();
            }
        }

        private void UpdatePersonalDataInDatabase(string id_sotrudnik, string familia, string name, string lastname, string data_roda, string seria_pasport, string namber_pasport, string grash, string address, string number_phone, string email, string dolg, string data_rabota, string zp)
        {
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = "UPDATE Сотрудник SET ID = @id_sotrudnika, familia = @familia, name = @name, lastname = @lastname, data_roda = @data_roda, seria_pasport = @seria_pasport, namber_pasport = @namber_pasport, grash = @grash, address = @address, number_phone = @number_phone, email = @email, dolg = @dolg, data_rabota = @data_rabota, zp = @zp WHERE ID = @id";
                command.Parameters.AddWithValue("@id_sotrudnik", id_sotrudnik);
                command.Parameters.AddWithValue("@familia", familia);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@lastname", lastname);
                command.Parameters.AddWithValue("@data_roda", data_roda);
                command.Parameters.AddWithValue("@seria_pasport", seria_pasport);
                command.Parameters.AddWithValue("@namber_pasport", namber_pasport);
                command.Parameters.AddWithValue("@grash", grash);
                command.Parameters.AddWithValue("@address", address);
                command.Parameters.AddWithValue("@number_phone", number_phone);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@dolg", dolg);
                command.Parameters.AddWithValue("@data_rabota", data_rabota);
                command.Parameters.AddWithValue("@zp", zp);
                command.ExecuteNonQuery();
            }
        }

        private void DeletePersonalDataFromDatabase(string id)
        {
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = "DELETE FROM Сотрудник WHERE ID = @id";
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            // Получаем значения из полей ввода
            string id_plata = textBox52.Text;
            string id_credit = textBox53.Text;
            string data_plata = textBox54.Text;
            string tip_plata = textBox55.Text;
            string vid_plata = textBox56.Text;
            string summa_plata = textBox57.Text;
            string ostatok_dolga = textBox58.Text;



            // Вставляем данные в базу данных
            InsertPlatechDataToDatabase(id_plata, id_credit, data_plata, tip_plata, vid_plata,summa_plata,ostatok_dolga);
        }

        private void button14_Click_1(object sender, EventArgs e)
        {
            // Получаем выбранную строку в DataGridView
            DataGridViewRow selectedRow = dataGridView5.SelectedRows[0];

            // Обновляем данные в базе данных
            UpdatePlatechDataInDatabase(
                selectedRow.Cells[0].Value.ToString(),
                textBox53.Text,
                textBox54.Text,
                textBox55.Text,
                textBox56.Text,
                textBox57.Text,
                textBox58.Text
            );

            // Обновляем DataGridView
            LoadPlatechData();
        }

        private void button15_Click_1(object sender, EventArgs e)
        {
            // Получаем выбранную строку в DataGridView
            if (dataGridView5.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите клиента для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void InsertPlatechDataToDatabase(string id_plata,string id_credit,string data_plata,string tip_plata,string vid_plata,string summa_plata,string ostatok_dolga)
        {
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = "INSERT INTO Платеж (ID_Платежа, ID_Кредита, Дата_платежа, Тип_платежа, Вид_платежа, Сумма_платежа, Остаток_долга) VALUES (@id_plata, @id_credit, @data_plata, @tip_plata, @vid_plata, @summa_plata, @ostatok_dolga)";
                command.Parameters.AddWithValue("@id_plata", id_plata);
                command.Parameters.AddWithValue("@id_credit", id_credit);
                command.Parameters.AddWithValue("@data_plata", data_plata);
                command.Parameters.AddWithValue("@tip_plata", tip_plata);
                command.Parameters.AddWithValue("@vid_plata", vid_plata);
                command.Parameters.AddWithValue("@summa_plata", summa_plata);
                command.Parameters.AddWithValue("@ostatok_dolga", ostatok_dolga);
                command.ExecuteNonQuery();
            }
        }

        private void UpdatePlatechDataInDatabase(string id_plata, string id_credit, string data_plata, string tip_plata, string vid_plata, string summa_plata, string ostatok_dolga)
        {
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = "UPDATE Платеж SET id_plata = @id_plata, id_credit = @id_credit, data_plata = @data_plata, tip_plata = @tip_plata, vid_plata = @vid_plata, summa_plata = @summa_plata, ostatok_dolga = @ostatok_dolga WHERE ID = @id";
                command.Parameters.AddWithValue("@id_plata", id_plata);
                command.Parameters.AddWithValue("@id_credit", id_credit);
                command.Parameters.AddWithValue("@data_plata", data_plata);
                command.Parameters.AddWithValue("@tip_plata", tip_plata);
                command.Parameters.AddWithValue("@vid_plata", vid_plata);
                command.Parameters.AddWithValue("@summa_plata", summa_plata);
                command.Parameters.AddWithValue("@ostatok_dolga", ostatok_dolga);
                command.ExecuteNonQuery();
            }
        }

        private void DeletePlatechDataFromDatabase(string id)
        {
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = "DELETE FROM country WHERE ID = @id";
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

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
            this.Width = 800;
            this.Height = 600;
        }

        private void RefreshMenuItem_Click(object sender, EventArgs e)
        {
            // Логика для кнопки "Обновить"
            LoadClientData();
            LoadDogovorData();
            LoadKreditData();
            LoadPlatechData();
            LoadPersonalData();
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

        private void button16_Click(object sender, EventArgs e)
        {

        }
    }
}
    

