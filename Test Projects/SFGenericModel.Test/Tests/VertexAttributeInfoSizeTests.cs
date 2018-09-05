﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;

namespace SFGenericModel.Test
{
    [TestClass]
    public class VertexAttributeInfoSizeTests
    {
        [TestMethod]
        public void Byte()
        {
            CheckAttribPointerSizeTwoComponents(sizeof(byte) * 2, VertexAttribPointerType.Byte);
        }

        [TestMethod]
        public void UnsignedByte()
        {
            CheckAttribPointerSizeTwoComponents(sizeof(byte) * 2, VertexAttribPointerType.UnsignedByte);
        }

        [TestMethod]
        public void Int()
        {
            CheckAttribPointerSizeTwoComponents(sizeof(int) * 2, VertexAttribPointerType.Int);
        }

        [TestMethod]
        public void UnsignedInt()
        {
            CheckAttribPointerSizeTwoComponents(sizeof(int) * 2, VertexAttribPointerType.UnsignedInt);
        }

        [TestMethod]
        public void Short()
        {
            CheckAttribPointerSizeTwoComponents(sizeof(short) * 2, VertexAttribPointerType.Short);
        }

        [TestMethod]
        public void UnsignedShort()
        {
            CheckAttribPointerSizeTwoComponents(sizeof(short) * 2, VertexAttribPointerType.UnsignedShort);
        }

        [TestMethod]
        public void Float()
        {
            CheckAttribPointerSizeTwoComponents(sizeof(float) * 2, VertexAttribPointerType.Float);
        }

        [TestMethod]
        public void Double()
        {
            CheckAttribPointerSizeTwoComponents(sizeof(double) * 2, VertexAttribPointerType.Double);
        }

        private static void CheckAttribPointerSizeTwoComponents(int expected, VertexAttribPointerType type)
        {
            VertexAttributeInfo attribInfo = new VertexAttributeInfo("", ValueCount.Two, type);
            Assert.AreEqual(expected, attribInfo.sizeInBytes);
        }
    }
}