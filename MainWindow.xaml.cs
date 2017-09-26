using System;
using System.Windows;
using System.Windows.Media;
using System.IO;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Collections.Generic;

namespace ASCII_Images
{
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    private void ChooseImage_Click(object sender, RoutedEventArgs e)
    {
      string path = "";
      OpenFileDialog OPF = new OpenFileDialog();
      OPF.Filter = "Файлы png and jpg|*.png;*.jpg";
      if (OPF.ShowDialog() == true)
      {
        path = OPF.FileName;
      }

      if (path != string.Empty)
      {
        BitmapImage bmi = new BitmapImage(new Uri(path));
        var grayscaleBitmap = new FormatConvertedBitmap(bmi, PixelFormats.Gray8, null, 0d);

        int height = grayscaleBitmap.PixelHeight;
        int width = grayscaleBitmap.PixelWidth;
        var bufferSize = height * width;
        var buffer = new byte[bufferSize];
        grayscaleBitmap.CopyPixels(buffer, width, 0);


        int heightRes;
        int widthRes;
        if (height < 100 || width < 100)
        {
          heightRes = height / 2;
          widthRes = width;
        }
        else
        {
          heightRes = 100;
          widthRes = width / (height / 200);
        }


        int hblock = height / heightRes;
        int wblock = width / widthRes;

        string res = "";
        double sum;


        for (int i = 0; i < heightRes; ++i)
        {
          for (int j = 0; j < widthRes; ++j)
          {
            sum = 0;
            for (int k = 0; k < hblock; ++k)
            {
              for (int l = 0; l < wblock; ++l)
              {
                sum += buffer[i * hblock * width + k * width + j * wblock + l];
              }
            }
            sum /= hblock * wblock;
            if (sum >= 192)
            {
              res += "  ";
            }
            else if (sum >= 128)
            {
              res += "._";
            }
            else if (sum >= 64)
            {
              res += "r.";
            }
            else
            {
              res += "#";
            }
          }
          res += "\n";
        }

        tmpBlock.Text = res;
      }
    }

    private void SaveImage_Click(object sender, RoutedEventArgs e)
    {
      string path = "";
      string ans = tmpBlock.Text;
      List<string> imageInLines = new List<string>(ans.Split('\n'));
      if (ans != "")
      {
        SaveFileDialog SFD = new SaveFileDialog();
        SFD.Filter = "Файлы txt|*.txt";
        if (SFD.ShowDialog() == true)
        {
          path = SFD.FileName;
        }
        StreamWriter sw = new StreamWriter(path);
        for (int i = 0; i < imageInLines.Count; ++i)
        {
          sw.WriteLine(imageInLines[i]);
        }
        sw.Close();
      }
    }
  }
}