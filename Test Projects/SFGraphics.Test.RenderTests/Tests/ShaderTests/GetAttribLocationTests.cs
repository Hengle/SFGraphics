﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;

namespace SFGraphics.Test.RenderTests.ShaderTests
{
    [TestClass]
    public class GetAttribLocationTests
    {
        public static Shader shader;

        [TestInitialize()]
        public void Initialize()
        {
            shader = ShaderTestUtils.SetupContextCreateValidShader();
            // Allow for setting uniforms.
            shader.UseProgram();
        }

        [TestMethod]
        public void GetAttribLocationValidName()
        {
            Assert.AreEqual(GL.GetAttribLocation(shader.Id, "position"), shader.GetAttribLocation("position"));
        }

        [TestMethod]
        public void GetAttribLocationInvalidName()
        {
            Assert.AreEqual(GL.GetAttribLocation(shader.Id, "memes"), shader.GetAttribLocation("memes"));
        }
    }
}
