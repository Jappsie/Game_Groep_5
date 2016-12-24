using UnityEngine;
using System.Collections;

class inputOutputpair
{
    float[] input;
    float output;

    public inputOutputpair( float[] input, float output )
    {
        this.input = input;
        this.output = output;
    }

    public float[] getInput()
    {
        return this.input;
    }

    public float getOutput()
    {
        return this.output;
    }
}
