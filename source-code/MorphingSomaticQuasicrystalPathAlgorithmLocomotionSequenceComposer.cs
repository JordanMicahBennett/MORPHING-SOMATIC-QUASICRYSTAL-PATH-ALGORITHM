//Author >> Jordan Micah Bennett  (  manufactured mind  ( c )  2014  ) 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MorphingSomaticQuasicrystalPathAlgorithmLocomotionSequenceComposer : MonoBehaviour 
{
	private MorphingSomaticQuasicrystalPathAlgorithmDiffractionPatternFieldGenerator fieldGenerator; 
	private MorphingSomaticQuasicrystalPathAlgorithm model; 
	public List <Vector3> QUASICRYSTAL_POLYGON_FLAG; //discovered nodes {that all compose a single quasicrystal flag}
	public GameObject FRUSTUM; 

	public void Awake ( )
	{
		fieldGenerator = GameObject.FindGameObjectWithTag ( Tags.gameController ).GetComponent <MorphingSomaticQuasicrystalPathAlgorithmDiffractionPatternFieldGenerator> ( );
		model = GameObject.FindGameObjectWithTag ( Tags.gameController ).GetComponent <MorphingSomaticQuasicrystalPathAlgorithm> ( );

		QUASICRYSTAL_POLYGON_FLAG = new List <Vector3> ( );
	}

	//simply checks if attached frustum contains any points within the quasicrystal field
	public void Update ( )
	{
		if ( FRUSTUM != null )
			collisionCheck ( ); 
	}

	public void establishFrustum ( GameObject value )
	{
		this.FRUSTUM = value;
	}


	public void collisionCheck ( ) 
	{
		for ( int N = 0; N < fieldGenerator.NODES.Count; N ++ )
		{
			if ( FRUSTUM.collider.bounds.Contains ( fieldGenerator.NODES [ N ].collider.transform.position ) )
		    {
				QUASICRYSTAL_POLYGON_FLAG.Add ( fieldGenerator.NODES [ N ].transform.position );

				if ( fieldGenerator.FIELD_VISIBILITY )
					new GameUtilities ( ).colourItem ( fieldGenerator.NODES [ N ], 0f, 0f, 1f, 4f );
			}
	    }
	}
		
}
