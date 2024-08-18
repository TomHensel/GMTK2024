shader_type spatial;
render_mode blend_mix, cull_front;

uniform vec3 AtmosphereColor : source_color;
uniform float emmisionStrength = 1.0;
uniform float fesnelExponent = 5.0;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	
	float NDotV = clamp(pow(dot(NORMAL, VIEW),fesnelExponent),0.0,1.0);
	
	ALBEDO = AtmosphereColor;
	EMISSION = AtmosphereColor * emmisionStrength;
	ALPHA = NDotV;
	//ALBEDO = vec3(1.0);
	// Called for every pixel the material is visible on.
}

//void light() {
	// Called for every pixel for every light affecting the material.
	// Uncomment to replace the default light processing function with this one.
//}