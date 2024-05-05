using System.Windows;
using System.Windows.Input;

namespace ToolsMaster.Views;

/// <summary>
/// Lógica interna para Menu.xaml
/// </summary>
public partial class Menu : Window
{
    string? connectionString = null;
    public Menu()
    {
        InitializeComponent();
    }
    private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {

        Application.Current.Shutdown();
    }
    private void PanelSqlEditor_Click(object sender, MouseButtonEventArgs e)
    {
        if (connectionString == null)
        {
            if (GetStringConnection())
                AbrirSqlEditor();
            else MessageBox.Show("Falha ao se connectar com o banco de dados!");
        }
        else
            AbrirModelGenerator();

    }


    private void PanelModelGenerator_Click(object sender, MouseButtonEventArgs e)
    {
        if (connectionString == null)
        {
            if (GetStringConnection())
                AbrirModelGenerator();
            else MessageBox.Show("Falha ao se connectar com o banco de dados!");
        }
        else
            AbrirModelGenerator();
    }


    private bool GetStringConnection()
    {
        Conexao c = new();
        bool? d = c.ShowDialog();

        if (d.HasValue && d.Value)
        {
            connectionString = c.stringConnection;
            return true;

        }
        else
            return false;
    }

    private void AbrirSqlEditor()
    {
        SqlEditor s = new(connectionString);
        s.Show();
    }

    private void AbrirModelGenerator()
    {
        ModelGenerator m = new(connectionString);
        m.Show();

    }

    private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {

        Inversor i = new();
        i.Show();

    }
}
