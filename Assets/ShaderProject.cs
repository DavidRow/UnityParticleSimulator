using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public struct Agent{ public Vector2 position; public float angle;public Vector4 color; }
public struct Person {public Vector2 position; public int health; }

public class ShaderProject : MonoBehaviour {

    //setup stuff
    public ComputeShader ParticleShader;
    public RenderTexture ParticleTexture;
    public int followON = 1;

    ComputeBuffer agentsBuffer;

    private Agent[] agents;
    private int KIdrawAgents = 1;
    private int KIdarken = 0;
    private int numAgents = 100000;
    private int width = 1000;   
    private int height = 1000;
    private int XthreadsAgents = 0;
    int Xthreadsdarken = 0;
    int Ythreadsdarken = 0;
    Vector4 color1 = new Vector4(1 , 0 , 0 , 0);
    Vector4 color2 = new Vector4(0 , 1 , 0 , 0);
    //Vector4 color3 = new Vector4(0 , 0 , 1 , 0);
    //Vector4 color4 = new Vector4(1 , 1 , 1 , 0);
    
    void Start(){

        //make the texture
        ParticleTexture = Helper.makeTexture(ParticleTexture, width, height);
        
        //make a list of agents
        //agents =  Helper.makeAgents(numAgents, width, height, color);
        Vector4[] colors = new Vector4[] {color1, color2};
        agents = Helper.makeAgentsInCircleAngleCenter(numAgents, width, height, 500, colors );

        //get the IDs of the shaders
        KIdrawAgents = ParticleShader.FindKernel("drawAgents");
        KIdarken = ParticleShader.FindKernel("darken");

        //set all the var to the shaders
        ParticleShader.SetInt("width", width);
        ParticleShader.SetInt("height", height);
        ParticleShader.SetInt("numAgents", numAgents);
        int stride = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Agent));
        agentsBuffer = new ComputeBuffer(numAgents,stride);
        agentsBuffer.SetData(agents);
        ParticleShader.SetBuffer(KIdrawAgents, "agents", agentsBuffer);
        ParticleShader.SetFloat("deltaTime", Time.fixedDeltaTime);
        ParticleShader.SetTexture(KIdrawAgents, "Result", ParticleTexture);
        ParticleShader.SetTexture(KIdarken , "Result", ParticleTexture);

        //set the ParticleTexture to the texture of the object we are on
        transform.GetComponentInChildren<Renderer>().material.mainTexture = ParticleTexture;

        //get # of threads to put into compute shaders
        XthreadsAgents =  Mathf.CeilToInt(numAgents/512f);
        Xthreadsdarken = Mathf.CeilToInt(width/32f);
        Ythreadsdarken =Mathf.CeilToInt(height/32f);
    }

    // Update is called once per frame
    void Update()   
    {
        // set var to shader
        ParticleShader.SetFloat("time", Time.fixedTime);
        ParticleShader.SetInt("followON", followON);

        //run the shader
        ParticleShader.Dispatch(KIdarken, Xthreadsdarken,Ythreadsdarken,1 );
        ParticleShader.Dispatch(KIdrawAgents, XthreadsAgents,1,1 );
    }
    void OnDestroy()
	{

		agentsBuffer.Release();
	}

    
}
