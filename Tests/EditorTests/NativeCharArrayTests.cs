using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Refsa.Collections.String;
using Unity.PerformanceTesting;
using UnityEngine;
using UnityEngine.TestTools;

namespace Refsa.Collections.Tests
{
    public class NativeCharArrayTests
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
        public void NativeCharArrayFromString()
        {
            NativeCharArray resultString1 = "";
            NativeCharArray resultString2 = "";

            for (int i = 0; i < 10000; i++)
            {
                string testString = RandomString(UnityEngine.Random.Range(1, 128));

                resultString1 = testString;
                resultString2 = new NativeCharArray(testString);

                Assert.AreEqual(testString, resultString1.ToString());
                Assert.AreEqual(testString, resultString2.ToString());
            }

            resultString1.Dispose();
            resultString2.Dispose();
        }

        [Test]
        [Performance]
        public void NativeCharArrayFromStringPerformance()
        {
            int testLength = 1000;
            string[] testStrings = new string[testLength];
            for (int i = 0; i < testLength; i++)
            {
                testStrings[i] = RandomString(30);
            }

            NativeCharArray resultString = "";

            Measure
            .Method(() => 
            {
                for (int i = 0; i < testLength; i++)
                {
                    resultString = new NativeCharArray(testStrings[i]);
                    resultString.Dispose();
                }
            })
            .WarmupCount(10)
            .MeasurementCount(100)
            .GC()
            .Run();
        }

        [Test]
        [Performance]
        public void NativeCharArrayToStringPerformance()
        {
            int testLength = 1000;
            NativeCharArray[] testStrings = new NativeCharArray[testLength];
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

            for (int i = 0; i < testLength; i++) testStrings[i].Dispose();
        }

        [Test]
        [Performance]
        public void NativeCharArrayComparePerformance()
        {
            int testLength = 1000;
            NativeCharArray[] testStrings = new NativeCharArray[testLength];
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

            for (int i = 0; i < testLength; i++) testStrings[i].Dispose();
        }
    }
}
