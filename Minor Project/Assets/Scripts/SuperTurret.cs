using UnityEngine;
using System.Collections;

public class SuperTurret : Turret {

	public float alpha;

	int inputSize;
	int hiddenSize;
	int outputSize;
	float[][] weightsInput = new float[hiddenSize][inputSize];
	float[][] weightsOutput = new float[outputSize][hiddenSize];
	float[][] inputs = new float[0][inputSize];
	float[] outputs = new float[outputSize];
	float[] desOutputs = new float[outputSize];

	public void BulletTrigger(){
	// Fire a bullet

		Player = GameObject.FindGameObjectWithTag( "Player" );

		if (Player != null) {
			Vector3 playerPos = Player.transform.position;
			Vector3 objectPos = gameObject.transform.position;
			if ( Vector3.Distance( playerPos, objectPos ) < LineofSight && gameObject.activeSelf)
			{
				Instantiate (bullet, transform.position, transform.rotation);
			}
		}
	}

	private float sigmoid ( float X ) {
		return 1f / (1f + Mathf.Exp (-X));
	}
		
	private float summation ( float[] input, float[] weights) {
		float res = 0;
		for (int index = 0; index < input.Length; index++) {
			res += input [index] * weights [index];
		}
		return res;
	}

	private float residuals ( float output, float desOutput) {
		return desOutput - output;
	}

	private float deltaWeightsOutput( float input, float output, float desOutput ) {
		float grad = output * (1f - output) * residuals(output, desOutput);
		return alpha * input * grad;
	}

	private float deltaWeightsHidden( float input, float output, float[] weightsOutput, float[] gradOutput ) {
		float grad;
		for (int index = 0; index < gradOutput.Length; index++) {
			grad += weightsOutput [index] * gradOutput [index];
		}
		grad *= output * (1f - output);
		return alpha * input * grad;
	}

	private void backPropagation() {
		for (int i = 0; i < inputs.GetLength (0); i++) {
			float[] hiddenOutput;
			float[] outputOutput;
			float[] curInput = inputs [i];
			for (int j = 0; j < hiddenSize; j++) {
				hiddenOutput[j] = sigmoid( summation( curInput, weightsInput[j] ) );
			}
			for (int k = 0; k < outputSize; k++) {
				outputOutput [k] = sigmoid (summation (hiddenOutput, weightsOutput[k]));
			}
			float[] gradOutput;
			for (int k = 0; k < outputSize; k++) {
				
			}
		}
	}

}