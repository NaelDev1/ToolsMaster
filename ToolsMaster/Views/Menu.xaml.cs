using System.Windows;
using System.Windows.Input;

namespace ToolsMaster.Views;

/// <summary>
/// Lógica interna para Menu.xaml
/// </summary>
public partial class Menu : Window
{
    public Menu()
    {
        InitializeComponent();
    }

    private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        Conexao c = new();
        c.ShowDialog();
    }

    private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {

        Application.Current.Shutdown();
    }
}
