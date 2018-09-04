﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Shaders;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.Test.RenderTests.ShaderTests.SetterTests
{
    public partial class ShaderTest
    {
        [TestClass]
        public class SetInt
        {
            Shader shader;

            [TestInitialize()]
            public void Initialize()
            {
                shader = ShaderTestUtils.SetupContextCreateValidFragShader();
            }

            [TestMethod]
            public void SetIntValidNameValidType()
            {
                shader.SetInt("int1", 0);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("int1", ActiveUniformType.Int);
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetIntInvalidName()
            {
                shader.SetInt("memes", 0);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("memes", ActiveUniformType.Int);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetIntInvalidType()
            {
                shader.SetInt("float1", 0);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("float1", ActiveUniformType.Int);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }
        }
    }
}