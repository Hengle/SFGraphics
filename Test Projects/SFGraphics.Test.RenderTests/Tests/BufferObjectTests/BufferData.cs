﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.BufferObjects;

namespace BufferObjectTests
{
    [TestClass]
    public class BufferData
    {
        private BufferObject buffer;
        private readonly float[] originalData = new float[] { 1.5f, 2.5f, 3.5f };

        [TestInitialize()]
        public void Initialize()
        {
            // Set up the context for all the tests.
            RenderTestUtils.OpenTKWindowlessContext.BindDummyContext();

            buffer = new BufferObject(BufferTarget.ArrayBuffer);
            buffer.SetData(originalData, BufferUsageHint.StaticDraw);
        }

        [TestMethod]
        public void GetFloats()
        {
            float[] readData = buffer.GetData<float>();
            CollectionAssert.AreEqual(originalData, readData);
        }
        
        [TestMethod]
        public void GetVector3FromFloats()
        {
            // The 3 floats should make a single Vector3 struct.
            Vector3[] expectedData = new Vector3[] { new Vector3(1.5f, 2.5f, 3.5f) };
            Vector3[] readData = buffer.GetData<Vector3>();
            CollectionAssert.AreEqual(expectedData, readData);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void GetVector4FromFloats()
        {
            Vector4[] readData = buffer.GetData<Vector4>();
        }
    }
}
