using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace exer_Menu_20930
{
    public partial class FrmCategorias : Form
    {
        MySqlConnection conexao;
        MySqlCommand comando;
        String banco = "server=localhost;port=3308;uid=root;pwd=etecjau;database=vendas";
        MySqlDataAdapter adaptardor;
        DataTable datTabela;

        public FrmCategorias()
        {
            InitializeComponent();
        }

        private void FrmCategorias_Load(object sender, EventArgs e)
        {
            btnConsultar.PerformClick();
        }

        private void dgvCategorias_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtID.Text = dgvCategorias.CurrentRow.Cells[0].Value.ToString();
            txtDescricao.Text = dgvCategorias.CurrentRow.Cells[1].Value.ToString();
        }

        private void btnInserir_Click(object sender, EventArgs e)
        {
            try
            {
                conexao = new MySqlConnection(banco);
                conexao.Open();
                comando = new MySqlCommand("insert into categorias " +
                                           "(descricao)" +
                                           "values" +
                                           "(@descricao)", conexao);
                comando.Parameters.AddWithValue("@descricao", txtDescricao.Text);
                comando.ExecuteNonQuery();

                btnCancelar.PerformClick();
                btnConsultar.PerformClick();
                txtDescricao.Focus();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conexao.Close();
            }
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                conexao = new MySqlConnection(banco);
                conexao.Open();

                comando = new MySqlCommand("select * " +
                                           "from categorias " +
                                           "where descricao like @descricao " +
                                           "order by descricao", conexao);
                comando.Parameters.AddWithValue("@descricao", txtPesquisa.Text + "%");
                adaptardor = new MySqlDataAdapter(comando);
                adaptardor.Fill(datTabela = new DataTable());
                dgvCategorias.DataSource = datTabela;

                btnCancelar.PerformClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conexao.Close();
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Deseja excluir o registro", "Exclusão", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    conexao = new MySqlConnection(banco);
                    conexao.Open();
                    comando = new MySqlCommand("delete from categorias where id=@id", conexao);
                    comando.Parameters.AddWithValue("@id", txtID.Text);
                    comando.ExecuteNonQuery();

                    btnCancelar.PerformClick();
                    btnConsultar.PerformClick();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conexao.Close();
            }
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            try
            {
                conexao = new MySqlConnection(banco);
                conexao.Open();
                comando = new MySqlCommand("update categorias set descricao=@descricao where id=@id", conexao);
                comando.Parameters.AddWithValue("@id", txtID.Text);
                comando.Parameters.AddWithValue("@descricao", txtDescricao.Text);
                comando.ExecuteNonQuery();

                btnConsultar.PerformClick();
                btnCancelar.PerformClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conexao.Close();
            }

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            txtID.Clear();
            txtDescricao.Clear();

            txtDescricao.Focus();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            Close();
        }
        
    }
}