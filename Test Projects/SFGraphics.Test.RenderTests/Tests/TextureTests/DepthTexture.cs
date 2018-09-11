﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Textures;
using OpenTK.Graphics.OpenGL;


namespace SFGraphics.Test.RenderTests.TextureTests
{
    [TestClass]
    public class ConstructorTestsDepth
    {
        private static readonly List<byte[]> mipmaps = new List<byte[]>();

        [TestInitialize()]
        public void Initialize()
        {
            // Set up the context for all the tests.
            TestTools.OpenTKWindowlessContext.BindDummyContext();
            // Binding a pixel unpack buffer affects texture loading methods.
            GL.BindBuffer(BufferTarget.PixelUnpackBuffer, 0);
        }

        [TestMethod]
        public void DepthFormat()
        {
            // Doesn't throw an exception.
            DepthTexture texture = new DepthTexture(1, 1, PixelInternalFormat.DepthComponent);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NotDepthFormat()
        {
            DepthTexture texture = new DepthTexture(1, 1, PixelInternalFormat.Rgba);
        }
    }
}