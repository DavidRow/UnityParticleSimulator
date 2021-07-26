using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public struct Agent{ public Vector2 position; public float angle; }
public class ShaderProject : MonoBehaviour {

    //setup stuff
    public ComputeShader ParticleShader;
    public RenderTexture ParticleTexture;
    public RenderTexture Another;
    ComputeBuffer agentsBuffer;

    private Agent[] agents;
    private int KIdrawAgents = 0;
    private int KIdarken = 0;
    private int numAgents = 5000;

    private int width = 250;
    private int height = 250;

    void Start(){

        //make the texture
        ParticleTexture = Helper.makeTexture(ParticleTexture, width, height);
        
        //make a list of agents
        agents =  Helper.makeAgents(numAgents, width, height);
 
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

    }

    // Update is called once per frame
    void Update()   
    {
        // set var to shader
        ParticleShader.SetFloat("time", Time.fixedTime);
        //run the shader
        ParticleShader.Dispatch(KIdarken, width,height,1 );
        ParticleShader.Dispatch(KIdrawAgents, numAgents,1,1 );
    }
    void OnDestroy()
	{

		agentsBuffer.Release();
	}

    
}
