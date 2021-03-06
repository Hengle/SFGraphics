﻿using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GLObjects.VertexArrays
{
    /// <summary>
    /// Encapsulates an OpenGL vertex array object. 
    /// Vertex array objects cannot be shared between contexts.
    /// </summary>
    public sealed class VertexArrayObject : GLObject
    {
        internal override GLObjectType ObjectType { get { return GLObjectType.VertexArrayObject; } }

        /// <summary>
        /// Creates an empty vertex array object.
        /// The vertex array object must first be bound with <see cref="Bind"/>.
        /// </summary>
        public VertexArrayObject() : base(GL.GenVertexArray())
        {

        }

        /// <summary>
        /// Binds the vertex array <see cref="GLObject.Id"/>
        /// </summary>
        public void Bind()
        {
            GL.BindVertexArray(Id);
        }

        /// <summary>
        /// Binds the default vertex array value of <c>0</c>.
        /// </summary>
        public void Unbind()
        {
            GL.BindVertexArray(0);
        }
    }
}
