﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;

namespace ShaderTests
{
    [TestClass]
    public class GetUniformBlockIndex : ShaderTest
    {
        [TestMethod]
        public void ValidName()
        {
            Assert.AreEqual(GL.GetUniformBlockIndex(shader.Id, "UniformBlock"), shader.GetUniformBlockIndex("UniformBlock"));
        }

        [TestMethod]
        public void InvalidName()
        {
            Assert.AreEqual(-1, shader.GetUniformBlockIndex("memes"));
        }

        [TestMethod]
        public void ShaderNotLinked()
        {
            var shader = new SFGraphics.GLObjects.Shaders.Shader();
            Assert.AreEqual(-1, shader.GetUniformBlockIndex("memes"));
        }
    }
}