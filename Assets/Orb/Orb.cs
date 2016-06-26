using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Orb : MonoBehaviour {

    /** Size of the core **/
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float _core = 0.5f;
    public float core {
        get {
            return _core;
        }
        set {
            _core = value;
            Revalidate();
        }
    }

    /** Thickness of the glow **/
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float _glowThickness = 0.5f;
    public float glowThickness {
        get {
            return _glowThickness;
        }
        set {
            _glowThickness = value;
            Revalidate();
        }
    }

    /** Color of the core **/
    [SerializeField]
    private Color _coreColor = Color.white;
    public Color coreColor {
        get {
            return _coreColor;
        }
        set {
            _coreColor = value;
            Revalidate();
        }
    }

    /** Color of the edge **/
    [SerializeField]
    private Color _edgeColor = Color.black;
    public Color edgeColor {
        get {
            return _edgeColor;
        }
        set {
            _edgeColor = value;
            Revalidate();
        }
    }

    /** Color of the glow **/
    [SerializeField]
    private Color _glowColor = Color.white;
    public Color glowColor {
        get {
            return _glowColor;
        }
        set {
            _glowColor = value;
            Revalidate();
        }
    }

    /** Color of the noise **/
    [SerializeField]
    private Color _noiseColor = Color.black;
    public Color noiseColor {
        get {
            return _noiseColor;
        }
        set {
            _noiseColor = value;
            Revalidate();
        }
    }

    /** Color of the noise **/
    [SerializeField]
    private Vector3 _noiseAnimation = Vector3.up;
    public Vector3 noiseAnimation {
        get {
            return _noiseAnimation;
        }
        set {
            _noiseAnimation = value;
            Revalidate();
        }
    }

    /** Amount of noise **/
    [SerializeField]
    private float _noiseAmount = 10.0f;
    public float noiseAmount {
        get {
            return _noiseAmount;
        }
        set {
            _noiseAmount = value;
            Revalidate();
        }
    }

    /** Amplitude of noise **/
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float _noiseAmplitude = 0.3f;
    public float noiseAmplitude {
        get {
            return _noiseAmplitude;
        }
        set {
            _noiseAmplitude = value;
            Revalidate();
        }
    }

    /** Light intensity **/
    [SerializeField]
    [Range(0.0f, 8.0f)]
    private float _lightIntensity = 2.0f;
    public float lightIntensity {
        get {
            return _lightIntensity;
        }
        set {
            _lightIntensity = value;
            Revalidate();
        }
    }

    /** The range of light relative to the scale**/
    [SerializeField]
    private float _relativeLightRange = 4.0f;
    public float relativeLightRange {
        get {
            return _relativeLightRange;
        }
        set {
            _relativeLightRange = value;
            Revalidate();
        }
    }

    /** Shader IDs **/
    private int coreID;
    private int glowThicknessID;
    private int coreColorID;
    private int edgeColorID;
    private int glowColorID;
    private int noiseColorID;
    private int noiseAnimationID;
    private int noiseAmountID;
    private int noiseAmplitudeID;
    private int centerID;
    private int radiusID;

    /** Components **/
    private Material material;
    private Renderer renders;
    private Light emitter;
    
    void Start() {
        // Get Shader IDs
        coreID = Shader.PropertyToID("_Core");
        glowThicknessID = Shader.PropertyToID("_GlowThickness");
        coreColorID = Shader.PropertyToID("_Color");
        edgeColorID = Shader.PropertyToID("_EdgeColor");
        glowColorID = Shader.PropertyToID("_GlowColor");
        noiseColorID = Shader.PropertyToID("_NoiseColor");
        noiseAnimationID = Shader.PropertyToID("_Animation");
        noiseAmountID = Shader.PropertyToID("_Amount");
        noiseAmplitudeID = Shader.PropertyToID("_Amplitude");
        centerID = Shader.PropertyToID("_Center");
        radiusID = Shader.PropertyToID("_Radius");

        // Get Components
        renders = GetComponent<Renderer>();
        emitter = GetComponent<Light>();

        // Get Material
        if (material == null) {
            material = new Material(Shader.Find("Orb/OrbShader"));
        }
        renders.sharedMaterial = material;
        renders.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renders.receiveShadows = false;
        
        // Revalidate variables
        OnValidate();
    }
    
    // Returns the minimum radius of the ellipsoid
    private float MinRadius() {
        return Mathf.Min(Mathf.Abs(transform.lossyScale.x),
                         Mathf.Abs(transform.lossyScale.y),
                         Mathf.Abs(transform.lossyScale.z)) / 2;
    }

    void Update() {
        // Set shader properties
        renders.sharedMaterial.SetVector(centerID, transform.position);
        renders.sharedMaterial.SetFloat(radiusID, MinRadius());
    }

    void OnValidate() {
        Revalidate();
    }

    void Revalidate() {
        // Validate variables
        _core = Mathf.Clamp(_core, 0, 1);
        _glowThickness = Mathf.Clamp(_glowThickness, 0, 1);
        _noiseAmplitude = Mathf.Clamp(_noiseAmplitude, 0, 1);
        _noiseAmount = Mathf.Abs(_noiseAmount);
        _lightIntensity = Mathf.Max(_lightIntensity, 0);
        _relativeLightRange = Mathf.Max(_relativeLightRange, 0);
        
        // Update shader properties
        if (renders != null) {
            renders.sharedMaterial.SetFloat(coreID, _core);
            renders.sharedMaterial.SetFloat(glowThicknessID, _glowThickness);
            renders.sharedMaterial.SetColor(coreColorID, _coreColor);
            renders.sharedMaterial.SetColor(edgeColorID, _edgeColor);
            renders.sharedMaterial.SetColor(glowColorID, _glowColor);
            renders.sharedMaterial.SetColor(noiseColorID, _noiseColor);
            renders.sharedMaterial.SetVector(noiseAnimationID, _noiseAnimation);
            renders.sharedMaterial.SetFloat(noiseAmountID, _noiseAmount);
            renders.sharedMaterial.SetFloat(noiseAmplitudeID, _noiseAmplitude);
        }

        // Recalculate light
        RecalculateLight();
    }

    private void RecalculateLight() {
        if (emitter == null) return;

        float noise = _noiseAmplitude * (1 - _core);
        float total = noise + _glowThickness * (1 - _core) + _core;

        if (total == 0) return;

        // Calculate weighed average of colors
        emitter.color = (noise         * _noiseColor
                      +  _core * (_coreColor + _edgeColor)
                      +  _glowThickness * (1 - _core) * _glowColor)
                        / total;

        // Set intensity and range
        emitter.intensity = total * _lightIntensity;
        emitter.range = MinRadius() * _relativeLightRange;
    }
    
}
