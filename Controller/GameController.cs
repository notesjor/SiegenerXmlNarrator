using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using SiegenerXmlNarrator.Model;
using game = SiegenerXmlNarrator.Model.game;
using options = SiegenerXmlNarrator.Model.options;
using text = SiegenerXmlNarrator.Model.text;

namespace SiegenerXmlNarrator.Controller
{
  public class GameController
  {
    private MainWindow _window;
    private game _game;

    public GameController(MainWindow window, string path)
    {
      _window = window;

      // Deserialisiere die XML-Daten
      var serializer = new XmlSerializer(typeof(game));
      using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        _game = serializer.Deserialize(fs) as game;

      // Starte das Spiel mit der ersten Anweisung
      Next(GetId(_game.sec.First().Items.First()));
    }

    /// <summary>
    /// Gibt die Id zu einer Spiel-Anweisund (text/options) zurück.
    /// </summary>
    /// <param name="obj">Spiel-Anweisund</param>
    /// <returns></returns>
    private string GetId(object obj)
    {
      switch (obj)
      {
        case options o:
          return o.id;
        case text t:
          return t.id;
      }

      return "";
    }

    /// <summary>
    /// Sucht nach Spiel-Anweisungen mit Id und führt diese aus.
    /// </summary>
    /// <param name="id"></param>
    private void Next(string id)
    {
      var task = FindXmlNode(id);
      NextSetupDefault(task);

      switch (task)
      {
        // wenn task von Typ text
        case text t:
          var btn = new Button();
          btn.Content = "Weiter ...";
          btn.Click += (s, e) => Next(t.jump); // "+= (s, e) =>" verknüpft das Click-Ereignis mit der Funktion Next(string id) 

          _window.list_Answers.Children.Add(btn);
          break;
        // wenn task vom Typ options
        case options o:
          foreach (var op in o.option)
          {
            var obtn = new Button();
            obtn.Content = op.Text?.Trim();
            obtn.Click += (s, e) => Next(op.jump); // "+= (s, e) =>" verknüpft das Click-Ereignis mit der Funktion Next(string id) 

            _window.list_Answers.Children.Add(obtn);
          }

          break;
      }
    }

    private void NextSetupDefault(gametask task)
    {
      if (task == null)
      {
        _window.Close();
        return;
      }

      // Setzt die Bilder und den Ansprachtext für den Spieler
      _window.text_Question.Text = task.question?.Trim(); // Ansprachetext

      if (task.ui != null)
      {
        if (!string.IsNullOrEmpty(task.ui.left))
          _window.img_left.Source = LoadImage(task.ui.left);
        if (!string.IsNullOrEmpty(task.ui.right))
          _window.img_right.Source = LoadImage(task.ui.right);
        if (!string.IsNullOrEmpty(task.ui.background))
          _window.background.Background = new ImageBrush(LoadImage(task.ui.background)) { Stretch = Stretch.Uniform };
      }

      // Löscht alle Spieler-Antwortoptionen und NPC-Beiträge
      _window.list_Answers.Children.Clear();
      _window.npc_talk.Children.Clear();

      // Erstellt alle NPC-Beiträge für die aktuelle Spielanweisung
      if (task.npcs != null)
        foreach (var npc in task.npcs)
        {
          // Erzeugt einen farbigen Rahmen
          var border = new Border();
          border.BorderBrush = new BrushConverter().ConvertFromString(npc.color) as SolidColorBrush;
          border.BorderThickness = new Thickness(5);
          border.Margin = new Thickness(5, 2.5, 5, 2.5);
          border.Padding = new Thickness(5);
          // Anpassung für den Rahmen abhängig von der Ausrichtung
          border.CornerRadius = npc.isLeft ? new CornerRadius(20, 20, 20, 0) : new CornerRadius(20, 20, 0, 20);
          border.HorizontalAlignment = npc.isLeft ? HorizontalAlignment.Left : HorizontalAlignment.Right;

          // border kann nur ein Child haben. Daher verpacken wir Name (NPC) und Äußerung (NPC) in einen Stack
          var stack = new StackPanel();
          border.Child = stack;

          // Name des NPC
          var name = new TextBlock();
          name.FontWeight = FontWeights.Bold; // Name fett hervorheben
          name.Text = npc.name;
          stack.Children.Add(name);

          // Äußerung NPC
          var turn = new TextBlock();
          turn.Text = npc.turn;
          turn.TextWrapping = TextWrapping.Wrap; // Bricht Text automatisch um.
          stack.Children.Add(turn);
        }
    }

    /// <summary>
    /// Lädt ein Bild von der Festplatte als WPF-ImageSource
    /// </summary>
    /// <param name="imagePath">Pfad des Bildes</param>
    /// <returns>Bild als WPF-ImageSource</returns>
    private ImageSource LoadImage(string imagePath)
    {
      if (imagePath == null || !File.Exists(imagePath))
        return null;

      using (var fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
        return BitmapFrame.Create(fs, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
    }

    /// <summary>
    /// Durchsucht alle Spiel-Anweisungen und gibt den ersten passend Fund zur Id zurück.
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Spiel-Anweisung</returns>
    private gametask FindXmlNode(string id)
    {
      foreach (var sec in _game.sec)
        foreach (var item in sec.Items)
        {
          if (item is gametask task && GetId(task) == id)
            return task;
        }
      return null;
    }
  }
}
