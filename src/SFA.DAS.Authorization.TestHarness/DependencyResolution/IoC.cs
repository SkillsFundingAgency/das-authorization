﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StructureMap;

namespace SFA.DAS.Authorization.TestHarness.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            return new Container(c =>
            {
                c.AddRegistry<AuthorizationRegistry>();
                c.AddRegistry<TestHarnessAuthorizationRegistry>();
            });
        }
    }
}