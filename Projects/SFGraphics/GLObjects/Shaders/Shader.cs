﻿using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

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

        /// <summary>
        /// <c>true</c> when the link status is ok and all attached shaders compiled.
        /// If <c>false</c>, rendering with this shader will most likely cause an <see cref="AccessViolationException"/>.
        /// <para></para><para></para>
        /// The status is updated with each call to <see cref="LoadShader(string, ShaderType, string)"/> or
        /// <see cref="AttachShader(int, ShaderType)"/>.
        /// </summary>
        public bool ProgramCreatedSuccessfully
        {
            get { return programCreatedSuccessfully; }
        }
        private bool programCreatedSuccessfully = false;


        private ShaderLog errorLog = new ShaderLog();

        // Vertex Attributes and Uniforms
        int activeUniformCount = 0;
        int activeAttributeCount = 0;
        private Dictionary<string, int> vertexAttributeAndUniformLocations = new Dictionary<string, int>();

        // Used to check for type mismatches when setting attributes and uniforms.
        private Dictionary<string, ActiveUniformType> activeUniformTypesByName = new Dictionary<string, ActiveUniformType>();
        private Dictionary<string, ActiveAttribType> activeAttribTypesByName = new Dictionary<string, ActiveAttribType>();

        // Write these names to the error log later rather than throwing an exception.
        private HashSet<string> invalidUniformNames = new HashSet<string>();
        private Dictionary<string, ActiveUniformType> invalidUniformTypes = new Dictionary<string, ActiveUniformType>();

        /// <summary>
        /// Initializes the programID. Attach and compile shaders with LoadShader() before using.
        /// </summary>
        public Shader()
        {
            Id = GL.CreateProgram();
            ReferenceCounting.AddReference(GLObjectManager.referenceCountByProgramId, Id);
            errorLog.AppendHardwareAndVersionInfo();
        }

        /// <summary>
        /// Decrement the reference count for <see cref="Id"/>. 
        /// The context probably isn't current, so the data is deleted later by <see cref="GLObjectManager"/>.
        /// </summary>
        ~Shader()
        {
            ReferenceCounting.RemoveReference(GLObjectManager.referenceCountByProgramId, Id);
        }

        /// <summary>
        /// Use this shader program's program ID for rendering.
        /// </summary>
        public void UseProgram()
        {
            GL.UseProgram(Id);
        }

        private bool UniformTypeAndNameCorrect(string uniformName, ActiveUniformType inputType)
        {
            if (!CorrectUniformAttributeName(uniformName, invalidUniformNames))
                return false;
            else if (!CorrectUniformType(uniformName, inputType))
                return false;

            return true;
        }

        private bool CorrectUniformAttributeName(string name, HashSet<string> invalidNames)
        {
            // Check for spelling mistakes and names optimized out by the compiler.
            // Avoid adding duplicates because this is checked a lot.
            if (!vertexAttributeAndUniformLocations.ContainsKey(name) && !invalidNames.Contains(name))
            {
                invalidNames.Add(name);
                return false;
            }

            return true;
        }

        private bool CorrectUniformType(string name, ActiveUniformType inputType)
        {
            // Check for valid names with type mismatches.
            // Avoid adding duplicates because this is checked a lot.
            if (activeUniformTypesByName.ContainsKey(name))
            {
                bool correctType = activeUniformTypesByName[name] == inputType;
                if (!correctType && !invalidUniformTypes.ContainsKey(name))
                {
                    invalidUniformTypes.Add(name, inputType);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Names not present in the shader are ignored and saved to the error log.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetFloat(string uniformName, float value)
        {
            if (!UniformTypeAndNameCorrect(uniformName, ActiveUniformType.Float))
                return;

            GL.Uniform1(GetVertexAttributeUniformLocation(uniformName), value);
        }

        /// <summary>
        /// Names not present in the shader are ignored and saved to the error log.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetInt(string uniformName, int value)
        {
            if (!UniformTypeAndNameCorrect(uniformName, ActiveUniformType.Int))
                return;

            GL.Uniform1(GetVertexAttributeUniformLocation(uniformName), value);
        }

        /// <summary>
        /// Names not present in the shader are ignored and saved to the error log.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetUint(string uniformName, uint value)
        {
            if (!UniformTypeAndNameCorrect(uniformName, ActiveUniformType.UnsignedInt))
                return;

            GL.Uniform1(GetVertexAttributeUniformLocation(uniformName), value);
        }

        /// <summary>
        /// Names not present in the shader are ignored and saved to the error log.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform. True = 1. False = 0.</param>
        public void SetBoolToInt(string uniformName, bool value)
        {
            if (!UniformTypeAndNameCorrect(uniformName, ActiveUniformType.Int))
                return;

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
            if (!UniformTypeAndNameCorrect(uniformName, ActiveUniformType.FloatVec2))
                return;

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
            if (!UniformTypeAndNameCorrect(uniformName, ActiveUniformType.FloatVec3))
                return;

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
            if (!UniformTypeAndNameCorrect(uniformName, ActiveUniformType.FloatVec4))
                return;

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
            SetVector4(uniformName, new Vector4(x, y, z, w));
        }

        /// <summary>
        /// Names not present in the shader are ignored and saved to the error log.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetMatrix4x4(string uniformName, ref Matrix4 value)
        {
            if (!UniformTypeAndNameCorrect(uniformName, ActiveUniformType.FloatMat4))
                return;

            GL.UniformMatrix4(GetVertexAttributeUniformLocation(uniformName), false, ref value);
        }

        /// <summary>
        /// <paramref name="textureId"/> is bound to <paramref name="textureUnit"/> before 
        /// setting the uniform. Names not present in the shader are ignored and saved to the error log.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="textureId">The integer ID generated by GL.GenTexture()</param>
        /// <param name="textureTarget">The target to which <paramref name="textureId"/> is bound</param>
        /// <param name="textureUnit">The texture unit to which <paramref name="textureId"/> is bound</param>
        public void SetTexture(string uniformName, int textureId, TextureTarget textureTarget, int textureUnit)
        {
            if (!vertexAttributeAndUniformLocations.ContainsKey(uniformName) && !invalidUniformNames.Contains(uniformName))
            {
                invalidUniformNames.Add(uniformName);
                return;
            }

            GL.ActiveTexture(TextureUnit.Texture0 + textureUnit);
            GL.BindTexture(textureTarget, textureId);
            GL.Uniform1(GetVertexAttributeUniformLocation(uniformName), textureUnit);
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
            errorLog.AppendUniformTypeErrors(invalidUniformTypes);

            return errorLog.ToString();
        }

        private void AddVertexAttribute(string name, ActiveAttribType activeAttribType)
        {
            // Overwrite existing vertex attributes.
            if (vertexAttributeAndUniformLocations.ContainsKey(name))
                vertexAttributeAndUniformLocations.Remove(name);
            int position = GL.GetAttribLocation(Id, name);
            vertexAttributeAndUniformLocations.Add(name, position);

            if (activeAttribTypesByName.ContainsKey(name))
                activeAttribTypesByName.Remove(name);
            activeAttribTypesByName.Add(name, activeAttribType);
        }

        private void AddUniform(string name, ActiveUniformType activeUniformType)
        {
            // Overwrite existing uniforms.
            if (vertexAttributeAndUniformLocations.ContainsKey(name))
                vertexAttributeAndUniformLocations.Remove(name);
            int position = GL.GetUniformLocation(Id, name);
            vertexAttributeAndUniformLocations.Add(name, position);

            if (activeUniformTypesByName.ContainsKey(name))
                activeUniformTypesByName.Remove(name);
            activeUniformTypesByName.Add(name, activeUniformType);
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
                    AddUniform(uniform, uniformType);
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
                    AddVertexAttribute(attribute, attributeType);
                }
            }
        }

        /// <summary>
        /// Returns the integer ID created by GL.CreateShader(). Compiles the shader.
        /// This method can reduce load times by avoiding redundant shader compilations when used
        /// in conjunction with <see cref="AttachShader(int, ShaderType)"/>
        /// </summary>
        /// <param name="shaderSource">A string containing the shader source text</param>
        /// <param name="shaderType">Supported types are ShaderType.FragmentShader, ShaderType.VertexShader, or ShaderType.GeometryShader</param>
        /// <returns></returns>
        public static int CreateGlShader(string shaderSource, ShaderType shaderType)
        {
            int id = GL.CreateShader(shaderType);
            GL.ShaderSource(id, shaderSource);
            GL.CompileShader(id);
            return id;
        }

        /// <summary>
        /// Attempts to compile and attach the shader. 
        /// The value returned by <see cref="ProgramCreatedSuccessfully"/> is updated.
        /// Supported shader types are fragment, vertex , and geometry.
        /// </summary>
        /// <param name="shaderSource">A string containing the shader source text</param>
        /// <param name="shaderType">The type of shader to load. Ex: ShaderType.FragmentShader</param>
        /// <param name="shaderName">The title used for the compilation errors section of the error log</param>
        public void LoadShader(string shaderSource, ShaderType shaderType, string shaderName = "Shader")
        {
            // Compile and attach before linking.
            int shaderId = LoadShaderBasedOnType(shaderSource, shaderType);
            AppendShaderCompilationErrors(shaderName, shaderId);
            GL.LinkProgram(Id);

            // Some errors may not appear until all shaders are loaded.
            programCreatedSuccessfully = CheckProgramStatus();

            // The shader won't be deleted until the program is deleted.
            GL.DeleteShader(shaderId);

            // Scary things happen if we do this after a linking error.
            if (programCreatedSuccessfully)
            {
                LoadAttributes();
                LoadUniforms();
            }
        }

        private int LoadShaderBasedOnType(string shaderSource, ShaderType shaderType)
        {
            // Returns the shader Id that was generated.
            int id = AttachAndCompileShader(shaderSource, shaderType, Id);
            return id;
        }

        private int AttachAndCompileShader(string shaderText, ShaderType type, int program)
        {
            int id = CreateGlShader(shaderText, type);
            GL.AttachShader(program, id);
            return id;
        }

        /// <summary>
        /// Attaches <paramref name="shaderId"/> and links the program. 
        /// The value returned by <see cref="ProgramCreatedSuccessfully"/> is updated.
        /// </summary>
        /// <param name="shaderId">The integer ID returned by <see cref="CreateGlShader(string, ShaderType)"/></param>
        /// <param name="shaderType">Supported types are ShaderType.FragmentShader, ShaderType.VertexShader, or ShaderType.GeometryShader</param>
        public void AttachShader(int shaderId, ShaderType shaderType)
        {
            GL.AttachShader(Id, shaderId);
            GL.LinkProgram(Id);

            programCreatedSuccessfully = CheckProgramStatus();

            // Scary things happen if we do this after a linking error.
            if (programCreatedSuccessfully)
            {
                LoadAttributes();
                LoadUniforms();
            }
        }

        private void AppendShaderCompilationErrors(string shaderName, int id)
        {
            errorLog.AppendShaderInfoLog(shaderName, id);
        }

        private bool CheckProgramStatus()
        {
            // Check for linker errors. 
            // Compilation errors in individual shaders will also cause linker errors.
            bool programLinkedSuccessfully = ProgramLinked();
            return programLinkedSuccessfully;
        }

        private bool ProgramLinked()
        {
            // 1: linked successfully. 0: linker errors
            int linkStatus = 1;
            GL.GetProgram(Id, GetProgramParameterName.LinkStatus, out linkStatus);
            return linkStatus != 0;
        }
    }
}
