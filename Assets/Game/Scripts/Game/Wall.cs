using UnityEngine;
using System.Collections.Generic;

public class Wall : MonoBehaviour
{
    private static List<Wall> instances = new List<Wall>();

    private float initialWidth_ = 0.36f;
    public float brightenedWidth = 0.72f;
    public float duration = 0.3f;
    private float currentWidth_ = 0.36f;

    private Color initialColor_;
    public Color brightenedColor;
    private Color currentColor_;

    private float scroll_;


    private bool isBrightened_ = false;

    private Renderer renderer_;

    void OnEnable()
    {
        instances.Add(this);
        renderer_ = GetComponent<Renderer>();
        initialWidth_ = currentWidth_ = renderer_.material.GetFloat("_Width");
        initialColor_ = currentColor_ = renderer_.material.GetColor("_Color");
        scroll_ = renderer_.material.GetFloat("_ScrollSpeed");
        renderer_.material.SetFloat("_ScrollSpeed", 0f);
    }

    void OnDisable()
    {
        instances.Remove(this);
    }
	
	void Update()
    {
        if (isBrightened_) {
            currentWidth_ += (brightenedWidth - currentWidth_) * 0.1f;
            currentColor_ += (brightenedColor - currentColor_) * 0.1f;
        } else {
            currentWidth_ += (initialWidth_ - currentWidth_) * 0.1f;
            currentColor_ += (initialColor_ - currentColor_) * 0.1f;
        }
        renderer_.material.SetFloat("_Width", currentWidth_);
        renderer_.material.SetColor("_Color", currentColor_);
	}

    static public void Activate()
    {
        foreach (var instance in instances) {
            instance.renderer_.material.SetFloat("_ScrollSpeed", instance.scroll_);
        }
    }

    static public void Brighten()
    {
        foreach (var instance in instances) {
            instance._StartBrighten();
        }
    }

    void _StartBrighten()
    {
        isBrightened_ = true;
        Invoke("_StopBrighten", duration);
    }

    void _StopBrighten()
    {
        isBrightened_ = false;
    }
}
