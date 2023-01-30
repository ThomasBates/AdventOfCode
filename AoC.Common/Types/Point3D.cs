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

	public bool Equals(Point3D p)
	{
		if (p == null)
			return false;
		return (p.X == X && p.Y == Y && p.Z == Z);
	}
	public override bool Equals(object obj)
	{
		if (obj is not Point3D p) return false;
		return Equals(p);
	}
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
	public override string ToString() => $"<{X},{Y},{Z}>";

	public static Point3D operator +(Point3D p1, Point3D p2) => new(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
	public static Point3D operator -(Point3D p1, Point3D p2) => new(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
	public static bool operator ==(Point3D p1, Point3D p2) => p1.Equals(p2);
	public static bool operator !=(Point3D p1, Point3D p2) => !p1.Equals(p2);

	public long ManhattanSize() => Math.Abs(x) + Math.Abs(y) + Math.Abs(z);
	public long ManhattanDistance(Point3D p) => Math.Abs(x - p.x) + Math.Abs(y - p.y) + Math.Abs(z - p.z);
}
