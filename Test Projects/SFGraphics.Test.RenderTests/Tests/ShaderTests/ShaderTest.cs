﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.GLObjects.Shaders;
using System.Collections.Generic;
using SFGraphics.GLObjects.Shaders.ShaderEventArgs;

namespace ShaderTests
{
    [TestClass]
    public abstract class ShaderTest : Tests.ContextTest
    {
        protected Shader shader;

        protected List<UniformSetEventArgs> invalidUniformSets = new List<UniformSetEventArgs>();
        protected List<UniformSetEventArgs> invalidTextureSets = new List<UniformSetEventArgs>();

        [TestInitialize()]
        public override void Initialize()
        {
            base.Initialize();

            if (shader == null)
            {
                shader = RenderTestUtils.ShaderTestUtils.CreateValidShader();
                shader.OnInvalidUniformSet += Shader_OnInvalidUniformSet;
                shader.OnTextureUnitTypeMismatch += Shader_OnTextureUnitTypeMismatch;
            }

            invalidUniformSets.Clear();
            invalidTextureSets.Clear();
        }

        private void Shader_OnTextureUnitTypeMismatch(Shader sender, UniformSetEventArgs e)
        {
            invalidTextureSets.Add(e);
        }

        private void Shader_OnInvalidUniformSet(Shader sender, UniformSetEventArgs e)
        {
            invalidUniformSets.Add(e);
        }
    }
}