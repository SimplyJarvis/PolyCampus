using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawLineUI : Graphic
{

    [SerializeField] float thickness;
    public Vector2 curMousePos;
    public Vector2 startMousePos;
    [SerializeField] Canvas UIcanvas;

    Vector3 vecDirection;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        if (curMousePos == Vector2.zero)
        {
            vh.Clear();
        }
        else
        {

            //float screenScale = (720f / Camera.main.pixelHeight); //This works to fix the scaling issue, however you are limited to 16:9 screens with this
            //GetComponent<RectTransform>().localScale = new Vector3(screenScale, screenScale, screenScale);

            float lineThickness = thickness + (Camera.main.pixelHeight / 720f);

            vecDirection = (curMousePos - startMousePos).normalized;
            Vector3 vecCross = Vector2.Perpendicular(vecDirection);

            vh.Clear();

            float width = rectTransform.rect.width;
            float height = rectTransform.rect.height;

            UIVertex vertex = UIVertex.simpleVert;
            vertex.color = color;


            vertex.position = new Vector3(startMousePos.x + (vecCross.x * lineThickness), startMousePos.y + (vecCross.y * lineThickness));
            vh.AddVert(vertex);
            vertex.position = new Vector3(startMousePos.x - (vecCross.x * lineThickness), startMousePos.y - (vecCross.y * lineThickness));
            vh.AddVert(vertex);


            vertex.position = new Vector3(curMousePos.x + (vecCross.x * lineThickness), curMousePos.y + (vecCross.y * lineThickness));
            vh.AddVert(vertex);
            vertex.position = new Vector3(curMousePos.x - (vecCross.x * lineThickness), curMousePos.y - (vecCross.y * lineThickness));
            vh.AddVert(vertex);

            //Debug.Log(screenScale);

            vh.AddTriangle(0, 1, 3);
            vh.AddTriangle(3, 2, 0);

        }
    }

    public void SetStart(Vector2 value)
    {
        startMousePos = value;
    }

    public void setMousePos(Vector2 value)
    {
        curMousePos = value;
        SetVerticesDirty();
    }


}
