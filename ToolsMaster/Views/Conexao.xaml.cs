using Npgsql;
using System.Windows;

namespace ToolsMaster.Views;

/// <summary>
/// Lógica interna para Conexao.xaml
/// </summary>
public partial class Conexao : Window
{
    internal string stringConnection { get; set; }
    private static Conexao _instance = null;
    public Conexao()
    {
        InitializeComponent();
        _instance = this;
    }

    public static Conexao Instance()
    {
        return _instance;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(edt_connectionString.Text))
            {
                using (NpgsqlConnection conn = new(edt_connectionString.Text))
                {
                    conn.Open();

                    MessageBox.Show("Sucesso!");
                    stringConnection = edt_connectionString.Text;
                    Menu m = new();
                    m.Show();
                    this.Hide();
                }
            }
            else
            {
                MessageBox.Show("Insira a string de conexão!");
            }
          
        }
        catch (Exception )
        {

            throw;
        }

    }
}
