
using UnityEngine;
public static class  Helper {
    //make a texture you can write to 
    public static RenderTexture makeTexture(RenderTexture Texture , int width,int  height ){
        Texture = new RenderTexture(width,height,0 , RenderTextureFormat.ARGBFloat);
        Texture.enableRandomWrite = true;
        Texture.Create();
        Texture.filterMode = FilterMode.Point;
        return Texture;
    }
    //make a list of agents 
    public static Agent[] makeAgents(int numAgents, int width,int  height ){
        Agent[] agents = new Agent[numAgents];
        float halfwidth = width/2;
        float halfheight = height/2;

        for(int i =0; i < numAgents; i++){
			Vector2 startPos = new Vector2(halfwidth, halfheight);
			float randomAngle = UnityEngine.Random.value * Mathf.PI * 2;
            agents[i] = new Agent() {position = startPos, angle = randomAngle };
        }
        return agents;
    }
}