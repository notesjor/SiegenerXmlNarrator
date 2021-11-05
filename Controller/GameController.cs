using System;
using System.Collections.Generic;
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
    private Dictionary<string, string> _vars = new Dictionary<string, string>();

    public GameController(MainWindow window, string path)
    {
      _window = window;

      // Deserialisiere die XML-Daten
      var serializer = new XmlSerializer(typeof(game));
      using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        _game = serializer.Deserialize(fs) as game;

      // Lade Variablen für leichteren Zugriff (Zugriff erfolgt dann über GetVar und SetVar)
      // id = name / value => Wert (immer ein String)
      if (_game.vars != null)
        foreach (var v in _game.vars)
          SetVar(v.id, v.value);

      // Starte das Spiel mit der ersten Anweisung
      Next(GetId(_game.sec.First().Items.First()));
    }

    /// <summary>
    /// Gibt die Variable zurück
    /// </summary>
    /// <param name="name">Name der Variable</param>
    /// <returns>Wert als String</returns>
    private string GetVar(string name)
    {
      return _vars.ContainsKey(name) ? _vars[name] : null;
    }

    /// <summary>
    /// Speichere eine Variable als Text
    /// </summary>
    /// <param name="name">Name der Variable</param>
    /// <param name="value">Wert</param>
    private void SetVar(string name, string value)
    {
      if (_vars.ContainsKey(name))
        _vars[name] = value;
      else
        _vars.Add(name, value);

      // Jedes Mal wenn eine Variable verändert wird, muss der Fenstertitel geändert werden
      var vars = new List<string>();
      foreach (var v in _vars)
        vars.Add(v.Key + ": " + v.Value);

      _window.Title = _game.name + " - " + string.Join(" | ", vars);

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
    /// <param name="id">id der Spiel-Anweisung zu der gesprungen werden soll</param>
    /// <param name="checkRules">Sollen zu der gesprungen werden soll (Standard: true)</param>
    private void Next(string id, bool checkRules = true)
    {
      var task = FindXmlNode(id);
      if (task == null)
      {
        NullGameTask?.Invoke(this, null); // Schließt des Fenster, wenn kein Task verfügbar ist.
        return;
      }

      // Wende changes an
      ApplyChanges(task.change);
      // Überprüfe rules (nach den Changes)
      if (checkRules && CheckRules()) return;

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
            obtn.Content = new TextBlock { TextWrapping = TextWrapping.Wrap, Text = op.Text?.Trim() };
            obtn.Click += (s, e) =>
            {
              ApplyChanges(op.change);
              Next(op.jump);
            }; // "+= (s, e) =>" verknüpft das Click-Ereignis mit der Funktion Next(string id) 

            _window.list_Answers.Children.Add(obtn);
          }

          break;
      }
    }

    private bool CheckRules()
    {
      if (_game.rules == null)
        return false;

      foreach (var r in _game.rules)
      {
        // vT = Variable (Input) als String
        // vI = Variable (Input) als Int (cast von vT)
        // vR = Variable (Rule) als Int

        var vT = GetVar(r.id); // Variablen werden im Spiel immer als Text/String gespeichert
        var vI = string.IsNullOrEmpty(vT) ? 0 : int.Parse(vT); // Überprüfen ob Variable leer (0)
        var vR = int.Parse(r.value);

        // Wenn eine Regel zutrifft, wird die weitere Ausführung mit return true unterbrochen
        // Wichtig: checkRules für Next() muss false sein um ein endloses Überprüfen der Regeln zu unterbinden.
        switch (r.@operator)
        {
          case "smaller":
            if (vI < vR)
            {
              Next(r.jump, false);
              return true;
            }
            break;
          case "equal":
            if (vI == vR)
            {
              Next(r.jump, false);
              return true;
            }
            break;
          case "bigger":
            if (vI > vR)
            {
              Next(r.jump, false);
              return true;
            }
            break;
        }
      }

      // Wenn keine Regel zutriff, wird ganz normal fortgesetzt.
      return false;
    }

    private void ApplyChanges(change[] changes)
    {
      if (changes == null)
        return;

      foreach (var c in changes)
      {
        // vT = Variable (Input) als String
        // vI = Variable (Input) als Int (cast von vT)
        // vC = Variable (Change) als Int
        // vR = Variable (Output) = Input (@operator) Change

        var vT = GetVar(c.id); // Variablen werden im Spiel immer als Text/String gespeichert
        var vI = string.IsNullOrEmpty(vT) ? 0 : int.Parse(vT); // Überprüfen ob Variable leer (0)
        var vC = int.Parse(c.value);
        var vR = 0; // muss hier benannt werden (scope von switch beachten)

        switch (c.@operator)
        {
          case "inc":
          case "add":
            vR = vI + vC;
            break;
          case "dec":
          case "sub":
            vR = vI - vC;
            break;
        }

        SetVar(c.id, vR.ToString());
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
          border.Background = new SolidColorBrush(Color.FromArgb(220, 255, 255, 255));
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
          _window.npc_talk.Children.Add(border);
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

    // Event
    public event EventHandler NullGameTask;
  }
}
