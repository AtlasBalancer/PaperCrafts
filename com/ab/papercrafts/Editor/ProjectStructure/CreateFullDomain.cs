using System.IO;
using UnityEditor;
using UnityEngine;

namespace com.ab.papercrafts.editor
{
    public class CreateFullDomain : EditorWindow
    {
        const string SUBFOLDER_MEDIA = "Media";
        const string SUBFOLDER_SCRIPTS = "Scripts";
        const string WINDOW_TITLE = "Create domain structure";

        string _rootFolderName = "NewDomain";
        string _asmdefName = "com.ab.newdomain";

        [MenuItem("Assets/Create/Plugins/Project Structure/Create full domain", false, 100)]
        public static void ShowWindow() => 
            ProjectStructure.ShowWindow<CreateFullDomain>(WINDOW_TITLE);

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
            string basePath = ProjectStructure.GetSelectedPathOrFallback();
            string rootPath = Path.Combine(basePath, _rootFolderName);

            if (!AssetDatabase.IsValidFolder(rootPath))
                AssetDatabase.CreateFolder(basePath, _rootFolderName);

            CreateSubFolderMedia(rootPath);
            CreateSubFolderScripts(rootPath);

            ProjectStructure.CreateReadme(rootPath);

            string amsDefName = string.IsNullOrEmpty(_asmdefName) ? _rootFolderName : _asmdefName;
            ProjectStructure.CreateAsmDef(rootPath, amsDefName);

            AssetDatabase.Refresh();
            Debug.Log($"PaperCrafts:: Folder structure '{_rootFolderName}' created under {basePath}");
        }

        void CreateSubFolderScripts(string rootPath)
        {
            ProjectStructure.CreateFolder(rootPath, SUBFOLDER_SCRIPTS);

            string subfolderPath = Path.Combine(rootPath, SUBFOLDER_SCRIPTS);

            ProjectStructure.CreateFolder(subfolderPath, "API");
            ProjectStructure.CreateFolder(subfolderPath, "Model");
            ProjectStructure.CreateFolder(subfolderPath, "Logic");
        }

        void CreateSubFolderMedia(string rootPath)
        {
            ProjectStructure.CreateFolder(rootPath, SUBFOLDER_MEDIA);

            string subfolderPath = Path.Combine(rootPath, SUBFOLDER_MEDIA);

            ProjectStructure.CreateFolder(subfolderPath, "Scenes");
            ProjectStructure.CreateFolder(subfolderPath, "Prefabs");
            ProjectStructure.CreateFolder(subfolderPath, "Materials");
            ProjectStructure.CreateFolder(subfolderPath, "Textures");
            ProjectStructure.CreateFolder(subfolderPath, "Audio");
        }
    }
}