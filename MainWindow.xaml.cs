using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using Microsoft.Win32;
using SiegenerXmlNarrator.Controller;

namespace SiegenerXmlNarrator
{
  /// <summary>
  /// Interaktionslogik für MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private GameController _controller;

    public MainWindow()
    {
      InitializeComponent();
    }

    private void StartGame(object sender, RoutedEventArgs e)
    {
      // Durchsucht das Verzeichnis in dem die EXE-Datei liegt nach Dateien mit der Endung .game und gibt das erste Ergebnis zurück.
      var game = Directory.GetFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "*.game").FirstOrDefault();

      // Wenn keine .game-Datei gefunden wurd, wähle diese manuell aus.
      while (game == null)
      {
        var ofd = new OpenFileDialog();
        ofd.Filter = "SiegenerXmlNarrator .game-Datei (*.game)|*.game";
        if (ofd.ShowDialog() == true)
          game = ofd.FileName;
      }

      _controller = new GameController(this, game);
      _controller.NullGameTask += ControllerOnNullGameTask;
    }

    private void ControllerOnNullGameTask(object sender, EventArgs e)
    {
      Close();
    }

    private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      list_Answers.Width = e.NewSize.Width;
    }
  }
}
