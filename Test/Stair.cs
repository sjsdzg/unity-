using UnityEngine;
using System.Collections;

	
public class Stair : MonoBehaviour {
	public Material StairStyleMaterial;
	public float Xwidth = 5f;
	public float Zwidth = 0.5f;
	public float Yheigh = 0.5f;
	public int StairNumber = 10;
	private Vector3 position =Vector3.zero;

	private GameObject EveryCube;
	private Transform myTransform;
	void Awake(){
		myTransform = transform;
	}
	void Start () {
		position = myTransform.position;
		position +=new Vector3(0,Yheigh/2f,0);
		cubeInformation();
		CreatStair();
	}
	void cubeInformation(){
		EveryCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		EveryCube.transform.localScale =new Vector3(Xwidth,Yheigh,Zwidth);
		EveryCube.transform.rotation = myTransform.rotation;
		EveryCube.transform.position = position;
		EveryCube.transform.parent = myTransform;
		if(StairStyleMaterial !=null)
		EveryCube.transform.GetComponent<Renderer>().material = StairStyleMaterial;
	}
	void CreatStair(){
		for(int i = 0;i <StairNumber;i++){
//			new Vector3(position.x,position.y*2*(i+1) ,position.z+Zwidth*(i+1))
			GameObject cube = Instantiate(EveryCube,position+myTransform.forward*Zwidth*(i+1)+new Vector3(0,Yheigh*(i+1),0),transform.rotation) as GameObject;
			cube.transform.parent = transform;
		}
	}
}
