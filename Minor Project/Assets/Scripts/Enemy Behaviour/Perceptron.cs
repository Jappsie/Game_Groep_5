using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Perceptron : Turret
{
    public static int inputSize = 2;
    static List<inputOutputpair> input = new List<inputOutputpair>();
    float[] weights = new float[ inputSize ];
    float[] outputs;
    float[] delta;
    float alpha = 0.25f;
    float error = 0.1f;

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
                float[] curInput = input[i].getInput();
                float desOutput = input[ i ].getOutput();
                //Debug.Log( "Input: " + curInput[0] + " " + curInput[1] );
				outputs[ i ] = calculate( curInput, weights, 0.2f );
                    
                //Debug.Log( "output: " + outputs[ i ] );
                //Debug.Log( "desOut: " + desOutputs[ i ] );
                delta[ i ] = desOutput - outputs[ i ];
                //Debug.Log( "delta: " + delta[ i ] );
                float[] deltaWeights = multiply( curInput, alpha * delta[ i ] );
                weights = add( weights, deltaWeights );
                //Debug.Log( weights[ 0 ] + " " + weights[ 1 ] );
            }
            MeanSquaredError = MSE( delta );
            counter++;
        } while ( counter < maxIteration && MeanSquaredError > error );
        Debug.Log( MeanSquaredError );
    }

    protected override void BulletTrigger()
    {
        Player = GameObject.FindGameObjectWithTag( "Player" );
        if (Player != null)
        {
            Vector3 playerPos = Player.transform.position;
            Vector3 objectPos = gameObject.transform.position;
            RaycastHit hit;
            Ray playerRay = new Ray( objectPos, playerPos - objectPos + new Vector3(0,0.5f,0)); // Vector as compensation for mass-position

            // TODO: Change order of life of sight and raycasting for performance
            if ( Physics.Raycast(playerRay , out hit ) )
            {
				//Debug.Log (hit.collider.gameObject.name);
                if (hit.collider.gameObject.Equals(Player))
                {
                    if ( Vector3.Distance( playerPos, objectPos ) < LineofSight && gameObject.activeSelf )
                    {
                        Vector3 distance = Player.transform.position - gameObject.transform.position;
                        Vector3 plane = Vector3.Cross( gameObject.transform.rotation * Vector3.forward , Vector3.up );
                        float dodge = Vector3.Dot( plane, distance );
                        GameObject[] turrets = GameObject.FindGameObjectsWithTag( "Turret" );
                        float turretLeftRight = 0;
                        foreach ( GameObject turret in turrets )
                        {
                            Vector3 turretDistance = turret.transform.position - gameObject.transform.position;
                            if ( turretDistance.magnitude < 2 * LineofSight )
                            {
                                turretLeftRight += Mathf.Sign( Vector3.Dot( plane, turretDistance ) );
                            }
                        }
                        float offset = calculate( new float[] { distance.magnitude, turretLeftRight }, weights, 0.2f );

                        Instantiate( bullet, transform.position, transform.rotation * Quaternion.Euler( 0, offset * -20f, 0 ) );
                        IEnumerator coroutine = trackPlayer(playerRay.direction);
                        StartCoroutine( coroutine );
                    }
                }
            }
        }
    }

    

    private IEnumerator trackPlayer( Vector3 direction)
    {
        yield return new WaitForSecondsRealtime( 1 );

        // Make method
        Vector3 distance = Player.transform.position - gameObject.transform.position;
        Vector3 plane = Vector3.Cross( direction, Vector3.up);
        float dodge = Vector3.Dot(plane.normalized,distance.normalized);
        GameObject[] turrets = GameObject.FindGameObjectsWithTag( "Turret" );
        float turretLeftRight = 0;
        foreach (GameObject turret in turrets)
        {
            Vector3 turretDistance = turret.transform.position - gameObject.transform.position;
            if (turretDistance.magnitude < 2*LineofSight)
            {
                turretLeftRight += Mathf.Sign( Vector3.Dot( plane, turretDistance ) );
            }
        }
        // Till here
        input.Add( new inputOutputpair( new float[] { distance.magnitude, turretLeftRight }, Mathf.RoundToInt(dodge) ));
        Debug.Log( Mathf.RoundToInt(dodge) );
        learn( 20 );
    }

}

class inputOutputpair {
    float[] input;
    float output;

    public inputOutputpair(float[] input, float output)
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
