using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace exer_Menu_20930
{
    public partial class FrmConsultaVendas : Form
    {
        MySqlConnection conexao;
        MySqlCommand comando;
        String banco = "server=localhost;port=3308;uid=root;pwd=etecjau;database=vendas";
        MySqlDataAdapter adaptardor;
        DataTable datTabela;

        public FrmConsultaVendas()
        {
            InitializeComponent();
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                conexao = new MySqlConnection(banco);
                conexao.Open();
                comando = new MySqlCommand("SELECT c.descricao Categorias, sum(vd.qtde) Quantidade FROM categorias c " +
                                           "inner join produtos p ON (c.id = p.id_categoria)" +
                                           "inner join vendas_det vd ON (vd.id_produto = p.id)" +
                                           "inner join vendas_cab vc ON (vc.id = vd.id_venda)" +
                                           "where vc.data >= @dataini and vc.data <= @datafim group by c.id", conexao);

                comando.Parameters.AddWithValue("@dataini", DateTime.Parse(dtpInicio.Value.ToString("dd/MM/yyyy")));
                comando.Parameters.AddWithValue("@datafim", DateTime.Parse(dtpFinal.Value.ToString("dd/MM/yyyy")));
                adaptardor = new MySqlDataAdapter(comando);
                adaptardor.Fill(datTabela = new DataTable());
                dgvConsulta.DataSource = datTabela;
               

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

        private void btnGrafico_Click(object sender, EventArgs e)
        {
            //
            String[] categoria = new String[dgvConsulta.RowCount];
            double[] qtde = new double[dgvConsulta.RowCount];
            int i = 0;

            foreach (DataGridViewRow linha in dgvConsulta.Rows)
            {
                categoria[i] = linha.Cells[0].Value.ToString();
                qtde[i] = Convert.ToDouble(linha.Cells[1].Value);
                i += 1;
            }
            chartvendas.Series[0].ChartType = SeriesChartType.Pie;
            chartvendas.Titles.Add("Gráfico de vendas por categoria");
            chartvendas.ChartAreas[0].Area3DStyle.Enable3D = true;
            chartvendas.Series[0].Points.DataBindXY(categoria, qtde);
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.WindowState = FormWindowState.Maximized;
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //Desenvolvimento do cabeçalho do relatório
            e.Graphics.DrawString("Relatorios de vendas", new Font("Arial", 30, FontStyle.Bold), Brushes.Black, 230, 10);
            e.Graphics.DrawLine(Pens.Black, 100, 90, 720, 90);
            e.Graphics.DrawString("Categoria", new Font("Arial", 10), Brushes.Black, 100, 95);
            e.Graphics.DrawString("Quantidade", new Font("Arial", 10), Brushes.Black, 600, 95);
            e.Graphics.DrawLine(Pens.Black, 100, 120, 720, 120);
            //Desenvolvimento da interface do corpo do relatório
            int posicao_y = 100;
            foreach (DataGridViewRow linha in dgvConsulta.Rows)
            {
                posicao_y += 30;
                e.Graphics.DrawString(linha.Cells[0].Value.ToString(), new Font("Arial", 10), Brushes.Black, 100, posicao_y);
                e.Graphics.DrawString(linha.Cells[1].Value.ToString(), new Font("Arial", 10), Brushes.Black, 600, posicao_y);
            }
            // Desenvolvimento da interface do relatório
            e.Graphics.DrawLine(Pens.Black, 100, 1060, 720, 1060);
            e.Graphics.DrawString("Total de Ctegorias:", new Font("Arial", 10), Brushes.Black, 100, 1065);
            e.Graphics.DrawString(dgvConsulta.RowCount.ToString(), new Font("Arial", 10), Brushes.Black, 250, 1065);
            e.Graphics.DrawString(System.DateTime.Now.ToString(), new Font("Arial", 10), Brushes.Black, 500, 1065);
            e.Graphics.DrawLine(Pens.Black, 100, 1090, 720, 1090);
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
