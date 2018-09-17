﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ShaderTests.SetterTests
{
    [TestClass]
    public class SetVector2 : ShaderTest
    {
        [TestMethod]
        public void ValidName()
        {
            shader.SetVector2("vector2a", new Vector2(1));
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("vector2a", ActiveUniformType.FloatVec2);
            Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(0, invalidUniformSets.Count);
        }

        [TestMethod]
        public void InvalidName()
        {
            shader.SetVector2("memes", new Vector2(1));
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("memes", ActiveUniformType.FloatVec2);
            Assert.IsTrue(shader.GetErrorLog().Contains(expected));
        }

        [TestMethod]
        public void FloatsValidName()
        {
            shader.SetVector2("vector2a", 1, 1);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("vector2a", ActiveUniformType.FloatVec2);
            Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(0, invalidUniformSets.Count);
        }

        [TestMethod]
        public void FloatsInvalidName()
        {
            shader.SetVector2("memes2", 1, 1);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("memes2", ActiveUniformType.FloatVec2);
            Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(1, invalidUniformSets.Count);
        }

        [TestMethod]
        public void InvalidType()
        {
            shader.SetVector2("float1", 1, 1);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("float1", ActiveUniformType.FloatVec2);
            Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(1, invalidUniformSets.Count);
        }
    }
}