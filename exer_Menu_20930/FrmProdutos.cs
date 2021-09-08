using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace exer_Menu_20930
{
    public partial class FrmProdutos : Form
    {
        MySqlConnection conexao;
        MySqlCommand comando;
        String banco = "server=localhost;port=3308;uid=root;pwd=etecjau;database=vendas";
        MySqlDataAdapter adaptardor;
        DataTable datTabela;

        public FrmProdutos()
        {
            InitializeComponent();
        }

        private void FrmProdutos_Load(object sender, EventArgs e)
        {
            try
            {
                carregarComboCategorias();
                carregarComboFornecedores();
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

        private void dgvProdutos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtID.Text = dgvProdutos.CurrentRow.Cells[0].Value.ToString();
            txtdescricao.Text = dgvProdutos.CurrentRow.Cells[1].Value.ToString();
            txtCodBarra.Text = dgvProdutos.CurrentRow.Cells[2].Value.ToString();
            //cboCategoria.Text = dgvProdutos.CurrentRow.Cells[3].Value.ToString();
            //cboFornecedor.Text = dgvProdutos.CurrentRow.Cells[4].Value.ToString();
            txtEstoque.Text = dgvProdutos.CurrentRow.Cells[5].Value.ToString();
            txtEstoqueMini.Text = dgvProdutos.CurrentRow.Cells[6].Value.ToString();
            txtVenda.Text = dgvProdutos.CurrentRow.Cells[7].Value.ToString();
            txtCusto.Text = dgvProdutos.CurrentRow.Cells[8].Value.ToString();
            picFoto.ImageLocation = dgvProdutos.CurrentRow.Cells[9].Value.ToString();
            txtLinkVideo.Text = dgvProdutos.CurrentRow.Cells[10].Value.ToString();
            chkForaLinha.Checked = (bool)dgvProdutos.CurrentRow.Cells[11].Value;
            cboFornecedor.Text = dgvProdutos.CurrentRow.Cells[12].Value.ToString();
            txtFantasia.Text = dgvProdutos.CurrentRow.Cells[13].Value.ToString();
            cboCategoria.Text = dgvProdutos.CurrentRow.Cells[14].Value.ToString();
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                conexao = new MySqlConnection(banco);
                conexao.Open();
                comando = new MySqlCommand("select produtos.*, fornecedores.razao_social, fornecedores.fantasia, categorias.descricao cat_descricao " +
                                                  "from produtos  " +
                                                  "inner join fornecedores ON (produtos.id_fornecedor = fornecedores.id)" +
                                                  "inner join categorias ON (produtos.id_categoria = categorias.id)" +
                                                  "where produtos.descricao like @nome " +
                                                  "order by produtos.descricao", conexao);
                comando.Parameters.AddWithValue("@nome", txtPesquisa.Text + "%");

                adaptardor = new MySqlDataAdapter(comando);
                adaptardor.Fill(datTabela = new DataTable());
                dgvProdutos.DataSource = datTabela;

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

                    comando = new MySqlCommand("delete from produtos where id=@id", conexao);
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
                comando = new MySqlCommand("update produtos set descricao = @descricao," +
                                                               "codigobarra = @codigobarra, " +
                                                               "id_categoria = @id_categoria, " +
                                                               "id_fornecedor = @id_fornecedor, " +
                                                               "estoque = @estoque, " +
                                                               "estoqueMinimo = @estoqueMinimo, " +
                                                               "valorVenda = @valorVenda, " +
                                                               "valorCusto = @valorCusto, " +
                                                               "foto = @foto, " +
                                                               "linkVideo = @linkVideo, " +
                                                               "foraLinha = @foraLinha where id=@id", conexao);
                comando.Parameters.AddWithValue("@id", txtID.Text);
                comando.Parameters.AddWithValue("@descricao", txtdescricao.Text);
                comando.Parameters.AddWithValue("@codigobarra", txtCodBarra.Text);
                comando.Parameters.AddWithValue("id_categoria", cboCategoria.SelectedValue);
                comando.Parameters.AddWithValue("id_fornecedor", cboFornecedor.SelectedValue);
                comando.Parameters.AddWithValue("@estoque", Convert.ToDouble(txtEstoque.Text));
                comando.Parameters.AddWithValue("@estoqueMinimo", Convert.ToDouble(txtEstoqueMini.Text));
                comando.Parameters.AddWithValue("@valorVenda", Convert.ToDouble(txtVenda.Text));
                comando.Parameters.AddWithValue("@valorCusto", Convert.ToDouble(txtCusto.Text));
                comando.Parameters.AddWithValue("@foto", picFoto.ImageLocation);
                comando.Parameters.AddWithValue("@linkVideo", txtLinkVideo.Text);
                comando.Parameters.AddWithValue("@foraLinha", Convert.ToBoolean(chkForaLinha.Checked));
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
                comando = new MySqlCommand("insert into produtos " +
                                           "(descricao, " +
                                           "codigobarra, " +
                                           "id_categoria, " +
                                           "id_Fornecedor, " +
                                           "estoque, " +
                                           "estoqueMinimo, " +
                                           "valorVenda, " +
                                           "valorCusto, " +
                                           "foto, " +
                                           "linkVideo," +
                                           "foraLinha)" +
                                           "values" +
                                           "(@descricao, " +
                                           "@codigobarra, " +
                                           "@id_categoria, " +
                                           "@id_fornecedor, " +
                                           "@estoque, " +
                                           "@estoqueMinimo, " +
                                           "@valorVenda, " +
                                           "@valorCusto, " +
                                           "@foto, " +
                                           "@linkVideo, " +
                                           "@foraLinha)", conexao);

                comando.Parameters.AddWithValue("@descricao", txtdescricao.Text);
                comando.Parameters.AddWithValue("@codigobarra", txtCodBarra.Text);
                comando.Parameters.AddWithValue("id_categoria", cboCategoria.SelectedValue);
                comando.Parameters.AddWithValue("id_fornecedor", cboFornecedor.SelectedValue);
                comando.Parameters.AddWithValue("@estoque", Convert.ToDouble(txtEstoque.Text));
                comando.Parameters.AddWithValue("@estoqueMinimo", Convert.ToDouble(txtEstoqueMini.Text));
                comando.Parameters.AddWithValue("@valorVenda", Convert.ToDouble(txtVenda.Text));
                comando.Parameters.AddWithValue("@valorCusto", Convert.ToDouble(txtCusto.Text));
                comando.Parameters.AddWithValue("@foto", picFoto.ImageLocation);
                comando.Parameters.AddWithValue("@linkVideo", txtLinkVideo.Text);
                comando.Parameters.AddWithValue("@foraLinha", Convert.ToBoolean(chkForaLinha.Checked));
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
            txtdescricao.Clear();
            txtEstoque.Clear();
            txtEstoqueMini.Clear();
            txtPesquisa.Clear();
            txtFantasia.Clear();
            cboFornecedor.Text = "";
            cboCategoria.Text = "";
            picFoto.ImageLocation = null;
            txtLinkVideo.Clear();
            chkForaLinha.Checked = false;
            txtCusto.Clear();
            txtVenda.Clear();
            txtCodBarra.Clear();

            txtdescricao.Focus();
        }

        private void carregarComboCategorias()
        {
            try
            {
                conexao = new MySqlConnection(banco);
                conexao.Open();
                comando = new MySqlCommand("SELECT * " +
                                           "FROM categorias " +
                                           "order by descricao", conexao);

                adaptardor = new MySqlDataAdapter(comando);
                adaptardor.Fill(datTabela = new DataTable());
                cboCategoria.DataSource = datTabela;
                cboCategoria.ValueMember = "id";
                cboCategoria.DisplayMember = "descricao";
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

        private void carregarComboFornecedores()
        {
            try
            {

                conexao = new MySqlConnection(banco);
                conexao.Open();
                comando = new MySqlCommand("SELECT id, razao_social,fantasia " +
                                           "FROM fornecedores " +
                                           "order by razao_social", conexao);

                adaptardor = new MySqlDataAdapter(comando);
                adaptardor.Fill(datTabela = new DataTable());
                cboFornecedor.DataSource = datTabela;
                cboFornecedor.ValueMember = "id";
                cboFornecedor.DisplayMember = "razao_social";
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

        private void cboFornecedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboFornecedor.SelectedIndex != -1)
            {
                DataRowView reg = (DataRowView)cboFornecedor.SelectedItem;
                txtFantasia.Text = reg["fantasia"].ToString();
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

        private void btnVisualizar_Click(object sender, EventArgs e)
        {
            Process.Start(txtLinkVideo.Text);
        }
    }
}
