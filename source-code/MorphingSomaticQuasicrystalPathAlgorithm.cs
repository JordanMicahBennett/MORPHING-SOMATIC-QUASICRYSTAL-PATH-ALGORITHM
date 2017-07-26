//Author Data >> Scratch written by Jordan Micah Bennett  (  manufactured mind  ( c )  2014  ) 


using UnityEngine;
using System;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;

public class MorphingSomaticQuasicrystalPathAlgorithm : MonoBehaviour 
{
	public GameObject AGENT;
	public GameObject AGENT_FRUSTUM;
	public List <GameObject> CLUSTER, FRUSTA;
	public GameObject CLUSTER_FIELD, FRUSTA_FIELD;
	public bool FRUSTA_VISIBILITY;
	public float FRUSTA_VISIBILITY_ALPHA;
	public List <Vector3> INITIAL_CLUSTER_VECTOR_GROUP, INITIAL_FRUSTA_VECTOR_GROUP;  //made public to be accessible amidst 'MorphingSomaticQuasicrystalPathAlgorithmGenerator'. No values need be passed to these midst run time.
	public int CLUSTER_CARDINALITY;

	//locomotion
	public List <Vector3> commencementVectors;
	public float commencementTime;

	public float TRAVERSAL_RATE;
	public IEnumerator waitEnumerator;
	public bool contractionQuery;
	public int AREA_TRAVERSAL_LIMIT_PADDING;

	void Awake ( )
	{
		CLUSTER = new List <GameObject> ( );
		CLUSTER_FIELD = new GameObject ( );

		CLUSTER_FIELD.name = "msqnn_cluster";

		FRUSTA = new List <GameObject> ( );
		FRUSTA_FIELD = new GameObject ( );

		FRUSTA_FIELD.name = "msqnn_frusta";

		INITIAL_CLUSTER_VECTOR_GROUP = new List <Vector3> ( );
		INITIAL_FRUSTA_VECTOR_GROUP = new List <Vector3> ( );



		//locomotion
		commencementVectors = new List <Vector3> ( );
		commencementTime = Time.time;
		TRAVERSAL_RATE = .009f;
		waitEnumerator = waitEnumeratorFunction ( 5 );
		contractionQuery = false;
		AREA_TRAVERSAL_LIMIT_PADDING = 38;
	}

	public void establishAgents ( Vector3 center, float clusterSpacing, float frustumSpacing )
	{
		//I appropriately situate my agents ( antagonists ) midst origin our morphing quasicrystal based field.
		//our loops limit ends @ somewhere midst the origin of our field, bounded in agent's cardinality
		INITIAL_CLUSTER_VECTOR_GROUP = new GameUtilities ( ).getCircularVectorCollection ( center, clusterSpacing, CLUSTER_CARDINALITY );
		INITIAL_FRUSTA_VECTOR_GROUP = new GameUtilities ( ).getCircularVectorCollection ( center, frustumSpacing, CLUSTER_CARDINALITY );


		//establish variables needed to align agents facing outward
		float outwardClusterRotation = 0f, outwardClusterRotationDisplacementFactor = 15f;

		///generate cluster based on INITIAL_CLUSTER_VECTOR_GROUP
		for ( int A = 0; A < CLUSTER_CARDINALITY; A ++ )
		{
			CLUSTER.Add ( ( GameObject ) Instantiate ( AGENT, INITIAL_CLUSTER_VECTOR_GROUP [ A ], Quaternion.identity ) );
			CLUSTER [ A ].transform.parent = CLUSTER_FIELD.transform;
			CLUSTER [ A ].transform.localEulerAngles = new Vector3 ( 0f, outwardClusterRotation, 0f );

			//'guessed' agent orientation outward computation
			outwardClusterRotation += outwardClusterRotationDisplacementFactor * CLUSTER_CARDINALITY;


			//locomotion
			commencementVectors.Add ( CLUSTER [ A ].transform.position );
		}


		//establish variables needed to align frustums facing outward
		float outwardFrustaRotation = 0f, outwardFrustaRotationDisplacementFactor = 15f;

		//generate FRUSTA based on INITIAL_FRUSTA_VECTOR_GROUP
		for ( int A = 0; A < CLUSTER_CARDINALITY; A ++ )
		{
			FRUSTA.Add ( ( GameObject ) Instantiate ( AGENT_FRUSTUM, INITIAL_FRUSTA_VECTOR_GROUP [ A ], Quaternion.identity ) );
			FRUSTA [ A ].transform.parent = FRUSTA_FIELD.transform;

			FRUSTA [ A ].transform.localEulerAngles = new Vector3 ( 0f, outwardFrustaRotation, 0f );

			//fixed position adjustment. The system is dynamic albeit, however, this fixes each frustum midst our agent posisition wise a certain manner, such that it spans
			//outwards upon the quaisicrystal field.
			//need to extend to make this even more dyanamic, by accepting 'spans' and manipulating here appropriately, wrt to such span input.
			FRUSTA [ A ].transform.Translate ( new Vector3 ( 0f, 0f, 60f ) );
			FRUSTA [ A ].transform.localScale = new Vector3 ( FRUSTA [ A ].transform.localScale.x, FRUSTA [ A ].transform.localScale.y, FRUSTA [ A ].transform.localScale.z * 15 );
			outwardFrustaRotation += outwardFrustaRotationDisplacementFactor * CLUSTER_CARDINALITY;

			//each frustum has it's own generated flag!
			//these are populated midst MorphingSomaticQuasicrystalPathAlgorithmLocomotionSequenceComposer
				//add MorphingSomaticQuasicrystalPathAlgorithmLocomotionSequenceComposer, so as to enable locomotion sequence construction.
				//the added script utilizes collison bounds routines to detect rather msq nn field nodes are within
				//the bounds of any agent frustum. If so, the aforsaid script automatically generates a List of <Vector3> elements, that
				//consists of the accumilation of vectors found.
				FRUSTA [ A ].AddComponent ( "MorphingSomaticQuasicrystalPathAlgorithmLocomotionSequenceComposer" );
				//then establish frustum object to offset collision check, to fulfill somatic traversal node set construction
				FRUSTA [ A ].GetComponent <MorphingSomaticQuasicrystalPathAlgorithmLocomotionSequenceComposer> ( ).establishFrustum ( FRUSTA [ A ] );
				//then alter the discovered node set par sequence composer, such that x and z components are maintained, but y components are toggled 
				//to default y set generated here {for valid y position = any cluster unit's y configuration}
				for ( int V = 0; V < FRUSTA [ A ].GetComponent <MorphingSomaticQuasicrystalPathAlgorithmLocomotionSequenceComposer> ( ).QUASICRYSTAL_POLYGON_FLAG.Count; V ++ )
				{
					Vector3 sequenceComposerVector = FRUSTA [ A ].GetComponent <MorphingSomaticQuasicrystalPathAlgorithmLocomotionSequenceComposer> ( ).QUASICRYSTAL_POLYGON_FLAG [ V ];
					FRUSTA [ A ].GetComponent <MorphingSomaticQuasicrystalPathAlgorithmLocomotionSequenceComposer> ( ).QUASICRYSTAL_POLYGON_FLAG [ V ].Set ( sequenceComposerVector.x, CLUSTER [ 0 ].transform.position.y, sequenceComposerVector.z );
				}
		}


		//setup field positions
		CLUSTER_FIELD.transform.position = center;
		FRUSTA_FIELD.transform.position = center;


		// enableFrustumVisibilityControl 
		enableVisibilityControl ( );
	}

	public void enableVisibilityControl ( )
	{
		if ( !FRUSTA_VISIBILITY )
			new GameUtilities ( ).colourItems ( FRUSTA, 0f, 0f, 0f, FRUSTA_VISIBILITY_ALPHA );	
		else
			new GameUtilities ( ).colourItems ( FRUSTA, 1f, 0f, 0f, 1f );
	}

	public IEnumerator waitEnumeratorFunction ( int secondCardinality )
	{
		yield return new WaitForSeconds ( secondCardinality );
	}

	public void Update ( )
	{
		//print ( ">>>" + contractionQuery );
		for ( int A = 0; A < CLUSTER_CARDINALITY; A ++ )
		{
			//expanding behaviour
			if ( CLUSTER [ A ].GetComponent <Animator> ( ).GetBool ( "Scanning" ) )
			{
				if ( !contractionQuery )
				{
					MorphingSomaticQuasicrystalPathAlgorithmLocomotionSequenceComposer expandingSequenceComposer = FRUSTA [ A ].GetComponent <MorphingSomaticQuasicrystalPathAlgorithmLocomotionSequenceComposer> ( );

					if ( expandingSequenceComposer != null )
					{
						List <Vector3> lerpVectors = expandingSequenceComposer.QUASICRYSTAL_POLYGON_FLAG;

						for ( int V = 0; V < expandingSequenceComposer.QUASICRYSTAL_POLYGON_FLAG.Count;  V ++ )
						{
							if ( waitEnumerator.MoveNext ( ) )
								CLUSTER [ A ].transform.position = Vector3.Lerp ( CLUSTER [ A ].transform.position, lerpVectors [ V ], TRAVERSAL_RATE );
						}
					}
				}
			}

			//expanse/contraction determinism
			if ( CLUSTER [ A ].GetComponent <Animator> ( ).GetBool ( "Scanning" ) )
			{
				MorphingSomaticQuasicrystalPathAlgorithmLocomotionSequenceComposer bluntSequenceComposer = FRUSTA [ A ].GetComponent <MorphingSomaticQuasicrystalPathAlgorithmLocomotionSequenceComposer> ( );

				print ( "contractionQuery >>> " + contractionQuery + " _CLUSTER >>>" + CLUSTER [ A ].transform.position.z  + "   _QUASICRYSTAL_POLYGON_FLAG >>>" + ( bluntSequenceComposer.QUASICRYSTAL_POLYGON_FLAG [ 0 ].z - AREA_TRAVERSAL_LIMIT_PADDING ) );
				if ( CLUSTER [ A ].transform.position.z >= ( bluntSequenceComposer.QUASICRYSTAL_POLYGON_FLAG [ 0 ].z - AREA_TRAVERSAL_LIMIT_PADDING ) )
					contractionQuery = true;
				else if ( CLUSTER [ A ].transform.position.z == bluntSequenceComposer.QUASICRYSTAL_POLYGON_FLAG [ bluntSequenceComposer.QUASICRYSTAL_POLYGON_FLAG.Count ].z )
					contractionQuery = false;
			}

			//contracting behaviour
			if ( CLUSTER [ A ].GetComponent <Animator> ( ).GetBool ( "Scanning" ) )
			{
				if ( contractionQuery )
				{
					MorphingSomaticQuasicrystalPathAlgorithmLocomotionSequenceComposer contractingSequenceComposer = FRUSTA [ A ].GetComponent <MorphingSomaticQuasicrystalPathAlgorithmLocomotionSequenceComposer> ( );
					
					if ( contractingSequenceComposer != null )
					{
						List <Vector3> lerpVectors = contractingSequenceComposer.QUASICRYSTAL_POLYGON_FLAG;
						
						for ( int V = contractingSequenceComposer.QUASICRYSTAL_POLYGON_FLAG.Count; V > 0;  V -- )
						{
							if ( waitEnumerator.MoveNext ( ) )
								CLUSTER [ A ].transform.position = Vector3.Lerp ( CLUSTER [ A ].transform.position, lerpVectors [ V ], TRAVERSAL_RATE );
						}
					}
				}
			}
		}
	}
}
