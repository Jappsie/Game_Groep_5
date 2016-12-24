using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NeuralNet : StatueTurret
{

    public static int inputSize = 2;
    public float effectRatio = 7;
    private static List<inputOutputpair> input = new List<inputOutputpair>();
    private float[] weights = new float[ inputSize ];
    private float[] outputs;
    private float[] delta;
    private float alpha = 0.05f;
    private float error = 0.1f;
    private float threshold = 0;

    protected override void Start()
    {
        base.Start();
        for ( int i = 0; i < inputSize; i++ )
        {
            weights[ i ] = 0;
        }
    }

    // Linear algebra vector-vector addition
    float[] add( float[] input1, float[] input2 )
    {
        float[] temp = new float[ input1.Length ];
        for ( int i = 0; i < temp.Length; i++ )
        {
            temp[ i ] = input1[ i ] + input2[ i ];
        }
        return temp;
    }

    // Linear algebra scalar-vector product
    float[] multiply( float[] input, float number )
    {
        float[] temp = new float[ input.Length ];
        for ( int i = 0; i < temp.Length; i++ )
        {
            temp[ i ] = input[ i ] * number;
        }
        return temp;
    }

    // Linear algebra dot-product
    float dotProduct( float[] x, float[] y )
    {
        float res = 0;
        for ( int i = 0; i < x.Length; i++ )
        {
            res += x[ i ] * y[ i ];
        }
        return res;
    }

    // Step function [-1,1]
    float step( float x )
    {
        return x > 0.5f ? 1 : -1;
    }

    // Calculate output network
    float calculate( float[] input, float[] weights, float threshold )
    {
        return step( dotProduct( input, weights ) - threshold );
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

    // Update weights to match input data
    void learn( int maxIteration )
    {
        int counter = 0;
        float MeanSquaredError;
        outputs = new float[ input.Count ];
        delta = new float[ input.Count ];
        do
        {
            for ( int i = 0; i < input.Count; i++ )
            {
                float[] curInput = input[ i ].getInput();
                float desOutput = input[ i ].getOutput();
                outputs[ i ] = calculate( curInput, weights, threshold );
                
                delta[ i ] = desOutput - outputs[ i ];
                float[] deltaWeights = multiply( curInput, alpha * delta[ i ] );
                string res = "";
                foreach (float value in deltaWeights)
                {
                    res += value + " ";
                }
                weights = add( weights, deltaWeights );
            }
            MeanSquaredError = MSE( delta );
            counter++;
        } while ( counter < maxIteration && MeanSquaredError > error );
    }

    private IEnumerator trackPlayer( Vector3 direction )
    {
        yield return new WaitForSecondsRealtime( 1 );
        // Find dodge direction
        Vector3 distance = player.transform.position - gameObject.transform.position;
        Vector3 plane = Vector3.Cross( direction, Vector3.up );
        float dodge = Vector3.Dot( plane.normalized, distance.normalized );

        // Add input to storage
        input.Add( new inputOutputpair( getData( direction ), Mathf.Sign( dodge ) ) );
        learn( 20 );
    }

    private float[] getData( Vector3 direction )
    {
        Vector3 distance = player.transform.position - gameObject.transform.position;
        Vector3 plane = Vector3.Cross( direction, Vector3.up );
        // Find surrounding turrets
        GameObject[] turrets = GameObject.FindGameObjectsWithTag( "Turret" );
        float turretLeftRight = 0;
        foreach ( GameObject turret in turrets )
        {
            Vector3 turretDistance = turret.transform.position - gameObject.transform.position;
            if ( turretDistance.magnitude < 2 * LineOfSight )
            {
                turretLeftRight += Mathf.Sign( Vector3.Dot( plane, turretDistance ) );
            }
        }
        return new float[] { distance.magnitude, turretLeftRight };
    }

    protected override IEnumerator fire( Vector3 PlayerPos )
    {
        
        yield return new WaitForSecondsRealtime( activationTime );
        Quaternion angleY = Quaternion.LookRotation( PlayerPos - bulletPos );
        Vector3 direction = angleY * Vector3.forward;
        float[] data = getData( direction );
        float dodgeDirection = calculate( data , weights, threshold );
        float playerSpeed = Vector3.Distance( PlayerPos, player.transform.position + new Vector3( 0, 1f, 0 ) ) / Time.deltaTime;
        Instantiate( bullet, bulletPos, Quaternion.Euler( angleY.eulerAngles.x, objectRot.eulerAngles.y - effectRatio * dodgeDirection*playerSpeed/data[0], 0 ) );
        yield return new WaitForSecondsRealtime( cooldownTime );
        StartCoroutine( trackPlayer( direction ) );
        canFire = true;
    }
}
