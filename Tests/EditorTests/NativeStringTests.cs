using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Refsa.Collections.String;
using Unity.PerformanceTesting;

namespace Tests
{
    public class NativeStringTests
    {
        static string alphaNumeric = "abcdefghijklmnopqrstuvxyz0123456789";
        static string RandomString(int length)
        {
            string randomString = "";
            for (int i = 0; i < length; i++)
            {
                randomString += alphaNumeric[UnityEngine.Random.Range(0, alphaNumeric.Length - 1)];
            }
            return randomString;
        }

        [Test]
        public void NativeStringFromString()
        {
            for (int i = 0; i < 10000; i++)
            {
                string testString = RandomString(UnityEngine.Random.Range(1, NativeString_32.MaxLength));

                NativeString_32 resultString1 = testString;
                NativeString_32 resultString2 = new NativeString_32(testString);

                Assert.AreEqual(testString, resultString1.ToString());
                Assert.AreEqual(testString, resultString2.ToString());
            }
        }

        [Test]
        public void NativeStringSetString()
        {
            NativeString_32 resultString1 = "";
            for (int i = 0; i < 10000; i++)
            {
                string testString = RandomString(UnityEngine.Random.Range(1, NativeString_32.MaxLength));
                resultString1.SetString(testString);

                Assert.AreEqual(testString, resultString1.ToString());
            }
        }

        [Test]
        [Performance]
        public void NativeStringFromStringPerformance()
        {
            int testLength = 1000;
            string[] testStrings = new string[testLength];
            for (int i = 0; i < testLength; i++)
            {
                testStrings[i] = RandomString(30);
            }

            Measure
            .Method(() => 
            {
                for (int i = 0; i < testLength; i++)
                {
                    NativeString_32 resultString = new NativeString_32(testStrings[i]);
                }
            })
            .WarmupCount(10)
            .MeasurementCount(100)
            .GC()
            .Run();
        }

        [Test]
        [Performance]
        public void NativeStringSetStringPerformance()
        {
            int testLength = 1000;
            string[] testStrings = new string[testLength];
            for (int i = 0; i < testLength; i++)
            {
                testStrings[i] = RandomString(30);
            }

            NativeString_32 resultString = "";

            Measure
            .Method(() => 
            {
                for (int i = 0; i < testLength; i++)
                {
                    resultString.SetString(testStrings[i]);
                }
            })
            .WarmupCount(10)
            .MeasurementCount(100)
            .GC()
            .Run();
        }

        [Test]
        [Performance]
        public void NativeStringToStringPerformance()
        {
            int testLength = 1000;
            NativeString_32[] testStrings = new NativeString_32[testLength];
            for (int i = 0; i < testLength; i++)
            {
                testStrings[i] = RandomString(30);
            }

            Measure
            .Method(() => 
            {
                for (int i = 0; i < testLength; i++)
                {
                    string resultString = testStrings[i].ToString();
                }
            })
            .WarmupCount(10)
            .MeasurementCount(100)
            .GC()
            .Run();
        }

        [Test]
        [Performance]
        public void NativeStringComparePerformance()
        {
            int testLength = 1000;
            NativeString_32[] testStrings = new NativeString_32[testLength];
            for (int i = 0; i < testLength; i++)
            {
                testStrings[i] = RandomString(30);
            }

            Measure
            .Method(() => 
            {
                for (int i = 0; i < testLength - 1; i++)
                {
                    bool equals = testStrings[i].Equals(testStrings[i + 1]);
                }
            })
            .WarmupCount(10)
            .MeasurementCount(100)
            .GC()
            .Run();
        }
    }
}
