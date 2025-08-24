#[compute]
#version 450

// Invocations in the (x, y, z) dimension
layout(local_size_x = 1024, local_size_y = 1, local_size_z = 1) in;

// A binding to the buffer we create in our script
layout(set = 0, binding = 0, std430) restrict buffer BoidsDataBuffer {
    // x = pos.x
    // y = pos.y
    // z = rotation
    // a = scale
    vec4 data[];
}
boids_data_buffer;

layout(set = 0, binding = 1, std430) restrict buffer MouseDataBuffer {
    float x;
    float y;
}
mouseData;

void main() {
    const ivec2 coord = ivec2(gl_GlobalInvocationID.xy);
}