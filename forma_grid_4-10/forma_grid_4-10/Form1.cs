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

namespace forma_grid_4_10
{
    public partial class Form1 : Form
    {
        int broj_sloga;
        SqlConnection veza;
        DataTable dt_predmet, dt_ucenik, dt_ocena, dt_ocena_o;
        public Form1()
        {
            InitializeComponent();
        }

        private void predmet_populate()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM predmet", veza);
            dt_predmet = new DataTable();
            adapter.Fill(dt_predmet);
            comboBox2.DataSource = dt_predmet;
            comboBox2.ValueMember = "id";
            comboBox2.DisplayMember = "naziv";
        }
        private void ucenik_populate()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT id, ime+' '+prezime as ucenik FROM ucenik", veza);
            dt_ucenik = new DataTable();
            adapter.Fill(dt_ucenik);
            comboBox1.DataSource = dt_ucenik;
            comboBox1.ValueMember = "id";
            comboBox1.DisplayMember = "ucenik";
        }
        private void grid_populate()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM ocena ORDER BY id", veza);
            dt_ocena_o = new DataTable();
            adapter.Fill(dt_ocena_o);

            dt_ocena = new DataTable();
            adapter = new SqlDataAdapter("SELECT ocena.id, ime+' '+prezime as ucenik, naziv, ocena FROM ocena JOIN predmet ON predmet.id = predmet_id JOIN ucenik ON ucenik.id = ucenik_id  ORDER BY id", veza);
            adapter.Fill(dt_ocena);

            dataGridView1.DataSource = dt_ocena;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.Columns["id"].Visible = false;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            string CS = "Data Source = SMOKI-PC\\SQLEXPRESS; Initial Catalog = milosp2022; Integrated Security = True";
            veza = new SqlConnection(CS);
            predmet_populate();
            ucenik_populate();
            grid_populate();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                broj_sloga = dataGridView1.CurrentRow.Index;
                if (broj_sloga >= 0 && dataGridView1.RowCount != 0)
                {
                    comboBox1.SelectedValue = dt_ocena_o.Rows[broj_sloga]["ucenik_id"].ToString();
                    comboBox2.SelectedValue = dt_ocena_o.Rows[broj_sloga]["predmet_id"].ToString();
                    textBox1.Text = dt_ocena_o.Rows[broj_sloga]["ocena"].ToString();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string naredba = "INSERT INTO ocena(ucenik_id, predmet_id, ocena) VALUES ('";
            naredba = naredba + comboBox1.SelectedValue.ToString() + "', '";
            naredba = naredba + comboBox2.SelectedValue.ToString() + "', '";
            naredba = naredba + textBox1.Text + "')";
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
            string naredba = "DELETE FROM ocena WHERE id = " + dt_ocena_o.Rows[broj_sloga]["id"].ToString();
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

    }
}
