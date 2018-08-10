﻿using System;
using OpenTK.Graphics.OpenGL;


namespace SFGraphics.GLObjects.Textures
{
    /// <summary>
    /// Helpful tools for working with PixelInternalFormat and InternalFormat 
    /// with OpenTK's OpenGL texture functions.
    /// </summary>
    public static class TextureFormatTools
    {
        /// <summary>
        /// Calculates the imageSize parameter for GL.CompressedTexImage. 
        /// The imageSize should be recalculated for each mip level when reading mipmaps from existing image data.
        /// </summary>
        /// <param name="width">The width of the mip level in pixels</param>
        /// <param name="height">The height of the mip level in pixels</param>
        /// <param name="internalFormat">The <paramref name="internalFormat"/> should be a compressed format.</param>
        /// <returns></returns>
        public static int CalculateImageSize(int width, int height, InternalFormat internalFormat)
        {
            int blockSize = CalculateBlockSize(internalFormat);

            int imageSize = blockSize * (int)Math.Ceiling(width / 4.0) * (int)Math.Ceiling(height / 4.0);
            return imageSize;
        }

        private static int CalculateBlockSize(InternalFormat internalFormat)
        {
            int blockSizeInBytes = CompressedBlockSize.blockSizeByFormat[internalFormat.ToString()];
            return blockSizeInBytes;
        }

        /// <summary>
        /// Determines whether a format is compressed.
        /// Compressed formats should use GL.CompressedTexImage instead of GL.TexImage.
        /// </summary>
        /// <param name="internalFormat">The image format for the texture data</param>
        /// <returns>True if the format is compressed</returns>
        public static bool IsCompressed(InternalFormat internalFormat)
        {
            // All the enum value names should follow this convention.
            return internalFormat.ToString().ToLower().Contains("compressed");
        }

        /// <summary>
        /// Determines whether a format is compressed.
        /// Compressed formats should use GL.CompressedTexImage instead of GL.TexImage.
        /// </summary>
        /// <param name="pixelInternalFormat">The image format for the texture data</param>
        /// <returns>True if the format is compressed</returns>
        public static bool IsCompressed(PixelInternalFormat pixelInternalFormat)
        {
            // All the enum value names should follow this convention.
            return pixelInternalFormat.ToString().ToLower().Contains("compressed");
        }

        /// <summary>
        /// Determines if <paramref name="pixelInternalFormat"/> is a valid format for a 
        /// <see cref="DepthTexture"/>.
        /// </summary>
        /// <param name="pixelInternalFormat">The image format for the texture data</param>
        /// <returns>True if the format is a valid depth texture format</returns>
        public static bool IsDepthFormat(PixelInternalFormat pixelInternalFormat)
        {
            string formatName = pixelInternalFormat.ToString().ToLower();
            return formatName.Contains("depthcomponent");
        }
    }
}