using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace _4_9_grid
{
    public partial class Form1 : Form
    {
        SqlConnection veza;
        DataTable dt_ocena, dt_ocena_j;
        public Form1()
        {
            InitializeComponent();
        }
        private void ucenik_populate()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT id, ime+' '+prezime as naziv FROM ucenik", veza);
            DataTable dt_ucenik = new DataTable();
            adapter.Fill(dt_ucenik);
            comboBox1.DataSource = dt_ucenik;
            comboBox1.ValueMember = "id";
            comboBox1.DisplayMember = "naziv";
        }
        private void predmet_populate()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM predmet", veza);
            DataTable dt_predmet = new DataTable();
            adapter.Fill(dt_predmet);
            comboBox2.DataSource = dt_predmet;
            comboBox2.ValueMember = "id";
            comboBox2.DisplayMember = "naziv";
        }
        private void grid_populate()
        {
            string tmp = "select ocena.id, ime+' '+prezime as ucenik, naziv as predmet, ocena from ocena join predmet on predmet.id = predmet_id join ucenik on ucenik.id = ucenik_id";
            SqlDataAdapter adapter = new SqlDataAdapter(tmp, veza);
            dt_ocena_j = new DataTable();
            adapter.Fill(dt_ocena_j);
            dataGridView1.DataSource = dt_ocena_j;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.Columns["id"].Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string CS = "Data Source = SMOKI-PC\\SQLEXPRESS; Initial Catalog = milosp2022; Integrated Security = True";
            veza = new SqlConnection(CS);
            ucenik_populate();
            predmet_populate();
            grid_populate();
        }
    }
}
