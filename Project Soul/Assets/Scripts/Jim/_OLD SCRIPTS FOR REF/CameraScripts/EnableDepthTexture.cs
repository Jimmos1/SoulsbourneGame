using UnityEngine;
[ExecuteInEditMode]
public class EnableDepthTexture : MonoBehaviour
{
    //Enables camera depth mode so that fading material work /problem with unity
    void Start()
    {
        Camera.main.depthTextureMode = DepthTextureMode.Depth;
    }
}
