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
    public partial class FrmMenu : Form
    {
        MySqlConnection conexao;
        MySqlCommand comando;

        public FrmMenu()
        {
            InitializeComponent();
        }

        //criando o banco de dados do sistema 
        private void FrmMenu_Load(object sender, EventArgs e)
        {
            try
            {
                //conexao = new MySqlConnection("server=127.0.0.1;port=3308;uid=root;pwd=etecjau;database=controle_produtos");
                conexao = new MySqlConnection("server=localhost;port=3308;uid=root;pwd=etecjau");
                conexao.Open();

                comando = new MySqlCommand("CREATE DATABASE IF NOT EXISTS vendas; USE vendas", conexao);
                comando.ExecuteNonQuery();

                comando = new MySqlCommand("CREATE TABLE IF NOT EXISTS cidades" +
                                           "(id integer auto_increment primary key, " +
                                           "nome char(40), " +
                                           "uf char(2))", conexao);
                comando.ExecuteNonQuery();

                comando = new MySqlCommand("CREATE TABLE IF NOT EXISTS clientes" +
                                           "(id integer auto_increment primary key, " +
                                           "nome char(40), " +
                                           "endereco char(40), " +
                                           "bairro char(30), " +
                                           "id_cidade int(11), " +
                                           "cpf char(14), " +
                                           "rg char(12), " +
                                           "fone char(14), " +
                                           "celular char(14), " +
                                           "email varchar(50), " +
                                           "renda decimal(10,2), " +
                                           "data_nasc date, " +
                                           "foto varchar(100), " +
                                           "bloqueado boolean) ", conexao);
                comando.ExecuteNonQuery();

                comando = new MySqlCommand("CREATE TABLE IF NOT EXISTS categorias" +
                                           "(id integer auto_increment primary key, " +
                                           "descricao char(30))", conexao);
                comando.ExecuteNonQuery();

                comando = new MySqlCommand("CREATE TABLE IF NOT EXISTS fornecedores" +
                                           "(id integer auto_increment primary key, " +
                                           "razao_social char(40), " +
                                           "fantasia char(30), " +
                                           "endereco char(40), " +
                                           "bairro char(30), " +
                                           "id_cidade int(11), " +
                                           "cnpj char(18), " +
                                           "ie char(15), " +
                                           "fone char(14), " +
                                           "contato char(40), " +
                                           "email varchar(60)) ", conexao);
                comando.ExecuteNonQuery();

                comando = new MySqlCommand("CREATE TABLE IF NOT EXISTS produtos" +
                                           "(id integer auto_increment primary key, " +
                                           "descricao char(40), " +
                                           "codigobarra char(14), " +
                                           "id_categoria int(11), " +
                                           "id_fornecedor int(11), " +
                                           "estoque decimal(10,3), " +
                                           "estoqueMinimo decimal(10,3), " +
                                           "valorVenda decimal(10,2), " +
                                           "valorCusto decimal(10,2), " +
                                           "foto varchar(100), " +
                                           "linkVideo varchar(100), " +
                                           "foraLinha tinyint(1)) ", conexao);
                comando.ExecuteNonQuery();

                //criando cabeçalho da venda
                comando = new MySqlCommand("CREATE TABLE IF NOT EXISTS vendas_cab" +
                                           "(id integer auto_increment primary key, " +
                                           "id_cliente smallint," +
                                           "data date," +
                                           "total double(10,2))", conexao);
                comando.ExecuteNonQuery();

                //criando a tabela de detalhe da venda 
                comando = new MySqlCommand("CREATE TABLE IF NOT EXISTS vendas_det" +
                                          "(id integer auto_increment primary key, " +
                                          "id_venda smallint," +
                                          "id_produto smallint," +
                                          "qtde double(10,3)," +
                                          "vlr_unit double(10,2))", conexao);
                comando.ExecuteNonQuery();

                comando = new MySqlCommand("CREATE TABLE IF NOT EXISTS caixa" +
                                           "(id integer auto_increment primary key, " +
                                           "id_venda int not null," +
                                           "dinheiro decimal(10,2)," +
                                           "cartao decimal(10,2)," +
                                           "cheque decimal(10,2)," +
                                           "troco decimal(10,2)," +
                                           "tipo_mov char(1))", conexao);
                comando.ExecuteNonQuery();

                conexao.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conexao.Close();
            }
        }

        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //criando um objeto do tipo do  formulário que nós queremos abrir 
            FrmClientes Frm = new FrmClientes();

            // abrindo o formulário 
            Frm.Show();
        }

        private void cidadesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //criando um objeto do tipo do  formulário que nós queremos abrir 
            FrmCidades Frm = new FrmCidades();

            // abrindo o formulário 
            Frm.Show();
        }

        private void categoriasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //criando um objeto do tipo do  formulário que nós queremos abrir 
            FrmCategorias Frm = new FrmCategorias  ();

            // abrindo o formulário 
            Frm.Show();
        }

        private void produtosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //criando um objeto do tipo do  formulário que nós queremos abrir 
            FrmProdutos Frm = new FrmProdutos();

            // abrindo o formulário 
            Frm.Show();
        }

        private void fornecedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //criando um objeto do tipo do  formulário que nós queremos abrir 
            FrmFornecedores Frm = new FrmFornecedores();

            // abrindo o formulário 
            Frm.Show();
        }

        private void finalizarOSistemaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pedidosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //criando um objeto do tipo do  formulário que nós queremos abrir 
            FrmVendas Frm = new FrmVendas();

            // abrindo o formulário 
            Frm.Show();

        }

        private void vendasToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FrmConsultaVendas Frm = new FrmConsultaVendas();
            Frm.Show();
        }

    }
}
        


