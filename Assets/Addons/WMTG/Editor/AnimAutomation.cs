using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace WMTG.Editor {
    public static class AnimAutomation {
        
        [MenuItem("Assets/WMTG/AutoSet", false, 10)]
        static void AutoSetAnimation(MenuCommand menuCommand)
        {
            foreach(var obj in Selection.objects)
            {
                if (!(obj is GameObject gameObject)) continue;
                
                Debug.Log($"{gameObject.name}를 AutoSet합니다.");
                var importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(gameObject)) as ModelImporter;

                if (!importer || importer.defaultClipAnimations == null || importer.defaultClipAnimations.Length <= 0) continue;
                if (importer.animationType == ModelImporterAnimationType.Human || importer.animationType == ModelImporterAnimationType.Generic)
                {
                    ModelImporterClipAnimation[] modifiedClips = new ModelImporterClipAnimation[importer.defaultClipAnimations.Length];
                    var order = 0;
                    foreach ( var clip in importer.defaultClipAnimations)
                    {
                        clip.keepOriginalOrientation = true;
                        clip.keepOriginalPositionY = true;
                        clip.keepOriginalPositionXZ = true;
                        clip.lockRootRotation = true;
                        clip.lockRootHeightY = true;
                        modifiedClips[order] = clip;
                        order++;
                    }

                    importer.clipAnimations = modifiedClips;
                    importer.SaveAndReimport();
                }
                else
                {
                    Debug.LogError($"{gameObject.name}의 애니메이션이 Humanoid 또는 Generic으로 설정되지 않았습니다.");
                }
            }
        }
        [MenuItem("Assets/WMTG/SceneSet", false, 10)]
        static void SceneSetAnimation(MenuCommand menuCommand)
        {
            foreach(var obj in Selection.objects)
            {
                if (!(obj is GameObject gameObject)) continue;
                
                Debug.Log($"{gameObject.name}를 SceneSet합니다.");
                var importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(gameObject)) as ModelImporter;

                if (!importer || importer.defaultClipAnimations == null || importer.defaultClipAnimations.Length <= 0) continue;
                if (importer.animationType == ModelImporterAnimationType.Human || importer.animationType == ModelImporterAnimationType.Generic)
                {
                    ModelImporterClipAnimation[] modifiedClips = new ModelImporterClipAnimation[importer.defaultClipAnimations.Length];
                    var order = 0;
                    foreach ( var clip in importer.defaultClipAnimations)
                    {
                        clip.loopTime = true;
                        clip.keepOriginalOrientation = true;
                        clip.keepOriginalPositionY = true;
                        clip.keepOriginalPositionXZ = true;
                        clip.lockRootRotation = true;
                        clip.lockRootHeightY = true;
                        clip.lockRootPositionXZ = true;
                        modifiedClips[order] = clip;
                        order++;
                    }

                    importer.clipAnimations = modifiedClips;
                    importer.SaveAndReimport();
                }
                else
                {
                    Debug.LogError($"{gameObject.name}의 애니메이션이 Humanoid 또는 Generic으로 설정되지 않았습니다.");
                }
            }
        }
    }
}
