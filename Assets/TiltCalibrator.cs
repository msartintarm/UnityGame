using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltCalibrator : MonoBehaviour {

    private enum CalibrationState { NON_TILT_DEVICE, UNCALIBRATED, TILTING_LEFT, TILTING_RIGHT };

    private CalibrationState cali ;

    private Vector3 tiltAxis;

    private static float Threshold = 0.2f;

    private LineRenderer tiltRenderer;

    private Playerrr player;

	void Start () {
        tiltRenderer = GetComponent<LineRenderer>();
        tiltRenderer.enabled = false;
        cali = CalibrationState.NON_TILT_DEVICE;
        player = GameObject.FindObjectOfType<Playerrr>();
        tiltAxis = Vector3.Normalize(Input.acceleration);
    }

    // On update, start listening to tilt input.
    void Update () {

        Vector3 newAxis = Vector3.Normalize(Input.acceleration);

        switch (cali)
        {
            case CalibrationState.NON_TILT_DEVICE:
                if (Vector3.Magnitude(newAxis) > Threshold) {
                    tiltAxis = newAxis;
                    cali = CalibrationState.UNCALIBRATED;
                    tiltRenderer.enabled = true;
                    player.enableTiltControls = true;
                }
                break;

            case CalibrationState.UNCALIBRATED:
                if (Input.GetButtonDown("Fire1"))
                {
                    cali = CalibrationState.NON_TILT_DEVICE;
                    player.enableTiltControls = false;
                }
                break;
        }
	}
}
