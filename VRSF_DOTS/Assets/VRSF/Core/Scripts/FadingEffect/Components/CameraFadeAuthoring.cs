﻿using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using VRSF.Core.Utils;

namespace VRSF.Core.FadingEffect
{
    /// <summary>
    /// Component handling the fading effects. Need to be placed a canvas with an Image in front of the user's eyes
    /// </summary>
    [RequiresEntityConversion]
    public class CameraFadeAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        [Header("Fading Parameters")]
        /// <summary>
        ///  How long, in seconds, the fade-in/fade-out animation should take
        /// </summary>
        [Tooltip("Duration of the \"blink\" animation (fading in and out upon teleport) in seconds.")]
        public float FadingSpeed = 1;
        /// <summary>
        ///  How long, in seconds, the fade-in/fade-out animation should take
        /// </summary>
        [Tooltip("Whether a Fade In effect should take place when the OnSetupVRReady is called.")]
        public bool FadeInOnSetupVRReady = true;

        [Header("Required Fading Components")]
        [Tooltip("Plane Mesh used to fade")]
        [SerializeField] private Mesh _mesh;
        [Tooltip("Plane Mesh used to fade")]
        [SerializeField] private Material _fadeMaterial;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            _fadeMaterial.color = new Color(_fadeMaterial.color.r, _fadeMaterial.color.g, _fadeMaterial.color.b, FadeInOnSetupVRReady ? 1.0f : 0.0f);

            dstManager.AddSharedComponentData(entity, new RenderMesh()
            {
                mesh = _mesh,
                material = _fadeMaterial
            });
            
            dstManager.AddComponentData(entity, new CameraFadeParameters()
            {
                FadingSpeed = FadingSpeed,
                ShouldImmediatlyFadeIn = false,
                OldFadingSpeedFactor = FadingSpeed,
                FadeInOnSceneLoaded = FadeInOnSetupVRReady
            });

            dstManager.AddComponentData(entity, new DestroyOnSceneUnloaded());

#if UNITY_EDITOR
            // Set it's name in Editor Mode for the Entity Debugger Window
            dstManager.SetName(entity, "Camera Fade Entity");
#endif
        }
    }
} 