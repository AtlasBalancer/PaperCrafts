using System.IO;
using UnityEditor;
using UnityEditorInternal;
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
        
        const string DOMAIN = "domain";
        
        const string FOLDER_NAME_INPUT = "Folder name";
        const string ASMDEF_INPUT = "AsmDef name";

        string _rootFolderName = "CAModule";
        string _asmdefName = "com.ab.camodule";

        [MenuItem("Assets/Create/Plugins/Project Structure/Create clean architecture module", false, 100)]
        public static void ShowWindow() =>
            ProjectStructure.ShowWindow<CreateCleanArchitectureModule>(WINDOW_TITLE);

        void OnGUI()
        {
            GUILayout.Label("Enter common module name", EditorStyles.boldLabel);

            _rootFolderName = EditorGUILayout.TextField(FOLDER_NAME_INPUT, _rootFolderName);
            _asmdefName = EditorGUILayout.TextField(ASMDEF_INPUT, _asmdefName);

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
            string asmDefDomainName = $"{(string.IsNullOrEmpty(_asmdefName) ? _rootFolderName : _asmdefName)}.{DOMAIN}";
            string apiFolder = ProjectStructure.CreateFolder(rootPath, INFRASTRUCTURE_FOLDER);

            if (!AssetDatabase.IsValidFolder(rootPath))
                AssetDatabase.CreateFolder(basePath, _rootFolderName);

            ProjectStructure.CreateFolder(rootPath, DOMAIN_FOLDER);
            ProjectStructure.CreateFolder(rootPath, APLICATION_FLODER);
            ProjectStructure.CreateFolder(rootPath, PRESENTATION_FOLDER);

            ProjectStructure.CreateReadme(rootPath, _rootFolderName);
            AssemblyDefinitionAsset asmDefDomain = ProjectStructure.CreateAsmDef(rootPath, asmDefDomainName);
            ProjectStructure.CreateAsmDef(apiFolder, _asmdefName, new[] { asmDefDomain.name });

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"PaperCrafts:: Folder structure '{_rootFolderName}' created under {basePath}");
        }
    }
}