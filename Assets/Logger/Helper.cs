using System;
using System.IO;
using NaughtyAttributes;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LoggerNS
{
    public class Helper : MonoBehaviour
    {
        [SerializeField] private Logger logger;

        private void OnValidate()
        {
            if (logger != null)
            {
                return;
            }

            logger = GetComponent<Logger>();
        }

        [Button]
        private void AllowEverything()
        {
            if (logger == null)
            {
                return;
            }

            logger.ManageSetups(true);
            SaveChanges();
        }

        [Button]
        private void DisallowEverything()
        {
            if (logger == null)
            {
                return;
            }

            logger.ManageSetups(false);
            SaveChanges();
        }

        private void SaveChanges()
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(logger.gameObject);
#endif
        }

        [Button]
        private void OpenLogFile()
        {
            Application.OpenURL(Logger.FilePath);
        }

        [Button]
        private void ClearLogFile()
        {
            try
            {
                File.WriteAllText(Logger.FilePath,string.Empty);
            }
            catch (Exception _e)
            {
                Debug.LogError($"Failed to write to log file: {_e.Message}");
            }
        }
    }
}