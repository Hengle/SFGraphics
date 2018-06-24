﻿using System;
using System.IO;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Text;

namespace SFGraphics.GLObjects.Shaders
{
    /// <summary>
    /// Encapsulates a shader program and attached shaders. 
    /// Errors are stored to an internal log, which can be exported with <see cref="GetErrorLog"/>.
    /// <para></para> <para></para>
    /// Ensure that <see cref="ProgramCreatedSuccessfully"/> returns <c>true</c> before rendering to avoid crashes.
    /// </summary>
    public sealed class Shader : IGLObject
    {
        /// <summary>
        /// The ID generated by GL.CreateProgram().
        /// </summary>
        public int Id { get; }

        // True when all shaders compiled and linked correctly.
        private bool programStatusIsOk = false;
        private bool hasCheckedProgramCreation = false;

        // GL.CreateShader() will not return 0 if shader creation was successful.
        private int vertShaderId = 0;
        private int fragShaderId = 0;
        private int geomShaderId = 0;


        private ShaderLog errorLog = new ShaderLog();

        // Vertex Attributes and Uniforms
        int activeUniformCount = 0;
        int activeAttributeCount = 0;
        private Dictionary<string, int> vertexAttributeAndUniformLocations = new Dictionary<string, int>();

        // Write these names to the error log later rather than throwing an exception.
        private HashSet<string> invalidUniformNames = new HashSet<string>();

        /// <summary>
        /// Initializes the programID. Attach and compile shaders with LoadShader() before using.
        /// </summary>
        public Shader()
        {
            Id = GL.CreateProgram();
            GLObjectManager.AddReference(GLObjectManager.referenceCountByProgramId, Id);
            errorLog.AppendHardwareAndVersionInfo();
        }

        /// <summary>
        /// Decrement the reference count for <see cref="Id"/>. 
        /// The context probably isn't current, so the data is deleted later by <see cref="GLObjectManager"/>.
        /// </summary>
        ~Shader()
        {
            GLObjectManager.RemoveReference(GLObjectManager.referenceCountByProgramId, Id);
        }

        /// <summary>
        /// Names not present in the shader are ignored and saved to the error log.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetFloat(string uniformName, float value)
        {
            if (!vertexAttributeAndUniformLocations.ContainsKey(uniformName) && !invalidUniformNames.Contains(uniformName))
            {
                invalidUniformNames.Add(uniformName);
                return;
            }

            GL.Uniform1(GetVertexAttributeUniformLocation(uniformName), value);
        }

        /// <summary>
        /// Names not present in the shader are ignored and saved to the error log.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetInt(string uniformName, int value)
        {
            if (!vertexAttributeAndUniformLocations.ContainsKey(uniformName) && !invalidUniformNames.Contains(uniformName))
            {
                invalidUniformNames.Add(uniformName);
                return;
            }

            GL.Uniform1(GetVertexAttributeUniformLocation(uniformName), value);
        }

        /// <summary>
        /// Names not present in the shader are ignored and saved to the error log.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetUint(string uniformName, uint value)
        {
            if (!vertexAttributeAndUniformLocations.ContainsKey(uniformName) && !invalidUniformNames.Contains(uniformName))
            {
                invalidUniformNames.Add(uniformName);
                return;
            }

            GL.Uniform1(GetVertexAttributeUniformLocation(uniformName), value);
        }

        /// <summary>
        /// Names not present in the shader are ignored and saved to the error log.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform. True = 1. False = 0.</param>
        public void SetBoolToInt(string uniformName, bool value)
        {
            if (!vertexAttributeAndUniformLocations.ContainsKey(uniformName) && !invalidUniformNames.Contains(uniformName))
            {
                invalidUniformNames.Add(uniformName);
                return;
            }

            // if/else is faster than the ternary operator. 
            if (value)
                GL.Uniform1(GetVertexAttributeUniformLocation(uniformName), 1);
            else
                GL.Uniform1(GetVertexAttributeUniformLocation(uniformName), 0);
        }

        /// <summary>
        /// Names not present in the shader are ignored and saved to the error log.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetVector2(string uniformName, Vector2 value)
        {
            if (!vertexAttributeAndUniformLocations.ContainsKey(uniformName) && !invalidUniformNames.Contains(uniformName))
            {
                invalidUniformNames.Add(uniformName);
                return;
            }

            GL.Uniform2(GetVertexAttributeUniformLocation(uniformName), value);
        }

        /// <summary>
        /// Names not present in the shader are ignored and saved to the error log.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="x"></param>        
        /// <param name="y"></param>
        public void SetVector2(string uniformName, float x, float y)
        {
            SetVector2(uniformName, new Vector2(x, y));
        }

        /// <summary>
        /// Names not present in the shader are ignored and saved to the error log.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetVector3(string uniformName, Vector3 value)
        {
            if (!vertexAttributeAndUniformLocations.ContainsKey(uniformName) && !invalidUniformNames.Contains(uniformName))
            {
                invalidUniformNames.Add(uniformName);
                return;
            }

            GL.Uniform3(GetVertexAttributeUniformLocation(uniformName), value);
        }

        /// <summary>
        /// Names not present in the shader are ignored and saved to the error log.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="x"></param>        
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void SetVector3(string uniformName, float x, float y, float z)
        {
            SetVector3(uniformName, new Vector3(x, y, z));
        }

        /// <summary>
        /// Names not present in the shader are ignored and saved to the error log.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetVector4(string uniformName, Vector4 value)
        {
            if (!vertexAttributeAndUniformLocations.ContainsKey(uniformName) && !invalidUniformNames.Contains(uniformName))
            {
                invalidUniformNames.Add(uniformName);
                return;
            }

            GL.Uniform4(GetVertexAttributeUniformLocation(uniformName), value);
        }

        /// <summary>
        /// Names not present in the shader are ignored and saved to the error log.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="w"></param>
        public void SetVector4(string uniformName, float x, float y, float z, float w)
        {
            if (!vertexAttributeAndUniformLocations.ContainsKey(uniformName) && !invalidUniformNames.Contains(uniformName))
            {
                invalidUniformNames.Add(uniformName);
                return;
            }

            GL.Uniform4(GetVertexAttributeUniformLocation(uniformName), x, y, z, w);
        }

        /// <summary>
        /// Names not present in the shader are ignored and saved to the error log.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetMatrix4x4(string uniformName, ref Matrix4 value)
        {
            if (!vertexAttributeAndUniformLocations.ContainsKey(uniformName) && !invalidUniformNames.Contains(uniformName))
            {
                invalidUniformNames.Add(uniformName);
                return;
            }

            GL.UniformMatrix4(GetVertexAttributeUniformLocation(uniformName), false, ref value);
        }

        /// <summary>
        /// Gets the index of the vertex attribute or uniform variable. Returns -1 if not found.
        /// </summary>
        /// <param name="name">The name of the vertex attribute or uniform</param>
        /// <returns>The index of the attribute/uniform or -1 if not found </returns>
        public int GetVertexAttributeUniformLocation(string name)
        {
            int value;
            if (vertexAttributeAndUniformLocations.TryGetValue(name, out value))
            {
                return value;
            }
            else
                return -1;
        }

        /// <summary>
        /// Enables the vertex attribute arrays for all active attributes.
        /// Ensures that extra attributes aren't enabled, which causes crashes on Geforce drivers.
        /// </summary>
        public void EnableVertexAttributes()
        {
            // Only enable the necessary vertex attributes.
            // Enabling extra vertex attributes crashes on Nvidia.
            for (int location = 0; location < activeAttributeCount; location++)
            {
                GL.EnableVertexAttribArray(location);
            }
        }

        /// <summary>
        /// Disables the vertex attribute arrays for all active attributes.
        /// Ensures that extra attributes aren't enabled, which causes crashes on Geforce drivers.
        /// </summary>
        public void DisableVertexAttributes()
        {
            // Disable all the vertex attributes. This could be used with a VAO in the future.
            for (int location = 0; location < activeAttributeCount; location++)
            {
                GL.DisableVertexAttribArray(location);
            }
        }

        /// <summary>
        /// Gets the error log containing hardware info, version number, compilation/linker errors, 
        /// and attempts to initialize invalid uniform or vertex attribute names.
        /// </summary>
        /// <returns>A String of all detected errors</returns>
        public string GetErrorLog()
        {
            // Don't append program errors until all the shaders are attached and compiled.
            errorLog.AppendProgramInfoLog(Id);

            // Collect all of the spelling mistakes.
            errorLog.AppendUniformNameErrors(invalidUniformNames);

            return errorLog.ToString();
        }

        private void AddVertexAttribute(string name)
        {
            if (vertexAttributeAndUniformLocations.ContainsKey(name))
                vertexAttributeAndUniformLocations.Remove(name);
            int position = GL.GetAttribLocation(Id, name);
            vertexAttributeAndUniformLocations.Add(name, position);
        }

        private void AddUniform(string name)
        {
            if (vertexAttributeAndUniformLocations.ContainsKey(name))
                vertexAttributeAndUniformLocations.Remove(name);
            int position = GL.GetUniformLocation(Id, name);
            vertexAttributeAndUniformLocations.Add(name, position);
        }

        private void LoadUniforms()
        {
            GL.GetProgram(Id, GetProgramParameterName.ActiveUniforms, out activeUniformCount);

            for (int i = 0; i < activeUniformCount; i++)
            {
                // Ignore invalid uniforms. 0 is "None" for type.
                ActiveUniformType uniformType;
                int uniformSize;
                string uniform = GL.GetActiveUniform(Id, i, out uniformSize, out uniformType);
                if (uniformType != 0)
                {
                    AddUniform(uniform);
                }
            }
        }

        private void LoadAttributes()
        {
            GL.GetProgram(Id, GetProgramParameterName.ActiveAttributes, out activeAttributeCount);

            for (int i = 0; i < activeAttributeCount; i++)
            {
                // Ignore invalid attributes.
                ActiveAttribType attributeType;
                int attributeSize;
                string attribute = GL.GetActiveAttrib(Id, i, out attributeSize, out attributeType);
                if (attributeType != ActiveAttribType.None)
                {
                    AddVertexAttribute(attribute);
                }
            }
        }

        /// <summary>
        /// Attempts to compile and attach the shader. 
        /// The value returned by <see cref="ProgramCreatedSuccessfully"/> is updated.
        /// Supported shader types are fragment, vertex , and geometry.
        /// </summary>
        /// <param name="shaderSource">A string containing the shader text</param>
        /// <param name="shaderType">Supported types are ShaderType.FragmentShader, ShaderType.VertexShader, or ShaderType.GeometryShader</param>
        /// <param name="shaderName">The title used for the compilation errors section of the error log</param>
        public void LoadShader(string shaderSource, ShaderType shaderType, string shaderName = "Shader")
        {
            // Compile and attach before linking.
            int shaderId = LoadShaderBasedOnType(shaderSource, shaderType);
            AppendShaderCompilationErrors(shaderName, shaderId);
            GL.LinkProgram(Id);

            // Some errors may not appear until all shaders are loaded.
            programStatusIsOk = CheckProgramStatus();

            // The shader won't be deleted until the program is deleted.
            GL.DeleteShader(shaderId);

            LoadAttributes();
            LoadUniforms();
        }

        private int LoadShaderBasedOnType(string shaderSource, ShaderType shaderType)
        {
            // Returns the shader Id that was generated.
            switch (shaderType)
            {
                case ShaderType.FragmentShader:
                    AttachAndCompileShader(shaderSource, shaderType, Id, out fragShaderId);
                    return fragShaderId;
                case ShaderType.VertexShader:
                    AttachAndCompileShader(shaderSource, shaderType, Id, out vertShaderId);
                    return vertShaderId;
                case ShaderType.GeometryShader:
                    AttachAndCompileShader(shaderSource, shaderType, Id, out geomShaderId);
                    return geomShaderId;
                default:
                    // No compute shaders or anything fancy like that currently.
                    throw new NotSupportedException(shaderType.ToString());
            }
        }

        private void AttachAndCompileShader(string shaderText, ShaderType type, int program, out int id)
        {
            id = GL.CreateShader(type);
            GL.ShaderSource(id, shaderText);
            GL.CompileShader(id);
            GL.AttachShader(program, id);
        }

        private void AppendShaderCompilationErrors(string shaderName, int id)
        {
            errorLog.AppendShaderInfoLog(shaderName, id);
        }

        /// <summary>
        /// Returns <c>true</c> when the link status is ok and all attached shaders compiled.
        /// If <c>false</c>, rendering with this shader will most likely cause a crash.
        /// <para></para><para></para>
        /// The status is only updated the first time <see cref="ProgramCreatedSuccessfully"/> is called 
        /// and with each call to <see cref="LoadShader(string, ShaderType, string)"/>,
        /// so there is little cost in checking this method frequently.
        /// </summary>
        public bool ProgramCreatedSuccessfully()
        {
            // Only check once for performance reasons.
            if (!hasCheckedProgramCreation)
            {
                programStatusIsOk = CheckProgramStatus();
                hasCheckedProgramCreation = true;
            }
            return programStatusIsOk;
        }

        private bool CheckProgramStatus()
        {
            // Check for linker errors first. 
            bool programLinkedSuccessfully = ProgramLinked();
            if (!programLinkedSuccessfully)
                return false;

            // Check fragment and vertex shader compilation.
            bool vertShaderCompiled = ShaderCompiled(vertShaderId);
            bool fragShaderCompiled = ShaderCompiled(fragShaderId);

            // Only check the geometry shader if present.
            bool geomShaderCompiled = true;
            if (geomShaderId != 0)
                geomShaderCompiled = ShaderCompiled(geomShaderId);

            // The program was linked, but the shaders may have syntax errors.
            return (fragShaderCompiled && vertShaderCompiled && geomShaderCompiled);
        }

        private bool ProgramLinked()
        {
            // 1: linked successfully. 0: linker errors
            int linkStatus = 1;
            GL.GetProgram(Id, GetProgramParameterName.LinkStatus, out linkStatus);
            return linkStatus != 0;
        }

        private bool ShaderCompiled(int shaderId)
        {
            // 1: shader compiled. 0: compilation errors
            int compileStatus = 1;
            GL.GetShader(shaderId, ShaderParameter.CompileStatus, out compileStatus);
            return compileStatus != 0;
        }
    }
}

