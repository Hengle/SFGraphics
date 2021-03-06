﻿using OpenTK;
using OpenTK.Graphics.OpenGL;
using SFGenericModel;
using SFGenericModel.VertexAttributes;
using System.Collections.Generic;
using SFGenericModel.ShaderGenerators;
using System;

namespace SFShapes
{
    /// <summary>
    /// Draws simple geometry given a collection of vertex positions.
    /// </summary>
    public class Mesh3D : GenericMesh<Vector3>
    {
        public Mesh3D(List<Vector3> vertices, PrimitiveType primitiveType) : base(vertices, primitiveType)
        {

        }

        public Mesh3D(Tuple<List<Vector3>, PrimitiveType> vertices) : base(vertices.Item1, vertices.Item2)
        {
        }

        public override List<VertexAttribute> GetVertexAttributes()
        {
            return new List<VertexAttribute>()
            {
                new VertexFloatAttribute("position", ValueCount.Three, VertexAttribPointerType.Float)
            };
        }

        public List<VertexAttributeRenderInfo> GetRenderAttributes()
        {
            return new List<VertexAttributeRenderInfo>()
            {
                new VertexAttributeRenderInfo(GetVertexAttributes()[0], true, true)
            };
        }
    }
}
