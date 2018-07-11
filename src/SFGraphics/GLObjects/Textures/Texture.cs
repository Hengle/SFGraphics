﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics.OpenGL;


namespace SFGraphics.GLObjects.Textures
{
    /// <summary>
    /// Encapsulates the state for an OpenGL texture object. To support texture types other than 
    /// <see cref="Texture2D"/> and <see cref="TextureCubeMap"/>, inherit from this class
    /// and add the necessary additional methods. 
    /// <para></para> <para></para>
    /// Avoid creating textures manually to prevent issues with textures being deleted by 
    /// <see cref="GLObjectManager.DeleteUnusedGLObjects"/>.
    /// </summary>
    public abstract class Texture : IGLObject
    {
        /// <summary>
        /// The value generated by GL.GenTexture(). Do not bind <see cref="Id"/> when this object is unreachable.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// The <see cref="TextureTarget"/> used for all GL functions.
        /// </summary>
        protected TextureTarget textureTarget = TextureTarget.Texture2D;

        /// <summary>
        /// 
        /// </summary>
        public PixelInternalFormat PixelInternalFormat { get; }

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
        private TextureMinFilter minFilter;

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
        private TextureMagFilter magFilter;

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
        private TextureWrapMode textureWrapS;

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
        private TextureWrapMode textureWrapT;

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
        private TextureWrapMode textureWrapR;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="pixelInternalFormat"></param>
        public Texture(TextureTarget target, int width, int height, PixelInternalFormat pixelInternalFormat = PixelInternalFormat.Rgba)
        {
            // These should only be set once at object creation.
            Id = GL.GenTexture();
            GLObjectManager.AddReference(GLObjectManager.referenceCountByTextureId, Id);

            textureTarget = target;
            PixelInternalFormat = pixelInternalFormat;

            Bind();

            // Use properties because the GL texture needs to be updated.
            TextureWrapS = TextureWrapMode.ClampToEdge;
            TextureWrapT = TextureWrapMode.ClampToEdge;
            TextureWrapR = TextureWrapMode.ClampToEdge;
            MinFilter = TextureMinFilter.LinearMipmapLinear;
            MagFilter = TextureMagFilter.Linear;
        }

        /// <summary>
        /// Decrement the reference count for <see cref="Id"/>. The context probably isn't current, so the data is deleted later by <see cref="GLObjectManager"/>.
        /// </summary>
        ~Texture()
        {
            GLObjectManager.RemoveReference(GLObjectManager.referenceCountByTextureId, Id);
        }

        /// <summary>
        /// Binds the Id to <see cref="textureTarget"/>.
        /// </summary>
        public void Bind()
        {
            GL.BindTexture(textureTarget, Id);
        }
    }
}
