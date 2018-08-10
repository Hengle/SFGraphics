﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Textures;
using OpenTK.Graphics.OpenGL;


namespace SFGraphicsRenderTests.TextureTests
{
    public partial class TextureTest
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
            }

            [TestMethod]
            public void CorrectFormat()
            {
                // Doesn't throw an exception.
                DepthTexture texture = new DepthTexture(1, 1, PixelInternalFormat.DepthComponent);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void IncorrectFormat()
            {
                DepthTexture texture = new DepthTexture(1, 1, PixelInternalFormat.Rgba);
            }
        }
    }
}