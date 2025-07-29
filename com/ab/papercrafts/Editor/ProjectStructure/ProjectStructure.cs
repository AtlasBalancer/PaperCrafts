using System.IO;
using UnityEditor;
using UnityEngine;

namespace com.ab.papercrafts.editor
{
    public class ProjectStructure : EditorWindow
    {
        public const string README_DESC = "Module description here.";
        public const string README_TAG = "Tag";

        public static void ShowWindow<TWindow>(
            string title,
            float width = 150f,
            float height = 80f)
            where TWindow : EditorWindow
        {
            TWindow window = CreateInstance<TWindow>();
            window.titleContent = new GUIContent(title);
            window.minSize = new Vector2(width, height);
            window.ShowUtility();
        }

        public static void CreateSubFolder(string parentPath, string folderName)
        {
            if (!AssetDatabase.IsValidFolder(Path.Combine(parentPath, folderName)))
                AssetDatabase.CreateFolder(parentPath, folderName);
        }

        public static string GetSelectedPathOrFallback()
        {
            string path = "Assets";
            foreach (Object obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
            {
                path = AssetDatabase.GetAssetPath(obj);
                if (File.Exists(path)) path = Path.GetDirectoryName(path);
                break;
            }

            return path;
        }

        public static void CreateReadme(
            string rootPath,
            string descriptionTag = README_TAG,
            string description = README_DESC)
        {
            string readmePath = Path.Combine(rootPath, "README.md");
            if (!File.Exists(readmePath))
            {
                File.WriteAllText(readmePath, $"# {descriptionTag}\n\n{description}");
            }
        }

        public static void CreateAsmDef(string targetPath, string name)
        {
            string asmdefPath = Path.Combine(targetPath, $"{name}.asmdef");
            if (!File.Exists(asmdefPath))
            {
                string json = $"{{\n  \"name\": \"{name}\"\n}}";
                File.WriteAllText(asmdefPath, json);
            }
        }
    }
}