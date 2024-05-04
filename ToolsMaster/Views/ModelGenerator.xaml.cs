using Dapper;
using Npgsql;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace ToolsMaster.Views;

/// <summary>
/// Lógica interna para ModelGenerator.xaml
/// </summary>
public partial class ModelGenerator : Window
{
    private string? _connectionString;

    public List<string>? tabela { get; set; }

    public ModelGenerator(string connectionString)
    {
        InitializeComponent();
        _connectionString = connectionString;
        DataContext = this;
        CarregarTabelas();
        cb_BancoDados.SelectedIndex = 0;
    }

   
    private void CarregarTabelas()
    {
        try
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();

                tabela = conn.Query<string>("select table_name from information_schema.tables where table_schema = 'public'").ToList();

            }
        }
        catch (Exception)
        {
           MessageBox.Show("Erro ao buscar tabelas!");
        }
      


    }
    private List<(string, string)> PegarMaps()
    {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
            conn.Open();
            return conn.Query<(string, string)>($"select column_name, data_type from information_schema.columns where table_name = '{cb_BancoDados.SelectedItem}' ").ToList()!;
        }
    }


    private IDictionary<string, string> DictionaryMap = new Dictionary<string, string>
{
    {"integer", "int"},
    {"numeric", "decimal"},
    {"timestamp without time zone", "DateTime"},
    {"time without time zone", "TimeSpan"},
    {"double precision", "double"},
    {"character varying", "string"},
    {"boolean", "bool"},
    {"\"char\"", "char"}
};
    public int MyProperty { get; set; }
    private List<(string nome_coluna, string tipo)> maps = new();

    private void bt_gerarModel_Click(object sender, RoutedEventArgs e)
    {
        //gerando o medelo a partir do tipo no banco
        maps = PegarMaps();

        List<(string coluna, string tipo)> tipo = new();

        foreach(var map in maps)
        {
            if(DictionaryMap.TryGetValue(map.tipo, out string tipoColuna))
            {
                tipo.Add((map.nome_coluna, tipoColuna));
            }
            else
            {
                MessageBox.Show("Tipo não encontrado para mapear!");
            }
        }
        
        FlowDocument flow = new();

        Paragraph p = new Paragraph();
        p.FontSize = 15;
        p.Margin = new  Thickness(10,0,0,0);
        p.FontFamily = new FontFamily("Segoi UI");

        StringBuilder b = new();

        for(int i =0; i< tipo.Count; i++)
        {
            p.Inlines.Add(new LineBreak());
            Span span = new();

            Run runPublic = new Run(" public  ");
            runPublic.Foreground = Brushes.CornflowerBlue;
            
            span.Inlines.Add(runPublic);


            Run runNomeColuna = new Run($"   {tipo[i].coluna}  ");
            runNomeColuna.Foreground = Brushes.White;
            span.Inlines.Add(runNomeColuna);


            Run runTipo = new Run($"   {tipo[i].tipo}   ");
            runTipo.Foreground = Brushes.DarkOrange;
            span.Inlines.Add(runTipo);

            Run runChave = new Run(" { ");
            runChave.Foreground = Brushes.White;
            span.Inlines.Add(runChave);

            Run runGetSet = new Run(" get; ");
            runGetSet.Foreground = Brushes.CornflowerBlue;
            span.Inlines.Add(runGetSet);

            Run runGetSet2 = new Run(" get; ");
            runGetSet2.Foreground = Brushes.CornflowerBlue;
            span.Inlines.Add(runGetSet2);

            Run runChave2 = new Run(" }");
            runChave2.Foreground = Brushes.White;
            span.Inlines.Add(runChave2);

            p.Inlines.Add(span);
            p.Inlines.Add(new LineBreak());

        }
        flow.Blocks.Add(p);

        edt_resultado.Document = flow;






    }
}
