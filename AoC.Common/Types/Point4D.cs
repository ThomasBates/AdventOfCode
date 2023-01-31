using System;

namespace AoC.Common.Types;

public class Point4D : IEquatable<Point4D>
{
	private long x;
	private long y;
	private long z;
	private long t;

	public Point4D(long x, long y, long z, long t)
	{
		X = x;
		Y = y;
		Z = z;
		T = t;
	}
	public long X { get => x; set => x = value; }
	public long Y { get => y; set => y = value; }
	public long Z { get => z; set => z = value; }
	public long T { get => t; set => t = value; }

	public static bool Equals(Point4D p1, Point4D p2)
	{
		if ((object)p1 == p2)	//	reference check
			return true;
		if (p1 is null || p2 is null)
			return false;
		return p1.x == p2.x && p1.y == p2.y && p1.z == p2.z && p1.t == p2.t;
	}
	public bool Equals(Point4D p)
	{
		return Equals(this, p);
	}
	public override bool Equals(object obj)
	{
		if (obj is not Point4D p) return false;
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
			hash = hash * 23 + z.GetHashCode();
			hash = hash * 23 + t.GetHashCode();
			return hash;
		}
	}
	public override string ToString() => $"<{X},{Y},{Z},{T}>";

	public static Point4D operator +(Point4D p1, Point4D p2) => new(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z, p1.T + p2.T);
	public static Point4D operator -(Point4D p1, Point4D p2) => new(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z, p1.T - p2.T);
	public static bool operator ==(Point4D p1, Point4D p2) => Equals(p1, p2);
	public static bool operator !=(Point4D p1, Point4D p2) => !Equals(p1, p2);

	public long ManhattanSize() => Math.Abs(x) + Math.Abs(y) + Math.Abs(z) + Math.Abs(t);
	public long ManhattanDistance(Point4D p) => Math.Abs(x - p.x) + Math.Abs(y - p.y) + Math.Abs(z - p.z) + Math.Abs(t - p.t);
}
