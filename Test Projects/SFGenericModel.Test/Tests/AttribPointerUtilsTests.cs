﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;

namespace SFGenericModel.Test
{
    [TestClass]
    public class AttribPointerUtilsTests
    {
        [TestMethod]
        public void Byte()
        {
            CheckAttribPointerSize(sizeof(byte), VertexAttribPointerType.Byte);
        }

        [TestMethod]
        public void UnsignedByte()
        {
            CheckAttribPointerSize(sizeof(byte), VertexAttribPointerType.UnsignedByte);
        }

        [TestMethod]
        public void Int()
        {
            CheckAttribPointerSize(sizeof(int), VertexAttribPointerType.Int);
        }

        [TestMethod]
        public void UnsignedInt()
        {
            CheckAttribPointerSize(sizeof(int), VertexAttribPointerType.UnsignedInt);
        }

        [TestMethod]
        public void Short()
        {
            CheckAttribPointerSize(sizeof(short), VertexAttribPointerType.Short);
        }

        [TestMethod]
        public void UnsignedShort()
        {
            CheckAttribPointerSize(sizeof(short), VertexAttribPointerType.UnsignedShort);
        }

        [TestMethod]
        public void Float()
        {
            CheckAttribPointerSize(sizeof(float), VertexAttribPointerType.Float);
        }

        [TestMethod]
        public void Double()
        {
            CheckAttribPointerSize(sizeof(double), VertexAttribPointerType.Double);
        }

        private static void CheckAttribPointerSize(int expected, VertexAttribPointerType type)
        {
            Assert.AreEqual(expected, AttribPointerUtils.sizeInBytesByType[type]);
        }
    }
}
