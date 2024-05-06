using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using Tesseract;

namespace ToolsMaster.Views;

/// <summary>
/// Lógica interna para ImageToText.xaml
/// </summary>
public partial class ImageToText : Window
{
    public ImageToText()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog f = new();
        f.Filter = "Arquivos de Imagem|*.jpg;*jpeg;*.png;*.bmp";

        if(f.ShowDialog() == true)
        {
            string caminhoImagem = f.FileName;
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(caminhoImagem);
            image.EndInit();

            edt_image.Source = image;



            FlowDocument flow = new FlowDocument(new Paragraph( new Run(LerImage(image))));
            edt_resultado.Document = flow;
            
        }
    }

    private string LerImage(BitmapImage image)
    {
        try
        {
            using (var engine = new TesseractEngine(@$"{AppDomain.CurrentDomain.BaseDirectory}/tessdata", "por", EngineMode.Default))
            {
                var bitMapEncoder = new BmpBitmapEncoder();
                bitMapEncoder.Frames.Add(BitmapFrame.Create(image));

                using (var stream = new MemoryStream())
                {

                    bitMapEncoder.Save(stream);
                    stream.Seek(0, SeekOrigin.Begin);

                    using (var pix = Pix.LoadFromMemory(stream.ToArray()))
                    {
                        using (var page = engine.Process(pix))
                        {
                            return page.GetText();
                        }
                    }

                }
            }
        }
        catch (Exception e)
        {
            //MessageBox.Show(e.Message);
            return null;
        }
     

    }

    private void Convet()
    {
      
    }
}
