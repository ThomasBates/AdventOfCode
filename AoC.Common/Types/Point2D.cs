using System;

namespace AoC.Common.Types;

public class Point2D : IEquatable<Point2D>
{
	private long x;
	private long y;

	public Point2D(long x, long y)
	{
		X = x;
		Y = y;
	}
	public long X { get => x; set => x = value; }
	public long Y { get => y; set => y = value; }

	public static bool Equals(Point2D p1, Point2D p2)
	{
		if ((object)p1 == p2)   //	reference check
			return true;
		if (p1 is null || p2 is null)
			return false;
		return p1.x == p2.x && p1.y == p2.y;
	}
	public bool Equals(Point2D p)
	{
		return Equals(this, p);
	}
	public override bool Equals(object obj)
	{
		if (obj is not Point2D p) return false;
		return Equals(this, p);
	}
	public override int GetHashCode()
	{
		//	https://stackoverflow.com/a/263416
		unchecked // Overflow is fine, just wrap
		{
			int hash = 17;
			// Suitable nullity checks etc, of course :)
			hash = hash * 23 + x.GetHashCode();
			hash = hash * 23 + y.GetHashCode();
			return hash;
		}
	}
	public override string ToString() => $"<{X},{Y}>";

	public static Point2D operator +(Point2D p1, Point2D p2) => new(p1.X + p2.X, p1.Y + p2.Y);
	public static Point2D operator -(Point2D p1, Point2D p2) => new(p1.X - p2.X, p1.Y - p2.Y);
	public static bool operator ==(Point2D p1, Point2D p2) => Equals(p1, p2);
	public static bool operator !=(Point2D p1, Point2D p2) => !Equals(p1, p2);

	public long ManhattanSize() => Math.Abs(x) + Math.Abs(y);
	public long ManhattanDistance(Point2D p) => Math.Abs(x - p.x) + Math.Abs(y - p.y);
}
