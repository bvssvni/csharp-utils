 
attribute vec4 position;

uniform mat4 transform;
uniform lowp vec4 color;

void main()
{
    gl_Position = transform * position;
}
