﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using SFGraphics.Utils;

namespace ColorToolsTests
{
    public partial class ColorToolsTest
    {
        [TestClass]
        public class InvertColorTest
        {
            [TestMethod]
            public void InvertColor()
            {
                Color color = Color.FromArgb(255, 255, 255, 255);
                Color inverted = ColorUtils.InvertColor(color);

                Color expected = Color.FromArgb(255, 0, 0, 0);
                Assert.AreEqual(expected, inverted);
            }
        }
    }
}
