
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
    public static Agent[] makeAgents(int numAgents, int width,int  height, Vector4 color ){
        Agent[] agents = new Agent[numAgents];
        float halfwidth = width/2;
        float halfheight = height/2;

        for(int i =0; i < numAgents; i++){
			Vector2 startPos = new Vector2(halfwidth, halfheight);
			float randomAngle = UnityEngine.Random.value * Mathf.PI * 2;
            agents[i] = new Agent() {position = startPos, angle = randomAngle, color = color };
        }
        return agents;
    }
    public static Agent[] makeAgentsInCircle(int numAgents, int width,int  height, Vector4 color){
        Agent[] agents = new Agent[numAgents];
        float halfwidth = width/2;
        float halfheight = height/2;

        for(int i =0; i < numAgents; i++){
            Vector2 startPosCircle = Random.insideUnitCircle * 10;
			Vector2 startPos = new Vector2(halfwidth + startPosCircle.x , halfheight+ startPosCircle.y);
			float randomAngle = UnityEngine.Random.value * Mathf.PI * 2;
            agents[i] = new Agent() {position = startPos, angle = randomAngle, color = color  };
        }
        return agents;
    }
    public static Agent[] makeAgentsInCircleAngleCenter(int numAgents, int width,int  height, int CircleRadius, Vector4[] color){
        int length = color.Length;
        int index = 0;
        Agent[] agents = new Agent[numAgents];
        float halfwidth = width/2;
        float halfheight = height/2;
        
        for(int i =0; i < numAgents; i++){
            Vector2 startPosCircle = Random.insideUnitCircle * CircleRadius;
			Vector2 startPos = new Vector2(halfwidth + startPosCircle.x , halfheight+ startPosCircle.y);
			float AngleToCenter = getAngleBetween(startPos.x,startPos.y,halfwidth,halfheight);
            if(index >=  length){
                index = 0;
            }
            agents[i] = new Agent() {position = startPos, angle = AngleToCenter, color = color[index]};
            index++;
        }
        return agents;
    }
    public static float getAngleBetween(float startX, float startY, float endX,float endY){
        float deltaX = endX - startX;  
        float deltaY = endY - startY;  
        return Mathf.Atan2(deltaY , deltaX );
    }
}