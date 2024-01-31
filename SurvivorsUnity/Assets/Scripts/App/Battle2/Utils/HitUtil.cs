using System;
using UnityEngine;

namespace App.Battle2.Utils
{
    /// <summary>
    /// 当たり判定Util
    /// </summary>
    public static class HitUtil
    {
		public static readonly Vector3 Vector3Zero = new Vector3(0, 0, 0);

		public enum Pivot
		{
			LeftTop, Top, RightTop,
			Left, Center, Right,
			LeftBottom, Bottom, RightBottom
		}

		private const float PI = 3.1415926f;
		
		private const float Deg2Rad = PI * 2f / 360f;

		private const float Rad2Deg = 1f / Deg2Rad;

		
		/// <summary>Sin</summary>
		private static float Sin(float x)
		{
			return (float)Math.Sin(x);
		}

		/// <summary>Cos</summary>
		private static float Cos(float x)
		{
			return (float)Math.Cos(x);
		}

		/// <summary>外積</summary>
		public static float Cross(float x1, float y1, float x2, float y2)
		{
			return x1 * y2 - y1 * x2;
		}

		/// <summary>内積</summary>
		public static float Dot(float x1, float y1, float x2, float y2)
		{
			return x1 * x2 + y1 * y2;
		}

		public static float Angle(float x1, float y1, float x2, float y2)
		{
			return 180.0f - (float)Math.Atan2(x1 - x2, y1 - y2) * 180.0f / PI;
		}

		/// <summary>距離(ルート計算はしない)</summary>
		public static float Distance(float x1, float y1, float x2, float y2)
		{
			float x = x1 - x2;
			float y = y1 - y2;
			return x * x + y * y;
		}

		/// <summary>Pivot計算のOffset取得</summary>
		public static Vector3 GetPivotOffset(Vector3 size, Pivot pivot)
		{
			Vector3 offset = Vector3Zero;
			switch(pivot)
			{
				case Pivot.Left:
				case Pivot.LeftTop:
				case Pivot.LeftBottom:
					offset.x = size.x * 0.5f;
					break;
				case Pivot.Right:
				case Pivot.RightTop:
				case Pivot.RightBottom:
					offset.x = -size.x * 0.5f;
					break;
			}

			switch(pivot)
			{
				case Pivot.Top:
				case Pivot.LeftTop:
				case Pivot.RightTop:
					offset.y = -size.y * 0.5f;
					break;
				case Pivot.Bottom:
				case Pivot.LeftBottom:
				case Pivot.RightBottom:
					offset.y = size.y * 0.5f;
					break;
			}

			return offset;
		}

		/// <summary>Vector3回転</summary>
		private static Vector3 Rotate(Vector3 point, Vector3 center, float angle)
		{
			return Rotate(point.x, point.y, center.x, center.y, angle);
		}

		/// <summary>Vector3回転</summary>
		private static Vector3 Rotate(float x, float y, float cx, float cy, float angle)
		{
			angle = angle * PI / 180.0f;
			x-=cx;
			y-=cy;

			float cos = Cos(angle);
			float sin = Sin(angle);

			return new Vector3(
				x * cos - y * sin + cx,
				0,
				x * sin + y * cos + cy
			);
		}

		private static float Max(float value1, float value2)
		{
			return value1 >= value2 ? value1 : value2;
		}

		private static float Min(float value1, float value2)
		{
			return value1 < value2 ? value1 : value2;
		}

		private static float Sqrt(float value)
		{
			return (float)Math.Sqrt(value);
		}

		private static Vector3 AddVector(Vector3 value1, Vector3 value2)
		{
			return new Vector3(value1.x + value2.x, value1.y + value2.y, value1.z + value2.z);
		}

		/// <summary>線と円の当たり判定</summary>
		public static bool IsLineAndCircle(Vector3 point1, Vector3 point2, Vector3 circlePoint, float radius)
		{
			return IsLineAndCircle(point1, point2, circlePoint, radius, out Vector3 crossPoint1, out Vector3 crossPoint2) > 0;
		}
		/// <summary>線と円の当たり判定</summary>
		public static int IsLineAndCircle(Vector3 point1, Vector3 point2, Vector3 circlePoint, float radius, out Vector3 crossPoint1, out Vector3 crossPoint2)
		{
			crossPoint1 = Vector3Zero;
			crossPoint2 = Vector3Zero;

			float p1x = point1.x - circlePoint.x;
			float p1y = point1.y - circlePoint.y;
			float p2x = point2.x - circlePoint.x;
			float p2y = point2.y - circlePoint.y;

			float linex = p1x - p2x;
			float liney = p1y - p2y;

			float a = -liney;
			float b = linex;
			float c = -Cross(p1x, p1y, p2x, p2y);

			float ab = a * a + b * b;
			float ac = a * c;
			float bc = b * c;
			float rr = radius * radius * ab - c * c;
			if(rr < 0)return 0;

			if(rr < 0.000001f)rr = 0;

			if(rr == 0)
			{
				crossPoint1 = AddVector( new Vector3(-ac / ab, -bc / ab, 0), circlePoint);
				return 1;
			}

			rr = Sqrt(rr);

			Vector3 v1 = AddVector( new Vector3( (-ac - b * rr) / ab, (-bc + a * rr) / ab, 0), circlePoint);
			Vector3 v2 = AddVector( new Vector3( (-ac + b * rr) / ab, (-bc - a * rr) / ab, 0), circlePoint);

			int resultCount = 0;
			float right  = Max(point1.x, point2.x);
			float left   = Min(point1.x, point2.x);
			float bottom = Min(point1.y, point2.y);
			float top    = Max(point1.y, point2.y);

			if(v1.x <= right && v1.x >= left && v1.y >= bottom && v1.y <= top)
			{
				resultCount+=1;
			}

			if(v2.x <= right && v2.x >= left && v2.y >= bottom && v2.y <= top)
			{
				resultCount+=2;
			}

			switch(resultCount)
			{
				case 1:
					crossPoint1 = v1;
					crossPoint2 = v1;
					return 1;
				case 2:
					crossPoint1 = v2;
					crossPoint2 = v2;
					return 2;
				case 3:
					// 近い方から入れる
					if(Distance(v1.x, v1.y, point1.x, point1.y) <= Distance(v2.x, v2.y, point1.x, point1.y))
					{
						crossPoint1 = v1;
						crossPoint2 = v2;
					}
					else
					{
						crossPoint1 = v2;
						crossPoint2 = v1;
					}
					return 3;
			}

			return 0;
		}

		/// <summary>矩形と円</summary>
		public static bool IsBoxAndCircle(Vector3 boxPosition, Vector3 boxSize, float boxRotation, Pivot boxPivot, Vector3 circlePosition, float radius)
		{
			float w = boxSize.x * 0.5f;
			float h = boxSize.y * 0.5f;

			// 回転がある場合
			if(boxRotation != 0)
			{
				circlePosition = Rotate(circlePosition, boxPosition, -boxRotation);
			}
			// Pivot計算
			boxPosition = AddVector(boxPosition, GetPivotOffset(boxSize, boxPivot) );

			Vector3 pos = circlePosition;

			if(circlePosition.x < boxPosition.x - w)
			{
				circlePosition.x = boxPosition.x - w;
			}
			else if(circlePosition.x > boxPosition.x + w)
			{
				circlePosition.x = boxPosition.x + w;
			}

			if(circlePosition.y < boxPosition.y - h)
			{
				circlePosition.y = boxPosition.y - h;
			}
			else if(circlePosition.y > boxPosition.y + h)
			{
				circlePosition.y = boxPosition.y + h;
			}

			float dis = Distance(circlePosition.x, circlePosition.y, pos.x, pos.y);
			return dis <= radius * radius;
		}


		/// <summary>矩形と点</summary>
		public static bool IsBoxAndPoint(Vector3 boxPoint, Vector3 boxSize, Pivot boxPivot, Vector3 point)
		{
			Vector3 offset = GetPivotOffset(boxSize, boxPivot);
			float left   = boxPoint.x + offset.x - boxSize.x * 0.5f;
			float right  = left + boxSize.x;
			float bottom = boxPoint.y + offset.y - boxSize.y * 0.5f;
			float top    = bottom + boxSize.y;

			return
				left <= point.x && right >= point.x &&
				bottom <= point.y && top >= point.y;
		}

		/// <summary>矩形と点</summary>
		public static bool IsBoxAndPoint(Vector3 boxPoint, Vector3 boxSize, float boxRotation, Pivot boxPivot, Vector3 point)
		{
			// 回転がない場合
			if(boxRotation % 180 == 0)
			{
				return IsBoxAndPoint(boxPoint, boxSize, boxPivot, point);
			}
			// 回転
			point = Rotate(point, boxPoint, -boxRotation);
			return IsBoxAndPoint(boxPoint, boxSize, boxPivot, point);
		}

		/// <summary>円と円の判定</summary>
		public static bool IsCircleAndCircle(Vector3 circlePoint1, float radius1, Vector3 circlePoint2, float radius2)
		{
			return Distance(circlePoint1.x, circlePoint1.y, circlePoint2.x, circlePoint2.y) <= (radius1 + radius2) * (radius1 + radius2);
		}

		/// <summary>点と円の判定</summary>
		public static bool IsCircleAndPoint(Vector3 circlePoint, float radius, Vector3 point)
		{
			return Distance(point.x, point.y, circlePoint.x, circlePoint.y) <= radius * radius;
		}



		/// <summary>円弧と円の判定</summary>
		public static bool IsArcAndCircle(Vector3 arcPoint, float arcRadius, float angleFrom, float angle, Vector3 circlePoint, float circleRadius)
		{
			// 円同士で判定
			if(IsCircleAndCircle(arcPoint, arcRadius, circlePoint, circleRadius) == false)return false;


			// 円弧のベクトル生成
			Vector3 v1 = arcPoint;
			v1.z += arcRadius;
			Vector3 v2 = Rotate(v1, arcPoint, -angleFrom);
			Vector3 v3 = Rotate(v1, arcPoint, -(angleFrom + angle));
			// 円弧のベクトルと判定
			if(IsLineAndCircle(arcPoint, v2, circlePoint, circleRadius) || IsLineAndCircle(arcPoint, v3, circlePoint, circleRadius))return true;
			// 円の中心が円弧内にあるか（角度が
			float circleAngle = Angle(arcPoint.x, arcPoint.y, circlePoint.x, circlePoint.y);
			// 範囲内
			if(circleAngle <= angleFrom && circleAngle >= angleFrom - angle)return true;
			// 円と円弧のベクトルとの点
			if(IsCircleAndPoint(circlePoint, circleRadius, v2) || IsCircleAndPoint(circlePoint, circleRadius, v3) )return true;
			return false;
		}

		/// <summary>円弧と点の判定</summary>
		public static bool IsArcAndPoint(Vector3 arcPoint, float radius, float angleFrom, float angle, Vector3 point)
		{
			// 円の当たり判定
			if(IsCircleAndPoint(arcPoint, radius, point) == false)return false;
			// 角度
			float pointAngle = Angle(arcPoint.x, arcPoint.y, point.x, point.y);
			// 範囲外
			if(pointAngle > angleFrom || pointAngle < angleFrom - angle)return false;
			return true;
		}

		/// <summary>
		/// 円錐と点の当たり判定(conePointから見て順・反時計回りで2等分)
		/// </summary>
		public static bool IsArcAndPoint(Vector3 arcPoint, float arcRadius, Vector3 arcDir, float arcAngle, Vector3 point)
		{
			if (!IsCircleAndPoint(arcPoint, arcRadius, point))
			{
				return false;
			}
			if (arcAngle >= 360)
			{
				return true;
			}

			// var dirAngle = MathF.Atan2(arcDir.y, arcDir.x) * Rad2Deg;
			// var fromAngle = dirAngle - arcAngle / 2;
			// if (fromAngle < 0)
			// {
			// 	fromAngle = fromAngle + 360;
			// }
			// var toAngle = dirAngle + arcAngle / 2;
			
			return  Vector3.Angle(arcDir, point - arcPoint) <= arcAngle / 2;
		}

		/// <summary>点と多角形の判定</summary>
		public static bool IsPolygonAndPoint(Vector3[] points, Vector3 point)
		{
			// 交差回数
			int crossingCount = 0;

			Vector3 p0 = points[0];
			bool bFlag0x = (point.x <= p0.x);
			bool bFlag0y = (point.y <= p0.y);

			// レイの方向は、Ｘプラス方向
			for(int i=1;i<points.Length+1;i++)
			{
				Vector3 p1 = points[i%points.Length];
				bool bFlag1x = (point.x <= p1.x);
				bool bFlag1y = (point.y <= p1.y);
				if(bFlag0y != bFlag1y)
				{
					// 線分はレイを横切る可能性あり。
					if(bFlag0x == bFlag1x)
					{
						// 線分の２端点は対象点に対して両方右か両方左にある
						if(bFlag0x)
						{
							// 完全に右。⇒線分はレイを横切る
							// 上から下にレイを横切るときには、交差回数を１引く、下から上は１足す。
							crossingCount += (bFlag0y ? -1 : 1);
						}
					}
					else
					{
						// レイと交差するかどうか、対象点と同じ高さで、対象点の右で交差するか、左で交差するかを求める。
						if( point.x <= ( p0.x + (p1.x - p0.x) * (point.z - p0.y ) / (p1.z - p0.y) ) )
						{
							// 線分は、対象点と同じ高さで、対象点の右で交差する。⇒線分はレイを横切る
							// 上から下にレイを横切るときには、交差回数を１引く、下から上は１足す。
							crossingCount += (bFlag0y ? -1 : 1);
						}
					}
				}
				// 次の判定のために、
				p0 = p1;
				bFlag0x = bFlag1x;
				bFlag0y = bFlag1y;
			}

			// クロスカウントがゼロのとき外、ゼロ以外のとき内。
			return (0 != crossingCount);
		}
    }
}