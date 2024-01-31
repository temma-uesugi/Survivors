// using System.Collections.Generic;
// using System.Linq;
// using App.AppCommon.Utils;
// using NUnit.Framework;
//
// namespace App.Test
// {
//     /// <summary>
//     /// ランダムテスト
//     /// </summary>
//     public class RandomTest
//     {
//         private const int TestCount = 1000;
//         
//         /// <summary>
//         /// RangeIntのテスト
//         /// </summary>
//         [TestCase(0, 2, false)]
//         [TestCase(3, 10, true)]
//         public void RangeIntTest(int min, int max, bool isLessThanTo)
//         {
//             var countMap = new Dictionary<int, int>();
//             for (int i = min; i < max; i++)
//             {
//                 countMap.Add(i, 0); 
//             }
//             if (isLessThanTo)
//             {
//                 countMap.Add(max, 0);
//             }
//
//             var expectedRate = 1f / countMap.Count;
//
//             for (int i = 0; i < TestCount; i++)
//             {
//                 var val = RandomUtil.Range(min, max, isLessThanTo);
//                 countMap[val]++;
//             }
//
//             var value0Keys = countMap.Where(x => x.Value == 0);
//             Assert.That(value0Keys.Count(), Is.EqualTo(0));
//            
//             foreach (var (key, val) in countMap)
//             {
//                 float rate = (float)val / TestCount;
//                 Assert.That(rate, Is.EqualTo(expectedRate).Within(0.05f));
//             }
//         }
//
//         /// <summary>
//         /// RangeFloatのテスト
//         /// </summary>
//         [TestCase(0f, 2f)]
//         [TestCase(3f, 10f)]
//         public void RangeFloatTest(float min, float max)
//         {
//             var countMap = new Dictionary<int, int>();
//             for (int i = (int)min; i < (int)max; i++)
//             {
//                 countMap.Add(i, 0); 
//             }
//
//             var expectedRate = 1f / countMap.Count;
//
//             for (int i = 0; i < TestCount; i++)
//             {
//                 int val = (int)RandomUtil.Range(min, max);
//                 countMap[val]++;
//             }
//
//             var value0Keys = countMap.Where(x => x.Value == 0);
//             Assert.That(value0Keys.Count(), Is.EqualTo(0));
//            
//             foreach (var (key, val) in countMap)
//             {
//                 float rate = (float)val / TestCount;
//                 Assert.That(rate, Is.EqualTo(expectedRate).Within(0.05f));
//             }
//         }
//
//         /// <summary>
//         /// Judgeテスト
//         /// </summary>
//         [TestCase(0.5f)]
//         [TestCase(0.2f)]
//         [TestCase(0.7f)]
//         [TestCase(1f)]
//         public void JudgeTest(float probability)
//         {
//             var trueCount = 0;
//             for (int i = 0; i < TestCount; i++)
//             {
//                 if (RandomUtil.Judge(probability))
//                 {
//                     trueCount++;
//                 }
//             }
//             
//             Assert.That((float)trueCount / TestCount, Is.EqualTo(probability).Within(0.05f));
//         }
//     }
// }