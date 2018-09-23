﻿using OpenTK;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders.Utils;
using SFGraphics.GLObjects.Shaders.ShaderEventArgs;
using SFGraphics.GLObjects.Textures;

namespace SFGraphics.GLObjects.Shaders
{
    public sealed partial class Shader
    {
        /// <summary>
        /// Sets a float uniform variable. Logs invalid names.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetFloat(string uniformName, float value)
        {
            if (!IsValidUniform(uniformName, ActiveUniformType.Float))
            {
                LogInvalidUniformSetRaiseEvent(uniformName, value, ActiveUniformType.Float);
                return;
            }

            GL.Uniform1(activeUniformByName[uniformName].location, value);
        }

        /// <summary>
        /// Sets all values for a float[] uniform variable. Logs invalid names.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetFloat(string uniformName, float[] value)
        {
            if (!IsValidUniform(uniformName, ActiveUniformType.Float, value.Length))
            {
                LogInvalidUniformSetRaiseEvent(uniformName, value, ActiveUniformType.Float, value.Length);
                return;
            }

            GL.Uniform1(activeUniformByName[uniformName].location, value.Length, value);
        }

        /// <summary>
        /// Sets an int uniform variable. Logs invalid names.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetInt(string uniformName, int value)
        {
            if (!IsValidUniform(uniformName, ActiveUniformType.Int))
            {
                LogInvalidUniformSetRaiseEvent(uniformName, value, ActiveUniformType.Int);
                return;
            }

            GL.Uniform1(activeUniformByName[uniformName].location, value);
        }

        /// <summary>
        /// Sets all values for an int[] uniform variable. Logs invalid names.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetInt(string uniformName, int[] value)
        {
            if (!IsValidUniform(uniformName, ActiveUniformType.Int, value.Length))
            {
                LogInvalidUniformSetRaiseEvent(uniformName, value, ActiveUniformType.Int, value.Length);
                return;
            }

            GL.Uniform1(activeUniformByName[uniformName].location, value.Length, value);
        }

        /// <summary>
        /// Sets a uint uniform variable. Logs invalid names.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetUint(string uniformName, uint value)
        {
            if (!IsValidUniform(uniformName, ActiveUniformType.UnsignedInt))
            {
                LogInvalidUniformSetRaiseEvent(uniformName, value, ActiveUniformType.UnsignedInt);
                return;
            }

            GL.Uniform1(activeUniformByName[uniformName].location, value);
        }

        /// <summary>
        /// Sets all values for a uint[] uniform variable. Logs invalid names.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetUint(string uniformName, uint[] value)
        {
            if (!IsValidUniform(uniformName, ActiveUniformType.UnsignedInt, value.Length))
            {
                LogInvalidUniformSetRaiseEvent(uniformName, value, ActiveUniformType.UnsignedInt, value.Length);
                return;
            }

            GL.Uniform1(activeUniformByName[uniformName].location, value.Length, value);
        }

        /// <summary>
        /// Converts <paramref name="value"/> to an int and sets an int uniform. 
        /// <c>true</c> = 1. <c>false</c> = 0. uniform variable. Logs invalid names.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetBoolToInt(string uniformName, bool value)
        {
            if (!IsValidUniform(uniformName, ActiveUniformType.Int))
            {
                int intValue = value ? 1 : 0;
                LogInvalidUniformSetRaiseEvent(uniformName, value, ActiveUniformType.Int);
                return;
            }

            // if/else is faster than the ternary operator. 
            if (value)
                GL.Uniform1(activeUniformByName[uniformName].location, 1);
            else
                GL.Uniform1(activeUniformByName[uniformName].location, 0);
        }

        /// <summary>
        /// Sets vec2 uniform variable. Logs invalid names.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetVector2(string uniformName, Vector2 value)
        {
            if (!IsValidUniform(uniformName, ActiveUniformType.FloatVec2))
            {
                LogInvalidUniformSetRaiseEvent(uniformName, value, ActiveUniformType.FloatVec2);
                return;
            }

            GL.Uniform2(activeUniformByName[uniformName].location, value);
        }

        /// <summary>
        /// Sets all values for a vec2[] uniform variable. Logs invalid names.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetVector2(string uniformName, Vector2[] value)
        {
            if (!IsValidUniform(uniformName, ActiveUniformType.FloatVec2, value.Length))
            {
                LogInvalidUniformSetRaiseEvent(uniformName, value, ActiveUniformType.FloatVec2, value.Length);
                return;
            }

            string name = GetNameNoArrayBrackets(uniformName);
            for (int i = 0; i < value.Length; i++)
            {
                GL.Uniform2(GetUniformLocation($"{name}[{i}]"), value[i]); 
            }
        }

        /// <summary>
        /// Sets vec2 uniform variable. Logs invalid names.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="x">The value for uniformName.x</param>
        /// <param name="y">The value for uniformName.y</param>
        public void SetVector2(string uniformName, float x, float y)
        {
            SetVector2(uniformName, new Vector2(x, y));
        }

        /// <summary>
        /// Sets vec3 uniform variable. Logs invalid names.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetVector3(string uniformName, Vector3 value)
        {
            if (!IsValidUniform(uniformName, ActiveUniformType.FloatVec3))
            {
                LogInvalidUniformSetRaiseEvent(uniformName, value, ActiveUniformType.FloatVec3);
                return;
            }

            GL.Uniform3(activeUniformByName[uniformName].location, value);
        }

        /// <summary>
        /// Sets all values for a vec3 uniform variable. Logs invalid names.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetVector3(string uniformName, Vector3[] value)
        {
            if (!IsValidUniform(uniformName, ActiveUniformType.FloatVec3, value.Length))
            {
                LogInvalidUniformSetRaiseEvent(uniformName, value, ActiveUniformType.FloatVec3, value.Length);
                return;
            }

            string name = GetNameNoArrayBrackets(uniformName);
            for (int i = 0; i < value.Length; i++)
            {
                GL.Uniform3(GetUniformLocation($"{name}[{i}]"), value[i]);
            }
        }

        /// <summary>
        /// Sets vec3 uniform variable. Logs invalid names.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="x">The value for uniformName.x</param>
        /// <param name="y">The value for uniformName.y</param>
        /// <param name="z">The value for uniformName.z</param>
        public void SetVector3(string uniformName, float x, float y, float z)
        {
            SetVector3(uniformName, new Vector3(x, y, z));
        }

        /// <summary>
        /// Sets vec4 uniform variable. Logs invalid names.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetVector4(string uniformName, Vector4 value)
        {
            if (!IsValidUniform(uniformName, ActiveUniformType.FloatVec4))
            {
                LogInvalidUniformSetRaiseEvent(uniformName, value, ActiveUniformType.FloatVec4);
                return;
            }

            GL.Uniform4(activeUniformByName[uniformName].location, value);
        }

        /// <summary>
        /// Sets vec4 uniform variable. Logs invalid names.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="x">The value for uniformName.x</param>
        /// <param name="y">The value for uniformName.y</param>
        /// <param name="z">The value for uniformName.z</param>
        /// <param name="w">The value for uniformName.w</param>
        public void SetVector4(string uniformName, float x, float y, float z, float w)
        {
            SetVector4(uniformName, new Vector4(x, y, z, w));
        }

        /// <summary>
        /// Sets all values for a vec4 uniform variable. Logs invalid names.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetVector4(string uniformName, Vector4[] value)
        {
            if (!IsValidUniform(uniformName, ActiveUniformType.FloatVec4, value.Length))
            {
                LogInvalidUniformSetRaiseEvent(uniformName, value, ActiveUniformType.FloatVec4, value.Length);
                return;
            }

            string name = GetNameNoArrayBrackets(uniformName);
            for (int i = 0; i < value.Length; i++)
            {
                GL.Uniform4(GetUniformLocation($"{name}[{i}]"), value[i]);
            }
        }

        /// <summary>
        /// Sets a mat4 uniform variable. Logs invalid names.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetMatrix4x4(string uniformName, ref Matrix4 value)
        {
            if (!IsValidUniform(uniformName, ActiveUniformType.FloatMat4))
            {
                LogInvalidUniformSetRaiseEvent(uniformName, value, ActiveUniformType.FloatMat4);
                return;
            }

            GL.UniformMatrix4(activeUniformByName[uniformName].location, false, ref value);
        }

        /// <summary>
        /// Sets all values for a mat4[] uniform variable. Logs invalid names.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="value">The value to assign to the uniform</param>
        public void SetMatrix4x4(string uniformName, Matrix4[] value)
        {
            if (!IsValidUniform(uniformName, ActiveUniformType.FloatMat4, value.Length))
            {
                LogInvalidUniformSetRaiseEvent(uniformName, value, ActiveUniformType.FloatMat4);
                return;
            }

            string name = GetNameNoArrayBrackets(uniformName);
            for (int i = 0; i < value.Length; i++)
            {
                GL.UniformMatrix4(GetUniformLocation($"{name}[{i}]"), false, ref value[i]);
            }
        }

        /// <summary>
        /// <paramref name="texture"/> is bound to <paramref name="textureUnit"/> before 
        /// setting the uniform. 
        /// Logs invalid names.
        /// </summary>
        /// <param name="uniformName">The uniform variable name</param>
        /// <param name="texture">The integer ID generated by GL.GenTexture()</param>
        /// <param name="textureUnit">The texture unit to which <paramref name="texture"/> will be bound</param>
        public void SetTexture(string uniformName, Texture texture, int textureUnit)
        {
            if (CheckIfTextureSetIsValid(uniformName, texture, textureUnit))
                BindTextureSetUniform(uniformName, texture, textureUnit);
        }

        private void BindTextureSetUniform(string uniformName, Texture texture, int textureUnit)
        {
            GL.ActiveTexture(TextureUnit.Texture0 + textureUnit);
            texture.Bind();
            GL.Uniform1(activeUniformByName[uniformName].location, textureUnit);
        }

        private bool CheckIfTextureSetIsValid(string uniformName, Texture texture, int textureUnit)
        {
            ActiveUniformType uniformType = ShaderTypeConversions.GetUniformType(texture.TextureTarget);
            bool validUniform = IsValidUniform(uniformName, uniformType);
            if (!validUniform)
                LogInvalidUniformSetRaiseEvent(uniformName, texture, uniformType);

            bool validSamplerType = IsValidSamplerType(textureUnit, uniformType);
            if (!validSamplerType)
            {
                OnTextureUnitTypeMismatch?.Invoke(this,
                    new UniformSetEventArgs(uniformName, uniformType, texture, 1));
            }

            bool validSet = validUniform && validSamplerType;
            return validSet;
        }

        private bool IsValidSamplerType(int textureUnit, ActiveUniformType samplerType)
        {
            // Binding two different texture types to the same texture unit
            // will generate an error.
            if (!samplerTypeByTextureUnit.ContainsKey(textureUnit))
            {
                samplerTypeByTextureUnit.Add(textureUnit, samplerType);
                return true;
            }
            else
            {
                return samplerTypeByTextureUnit[textureUnit] == samplerType;
            }
        }

        private void LogInvalidUniformSetRaiseEvent(string uniformName, object value, ActiveUniformType type, int length = 1)
        {
            // TODO: This does multiple things and isn't very clear.
            UniformSetEventArgs e = new UniformSetEventArgs(uniformName, type, value, length);
            LogInvalidUniformSet(e);
            OnInvalidUniformSet?.Invoke(this, e);
        }

        private bool IsValidUniform(string name, ActiveUniformType type, int size = 1)
        {
            bool validName = activeUniformByName.ContainsKey(name);
            bool validType = validName && activeUniformByName[name].type == type;
            bool validSize = validName && activeUniformByName[name].size == size;
            
            return validName && validType && validSize;
        }

        // TODO: This code can be shared with above.
        private bool IsValidAttribute(string name, ActiveAttribType type, int size)
        {
            bool validName = activeAttribByName.ContainsKey(name);
            bool validType = validName && activeAttribByName[name].type == type;
            bool validSize = validName && activeAttribByName[name].size == size;

            return validName && validType && validSize;
        }
    }
}
