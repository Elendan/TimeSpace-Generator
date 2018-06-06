﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSpaceGenerator.Core.Utilities
{
    /// <summary>
    ///     Instanciate a T Thread Safe Singleton
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : class, new()
    {
        private static readonly Lazy<T> Lazy = new Lazy<T>(() => new T());

        public static T Instance => Lazy.Value;
    }
}
