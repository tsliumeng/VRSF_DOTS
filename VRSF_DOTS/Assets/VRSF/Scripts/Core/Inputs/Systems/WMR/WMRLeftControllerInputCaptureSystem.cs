﻿using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using VRSF.Core.Controllers;
using VRSF.Core.SetupVR;

namespace VRSF.Core.Inputs
{
    public class WMRLeftControllerInputCaptureSystem : JobComponentSystem
    {
        protected override void OnCreate()
        {
            OnSetupVRReady.Listeners += CheckForComponents;
            base.OnCreate();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            return new MenuInputCaptureJob()
            {
                MenuButtonDown = Input.GetButtonDown("WMRLeftMenuClick"),
                MenuButtonUp = Input.GetButtonUp("WMRLeftMenuClick")
            }.Schedule(this, inputDeps);
        }

        protected override void OnDestroy()
        {
            OnSetupVRReady.Listeners -= CheckForComponents;
            base.OnDestroy();
        }

        [Unity.Burst.BurstCompile]
        struct MenuInputCaptureJob : IJobForEach<MenuInputCapture>
        {
            public bool MenuButtonDown;
            public bool MenuButtonUp;

            public void Execute(ref MenuInputCapture menuInput)
            {
                if (menuInput.Hand == EHand.LEFT)
                {
                    // Check Click Events
                    if (MenuButtonDown)
                    {
                        menuInput.MenuClick = true;
                        new ButtonClickEvent(EHand.LEFT, EControllersButton.MENU);
                    }
                    else if (MenuButtonUp)
                    {
                        menuInput.MenuClick = false;
                        new ButtonUnclickEvent(EHand.LEFT, EControllersButton.MENU);
                    }
                }
            }
        }

        #region PRIVATE_METHODS
        /// <summary>
        /// Check if there's at least one MenuInputCapture component and that it has the LEFT as Hand
        /// </summary>
        /// <param name="info"></param>
        private void CheckForComponents(OnSetupVRReady info)
        {
            if (VRSF_Components.DeviceLoaded == EDevice.WMR)
            {
                var entityQuery = GetEntityQuery(typeof(MenuInputCapture)).ToComponentDataArray<MenuInputCapture>(Unity.Collections.Allocator.TempJob, out JobHandle jobHandle);
                if (entityQuery.Length > 0)
                {
                    foreach (var tic in entityQuery)
                    {
                        if (tic.Hand == EHand.LEFT)
                        {
                            this.Enabled = true;
                            jobHandle.Complete();
                            entityQuery.Dispose();
                            return;
                        }
                    }
                }
                jobHandle.Complete();
                entityQuery.Dispose();
            }
            this.Enabled = false;
        }
        #endregion PRIVATE_METHODS
    }
}