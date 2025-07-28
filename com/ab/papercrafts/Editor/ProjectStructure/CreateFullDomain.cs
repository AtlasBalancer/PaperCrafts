#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
#endif

namespace com.ab.papercrafts.editor
{
    public class CreateFullDomain : EditorWindow
    {
#if UNITY_EDITOR
        const string SUBFOLDER_MEDIA = "Media";
        const string SUBFOLDER_SCRIPTS = "Scripts";

        string _rootFolderName = "NewDomain";
        string _asmdefName = "com.ab.newdomain";

        [MenuItem("Assets/Create/Plugins/Project Structure/Create full domain", false, 100)]
        public static void ShowWindow()
        {
            CreateFullDomain window = CreateInstance<CreateFullDomain>();
            window.titleContent = new GUIContent("Create Folder Structure");
            window.minSize = new Vector2(300, 80);
            window.ShowUtility();
        }

        void OnGUI()
        {
            GUILayout.Label("Enter domain folder name", EditorStyles.boldLabel);
            _rootFolderName = EditorGUILayout.TextField("Folder name", _rootFolderName);
            _asmdefName = EditorGUILayout.TextField("AsmDef name", _asmdefName);
            GUILayout.Space(10);
            if (GUILayout.Button("Create"))
            {
                CreateStructure();
                Close();
            }
        }

        void CreateStructure()
        {
            string basePath = GetSelectedPathOrFallback();
            string rootPath = Path.Combine(basePath, _rootFolderName);

            if (!AssetDatabase.IsValidFolder(rootPath))
                AssetDatabase.CreateFolder(basePath, _rootFolderName);

            CreateSubFolderMedia(rootPath);
            CreateSubFolderScripts(rootPath);

            CreateReadme(rootPath);

            string amsDefName = string.IsNullOrEmpty(_asmdefName) ? _rootFolderName : _asmdefName;
            CreateAsmDef(rootPath, amsDefName);

            AssetDatabase.Refresh();
            Debug.Log($"PaperCrafts:: Folder structure '{_rootFolderName}' created under {basePath}");
        }

        void CreateSubFolderScripts(string rootPath)
        {
            CreateSubFolder(rootPath, SUBFOLDER_SCRIPTS);

            string subfolderPath = Path.Combine(rootPath, SUBFOLDER_SCRIPTS);

            CreateSubFolder(subfolderPath, "API");
            CreateSubFolder(subfolderPath, "Model");
            CreateSubFolder(subfolderPath, "Logic");
        }

        void CreateSubFolderMedia(string rootPath)
        {
            CreateSubFolder(rootPath, SUBFOLDER_MEDIA);

            string subfolderPath = Path.Combine(rootPath, SUBFOLDER_MEDIA);

            CreateSubFolder(subfolderPath, "Scenes");
            CreateSubFolder(subfolderPath, "Prefabs");
            CreateSubFolder(subfolderPath, "Materials");
            CreateSubFolder(subfolderPath, "Textures");
            CreateSubFolder(subfolderPath, "Audio");
        }

        void CreateSubFolder(string parentPath, string folderName)
        {
            if (!AssetDatabase.IsValidFolder(Path.Combine(parentPath, folderName)))
                AssetDatabase.CreateFolder(parentPath, folderName);
        }

        static string GetSelectedPathOrFallback()
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

        void CreateReadme(string rootPath)
        {
            string readmePath = Path.Combine(rootPath, "README.md");
            if (!File.Exists(readmePath))
            {
                File.WriteAllText(readmePath, $"# {_rootFolderName}\n\nModule description here.");
            }
        }

        void CreateAsmDef(string targetPath, string name)
        {
            string asmdefPath = Path.Combine(targetPath, $"{name}.asmdef");
            if (!File.Exists(asmdefPath))
            {
                string json = $"{{\n  \"name\": \"{name}\"\n}}";
                File.WriteAllText(asmdefPath, json);
            }
        }
#endif
    }
}