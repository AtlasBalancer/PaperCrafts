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
            ProjectStructure.CreateSubFolder(rootPath, SUBFOLDER_SCRIPTS);

            string subfolderPath = Path.Combine(rootPath, SUBFOLDER_SCRIPTS);

            ProjectStructure.CreateSubFolder(subfolderPath, "API");
            ProjectStructure.CreateSubFolder(subfolderPath, "Model");
            ProjectStructure.CreateSubFolder(subfolderPath, "Logic");
        }

        void CreateSubFolderMedia(string rootPath)
        {
            ProjectStructure.CreateSubFolder(rootPath, SUBFOLDER_MEDIA);

            string subfolderPath = Path.Combine(rootPath, SUBFOLDER_MEDIA);

            ProjectStructure.CreateSubFolder(subfolderPath, "Scenes");
            ProjectStructure.CreateSubFolder(subfolderPath, "Prefabs");
            ProjectStructure.CreateSubFolder(subfolderPath, "Materials");
            ProjectStructure.CreateSubFolder(subfolderPath, "Textures");
            ProjectStructure.CreateSubFolder(subfolderPath, "Audio");
        }
    }
}