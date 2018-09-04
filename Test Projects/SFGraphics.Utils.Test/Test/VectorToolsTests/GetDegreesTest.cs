﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.Utils;

namespace SFGraphicsTest.VectorToolsTests
{
    public partial class VectorToolsTest
    {
        [TestClass]
        public class GetDegreesTest
        {
            private readonly double delta = 0.00001;

            [TestMethod]
            public void ZeroRadiansToDegrees()
            {
                Assert.AreEqual(0, VectorTools.GetDegrees(0), delta);
            }

            [TestMethod]
            public void SmallRadiansToDegrees()
            {
                Assert.AreEqual(35, VectorTools.GetDegrees(0.6108652382), delta);
            }

            [TestMethod]
            public void LargeRadiansToDegrees()
            {
                Assert.AreEqual(720, VectorTools.GetDegrees(12.566370614), delta);
            }
        }
    }
}