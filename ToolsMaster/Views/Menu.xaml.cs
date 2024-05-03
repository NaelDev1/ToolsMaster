using Dapper;
using Npgsql;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace ToolsMaster.Views;

/// <summary>
/// Interação lógica para Menu.xam
/// </summary>
public partial class Menu : Window
{
    StackPanel stack = new();
    StackPanel stackWheres = new();
    public List<string> bancoDados { get; set; }
    public List<string> tipoOperacao { get; set; }
    public List<string> tipo { get; set; }

    string connectionString = null;

    public Conexao conexao = Conexao.Instance();

    public Menu()
    {

        InitializeComponent();
        connectionString = conexao.stringConnection;

        CarregarTabelas();

        tipoOperacao = new() { "INSERT", "SELECT", "UPDATE" };

        tipo = new() { "TODAS COLUNAS", "APENAS" };

        DataContext = this;
        cb_tipo.SelectedIndex = 0;
    }


    private void CarregarTabelas()
    {

        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();

            bancoDados = conn.Query<string>("select table_name from information_schema.tables where table_schema = 'public'").ToList();

            if (bancoDados.Count > 0)
            {
                MessageBox.Show("Ok");
            }

        }


    }


    private void GerarSql()
    {

        if (cb_operacao.SelectedItem == "INSERT")
            GerarInsert();
        else if (cb_operacao.SelectedItem == "UPDATE")
        {
            GerarUpdate();
        }


    }

    private void GerarInsert()
    {
        List<string?> nomeWheres = null;
        List<string?> nomeColunas = PegarNomeColunas();

        if (cb_tipo.SelectedIndex == 1)
        {
            nomeColunas.Clear();
            nomeColunas = stack.Children.OfType<CheckBox>().Where(c => c.IsChecked == true && c.Tag != "MarcarTodos").Select(c => c.Content.ToString()).ToList();

        }
        if (rb_temWhre.IsChecked == true)
        {
            nomeWheres = stackWheres.Children.OfType<CheckBox>().Where(c => c.IsChecked == true && c.Tag != "MarcarTodos").Select(c => c.Content.ToString()).ToList();

        }


        StringBuilder b = new();

        b.Append($"INSERT INTO {cb_BancoDados.SelectedItem}\r");
        b.Append($"                                    (");
        for (int i = 0; i < nomeColunas.Count; i++)
        {
            if (i > 0)
                b.Append($",{nomeColunas[i]}");
            else
                b.Append($"{nomeColunas[i]}");
        }
        b.Append(")\r");
        b.Append("                     VALUES   (");
        for (int i = 0; i < nomeColunas.Count; i++)
        {
            if (i > 0)
                b.Append($",@{nomeColunas[i]}");
            else
                b.Append($"@{nomeColunas[i]}");
        }
        b.Append(")");

        if (rb_temWhre.IsChecked == true && nomeWheres.Count > 0)
        {
            b.Append("\r WHERE ");
            for (int i = 0; i < nomeWheres.Count; i++)
            {
                if (i > 0)
                    b.Append($" AND {nomeWheres[i]}=@{nomeWheres[i]}");
                else
                    b.Append($"{nomeWheres[i]}=@{nomeWheres[i]}");
            }
        }




        FlowDocument flowDocument = new FlowDocument(new Paragraph(new Run(b.ToString())));
        edt_resultado.Document = flowDocument;

    }

    private void GerarSelect()
    {
        List<string?> nomeWheres = null;
        List<string?> nomeColunas = PegarNomeColunas();

        if (cb_tipo.SelectedIndex == 1)
        {
            nomeColunas.Clear();
            nomeColunas = stack.Children.OfType<CheckBox>().Where(c => c.IsChecked == true && c.Tag != "MarcarTodos").Select(c => c.Content.ToString()).ToList();

        }
        if (rb_temWhre.IsChecked == true)
        {
            nomeWheres = stackWheres.Children.OfType<CheckBox>().Where(c => c.IsChecked == true && c.Tag != "MarcarTodos").Select(c => c.Content.ToString()).ToList();

        }


        StringBuilder b = new();

        b.Append($"INSERT INTO {cb_BancoDados.SelectedItem}\r");
        b.Append($"                                    (");
        for (int i = 0; i < nomeColunas.Count; i++)
        {
            if (i > 0)
                b.Append($",{nomeColunas[i]}");
            else
                b.Append($"{nomeColunas[i]}");
        }
        b.Append(")\r");
        b.Append("                     VALUES   (");
        for (int i = 0; i < nomeColunas.Count; i++)
        {
            if (i > 0)
                b.Append($",@{nomeColunas[i]}");
            else
                b.Append($"@{nomeColunas[i]}");
        }
        b.Append(")");

        if (rb_temWhre.IsChecked == true && nomeWheres.Count > 0)
        {
            b.Append("\r WHERE ");
            for (int i = 0; i < nomeWheres.Count; i++)
            {
                if (i > 0)
                    b.Append($" AND {nomeWheres[i]}=@{nomeWheres[i]}");
                else
                    b.Append($"{nomeWheres[i]}=@{nomeWheres[i]}");
            }
        }




        FlowDocument flowDocument = new FlowDocument(new Paragraph(new Run(b.ToString())));
        edt_resultado.Document = flowDocument;

    }


    private void GerarUpdate()
    {
        List<string> nomeColunas = stack.Children.OfType<CheckBox>().Where(c => c.IsChecked == true && c.Tag != "MarcarTodos").Select(c => c.Content.ToString()).ToList();
        List<string> nomeWheres = stackWheres.Children.OfType<CheckBox>().Where(c => c.IsChecked == true && c.Tag != "MarcarTodos").Select(c => c.Content.ToString()).ToList();
        StringBuilder b = new();

        b.Append($"UPDATE {cb_BancoDados.SelectedItem}\r");
        b.Append($"SET ");
        for (int i = 0; i < nomeColunas.Count; i++)
        {
            if (i > 0)
                b.Append($",{nomeColunas[i]}=@{nomeColunas[i]}");
            else
                b.Append($"{nomeColunas[i]}=@{nomeColunas[i]}");
        }

        if(nomeWheres.Count > 0)
        {
            b.Append("\r WHERE ");

            for (int i = 0; i < nomeWheres.Count; i++)
            {
                if (i > 0)
                    b.Append($" AND {nomeWheres[i]}=@{nomeWheres[i]}");
                else
                    b.Append($"{nomeWheres[i]}=@{nomeWheres[i]}");
            }

        }


        FlowDocument flowDocument = new FlowDocument(new Paragraph(new Run(b.ToString())));
        edt_resultado.Document = flowDocument;

    }




    private List<string?> PegarNomeColunas()
    {
        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();
            return conn.Query<string>($"select column_name from information_schema.columns where table_name = '{cb_BancoDados.SelectedItem}' ").ToList();
        }
    }

    private void bt_gerarSql_Click(object sender, RoutedEventArgs e)
    {
        GerarSql();
    }

    private void cb_operacao_Selected(object sender, RoutedEventArgs e)
    {

    }

    private void cb_operacao_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (cb_operacao.SelectedItem == "UPDATE")
        {
            cb_tipo.Visibility = Visibility.Hidden;
            lb_tipo.Visibility = Visibility.Hidden;
            rb_temWhre.Visibility = Visibility.Hidden;


            ListarColunas();
            ListarColunasWhere();

        }
        else if (cb_operacao.SelectedItem == "INSERT")
        {
            gridCenter.Children.Remove(stack);
            gridCenter.Children.Remove(stackWheres);
            stack.Children.Clear();
            stackWheres.Children.Clear();
            cb_tipo.Visibility = Visibility.Visible;
            lb_tipo.Visibility = Visibility.Visible;
            rb_temWhre.Visibility = Visibility.Visible;

        }
        else if (cb_operacao.SelectedItem == "SELECT")
        {
            cb_tipo.Visibility = Visibility.Hidden;
            lb_tipo.Visibility = Visibility.Hidden;
            rb_temWhre.Visibility = Visibility.Hidden;

            gridCenter.Children.Remove(stack);
            gridCenter.Children.Remove(stackWheres);
            stack.Children.Clear();
            stackWheres.Children.Clear();
        }
    }

    #region ListarColunas
    private void ListarColunas()
    {
        List<string> nomeColunas = PegarNomeColunas();
        gridCenter.Children.Remove(stack);
        stack.Children.Clear();


        ScrollViewer scroll = new ScrollViewer();
        scroll.SetValue(Grid.ColumnProperty, 0);
        scroll.SetValue(Grid.RowProperty, 2);
        scroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
        scroll.Content = stack;

        foreach (var nome in nomeColunas)
        {
            CheckBox check = new();
            check.Content = nome;
            stack.Children.Add(check);

        }
        if (stack.Children.Count > 0)
        {
            CheckBox checkAll = new();
            checkAll.Content = "Marcar Todos";
            checkAll.Tag = "MarcarTodos";
            checkAll.Click += (s, e) =>
            {
                if (checkAll.IsChecked == true)
                {
                    stack.Children.OfType<CheckBox>().ToList().ForEach(c => c.IsChecked = true);
                }
                else if (checkAll.IsChecked == false)
                {
                    stack.Children.OfType<CheckBox>().ToList().ForEach(c => c.IsChecked = false);
                }

            };

            stack.Children.Insert(0, checkAll);
        }
        gridCenter.Children.Add(scroll);
    }


    private void ListarColunasWhere()
    {

        List<string> nomeColunas = PegarNomeColunas();
        gridCenter.Children.Remove(stackWheres);

        stackWheres.Children.Clear();



        ScrollViewer scrollWheres = new ScrollViewer();
        scrollWheres.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
        scrollWheres.Content = stackWheres;
        scrollWheres.SetValue(Grid.ColumnProperty, 1);
        scrollWheres.SetValue(Grid.RowProperty, 2);







        foreach (var nome in nomeColunas)
        {
            CheckBox check = new();
            check.Content = nome;

            stackWheres.Children.Add(check);
        }

        if (stackWheres.Children.Count > 0)
        {
            CheckBox checkAll = new();
            checkAll.Content = "Marcar Todos";
            checkAll.Tag = "MarcarTodos";


            checkAll.Click += (s, e) =>
            {
                if (checkAll.IsChecked == true)
                {
                    stackWheres.Children.OfType<CheckBox>().ToList().ForEach(c => c.IsChecked = true);
                }
                else if (checkAll.IsChecked == false)
                {
                    stackWheres.Children.OfType<CheckBox>().ToList().ForEach(c => c.IsChecked = false);
                }

            };

            stackWheres.Children.Insert(0, checkAll);
        }



        gridCenter.Children.Add(scrollWheres);
    }

    #endregion


    private void cb_tipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (cb_tipo.SelectedIndex == 1)
        {
            ListarColunas();
           

        }
        else
        {
            gridCenter.Children.Remove(stack);
            gridCenter.Children.Remove(stackWheres);
            stack.Children.Clear();
            stackWheres.Children.Clear();
        }
    }

    private void rb_temWhre_Checked(object sender, RoutedEventArgs e)
    {
        if(rb_temWhre.IsChecked == true)
        {

        ListarColunasWhere();
        }
    }

    private void rb_temWhre_Unchecked(object sender, RoutedEventArgs e)
    {

        gridCenter.Children.Remove(stack);
        gridCenter.Children.Remove(stackWheres);
        stack.Children.Clear();
        stackWheres.Children.Clear();
    }
}
