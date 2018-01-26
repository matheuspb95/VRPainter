using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBrush : MonoBehaviour
{
    public int resolution = 512;
    Texture2D whiteMap;
    public float brushSize;
    public Texture2D brushTexture;
    Vector2 stored, lastPoint;
    public Color brushColor;
    Color[] resetColorArray;
    public static Dictionary<Collider, RenderTexture> paintTextures = new Dictionary<Collider, RenderTexture>();
    void Start()
    {
        CreateClearTexture();// clear white texture to draw on
        resetColorArray = brushTexture.GetPixels();
        SetBrushColor();
    }

    void SetBrushColor(){
        for (int i = 0; i < resetColorArray.Length; i++) {
            resetColorArray[i] = new Color(brushColor.r, brushColor.g, brushColor.b, resetColorArray[i].a);
        }        
        brushTexture.SetPixels(resetColorArray);
        brushTexture.Apply();
    }

    void Update()
    {

        Debug.DrawRay(transform.position, transform.forward * 10, Color.magenta);
        if(Input.GetMouseButton(0)){
            
        
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //if (Physics.Raycast(transform.position, transform.forward, out hit))
            if (Physics.Raycast(new Ray(ray.GetPoint(100), -ray.direction), out hit)) // delete previous and uncomment for mouse painting
            {
                Collider coll = hit.collider;
                Debug.Log(hit.collider);
                if (coll != null)
                {
                    if (!paintTextures.ContainsKey(coll)) // if there is already paint on the material, add to that
                    {
                        Renderer rend = hit.transform.GetComponent<Renderer>();
                        paintTextures.Add(coll, getWhiteRT());
                        rend.material.SetTexture("_PaintMap", paintTextures[coll]);
                    }
                    if (stored != hit.lightmapCoord) // stop drawing on the same point
                    {
                        stored = hit.lightmapCoord;
                        Vector2 pixelUV = hit.lightmapCoord;
                        pixelUV.y *= resolution;
                        pixelUV.x *= resolution;
                        if(lastPoint ==  Vector2.zero)
                            DrawTexture(paintTextures[coll], pixelUV.x, pixelUV.y);
                        else
                            DrawLine(paintTextures[coll], lastPoint, pixelUV);
                        lastPoint = pixelUV;
                    }
                }
            }
        }

        if(Input.GetMouseButtonUp(0)){
            lastPoint = Vector2.zero;
        }
        
    }

    void DrawTexture(RenderTexture rt, float posX, float posY)
    {

        RenderTexture.active = rt; // activate rendertexture for drawtexture;
        GL.PushMatrix();                       // save matrixes
        GL.LoadPixelMatrix(0, resolution, resolution, 0);      // setup matrix for correct size

        // draw brushtexture
        Graphics.DrawTexture(new Rect(posX - brushTexture.width / brushSize, (rt.height - posY) - brushTexture.height / brushSize, brushTexture.width / (brushSize * 0.5f), brushTexture.height / (brushSize * 0.5f)), brushTexture);
        GL.PopMatrix();
        RenderTexture.active = null;// turn off rendertexture

    }

    public void DrawLine(RenderTexture rt, Vector2 init, Vector2 end){
		int divs = (int)(Vector2.Distance(init, end) * brushSize);
		float diffX = end.x - init.x;
		float diffY = end.y - init.y;
		float propx = diffX / divs;
		float propy = diffY / divs;

		for(int i = 0; i < divs; i++){
			DrawTexture(rt, init.x + propx * i, init.y + propy * i);
		}
	}

    RenderTexture getWhiteRT()
    {
        RenderTexture rt = new RenderTexture(resolution, resolution, 32);
        Graphics.Blit(whiteMap, rt);
        return rt;
    }

    void CreateClearTexture()
    {
        whiteMap = new Texture2D(1, 1);
        whiteMap.SetPixel(0, 0, Color.white);
        whiteMap.Apply();
    }
}