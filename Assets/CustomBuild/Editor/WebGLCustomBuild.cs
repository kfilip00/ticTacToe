using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace CustomBuildWebGL
{
    public static class WebGLCustomBuild
    {
        private const string BUILD_FOLDER_NAME = "Build";

        public static string IndexPath => AssetDatabase.GetAssetPath(Resources.Load<TextAsset>("WebTemplate/index"));
        public static string RootFolder => IndexPath.Replace("/index.html", string.Empty);
        
        [PostProcessBuild(1)]
        public static void OnPostprocessBuild(BuildTarget _target, string _destinationPath)
        {
            if (_target != BuildTarget.WebGL)
            {
                return;
            }

            DeleteEverythingButBuildFolder(_destinationPath);
            CopyEverything(_destinationPath);
            DeleteMetaFiles(_destinationPath);
            RenameFiles(_destinationPath);
        }

        private static void DeleteEverythingButBuildFolder(string _destinationPath)
        {
            DirectoryInfo _destinationDir = new DirectoryInfo(_destinationPath);
            foreach (FileSystemInfo _fsInfo in _destinationDir.GetFileSystemInfos())
            {
                if (_fsInfo.Name == BUILD_FOLDER_NAME)
                {
                    continue;
                }

                if (_fsInfo is DirectoryInfo _info)
                {
                    _info.Delete(true);
                }
                else
                {
                    _fsInfo.Delete();
                }
            }
        }

        private static void CopyEverything(string _destinationPath)
        {
            string _templateFolderPath = WebGLCustomBuild.RootFolder;
            if (Directory.Exists(_templateFolderPath))
            {
                CopyDirectory(_templateFolderPath, _destinationPath);
            }
            else
            {
                Debug.LogError($"Template folder not found at {_templateFolderPath}");
            }
        }

        private static void CopyDirectory(string _sourceDir, string _destinationDir)
        {
            if (!Directory.Exists(_destinationDir))
            {
                Directory.CreateDirectory(_destinationDir);
            }

            foreach (string _filePath in Directory.GetFiles(_sourceDir))
            {
                string _fileName = Path.GetFileName(_filePath);
                string _destFilePath = Path.Combine(_destinationDir, _fileName);
                File.Copy(_filePath, _destFilePath, true);
            }

            foreach (string _dirPath in Directory.GetDirectories(_sourceDir))
            {
                string _dirName = Path.GetFileName(_dirPath);
                string _destDirPath = Path.Combine(_destinationDir, _dirName);
                CopyDirectory(_dirPath, _destDirPath);
            }
        }

        private static void DeleteMetaFiles(string _directoryPath)
        {
            foreach (string _filePath in Directory.GetFiles(_directoryPath, "*.meta", SearchOption.AllDirectories))
            {
                File.Delete(_filePath);
            }
        }

        private static void RenameFiles(string _destinationPath)
        {
            string _buildFolderPath = Path.Combine(_destinationPath, BUILD_FOLDER_NAME);
            if (Directory.Exists(_buildFolderPath))
            {
                RenameFilesInBuildFolder(_buildFolderPath);
            }
            else
            {
                Logger.Log("Build folder not found; skipping file renaming.");
            }
        }

        private static void RenameFilesInBuildFolder(string _buildFolderPath)
        {
            DirectoryInfo _buildDir = new DirectoryInfo(_buildFolderPath);
            foreach (FileInfo _file in _buildDir.GetFiles("*", SearchOption.TopDirectoryOnly))
            {
                string _extension = string.Empty;
                bool _shouldNote = false;
                foreach (var _character in _file.Name)
                {
                    if (_character == '.')
                    {
                        _shouldNote = true;
                    }

                    if (_shouldNote)
                    {
                        _extension += _character;
                    }
                }

                string _newFileName = $"{Application.productName}{_extension}";
                if (_file.DirectoryName == null)
                {
                    continue;
                }
                
                string _newFilePath = Path.Combine(_file.DirectoryName, _newFileName);
                _file.MoveTo(_newFilePath);
            }
        }
    }
}