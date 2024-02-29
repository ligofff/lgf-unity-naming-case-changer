using System;
using System.Collections.Generic;
using System.Linq;
using CaseExtensions;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEditor;

namespace Editor
{
    public class NamingCaseChangerWindow : OdinEditorWindow
    {
        public enum CaseType
        {
            CamelCase,
            KebabCase,
            PascalCase,
            SnakeCase,
            TrainCase,
            NormalCase
        }

        [MenuItem("Ligofff/Naming Case Changer Window")]
        public static void OpenWindow()
        {
            GetWindow<NamingCaseChangerWindow>().Show();
        }
        
        [Serializable]
        public class PreviewChanges
        {
            public string oldName;
            public string newName;

            public PreviewChanges(string oldName, string newName)
            {
                this.oldName = oldName;
                this.newName = newName;
            }
        }
        
        [FolderPath]
        public string folderPath;

        public CaseType targetCaseType = CaseType.CamelCase;

        [TableList(IsReadOnly = true, AlwaysExpanded = true), ReadOnly]
        public List<PreviewChanges> previewChangesList = new List<PreviewChanges>()
        {
            new PreviewChanges("Sample Old Name 1", "Sample-Old-Name-1"),
            new PreviewChanges("Sample Old Name 2", "Sample-Old-Name-2"),
            new PreviewChanges("Sample Old Name 3", "Sample-Old-Name-3"),
            new PreviewChanges("Sample Old Name 4", "Sample-Old-Name-4"),
        };

        [Button, PropertyOrder(-1)]
        public void UpdatePreview()
        {
            previewChangesList.Clear();
            
            foreach (var asset in GetAssetsInFolder())
            {
                var assetName = asset.name;
                var changedAssetName = ChangeCase(assetName);
                
                previewChangesList.Add(new PreviewChanges(assetName, changedAssetName));
            }
        }

        [Button, PropertyOrder(-1)]
        public void RenameAll()
        {
            foreach (var asset in GetAssetsInFolder())
            {
                var assetName = asset.name;
                var changedAssetName = ChangeCase(assetName);

                AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(asset), changedAssetName);
            }
        }

        private string ChangeCase(string text)
        {
            switch (targetCaseType)
            {
                case CaseType.CamelCase:
                    return text.ToCamelCase();
                case CaseType.KebabCase:
                    return text.ToKebabCase();
                case CaseType.PascalCase:
                    return text.ToPascalCase();
                case CaseType.SnakeCase:
                    return text.ToCamelCase();
                case CaseType.TrainCase:
                    return text.ToTrainCase();
                case CaseType.NormalCase:
                    return text.ToNormalCase();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IEnumerable<UnityEngine.Object> GetAssetsInFolder()
        {
            return AssetDatabase.FindAssets("", new[] { folderPath })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>);
        }
    }
}