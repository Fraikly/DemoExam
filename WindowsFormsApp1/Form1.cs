using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        NpgsqlDataAdapter adapter;
        DataTable table = new DataTable();

        string[] comboBoxTables = {
            "Роли",
            "Пользователи",
            "Типы товаров",
            "Товары",
            "orders",
            "order_products"
        };

        string[] tables = {
            "roles",
            "users",
            "product_types",
            "products",
            "orders",
            "order_products"
        };

        ToolStripMenuItem deleteMenuItem = new ToolStripMenuItem("Удалить выбранное");

        public Form1()
        {
            InitializeComponent();

            comboBox1.Items.AddRange(comboBoxTables);
            comboBox1.SelectedIndex = 0;

            contextMenuStrip1.Items.AddRange(new[] { deleteMenuItem });
            dataGridView1.ContextMenuStrip = contextMenuStrip1;

            deleteMenuItem.Click += deleteMenuItem_Click;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int tableNumber = comboBox1.SelectedIndex;

            string connString = "Host=127.0.0.1;Username=postgres;Password=root;Database=jewlery_shop";
            NpgsqlConnection conn = new NpgsqlConnection(connString);
            conn.Open();

            string cmnd = $"SELECT * from {tables[tableNumber]}";
            adapter = new NpgsqlDataAdapter(cmnd, conn);

            DataSet ds = new DataSet();
            ds.Reset();
            adapter.Fill(ds);

            table = ds.Tables[0];
            dataGridView1.DataSource = table;

            conn.Close();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            this.saveData();
        }

        private void deleteMenuItem_Click(object sender, EventArgs e)
        {
            int row = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            dataGridView1.Rows.RemoveAt(row);

            this.saveData();
        }

        private void saveData()
        {
            try
            {
                NpgsqlCommandBuilder scb = new NpgsqlCommandBuilder(adapter);
                adapter.Update(table);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
