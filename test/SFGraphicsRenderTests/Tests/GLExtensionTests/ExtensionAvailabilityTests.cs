﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFGraphics.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFGraphicsRenderTests.OpenTKExtensionTests
{
    [TestClass()]
    public class ExtensionAvailabilityTests
    {
        [TestInitialize]
        public void SetUpExtensions()
        {
            // Set up the context for all the tests.
            var window = TestTools.OpenTKWindowlessContext.CreateDummyContext();
            window.MakeCurrent();
            OpenGLExtensions.InitializeCurrentExtensions();
        }

        [TestMethod()]
        public void IsAvailableTest()
        {
            Assert.Fail("No extensions?");
        }
    }
}