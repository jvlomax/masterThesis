using UnityEngine;
using System.Collections;

public class AutoFade : MonoBehaviour {

    //taken from http://answers.unity3d.com/questions/119918/transitions-between-changing-scenes.html
    private static AutoFade m_Instance = null;
    private Material m_Material = null;
    private string m_LevelName = "";
    private int m_LevelIndex = 0;
    private bool m_Fading = false;


    private static AutoFade Instance {
        get {
            if(m_Instance == null){
                m_Instance = (new GameObject("AutoFade")).AddComponent<AutoFade>();
            }
            return m_Instance;
        }
    }

    public static bool Fading {
        get {
            return Instance.m_Fading;
        }
    }

    private void Awake() {
        DontDestroyOnLoad(this);
        m_Instance = this;
        m_Material = new Material("Shader \"plane/No zTest\" { SubShader { Pass {Blend SrcAlpha OneMinusSrcAlpha ZWrite Off Cull Off Fog {Mode Off} BindChannels { Bind\"Color\",color}}}}");
    }

    private void DrawQuad(Color color, float alpha) {
        color.a = alpha;
        m_Material.SetPass(0);
        GL.PushMatrix();
        GL.LoadOrtho();
        GL.Begin(GL.QUADS);
        GL.Color(color);
        GL.Vertex3(0, 0, -1);
        GL.Vertex3(0, 1, -1);
        GL.Vertex3(1, 1, -1);
        GL.Vertex3(1, 0, -1);
        GL.End();
        GL.PopMatrix();
    }


    private IEnumerator Fade(float fadeOutTime, float fadeInTime, Color color) {
        float t = 0.0f;
        while (t < 1.0f) {
            yield return new WaitForEndOfFrame();
            t = Mathf.Clamp01(t + Time.deltaTime / fadeOutTime);
            DrawQuad(color, t);
        }
        if (m_LevelName != "") {
            Application.LoadLevel(m_LevelName);
        } else {
            Application.LoadLevel(m_LevelIndex);
        }

        while (t > 0.0f) {
            yield return new WaitForEndOfFrame();
            t = Mathf.Clamp01(t - Time.deltaTime / fadeInTime);
            DrawQuad(color, t);
        }
        m_Fading = false;
    }

    private void StartFade(float fadeOutTime, float fadeInTime, Color color) {
        m_Fading = true;
        StartCoroutine(Fade(fadeOutTime, fadeInTime, color));
    }

    public static void LoadLevel(string levelName, float fadeOutTime, float fadeInTime, Color color) {
        if (Fading) return;
        Instance.m_LevelName = levelName;
        Instance.StartFade(fadeOutTime, fadeInTime, color);
    }

    public static void LoadLevel(int levelIndex, float fadeOutTime, float fadeInTime, Color color) {
        if (Fading) return;
        Instance.m_LevelName = "";
        Instance.m_LevelIndex = levelIndex;
        Instance.StartFade(fadeOutTime, fadeInTime, color);
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
