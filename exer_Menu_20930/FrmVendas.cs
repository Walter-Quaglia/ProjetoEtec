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
    public partial class FrmVendas : Form
    {
        MySqlConnection conexao;
        MySqlCommand comando;
        String banco = "server=localhost;port=3308;uid=root;pwd=etecjau;database=vendas";
        MySqlDataAdapter adaptardor;
        DataTable datTabela;
        bool bloqueado = false;
        double total, qtd;

        public FrmVendas()
        {
            InitializeComponent();
        }

        private void FrmVendas_Load(object sender, EventArgs e)
        {
            carregarComboClientes();
            carregarComboProdutos();
            btnCancelar.PerformClick();
        }
        
        private void cboClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboClientes.SelectedIndex != -1)
            {
                DataRowView reg = (DataRowView)cboClientes.SelectedItem;
                txtEndereco.Text = reg["endereco"].ToString();
                txtCidade.Text = reg["cidade"].ToString();
                txtUF.Text = reg["uf"].ToString();
                txtCPF.Text = reg["cpf"].ToString();
                txtCelular.Text = reg["celular"].ToString();
                mtbNasc.Text = reg["data_nasc"].ToString();
                picCliente.ImageLocation = reg["foto"].ToString();
                bloqueado = bool.Parse(reg["bloqueado"].ToString());
            }
            
        }
        
        private void cboProdutos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProdutos.SelectedIndex != -1) 
            { 
                DataRowView reg = (DataRowView)cboProdutos.SelectedItem;
                txtEstoque.Text = reg["estoque"].ToString();
                txtPreco.Text = reg["valorVenda"].ToString();
                picProduto.ImageLocation = reg["foto"].ToString();
            }
        }

        private void btnInserir_Click(object sender, EventArgs e)
        {
            if (txtQtd.Text == "") 
            {
                    MessageBox.Show("Inserir Quantidade de produto !", this.Text,
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtQtd.SelectAll(); //Selecionando todo o texto de controle 
                    txtQtd.Select(); // Enviando Cursor  para o Crontole
                    return;
            }
            else if (Convert.ToDouble(txtQtd.Text) > Convert.ToDouble(txtEstoque.Text))
            {
                MessageBox.Show("Estoque insuficiente!", this.Text,
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtQtd.SelectAll(); //Selecionando todo o texto de controle 
                txtQtd.Select(); // Enviando Cursor  para o Crontole
                return;
            }
            qtd = double.Parse(txtQtd.Text);
            if (qtd == 0)
            {
                MessageBox.Show("Inserir uma Quantidade de produto valida!", this.Text,
                                     MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtQtd.SelectAll(); //Selecionando todo o texto de controle 
                txtQtd.Select(); // Enviando Cursor  para o Crontole
                return;
            }
            

            dgvProdutos.Rows.Add(cboProdutos.SelectedValue, cboProdutos.Text, txtQtd.Text, txtPreco.Text);
            double quantidade = double.Parse(txtQtd.Text);
            double preco = double.Parse(txtPreco.Text);

            total += quantidade * preco;
            lblTotal.Text = total.ToString("C");
            cboProdutos.SelectedIndex = -1;
            txtEstoque.Clear();
            txtPreco.Clear();
            txtQtd.Clear();
            picProduto.ImageLocation = "";
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (cboClientes.SelectedIndex != -1)
            {
                if (bloqueado)
                {
                    MessageBox.Show("Cliente Bloqueado para venda.",this.Text,
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnCancelar.PerformClick();
                    return;
                }

                grbClientes.Enabled = false;
                grbProdutos.Enabled = true;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            
            limpaProdutos();
        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            try
            {
                // gravando o venda_cab
                conexao = new MySqlConnection(banco);
                conexao.Open();
                comando = new MySqlCommand("insert into vendas_cab (id_cliente, data, total) " +
                                                                 "values (@id_cliente, @data, @total)", conexao);
                comando.Parameters.AddWithValue("@id_cliente", cboClientes.SelectedValue);
                comando.Parameters.AddWithValue("@data", DateTime.Now);
                comando.Parameters.AddWithValue("@total", total);
                comando.ExecuteNonQuery();

                //retorna o maior ID para gravar no detalhe 
                int idvenda = int.Parse(comando.LastInsertedId.ToString());

                foreach (DataGridViewRow linha in dgvProdutos.Rows)
                {
                    //gravando o detalhe da venda
                    comando = new MySqlCommand("insert into vendas_det (id_venda, id_produto, qtde, vlr_unit)" +
                                                                     "values (@id_venda, @id_produto, @qtde, @vlr_unit)", conexao);
                    comando.Parameters.AddWithValue("@id_venda", idvenda);
                    comando.Parameters.AddWithValue("@id_produto", linha.Cells[0].Value);
                    comando.Parameters.AddWithValue("@qtde", double.Parse(linha.Cells[2].Value.ToString()));
                    comando.Parameters.AddWithValue("vlr_unit", double.Parse(linha.Cells[3].Value.ToString()));
                    comando.ExecuteNonQuery();

                    // Atualizando o estoque dos produtos vendidos 
                    comando = new MySqlCommand("update produtos set estoque = estoque - @qtde where id = @id", conexao);
                    comando.Parameters.AddWithValue("@qtde", double.Parse(linha.Cells[2].Value.ToString()));
                    comando.Parameters.AddWithValue("@id", linha.Cells[0].Value);
                    comando.ExecuteNonQuery();
                    
                }

                FrmCaixa.idCliente = int.Parse(cboClientes.SelectedValue.ToString());
                FrmCaixa.nomeCliente = cboClientes.Text;
                FrmCaixa.idvenda = idvenda;
                FrmCaixa.total = total;

                FrmCaixa form = new FrmCaixa();
                form.ShowDialog();

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

        private void btnRemover_Click(object sender, EventArgs e)
        {
            if(dgvProdutos.RowCount > 0)
            {
                double quantidade = double.Parse(dgvProdutos.CurrentRow.Cells[2].Value.ToString());
                double preco = double.Parse(dgvProdutos.CurrentRow.Cells[3].Value.ToString());

                total -= quantidade * preco;
                lblTotal.Text = total.ToString("C");

                dgvProdutos.Rows.RemoveAt(dgvProdutos.CurrentRow.Index);
            }
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void limpaProdutos()
        {
            dgvProdutos.RowCount = 0;
            cboClientes.SelectedIndex = -1;
            cboProdutos.SelectedIndex = -1;
            txtEndereco.Clear();
            txtCidade.Clear();
            txtUF.Clear();
            txtEstoque.Clear();
            txtPreco.Clear();
            txtQtd.Clear();
            txtCPF.Clear();
            txtCelular.Clear();
            mtbNasc.Clear();
            picCliente.ImageLocation = "";
            picProduto.ImageLocation = "";
            total = 0;
            lblTotal.Text = total.ToString("C");
            grbClientes.Enabled = true;
            grbProdutos.Enabled = false;
        }

        private void carregarComboProdutos()
        {
            try
            {
                conexao = new MySqlConnection(banco);
                conexao.Open();
                comando = new MySqlCommand("SELECT *" +
                                           "FROM produtos " +
                                           "order by descricao", conexao);

                adaptardor = new MySqlDataAdapter(comando);
                adaptardor.Fill(datTabela = new DataTable());
                cboProdutos.DataSource = datTabela;
                cboProdutos.ValueMember = "id";
                cboProdutos.DisplayMember = "descricao";
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

        private void carregarComboClientes()
        {
            try
            {
                conexao = new MySqlConnection(banco);
                conexao.Open();
                comando = new MySqlCommand("SELECT cl.*, ci.nome cidade, ci.uf FROM clientes cl  " +
                                           "inner join cidades ci ON (ci.id = cl.id_cidade)" +
                                           "order by nome", conexao);

                adaptardor = new MySqlDataAdapter(comando);
                adaptardor.Fill(datTabela = new DataTable());
                cboClientes.DataSource = datTabela;
                cboClientes.ValueMember = "id";
                cboClientes.DisplayMember = "nome";
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
        

    }
}
