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

	public bool Equals(Point2D p)
	{
		if (p == null)
			return false;
		return (p.X == X && p.Y == Y);
	}
	public override bool Equals(object obj)
	{
		if (obj is not Point2D p) return false;
		return Equals(p);
	}
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
	public override string ToString() => $"<{X},{Y}>";

	public static Point2D operator +(Point2D p1, Point2D p2) => new(p1.X + p2.X, p1.Y + p2.Y);
	public static Point2D operator -(Point2D p1, Point2D p2) => new(p1.X - p2.X, p1.Y - p2.Y);
	public static bool operator ==(Point2D p1, Point2D p2) => p1.Equals(p2);
	public static bool operator !=(Point2D p1, Point2D p2) => !p1.Equals(p2);

	public long ManhattanSize() => Math.Abs(x) + Math.Abs(y);
	public long ManhattanDistance(Point2D p) => Math.Abs(x - p.x) + Math.Abs(y - p.y);
}
