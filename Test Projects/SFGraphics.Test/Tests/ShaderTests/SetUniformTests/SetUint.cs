﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Shaders;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders.ShaderEventArgs;
using System.Collections.Generic;

namespace ShaderTests.SetterTests
{
    [TestClass]
    public class SetUint
    {
        private Shader shader;
        private List<UniformSetEventArgs> eventArgs = new List<UniformSetEventArgs>();

        [TestInitialize()]
        public void Initialize()
        {
            if (shader == null)
            {
                shader = RenderTestUtils.ShaderTestUtils.CreateValidShader();
                shader.OnInvalidUniformSet += Shader_OnInvalidUniformSet;
            }

            eventArgs.Clear();
        }

        private void Shader_OnInvalidUniformSet(Shader sender, UniformSetEventArgs e)
        {
            eventArgs.Add(e);
        }

        [TestMethod]
        public void ValidNameValidType()
        {
            shader.SetUint("uint1", 1);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("uint1", ActiveUniformType.UnsignedInt);
            Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(0, eventArgs.Count);
        }

        [TestMethod]
        public void InvalidName()
        {
            shader.SetUint("memes", 0);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("memes", ActiveUniformType.UnsignedInt);
            Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(1, eventArgs.Count);

        }

        [TestMethod]
        public void InvalidType()
        {
            shader.SetUint("float1", 0);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("float1", ActiveUniformType.UnsignedInt);
            Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(1, eventArgs.Count);
        }
    }
}