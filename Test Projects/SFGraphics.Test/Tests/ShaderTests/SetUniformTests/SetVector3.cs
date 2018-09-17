﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ShaderTests.SetterTests
{
    [TestClass]
    public class SetVector3 : ShaderTest
    {
        [TestMethod]
        public void ValidName()
        {
            shader.SetVector3("vector3a", new Vector3(1));
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("vector3a", ActiveUniformType.FloatVec3);
            Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(0, invalidUniformSets.Count);
        }

        [TestMethod]
        public void InvalidName()
        {
            shader.SetVector3("memes", new Vector3(1));
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("memes", ActiveUniformType.FloatVec3);
            Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(1, invalidUniformSets.Count);
        }

        [TestMethod]
        public void InvalidType()
        {
            shader.SetVector3("float1", 1, 1, 1);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("float1", ActiveUniformType.FloatVec3);
            Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(1, invalidUniformSets.Count);
        }

        [TestMethod]
        public void FloatsValidName()
        {
            shader.SetVector3("vector3a", 1, 1, 1);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("vector3a", ActiveUniformType.FloatVec3);
            Assert.IsFalse(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(0, invalidUniformSets.Count);
        }

        [TestMethod]
        public void FloatsInvalidName()
        {
            shader.SetVector3("memes2", 1, 1, 1);
            string expected = RenderTestUtils.ShaderTestUtils.GetInvalidUniformErrorMessage("memes2", ActiveUniformType.FloatVec3);
            Assert.IsTrue(shader.GetErrorLog().Contains(expected));
            Assert.AreEqual(1, invalidUniformSets.Count);
        }
    }
}