﻿/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace Lucene.Net.Util
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
    using Xunit;

    public class LuceneTestCase
    {
        /// <summary>
        ///  A random multiplier which you should use when writing random tests:
        ///  multiply it by the number of iterations to scale your tests (for nightly builds).
        /// </summary>
        public static int RANDOM_MULTIPLIER = SystemProps.Get<int>("tests:multiplier", 1);

        /// <summary>
        /// Whether or not <see cref="NightlyAttribute" /> tests should run.
        /// </summary>
        public static bool TEST_NIGHTLY = SystemProps.Get<Boolean>("tests:nightly", false);

        private static ThreadLocal<System.Random> random;

        static LuceneTestCase()
        {
            random = new ThreadLocal<System.Random>(() => {

              
                return new System.Random((int) DateTime.Now.Ticks & 0x0000FFFF);
            });
        }

        public LuceneTestCase()
        {
           
        }

        /// <summary>
        /// Placeholder for random values.
        /// </summary>
        public System.Random Random
        {
            get { return random.Value; }  
        }

#if XUNIT

        /// <summary>
        /// Asserts that two object are the same.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        [DebuggerHidden]
        public static void Same(object expected, object actual)
        {
            Assert.Same(expected, actual);
        }

        /// <summary>
        /// Assert that two objects are not the same.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        [DebuggerHidden]
        public static void NotSame(object expected, object actual)
        {
            Assert.NotSame(expected, actual);
        }

        [DebuggerHidden]
        public static void Equal(string expected, string actual)
        {
            Assert.Equal(expected, actual);
        }

        [DebuggerHidden]
        public static void Equal<T>(T expected, T actual)
        {
            Assert.Equal(expected, actual);
        }

        [DebuggerHidden]
        public static void Equal<T>(IEnumerable<T> expected, IEnumerable<T> actual)
        {
            Assert.Equal(expected, actual);
        }

        [DebuggerHidden]
        public static void NotEqual<T>(T expected, T actual)
        {
            Assert.NotEqual(expected, actual);
        }


        [DebuggerHidden]
        public static void Ok(bool condition, string message = null, params object[] values)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                var exceptionMessage = message;

                if(values != null && values.Length > 0)
                {
                    exceptionMessage = String.Format(exceptionMessage, values);
                }

                Assert.True(condition, exceptionMessage);
            }
            else 
            {
                Assert.True(condition);    
            }
        }

        [DebuggerHidden]
        public static T Throws<T>(Action code) where T : Exception
        {
            return Assert.Throws<T>(code);
        }
        
        #endif
    }
}