using UnityEngine;
using Random = UnityEngine.Random;

namespace Renge.Util {

	public static class Extensions {

		public static Texture2D ToTexture2D(this AnimationCurve curve, int width = 500, int height = 1) {
			if (curve == null) return new Texture2D(1, 1);
			Texture2D texture = new Texture2D(width, height, TextureFormat.RGBAFloat, false);
			texture.wrapMode = TextureWrapMode.Clamp;
			for (int i = 0; i < width; i++) {
				float v = curve.Evaluate((float)i / width);
				Color c = new Color(v, v, v, v);
				for (int j = 0; j < height; j++) {
					texture.SetPixel(i, j, c);
				}
			}
			texture.Apply();
			return texture;
		}

        public static Texture2D ToTexture2D(this Gradient gradient, int width = 250, int height = 1) {
			if (gradient == null) return new Texture2D(1, 1);
			Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
			texture.wrapMode = TextureWrapMode.Clamp;
			if (gradient.mode == GradientMode.Fixed) {
				texture.filterMode = FilterMode.Point;
			}
			for (int i = 0; i < width; i++) {
				Color c = gradient.Evaluate((float)i / width);
				for (int j = 0; j < height; j++) {
					texture.SetPixel(i, j, c);
				}
			}
			texture.Apply();
			return texture;
		}
		public static bool EqualTo(this Gradient a, Gradient b, int samples = 5) {
			if (a.mode != b.mode || a.colorKeys.Length != b.colorKeys.Length || a.alphaKeys.Length != b.alphaKeys.Length) return false;

			for (int i = 0; i < a.colorKeys.Length; i++) {
                if(!a.colorKeys[i].Equals(b.colorKeys[i]))
                    return false;
			}
            for (int i = 0; i < a.alphaKeys.Length; i++) {
                if(!a.alphaKeys[i].Equals(b.alphaKeys[i]))
                    return false;
			}
			return true;
		}

		public static Gradient Clone(this Gradient gradient) {
			Gradient clone = new Gradient();
			clone.mode = gradient.mode;
			clone.alphaKeys = (GradientAlphaKey[])gradient.alphaKeys.Clone();
			clone.colorKeys = (GradientColorKey[])gradient.colorKeys.Clone();
			return clone;
		}

		public static bool EqualTo(this AnimationCurve a, AnimationCurve b) {
			if (a.length != b.length) return false;

			for (int i = 0; i < a.keys.Length; i++) {
				var key1 = a.keys[i];
				var key2 = b.keys[i];
				if (key1.time != key2.time || key1.value != key2.value)
					return false;
			}
			return true;
		}

		public static AnimationCurve Clone(this AnimationCurve curve) {
			AnimationCurve clone = new AnimationCurve();
			clone.keys = (Keyframe[])curve.keys.Clone();
			return clone;
		}

		public static Color RandomColor(this Color color, bool includeAlpha = false, float alphaCutoff = 0.5f) {
			float alpha = System.Convert.ToInt32(includeAlpha) * Random.value;
			return new Color(Random.value, Random.value, Random.value, (alpha <= alphaCutoff ? 0 : alpha) + System.Convert.ToInt32(!includeAlpha));
        }

        //public static float? ReadAsFloat(this JsonReader reader) {
        //    var str = reader.ReadAsString();

        //    if (string.IsNullOrEmpty(str)) {
        //        return null;
        //    } else if (float.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out var valueParsed)) {
        //        return valueParsed;
        //    } else {
        //        return 0f;
        //    }
        //}

        //public static JsonSerializationException CreateSerializationException(this JsonReader reader, string message, Exception innerException = null) {
        //    StringBuilder builder = CreateStringBuilderWithSpaceAfter(message);

        //    builder.AppendFormat(CultureInfo.InvariantCulture, "Path '{0}'", reader.Path);

        //    var lineInfo = reader as IJsonLineInfo;
        //    int lineNumber = default;
        //    int linePosition = default;

        //    if (lineInfo?.HasLineInfo() == true) {
        //        lineNumber = lineInfo.LineNumber;
        //        linePosition = lineInfo.LinePosition;
        //        builder.AppendFormat(CultureInfo.InvariantCulture, ", line {0}, position {1}", lineNumber, linePosition);
        //    }

        //    builder.Append('.');

        //    return new JsonSerializationException(
        //        message: builder.ToString(), reader.Path, lineNumber, linePosition, innerException);
        //}
    }
}