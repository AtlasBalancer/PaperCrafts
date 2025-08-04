using System.IO;
using UnityEditor;
using UnityEngine;

namespace com.ab.papercrafts.editor
{
    public class CreateCommonModule : EditorWindow
    {
        const string WINDOW_TITLE = "Create common module structure";
        
        string _rootFolderName = "NewDomain";
        string _asmdefName = "com.ab.newdomain";
        
        [MenuItem("Assets/Create/Plugins/Project Structure/Create common module", false, 100)]
        public static void ShowWindow() => 
            ProjectStructure.ShowWindow<CreateCommonModule>(WINDOW_TITLE);
 
        
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

            ProjectStructure.CreateReadme(rootPath, _rootFolderName);

            string amsDefName = string.IsNullOrEmpty(_asmdefName) ? _rootFolderName : _asmdefName;
            ProjectStructure.CreateAsmDef(rootPath, amsDefName);

            AssetDatabase.Refresh();
            Debug.Log($"PaperCrafts:: Folder structure '{_rootFolderName}' created under {basePath}");
        }
    }
}