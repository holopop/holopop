using UnityEngine;

public class ExampleOscillator : MonoBehaviour {

    // Reference to the orb itself
    Orb orb;

    // Period of 3 seconds
    float period = 3.0f;

    // Set range of sinusoids
    float min = 0.2f;
    float max = 0.8f;

    Color colorA = Color.blue;
    Color colorB = Color.green;

    void Start() {
        orb = GetComponent<Orb>();
    }

    // Calculates the value of a sinusoid pulsing between 0 and 1 for the current time.
    float sinusoid() {
        return (1 + Mathf.Sin(2 * Mathf.PI * (Time.time % period) / period)) / 2;
    }

    void Update() {
        // Make the size of the core oscillate between min and max
        orb.core = min + sinusoid() * (max - min);

        orb.coreColor = Color.Lerp(colorA, colorB, sinusoid());
    }

}
