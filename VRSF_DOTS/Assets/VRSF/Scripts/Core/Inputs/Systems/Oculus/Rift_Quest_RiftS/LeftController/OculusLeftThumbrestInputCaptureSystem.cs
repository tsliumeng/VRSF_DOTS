﻿using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using VRSF.Core.Controllers;
using VRSF.Core.SetupVR;

namespace VRSF.Core.Inputs
{
    /// <summary>
    /// 
    /// </summary>
    public class OculusLeftThumbrestInputCaptureSystem : JobComponentSystem
    {
        protected override void OnCreate()
        {
            OnSetupVRReady.Listeners += CheckDevice;
            base.OnCreate();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            return new ThumbrestButtonInputCaptureJob()
            {
                ThumbrestTouchButtonDown = Input.GetButtonDown("OculusLeftThumbrestTouch"),
                ThumbrestTouchButtonUp = Input.GetButtonUp("OculusLeftThumbrestTouch")
            }.Schedule(this, inputDeps);
        }

        protected override void OnDestroy()
        {
            OnSetupVRReady.Listeners -= CheckDevice;
            base.OnDestroy();
        }

        [Unity.Burst.BurstCompile]
        struct ThumbrestButtonInputCaptureJob : IJobForEach<ThumbrestInputCapture>
        {
            public bool ThumbrestTouchButtonDown;
            public bool ThumbrestTouchButtonUp;

            public void Execute(ref ThumbrestInputCapture thumbrestCapture)
            {
                if (thumbrestCapture.Hand == EHand.LEFT)
                {
                    if (ThumbrestTouchButtonDown)
                    {
                        thumbrestCapture.ThumbrestTouch = true;
                        new ButtonTouchEvent(EHand.LEFT, EControllersButton.THUMBREST);
                    }
                    else if (ThumbrestTouchButtonUp)
                    {
                        thumbrestCapture.ThumbrestTouch = false;
                        new ButtonUntouchEvent(EHand.LEFT, EControllersButton.THUMBREST);
                    }
                }
            }
        }

        #region PRIVATE_METHODS
        /// <summary>
        /// Check if we use the good device
        /// </summary>
        /// <param name="info"></param>
        private void CheckDevice(OnSetupVRReady info)
        {
            this.Enabled = IsOculusHeadset();

            bool IsOculusHeadset()
            {
                return VRSF_Components.DeviceLoaded == EDevice.OCULUS_RIFT || VRSF_Components.DeviceLoaded == EDevice.OCULUS_QUEST || VRSF_Components.DeviceLoaded == EDevice.OCULUS_RIFT_S;
            }
        }
        #endregion PRIVATE_METHODS
    }
}