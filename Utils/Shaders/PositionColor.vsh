 
attribute vec4 position;
attribute vec4 color;

uniform mat4 transform;

varying vec4 fragColor;

void main()
{
    gl_Position = transform * position;

    fragColor = color;
}
