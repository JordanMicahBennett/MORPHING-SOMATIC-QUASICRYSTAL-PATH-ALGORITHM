//Author  >> Jordan Micah Bennett  (  manufactured mind  ( c )  2014  ) 
//Author Notes 0 >> Comments indicate details regarding the scope of information gathered [See 'Author Notes 2'], thereafter being applied in 'MorphingSomaticQuasicrystalNeuralNetworkDiffractionPatternFieldGenerator'.
//Author Notes 1 >> This is based on a n dimensional petrie polygon / orthogonalized hypercube (range(tesseract TO dekeract))
//Author Notes 2 >> Credits --> wikipedia users : TomRuen, Jgmoxness, Claudio Rochhini for quasicrystalline petrie polygon measure polytope information.

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MorphingSomaticQuasicrystalNeuralNetworkDiffractionPatternFieldGenerator : MonoBehaviour 
{
	//establish unity3d game objects
	public List <GameObject> NODES;
	private LineRenderer edge;
	public GameObject FIELD;

	public float SPATIAL_DISPLACEMENT_VALUE;
	public int DIMENSION_CARDINALITY;
	public Vector3 FIELD_POSITION;
	private int VERTEX_CARDINALITY;
	public bool FIELD_VISIBILITY;
	public float FIELD_VISIBILITY_ALPHA;

	void Awake ( ) 
	{
		if ( SPATIAL_DISPLACEMENT_VALUE == null )
			SPATIAL_DISPLACEMENT_VALUE = 30; //default to dense grouping, if input is null

		if ( DIMENSION_CARDINALITY == null )
			DIMENSION_CARDINALITY = 4; //default to tesseract computation, if input is null

		if ( FIELD_POSITION == null )
			FIELD_POSITION = new Vector3 ( 247.6164f, 8.124113f, -772.6021f ); //default to dsr0 pos, if input is null

		generateField ( );
		enableVisibilityControl ( );
	}

	public void generateField ( )
	{
		VERTEX_CARDINALITY = ( 1 << DIMENSION_CARDINALITY ) ; //bitwise leftshift operator

		
		//compute edge cardinality from n choose 1 * 2 ^ (n-1)      OR     n!/1!*(n-1)! * 2 ^ (n-1)
		//formulae source >> http://www.mathematische-basteleien.de/hypercube.htm
		int NE = ( int ) ( factorial ( DIMENSION_CARDINALITY ) / ( factorial ( 1 ) * factorial ( DIMENSION_CARDINALITY - 1 ) ) * Math.Pow ( 2, DIMENSION_CARDINALITY - 1 ) );

		//establish electron diffusion pattern field
		FIELD = new GameObject ( );
		FIELD.name = "msqnn_field";

		//establish node objects based on vertex cardinality
		NODES = new List<GameObject> ( );

		for ( int node = 0; node < VERTEX_CARDINALITY; node++ ) 
		{
			NODES.Add ( GameObject.CreatePrimitive ( PrimitiveType.Sphere ) );
			NODES [ node ].name = "msqnn_field_mesh_node";
		}

		//establish edge objects based on edge cardinality
		edge = gameObject.AddComponent<LineRenderer> ( );


		//establish field
		for ( int node = 0; node < VERTEX_CARDINALITY; node++ ) 
			NODES [ node ].transform.parent = FIELD.transform;
		edge.transform.parent = FIELD.transform;


		//begin orthogonalized quasicrystal electron diffraction process 
		double [ , ] v = new double [ VERTEX_CARDINALITY, DIMENSION_CARDINALITY ] ; 
		int [ , ] e = new int [ NE, 2 ] ; 

		string [ ] horizontal_projection_vector_scalars = 
		{
			/*tesseract*/"0.270598050073,0.653281482438,0.653281482438,0.270598050073",
			/*penteract*/"0.195439507585,0.511667273602,0.632455532034,0.511667273602,0.195439507585",
			/*hexeract*/"0.149429245361,0.408248290464,0.557677535825,0.557677535825,0.408248290464,0.149429245361",
			/*hepteract*/"0.118942442321,0.333269317529,0.481588117120,0.534522483825,0.481588117120,0.333269317529,0.118942442321",
			/*octeract*/"0.097545161008,0.277785116510,0.415734806151,0.490392640202,0.490392640202,0.415734806151,0.277785116510,0.097545161008",
			/*enneract*/"0.081858535979,0.235702260396,0.361116813613,0.442975349592,0.471404520791,0.442975349592,0.361116813613,0.235702260396,0.081858535979",
			/*dekeract*/"0.069959619571,0.203030723711,0.316227766017,0.398470231296,0.441707654031,0.441707654031,0.398470231296,0.316227766017,0.203030723711,0.069959619571",
		};

		string [ ] vertical_projection_vector_scalars =
		{
			/*tesseract*/"-0.653281482438,-0.270598050073,0.270598050073,0.653281482438",
			/*penteract*/"-0.601500955008,-0.371748034460,0.000000000000,0.371748034460,0.601500955008",
			/*hexeract*/"-0.557677535825,-0.408248290464,-0.149429245361,0.149429245361,0.408248290464,0.557677535825",
			/*hepteract*/"-0.521120889170,-0.417906505941,-0.231920613924,0.000000000000,0.231920613924,0.417906505941,0.521120889170",
			/*octeract*/"-0.490392640202,-0.415734806151,-0.277785116510,-0.097545161008,0.097545161008,0.277785116510,0.415734806151,0.490392640202",
			/*enneract*/"-0.464242826880,-0.408248290464,-0.303012985115,-0.161229841765,0.000000000000,0.161229841765,0.303012985115,0.408248290464,0.464242826880",
			/*dekeract*/"-0.441707654031,-0.398470231296,-0.316227766017,-0.203030723711,-0.069959619571,0.069959619571,0.203030723711,0.316227766017,0.398470231296,0.441707654031",
		};
		

		//establish petrie polygon coxeter dynkin pre-baked depth vector direction scalars
		//we just distribute it z-wise appropriately per desired region.

		double [ ] HORIZONTAL_VECTORS = new double [ VERTEX_CARDINALITY ] ; 
		double [ ] VERTICAL_VECTORS = new double [ VERTEX_CARDINALITY ] ; 
		
		int i,j,k,l;
		
		
		
		for  ( i = 0; i < VERTEX_CARDINALITY; ++ i )  
			for  ( j = 0; j < DIMENSION_CARDINALITY; ++ j )  
			{
				long rightShiftedIJindex =  ( i >> j ) & 1; //newly introduced integer, for the facing/following right shift realignment measure. 
				v  [ i, j ] = rightShiftedIJindex == 1 ? -0.5 : 0.5; //Needed to realign, such that right shift operation worked with natural comparator, for easy translation into c#.  ( i>>j ) &1 ? .5 : .5; does not work in c#. C# sees  ( i>>j ) &1 as an integer, rather than boolean, as seen in c++.
				
			}
		l = 0;
		
		for ( i = 0; i < VERTEX_CARDINALITY - 1; ++ i )  
		{
			for ( j = i + 1; j < VERTEX_CARDINALITY; ++ j )  
			{
				double d = 0;
				for ( k = 0; k < DIMENSION_CARDINALITY; ++ k )  
					d +=  ( v [ i, k ] -v [ j, k ]  ) * ( v [ i, k ] -v [ j, k ]  ) ;
				d = Math.Sqrt ( d ) ;
				if ( d == 1 )  { e [ l, 0 ] =i; e [ l, 1 ] = j; ++l;}
			}
		}
		
		//Debug.Assert ( l==NE ) ; //Unity3d and C#'s Assert clash. However, this assert function is not really neccessary. It is a mere precausion per orthogonal quasicrystal configuration change.
		
		for ( i = 0; i < VERTEX_CARDINALITY;++i ) 
		{
			HORIZONTAL_VECTORS [ i ] = 0; for ( l = 0; l < DIMENSION_CARDINALITY; ++ l ) HORIZONTAL_VECTORS [ i ] += v [ i,l ] *( Double.Parse ( horizontal_projection_vector_scalars [ DIMENSION_CARDINALITY - 4 ].Split ( ',' ) [ l ] ) ) ;
			VERTICAL_VECTORS [ i ] = 0; for ( l = 0; l < DIMENSION_CARDINALITY; ++ l ) VERTICAL_VECTORS [ i ] += v [ i,l ] *( Double.Parse ( vertical_projection_vector_scalars [ DIMENSION_CARDINALITY - 4 ].Split ( ',' ) [ l ] ) ) ;
		}
		
		double SX = 800;  double SY = 800;
		double B  = 64;   double R  = 5;
		
		double sca  = Math.Min (  ( SX-2*B ) /2, ( SY-2*B ) /2 ) ;
		
		for ( i = 0; i < VERTEX_CARDINALITY;++i )  
		{
			HORIZONTAL_VECTORS [ i ] = B+ ( HORIZONTAL_VECTORS [ i ] +1 ) *sca; VERTICAL_VECTORS [ i ] = B+ ( VERTICAL_VECTORS [ i ] +1 ) *sca; 
		}
		
		
		//GENERATE ORTHOGONALIZED QUASICRYSTAL FIELD

		//Generate NODES
		for ( i = 0; i < VERTEX_CARDINALITY; ++i ) 
		{
			//ensure collider is big enough to be detectectable
				NODES [ i ].collider.isTrigger = true; //ensure that they are non collidable
				//NODES [ i ].GetComponent <SphereCollider> ( ).radius = 5f; 
				//NODES [ i ].collider.material.name = "msqnn material";
			//appropriate position
				NODES [ i ].transform.position = new Vector3 ( ( float ) HORIZONTAL_VECTORS [ i ] / SPATIAL_DISPLACEMENT_VALUE, ( float ) VERTICAL_VECTORS [ i ] / SPATIAL_DISPLACEMENT_VALUE, 0f  ) ;
		}


		//Generate edges
		edge.SetWidth ( .04f, 0.04f );
		edge.SetVertexCount ( NE );

		//This works, albeit it requires a bit of computation space
		/*
		for ( int edgeCount=0; edgeCount < NE; edgeCount++ )
		{
			Vector3 initialPosition = new Vector3 ( ( float ) HORIZONTAL_VECTORS [ e [ edgeCount, 0 ] ] / SPATIAL_DISPLACEMENT_VALUE, ( float ) VERTICAL_VECTORS [ e [ edgeCount, 0 ]  ] / SPATIAL_DISPLACEMENT_VALUE, 0f );
			Vector3 incidentPosition = new Vector3 ( ( float ) HORIZONTAL_VECTORS [ e [ edgeCount, 1 ] ] / SPATIAL_DISPLACEMENT_VALUE, ( float ) VERTICAL_VECTORS [ e [ edgeCount, 1 ] ] / SPATIAL_DISPLACEMENT_VALUE, 0f );
			edge.SetPosition ( edgeCount, initialPosition );
			edge.SetPosition ( edgeCount, incidentPosition );
		}*/


		//optimize field ( issue proper rotation, and proper scaling per target region )
		//in this case, m squared's target region occurs such that the field must be .4f, .4f, .4f large ( or small )
		FIELD.transform.position = FIELD_POSITION; 
		FIELD.transform.localEulerAngles = new Vector3 ( 90, 0, 0 );
		//FIELD.transform.localScale.Set ( .4f, .4f, .4f );
		//real research 0 area Vector3 ( 247.6164f, 8.124113f, -772.6021f );
	}

	public void enableVisibilityControl ( )
	{
		if ( !FIELD_VISIBILITY )
			new GameUtilities ( ).colourItems ( NODES, 0f, 0f, 0f, FIELD_VISIBILITY_ALPHA );	
		else
			new GameUtilities ( ).colourItems ( NODES, 0f, 1f, 0f, 1f );
	}

	//quintessential factorial recursive
	long factorial ( int n )
	{
		if ( n == 0 )
			return 1;
		else
			return ( n * factorial ( n - 1 ) );
	}
}

