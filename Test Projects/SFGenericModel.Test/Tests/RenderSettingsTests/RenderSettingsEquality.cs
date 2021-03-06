﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGenericModel.RenderState;

namespace RenderSettingsTests
{
    [TestClass]
    public class RenderSettingsEquality
    {
        [TestMethod]
        public void SameObject()
        {
            RenderSettings settings = new RenderSettings();
            Assert.IsTrue(settings.Equals(settings));
        }

        [TestMethod]
        public void DefaultSettings()
        {
            RenderSettings settings = new RenderSettings();
            RenderSettings settings2 = new RenderSettings();

            Assert.IsTrue(settings.Equals(settings2));
        }

        [TestMethod]
        public void DifferentAlphaTest()
        {
            RenderSettings settings = new RenderSettings();
            RenderSettings settings2 = new RenderSettings();
            settings2.alphaTestSettings = new AlphaTestSettings(true, OpenTK.Graphics.OpenGL.AlphaFunction.Gequal, 0.5f);
            Assert.IsFalse(settings.Equals(settings2));
        }

        [TestMethod]
        public void DifferentFaceCull()
        {
            RenderSettings settings = new RenderSettings();
            RenderSettings settings2 = new RenderSettings();
            settings2.faceCullingSettings = new FaceCullingSettings(false, OpenTK.Graphics.OpenGL.CullFaceMode.Back);

            Assert.IsFalse(settings.Equals(settings2));
        }

        [TestMethod]
        public void DifferentAlphaBlend()
        {
            RenderSettings settings = new RenderSettings();
            RenderSettings settings2 = new RenderSettings();
            settings2.alphaBlendSettings.enabled = !settings.alphaBlendSettings.enabled;

            Assert.IsFalse(settings.Equals(settings2));
        }

        [TestMethod]
        public void DifferentDepthTest()
        {
            RenderSettings settings = new RenderSettings();
            RenderSettings settings2 = new RenderSettings();
            settings2.depthTestSettings = new DepthTestSettings(false, true, OpenTK.Graphics.OpenGL.DepthFunction.Lequal);

            Assert.IsFalse(settings.Equals(settings2));
        }
    }
}
