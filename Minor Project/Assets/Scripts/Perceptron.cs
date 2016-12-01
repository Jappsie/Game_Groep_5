using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Perceptron : MonoBehaviour
{

    static int inputSize = 2;
    List<float[]> inputs = new List<float[]>();
    float[] weights = new float[ inputSize ];
    float[] outputs;
    float[] desOutputs;
    float alpha = 0.3f;
    float error = 0.1f;

    void Start()
    {
        Debug.Log( Time.time );
        Debug.Log( calculate(new float[] { 1, 1 }, new float[] { 1, 1 }, 0.2f) );
        Debug.Log( calculate( new float[] { 1, 1 }, new float[] { 0.5f, 0 }, 0.2f ) );
        for ( int i = 0; i < inputSize; i++ )
        {
            weights[ i ] = Random.value;
        }
        inputs.Add( new float[] { 0, 0 } );
        inputs.Add( new float[] { 0, 1 } );
        inputs.Add( new float[] { 1, 0 } );
        inputs.Add( new float[] { 1, 1 } );
        desOutputs = new float[] { 1, 0, 0, 1 };
        learn( 25 );
        Debug.Log( Time.time );
    }

    float dotProduct( float[] x, float[] y )
    {
        float res = 0;
        for ( int i = 0; i < x.Length; i++ )
        {
            res += x[ i ] * y[ i ];
        }
        return res;
    }

    float sigmoid( float x )
    {
        return x > 0.5f ? 1 : 0;
        //return 2f / (1f + Mathf.Exp( -x )) - 1;
    }


    float calculate( float[] input, float[] weights, float threshold )
    {
        return sigmoid( dotProduct( input, weights ) - threshold );
    }

    float[] multiply( float[] input, float number )
    {
        float[] temp = new float[input.Length];
        for ( int i = 0; i < temp.Length; i++ )
        {
            temp[ i ] = input[i] * number;
        }
        return temp;
    }

    float[] add( float[] input1, float[] input2 )
    {
        float[] temp = new float[ input1.Length ];
        for ( int i = 0; i < temp.Length; i++ )
        {
            temp[ i ] = input1[i] + input2[ i ];
        }
        return temp;
    }

    float MSE( float[] delta )
    {
        float res = 0;
        foreach ( float d in delta )
        {
            res += Mathf.Pow( d, 2 );
        }
        return res / delta.Length;
    }

    void learn( int maxIteration )
    {
        int counter = 0;
        float MeanSquaredError;
        outputs = new float[ inputs.Count ];
        do
        {
            float[] delta = new float[ inputs.Count ];
            for ( int i = 0; i < inputs.Count; i++ )
            {
                float[] curInput = inputs[ i ];
                Debug.Log( "Input: " + curInput[0] + " " + curInput[1] );
                outputs[i]= (calculate( curInput, weights, 0.2f ));
                Debug.Log( "output: " + outputs[ i ] );
                Debug.Log( "desOut: " + desOutputs[ i ] );
                delta[ i ] = desOutputs[ i ] - outputs[ i ];
                Debug.Log( "delta: " + delta[ i ] );
                float[] deltaWeights = multiply( curInput, alpha * delta[ i ] );
                weights = add( weights, deltaWeights );
                Debug.Log( weights[ 0 ] + " " + weights[ 1 ] );
            }
            MeanSquaredError = MSE( delta );
            counter++;
        } while ( counter < maxIteration && MeanSquaredError > error );
        Debug.Log( MeanSquaredError );
    }

}
