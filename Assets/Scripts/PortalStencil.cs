using UnityEngine;
using System.Numerics;

public class PortalStencil : MonoBehaviour
{
    public float width = 1f;
    public float height = 1f;
    public float depth = 1f;
    public float thickness = 0.01f;

    public Color frame_color = new Color(0.5f, 0.5f, 0.5f, 1.0f);

    public Color box_color = new Color(0f, 1f, 0f, 1.0f);

    void Start()
    {
        GameObject window = GameObject.CreatePrimitive(PrimitiveType.Quad);
        window.name = "window_quad";
        window.transform.localPosition = new UnityEngine.Vector3(0f, 0f, 0f);
        window.transform.localScale = new UnityEngine.Vector3(width, height, thickness);
        window.transform.SetParent(transform, worldPositionStays: false);

        GameObject top = GameObject.CreatePrimitive(PrimitiveType.Cube);
        top.name = "window_top";
        top.transform.localPosition = new UnityEngine.Vector3(0, height / 2.0f, 0);
        top.transform.localScale = new UnityEngine.Vector3(width, thickness, thickness);
        top.transform.SetParent(transform, worldPositionStays: false);

        GameObject bottom = GameObject.CreatePrimitive(PrimitiveType.Cube);
        bottom.name = "window_bottom";
        bottom.transform.localPosition = new UnityEngine.Vector3(0, -height / 2.0f, 0);
        bottom.transform.localScale = new UnityEngine.Vector3(width, thickness, thickness);
        bottom.transform.SetParent(transform, worldPositionStays: false);

        GameObject right = GameObject.CreatePrimitive(PrimitiveType.Cube);
        right.name = "window_right";
        right.transform.localPosition = new UnityEngine.Vector3(width / 2.0f, 0, 0);
        right.transform.localScale = new UnityEngine.Vector3(thickness, height, thickness);
        right.transform.SetParent(transform, worldPositionStays: false);


        GameObject left = GameObject.CreatePrimitive(PrimitiveType.Cube);
        left.name = "window_left";
        left.transform.localPosition = new UnityEngine.Vector3(-width / 2.0f, 0, 0);
        left.transform.localScale = new UnityEngine.Vector3(thickness, height, thickness);
        left.transform.SetParent(transform, worldPositionStays: false);

        GameObject back_wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        back_wall.name = "back_wall";
        back_wall.transform.localScale = new UnityEngine.Vector3(width, height, thickness);
        back_wall.transform.localPosition = new UnityEngine.Vector3(0f, 0f, depth);
        back_wall.transform.SetParent(transform, worldPositionStays: false);


        GameObject left_wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        left_wall.name = "left_wall";
        left_wall.transform.localScale = new UnityEngine.Vector3(width, height, thickness);
        left_wall.transform.localRotation = UnityEngine.Quaternion.Euler(0, 90, 0);
        left_wall.transform.localPosition = new UnityEngine.Vector3(-width / 2.0f, 0f, depth / 2.0f);
        left_wall.transform.SetParent(transform, worldPositionStays: false);

        GameObject right_wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        right_wall.name = "right_wall";
        right_wall.transform.localScale = new UnityEngine.Vector3(width, height, thickness);
        right_wall.transform.localRotation = UnityEngine.Quaternion.Euler(0, -90, 0);
        right_wall.transform.localPosition = new UnityEngine.Vector3(width / 2.0f, 0f, depth / 2.0f);
        right_wall.transform.SetParent(transform, worldPositionStays: false);

        GameObject top_wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        top_wall.name = "top_wall";
        top_wall.transform.localScale = new UnityEngine.Vector3(width, height, thickness);
        top_wall.transform.localRotation = UnityEngine.Quaternion.Euler(90, 0, 0);
        top_wall.transform.localPosition = new UnityEngine.Vector3(0, height / 2.0f, depth / 2.0f);
        top_wall.transform.SetParent(transform, worldPositionStays: false);

        GameObject bottom_wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        bottom_wall.name = "bottom_wall";
        bottom_wall.transform.localScale = new UnityEngine.Vector3(width, height, thickness);
        bottom_wall.transform.localRotation = UnityEngine.Quaternion.Euler(-90, 0, 0);
        bottom_wall.transform.localPosition = new UnityEngine.Vector3(0, -height / 2.0f, depth / 2.0f);
        bottom_wall.transform.SetParent(transform, worldPositionStays: false);

        Shader shader = Shader.Find("Universal Render Pipeline/Lit"); // or "Universal Render Pipeline/Lit" for URP
        Material frame_mat = new Material(shader);
        Material wall_mat = new Material(shader);
        frame_mat.color = frame_color;
        wall_mat.color = box_color;

        //Shader window_shader = Shader.Find("Custom/PortalWindow");
        //Material window_mat = new Material(window_shader);

        Material window_mat = Resources.Load<Material>("Materials/PortalWindow");
        window.GetComponent<Renderer>().material = window_mat;

        Material backwall_mat = new Material(Resources.Load<Material>("Materials/StencilFilter"));
        backwall_mat.SetColor("_Color", Color.red);

        Material leftwall_mat = new Material(Resources.Load<Material>("Materials/StencilFilter"));
        leftwall_mat.SetColor("_Color", Color.blue);

        Material rightwall_mat = new Material(Resources.Load<Material>("Materials/StencilFilter"));
        rightwall_mat.SetColor("_Color", Color.yellow);

        Material topwall_mat = new Material(Resources.Load<Material>("Materials/StencilFilter"));
        topwall_mat.SetColor("_Color", Color.gray);

        Material bottomwall_mat = new Material(Resources.Load<Material>("Materials/StencilFilter"));
        bottomwall_mat.SetColor("_Color", Color.green);

        // set internal wall materials
        back_wall.GetComponent<Renderer>().material = backwall_mat;
        left_wall.GetComponent<Renderer>().material = leftwall_mat;
        right_wall.GetComponent<Renderer>().material = rightwall_mat;
        top_wall.GetComponent<Renderer>().material = topwall_mat;
        bottom_wall.GetComponent<Renderer>().material = bottomwall_mat;

        // set frame material
        top.GetComponent<Renderer>().material = frame_mat;
        bottom.GetComponent<Renderer>().material = frame_mat;
        left.GetComponent<Renderer>().material = frame_mat;
        right.GetComponent<Renderer>().material = frame_mat;

        // switch of window shadows
        top.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        bottom.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        left.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        right.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}