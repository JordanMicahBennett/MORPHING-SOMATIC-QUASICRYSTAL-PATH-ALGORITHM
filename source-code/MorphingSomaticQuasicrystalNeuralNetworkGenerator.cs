//Author >> Jordan Micah Bennett  (  manufactured mind  ( c )  2014  ) 


using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public class MorphingSomaticQuasicrystalNeuralNetworkGenerator : MonoBehaviour 
{
	private MorphingSomaticQuasicrystalNeuralNetworkDiffractionPatternFieldGenerator fieldGenerator; 
	private MorphingSomaticQuasicrystalNeuralNetwork nueralNetwork;


	void Awake  (   ) 
	{
		fieldGenerator = GameObject.FindGameObjectWithTag ( Tags.gameController ).GetComponent <MorphingSomaticQuasicrystalNeuralNetworkDiffractionPatternFieldGenerator> ( );
		nueralNetwork = GameObject.FindGameObjectWithTag ( Tags.gameController ).GetComponent <MorphingSomaticQuasicrystalNeuralNetwork> ( );

		//establish agents, by providing center, and spacing
		nueralNetwork.establishAgents ( new Vector3 ( 617f, 0f, 217f ), 10f, 15f );
	}
}