using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Documents;

namespace ToolsMaster.Views;

/// <summary>
/// Lógica interna para Inversor.xaml
/// </summary>
public partial class Inversor : Window
{
    public Inversor()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var document = edt_valor.Document;
        var range = new TextRange(document.ContentStart, document.ContentEnd);


        string valor = range.Text;


        string[] partes = valor.Split('\n');

        if(partes.Count() > 0)
        {

            Paragraph p = new();

            Span span = new();

            for (int i = 0; i < partes.Count(); i++)
            {
                string[] valores = partes[i].Split('=');

                if (valores.Length == 2)
                {
                    Run run = new Run(@$"{(valores[1].Trim().Contains(".ToString()")
                        ? valores[1].Trim().Replace(".ToString()", "")
                        : valores[1].Trim().Contains("int.Parse(")
                        ? valores[1].Trim().Replace("int.Parse(", "").Replace(")","")
                        : valores[1].Trim().Contains("bool.Parse(")
                        ? valores[1].Trim().Replace("bool.Parse(", "").Replace(")", "")
                        : valores[1].Trim())} = {(valores[1].Trim().EndsWith(".ToString()") ? ($"{valores[0].Trim()}.ToString()") : valores[0].Trim())}");
                    span.Inlines.Add(run);
                    span.Inlines.Add(new LineBreak());
                }
            }

            p.Inlines.Add(span);

            FlowDocument flow = new();
            flow.Blocks.Add(p);

            edt_resultado.Document = flow;
        }

    }
}
