﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Shaders;
using SFGraphics.GLObjects.Textures;

namespace ShaderTests
{
    [TestClass]
    public class ProgramValidateStatus
    {
        public static Shader shader;

        [TestInitialize()]
        public void Initialize()
        {
            // We can't share shaders between tests.
            shader = RenderTestUtils.ShaderTestUtils.SetUpContextCreateValidShader();
        }

        [TestMethod]
        public void ValidStatus()
        {
            shader.SetTexture("tex2D", new Texture2D(), 0);
            shader.SetTexture("texCube", new TextureCubeMap(), 1);

            Assert.IsTrue(shader.ValidateStatusIsOk);
        }

        [TestMethod]
        public void TwoTypesPerTextureUnit()
        {
            shader.SetTexture("tex2D", new Texture2D(), 0);
            shader.SetTexture("texCube", new TextureCubeMap(), 0);

            // There can only be a single texture type for each active texture.
            Assert.IsFalse(shader.ValidateStatusIsOk);
        }
    }
}
