using System;

namespace AoC.Common.Types;

public class Point3D : IEquatable<Point3D>
{
	private long x;
	private long y;
	private long z;

	public Point3D(long x, long y, long z)
	{
		X = x;
		Y = y;
		Z = z;
	}
	public long X { get => x; set => x = value; }
	public long Y { get => y; set => y = value; }
	public long Z { get => z; set => z = value; }

	public static bool Equals(Point3D p1, Point3D p2)
	{
		if ((object)p1 == p2)   //	reference check
			return true;
		if (p1 is null || p2 is null)
			return false;
		return p1.x == p2.x && p1.y == p2.y && p1.z == p2.z;
	}
	public bool Equals(Point3D p)
	{
		return Equals(this, p);
	}
	public override bool Equals(object obj)
	{
		if (obj is not Point3D p) return false;
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
			return hash;
		}
	}
	public override string ToString() => $"<{X},{Y},{Z}>";

	public static Point3D operator +(Point3D p1, Point3D p2) => new(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
	public static Point3D operator -(Point3D p1, Point3D p2) => new(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
	public static bool operator ==(Point3D p1, Point3D p2) => Equals(p1, p2);
	public static bool operator !=(Point3D p1, Point3D p2) => !Equals(p1, p2);

	public long ManhattanSize() => Math.Abs(x) + Math.Abs(y) + Math.Abs(z);
	public long ManhattanDistance(Point3D p) => Math.Abs(x - p.x) + Math.Abs(y - p.y) + Math.Abs(z - p.z);
}
