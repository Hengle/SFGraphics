﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;

namespace SFGraphics.Test.RenderTests.ShaderTests.SetterTests
{
    public partial class ShaderTest
    {
        [TestClass]
        public class SetVector4
        {
            Shader shader;

            [TestInitialize()]
            public void Initialize()
            {
                shader = ShaderTestUtils.SetupContextCreateValidShader();
            }

            [TestMethod]
            public void SetVector4ValidName()
            {
                shader.SetVector4("vector4a", new Vector4(1));
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("vector4a", ActiveUniformType.FloatVec4);
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetVector4InvalidName()
            {
                shader.SetVector4("memes", new Vector4(1));
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("memes", ActiveUniformType.FloatVec4);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetVector4FloatsValidName()
            {
                shader.SetVector4("vector4a", 1, 1, 1, 1);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("vector4a", ActiveUniformType.FloatVec4);
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetVector4FloatsInvalidName()
            {
                shader.SetVector4("memes2", 1, 1, 1, 1);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("memes2", ActiveUniformType.FloatVec4);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetVector4InvalidType()
            {
                shader.SetVector4("float1", 1, 1, 1, 1);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("float1", ActiveUniformType.FloatVec4);
                Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            }

            [TestMethod]
            public void SetVector4ValidType()
            {
                shader.SetVector4("vector4a", 1, 1, 1, 1);
                string expected = ShaderTestUtils.GetInvalidUniformErrorMessage("vector4a", ActiveUniformType.FloatVec4);
                Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            }
        }
    }
}
