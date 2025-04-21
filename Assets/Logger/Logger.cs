using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LoggerNS;
using Newtonsoft.Json;
using Type = LoggerNS.Type;

public class Logger : MonoBehaviour
{
   public static string FilePath => Path.Combine(Application.persistentDataPath, "log.txt");
   
   [SerializeField] private List<Setup> logSetups;
   private static Logger instance;

   private void Awake()
   {
      if (instance == null)
      {
         instance = this;
         DontDestroyOnLoad(gameObject);
         LogIntoFile("\n\n\n********* New Session *********\n\n\n");
      }
      else
      {
         Destroy(gameObject);
      }
   }

   public static void Log(object _object)
   {
      Log(JsonConvert.SerializeObject(_object));
   }

   public static void Log(string _message, Category _category = Category.Default, GameObject _sender = null, Type _type = Type.Normal)
   {
      Setup _setup = GetSetup(_category);

      if (_setup == null)
      {
         return;
      }

      if (!_setup.Allowed)
      {
         return;
      }

      string _log = GetText(_setup, _message);

      switch (_type)
      {
         case Type.Normal:
            Debug.Log(_log, _sender);
            break;
         case Type.Warning:
            Debug.LogWarning(_log, _sender);
            break;
         case Type.Error:
            Debug.LogError(_log, _sender);
            break;
         default:
            throw new ArgumentOutOfRangeException();
      }
   }

   public void ManageSetups(bool _allowed)
   {
      foreach (Setup _setup in logSetups)
      {
         _setup.SetAllowed(_allowed);
      }
   }

   public static void LogIntoFile(string _message, Category _category = Category.Default)
   {
      if (instance == null)
      {
         Debug.LogError("Logger instance not initialized.");
         return;
      }

      Setup _setup = GetSetup(_category);

      if (_setup == null)
      {
         return;
      }

      if (!_setup.Allowed)
      {
         return;
      }

      string _log = GetText(_setup, _message);
      _log = $"{DateTime.Now}: {_log}\n\n---------------";
      try
      {
         File.AppendAllText(FilePath, _log + System.Environment.NewLine);
      }
      catch (Exception _e)
      {
         Debug.LogError($"Failed to write to log file: {_e.Message}");
      }
   }

   private static Setup GetSetup(Category _category)
   {
      foreach (var _allowedLog in instance.logSetups)
      {
         if (_allowedLog.Category != _category)
         {
            continue;
         }

         return _allowedLog;
      }

      Debug.LogError("Didn't find log setup for category: " + _category);
      return null;
   }

   private static string GetText(Setup _setup, string _message)
   {
      string _prefix = GetPrefix(_setup);
      string _color = ColorUtility.ToHtmlStringRGB(_setup.Color);
      string _log = FormatText(_prefix, _message,_color);

      return _log;
   }

   private static string GetPrefix(Setup _setup)
   {
      var  _category = _setup.Category;
      return string.IsNullOrEmpty(_setup.Prefix) ? _category.ToString() : _setup.Prefix;
   }

   private static string FormatText(string _prefix, string _message, string _color)
   {
      return $"<color=#{_color}>{_prefix}:</color> {_message}";   
   }
}