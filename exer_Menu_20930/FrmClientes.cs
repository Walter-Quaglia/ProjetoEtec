using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace exer_Menu_20930
{
    public partial class FrmClientes : Form
    {
        MySqlConnection conexao;
        MySqlCommand comando;
        String banco = "server=localhost;port=3308;uid=root;pwd=etecjau;database=vendas";
        MySqlDataAdapter adaptardor;
        DataTable datTabela;

        public FrmClientes()
        {
            InitializeComponent();
        }

        private void FrmClientes_Load(object sender, EventArgs e)
        {
            try
            {
                carregarComboCidades();
                btnConsultar.PerformClick();
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

        private void dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtID.Text = dgvClientes.CurrentRow.Cells[0].Value.ToString();
            txtNomeCli.Text = dgvClientes.CurrentRow.Cells[1].Value.ToString();
            txtEndereco.Text = dgvClientes.CurrentRow.Cells[2].Value.ToString();
            txtBairro.Text = dgvClientes.CurrentRow.Cells[3].Value.ToString();
            mtbCPF.Text = dgvClientes.CurrentRow.Cells[5].Value.ToString();
            mtbRG.Text = dgvClientes.CurrentRow.Cells[6].Value.ToString();
            mtbTelefone.Text = dgvClientes.CurrentRow.Cells[7].Value.ToString();
            mtbCelular.Text = dgvClientes.CurrentRow.Cells[8].Value.ToString();
            txtEmail.Text = dgvClientes.CurrentRow.Cells[9].Value.ToString();
            mtbRenda.Text = dgvClientes.CurrentRow.Cells[10].Value.ToString();
            mtbDataNasci.Text = dgvClientes.CurrentRow.Cells[11].Value.ToString();
            picFoto.ImageLocation = dgvClientes.CurrentRow.Cells[12].Value.ToString();
            chkBloqueado.Checked = (bool)dgvClientes.CurrentRow.Cells[13].Value;
            cboCidade.Text = dgvClientes.CurrentRow.Cells[14].Value.ToString();
            txtUF.Text = dgvClientes.CurrentRow.Cells[15].Value.ToString();
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                conexao = new MySqlConnection(banco);
                conexao.Open();
                comando = new MySqlCommand("select clientes.*, cidades.nome cid_nome, cidades.uf   " +
                                           "from clientes  " +
                                           "inner join cidades ON (clientes.id_cidade = cidades.id)" +
                                           "where clientes.nome like @nome " +
                                           "order by clientes.nome", conexao);
                comando.Parameters.AddWithValue("@nome", txtPesquisa.Text + "%");

                adaptardor = new MySqlDataAdapter(comando);
                adaptardor.Fill(datTabela = new DataTable());
                dgvClientes.DataSource = datTabela;

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
                    comando = new MySqlCommand("delete from clientes where id=@id", conexao);
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

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            try
            {
                conexao = new MySqlConnection(banco);
                conexao.Open();
                comando = new MySqlCommand("update clientes set nome = @nome," +
                                                               "endereco = @endereco, " +
                                                               "bairro= @bairro, " +
                                                               "id_cidade = @id_cidade, " +
                                                               "cpf = @cpf," +
                                                               " rg = @rg, " +
                                                               "fone = @fone, " +
                                                               "celular = @celular, " +
                                                               "email = @email, " +
                                                               "renda = @renda, " +
                                                               "data_nasc = @data_nasc, " +
                                                               "foto = @foto, " +
                                                               "bloqueado = @bloqueado " +
                                                               "where id=@id", conexao);
                comando.Parameters.AddWithValue("@id", txtID.Text);
                comando.Parameters.AddWithValue("@nome", txtNomeCli.Text);
                comando.Parameters.AddWithValue("@endereco", txtEndereco.Text);
                comando.Parameters.AddWithValue("@bairro", txtBairro.Text);
                comando.Parameters.AddWithValue("@id_cidade", cboCidade.SelectedValue);
                comando.Parameters.AddWithValue("@cpf", mtbCPF.Text);
                comando.Parameters.AddWithValue("@rg", mtbRG.Text);
                comando.Parameters.AddWithValue("@fone", mtbTelefone.Text);
                comando.Parameters.AddWithValue("@celular", mtbCelular.Text);
                comando.Parameters.AddWithValue("@email", txtEmail.Text);
                comando.Parameters.AddWithValue("@renda", Convert.ToDouble(mtbRenda.Text));
                comando.Parameters.AddWithValue("@data_nasc", Convert.ToDateTime(mtbDataNasci.Text));
                comando.Parameters.AddWithValue("@foto", picFoto.ImageLocation);
                comando.Parameters.AddWithValue("@bloqueado", Convert.ToBoolean(chkBloqueado.Checked));
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
                comando = new MySqlCommand("insert into clientes " +
                                           "(nome, " +
                                           "endereco, " +
                                           "bairro, " +
                                           "id_cidade, " +
                                           "cpf, " +
                                           "rg, " +
                                           "fone, " +
                                           "celular, " +
                                           "email, " +
                                           "renda," +
                                           " data_nasc, " +
                                           "foto, " +
                                           "bloqueado)" +
                                           "values" +
                                           "(@nome, " +
                                           "@endereco, " +
                                           "@bairro, " +
                                           "@id_cidade, " +
                                           "@cpf, " +
                                           "@rg, " +
                                           "@fone, " +
                                           "@celular, " +
                                           "@email, " +
                                           "@renda, " +
                                           "@data_nasc, " +
                                           "@foto, " +
                                           "@bloqueado)", conexao);

                comando.Parameters.AddWithValue("@nome", txtNomeCli.Text);
                comando.Parameters.AddWithValue("@endereco", txtEndereco.Text);
                comando.Parameters.AddWithValue("@bairro", txtBairro.Text);
                comando.Parameters.AddWithValue("@id_cidade", cboCidade.SelectedValue);
                comando.Parameters.AddWithValue("@cpf", mtbCPF.Text);
                comando.Parameters.AddWithValue("@rg", mtbRG.Text);
                comando.Parameters.AddWithValue("@fone", mtbTelefone.Text);
                comando.Parameters.AddWithValue("@celular", mtbCelular.Text);
                comando.Parameters.AddWithValue("@email", txtEmail.Text);
                comando.Parameters.AddWithValue("@renda", Convert.ToDouble(mtbRenda.Text));
                comando.Parameters.AddWithValue("@data_nasc", Convert.ToDateTime(mtbDataNasci.Text));
                comando.Parameters.AddWithValue("@foto", picFoto.ImageLocation);
                //comando.Parameters.AddWithValue("@foto", ofdArquivo.FileName);
                comando.Parameters.AddWithValue("@bloqueado", Convert.ToBoolean(chkBloqueado.Checked));
                comando.ExecuteNonQuery();

                btnCancelar.PerformClick();
                btnConsultar.PerformClick();
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
            txtNomeCli.Clear();
            txtEndereco.Clear();
            txtBairro.Clear();
            txtPesquisa.Clear();
            cboCidade.Text ="";
            txtUF.Clear();
            picFoto.ImageLocation = null;
            mtbCPF.Clear();
            mtbRG.Clear();
            mtbTelefone.Clear();
            mtbCelular.Clear();
            txtEmail.Clear();
            mtbRenda.Clear();
            mtbDataNasci.Clear();
            chkBloqueado.Checked = false;

            txtNomeCli.Focus();
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

        private void picFoto_Click(object sender, EventArgs e)
        {
            ofdArquivo.InitialDirectory = "C:\\Pictures\\";
            ofdArquivo.FileName = "";
            ofdArquivo.ShowDialog();
            picFoto.ImageLocation = ofdArquivo.FileName;
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
