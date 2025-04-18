using UnityEditor;
using UnityEngine;

namespace CustomBuildWebGL
{
    public static class SelectWebGlTemplate
    {
        [MenuItem("Helpers/WebGLTemplate")]
        private static void SelectTemplate()
        {
            EditorUtility.FocusProjectWindow();
            Object _object = AssetDatabase.LoadAssetAtPath(WebGLCustomBuild.IndexPath, typeof(Object));
            Selection.activeObject = _object;
        }
    }
}