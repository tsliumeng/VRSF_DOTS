﻿using UnityEngine;
using UnityEditor;

namespace VRSF.Core.LaserPointer
{
    public class LaserPointerInstantiater : Editor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuCommand"></param>
        [MenuItem("GameObject/VRSF/Laser Pointer/Add Pointer (Without Laser)", priority = 2)]
        [MenuItem("VRSF/Laser Pointer/Add Pointer (Without Laser)", priority = 2)]
        private static void AddBasicPointer(MenuCommand menuCommand)
        {
            var pointerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/VRSF/Prefabs/Core/Pointer/BasicRaycastPointer.prefab");
            CreateGameObject(pointerPrefab, menuCommand);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuCommand"></param>
        [MenuItem("GameObject/VRSF/Laser Pointer/Add Laser Pointer", priority = 0)]
        [MenuItem("VRSF/Laser Pointer/Add Laser Pointer", priority = 0)]
        private static void AddLaserPointer(MenuCommand menuCommand)
        {
            GameObject pointerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/VRSF/Prefabs/Core/Pointer/LaserPointer.prefab");
            CreateGameObject(pointerPrefab, menuCommand);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="menuCommand"></param>
        [MenuItem("GameObject/VRSF/Laser Pointer/Add Laser Pointer With Click", priority = 0)]
        [MenuItem("VRSF/Laser Pointer/Add Laser Pointer With Click", priority = 0)]
        private static void AddLaserPointerWithClick(MenuCommand menuCommand)
        {
            GameObject pointerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/VRSF/Prefabs/Core/Pointer/LaserPointerWithClick.prefab");
            CreateGameObject(pointerPrefab, menuCommand);
        }

        private static void CreateGameObject(GameObject pointerPrefab, MenuCommand menuCommand)
        {
            // Create a custom game object
            GameObject pointer = PrefabUtility.InstantiatePrefab(pointerPrefab) as GameObject;

            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(pointer, "Create " + pointer.name);

            // Ensure it gets reparented if this was a context click (otherwise does nothing)
            GameObjectUtility.SetParentAndAlign(pointer, menuCommand.context as GameObject);

            Selection.activeObject = pointer;
        }
    }
}