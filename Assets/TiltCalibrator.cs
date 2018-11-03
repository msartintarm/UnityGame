using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltCalibrator : MonoBehaviour {

    private enum CalibrationState { NON_TILT_DEVICE, UNCALIBRATED, TILTING_LEFT, TILTING_RIGHT };

    private CalibrationState cali = CalibrationState.UNCALIBRATED;

    private Vector3 tiltAxis;

    private static float Threshold = 0.2f;

	void Start () {
        tiltAxis = Vector3.Normalize(Input.acceleration);
        gameObject.SetActive(false);
    }

    // On update, start listening to tilt input.
    void Update () {

        Vector3 newAxis = Vector3.Normalize(Input.acceleration);

        switch (cali)
        {
            case CalibrationState.NON_TILT_DEVICE:
                if (Vector3.Magnitude(newAxis - tiltAxis) > Threshold) {
                    Debug.Log("Tilt Device Detected.");
                    tiltAxis = newAxis;
                    cali = CalibrationState.UNCALIBRATED;
                    gameObject.SetActive(true);
                }
                break;

            case CalibrationState.UNCALIBRATED:
                if (Input.GetButtonDown("Fire1"))
                {
                    cali = CalibrationState.NON_TILT_DEVICE;
                }
                break;
        }
	}
}
