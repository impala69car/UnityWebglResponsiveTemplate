using System;
using System.Collections.Generic;
using NWH.VehiclePhysics2.Powertrain;
using UnityEngine;

namespace NWH.VehiclePhysics2.Effects
{
    [Serializable]
    public partial class SurfaceParticleManager : Effect
    {
        /// <summary>
        ///     How much will lateral slip contribute to the particle emission.
        ///     Ignored when particle type for the surface is set to other than Smoke.
        /// </summary>
        [Range(0, 5)]
        [Tooltip(
            "How much will lateral slip contribute to the particle emission.\r\nIgnored when particle type for the surface is set to other than Smoke.")]
        public float lateralSlipParticleCoeff = 1f;

        /// <summary>
        ///     How much will longitudinal slip contribute to the particle emission.
        ///     Ignored when particle type for the surface is set to other than Smoke.
        /// </summary>
        [Range(0, 5)]
        [Tooltip(
            "How much will longitudinal slip contribute to the particle emission.\r\nIgnored when particle type for the surface is set to other than Smoke.")]
        public float longitudinalSlipParticleCoeff = 1f;

        /// <summary>
        ///     Particle size multiplier specific to this vehicle.
        ///     Use to adjust particle size on per-vehicle basis.
        ///     For global particle size adjustment for individual surfaces check SurfacePresets.
        /// </summary>
        [Range(0, 2)]
        [Tooltip(
            "Particle size multiplier specific to this vehicle.\r\nUse to adjust particle size on per-vehicle basis.\r\nFor global particle size adjustment for individual surfaces check SurfacePresets.")]
        public float particleSizeCoeff = 1f;

        /// <summary>
        ///     Emission rate multiplier specific to this vehicle.
        ///     Use to adjust emission on per-vehicle basis.
        ///     For global emission adjustment for individual surfaces check SurfacePresets.
        /// </summary>
        [Range(0, 2)]
        [Tooltip(
            "Emission rate multiplier specific to this vehicle.\r\nUse to adjust emission on per-vehicle basis.\r\nFor global emission adjustment for individual surfaces check SurfacePresets.")]
        public float emissionRateCoeff = 1f;

        /// <summary>
        ///     When enabled the particle system will either emit or not emit, with no in-between. Also removes any smoothing.
        /// </summary>
        [Tooltip(
            "When enabled the particle system will either emit or not emit, with no in-between. Also removes any smoothing.")]
        public bool binaryEmission;

        /// <summary>
        ///     Current particle count for all surface particle systems.
        /// </summary>
        [Tooltip("    Current particle count for all surface particle systems.")]
        public int particleCount;

        [SerializeField]
        private List<SurfaceParticleSystem> particleSystems = new List<SurfaceParticleSystem>();


        public override void Initialize()
        {
            foreach (WheelComponent wheelWrapper in vc.Wheels)
            {
                SurfaceParticleSystem particle = new SurfaceParticleSystem();
                particle.Initialize(vc, wheelWrapper);
                particleSystems.Add(particle);
            }

            base.Initialize();
        }


        public override void Update()
        {
            particleCount = 0;

            if (!Active)
            {
                return;
            }

            foreach (SurfaceParticleSystem sps in particleSystems)
            {
                sps.longitudinalSlipCoeff = longitudinalSlipParticleCoeff;
                sps.lateralSlipCoeff      = lateralSlipParticleCoeff;
                sps.particleSizeCoeff     = particleSizeCoeff;
                sps.emissionRateCoeff     = emissionRateCoeff;
                sps.binaryEmission        = binaryEmission;
                sps.Update();
                particleCount += sps.particleCount;
            }
        }


        public override void FixedUpdate()
        {
        }


        public override void Enable()
        {
            base.Enable();

            foreach (SurfaceParticleSystem sps in particleSystems)
            {
                sps.Enable();
            }
        }


        public override void Disable()
        {
            base.Disable();

            foreach (SurfaceParticleSystem sps in particleSystems)
            {
                sps.Disable();
            }
        }
    }
}