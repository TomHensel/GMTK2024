shader_type canvas_item;
uniform sampler2D screen_texture:hint_screen_texture;


void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	vec3 screenColor = 1.0- texture(screen_texture,SCREEN_UV).xyz;
	COLOR = vec4(screenColor,1.0);
	// Called for every pixel the material is visible on.
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
