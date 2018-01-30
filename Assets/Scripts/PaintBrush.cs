using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBrush : MonoBehaviour
{
    GearControllerScript controllerScript;
    public int resolution = 512;
    Texture2D whiteMap;
    public float brushSize;
    public float MinBrushSize, MaxBrushSize;
    public Texture2D brushTexture;
    Texture2D brushBuffer;
    Vector2 stored, lastPoint;
    public Color brushColor;
    Color[] resetColorArray;

    public GameObject ColorWheel;
    public GameObject ColorSelect;

    public LineRenderer line;
    public Transform Point;

    public static Dictionary<Collider, RenderTexture> paintTextures = new Dictionary<Collider, RenderTexture>();
    void Start()
    {
        controllerScript = GetComponent<GearControllerScript>();
        CreateClearTexture();// clear white texture to draw on
        resetColorArray = brushTexture.GetPixels();
        SetBrushColor(brushColor);
    }

    void SetBrushColor(Color color){
        brushBuffer = new Texture2D(brushTexture.width, brushTexture.height);
        for (int i = 0; i < resetColorArray.Length; i++) {
            resetColorArray[i] = new Color(color.r, color.g, color.b, color.a * resetColorArray[i].a);
        }
        brushBuffer.SetPixels(resetColorArray);
        brushBuffer.Apply();
    }

    void Update()
    {

        //Debug.DrawRay(transform.position, transform.forward * 10, Color.magenta);
        bool ButtonDown = false;

        if(controllerScript.ControllerIsConnected){
            if(OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
                ButtonDown = true;
            if(OVRInput.Get(OVRInput.Button.PrimaryTouchpad)){
                ColorWheel.SetActive(true);
                ColorWheel.transform.position = controllerScript.ControllerWorldPosition;
                Vector3 ctrlRot = controllerScript.ControllerWorldRotation.eulerAngles;
                ColorSelect.transform.rotation = Quaternion.Euler(0,180,ctrlRot.z);
            }
            if(OVRInput.GetUp(OVRInput.Button.PrimaryTouchpad)){
                ColorWheel.SetActive(false);
                float hue = ColorSelect.transform.eulerAngles.z / 360;
                //Debug.Log(hue);
                SetBrushColor(hsvToRgb(hue));
            }
            if(OVRInput.Get(OVRInput.Touch.PrimaryTouchpad)){
                float y = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad).y;
                brushSize = (((y + 1) / 2) * MaxBrushSize) + MinBrushSize;
                Point.localScale = new Vector3(1000/brushSize, 1000/brushSize, 1);
            }
        }else {
            if(Input.GetMouseButton(0)){
                ButtonDown = true;
            }
        }
        Point.localScale = new Vector3(1/brushSize, 1/brushSize, 1);

        if(ButtonDown){       
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (controllerScript.ControllerIsConnected) {	
                ray = controllerScript.ControllerRay;
            }

            //if (Physics.Raycast(transform.position, transform.forward, out hit))
            if (Physics.Raycast(new Ray(ray.GetPoint(100), -ray.direction), out hit)) // delete previous and uncomment for mouse painting
            {
                Collider coll = hit.collider;
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

        //Debug.Log(1 / Time.deltaTime);
        
    }

    void DrawTexture(RenderTexture rt, float posX, float posY)
    {

        RenderTexture.active = rt; // activate rendertexture for drawtexture;
        GL.PushMatrix();                       // save matrixes
        GL.LoadPixelMatrix(0, resolution, resolution, 0);      // setup matrix for correct size

        // draw brushtexture
        Graphics.DrawTexture(new Rect(posX - brushBuffer.width / brushSize, (rt.height - posY) - brushBuffer.height / brushSize, brushBuffer.width / (brushSize * 0.5f), brushBuffer.height / (brushSize * 0.5f)), brushBuffer);
        GL.PopMatrix();
        RenderTexture.active = null;// turn off rendertexture

    }

    public void DrawLine(RenderTexture rt, Vector2 init, Vector2 end){
        float dist = Vector2.Distance(init, end);
        //Debug.Log(dist);
        if(dist > 500) return;
		int divs = (int)(dist * brushSize/10);
        //Debug.Log(divs);
        //if(divs > 500) return;
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

    Color hsvToRgb(float h){
        float r = 0, g = 0, b = 0;
        float s = 1,v = 1;

        int i = Mathf.FloorToInt(h * 6);
        float f = h * 6 - i;
        float p = v * (1 - s);
        float q = v * (1 - f * s);
        float t = v * (1 - (1 - f) * s);

        switch(i % 6){
            case 0: r = v; g = t; b = p; break;
            case 1: r = q; g = v; b = p; break;
            case 2: r = p; g = v; b = t; break;
            case 3: r = p; g = q; b = v; break;
            case 4: r = t; g = p; b = v; break;
            case 5: r = v; g = p; b = q; break;
        }

        return new Color(r, g, b, 1);
    }
}