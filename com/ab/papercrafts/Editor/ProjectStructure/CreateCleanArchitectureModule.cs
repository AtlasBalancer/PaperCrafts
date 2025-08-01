using System.IO;
using UnityEditor;
using UnityEngine;

namespace com.ab.papercrafts.editor
{
    public class CreateCleanArchitectureModule : EditorWindow
    {
        const string WINDOW_TITLE = "Create clean architecture module structure";
        
        const string DOMAIN_FOLDER = "Domain";
        const string APLICATION_FLODER = "Aplication";
        const string PRESENTATION_FOLDER = "Presentation";
        const string INFRASTRUCTURE_FOLDER = "Infrastructure";
        
        string _rootFolderName = "CAModule";
        string _asmdefName = "com.ab.camodule";
        
        [MenuItem("Assets/Create/Plugins/Project Structure/Create clean architecture module", false, 100)]
        public static void ShowWindow() => 
            ProjectStructure.ShowWindow<CreateCleanArchitectureModule>(WINDOW_TITLE);
        
        void OnGUI()
        {
            GUILayout.Label("Enter common module name", EditorStyles.boldLabel);
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

            ProjectStructure.CreateFolder(rootPath, DOMAIN_FOLDER);
            ProjectStructure.CreateFolder(rootPath, APLICATION_FLODER);
            ProjectStructure.CreateFolder(rootPath, PRESENTATION_FOLDER);
            ProjectStructure.CreateFolder(rootPath, INFRASTRUCTURE_FOLDER);
            
            ProjectStructure.CreateReadme(rootPath, _rootFolderName);

            string amsDefName = string.IsNullOrEmpty(_asmdefName) ? _rootFolderName : _asmdefName;
            ProjectStructure.CreateAsmDef(rootPath, amsDefName);

            AssetDatabase.Refresh();
            Debug.Log($"PaperCrafts:: Folder structure '{_rootFolderName}' created under {basePath}");
        }
    }
}