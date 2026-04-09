using System.Collections.Generic;
using UnityEngine;
using Mediapipe.Tasks.Vision.HandLandmarker;

public class handResultAdapter
{
    public handState[] convert(HandLandmarkerResult result)
    {
        if (result.handLandmarks == null) return new handState[0];
        if (result.handedness == null) return new handState[0];
        if (result.handLandmarks.Count != result.handedness.Count) return new handState[0];

        List<handState> hands = new List<handState>();

        for (int i = 0; i < result.handLandmarks.Count; i++)
        {
            var landmarks = result.handLandmarks[i].landmarks;
            var handed = result.handedness[i];

            if (handed.categories == null || handed.categories.Count == 0) continue;
            if (landmarks == null || landmarks.Count < 21) continue;

            handType type = handed.categories[0].categoryName == "Left" ? handType.left : handType.right;

            Dictionary<fingerType, fingerState> fingers = new Dictionary<fingerType, fingerState>
            {
                { fingerType.thumb, new fingerState(toVec(landmarks[4]), toVec(landmarks[2])) },
                { fingerType.index, new fingerState(toVec(landmarks[8]), toVec(landmarks[5])) },
                { fingerType.middle, new fingerState(toVec(landmarks[12]), toVec(landmarks[9])) },
                { fingerType.ring, new fingerState(toVec(landmarks[16]), toVec(landmarks[13])) },
                { fingerType.little, new fingerState(toVec(landmarks[20]), toVec(landmarks[17])) }
            };

            hands.Add(new handState(type, fingers));
        }

        return hands.ToArray();
    }

    private Vector3 toVec(Mediapipe.Tasks.Components.Containers.NormalizedLandmark l)
    {
        return new Vector3(l.x, l.y, l.z);
    }
}