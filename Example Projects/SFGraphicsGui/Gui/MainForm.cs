﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;
using SFGraphics.GLObjects.Textures;
using SFGraphics.GLObjects.GLObjectManagement;
using SFGenericModel.ShaderGenerators;
using OpenTK;

namespace SFGraphicsGui
{
    /// <summary>
    /// A short example of how to use SFGraphics and OpenTK to render a texture.
    /// This class also shows how to check for common errors to avoid the difficult to debug 
    /// <see cref="AccessViolationException"/>.
    /// </summary>
    public partial class MainForm : Form
    {
        private GraphicsResources graphicsResources;
        private Texture2D textureToRender;

        public MainForm()
        {
            // The context isn't current yet, so don't call any OpenTK methods here.
            InitializeComponent();
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            // Context creation and resource creation failed, so we can't render anything.
            if (graphicsResources == null)
                return;

            // Set up the viewport.
            glControl1.MakeCurrent();
            GL.Viewport(glControl1.ClientRectangle);

            // Draw a test pattern image to the screen.
            DrawScreenTexture(textureToRender);
            glControl1.SwapBuffers();

            // Clean up any unused resources.
            GLObjectManager.DeleteUnusedGLObjects();
        }

        private void DrawScreenTexture(Texture2D texture)
        {
            if (texture == null)
                return;

            // Always check program creation before using shaders to prevent crashes.
            Shader shader = graphicsResources.screenTextureShader;

            // Generated debug shader.
            List<VertexAttributeRenderInfo> vertAttributesRenderInfo = new List<VertexAttributeRenderInfo>();
            foreach (var attribute in graphicsResources.screenTriangle.GetVertexAttributes())
            {
                VertexAttributeRenderInfo vertexAttributeRenderInfo = new VertexAttributeRenderInfo(true, true, attribute);
                vertAttributesRenderInfo.Add(vertexAttributeRenderInfo);
            }

            //shader = VertexAttributeShaderGenerator.CreateShader(vertAttributesRenderInfo);
            System.Diagnostics.Debug.WriteLine(shader.GetErrorLog());
            if (!shader.LinkStatusIsOk)
                return;

            // Render using the shader.
            shader.UseProgram();

            // The sampler's parameters are used instead of the texture's parameters.
            int textureUnit = 0;
            graphicsResources.samplerObject.Bind(textureUnit);

            shader.SetInt("attributeIndex", 1);
            Matrix4 matrix4 = Matrix4.Identity;
            shader.SetMatrix4x4("mvpMatrix", ref matrix4);
                
            shader.SetTexture("uvTexture", texture, textureUnit);

            graphicsResources.screenTriangle.Draw(shader, null);
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            if (OpenTK.Graphics.GraphicsContext.CurrentContext != null)
            {
                SetUpRendering();
            }
            else
            {
                MessageBox.Show("Context Creation Failed");
            }
        }

        private void SetUpRendering()
        {
            graphicsResources = new GraphicsResources();

            // Display compilation warnings.
            if (!graphicsResources.screenTextureShader.LinkStatusIsOk)
            {
                MessageBox.Show(graphicsResources.screenTextureShader.GetErrorLog(), "Failed Shader Compilation");
            }

            // Trigger the render event.
            glControl1.Invalidate();
        }

        private void uVTestPatternToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (graphicsResources != null)
            {
                textureToRender = graphicsResources.uvTestPattern;
                glControl1.Invalidate();
            }
        }

        private void magentaBlackStripesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (graphicsResources != null)
            {
                textureToRender = graphicsResources.floatMagentaBlackStripes;
                glControl1.Invalidate();
            }
        }

        private void drawCubeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Vector3> cubeVertices = GetCubePositions();

            Mesh3D triangle = new Mesh3D(cubeVertices, PrimitiveType.Triangles);
            Shader shader = VertexAttributeShaderGenerator.CreateShader(triangle.GetRenderAttributes());
            shader.UseProgram();
            Matrix4 matrix4 = Matrix4.CreateOrthographicOffCenter(-2, 2, -2, 2, -2, 2);
            shader.SetMatrix4x4("mvpMatrix", ref matrix4);

            glControl1.MakeCurrent();
            GL.ClearColor(1, 1, 1, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            triangle.Draw(shader, null);
            glControl1.SwapBuffers();
        }

        private static List<Vector3> GetCubePositions()
        {
            float scaleX = 1.0f;
            float scaleY = 1.0f;
            float scaleZ = 1.0f;
            List<Vector3> cubeVertices = new List<Vector3>()
            {
                new Vector3(-scaleX, -scaleY, -scaleZ),
                new Vector3(-scaleX, -scaleY,  scaleZ),
                new Vector3(-scaleX,  scaleY,  scaleZ),
                new Vector3( scaleX,  scaleY, -scaleZ),
                new Vector3(-scaleX, -scaleY, -scaleZ),
                new Vector3(-scaleX,  scaleY, -scaleZ),
                new Vector3( scaleX, -scaleY,  scaleZ),
                new Vector3(-scaleX, -scaleY, -scaleZ),
                new Vector3( scaleX, -scaleY, -scaleZ),
                new Vector3( scaleX,  scaleY, -scaleZ),
                new Vector3( scaleX, -scaleY, -scaleZ),
                new Vector3(-scaleX, -scaleY, -scaleZ),
                new Vector3(-scaleX, -scaleY, -scaleZ),
                new Vector3(-scaleX,  scaleY,  scaleZ),
                new Vector3(-scaleX,  scaleY, -scaleZ),
                new Vector3 (scaleX, -scaleY,  scaleZ),
                new Vector3(-scaleX, -scaleY,  scaleZ),
                new Vector3(-scaleX, -scaleY, -scaleZ),
                new Vector3(-scaleX,  scaleY,  scaleZ),
                new Vector3(-scaleX, -scaleY,  scaleZ),
                new Vector3( scaleX, -scaleY,  scaleZ),
                new Vector3( scaleX,  scaleY,  scaleZ),
                new Vector3( scaleX, -scaleY, -scaleZ),
                new Vector3( scaleX,  scaleY, -scaleZ),
                new Vector3( scaleX, -scaleY, -scaleZ),
                new Vector3( scaleX,  scaleY,  scaleZ),
                new Vector3( scaleX, -scaleY,  scaleZ),
                new Vector3( scaleX,  scaleY,  scaleZ),
                new Vector3( scaleX,  scaleY, -scaleZ),
                new Vector3(-scaleX,  scaleY, -scaleZ),
                new Vector3( scaleX,  scaleY,  scaleZ),
                new Vector3(-scaleX,  scaleY, -scaleZ),
                new Vector3(-scaleX,  scaleY,  scaleZ),
                new Vector3( scaleX,  scaleY,  scaleZ),
                new Vector3(-scaleX,  scaleY,  scaleZ),
                new Vector3( scaleX, -scaleY,  scaleZ)
            };
            return cubeVertices;
        }
    }
}
