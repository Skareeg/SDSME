using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace MKDS_Course_Editor.Export3DTools
{
	public class OBJWriter
	{
		private List<Vector3> Vertices = new List<Vector3>();
		private List<Vector3> Normals = new List<Vector3>();
		private List<Vector2> TexCoords = new List<Vector2>();
		/*public void AddTriangles(Vector3[] Vertices, Vector2[] TexCoords, Vector3[] Normals)
		{
			this.Vertices.AddRange(Vertices);
			this.TexCoords.AddRange(TexCoords);
			this.Normals.AddRange(Normals);
		}*/
		public void AddTriangle(Vector3[] Vertice)
		{
			Vertices.AddRange(Vertice);
		}
		public void AddTriangle(Vector3[] Vertice, Vector2 TexCoord)
		{
			Vertices.AddRange(Vertice);
			TexCoords.Add(TexCoord);
		}
		public void AddTriangle(Vector3[] Vertice, Vector3 Normal)
		{
			Vertices.AddRange(Vertice);
			Normals.Add(Normal);
		}
		public void AddTriangle(Vector3[] Vertice, Vector2 TexCoord, Vector3 Normal)
		{
			Vertices.AddRange(Vertice);
			TexCoords.Add(TexCoord);
			Normals.Add(Normal);
		}

	}
	public class Triangle : Face
	{
		public System.Windows.Media.Media3D.Point3D[] Vertex = new System.Windows.Media.Media3D.Point3D[3];
		public System.Windows.Media.Media3D.Vector3D[] Normal = new System.Windows.Media.Media3D.Vector3D[3];
		public System.Windows.Point[] TexCoord = new System.Windows.Point[3];
	}
	public class TriangleStrip : Face
	{
		public List<System.Windows.Media.Media3D.Point3D> Vertex = new List<System.Windows.Media.Media3D.Point3D>();
		public List<System.Windows.Media.Media3D.Vector3D> Normal = new List<System.Windows.Media.Media3D.Vector3D>();
		public List<System.Windows.Point> TexCoord = new List<System.Windows.Point>();
	}
	public class Quad : Face
	{
		public System.Windows.Media.Media3D.Point3D[] Vertex = new System.Windows.Media.Media3D.Point3D[4];
		public System.Windows.Media.Media3D.Vector3D[] Normal = new System.Windows.Media.Media3D.Vector3D[4];
		public System.Windows.Point[] TexCoord = new System.Windows.Point[4];
	}
	public class QuadStrip : Face
	{
		public List<System.Windows.Media.Media3D.Point3D> Vertex = new List<System.Windows.Media.Media3D.Point3D>();
		public List<System.Windows.Media.Media3D.Vector3D> Normal = new List<System.Windows.Media.Media3D.Vector3D>();
		public List<System.Windows.Point> TexCoord = new List<System.Windows.Point>();
	}
	public class Face
	{

	}
}
