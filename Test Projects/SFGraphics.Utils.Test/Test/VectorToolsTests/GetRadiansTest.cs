﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.Utils;

namespace SFGraphicsTest.VectorToolsTests
{
    public partial class VectorToolsTest
    {
        [TestClass]
        public class GetRadiansTest
        {
            private readonly double delta = 0.00001;

            [TestMethod]
            public void ZeroDegreesToRadians()
            {
                Assert.AreEqual(0, VectorTools.GetRadians(0), delta);
            }

            [TestMethod]
            public void SmallDegreesToRadians()
            {
                Assert.AreEqual(0.6108652382, VectorTools.GetRadians(35), delta);
            }

            [TestMethod]
            public void LargeDegreesToRadians()
            {
                Assert.AreEqual(12.566370614, VectorTools.GetRadians(720), delta);
            }
        }
    }
}