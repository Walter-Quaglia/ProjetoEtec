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
    public partial class FrmFornecedores : Form
    {
        MySqlConnection conexao;
        MySqlCommand comando;
        String banco = "server=localhost;port=3308;uid=root;pwd=etecjau;database=vendas";
        MySqlDataAdapter adaptardor;
        DataTable datTabela;

        public FrmFornecedores()
        {
            InitializeComponent();
        }

        private void FrmFornecedores_Load(object sender, EventArgs e)
        {
            carregarComboCidades();
            btnConsultar.PerformClick(); 
        }

        private void dgvFornecedores_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtID.Text = dgvFornecedores.CurrentRow.Cells[0].Value.ToString();
            txtRazaoSocial.Text = dgvFornecedores.CurrentRow.Cells[1].Value.ToString();
            txtFantasia.Text = dgvFornecedores.CurrentRow.Cells[2].Value.ToString();
            txtEndereco.Text = dgvFornecedores.CurrentRow.Cells[3].Value.ToString();
            txtBairro.Text = dgvFornecedores.CurrentRow.Cells[4].Value.ToString();
            cboCidade.Text = dgvFornecedores.CurrentRow.Cells[5].Value.ToString();
            txtCnpj.Text = dgvFornecedores.CurrentRow.Cells[6].Value.ToString();
            txtIe.Text = dgvFornecedores.CurrentRow.Cells[7].Value.ToString();
            txtTelefone.Text = dgvFornecedores.CurrentRow.Cells[8].Value.ToString();
            txtContato.Text = dgvFornecedores.CurrentRow.Cells[9].Value.ToString();
            txtEmail.Text = dgvFornecedores.CurrentRow.Cells[10].Value.ToString();
            cboCidade.Text = dgvFornecedores.CurrentRow.Cells[11].Value.ToString();
            txtUF.Text = dgvFornecedores.CurrentRow.Cells[12].Value.ToString();
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                conexao = new MySqlConnection(banco);
                conexao.Open();
                comando = new MySqlCommand("select fornecedores.*, cidades.nome cid_nome, cidades.uf   " +
                                           "from fornecedores " +
                                           "inner join cidades ON (fornecedores.id_cidade = cidades.id)" +
                                           "where razao_social like @razao " +
                                           "order by razao_social, fantasia", conexao);
                comando.Parameters.AddWithValue("razao", txtPesquisa.Text + "%");

                adaptardor = new MySqlDataAdapter(comando);
                adaptardor.Fill(datTabela = new DataTable());
                dgvFornecedores.DataSource = datTabela;

                conexao.Close();
                btnCancelar.PerformClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conexao.Close();
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {

        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Deseja excluir o registro", "Exclusão", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    conexao = new MySqlConnection(banco);
                    conexao.Open();

                    comando = new MySqlCommand("delete from fornecedores where id = @id", conexao);
                    comando.Parameters.AddWithValue("@id", txtID.Text);
                    comando.ExecuteNonQuery();

                    conexao.Close();

                    btnCancelar.PerformClick();
                    btnConsultar.PerformClick();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conexao.Close();
            }
        }



        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            try
            {
                conexao = new MySqlConnection(banco);
                conexao.Open();
                comando = new MySqlCommand("update fornecedores set razao_social = @razao_social, " +
                                                                   "fantasia = @fantasia, " +
                                                                   "endereco = @endereco, " +
                                                                   "bairro = @bairro, " +
                                                                   "id_cidade = @id_cidade, " +
                                                                   "cnpj = @cnpj, " +
                                                                   "ie = @ie, " +
                                                                   "fone = @fone, " +
                                                                   "contato = @contato, " +
                                                                   "email = @email " +
                                                                   "where id=@id ", conexao);
                comando.Parameters.AddWithValue("@id", txtID.Text);
                comando.Parameters.AddWithValue("@razao_social", txtRazaoSocial.Text);
                comando.Parameters.AddWithValue("@fantasia", txtFantasia.Text);
                comando.Parameters.AddWithValue("@endereco", txtEndereco.Text);
                comando.Parameters.AddWithValue("@bairro", txtBairro.Text);
                comando.Parameters.AddWithValue("@id_cidade", cboCidade.SelectedValue);
                comando.Parameters.AddWithValue("@cnpj", txtCnpj.Text);
                comando.Parameters.AddWithValue("@ie", txtIe.Text);
                comando.Parameters.AddWithValue("@fone", txtTelefone.Text);
                comando.Parameters.AddWithValue("@contato", txtContato.Text);
                comando.Parameters.AddWithValue("@email", txtEmail.Text);
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

        private void btnIncluir_Click(object sender, EventArgs e)
        {
            try
            {
                conexao = new MySqlConnection(banco);
                conexao.Open();
                comando = new MySqlCommand("insert into fornecedores " +
                                           "(razao_social, " +
                                           "fantasia, " +
                                           "endereco, " +
                                           "bairro, " +
                                           "id_cidade, " +
                                           "cnpj, " +
                                           "ie, " +
                                           "fone, " +
                                           "contato, " +
                                           "email )" +
                                           "values" +
                                           "(@razao_social, " +
                                           "@fantasia, " +
                                           "@endereco, " +
                                           "@bairro, " +
                                           "@id_cidade, " +
                                           "@cnpj, " +
                                           "@ie, " +
                                           "@fone, " +
                                           "@contato, " +
                                           "@email )", conexao);

                comando.Parameters.AddWithValue("@razao_social", txtRazaoSocial.Text);
                comando.Parameters.AddWithValue("@fantasia", txtFantasia.Text);
                comando.Parameters.AddWithValue("@endereco", txtEndereco.Text);
                comando.Parameters.AddWithValue("@bairro", txtBairro.Text);
                comando.Parameters.AddWithValue("@id_cidade", cboCidade.SelectedValue);
                comando.Parameters.AddWithValue("@cnpj", txtCnpj.Text);
                comando.Parameters.AddWithValue("@ie", txtIe.Text);
                comando.Parameters.AddWithValue("@fone", txtTelefone.Text);
                comando.Parameters.AddWithValue("@contato", txtContato.Text);
                comando.Parameters.AddWithValue("@email", txtEmail.Text);
                comando.ExecuteNonQuery();

                conexao.Close();
                
                btnCancelar.PerformClick();
                btnConsultar.PerformClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conexao.Close();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            txtID.Clear();
            txtRazaoSocial.Clear();
            txtFantasia.Clear();
            txtEndereco.Clear();
            txtBairro.Clear();
            txtPesquisa.Clear();
            cboCidade.Text = "";
            txtUF.Clear();
            txtIe.Clear();
            txtCnpj.Clear();
            txtEmail.Clear();
            txtTelefone.Clear();
            txtContato.Clear();

            txtRazaoSocial.Focus();
        }

        private void carregarComboCidades()
        {
            try
            {

                conexao = new MySqlConnection(banco);
                conexao.Open();
                comando = new MySqlCommand("SELECT * " +
                                           "FROM cidades " +
                                           "order by nome", conexao);

                adaptardor = new MySqlDataAdapter(comando);
                adaptardor.Fill(datTabela = new DataTable());
                cboCidade.DataSource = datTabela;
                cboCidade.ValueMember = "id";
                cboCidade.DisplayMember = "nome";
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

        private void cboCidade_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboCidade.SelectedIndex != -1)
            {
                DataRowView reg = (DataRowView)cboCidade.SelectedItem;
                txtUF.Text = reg["uf"].ToString();
            }
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            Close();
        }

        
    }
}
