using Npgsql;
using System.Windows;

namespace ToolsMaster.Views;

/// <summary>
/// Lógica interna para Conexao.xaml
/// </summary>
public partial class Conexao : Window
{
    public string stringConnection { get; set; }
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

                    stringConnection = edt_connectionString.Text;
                    this.DialogResult = true;
                    this.Close();
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
