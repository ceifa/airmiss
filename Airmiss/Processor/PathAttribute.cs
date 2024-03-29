﻿using System;

namespace Airmiss.Processor
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class PathAttribute : Attribute
    {
        public PathAttribute(string name)
        {
            Name = name;
        }

        internal string Name { get; }
    }
}