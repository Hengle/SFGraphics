#version 330

uniform float float1;
uniform float floatArray1[3];

uniform uint uint1;
uniform uint uintArray1[3];
uniform int int1;
uniform int boolInt1;
uniform int intArray1[3];

uniform vec2 vector2a;
uniform vec2[8] vector2Arr;

uniform vec3 vector3a;
uniform vec3[8] vector3Arr;

uniform vec4 vector4a;
uniform vec4[8] vector4Arr;

uniform mat4 matrix4a;
uniform mat4[8] matrix4Arr;

uniform sampler2D tex2D;
uniform samplerCube texCube;

uniform UniformBlock
{
	float values[10];
} uniformBlock;

out vec4 fragColor;

void main()
{
    // Use all the uniforms, so they aren't optimized out.
    fragColor = vec4(1) * float1 * int1;
	if (boolInt1 == 1)
		fragColor *= 0.5;
    fragColor.rg *= vector2a;
	fragColor.rgb *= vector3a;
	fragColor *= vector4a * matrix4a;
	fragColor *= texture(tex2D, vec2(1));
	fragColor *= texture(texCube, vec3(1));

	// Use a for loop to access multiple elements.
	// Even array[0] can cause issues.
	for (int i = 0; i < 3; i++)
	{
		fragColor *= floatArray1[i];
		fragColor *= intArray1[i];
		fragColor *= uintArray1[i];

		fragColor.rgb *= uniformBlock.values[i];
		fragColor *= matrix4Arr[i];
		fragColor.rg *= vector2Arr[i];
		fragColor *= vector4Arr[i];
		fragColor.rgb *= vector3Arr[i];
	}

    fragColor.rgb *= uint1;
}
