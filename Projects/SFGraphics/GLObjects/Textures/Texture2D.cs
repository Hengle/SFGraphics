﻿using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.BufferObjects;
using SFGraphics.GLObjects.Textures.TextureFormats;
using SFGraphics.GLObjects.Textures.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace SFGraphics.GLObjects.Textures
{
    /// <summary>
    /// A TextureTarget.Texture2D texture that supports mipmaps.
    /// </summary>
    public class Texture2D : Texture
    {
        /// <summary>
        /// Creates an empty 2D texture. 
        /// The texture is incomplete until the dimensions and format are set.
        /// </summary>
        public Texture2D() : base(TextureTarget.Texture2D)
        {
            TextureWrapS = TextureWrapMode.ClampToEdge;
            TextureWrapT = TextureWrapMode.ClampToEdge;
            TextureWrapR = TextureWrapMode.ClampToEdge;

            // LoadImageData() create mipmaps, so enable them by default.
            MinFilter = TextureMinFilter.LinearMipmapLinear;
            MagFilter = TextureMagFilter.Linear;
        }

        /// <summary>
        /// Specifies the texture's dimensions and format but leaves the image data
        /// uninitialized.
        /// </summary>
        /// <param name="width">The new width in pixels</param>
        /// <param name="height">The new height in pixels</param>
        /// <param name="format">The format to store the image data</param>
        public void LoadImageData(int width, int height, TextureFormatUncompressed format)
        {
            Width = width;
            Height = height;

            // This only works for uncompressed texture data. 
            // GL.CompressedTexImage2D function will crash when using IntPtr.Zero.
            Bind();
            GL.TexImage2D(TextureTarget, 0, format.pixelInternalFormat, Width, Height, 0, 
                format.pixelFormat, format.pixelType, IntPtr.Zero);
        }

        /// <summary>
        /// Loads RGBA texture data with mipmaps generated from the specified bitmap.
        /// Binds the texture.
        /// </summary>
        /// <param name="image">the image data for the base mip level</param>
        public void LoadImageData(Bitmap image)
        {
            Width = image.Width;
            Height = image.Height;

            Bind();
            MipmapLoading.LoadBaseLevelGenerateMipmaps(TextureTarget, image);
        }

        /// <summary>
        /// Loads texture data of the specified format for the first mip level.
        /// Mipmaps are generated by OpenGL.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="width">The width of <paramref name="baseMipLevel"/> in pixels</param>
        /// <param name="height">The height of <paramref name="baseMipLevel"/> in pixels</param>
        /// <param name="baseMipLevel">The image data to load for the first mip level. The other levels are generated.</param>
        /// <param name="internalFormat">The image format of <paramref name="baseMipLevel"/></param>
        /// <exception cref="ArgumentException"><paramref name="internalFormat"/> is not a compressed format.</exception>
        public void LoadImageData<T>(int width, int height, T[] baseMipLevel, InternalFormat internalFormat)
            where T : struct
        {
            if (!TextureFormatTools.IsCompressed(internalFormat))
                throw new ArgumentException(TextureExceptionMessages.expectedCompressed);

            if (TextureFormatTools.IsGenericCompressed(internalFormat))
                throw new NotSupportedException(TextureExceptionMessages.genericCompressedFormat);

            Width = width;
            Height = height;

            Bind();
            MipmapLoading.LoadBaseLevelGenerateMipmaps(TextureTarget, width, height, baseMipLevel, internalFormat);
        }

        /// <summary>
        /// Loads texture data of the specified format for the first mip level.
        /// Mipmaps are generated by OpenGL.
        /// </summary>
        /// <param name="width">The width of <paramref name="baseMipLevel"/> in pixels</param>
        /// <param name="height">The height of <paramref name="baseMipLevel"/> in pixels</param>
        /// <param name="baseMipLevel">The image data of the first mip level</param>
        /// <param name="internalFormat">The image format for all mipmaps</param>
        public void LoadImageData(int width, int height, BufferObject baseMipLevel, InternalFormat internalFormat)
        {
            if (!TextureFormatTools.IsCompressed(internalFormat))
                throw new ArgumentException(TextureExceptionMessages.expectedCompressed);

            if (TextureFormatTools.IsGenericCompressed(internalFormat))
                throw new NotSupportedException(TextureExceptionMessages.genericCompressedFormat);

            Width = width;
            Height = height;

            Bind();
            MipmapLoading.LoadBaseLevelGenerateMipmaps(TextureTarget, width, height, baseMipLevel, internalFormat);
        }

        /// <summary>
        /// Loads texture data of the specified format for the first mip level.
        /// Mipmaps are generated by OpenGL.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="width">The width of <paramref name="baseMipLevel"/> in pixels</param>
        /// <param name="height">The height of <paramref name="baseMipLevel"/> in pixels</param>
        /// <param name="baseMipLevel">The image data of the first mip level</param>
        /// <param name="format">The image format for all mipmaps</param>
        public void LoadImageData<T>(int width, int height, T[] baseMipLevel, TextureFormatUncompressed format)
            where T : struct
        {
            Width = width;
            Height = height;

            Bind();
            MipmapLoading.LoadBaseLevelGenerateMipmaps(TextureTarget, width, height, baseMipLevel, format);
        }

        /// <summary>
        /// Loads texture data of the specified format for the first mip level.
        /// Mipmaps are generated by OpenGL.
        /// </summary>
        /// <param name="width">The width of the base mip level in pixels</param>
        /// <param name="height">The height of the base mip level in pixels</param>
        /// <param name="baseMipLevel">The image data of the first mip level</param>
        /// <param name="format">The image format for all mipmaps</param>
        public void LoadImageData(int width, int height, BufferObject baseMipLevel, TextureFormatUncompressed format)
        {
            Width = width;
            Height = height;

            Bind();
            MipmapLoading.LoadBaseLevelGenerateMipmaps(TextureTarget, width, height, baseMipLevel, format);
        }

        /// <summary>
        /// Loads a mip level of compressed texture data
        /// for each array in <paramref name="mipmaps"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="width">The width of the base mip level in pixels</param>
        /// <param name="height">The height of the base mip level in pixels</param>
        /// <param name="mipmaps">The image data for each mip level</param>
        /// <param name="internalFormat">The image format of <paramref name="mipmaps"/></param>
        /// <exception cref="ArgumentException"><paramref name="internalFormat"/> is not a compressed format.</exception>
        public void LoadImageData<T>(int width, int height, List<T[]> mipmaps, InternalFormat internalFormat)
            where T : struct
        {
            if (!TextureFormatTools.IsCompressed(internalFormat))
                throw new ArgumentException(TextureExceptionMessages.expectedCompressed);

            if (TextureFormatTools.IsGenericCompressed(internalFormat))
                throw new NotSupportedException(TextureExceptionMessages.genericCompressedFormat);

            Width = width;
            Height = height;

            MipmapLoading.LoadCompressedMipMaps(TextureTarget.Texture2D, width, height, mipmaps, internalFormat);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width">The width of the base mip level in pixels</param>
        /// <param name="height">The height of the base mip level in pixels</param>
        /// <param name="mipmaps">The image data for each mip level</param>
        /// <param name="format">The image format of <paramref name="mipmaps"/></param>
        public void LoadImageData(int width, int height, List<BufferObject> mipmaps,
            TextureFormatUncompressed format)
        {
            Width = width;
            Height = height;

            MipmapLoading.LoadUncompressedMipmaps(TextureTarget.Texture2D, width, height, mipmaps, format);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width">The width of the base mip level in pixels</param>
        /// <param name="height">The height of the base mip level in pixels</param>
        /// <param name="mipmaps">The image data for each mip level</param>
        /// <param name="internalFormat">The image format of <paramref name="mipmaps"/></param>
        public void LoadImageData(int width, int height, List<BufferObject> mipmaps,
            InternalFormat internalFormat)
        {
            if (!TextureFormatTools.IsCompressed(internalFormat))
                throw new ArgumentException(TextureExceptionMessages.expectedCompressed);

            if (TextureFormatTools.IsGenericCompressed(internalFormat))
                throw new NotSupportedException(TextureExceptionMessages.genericCompressedFormat);

            Width = width;
            Height = height;

            MipmapLoading.LoadCompressedMipmaps(TextureTarget.Texture2D, width, height, mipmaps, internalFormat);
        }
    }
}
