using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace com.ab.papercrafts.editor
{
    public class ProjectStructure : EditorWindow
    {
        public const string README_DESC = "Module description here.";
        public const string README_TAG = "Tag";

        public static void ShowWindow<TWindow>(
            string title,
            float width = 300f,
            float height = 180f)
            where TWindow : EditorWindow
        {
            TWindow window = CreateInstance<TWindow>();
            window.titleContent = new GUIContent(title);
            window.minSize = new Vector2(width, height);
            window.position = new Rect(new Vector2(0, 0), new Vector2(width, height));
            window.ShowUtility();
        }


        public static string CreateFolder(string parentPath, string folderName)
        {
            string path = Path.Combine(parentPath, folderName);

            if (!AssetDatabase.IsValidFolder(path))
                AssetDatabase.CreateFolder(parentPath, folderName);

            return path;
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
            string tag = README_TAG,
            string description = README_DESC)
        {
            string readmePath = Path.Combine(rootPath, "README.md");
            if (!File.Exists(readmePath))
            {
                File.WriteAllText(readmePath, $"#{tag}\n\n{description}");
            }
        }

        public static AssemblyDefinitionAsset CreateAsmDef(string folderPath, string asmDefName,
            string[] references = null)
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string asmdefPath = Path.Combine(folderPath, asmDefName + ".asmdef");
            AssemblyDefinition asmdef = new AssemblyDefinition
            {
                name = asmDefName,
                rootNamespace = asmDefName,
                references = references ?? new string[0],
                optionalUnityReferences = new string[0],
                includePlatforms = new string[0],
                excludePlatforms = new string[0],
                allowUnsafeCode = false,
                overrideReferences = false,
                precompiledReferences = new string[0],
                autoReferenced = true,
                defineConstraints = new string[0],
                versionDefines = new AssemblyDefinitionVersionDefine[0],
                noEngineReferences = false
            };

            string json = JsonUtility.ToJson(asmdef, true);
            File.WriteAllText(asmdefPath, json);
            AssetDatabase.ImportAsset(asmdefPath);

            return AssetDatabase.LoadAssetAtPath<AssemblyDefinitionAsset>(asmdefPath);
        }


        [Serializable]
        class AssemblyDefinition
        {
            public string name;
            public string rootNamespace;
            public string[] references;
            public string[] optionalUnityReferences;
            public string[] includePlatforms;
            public string[] excludePlatforms;
            public bool allowUnsafeCode;
            public bool overrideReferences;
            public string[] precompiledReferences;
            public bool autoReferenced;
            public string[] defineConstraints;
            public AssemblyDefinitionVersionDefine[] versionDefines;
            public bool noEngineReferences;
        }

        [Serializable]
        class AssemblyDefinitionVersionDefine
        {
            public string name;
            public string expression;
            public string define;
        }
    }
}