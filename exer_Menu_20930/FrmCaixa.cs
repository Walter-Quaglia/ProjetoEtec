using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace exer_Menu_20930
{
    public partial class FrmCaixa : Form
    {
        MySqlConnection conexao;
        MySqlCommand comando;
        String banco = "server=localhost;port=3308;uid=root;pwd=etecjau;database=vendas";

        public static int idCliente, idVenda;
        public static String nomeCliente;
        public static double total;

        double pago, troco, dinheiro, cartao, cheque;
        internal static int idvenda;

        public FrmCaixa()
        {
            InitializeComponent();
        }

        private void FrmCaixa_Load(object sender, EventArgs e)
        {
            txtidCliente.Text = idCliente.ToString();
            txtNomeCliente.Text = nomeCliente;
            txtValor.Text = total.ToString("C");
            txtVenda.Text = idvenda.ToString();
            calcularTroco();
        }

        private void btnCaixa_Click(object sender, EventArgs e)
        {
            if(troco < 0)
            {
                MessageBox.Show("Valor pago menor que o valor da venda ", "Caixa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                conexao = new MySqlConnection(banco);
                conexao.Open();
                comando = new MySqlCommand("insert into caixa (id_venda, dinheiro, cartao, cheque, troco, tipo_mov)" +
                                                        "Values (@id_venda, @dinheiro, @cartao, @cheque, @troco, @tipo_mov)", conexao);
                comando.Parameters.AddWithValue("@id_venda", idvenda);
                comando.Parameters.AddWithValue("@dinheiro", dinheiro);
                comando.Parameters.AddWithValue("@cartao", cartao);
                comando.Parameters.AddWithValue("@cheque", cheque);
                comando.Parameters.AddWithValue("@troco", troco);
                comando.Parameters.AddWithValue("@tipo_mov", "E");
                comando.ExecuteNonQuery();
                Close();
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

        private void txtCartao_TextChanged(object sender, EventArgs e)
        {
            calcularTroco();
        }

        private void txtCheque_TextChanged(object sender, EventArgs e)
        {
            calcularTroco();
        }

        private void txtDinheiro_TextChanged(object sender, EventArgs e)
        {
            calcularTroco();
        }


        void calcularTroco()
        {
            if (txtDinheiro.Text == "") dinheiro = 0; else dinheiro = double.Parse(txtDinheiro.Text);
            if (txtCheque.Text == "") cheque = 0; else cheque = double.Parse(txtCheque.Text);
            if (txtCartao.Text == "") cartao = 0; else cartao = double.Parse(txtCartao.Text);

            pago = dinheiro + cartao + cheque;
            troco = pago - total;
            txtTroco.Text = troco.ToString("C");
            if (troco < 0) txtTroco.ForeColor = Color.Red;
            else txtTroco.ForeColor = Color.Blue;
        }
    }
}
