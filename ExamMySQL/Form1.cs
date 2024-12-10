using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ExamMySQL
{
    public partial class Form1 : Form
    {
        string _server = "localhost"; //DB 서버 주소, 로컬일 경우 localhost
        int _port = 3306; //DB 서버 포트
        string _database = "new_schema"; //DB 이름
        string _id = "root"; //계정 아이디
        string _pw = "1234"; //계정 비밀번호
        string _connectionAddress = "";

        public Form1()
        {
            InitializeComponent();
            _connectionAddress = string.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4}", _server, _port, _database, _id, _pw);
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            textBoxName.Text = "";
            textBoxPhone.Text = "";
        }

        private void buttonInsert_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection mysql = new MySqlConnection(_connectionAddress))
                {
                    mysql.Open();
                    string insertQuery = string.Format("INSERT INTO accounts_table (name, phone) VALUES ('{0}', '{1}');", textBoxName.Text, textBoxPhone.Text);

                    MySqlCommand command = new MySqlCommand(insertQuery, mysql);
                    if (command.ExecuteNonQuery() != 1)
                        MessageBox.Show("Failed to insert data.");

                    textBoxName.Text = "";
                    textBoxPhone.Text = "";

                    selectTable();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection mysql = new MySqlConnection(_connectionAddress))
                {
                    mysql.Open();
                    int pos = listViewPhoneBook.SelectedItems[0].Index;
                    int index = Convert.ToInt32(listViewPhoneBook.Items[pos].Text);
                    string updateQuery = string.Format("UPDATE accounts_table SET name = '{1}', phone = '{2}' WHERE id={0};", index, textBoxName.Text, textBoxPhone.Text);

                    MySqlCommand command = new MySqlCommand(updateQuery, mysql);
                    if (command.ExecuteNonQuery() != 1)
                        MessageBox.Show("Failed to delete data.");

                    textBoxName.Text = "";
                    textBoxPhone.Text = "";

                    selectTable();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection mysql = new MySqlConnection(_connectionAddress))
                {
                    mysql.Open();
                    int pos = listViewPhoneBook.SelectedItems[0].Index;
                    int index = Convert.ToInt32(listViewPhoneBook.Items[pos].Text);
                    string deleteQuery = string.Format("DELETE FROM accounts_table WHERE id={0};", index);

                    MySqlCommand command = new MySqlCommand(deleteQuery, mysql);
                    if (command.ExecuteNonQuery() != 1)
                        MessageBox.Show("Failed to delete data.");

                    textBoxName.Text = "";
                    textBoxPhone.Text = "";

                    selectTable();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            selectTable();
        }

        private void selectTable()
        {
            _connectionAddress = string.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4}", _server, _port, _database, _id, _pw);

            try
            {
                using (MySqlConnection mysql = new MySqlConnection(_connectionAddress))
                {
                    mysql.Open();
                    string selectQuery = string.Format("SELECT * FROM accounts_table");

                    MySqlCommand command = new MySqlCommand(selectQuery, mysql);
                    MySqlDataReader table = command.ExecuteReader();

                    listViewPhoneBook.Items.Clear();

                    while (table.Read())
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = table["id"].ToString();
                        item.SubItems.Add(table["name"].ToString());
                        item.SubItems.Add(table["phone"].ToString());

                        listViewPhoneBook.Items.Add(item);
                    }

                    table.Close();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
    }
}
