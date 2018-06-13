﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GLObjects.Textures
{
    public abstract class Texture
    {
        private static HashSet<int> texturesToDelete = new HashSet<int>();

        /// <summary>
        /// Calls GL.DeleteTexture on all texture Ids still flagged for deletion.
        /// </summary>
        public static void DeleteUnusedTextures()
        {
            foreach (int texture in texturesToDelete)
            {
                GL.DeleteTexture(texture);
            }
            texturesToDelete.Clear();
        }

        /// <summary>
        /// Avoids the following scenario. Should be called on context destruction.
        /// <para>1. Texture is created.</para>
        /// <para>2. Context is destroyed and resources are freed.</para>
        /// <para>3. A texture with the same Id is made in a new context.</para>
        /// <para>4. The new texture is deleted because of the shared Id.</para>
        /// </summary>
        public static void ClearTexturesFlaggedForDeletion()
        {
            texturesToDelete.Clear();
        }

        /// <summary>
        /// The value generated by GL.GenTexture(). Do not attempt to bind this when the object has gone out of scope.
        /// </summary>
        public int Id { get; }

        protected TextureTarget textureTarget = TextureTarget.Texture2D;

        public PixelInternalFormat PixelInternalFormat { get; }

        private TextureMinFilter minFilter;
        /// <summary>
        /// Binds and updates the TextureParameter when set.
        /// </summary>
        public TextureMinFilter MinFilter
        {
            get { return minFilter; }
            set
            {
                Bind();
                minFilter = value;
                GL.TexParameter(textureTarget, TextureParameterName.TextureMinFilter, (int)value);
            }
        }

        private TextureMagFilter magFilter;
        /// <summary>
        /// Binds and updates the TextureParameter when set.
        /// </summary>
        public TextureMagFilter MagFilter
        {
            get { return magFilter; }
            set
            {
                Bind();
                magFilter = value;
                GL.TexParameter(textureTarget, TextureParameterName.TextureMagFilter, (int)value);
            }
        }

        private TextureWrapMode textureWrapS;
        /// <summary>
        /// Binds and updates the TextureParameter when set.
        /// </summary>
        public TextureWrapMode TextureWrapS
        {
            get { return textureWrapS; }
            set
            {
                Bind();
                textureWrapS = value;
                GL.TexParameter(textureTarget, TextureParameterName.TextureWrapS, (int)value);
            }
        }

        private TextureWrapMode textureWrapT;
        /// <summary>
        /// Binds and updates the TextureParameter when set.
        /// </summary>
        public TextureWrapMode TextureWrapT
        {
            get { return textureWrapT; }
            set
            {
                Bind();
                textureWrapT = value;
                GL.TexParameter(textureTarget, TextureParameterName.TextureWrapT, (int)value);
            }
        }

        private TextureWrapMode textureWrapR;
        /// <summary>
        /// Binds and updates the TextureParameter when set.
        /// </summary>
        public TextureWrapMode TextureWrapR
        {
            get { return textureWrapR; }
            set
            {
                Bind();
                textureWrapR = value;
                GL.TexParameter(textureTarget, TextureParameterName.TextureWrapR, (int)value);
            }
        }

        public Texture(TextureTarget target, int width, int height, PixelInternalFormat pixelInternalFormat = PixelInternalFormat.Rgba)
        {
            // These should only be set once at object creation.
            Id = GL.GenTexture();
            textureTarget = target;
            PixelInternalFormat = pixelInternalFormat;

            Bind();

            // The GL texture needs to be updated in addition to initializing the variables.
            TextureWrapS = TextureWrapMode.ClampToEdge;
            TextureWrapT = TextureWrapMode.ClampToEdge;
            TextureWrapR = TextureWrapMode.ClampToEdge;
            MinFilter = TextureMinFilter.NearestMipmapLinear;
            MagFilter = TextureMagFilter.Linear;

            // Setup the format and mip maps.
            GL.TexImage2D(textureTarget, 0, PixelInternalFormat, width, height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.Float, IntPtr.Zero);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        ~Texture()
        {
            // The context probably isn't current here, so any GL function will crash.
            // The texture will need to be cleaned up later. 
            if (!texturesToDelete.Contains(Id))
                texturesToDelete.Add(Id);            
        }

        /// <summary>
        /// Binds the Id to the specified target at creation.
        /// </summary>
        public void Bind()
        {
            GL.BindTexture(textureTarget, Id);
        }
    }
}
