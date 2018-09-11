﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;


namespace SFGraphics.Test.RenderTests.ShaderTests.ProgramCreationTests
{
    public partial class ShaderTest
    {
        [TestClass]
        public class ValidInvalidFragShader
        {
            [TestInitialize()]
            public void Initialize()
            {
                // Set up the context for all the tests.
                TestTools.OpenTKWindowlessContext.BindDummyContext();
            }

            [TestMethod]
            public void ValidAndInvalidFragShader()
            {
                // Load the shader file from the embedded resources.
                Shader shader = new Shader();
                string shaderSource = TestTools.ResourceShaders.GetShader("SFGraphics.Test.RenderTests.Shaders.validFrag.frag");
                shader.LoadShader(shaderSource, ShaderType.FragmentShader);

                string shaderSource2 = TestTools.ResourceShaders.GetShader("SFGraphics.Test.RenderTests.Shaders.invalidFrag.frag");
                shader.LoadShader(shaderSource2, ShaderType.FragmentShader);

                Assert.IsFalse(shader.LinkStatusIsOk);
            }
        }
    }
}