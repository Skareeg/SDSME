namespace HelixToolkit
{
    using LibNDSFormats.NSBMD;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;

    public class ObjExporter : Exporter
    {
        private readonly string directory;
        private int groupNo;
        private int matNo;
        private readonly StreamWriter mwriter;
        private int normalIndex;
        private int objectNo;
        private int textureIndex;
        private int vertexIndex;
        private readonly StreamWriter writer;

        public ObjExporter(string outputFileName) : this(outputFileName, null)
        {
        }

        public ObjExporter(string outputFileName, string comment)
        {
            this.groupNo = 1;
            this.matNo = 1;
            this.normalIndex = 1;
            this.objectNo = 1;
            this.textureIndex = 1;
            this.vertexIndex = 1;
            string fullPath = Path.GetFullPath(outputFileName);
            string path = Path.ChangeExtension(outputFileName, ".mtl");
            string fileName = Path.GetFileName(path);
            this.directory = Path.GetDirectoryName(fullPath);
            this.writer = new StreamWriter(outputFileName);
            this.mwriter = new StreamWriter(path);
            if (!string.IsNullOrEmpty(comment))
            {
                this.writer.WriteLine(string.Format("# {0}", comment));
            }
            this.writer.WriteLine("mtllib " + fileName);
        }

        public override void Close()
        {
            this.writer.Close();
            this.mwriter.Close();
            base.Close();
        }

        private void ExportMaterial(string matName, Material material, Material backMaterial)
        {
            SolidColorBrush brush;
            this.mwriter.WriteLine(string.Format("newmtl {0}", matName));
            DiffuseMaterial material2 = material as DiffuseMaterial;
            SpecularMaterial material3 = material as SpecularMaterial;
            MaterialGroup group = material as MaterialGroup;
            if (group != null)
            {
                foreach (Material material4 in group.Children)
                {
                    if (material4 is DiffuseMaterial)
                    {
                        material2 = material4 as DiffuseMaterial;
                    }
                    if (material4 is SpecularMaterial)
                    {
                        material3 = material4 as SpecularMaterial;
                    }
                }
            }
            if (material2 != null)
            {
                this.mwriter.WriteLine(string.Format("Ka {0}", this.ToColorString(material2.AmbientColor)));
                this.mwriter.WriteLine(string.Format("Kd {0}", this.ToColorString(material2.Color)));
                this.mwriter.WriteLine(string.Format(CultureInfo.InvariantCulture, "d {0:F4}", new object[] { material2.Brush.Opacity }));
                brush = material2.Brush as SolidColorBrush;
                if (brush == null)
                {
                    string str = matName + ".png";
                    string path = Path.Combine(this.directory, str);
                    ImageBrush brush2 = (ImageBrush) material2.Brush;
                    Exporter.RenderBrush(path, material2.Brush, (int) brush2.ImageSource.Width, (int) brush2.ImageSource.Height);
                    this.mwriter.WriteLine(string.Format("map_Kd {0}", str));
                    this.mwriter.WriteLine(string.Format("map_d {0}", str));
                }
            }
            if (material3 != null)
            {
                brush = material3.Brush as SolidColorBrush;
                if (brush != null)
                {
                    this.mwriter.WriteLine(string.Format("Ks {0}", this.ToColorString(brush.Color)));
                }
                this.mwriter.WriteLine(string.Format(CultureInfo.InvariantCulture, "Ns {0:F4}", new object[] { material3.SpecularPower }));
            }
        }

        public void ExportMesh(MeshGeometry3D m, Transform3D t)
        {
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
            Dictionary<int, int> dictionary3 = new Dictionary<int, int>();
            int num = 0;
            foreach (Point3D pointd in m.Positions)
            {
                dictionary.Add(num++, this.vertexIndex++);
                Point3D pointd2 = t.Transform(pointd);
                this.writer.WriteLine(string.Format(CultureInfo.InvariantCulture, "v {0} {1} {2}", new object[] { pointd2.X, pointd2.Y, pointd2.Z }));
            }
            num = 0;
            foreach (Point point in m.TextureCoordinates)
            {
                if (!(double.IsNegativeInfinity(point.X) || double.IsPositiveInfinity(point.Y)))
                {
                    dictionary2.Add(num++, this.textureIndex++);
                    this.writer.WriteLine(string.Format(CultureInfo.InvariantCulture, "vt {0} {1}", new object[] { point.X, point.Y }));
                }
                else
                {
                    num++;
                }
            }
            num = 0;
            foreach (Vector3D vectord in m.Normals)
            {
                if (!(double.IsNegativeInfinity(vectord.X) || double.IsPositiveInfinity(vectord.Y)))
                {
                    dictionary3.Add(num++, this.normalIndex++);
                    this.writer.WriteLine(string.Format(CultureInfo.InvariantCulture, "vn {0} {1} {2}", new object[] { vectord.X, vectord.Y, vectord.Z }));
                }
                else
                {
                    num++;
                }
            }
            for (int i = 0; i < m.TriangleIndices.Count; i += 3)
            {
                int num6;
                int key = m.TriangleIndices[i];
                int num4 = m.TriangleIndices[i + 1];
                int num5 = m.TriangleIndices[i + 2];
                this.writer.WriteLine("f {0}/{1}{2} {3}/{4}{5} {6}/{7}{8}", new object[] { dictionary[key], dictionary2.ContainsKey(key) ? (num6 = dictionary2[key]).ToString() : string.Empty, dictionary3.ContainsKey(key) ? ("/" + (num6 = dictionary3[key]).ToString()) : string.Empty, dictionary[num4], dictionary2.ContainsKey(num4) ? (num6 = dictionary2[num4]).ToString() : string.Empty, dictionary3.ContainsKey(num4) ? ("/" + (num6 = dictionary3[num4]).ToString()) : string.Empty, dictionary[num5], dictionary2.ContainsKey(num5) ? (num6 = dictionary2[num5]).ToString() : string.Empty, dictionary3.ContainsKey(num5) ? ("/" + (num6 = dictionary3[num5]).ToString()) : string.Empty });
            }
        }

        protected override void ExportModel(GeometryModel3D model, Transform3D transform)
        {
            this.writer.WriteLine(string.Format("o {0}", (string) ((DiffuseMaterial) model.Material).GetValue(NsbmdGlRenderer.polyName)));
            this.writer.WriteLine(string.Format("g {0}", (string) ((DiffuseMaterial) model.Material).GetValue(NsbmdGlRenderer.polyName)));
            string str = (string) ((DiffuseMaterial) model.Material).GetValue(NsbmdGlRenderer.matName);
            this.writer.WriteLine(string.Format("usemtl {0}", str));
            this.ExportMaterial(str, model.Material, model.BackMaterial);
            MeshGeometry3D geometry = model.Geometry as MeshGeometry3D;
            this.ExportMesh(geometry, Transform3DHelper.CombineTransform(transform, model.Transform));
        }

        private string ToColorString(Color color)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:F4} {1:F4} {2:F4}", new object[] { ((double) color.R) / 255.0, ((double) color.G) / 255.0, ((double) color.B) / 255.0 });
        }
    }
}

