﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GLObjects.Textures
{
    public class Texture2D : Texture
    {
        public Texture2D(int width, int height) : base(TextureTarget.Texture2D, width, height)
        {

        }

        public Texture2D(Bitmap image) : base(TextureTarget.Texture2D, image.Width, image.Height)
        {
            // Load the image data.
            BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(textureTarget, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            image.UnlockBits(data);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }
    }
}
