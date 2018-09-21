﻿using OpenTK;
using OpenTK.Graphics.OpenGL;
using SFGenericModel;
using SFGenericModel.VertexAttributes;
using System.Collections.Generic;
using SFGenericModel.ShaderGenerators;

namespace SFShapes
{
    public class Mesh3D : GenericMesh<Vector3>
    {
        public Mesh3D(List<Vector3> vertices) : base(vertices, PrimitiveType.Triangles)
        {

        }

        public override List<VertexAttributeInfo> GetVertexAttributes()
        {
            return new List<VertexAttributeInfo>()
            {
                new VertexAttributeInfo("position", ValueCount.Three, VertexAttribPointerType.Float)
            };
        }

        public List<VertexAttributeRenderInfo> GetRenderAttributes()
        {
            return new List<VertexAttributeRenderInfo>()
            {
                new VertexAttributeRenderInfo(true, true, GetVertexAttributes()[0])
            };
        }
    }
}
