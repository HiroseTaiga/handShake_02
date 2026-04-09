using System.Collections.Generic;
using UnityEngine;
using Mediapipe.Unity.Sample.HandLandmarkDetection;
using Mediapipe.Tasks.Vision.HandLandmarker;

public class handTrackingProvider : MonoBehaviour
{
    [SerializeField] private HandLandmarkerRunner runner;
    [SerializeField] private handInputDispatcher dispatcher;
    [SerializeField] private directionConfig directionConfig;

    private handResultAdapter adapter;
    private fingerDirectionAnalyzer analyzer;
    private fingerDirectionStateStore stateStore;

    private void Awake()
    {
        adapter = new handResultAdapter();
        analyzer = new fingerDirectionAnalyzer(directionConfig);
        stateStore = new fingerDirectionStateStore();
    }

    private void OnEnable()
    {
        if (runner != null)
        {
            runner.onHandResult += onHandResult;
        }
    }

    private void OnDisable()
    {
        if (runner != null)
        {
            runner.onHandResult -= onHandResult;
        }
    }

    private void onHandResult(HandLandmarkerResult result)
    {
        if (dispatcher == null) return;
        if (result.handLandmarks == null) return;

        var hands = adapter.convert(result);

        HashSet<handType> activeHands = new HashSet<handType>();

        foreach (var hand in hands)
        {
            activeHands.Add(hand.handType);

            foreach (var kv in hand.fingerStates)
            {
                fingerType finger = kv.Key;
                fingerState state = kv.Value;

                var dir = analyzer.analyze(state.tipPosition, state.basePosition, hand.handType);

                state.setDirection(dir);

                if (stateStore.tryUpdate(hand.handType, finger, dir))
                {
                    dispatcher.dispatchFinger(hand.handType, finger, dir);
                }
            }

            dispatcher.dispatchHand(hand);
        }

        stateStore.removeUnusedHands(activeHands);
    }
}