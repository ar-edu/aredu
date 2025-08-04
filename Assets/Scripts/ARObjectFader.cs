using UnityEngine;

public class ARObjectFader : MonoBehaviour
{
    public float fadeSpeed = 2f;
    private float targetAlpha = 1f;
    private float currentAlpha = 1f;
    private Renderer[] renderers;

    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }

    public void SetVisible(bool visible)
    {
        targetAlpha = visible ? 1f : 0f;
    }

    void Update()
    {
        if (Mathf.Approximately(currentAlpha, targetAlpha)) return;

        currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, Time.deltaTime * fadeSpeed);
        foreach (var rend in renderers)
        {
            foreach (var mat in rend.materials)
            {
                if (mat.HasProperty("_Color"))
                {
                    var color = mat.color;
                    color.a = currentAlpha;
                    mat.color = color;

                    // Enable transparency blending
                    if (currentAlpha < 1f)
                    {
                        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                        mat.SetInt("_ZWrite", 0);
                        mat.DisableKeyword("_ALPHATEST_ON");
                        mat.EnableKeyword("_ALPHABLEND_ON");
                        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                        mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                    }
                    else
                    {
                        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                        mat.SetInt("_ZWrite", 1);
                        mat.DisableKeyword("_ALPHATEST_ON");
                        mat.DisableKeyword("_ALPHABLEND_ON");
                        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                        mat.renderQueue = -1;
                    }
                }
            }
        }
    }
}
