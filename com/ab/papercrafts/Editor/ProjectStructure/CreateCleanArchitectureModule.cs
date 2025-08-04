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
        const string APPLICATION_FLODER = "Application";
        const string PRESENTATION_FOLDER = "Presentation";
        const string INTEGRATIONS_FOLDER = "Integrations";

        const string DOMAIN = "domain";

        const string FOLDER_NAME_INPUT = "Folder name";
        const string ASMDEF_INPUT = "AsmDef name";

        const string MEDIA = "Media";
        const string SOURCES = "Src";
        const string DEFINITIONS = "Def";

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
            string apiFolder = ProjectStructure.CreateFolder(rootPath, INTEGRATIONS_FOLDER);

            if (!AssetDatabase.IsValidFolder(rootPath))
                AssetDatabase.CreateFolder(basePath, _rootFolderName);

            string domainPath = ProjectStructure.CreateFolder(rootPath, DOMAIN_FOLDER);
            ProjectStructure.CreateReadme(domainPath, $"{_asmdefName}/{DOMAIN_FOLDER}");

            string applicationPath = ProjectStructure.CreateFolder(rootPath, APPLICATION_FLODER);
            ProjectStructure.CreateReadme(applicationPath, $"{_asmdefName}/{APPLICATION_FLODER}");

            string presentationPath = ProjectStructure.CreateFolder(rootPath, PRESENTATION_FOLDER);
            ProjectStructure.CreateReadme(presentationPath, $"{_asmdefName}/{PRESENTATION_FOLDER}");

            ProjectStructure.CreateReadme(rootPath, $"{_rootFolderName}\n#{_asmdefName}");
            AssemblyDefinitionAsset asmDefDomain = ProjectStructure.CreateAsmDef(rootPath, asmDefDomainName);
            ProjectStructure.CreateAsmDef(apiFolder, _asmdefName, new[] { asmDefDomain.name });
            ProjectStructure.CreateReadme(apiFolder, $"{_asmdefName}/{INTEGRATIONS_FOLDER}");
            CreateApiSubFolders(apiFolder);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log(
                $"PaperCrafts::{nameof(CreateCleanArchitectureModule)}: <color=green>creation module successfully completed</color>");
        }

        static void CreateApiSubFolders(string apiFolder)
        {
            ProjectStructure.CreateReadme(ProjectStructure.CreateFolder(apiFolder, MEDIA));
            ProjectStructure.CreateReadme(ProjectStructure.CreateFolder(apiFolder, SOURCES));
            ProjectStructure.CreateReadme(ProjectStructure.CreateFolder(apiFolder, DEFINITIONS));
        }
    }
}