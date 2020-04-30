using UnityEngine;

public class LightColor : MonoBehaviour
{
    static Color red = new Color(1, 0, 0, 1);
    static Color green = new Color(0, 0.5f, 0, 1);
    static Color blue = new Color(0, 0, 0.5f, 1);
    static Color cyan = new Color(0, 1, 1, 1);
    static Color yellow = new Color(1, 1, 0, 1);
    static Color magenta = new Color(1, 0, 0.6f, 1);
    static Color white = new Color(1, 1, 1, 1);

    public bool r = false;
    public bool g = false;
    public bool b = false;

    public LightColor SetColor(bool red, bool green, bool blue)
    {
        r = red;
        g = green;
        b = blue;
        return this;
    }

    public LightColor SetColor(LightColor c)
    {
        r = c.r;
        g = c.g;
        b = c.b;
        return this;
    }

    public Color GetColor()
    {
        if (r && g && b) return LightColor.white;
        else if (r && g) return LightColor.yellow;
        else if (r && b) return LightColor.magenta;
        else if (b && g) return LightColor.cyan;
        else if (r) return LightColor.red;
        else if (g) return LightColor.green;
        else if (b) return LightColor.blue;
        else return LightColor.white;
    }
}
