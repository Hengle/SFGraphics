﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;

namespace ShaderTests.SetterTests
{
    [TestClass]
    public class SetBoolToInt : ShaderTest
    {
        [TestMethod]
        public void ValidNameValidType()
        {
            shader.SetBoolToInt("boolInt1", true);
            Assert.IsTrue(IsValidSet("boolInt1", ActiveUniformType.Int));
        }

        [TestMethod]
        public void InvalidName()
        {
            shader.SetBoolToInt("memes", true);
            Assert.IsFalse(IsValidSet("memes", ActiveUniformType.Int));
        }

        [TestMethod]
        public void InvalidType()
        {
            shader.SetBoolToInt("float1", true);
            Assert.IsFalse(IsValidSet("float1", ActiveUniformType.Int));
        }
    }
}
