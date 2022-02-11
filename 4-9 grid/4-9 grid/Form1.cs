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
        int broj;
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
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM ocena ORDER BY id", veza);
            dt_ocena = new DataTable();
            adapter.Fill(dt_ocena);

            string tmp = "select ocena.id, ime+' '+prezime as ucenik, naziv as predmet, ocena from ocena join predmet on predmet.id = predmet_id join ucenik on ucenik.id = ucenik_id ORDER BY id";
            adapter = new SqlDataAdapter(tmp, veza);
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                broj = dataGridView1.CurrentRow.Index;
                comboBox1.SelectedValue = dt_ocena.Rows[broj]["ucenik_id"].ToString();
                comboBox2.SelectedValue = dt_ocena.Rows[broj]["predmet_id"].ToString();
                textBox1.Text = dt_ocena.Rows[broj]["ocena"].ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string naredba = "INSERT INTO ocena (ucenik_id, predmet_id, ocena) VALUES ('";
            naredba = naredba + comboBox1.SelectedValue.ToString() + "', '";
            naredba = naredba + comboBox2.SelectedValue.ToString() + "', '";
            naredba = naredba + textBox1.Text + "' )";
            SqlCommand komanda = new SqlCommand(naredba, veza);
            try
            {
                veza.Open();
                komanda.ExecuteNonQuery();
                veza.Close();
                grid_populate();
            }
            catch (Exception Greska)
            {
                MessageBox.Show(Greska.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string naredba = "DELETE FROM ocena WHERE id = " + dt_ocena.Rows[broj]["id"].ToString();
            SqlCommand komanda = new SqlCommand(naredba, veza);
            try
            {
                veza.Open();
                komanda.ExecuteNonQuery();
                veza.Close();
                grid_populate();
            }
            catch (Exception Greska)
            {
                MessageBox.Show(Greska.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string naredba = "UPDATE ocena SET ucenik_id='" + comboBox1.SelectedValue.ToString();
            naredba = naredba + "', predmet_id='" + comboBox2.SelectedValue.ToString();
            naredba = naredba + "', ocena = '" + textBox1.Text + "' WHERE id = '";
            naredba = naredba + dt_ocena.Rows[broj]["id"].ToString() + "'";
            SqlCommand komanda = new SqlCommand(naredba.ToString(), veza);
            try
            {
                veza.Open();
                komanda.ExecuteNonQuery();
                veza.Close();
                grid_populate();
            }
            catch (Exception Greska)
            {
                MessageBox.Show(Greska.Message);
            }

        }
    }
}
