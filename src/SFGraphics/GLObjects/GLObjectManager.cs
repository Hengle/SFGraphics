﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace SFGraphics.GLObjects
{
    /// <summary>
    /// Manages reference counting and resource management for all GLObjects. 
    /// Each GLObject has an ID generated by OpenTK. When an object is created, the reference count for that ID is incremented.
    /// When the finalizer is called for an object, the reference count is decremented. 
    /// <see cref="GLObjectManager"/> will maintain reference counts for any class inheriting from <see cref="Textures.Texture"/> or
    /// <see cref="BufferObject"/>. 
    /// <para>
    /// Call <see cref="DeleteUnusedGLObjects"/> with a valid OpenTK context current to delete objects with no references.
    /// The reference count is managed automatically, so there is no need to call GL.DeleteTexture(int), for example.
    /// </para>
    /// </summary>
    public static class GLObjectManager
    {
        // Use internal methods/variables so people can't mess this up.
        internal static ConcurrentDictionary<int, int> referenceCountByTextureId = new ConcurrentDictionary<int, int>();
        internal static ConcurrentDictionary<int, int> referenceCountByBufferId = new ConcurrentDictionary<int, int>();

        internal static void AddReference(ConcurrentDictionary<int, int> referenceCountById, int id)
        {
            if (referenceCountById.ContainsKey(id))
                referenceCountById[id] += 1;
            else
                referenceCountById.TryAdd(id, 1);
        }

        internal static void RemoveReference(ConcurrentDictionary<int, int> referenceCountById, int id)
        {
            // Don't allow negative references just in case.
            if (referenceCountById.ContainsKey(id))
            {
                if (referenceCountById[id] > 0)
                    referenceCountById[id] -= 1;
            }
        }

        /// <summary>
        /// The appropriate GL.Delete() function is called for all GLObjects if the <c>ID</c> has 0 references.
        /// This means GLObjects may not be cleaned up until long after the original object becomes unreachable.
        /// <para>
        /// Call <see cref="GC.WaitForPendingFinalizers"/> prior to <see cref="DeleteUnusedGLObjects"/> if more 
        /// immediate cleanup is desired.
        /// </para>
        /// </summary>
        public static void DeleteUnusedGLObjects()
        {
            DeleteUnusedTextures();
            DeleteUnusedBuffers();
        }

        private static void DeleteUnusedTextures()
        {
            HashSet<int> idsReadyForDeletion = FindIdsWithNoReferences(referenceCountByTextureId);

            // Remove any IDs with no more references.
            // Only delete the texture if the ID was present.
            foreach (int id in idsReadyForDeletion)
            {
                int value;
                if (referenceCountByTextureId.TryRemove(id, out value))
                    GL.DeleteTexture(id);
            }
        }

        private static void DeleteUnusedBuffers()
        {
            HashSet<int> idsReadyForDeletion = FindIdsWithNoReferences(referenceCountByBufferId);

            // Remove any IDs with no more references.
            // Only delete the texture if the ID was present.
            foreach (int id in idsReadyForDeletion)
            {
                int value;
                if (referenceCountByBufferId.TryRemove(id, out value))
                    GL.DeleteBuffer(id);
            }
        }

        private static HashSet<int> FindIdsWithNoReferences(ConcurrentDictionary<int, int> referenceCountById)
        {
            HashSet<int> idsReadyForDeletion = new HashSet<int>();
            foreach (var glObject in referenceCountById)
            {
                if (glObject.Value == 0)
                {
                    idsReadyForDeletion.Add(glObject.Key);
                }
            }

            return idsReadyForDeletion;
        }
    }
}
