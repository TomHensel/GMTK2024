shader_type canvas_item;

uniform sampler2D mainViewPort;
uniform sampler2D blackHoleViewPort;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	//vec3 test = vec3(texture(mainViewPort, UV).r);
	//COLOR = vec4(test, 1.0);
	COLOR =texture(mainViewPort, UV) + texture(blackHoleViewPort, UV);
	// Called for every pixel the material is visible on.
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
