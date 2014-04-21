using UnityEngine;

public static class Utilities
{
    public static bool IsOdd(int value)
    {
        return value % 2 != 0;
    }

    /// <summary>
    /// Convert HSV to RGB
    /// h is from 0-360
    /// s,v values are 0-1
    /// r,g,b values are 0-255
    /// Based upon http://ilab.usc.edu/wiki/index.php/HSV_And_H2SV_Color_Space#HSV_Transformation_C_.2F_C.2B.2B_Code_2
    /// </summary>
    public static void HsvToRgb(double h, double S, double V, out int r, out int g, out int b)
    {
        // ######################################################################
        // T. Nathan Mundhenk
        // mundhenk@usc.edu
        // C/C++ Macro HSV to RGB

        double H = h;
        while (H < 0) { H += 360; };
        while (H >= 360) { H -= 360; };
        double R, G, B;
        if (V <= 0)
        { R = G = B = 0; }
        else if (S <= 0)
        {
            R = G = B = V;
        }
        else
        {
            double hf = H / 60.0;
            int i = (int)System.Math.Floor(hf);
            double f = hf - i;
            double pv = V * (1 - S);
            double qv = V * (1 - S * f);
            double tv = V * (1 - S * (1 - f));
            switch (i)
            {

                // Red is the dominant color

                case 0:
                    R = V;
                    G = tv;
                    B = pv;
                    break;

                // Green is the dominant color

                case 1:
                    R = qv;
                    G = V;
                    B = pv;
                    break;
                case 2:
                    R = pv;
                    G = V;
                    B = tv;
                    break;

                // Blue is the dominant color

                case 3:
                    R = pv;
                    G = qv;
                    B = V;
                    break;
                case 4:
                    R = tv;
                    G = pv;
                    B = V;
                    break;

                // Red is the dominant color

                case 5:
                    R = V;
                    G = pv;
                    B = qv;
                    break;

                // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

                case 6:
                    R = V;
                    G = tv;
                    B = pv;
                    break;
                case -1:
                    R = V;
                    G = pv;
                    B = qv;
                    break;

                // The color is not defined, we should throw an error.

                default:
                    //LFATAL("i Value error in Pixel conversion, Value is %d", i);
                    R = G = B = V; // Just pretend its black/white
                    break;
            }
        }
        r = Utilities.Clamp((int)(R * 255.0));
        g = Utilities.Clamp((int)(G * 255.0));
        b = Utilities.Clamp((int)(B * 255.0));
    }

    /// <summary>
    /// Clamp a value to 0-255
    /// </summary>
    static int Clamp(int i)
    {
        if (i < 0) return 0;
        if (i > 255) return 255;
        return i;
    }

    public static Color ColorFromHSV(float h, float s, float v, float a = 1)
    {
        // no saturation, we can return the value across the board (grayscale)
        if (s == 0)
            return new Color(v, v, v, a);

        // which chunk of the rainbow are we in?
        float sector = h / 60;

        // split across the decimal (ie 3.87 into 3 and 0.87)
        int i = (int)sector;
        float f = sector - i;

        float p = v * (1 - s);
        float q = v * (1 - s * f);
        float t = v * (1 - s * (1 - f));

        // build our rgb color
        Color color = new Color(0, 0, 0, a);

        switch (i)
        {
            case 0:
                color.r = v;
                color.g = t;
                color.b = p;
                break;

            case 1:
                color.r = q;
                color.g = v;
                color.b = p;
                break;

            case 2:
                color.r = p;
                color.g = v;
                color.b = t;
                break;

            case 3:
                color.r = p;
                color.g = q;
                color.b = v;
                break;

            case 4:
                color.r = t;
                color.g = p;
                color.b = v;
                break;

            default:
                color.r = v;
                color.g = p;
                color.b = q;
                break;
        }

        return color;
    }

    public static void ColorToHSV(Color color, out float h, out float s, out float v)
    {
        float min = Mathf.Min(Mathf.Min(color.r, color.g), color.b);
        float max = Mathf.Max(Mathf.Max(color.r, color.g), color.b);
        float delta = max - min;

        // value is our max color
        v = max;

        // saturation is percent of max
        if (!Mathf.Approximately(max, 0))
            s = delta / max;
        else
        {
            // all colors are zero, no saturation and hue is undefined
            s = 0;
            h = -1;
            return;
        }

        // grayscale image if min and max are the same
        if (Mathf.Approximately(min, max))
        {
            v = max;
            s = 0;
            h = -1;
            return;
        }

        // hue depends which color is max (this creates a rainbow effect)
        if (color.r == max)
            h = (color.g - color.b) / delta;         	// between yellow & magenta
        else if (color.g == max)
            h = 2 + (color.b - color.r) / delta; 		// between cyan & yellow
        else
            h = 4 + (color.r - color.g) / delta; 		// between magenta & cyan

        // turn hue into 0-360 degrees
        h *= 60;
        if (h < 0)
            h += 360;
    }

    /// <summary>
    /// Converts HSV using [0, 255] to Unity format HSV using [0, 1.0]
    /// </summary>
    /// <param name="_h"></param>
    /// <param name="_s"></param>
    /// <param name="_v"></param>
    /// <returns>A vector containing HSV values</returns>
    public static Vector3 GetHSVUnitFromHSV255(float _h, float _s, float _v)
    {
        Vector3 unitHSV = new Vector3(_h / 255.0f, _s / 255.0f, _v / 255.0f);
        return unitHSV;
    }
}