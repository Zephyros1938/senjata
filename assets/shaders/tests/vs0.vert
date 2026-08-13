#version 460 core
layout(location = 0) in vec3 aPos;
layout(location = 1) in vec3 aColor;

layout (std140, binding=0) uniform GlobalMatrices
{
  mat4 view;
  mat4 projection;
};

out vec3 pColor;

void main()
{
  gl_Position = projection * view * vec4(aPos.x, aPos.y, aPos.z, 1.0);
  pColor = aColor;
}
